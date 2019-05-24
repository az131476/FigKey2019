using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public static class RingBufferMemoriesManager
	{
		private static Dictionary<int, uint> _memoryNrToRingBufferSizeKB;

		private static long _currentCardSizeKB;

		private static long _maxSumRingBufferSizesKB;

		private static long _minRingBufferSizeKB;

		public static long MaxSumRingBufferSizesKB
		{
			get
			{
				return RingBufferMemoriesManager._maxSumRingBufferSizesKB;
			}
			set
			{
				RingBufferMemoriesManager._maxSumRingBufferSizesKB = value;
			}
		}

		public static long MinRingBufferSizeKB
		{
			get
			{
				return RingBufferMemoriesManager._minRingBufferSizeKB;
			}
			set
			{
				RingBufferMemoriesManager._minRingBufferSizeKB = value;
			}
		}

		public static long CurrentCardSizeMB
		{
			get
			{
				return RingBufferMemoriesManager._currentCardSizeKB / 1024L;
			}
			set
			{
				RingBufferMemoriesManager._currentCardSizeKB = value * 1024L;
			}
		}

		static RingBufferMemoriesManager()
		{
			RingBufferMemoriesManager._memoryNrToRingBufferSizeKB = new Dictionary<int, uint>();
			RingBufferMemoriesManager._maxSumRingBufferSizesKB = 0L;
			RingBufferMemoriesManager._minRingBufferSizeKB = 0L;
			RingBufferMemoriesManager._currentCardSizeKB = 0L;
		}

		public static void Reset()
		{
			RingBufferMemoriesManager._memoryNrToRingBufferSizeKB.Clear();
			RingBufferMemoriesManager._maxSumRingBufferSizesKB = 0L;
			RingBufferMemoriesManager._minRingBufferSizeKB = 0L;
		}

		public static void Update(LoggingConfiguration loggingConfig, ILoggerSpecifics loggerSpecifics)
		{
			RingBufferMemoriesManager.Reset();
			foreach (TriggerConfiguration current in loggingConfig.TriggerConfigurations)
			{
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					RingBufferMemoriesManager._memoryNrToRingBufferSizeKB[current.MemoryNr] = current.MemoryRingBuffer.Size.Value;
				}
				else
				{
					RingBufferMemoriesManager._memoryNrToRingBufferSizeKB[current.MemoryNr] = 0u;
				}
			}
			RingBufferMemoriesManager._maxSumRingBufferSizesKB = (long)((ulong)loggerSpecifics.DataStorage.MaxRingBufferSize);
			RingBufferMemoriesManager._minRingBufferSizeKB = (long)((ulong)loggerSpecifics.DataStorage.MinRingBufferSize);
			if (RingBufferMemoriesManager._currentCardSizeKB > (long)((ulong)loggerSpecifics.DataStorage.MaxMemoryCardSize))
			{
				RingBufferMemoriesManager._currentCardSizeKB = (long)((ulong)loggerSpecifics.DataStorage.MaxMemoryCardSize);
			}
		}

		public static void SetMemoryRingBufferSize(int memoryNr, uint bufferSizeKB)
		{
			RingBufferMemoriesManager._memoryNrToRingBufferSizeKB[memoryNr] = bufferSizeKB;
		}

		public static long GetMaxAvailableRingBufferSize(int myMemNr)
		{
			if (RingBufferMemoriesManager._maxSumRingBufferSizesKB == 0L)
			{
				return 0L;
			}
			long num = RingBufferMemoriesManager._maxSumRingBufferSizesKB;
			foreach (int current in RingBufferMemoriesManager._memoryNrToRingBufferSizeKB.Keys)
			{
				if (current != myMemNr)
				{
					num -= (long)((ulong)RingBufferMemoriesManager._memoryNrToRingBufferSizeKB[current]);
				}
			}
			return num;
		}

		public static long GetNumOfFilesForCurrentCardSize(int myMemNr)
		{
			if (RingBufferMemoriesManager._currentCardSizeKB == 0L)
			{
				return 0L;
			}
			if (RingBufferMemoriesManager._memoryNrToRingBufferSizeKB[myMemNr] == 0u)
			{
				return 0L;
			}
			long num = (long)((ulong)(GUIUtil.GetActualMaxRingBufferSizeSDCard((uint)RingBufferMemoriesManager._currentCardSizeKB / 1024u) * 1024u / RingBufferMemoriesManager._memoryNrToRingBufferSizeKB[myMemNr]));
			if (num == 0L)
			{
				num = 1L;
			}
			return num;
		}

		public static bool AreAllMemoriesInactive()
		{
			bool flag = false;
			foreach (int current in RingBufferMemoriesManager._memoryNrToRingBufferSizeKB.Keys)
			{
				if (RingBufferMemoriesManager._memoryNrToRingBufferSizeKB[current] > 0u)
				{
					flag = true;
					break;
				}
			}
			return !flag;
		}
	}
}
