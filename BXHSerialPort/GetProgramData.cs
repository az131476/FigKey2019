using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    class GetProgramData
    {
        public List<List<string>> readPressure(string filePath)
        {
            List<List<string>> pressure = new List<List<string>>();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string strLine;
            while ((strLine = sr.ReadLine()) != null)
            {
                string[] strLineArray = strLine.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries); 
                List<string> lineList = new List<string>();
                for (int i = 0; i < strLineArray.Length; i++)
                {
                    lineList.Add(strLineArray[i]);
                }
                pressure.Add(lineList);
            }
            sr.Close();
            fs.Close();
            return pressure;
        }
        public void writeCurrent(string filePath, List<List<float>> current)
        {
            string s = "";
            int count = 0;
            foreach (var item in current)
            {
                s += "checksum";
                count = 1;
                foreach (var item2 in item)
                {
                    count++;
                    if (1 == count)
                    {
                        s += item2.ToString("f6");
                    }
                    else
                    {
                        s += "\t\t" + item2.ToString("f6");
                    }
                    if (count % 8 == 0)
                    {
                        s += "\r\n";
                        count = 0;
                    }
                }
            }
            File.WriteAllText(filePath, s);
        }
        public List<List<float>> readCSVgetCurrent(List<List<string>> pressure, string fileName, List<List<string>> currStandardData, List<List<string>> currStandardDataSearch)
        {
            List<List<float>> dataList;
            int lineCount = pressure.Count;
            List<List<float>> current = new List<List<float>>();
            for (int i = 0; i < lineCount; i++)
            {
                List<float> l = new List<float>();
                current.Add(l);
            }
            string[] tag = File.ReadAllLines(@".\SearchString.txt");
            if (tag.Length != 24)
            {
                string errormsg = "SearchString.txt文件中数据应为24行！";
                throw new Exception(errormsg);
            }
            //MessageBox.Show("pre open", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            dataList = CSVHelper.OpenCSV(fileName,tag);
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Count<=0)
                {
                    string errormsg = "SearchString.txt中"+(i+1)+"行("+tag[i]+")在原始数据里没有找到";
                    throw new Exception(errormsg);
                }
            }
            //MessageBox.Show("tail open", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //start
            INIHelper ini = new INIHelper(@"./CurrStandard.ini", "Demo");
            string[] iniKey = { "PinkValue" };
            string[] iniSetting = new string[1];
            ini.read(iniKey,out iniSetting);
            for (int i = 0; i < dataList.Count / 4; i++)
            {
                float[] x = dataList[i * 4].ToArray();
                float[] y = dataList[i * 4 + 1].ToArray();
                float[] xx = dataList[i * 4+2].ToArray();
                float[] yy = dataList[i * 4 + 3].ToArray();
                for (int k = 0; k < x.Length; k++)
                {
                    x[k] = x[k] / 1000;
                }
                for (int k = 0; k < xx.Length; k++)
                {
                    xx[k] = xx[k] / 1000;
                }
                for (int j = 0; j < pressure[i].Count; j++)
                {
                    //MessageBox.Show("average pre", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AverageXAverageY a = new BXHSerialPort.AverageXAverageY();
                    float v1=0;
                    float v2=0;
                    bool v1Find = a.ifFind(x, y, pressure[i][j], currStandardDataSearch[i][j]);
                    bool v2Find = a.ifFind(xx, yy, pressure[i][j], currStandardDataSearch[i][j]);
                    if (v1Find || v2Find)
                    {
                          v2 = a.getAverageX(xx, yy, pressure[i][j], currStandardData[i][j], currStandardDataSearch[i][j]);
                          v1 = a.getAverageX(x, y, pressure[i][j], currStandardData[i][j], currStandardDataSearch[i][j]);
                    }
                    //else if (v1Find&&(!v2Find))
                    //{
                    //    v1 = a.getAverageX(x, y, pressure[i][j], currStandardData[i][j], currStandardDataSearch[i][j]);
                    //    v2 = v1;
                    //    v2 = a.getAverageX(xx, yy, pressure[i][j], currStandardData[i][j], currStandardDataSearch[i][j]);
                    //}
                    //else if ((!v1Find) && v2Find)
                    //{
                    //    v2 = a.getAverageX(xx, yy, pressure[i][j], currStandardData[i][j], currStandardDataSearch[i][j]);
                    //    v1 = v2;
                    //    v1 = a.getAverageX(x, y, pressure[i][j], currStandardData[i][j], currStandardDataSearch[i][j]);
                    //}
                    else
                    {
                        string errormsg = "没有搜索到这个压力数据:" + pressure[i][j] + "" ;
                        throw new Exception(errormsg);
                    }
                    //MessageBox.Show("shuju", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    float resultValue = (v1 + v2) / 2;
                    if ((currStandardDataSearch[i][j]!="55")&& (currStandardDataSearch[i][j] != "66")&&(pressure[i][j]!="NA"))
                    {

                        float currStand;
                        float pinkValue;
                        try
                        {
                            currStand = Convert.ToSingle(currStandardData[i][j]);
                            pinkValue = Convert.ToSingle(iniSetting[0]);
                        }
                        catch (Exception)
                        {
                            string errormsg = "判断粉色部分的数据值，标准电流:" + currStandardData[i][j]+"，行:"+(i+1)+"第"+(j+1)+"个数据"+",差值基准:" + iniSetting[0];
                            throw new Exception(errormsg);
                        }
                        float abs = Math.Abs(resultValue - currStand);
                        if (abs> pinkValue)
                        {
                            string errormsg = "(粉色部分的数据值 - TCU内部标准值) =差值的绝对值 > "+ pinkValue+ "，\n行:" + (i + 1) + "第" + (j + 1) + "个数据,压力值："+ pressure[i][j]+",计算值："+ resultValue + "，标准值："+ currStandardData[i][j];
                            throw new Exception(errormsg);
                        }
                    }
                    current[i].Add(resultValue);
                }
            }
            //end
            
            return current;
        }
        public List<float> redDiff(List<List<float>> currList, List<List<string>> currStandardData)
        {
            int[] rowIndex = { 0,1,1, 2,4,5,5};
            int[] colIndex = { 4,2,17,2,7,7,17};
            List<float> list = new List<float>();
            for (int i = 0; i < rowIndex.Length; i++)
            {
                float currStand;
                float curr;
                try
                {
                    currStand = Convert.ToSingle(currStandardData[rowIndex[i]][colIndex[i]]);
                    curr = currList[rowIndex[i]][colIndex[i]];
                }
                catch (Exception)
                {
                    string errormsg = "计算红框差值，电流数据:\"" + currList[rowIndex[i]][colIndex[i]] + "\"，"+ "标准电流:\"" + currList[rowIndex[i]][colIndex[i]] + "\"。";
                    throw new Exception(errormsg);
                }
                float currDiff = curr - currStand;
                
                list.Add(currDiff);
            }
            return list;
        }
        public List<List<float>> greenCal(List<List<string>> isSearch, List<List<float>> currList,List<float> redValue, List<List<string>> currStandardData)
        {
            for (int i = 0; i < currList.Count; i++)
            {
                for (int j = 0; j < currList[i].Count; j++)
                {
                    if (isSearch[i][j]=="66")
                    {
                        float redCellValue = 0;
                        if (0==i)
                        {
                            redCellValue = redValue[0];
                        }
                        else if (1==i)
                        {
                            if (j < 9)
                            {
                                redCellValue = redValue[1];
                            }
                            else
                            {
                                redCellValue = redValue[2];
                            }
                        }
                        else if (2 == i)
                        {
                            redCellValue = redValue[3];
                        }
                        else if (3 == i)
                        {
                        }
                        else if (4 == i)
                        {
                            redCellValue = redValue[4];
                        }
                        else if (5 == i)
                        {
                            if (j < 9)
                            {
                                redCellValue = redValue[5];
                            }
                            else
                            {
                                redCellValue = redValue[6];
                            }
                        }
                        float currStand ;
                        try
                        {
                            currStand = Convert.ToSingle(currStandardData[i][j]);
                        }
                        catch (Exception)
                        {
                            string errormsg = "计算绿框值时，标准电流:\"" + currStandardData[i][j] + "\"。";
                            throw new Exception(errormsg);
                        }
                        float c = currStand + redCellValue;
                        if (c>1.1)
                        {
                            currList[i][j] = 1.1F;
                        }
                        else
                        {
                            currList[i][j] = c;
                        }
                    }
                }
            }
            return currList;
        }
        public bool judgeCurr(List<List<float>> curr)
        {
            bool isOk = true;
            
            CurrRangeCtl c = new BXHSerialPort.CurrRangeCtl();
            string[] Setting = c.read();
            if (Setting==null)
            {
                return false;
            }
            
            for (int i = 0; i < curr.Count; i++)
            {
                float itemHeadH = Convert.ToSingle(Setting[i*2]);
                float itemHeadL = Convert.ToSingle(Setting[i*2+1]);
                float itemTailH = Convert.ToSingle(Setting[i * 2+2]);
                float itemTailL = Convert.ToSingle(Setting[i *2 +3]);
                bool isHeadOk = curr[i][0] >= itemHeadL && curr[i][0] <= itemHeadH;
                int tailIndex = curr[i].Count - 1;
                bool isTailOk = curr[i][tailIndex] >= itemTailL && curr[i][tailIndex] <= itemTailH;
                isOk = isHeadOk && isTailOk;
                if (!isOk)
                {
                    MessageBox.Show("第"+(i+1)+"条电流曲线不在范围内", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            return isOk;
        }
    }
}
