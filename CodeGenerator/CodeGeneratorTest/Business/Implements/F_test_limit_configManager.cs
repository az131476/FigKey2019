using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_test_limit_configManager : BaseManager<F_test_limit_config>, IF_test_limit_configManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_test_limit_configService service = new F_test_limit_configService();

        public F_test_limit_configManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
