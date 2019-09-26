using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_test_result_dataManager : BaseManager<F_test_result_data>, IF_test_result_dataManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_test_result_dataService service = new F_test_result_dataService();

        public F_test_result_dataManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
