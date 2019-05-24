using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    class AverageXAverageY
    {

        public bool ifFind(float[] x,float[] y,string inputYstring, string isSearch)
        {
            if (inputYstring == "NA")
            {
                return true;
            }
            if (isSearch == "55")
            {
                return true;
            }
            if (isSearch == "66")
            {
                return true;
            }
            float inputY ;
            try
            {
                inputY = Convert.ToSingle(inputYstring);
            }
            catch (Exception)
            {
                string errormsg = "寻找是否存在此压力，压力数据:\""+ inputYstring+"\"";
                throw new Exception(errormsg);
            }

            int minYindex = 0;
            int maxYindex = 0;
            if (y.Length > 0)
            {
                float tempMin = y[0];
                float tempMax = y[0];
                for (int i = 0; i < y.Length; i++)
                {
                    if (tempMin > y[i])
                    {
                        tempMin = y[i];
                        minYindex = i;
                    }
                    if (tempMax < y[i])
                    {
                        tempMax = y[i];
                        maxYindex = i;
                    }
                }
            }
            float minX = x[minYindex];
            float maxX = x[maxYindex];

            float minY = y.Min();
            float maxY = y.Max();
            if (inputY < minY)
            {
                //string errormsg = "没有搜索到这个压力数据：" + inputYstring + "，最小压力为：" + minY + "！";
                //throw new Exception(errormsg);
                return false;
            }
            if (inputY > maxY)
            {
                //string errormsg = "没有搜索到这个压力数据：" + inputYstring + "，最大压力为：" + maxY + "！";
                //throw new Exception(errormsg);
                return false;
            }
            return true;
        }
        public float getAverageX(float[] x,float[] y, string inputYstring,string currStandard,string isSearch)
        {
            List<float> xList = new List<float>();
            if (inputYstring == "NA")
            {
                return 0;
            }
            float inputY;
            float inputCurr;
            try
            {
                 inputY = Convert.ToSingle(inputYstring);
                 inputCurr = Convert.ToSingle(currStandard);
            }
            catch (Exception)
            {
                string errormsg = "取电流时，压力数据:\"" + inputYstring + "\"，"+ "标准电流:\"" + currStandard + "\"。";
                throw new Exception(errormsg);
            }
            if (isSearch=="55")//黄色
            {
                return inputCurr;
            }
            if (isSearch == "66")//绿色
            {
                return inputCurr;
            }

            int minYindex = 0;
            int maxYindex = 0;
            if (y.Length > 0)
            {
                float tempMin = y[0];
                float tempMax = y[0];
                for (int i = 0; i < y.Length; i++)
                {
                    if (tempMin > y[i])
                    {
                        tempMin = y[i];
                        minYindex = i;
                    }
                    if (tempMax < y[i])
                    {
                        tempMax = y[i];
                        maxYindex = i;
                    }
                }
            }
            float minX = x[minYindex];
            float maxX = x[maxYindex];

            float minY = y.Min();
            float maxY = y.Max();
            if (inputY < minY)
            {
                return minX;
            }
            if (inputY > maxY)
            {
                //string errormsg = "没有搜索到这个压力数据：" + inputYstring + "，最大压力为：" + maxY + "！";
                //throw new Exception(errormsg);
                return maxX;
            }

            for (int i = 1; i < x.Length; i++)
            {
                float y1 = y[i];
                float y2 = y[i - 1];
                float x1 = x[i];
                float x2 = x[i - 1];
                if (inputY==y1)
                {
                    return x1;
                }
                if (inputY==y2)
                {
                    return x2;
                }
                LinegetXgetY line = new LinegetXgetY(x1,y1,x2,y2);
                if ((inputY > y1)^ (inputY > y2))
                {
                    xList.Add(line.getX(inputY));
                }
                else if ((inputY == y1)&&(inputY==y2))
                {
                    xList.Add((x1+x2)/2);
                }
            }
            if (xList.Count<=0)
            {
                //MessageBox.Show("no data", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errormsg = "no data";
                throw new Exception(errormsg);
                //return 0;
            }
            return xList.Sum()/xList.Count;
        }
    }
}
