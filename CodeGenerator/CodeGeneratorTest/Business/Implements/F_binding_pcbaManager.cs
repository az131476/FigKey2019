using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_binding_pcbaManager : BaseManager<F_binding_pcba>, IF_binding_pcbaManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_binding_pcbaService service = new F_binding_pcbaService();

        public F_binding_pcbaManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
