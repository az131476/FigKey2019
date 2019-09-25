using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
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
