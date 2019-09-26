using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_product_materialManager : BaseManager<F_product_material>, IF_product_materialManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_product_materialService service = new F_product_materialService();

        public F_product_materialManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
