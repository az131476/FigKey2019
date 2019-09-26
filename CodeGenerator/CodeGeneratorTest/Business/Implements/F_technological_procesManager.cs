using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Business.Implements
{
    using CodeGeneratorTest.Entity;
    using CodeGeneratorTest.Data.Interface;
    using CodeGeneratorTest.Data.Implements;
    using CodeGeneratorTest.Business.Interface;
    public class F_technological_procesManager : BaseManager<F_technological_proces>, IF_technological_procesManager
    {
		#region  Ù–‘◊¢»Î
		
        private IF_technological_procesService service = new F_technological_procesService();

        public F_technological_procesManager()
        {
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
