using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtils.ByteHelper;
using System.Data;
using System.Drawing;
using CommonUtils.Logger;
using Telerik.WinControls.UI;

namespace SentConfigurator.Controls
{
    /// <summary>
    /// 配置并转换指令
    /// </summary>
    /// <returns></returns>
    public class CommandConfig
    {
        private int codedataLen;
        private string checkSum;
        private int totalLen;
        private string cmdhead = "";
        private string cmdbase = "";
        private string cmdslowSig = "";
        private string cmdQuickSig = "";
        private SignalConfig.InputStringType strType;

        //帧头+长度(服务码+数据)+服务码+数据+校验和(长度+服务码+数据)
        //长度：服务码（1+1+1）数据（5+3*num+5）

        public CommandConfig()
        {
        }

        #region 计算命令长度
        /// <summary>
        /// 计算命令长度
        /// </summary>
        /// <param name="groupCount"></param>
        public void CalInitParams(int groupCount,SignalConfig.InputStringType inputType)
        {
            codedataLen = 1 + 5 + 1+3 * groupCount + 5;
            checkSum = "";
            totalLen = 2 + 1 + codedataLen + 1;

            cmdhead = "FF|EE|";
            cmdbase = "A0|";
            cmdslowSig = "";
            cmdQuickSig = "";
            this.strType = inputType;
        }
        #endregion

        #region 基础信号命令
        /// <summary>
        /// 基础信号命令
        /// </summary>
        /// <param name="cob_dataframe_type"></param>
        /// <param name="cob_battery_state"></param>
        /// <param name="cob_serial_msg"></param>
        /// <param name="cob_timeframe"></param>
        /// <returns></returns>
        public bool UnionBaseCommand(ComboBox cob_dataframe_type, ComboBox cob_battery_state,ComboBox cob_serial_msg, TextBox tbx_timeframe)
        {
            #region 基础信号
            //数据帧类型
            if (cob_dataframe_type.SelectedIndex == 0)
            {
                cmdbase += "00|";
            }
            else if (cob_dataframe_type.SelectedIndex == 1)
            {
                cmdbase += "01|";
            }
            //空闲电平状态
            if (cob_battery_state.SelectedIndex == 0)
            {
                cmdbase += "00|";
            }
            else if (cob_battery_state.SelectedIndex == 1)
            {
                cmdbase += "01|";
            }
            //扩展串行消息
            if (cob_serial_msg.SelectedIndex == 0)
            {
                cmdbase += "00|";
            }
            else if (cob_serial_msg.SelectedIndex == 1)
            {
                cmdbase += "01|";
            }
            //一帧时间
            if (!string.IsNullOrEmpty(tbx_timeframe.Text))
            {
                if (strType == SignalConfig.InputStringType.DEC)
                {
                    string timeframe = Convert.ToString(int.Parse(tbx_timeframe.Text), 16).PadLeft(4, '0');
                    timeframe = timeframe.Substring(2, 2) + "|" + timeframe.Substring(0, 2);
                    cmdbase += timeframe + "|";//转16 
                }
                else if (strType == SignalConfig.InputStringType.HEX)
                {
                    string timeframe = tbx_timeframe.Text.Trim().ToLower().Replace("0x", "").PadLeft(4, '0');
                    timeframe = timeframe.Substring(2, 2) + "|" + timeframe.Substring(0, 2);
                    cmdbase += timeframe + "|";
                }
            }
            else
            {
                MessageBox.Show("帧时间次数不能为空", "提示");
                return false;
            }
            LogHelper.Log.Info("基础信号："+cmdbase);
            return true;
            #endregion
        }
        #endregion

