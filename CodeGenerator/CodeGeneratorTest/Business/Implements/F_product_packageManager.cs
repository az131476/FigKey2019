using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_product_packageManager : BaseManager<F_product_package>, IF_product_packageManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_product_packageService service = new F_product_packageService();

        public F_product_packageManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
