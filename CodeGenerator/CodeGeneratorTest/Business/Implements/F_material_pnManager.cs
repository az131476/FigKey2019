using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_material_pnManager : BaseManager<F_material_pn>, IF_material_pnManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_material_pnService service = new F_material_pnService();

        public F_material_pnManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