        #region 慢信号命令
        /// <summary>
        /// 慢信号命令
        /// </summary>
        /// <param name="dgv_groupdata"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool UnionSlowCommand(RadGridView dgv_groupdata,string groupCount)
        {
            #region 慢信号
            int num = 0;
            try
            {
                //组数
                if (strType == SignalConfig.InputStringType.DEC)
                {
                    num = int.Parse(groupCount);
                    cmdslowSig += Convert.ToString(num, 16) + "|";
                }
                else if (strType == SignalConfig.InputStringType.HEX)
                {
                    num = int.Parse(ConvertString.ConvertToDec(groupCount));
                    cmdslowSig += groupCount.ToString().Trim().ToLower().Replace("0x", "")+"|";
                }

                for (int i = 0; i < num; i++)
                {
                    string group_serial_id = dgv_groupdata.Rows[i].Cells[1].Value.ToString();
                    string group_data = dgv_groupdata.Rows[i].Cells[2].Value.ToString();
                    string groupData = "";
                    if (strType == SignalConfig.InputStringType.DEC)
                    {
                        cmdslowSig += Convert.ToString(int.Parse(group_serial_id), 16) + "|";
                        //data1与data2为一组数据，低位在前，高位在后
                        groupData = ConvertString.ConvertToHex(group_data, 4).ToLower().Replace("0x","");

                    }
                    else if (strType == SignalConfig.InputStringType.HEX)
                    {
                        cmdslowSig += group_serial_id.Trim().ToLower().Replace("0x","").PadLeft(4,'0') + "|";
                        //data1与data2为一组数据，低位在前，高位在后
                        groupData = group_data.Trim().ToLower().Replace("0x", "").PadLeft(4,'0');
                    }
                    cmdslowSig += groupData.Substring(2, 2) + "|" + groupData.Substring(0, 2)+"|";
                }
                LogHelper.Log.Info(" 慢信号：" + cmdslowSig);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.Message+ex.StackTrace);
                return false;
            }
            #endregion
        }
        #endregion

        #region 快信号命令
        /// <summary>
        /// 快信号命令
        /// </summary>
        /// <param name="cob_quicksig_type"></param>
        /// <param name="tbx_quicksig_data1"></param>
        /// <param name="tbx_quicksig_data2"></param>
        /// <returns></returns>
        public bool UnionQuickCommand(ComboBox cob_quicksig_type,TextBox tbx_quicksig_data1,TextBox tbx_quicksig_data2,RadCheckBox chxOrder)
        {
            #region 快信号
            if (cob_quicksig_type.SelectedIndex == 0)
            {
                cmdQuickSig += "00|";
            }
            else if (cob_quicksig_type.SelectedIndex == 1)
            {
                cmdQuickSig += "01|";
            }
            if (!string.IsNullOrEmpty(tbx_quicksig_data1.Text))
            {
                if (strType == SignalConfig.InputStringType.DEC)
                {
                    string hexd1 = Convert.ToString(int.Parse(tbx_quicksig_data1.Text), 16).PadLeft(4, '0');
                    hexd1 = hexd1.Substring(2, 2) + "|" + hexd1.Substring(0, 2);
                    cmdQuickSig += hexd1 + "|";
                }
                else if (strType == SignalConfig.InputStringType.HEX)
                {
                    string hexd1 = tbx_quicksig_data1.Text.Trim().ToLower().Replace("0x", "").PadLeft(4, '0');
                    hexd1 = hexd1.Substring(2, 2) + "|" + hexd1.Substring(0, 2);
                    cmdQuickSig += hexd1 + "|";
                }
            }
            else
            {
                MessageBox.Show("快信号data1不能为空", "提示");
                return false;
            }

            if (!string.IsNullOrEmpty(tbx_quicksig_data2.Text))
            {
                if (strType == SignalConfig.InputStringType.DEC)
                {
                    //0345 勾选改变高低位顺序
                    string hexd2 = Convert.ToString(int.Parse(tbx_quicksig_data2.Text), 16).PadLeft(4, '0');
                    if (chxOrder.Checked)
                    {
                        //4305
                        hexd2 = hexd2.Substring(2, 1) + hexd2.Substring(1, 1) + "|" + hexd2.Substring(0, 1) + hexd2.Substring(3, 1);
                    }
                    else
                    {
                        //4503
                        hexd2 = hexd2.Substring(2, 2) + "|" + hexd2.Substring(0, 2);
                    }
                    cmdQuickSig += hexd2 + "|";
                }
                else if (strType == SignalConfig.InputStringType.HEX)
                {
                    string hexd2 = tbx_quicksig_data2.Text.Trim().ToLower().Replace("0x", "").PadLeft(4, '0');
                    if (chxOrder.Checked)
                    {
                        hexd2 = hexd2.Substring(2, 1) + hexd2.Substring(1, 1) + "|" + hexd2.Substring(0, 1) + hexd2.Substring(3, 1);
                    }
                    else
                    {
                        hexd2 = hexd2.Substring(2, 2) + "|" + hexd2.Substring(0, 2);
                    }
                    cmdQuickSig += hexd2 + "|";
                }
            }
            else
            {
                MessageBox.Show("快信号data2不能为空", "提示");
                return false;
            }
            LogHelper.Log.Info("快信号："+cmdQuickSig);
            return true;
            #endregion
        }
        #endregion

