using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_product_check_recordManager : BaseManager<F_product_check_record>, IF_product_check_recordManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_product_check_recordService service = new F_product_check_recordService();

        public F_product_check_recordManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
