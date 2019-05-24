using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public static class UsedIdsManager
	{
		private static Dictionary<IFeatureVirtualLogMessages, Dictionary<uint, List<CANIdInterval>>> _usedCanIdsOnChannelsForFeature;

		static UsedIdsManager()
		{
			UsedIdsManager._usedCanIdsOnChannelsForFeature = new Dictionary<IFeatureVirtualLogMessages, Dictionary<uint, List<CANIdInterval>>>();
		}

		public static void Reset()
		{
			UsedIdsManager._usedCanIdsOnChannelsForFeature.Clear();
		}

		public static bool AreFiltersRemovingLogOnlyMsgs(ReadOnlyCollection<FilterConfiguration> filterConfigs, ReadOnlyCollection<TriggerConfiguration> triggerConfigs, IApplicationDatabaseManager dbManager, string configFolderPath, ILoggerSpecifics loggerSpecifics, out IList<Feature> affectedFeatures)
		{
			affectedFeatures = new List<Feature>();
			HashSet<uint> hashSet = new HashSet<uint>();
			foreach (TriggerConfiguration current in triggerConfigs)
			{
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					switch (current.TriggerMode.Value)
					{
					case TriggerMode.Triggered:
						break;
					case TriggerMode.Permanent:
						hashSet.Add((uint)current.MemoryNr);
						continue;
					case TriggerMode.OnOff:
						using (IEnumerator<RecordTrigger> enumerator2 = current.OnOffTriggers.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								RecordTrigger current2 = enumerator2.Current;
								if (!hashSet.Contains((uint)current.MemoryNr) && current2.IsActive.Value && current2.TriggerEffect.Value == TriggerEffect.LoggingOn)
								{
									hashSet.Add((uint)current.MemoryNr);
									break;
								}
							}
							continue;
						}
						break;
					default:
						goto IL_155;
					}
					using (IEnumerator<RecordTrigger> enumerator3 = current.Triggers.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							RecordTrigger current3 = enumerator3.Current;
							if (!hashSet.Contains((uint)current.MemoryNr) && current3.IsActive.Value)
							{
								hashSet.Add((uint)current.MemoryNr);
								break;
							}
						}
						continue;
					}
					IL_155:
					bool result = false;
					return result;
				}
			}
			if (hashSet.Count == 0)
			{
				return false;
			}
			foreach (IFeatureVirtualLogMessages current4 in UsedIdsManager._usedCanIdsOnChannelsForFeature.Keys)
			{
				Dictionary<uint, List<CANIdInterval>> dictionary = UsedIdsManager._usedCanIdsOnChannelsForFeature[current4];
				foreach (uint current5 in dictionary.Keys)
				{
					if (current5 <= loggerSpecifics.CAN.NumberOfChannels)
					{
						foreach (CANIdInterval current6 in dictionary[current5])
						{
							int num = 0;
							foreach (FilterConfiguration current7 in filterConfigs)
							{
								if (hashSet.Contains((uint)current7.MemoryNr))
								{
									bool flag = false;
									int i = current7.Filters.Count - 1;
									while (i >= 0)
									{
										if (UsedIdsManager.IsFilterAffectingCANMsg(current7.Filters[i], current5, current6, dbManager, configFolderPath, out flag))
										{
											if (flag)
											{
												num++;
												break;
											}
											break;
										}
										else
										{
											i--;
										}
									}
								}
							}
							if (num >= hashSet.Count)
							{
								if (current4 is Feature)
								{
									affectedFeatures.Add(current4 as Feature);
									break;
								}
								break;
							}
						}
						if (current4 is Feature && affectedFeatures.Contains(current4 as Feature))
						{
							break;
						}
					}
				}
			}
			return affectedFeatures.Count > 0;
		}

		private static bool IsFilterAffectingCANMsg(Filter filter, uint canChannelNr, CANIdInterval canIdInterval, IApplicationDatabaseManager dbManager, string configFolderPath, out bool isStopping)
		{
			isStopping = false;
			if (!filter.IsActive.Value)
			{
				return false;
			}
			if (filter is DefaultFilter)
			{
				isStopping = (filter.Action.Value == FilterActionType.Stop);
				return true;
			}
			if (filter is ChannelFilter)
			{
				ChannelFilter channelFilter = filter as ChannelFilter;
				if (channelFilter.BusType.Value == BusType.Bt_CAN && channelFilter.ChannelNumber.Value == canChannelNr)
				{
					isStopping = (channelFilter.Action.Value == FilterActionType.Stop);
					return true;
				}
				return false;
			}
			else if (filter is SymbolicMessageFilter)
			{
				SymbolicMessageFilter symbolicMessageFilter = filter as SymbolicMessageFilter;
				MessageDefinition messageDefinition;
				if (symbolicMessageFilter.BusType.Value == BusType.Bt_CAN && symbolicMessageFilter.ChannelNumber.Value == canChannelNr && dbManager.ResolveMessageSymbolInDatabase(FileSystemServices.GetAbsolutePath(symbolicMessageFilter.DatabasePath.Value, configFolderPath), symbolicMessageFilter.NetworkName.Value, symbolicMessageFilter.MessageName.Value, BusType.Bt_CAN, out messageDefinition) && canIdInterval.Contains(new CANIdInterval(messageDefinition.ActualMessageId, messageDefinition.IsExtendedId)))
				{
					isStopping = (symbolicMessageFilter.Action.Value == FilterActionType.Stop);
					return true;
				}
				return false;
			}
			else
			{
				if (filter is CANIdFilter)
				{
					CANIdFilter cANIdFilter = filter as CANIdFilter;
					if (cANIdFilter.ChannelNumber.Value == canChannelNr)
					{
						CANIdInterval cANIdInterval;
						if (cANIdFilter.IsIdRange.Value)
						{
							cANIdInterval = new CANIdInterval(cANIdFilter.CANId.Value, cANIdFilter.CANIdLast.Value, cANIdFilter.IsExtendedId.Value);
						}
						else
						{
							cANIdInterval = new CANIdInterval(cANIdFilter.CANId.Value, cANIdFilter.IsExtendedId.Value);
						}
						if (cANIdInterval.Contains(canIdInterval))
						{
							isStopping = (cANIdFilter.Action.Value == FilterActionType.Stop);
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		public static void UpdateVirtualLogMsgIds(IFeatureVirtualLogMessages feature)
		{
			if (UsedIdsManager._usedCanIdsOnChannelsForFeature.ContainsKey(feature))
			{
				UsedIdsManager._usedCanIdsOnChannelsForFeature.Remove(feature);
			}
			Dictionary<uint, List<CANIdInterval>> dictionary = new Dictionary<uint, List<CANIdInterval>>();
			UsedIdsManager._usedCanIdsOnChannelsForFeature.Add(feature, dictionary);
			foreach (IVirtualCANLogMessage current in feature.ActiveVirtualCANLogMessages)
			{
				if (!dictionary.ContainsKey(current.ChannelNr.Value))
				{
					dictionary.Add(current.ChannelNr.Value, new List<CANIdInterval>());
				}
				List<CANIdInterval> list = dictionary[current.ChannelNr.Value];
				UsedIdsManager.MergeCanIdInList(ref list, current.Id.Value, current.IsIdExtended.Value);
			}
		}

		public static bool AreVirtualLogMsgIdsUpdated(IFeatureVirtualLogMessages feature)
		{
			return UsedIdsManager._usedCanIdsOnChannelsForFeature.ContainsKey(feature);
		}

		public static bool ValidateVirtualLogMsgs(IFeatureVirtualLogMessages featureToValidate, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			foreach (IVirtualCANLogMessage current in featureToValidate.ActiveVirtualCANLogMessages)
			{
				foreach (IFeatureVirtualLogMessages current2 in UsedIdsManager._usedCanIdsOnChannelsForFeature.Keys)
				{
					if (current2 != featureToValidate)
					{
						Dictionary<uint, List<CANIdInterval>> dictionary = UsedIdsManager._usedCanIdsOnChannelsForFeature[current2];
						if (dictionary.ContainsKey(current.ChannelNr.Value))
						{
							List<CANIdInterval> list = dictionary[current.ChannelNr.Value];
							foreach (CANIdInterval current3 in list)
							{
								if (current3.Contains(new CANIdInterval(current.Id.Value, current.IsIdExtended.Value)))
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.Id, Resources.ErrorSameCanIdUsedByOtherPage);
									result = false;
									break;
								}
							}
						}
					}
				}
			}
			return result;
		}

		private static void MergeCanIdInList(ref List<CANIdInterval> canIdList, uint canDbEncodedCanId)
		{
			bool flag = (canDbEncodedCanId & Constants.CANDbIsExtendedIdMask) == Constants.CANDbIsExtendedIdMask;
			uint singleCanId = canDbEncodedCanId;
			if (flag)
			{
				singleCanId = canDbEncodedCanId - Constants.CANDbIsExtendedIdMask;
			}
			UsedIdsManager.MergeCanIdInList(ref canIdList, singleCanId, flag);
		}

		private static void MergeCanIdInList(ref List<CANIdInterval> canIdList, uint singleCanId, bool isExtended)
		{
			foreach (CANIdInterval current in canIdList)
			{
				if (current.MergeWith(singleCanId, isExtended))
				{
					return;
				}
			}
			canIdList.Add(new CANIdInterval(singleCanId, isExtended));
		}

		private static void MergeCanIdInList(ref List<CANIdInterval> canIdList, uint canIdBegin, uint canIdEnd, bool isExtended)
		{
			CANIdInterval canIdToMerge = new CANIdInterval(canIdBegin, canIdEnd, isExtended);
			UsedIdsManager.MergeCanIdInList(ref canIdList, canIdToMerge);
		}

		private static void MergeCanIdInList(ref List<CANIdInterval> canIdList, CANIdInterval canIdToMerge)
		{
			List<CANIdInterval> list = new List<CANIdInterval>();
			foreach (CANIdInterval current in canIdList)
			{
				if (canIdToMerge.MergeWith(current))
				{
					list.Add(current);
				}
			}
			canIdList.Add(canIdToMerge);
			foreach (CANIdInterval current2 in list)
			{
				canIdList.Remove(current2);
			}
		}
	}
}
