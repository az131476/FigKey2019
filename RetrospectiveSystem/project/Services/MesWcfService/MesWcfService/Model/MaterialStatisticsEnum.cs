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
        STATUS_FAIL = 0,
        STATUS_USCCESS = 1,
        ERROR_IS_NULL_TYPNO = 2,
        ERROR_IS_NULL_STATION_NAME = 3,
        ERROR_IS_NULL_MATERIAL_CODE = 4,
        ERROR_IS_NULL_AMOUNTED = 5,
        ERROR_USE_AMOUNT_NOT_INT = 6,
        ERROR_NOT_MATCH_MATERIAL_PN = 7,
        ERROR_NOT_AMOUNT_STATE = 8
    }

    public enum MaterialStateReturnCode
    {
        STATUS_USING = 1,
        STATUS_COMPLETE_NORMAL = 2,
        STATUS_COMPLETE_UNUSUAL = 3,
        ERROR_NULL_PRODUCT_TYPENO = 4,
        ERROR_NULL_MATERIAL_CODE = 5,
        STATUS_NULL_QUERY = 6,
        ERROR_FORMAT_MATERIAL_CODE = 7,
        ERROR_MATRIAL_CODE_IS_NOT_MATCH_WITH_PRODUCT_TYPENO = 8
    }

    public enum MaterialCheckMatchReturnCode
    {
        IS_NOT_MATCH = 0,
        IS_MATCH = 1,
        ERROR_NULL_PRODUCT_TYPENO = 2,
        ERROR_NULL_MATERIAL_PN =3
    }
}