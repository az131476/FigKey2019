using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwaggerApiDemo.Models
{
    /// <summary>
    /// 返回处理结果
    /// </summary>
    public class ResultJson
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
    }
}