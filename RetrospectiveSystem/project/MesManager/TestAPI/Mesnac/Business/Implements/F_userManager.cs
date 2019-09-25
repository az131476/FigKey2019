using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_userManager : BaseManager<F_user>, IF_userManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_userService service = new F_userService();

        public F_userManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
