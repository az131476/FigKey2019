using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_product_package_storageManager : BaseManager<F_product_package_storage>, IF_product_package_storageManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_product_package_storageService service = new F_product_package_storageService();

        public F_product_package_storageManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
