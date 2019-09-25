using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_technological_procesManager : BaseManager<F_technological_proces>, IF_technological_procesManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_technological_procesService service = new F_technological_procesService();

        public F_technological_procesManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
