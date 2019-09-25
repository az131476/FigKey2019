using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
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
