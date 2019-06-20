using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESInterface.Model
{
    public enum FirstcheckResultEnum
    {
        /// <summary>
        /// 验证成功
        /// </summary>
        STATUS_SUCCESS = 0X00,
        /// <summary>
        /// 上一个站位失败
        /// </summary>
        ERR_LAST_STATION_FAIL = 0X01,
        /// <summary>
        /// 不是第一个站位
        /// </summary>
        ERR_NOT_FIRST_STATION = 0X02,
        /// <summary>
        /// 追溯码不存在
        /// </summary>
        ERR_RETROACTIVE_CODE_NOT_EXIST = 0X03,
        /// <summary>
        /// 型号不存在
        /// </summary>
        ERR_MODEL_NOT_EXIST = 0X04,
        /// <summary>
        /// 工站不存在
        /// </summary>
        ERR_STATION_NOT_EXIST = 0X05
    }
}