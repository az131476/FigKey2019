using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_pass_rate_statisticManager : BaseManager<F_pass_rate_statistic>, IF_pass_rate_statisticManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_pass_rate_statisticService service = new F_pass_rate_statisticService();

        public F_pass_rate_statisticManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
