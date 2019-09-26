using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_materialManager : BaseManager<F_material>, IF_materialManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_materialService service = new F_materialService();

        public F_materialManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
