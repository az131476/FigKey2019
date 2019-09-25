using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_pass_rate_statisticManager : BaseManager<F_pass_rate_statistic>, IF_pass_rate_statisticManager
    {
		#region ÊôĞÔ×¢Èë
		
        private IF_pass_rate_statisticService service = new F_pass_rate_statisticService();

        public F_pass_rate_statisticManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
