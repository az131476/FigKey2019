using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_test_log_dataManager : BaseManager<F_test_log_data>, IF_test_log_dataManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_test_log_dataService service = new F_test_log_dataService();

        public F_test_log_dataManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
