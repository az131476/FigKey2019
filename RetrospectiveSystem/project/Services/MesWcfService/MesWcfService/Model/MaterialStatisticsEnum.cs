using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesWcfService.Model
{
    public class MaterialStatisticsEnum
    {
    }

    public enum MaterialStatisticsReturnCode
    {
        ERROR_SUCCESS,
        ERROR_ARRAY_LENGTH_NOT_ENOUGH,
        ERROR_ARRAY_LENGTH_OVER_FLOW,
        ERROR_USE_AMOUNT_NOT_INT_INDEX17,
    }

    public enum MaterialStateReturnCode
    {
        ERROR_NULL_STRING = 0,
        STATUS_USING = 1,
        STATUS_COMPLETE_NORMAL = 2,
        STATUS_COMPLETE_UNUSUAL = 3,
        ERROR_NULL_QUERY
    }
}