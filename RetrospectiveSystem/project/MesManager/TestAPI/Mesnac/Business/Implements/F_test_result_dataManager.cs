using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_test_result_dataManager : BaseManager<F_test_result_data>, IF_test_result_dataManager
    {
		#region ÊôĞÔ×¢Èë
		
        private IF_test_result_dataService service = new F_test_result_dataService();

        public F_test_result_dataManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
