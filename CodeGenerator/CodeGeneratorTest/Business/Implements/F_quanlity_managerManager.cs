using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_quanlity_managerManager : BaseManager<F_quanlity_manager>, IF_quanlity_managerManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_quanlity_managerService service = new F_quanlity_managerService();

        public F_quanlity_managerManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
