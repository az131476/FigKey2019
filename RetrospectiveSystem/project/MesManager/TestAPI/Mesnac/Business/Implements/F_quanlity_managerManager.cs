using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_quanlity_managerManager : BaseManager<F_quanlity_manager>, IF_quanlity_managerManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_quanlity_managerService service = new F_quanlity_managerService();

        public F_quanlity_managerManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
