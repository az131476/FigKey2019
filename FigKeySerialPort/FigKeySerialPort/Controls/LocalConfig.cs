using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils.FileHelper;
using FigKeySerialPort.Model;
using CommonUtils.Logger;

namespace FigKeySerialPort.Controls
{
    class LocalConfig
    {
        public const string SENT_PATH_HEAD = "SENT_PATH";
        public const string SENT_CONFIG_LAST_PATH = "sent_local_path";
        public const string SENT_CONFIG_INI_NAME = "sentcfg.ini";

        //head
        public const string DATA_TYPE = "DATA_TYPE";
        public const string BASE_SIG = "BASE_SIG";
        public const string SLOW_SIG = "SLOW_SIG";
        public const string QUICK_SIG = "QUICK_SIG";

        //key
        public const string STORAGE_DATA_TYPE = "storage_data_type";

        public const string BASE_DATA_TYPE = "data_type";
        public const string BASE_BATTERY_STATE = "battery_state";
        public const string BASE_SERIAL_MSG = "serial_msg";
        public const string BASE_TIME_LEN = "time_len";

        public const string SLOW_GROUP_COUNT = "group_count";
        public const string SLOW_GROUP_ORDER = "group_order";
        public const string SLOW_GROUP_SERIAL_ID = "group_serial_id";
        public const string SLOW_GROUP_DATA = "group_data";

        public const string QUICK_DATA_TYPE = "data_type";
        public const string QUICK_SIG_DATA1 = "sig_data1";
        public const string QUICK_SIG_DATA2 = "sig_data2";
        public const string QUICK_DATA2_CHECK = "isCheck";

        public enum Storage_Data_Type
        {
            HEX,
            DEC,

        }
        public static void ReadBaseConfig(string path, SentConfig.BaseSigConfig basecfg, List<SentConfig.SlowSigConfig> cfgList,
            SentConfig.QuickSigConfig quickCfg, SentConfig sentCfg)
        {
            try
            {
                sentCfg.StorageDataType = (Storage_Data_Type)Enum.Parse(typeof(Storage_Data_Type), INIFile.GetValue(DATA_TYPE, STORAGE_DATA_TYPE, path));

                basecfg.DataType = INIFile.GetValue(BASE_SIG, BASE_DATA_TYPE, path);
                basecfg.BatteryState = INIFile.GetValue(BASE_SIG, BASE_BATTERY_STATE, path);
                basecfg.SerialMsg = INIFile.GetValue(BASE_SIG, BASE_SERIAL_MSG, path);
                basecfg.TimeLong = INIFile.GetValue(BASE_SIG, BASE_TIME_LEN, path);

                int groupCount = 0;
                if (sentCfg.StorageDataType == Storage_Data_Type.HEX)
                {
                    groupCount = Convert.ToInt32(INIFile.GetValue(SLOW_SIG, SLOW_GROUP_COUNT, path).ToLower().Replace("0x", ""), 16);
                }
                else if (sentCfg.StorageDataType == Storage_Data_Type.DEC)
                {
                    groupCount = Convert.ToInt32(INIFile.GetValue(SLOW_SIG, SLOW_GROUP_COUNT, path));
                }

                cfgList.Clear();
                for (int i = 0; i < groupCount; i++)
                {
                    SentConfig.SlowSigConfig slowCfg = new SentConfig.SlowSigConfig();
                    slowCfg.GroupOrder = INIFile.GetValue(SLOW_SIG, SLOW_GROUP_ORDER + "_" + i, path);
                    slowCfg.GroupCount = INIFile.GetValue(SLOW_SIG, SLOW_GROUP_COUNT, path);
                    slowCfg.GroupSerialID = INIFile.GetValue(SLOW_SIG,SLOW_GROUP_SERIAL_ID+"_"+i,path);
                    slowCfg.GroupData = INIFile.GetValue(SLOW_SIG, SLOW_GROUP_DATA + "_" + i, path);
                    cfgList.Add(slowCfg);
                }

                quickCfg.QuickSigType = INIFile.GetValue(QUICK_SIG, QUICK_DATA_TYPE, path);
                quickCfg.QuickSigData1 = INIFile.GetValue(QUICK_SIG, QUICK_SIG_DATA1, path);
                quickCfg.QuickSigData2 = INIFile.GetValue(QUICK_SIG, QUICK_SIG_DATA2, path);
                quickCfg.QuickDataCheck = (SentConfig.QuickDataOrder)Enum.Parse(typeof(SentConfig.QuickDataOrder), INIFile.GetValue(QUICK_SIG, QUICK_DATA2_CHECK, path));

            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message+ex.StackTrace);
                return;
            }
        }

        public static void SaveUpdateConfig(string path,SentConfig.BaseSigConfig basecfg, List<SentConfig.SlowSigConfig> cfgList, 
            SentConfig.QuickSigConfig quickCfg,bool IsCfgChanging,SentConfig sentCfg)
        {
            try
            {
                //数据类型
                INIFile.SetValue(DATA_TYPE, STORAGE_DATA_TYPE, sentCfg.StorageDataType + "", path);
                //基础信号配置
                INIFile.SetValue(BASE_SIG, BASE_DATA_TYPE, basecfg.DataType, path);
                INIFile.SetValue(BASE_SIG, BASE_BATTERY_STATE, basecfg.BatteryState, path);
                INIFile.SetValue(BASE_SIG, BASE_SERIAL_MSG, basecfg.SerialMsg, path);
                INIFile.SetValue(BASE_SIG, BASE_TIME_LEN, basecfg.TimeLong, path);
                //慢信号配置
                INIFile.SetValue(SLOW_SIG, SLOW_GROUP_COUNT, cfgList[0].GroupCount.ToString(), path);
                for (int i = 0; i < cfgList.Count; i++)
                {
                    INIFile.SetValue(SLOW_SIG, SLOW_GROUP_ORDER + "_" + i, i + "", path);
                    INIFile.SetValue(SLOW_SIG,SLOW_GROUP_SERIAL_ID+"_"+i,cfgList[i].GroupSerialID,path);
                    INIFile.SetValue(SLOW_SIG, SLOW_GROUP_DATA + "_" + i, cfgList[i].GroupData, path);
                }
                //快信号配置
                INIFile.SetValue(QUICK_SIG, QUICK_DATA_TYPE, quickCfg.QuickSigType, path);
                INIFile.SetValue(QUICK_SIG, QUICK_SIG_DATA1, quickCfg.QuickSigData1, path);
                INIFile.SetValue(QUICK_SIG, QUICK_SIG_DATA2, quickCfg.QuickSigData2, path);
                INIFile.SetValue(QUICK_SIG, QUICK_DATA2_CHECK, quickCfg.QuickDataCheck + "", path);

                //保存完成
                IsCfgChanging = false;
            }
            catch (Exception Ex)
            {
                LogHelper.Log.Error(Ex.Message+Ex.StackTrace);
            }
        }
    }
}