        #region 读配置数组
        /// <summary>
        /// 设置配置命令数组
        /// </summary>
        /// <returns></returns>
        public string[] SetCfgHexByte()
        {
            try
            {
                checkSum = Convert.ToString(codedataLen, 16) + "|" + "A0" + "|" + cmdbase.Substring(3) + cmdslowSig + cmdQuickSig.Substring(0,cmdQuickSig.Length-1);
                string hexStr = cmdhead + Convert.ToString(codedataLen, 16) + "|" + cmdbase + cmdslowSig + cmdQuickSig + HexSum();
                return hexStr.Split(new char[] { '|'}, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace+" \r\n");
                return null;
            }
        }
        #endregion

        #region 校验和
        /// <summary>
        /// 校验和
        /// </summary>
        /// <returns></returns>
        private string HexSum()
        {
            string[] hexCheckSum = checkSum.Split(new char[] { '|'});
            int sum = 0;
            string sumRes = "";
            for (int i = 0; i < hexCheckSum.Length; i++)
            {
                sum += Convert.ToInt32((hexCheckSum[i]),16);
            }

            if (sum > 255)
            {
                //超过上限，取低位
                sumRes = Convert.ToString(sum, 16).PadLeft(4, '0');
                sumRes = sumRes.Substring(2, 2);
            }
            else
            {
                sumRes = Convert.ToString(sum, 16);
            }
            LogHelper.Log.Info("校验和（int）："+sum+" hex:"+sumRes);
            return sumRes;
        }
        #endregion

        #region 16进制字符串转byte
        /// <summary>
        /// string命令转byte
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        public byte[] HexToByte(string[] strArray)
        {
            byte[] sendData;
            try
            {
                sendData = new byte[strArray.Length];
                ConvertByte.HexStringToByte(sendData, strArray, 0);
                LogHelper.Log.Info(BitConverter.ToString(sendData));
                return sendData;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
                return null;
            }
        }
        #endregion

        #region 写配置数组
        /// <summary>
        /// 读取配置命令数组
        /// </summary>
        /// <returns></returns>
        public string[] ReadCfgHexByte()
        {
            try
            {
                string hexstr = "FF|EE|01|B0|B1";
                totalLen = 7;
                return hexstr.Split(new char[] { '|' });
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
                return null;
            }
        }
        #endregion

        #region ASCII命令
        /// <summary>
        /// 返回String命令
        /// </summary>
        /// <returns></returns>
        public StringBuilder ASCIISendMsg()
        {
            StringBuilder sbstringcmd = new StringBuilder();
            //ASCII
            sbstringcmd.Append(cmdhead);
            sbstringcmd.Append(Convert.ToString(codedataLen, 16));
            sbstringcmd.Append(cmdbase);
            sbstringcmd.Append(cmdslowSig);
            sbstringcmd.Append(cmdQuickSig);
            //sbstringcmd.Append(Convert.ToString(checkSum, 16));
            return sbstringcmd;
        }
        #endregion
    }
}
