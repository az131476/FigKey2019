using System;
using System.Collections.Generic;
using System.IO;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class IdManager
	{
		private string mPath = "";

		private string mIniFile = "ml_rt2.ini";

		private Dictionary<uint, string> mMarkerNames;

		private static Dictionary<uint, int> mMarkerLabelOccurenceCounter;

		private Dictionary<uint, string> mTriggerNames;

		private static Dictionary<uint, int> mTriggerLabelOccurenceCounter;

		private int mMarkerIdCounterBits;

		public IdManager()
		{
			this.mMarkerNames = new Dictionary<uint, string>();
			IdManager.mMarkerLabelOccurenceCounter = new Dictionary<uint, int>();
			this.mTriggerNames = new Dictionary<uint, string>();
			IdManager.mTriggerLabelOccurenceCounter = new Dictionary<uint, int>();
			this.mMarkerIdCounterBits = 0;
		}

		public void Load(string path)
		{
			this.mMarkerNames.Clear();
			IdManager.mMarkerLabelOccurenceCounter.Clear();
			this.mTriggerNames.Clear();
			IdManager.mTriggerLabelOccurenceCounter.Clear();
			this.mMarkerIdCounterBits = 0;
			this.mPath = path;
			this.ReadINIFile();
		}

		public bool GetMarkerNameByID(uint id, out string name)
		{
			name = "";
			if (this.mMarkerNames.ContainsKey(id))
			{
				name = this.mMarkerNames[id];
				return true;
			}
			return false;
		}

		public bool GetTriggerNameByID(uint id, out string name)
		{
			name = "";
			if (this.mTriggerNames.ContainsKey(id))
			{
				name = this.mTriggerNames[id];
				return true;
			}
			return false;
		}

		public int GetMarkerIdCounterBits()
		{
			return this.mMarkerIdCounterBits;
		}

		private void ReadINIFile()
		{
			Stream iniFileAsStream = FileAccessManager.GetInstance().GetIniFileAsStream(this.mIniFile);
			if (iniFileAsStream == null)
			{
				return;
			}
			StreamReader streamReader = new StreamReader(iniFileAsStream);
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (text.StartsWith("Marker"))
				{
					int arg_40_0 = "Marker".Length;
					string[] array = text.Split(new char[]
					{
						'='
					}, 2);
					if (array.Length >= 2 && array[0].Length >= 6)
					{
						string s = array[0].Substring(6, array[0].Length - 6);
						string value = array[1];
						uint key;
						if (uint.TryParse(s, out key) && !this.mMarkerNames.ContainsKey(key))
						{
							this.mMarkerNames.Add(key, value);
						}
					}
				}
				else if (text.StartsWith("Trigger"))
				{
					string[] array2 = text.Split(new char[]
					{
						'='
					}, 2);
					if (array2.Length >= 2 && array2[0].Length >= 7)
					{
						string s2 = array2[0].Substring(7, array2[0].Length - 7);
						string value2 = array2[1];
						uint key2;
						if (uint.TryParse(s2, out key2))
						{
							this.mTriggerNames.Add(key2, value2);
						}
					}
				}
				else if (text.StartsWith("TM_CounterBits"))
				{
					string[] array3 = text.Split(new char[]
					{
						'='
					}, 2);
					int num;
					if (array3.Length >= 2 && int.TryParse(array3[1], out num))
					{
						this.mMarkerIdCounterBits = num;
					}
				}
			}
			streamReader.Close();
		}

		public int GetMarkerLabelOccurenceCount(uint id)
		{
			if (!IdManager.mMarkerLabelOccurenceCounter.ContainsKey(id))
			{
				IdManager.mMarkerLabelOccurenceCounter.Add(id, 1);
			}
			Dictionary<uint, int> dictionary;
			int result;
			(dictionary = IdManager.mMarkerLabelOccurenceCounter)[id] = (result = dictionary[id]) + 1;
			return result;
		}

		public int GetTriggerLabelOccurenceCount(uint id)
		{
			if (!IdManager.mTriggerLabelOccurenceCounter.ContainsKey(id))
			{
				IdManager.mTriggerLabelOccurenceCounter.Add(id, 1);
			}
			Dictionary<uint, int> dictionary;
			int result;
			(dictionary = IdManager.mTriggerLabelOccurenceCounter)[id] = (result = dictionary[id]) + 1;
			return result;
		}
	}
}
