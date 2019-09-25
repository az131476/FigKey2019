using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_binding_pcbaManager : BaseManager<F_binding_pcba>, IF_binding_pcbaManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_binding_pcbaService service = new F_binding_pcbaService();

        public F_binding_pcbaManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
