using CATest.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATest
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client sc = new Service1Client();
            Console.WriteLine(sc.GetData(123));
            CompositeType ct = new CompositeType();
            ct.BoolValue = true;
            ct.StringValue = "ddrsql";
            
            Console.WriteLine(sc.GetDataUsingDataContract(ct).StringValue);
            Console.ReadLine();
        }
    }
}
