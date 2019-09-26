using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_material_statisticManager : BaseManager<F_material_statistic>, IF_material_statisticManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_material_statisticService service = new F_material_statisticService();

        public F_material_statisticManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
