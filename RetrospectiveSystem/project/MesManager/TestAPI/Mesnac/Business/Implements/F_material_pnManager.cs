using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_material_pnManager : BaseManager<F_material_pn>, IF_material_pnManager
    {
		#region ����ע��
		
        private IF_material_pnService service = new F_material_pnService();

        public F_material_pnManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
