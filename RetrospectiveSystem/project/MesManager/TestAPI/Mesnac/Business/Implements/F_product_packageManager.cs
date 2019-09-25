using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_product_packageManager : BaseManager<F_product_package>, IF_product_packageManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_product_packageService service = new F_product_packageService();

        public F_product_packageManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
