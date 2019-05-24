using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public static class UsedLimitFilterIdsCount
	{
		private static Dictionary<int, ulong> _numberOfLimitFilterIdsOnMemory;

		public static readonly ulong MaxNumberOfIdsInLimitFilters;

		static UsedLimitFilterIdsCount()
		{
			UsedLimitFilterIdsCount.MaxNumberOfIdsInLimitFilters = 1000uL;
			UsedLimitFilterIdsCount._numberOfLimitFilterIdsOnMemory = new Dictionary<int, ulong>();
		}

		public static void Reset()
		{
			UsedLimitFilterIdsCount._numberOfLimitFilterIdsOnMemory.Clear();
		}

		public static void Update(FilterConfiguration filterConfig, IApplicationDatabaseManager databaseManager, string configFolderPath, out bool isMaxIdsExceeded)
		{
			isMaxIdsExceeded = false;
			ulong num = 0uL;
			foreach (Filter current in filterConfig.Filters)
			{
				if (current.IsActive.Value && current.Action.Value == FilterActionType.Limit)
				{
					if (current is SymbolicMessageFilter)
					{
						SymbolicMessageFilter symbolicMessageFilter = current as SymbolicMessageFilter;
						if (symbolicMessageFilter.BusType.Value != BusType.Bt_FlexRay)
						{
							num += 1uL;
						}
						else
						{
							IList<MessageDefinition> list = null;
							IList<string> list2 = null;
							bool flag;
							if (databaseManager.GetFlexrayFrameOrPDUInfo(FileSystemServices.GetAbsolutePath(symbolicMessageFilter.DatabasePath.Value, configFolderPath), symbolicMessageFilter.NetworkName.Value, symbolicMessageFilter.MessageName.Value, symbolicMessageFilter.IsFlexrayPDU.Value, out list, out list2, out flag))
							{
								Dictionary<uint, HashSet<int>> dictionary = new Dictionary<uint, HashSet<int>>();
								foreach (MessageDefinition current2 in list)
								{
									if (!dictionary.ContainsKey(current2.CanDbMessageId))
									{
										dictionary.Add(current2.CanDbMessageId, new HashSet<int>());
									}
									int num2 = current2.FrBaseCycle;
									while ((long)num2 <= (long)((ulong)Constants.MaximumFlexrayBaseCycle))
									{
										dictionary[current2.CanDbMessageId].Add(num2);
										num2 += current2.FrCycleRepetition;
									}
								}
								int num3 = 0;
								foreach (uint current3 in dictionary.Keys)
								{
									num3 += dictionary[current3].Count;
								}
								num += (ulong)((long)num3);
							}
						}
					}
					else if (current is CANIdFilter)
					{
						CANIdFilter cANIdFilter = current as CANIdFilter;
						if (cANIdFilter.IsIdRange.Value)
						{
							num += (ulong)(cANIdFilter.CANIdLast.Value - cANIdFilter.CANId.Value + 1u);
						}
						else
						{
							num += 1uL;
						}
					}
					else if (current is LINIdFilter)
					{
						LINIdFilter lINIdFilter = current as LINIdFilter;
						if (lINIdFilter.IsIdRange.Value)
						{
							num += (ulong)(lINIdFilter.LINIdLast.Value - lINIdFilter.LINId.Value + 1u);
						}
						else
						{
							num += 1uL;
						}
					}
					else if (current is FlexrayIdFilter)
					{
						FlexrayIdFilter flexrayIdFilter = current as FlexrayIdFilter;
						if (flexrayIdFilter.IsIdRange.Value)
						{
							num += (ulong)((flexrayIdFilter.FlexrayIdLast.Value - flexrayIdFilter.FlexrayId.Value + 1u) * (Constants.MaximumFlexrayBaseCycle + 1u));
						}
						else
						{
							num += (ulong)((Constants.MaximumFlexrayBaseCycle + 1u) / flexrayIdFilter.CycleRepetition.Value);
						}
					}
				}
			}
			UsedLimitFilterIdsCount._numberOfLimitFilterIdsOnMemory[filterConfig.MemoryNr] = num;
			ulong num4 = 0uL;
			using (Dictionary<int, ulong>.ValueCollection.Enumerator enumerator4 = UsedLimitFilterIdsCount._numberOfLimitFilterIdsOnMemory.Values.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					uint num5 = (uint)enumerator4.Current;
					num4 += (ulong)num5;
				}
			}
			isMaxIdsExceeded = (num4 > UsedLimitFilterIdsCount.MaxNumberOfIdsInLimitFilters);
		}
	}
}
