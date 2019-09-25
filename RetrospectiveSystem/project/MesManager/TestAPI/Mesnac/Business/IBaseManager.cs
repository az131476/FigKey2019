/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				IBaseManager.cs
 *      Description:
 *				 业务逻辑基础接口
 *      Author:
 *				作者
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2019年09月25日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business
{
    using Mesnac.Data;
    public interface IBaseManager<T> : IBaseService<T>
    {

    }
}
