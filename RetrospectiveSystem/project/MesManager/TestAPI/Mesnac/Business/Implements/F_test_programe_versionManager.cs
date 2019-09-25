using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Business.Implements
{
    using Mesnac.Entity;
    using Mesnac.Data.Interface;
    using Mesnac.Data.Implements;
    using Mesnac.Business.Interface;
    public class F_test_programe_versionManager : BaseManager<F_test_programe_version>, IF_test_programe_versionManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_test_programe_versionService service = new F_test_programe_versionService();

        public F_test_programe_versionManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
