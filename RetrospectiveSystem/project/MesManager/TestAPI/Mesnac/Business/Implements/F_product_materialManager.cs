using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_product_materialManager : BaseManager<F_product_material>, IF_product_materialManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_product_materialService service = new F_product_materialService();

        public F_product_materialManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
