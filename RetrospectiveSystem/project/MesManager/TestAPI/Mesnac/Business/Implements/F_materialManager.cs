using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_materialManager : BaseManager<F_material>, IF_materialManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_materialService service = new F_materialService();

        public F_materialManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
