using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
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
