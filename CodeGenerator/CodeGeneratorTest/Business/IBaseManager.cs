/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				IBaseManager.cs
 *      Description:
 *				 业务逻辑基础接口
 *      Author:
 *				唐小东
 *				1297953037@qq.com
 *				http://www.figkey.com
 *      Finish DateTime:
 *				2019年09月26日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business
{
    using CodeGeneratorTest.Data;
    public interface IBaseManager<T> : IBaseService<T>
    {

    }
}
