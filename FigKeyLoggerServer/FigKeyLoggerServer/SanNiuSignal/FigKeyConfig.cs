using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CommonUtils.Logger;
using FigKeyLoggerServer.Model;

namespace FigKeyLoggerServer.SanNiuSignal
{
    public class FigKeyConfig
    {
        /// <summary>
        /// 初始化配置
        /// </summary>
        public FigKeyConfig()
        {
            try
            {
                string sport = ConfigurationManager.AppSettings["clientPort"].ToString();
                if (!string.IsNullOrEmpty(sport))
                {
                    TcpPort = int.Parse(sport);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + " " + Diagnostis.GetLineNum());
            }
        }

        public int TcpPort { get; set; }
    }
}
