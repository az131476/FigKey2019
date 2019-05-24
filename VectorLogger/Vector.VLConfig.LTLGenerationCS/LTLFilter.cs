using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.Properties;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLFilter : LTLGenericCodePart
	{
		private ReadOnlyCollection<FilterConfiguration> filterConfigs;

		private DatabaseConfiguration databaseConfig;

		private ILoggerSpecifics loggerSpecifics;

		private IApplicationDatabaseManager databaseManager;

		private string configurationFolderPath;

		public string LastErrorText
		{
			get;
			set;
		}

		public LTLFilter(ReadOnlyCollection<FilterConfiguration> filterConfigList, DatabaseConfiguration databaseConfig, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath)
		{
			this.filterConfigs = filterConfigList;
			this.databaseConfig = databaseConfig;
			this.loggerSpecifics = loggerSpecifics;
			this.databaseManager = databaseManager;
			this.configurationFolderPath = configurationFolderPath;
			this.LastErrorText = string.Empty;
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLFilterCode()
		{
			this.LastErrorText = string.Empty;
			base.LtlCode = new StringBuilder();
			foreach (FilterConfiguration current in this.filterConfigs)
			{
				base.LtlCode.AppendLine();
				if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
				{
					base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment(string.Format("FILTERS ON MEMORY {0}", current.MemoryNr)));
				}
				else
				{
					base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("FILTERS"));
				}
				base.LtlCode.AppendLine();
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (current.DefaultFilter != null && current.DefaultFilter.Action.Value == FilterActionType.Stop)
				{
					stringBuilder.AppendLine();
					int num = 1;
					while ((long)num <= (long)((ulong)this.loggerSpecifics.CAN.NumberOfChannels))
					{
						stringBuilder.AppendFormat("  EXCLUDE CAN{0:D}  DATA 0x0 - 0x{1,-8:X}    EXCLUDE CAN{0:D}  RTR 0x0 - 0x{1,-8:X}", num, LTLUtil.GetHighestID(BusType.Bt_CAN, false));
						stringBuilder.AppendLine();
						stringBuilder.AppendFormat("  EXCLUDE CAN{0:D} XDATA 0x0 - 0x{1,-8:X}    EXCLUDE CAN{0:D} XRTR 0x0 - 0x{1,-8:X}", num, LTLUtil.GetHighestID(BusType.Bt_CAN, true));
						stringBuilder.AppendLine();
						num++;
					}
					int num2 = 1;
					while ((long)num2 <= (long)((ulong)this.loggerSpecifics.LIN.NumberOfChannels))
					{
						stringBuilder.AppendFormat("  EXCLUDE LIN{0:D}  DATA 0x0 - 0x{1:X}", num2, 2047);
						stringBuilder.AppendLine();
						num2++;
					}
					for (uint num3 = 1u; num3 <= this.loggerSpecifics.Flexray.NumberOfChannels; num3 += 1u)
					{
						stringBuilder.AppendFormat("  EXCLUDE  FR{0:D}  DATA 0   - {1:D}", num3, LTLUtil.GetHighestID(BusType.Bt_FlexRay, false));
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine();
					flag = true;
				}
				for (int i = 0; i < current.Filters.Count<Filter>(); i++)
				{
					Filter filter = current.Filters[i];
					if (filter.IsActive.Value && !(filter is DefaultFilter))
					{
						if (filter is ChannelFilter)
						{
							ChannelFilter channelFilter = filter as ChannelFilter;
							uint lastId;
							if (channelFilter.BusType.Value == BusType.Bt_LIN)
							{
								lastId = 2047u;
							}
							else
							{
								lastId = LTLUtil.GetHighestID(channelFilter.BusType.Value, false);
							}
							if (channelFilter.Action.Value == FilterActionType.Limit)
							{
								LTLGenerator.LTLGenerationResult result = LTLGenerator.LTLGenerationResult.FilterError;
								return result;
							}
							string arg;
							if (channelFilter.Action.Value == FilterActionType.Stop)
							{
								arg = "EXCLUDE";
							}
							else
							{
								arg = "INCLUDE";
							}
							string text = string.Format("  {0} {1}", arg, LTLUtil.GetIdString(channelFilter.BusType.Value, channelFilter.ChannelNumber.Value, false, 0u, 0u, 1u, true, lastId, true));
							stringBuilder.AppendFormat("{0,-40}", text);
							if (channelFilter.BusType.Value == BusType.Bt_CAN)
							{
								string text2 = string.Format("  {0} {1}", arg, LTLUtil.GetIdString(channelFilter.BusType.Value, channelFilter.ChannelNumber.Value, true, 0u, 0u, 1u, true, LTLUtil.GetHighestID(BusType.Bt_CAN, true), true));
								string arg2 = text.Replace("DATA", "RTR");
								string arg3 = text2.Replace("DATA", "RTR");
								stringBuilder.AppendFormat("{0,-40}", arg2);
								stringBuilder.AppendLine();
								stringBuilder.AppendFormat("{0,-40}{1,-40}", text2, arg3);
							}
							stringBuilder.AppendLine();
						}
						else if (filter is CANIdFilter)
						{
							CANIdFilter cANIdFilter = filter as CANIdFilter;
							stringBuilder.Append(LTLUtil.BuildFilterLine(this.loggerSpecifics, (uint)current.MemoryNr, cANIdFilter.Action.Value, cANIdFilter.LimitIntervalPerFrame.Value, BusType.Bt_CAN, cANIdFilter.ChannelNumber.Value, cANIdFilter.IsExtendedId.Value, cANIdFilter.CANId.Value, cANIdFilter.IsIdRange.Value, cANIdFilter.CANIdLast.Value));
						}
						else if (filter is LINIdFilter)
						{
							LINIdFilter lINIdFilter = filter as LINIdFilter;
							stringBuilder.Append(LTLUtil.BuildFilterLine(this.loggerSpecifics, (uint)current.MemoryNr, lINIdFilter.Action.Value, lINIdFilter.LimitIntervalPerFrame.Value, BusType.Bt_LIN, lINIdFilter.ChannelNumber.Value, false, lINIdFilter.LINId.Value, lINIdFilter.IsIdRange.Value, lINIdFilter.LINIdLast.Value));
						}
						else if (filter is FlexrayIdFilter)
						{
							FlexrayIdFilter flexrayIdFilter = filter as FlexrayIdFilter;
							stringBuilder.Append(LTLUtil.BuildFilterLine(this.loggerSpecifics, (uint)current.MemoryNr, flexrayIdFilter.Action.Value, flexrayIdFilter.LimitIntervalPerFrame.Value, BusType.Bt_FlexRay, flexrayIdFilter.ChannelNumber.Value, false, flexrayIdFilter.FlexrayId.Value, flexrayIdFilter.BaseCycle.Value, flexrayIdFilter.CycleRepetition.Value, flexrayIdFilter.IsIdRange.Value, flexrayIdFilter.FlexrayIdLast.Value));
						}
						else
						{
							if (!(filter is SymbolicMessageFilter))
							{
								LTLGenerator.LTLGenerationResult result;
								if (filter is SignalListFileFilter)
								{
									SignalListFileFilter signalListFileFilter = filter as SignalListFileFilter;
									List<string> list;
									if (!FileSystemServices.ReadSymbolsInColumnFromCSVFile(FileSystemServices.GetAbsolutePath(signalListFileFilter.FilePath.Value, this.configurationFolderPath), signalListFileFilter.Column.Value, out list))
									{
										this.LastErrorText = string.Format(Resources.LTLError_FilterError_SigListReadFailed, signalListFileFilter.FilePath.Value);
										result = LTLGenerator.LTLGenerationResult.FilterError_SigListReadFailed;
										return result;
									}
									Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
									Dictionary<string, MessageDefinition> dictionary2 = new Dictionary<string, MessageDefinition>();
									stringBuilder.AppendFormat("{{ Message filters generated from signal list in file '{0}' in column {1}  }}", signalListFileFilter.FilePath.Value, signalListFileFilter.Column.Value + 1u);
									stringBuilder.AppendLine();
									if (list.Count == 0)
									{
										stringBuilder.AppendLine("{ - empty signal list - }");
										goto IL_AB8;
									}
									foreach (string current2 in list)
									{
										IDictionary<string, MessageDefinition> dictionary3;
										if (this.databaseManager.FindMessagesForSignalName(this.databaseConfig, this.configurationFolderPath, current2, signalListFileFilter.BusType.Value, signalListFileFilter.ChannelNumber.Value, out dictionary3))
										{
											using (IEnumerator<KeyValuePair<string, MessageDefinition>> enumerator3 = dictionary3.GetEnumerator())
											{
												while (enumerator3.MoveNext())
												{
													KeyValuePair<string, MessageDefinition> current3 = enumerator3.Current;
													string key = current3.Key;
													if (!dictionary2.ContainsKey(key))
													{
														dictionary2.Add(current3.Key, current3.Value);
													}
													if (dictionary.ContainsKey(key))
													{
														if (!dictionary[key].Contains(current2))
														{
															dictionary[key].Add(current2);
														}
													}
													else
													{
														dictionary.Add(key, new List<string>());
														dictionary[key].Add(current2);
													}
												}
												continue;
											}
										}
										this.LastErrorText = string.Format(Resources.LTLError_FilterError_SigListResolve, current2, signalListFileFilter.FilePath.Value);
										result = LTLGenerator.LTLGenerationResult.FilterError_SigListResolve;
										return result;
									}
									using (Dictionary<string, MessageDefinition>.KeyCollection.Enumerator enumerator4 = dictionary2.Keys.GetEnumerator())
									{
										while (enumerator4.MoveNext())
										{
											string current4 = enumerator4.Current;
											stringBuilder.Append(LTLUtil.BuildFilterLine(this.loggerSpecifics, (uint)current.MemoryNr, signalListFileFilter.Action.Value, signalListFileFilter.LimitIntervalPerFrame.Value, signalListFileFilter.BusType.Value, signalListFileFilter.ChannelNumber.Value, dictionary2[current4].IsExtendedId, dictionary2[current4].ActualMessageId));
											stringBuilder.AppendFormat("{0,-14:}", " ");
											List<string> list2 = dictionary[current4];
											stringBuilder.AppendFormat("  {{message '{0}' contains signals '{1}'", current4, list2[0]);
											for (int j = 1; j < list2.Count<string>(); j++)
											{
												stringBuilder.AppendFormat(", '{0}'", list2[j]);
											}
											stringBuilder.AppendLine(" }");
										}
										goto IL_AAF;
									}
								}
								stringBuilder.AppendLine("{unknown filter type}");
								result = LTLGenerator.LTLGenerationResult.FilterError;
								return result;
							}
							SymbolicMessageFilter symbolicMessageFilter = filter as SymbolicMessageFilter;
							MessageDefinition messageDefinition;
							if (!this.databaseManager.ResolveMessageSymbolInDatabase(FileSystemServices.GetAbsolutePath(symbolicMessageFilter.DatabasePath.Value, this.configurationFolderPath), symbolicMessageFilter.NetworkName.Value, symbolicMessageFilter.MessageName.Value, symbolicMessageFilter.BusType.Value, out messageDefinition))
							{
								LTLGenerator.LTLGenerationResult result = LTLGenerator.LTLGenerationResult.FilterError_MessageResolve;
								return result;
							}
							if (symbolicMessageFilter.BusType.Value == BusType.Bt_FlexRay)
							{
								IList<MessageDefinition> list3 = null;
								IList<string> list4 = null;
								bool flag2;
								this.databaseManager.GetFlexrayFrameOrPDUInfo(FileSystemServices.GetAbsolutePath(symbolicMessageFilter.DatabasePath.Value, this.configurationFolderPath), symbolicMessageFilter.NetworkName.Value, symbolicMessageFilter.MessageName.Value, symbolicMessageFilter.IsFlexrayPDU.Value, out list3, out list4, out flag2);
								int num4 = 0;
								using (IEnumerator<MessageDefinition> enumerator5 = list3.GetEnumerator())
								{
									while (enumerator5.MoveNext())
									{
										MessageDefinition current5 = enumerator5.Current;
										stringBuilder.AppendFormat("{0,-46}", LTLUtil.BuildFilterLine(this.loggerSpecifics, (uint)current.MemoryNr, symbolicMessageFilter.Action.Value, symbolicMessageFilter.LimitIntervalPerFrame.Value, symbolicMessageFilter.BusType.Value, symbolicMessageFilter.ChannelNumber.Value, false, current5.ActualMessageId, (uint)current5.FrBaseCycle, (uint)current5.FrCycleRepetition));
										if (symbolicMessageFilter.IsFlexrayPDU.Value)
										{
											stringBuilder.AppendFormat("  {{symbolic FlexRay frame {0}::{1} containing PDU {2}}}", symbolicMessageFilter.DatabaseName, current5.Name, symbolicMessageFilter.MessageName);
										}
										else
										{
											stringBuilder.AppendFormat("  {{symbolic FlexRay frame {0}::{1}}}", symbolicMessageFilter.DatabaseName, symbolicMessageFilter.MessageName);
										}
										num4++;
										if (num4 != list3.Count)
										{
											stringBuilder.AppendLine();
										}
									}
									goto IL_AAF;
								}
							}
							stringBuilder.AppendFormat("{0,-46}", LTLUtil.BuildFilterLine(this.loggerSpecifics, (uint)current.MemoryNr, symbolicMessageFilter.Action.Value, symbolicMessageFilter.LimitIntervalPerFrame.Value, symbolicMessageFilter.BusType.Value, symbolicMessageFilter.ChannelNumber.Value, messageDefinition.IsExtendedId, messageDefinition.ActualMessageId, (uint)messageDefinition.FrBaseCycle, (uint)messageDefinition.FrCycleRepetition));
							stringBuilder.AppendFormat("  {{symbolic {0} message {1}::{2}}}", LTLUtil.GetBusTypeString(symbolicMessageFilter.BusType.Value), symbolicMessageFilter.DatabaseName, symbolicMessageFilter.MessageName);
						}
						IL_AAF:
						stringBuilder.AppendLine();
						flag = true;
					}
					IL_AB8:;
				}
				if (flag)
				{
					base.LtlCode.AppendFormat("RECORDFILTER {0}", (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u) ? current.MemoryNr.ToString() : "");
					base.LtlCode.AppendLine();
					base.LtlCode.Append(stringBuilder);
					base.LtlCode.AppendLine();
				}
				else
				{
					stringBuilder.AppendLine("  {no filters configured}");
				}
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
