using System;
using System.Linq;
using System.Reflection;

namespace Vector.VLConfig
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class AssemblyAttributeServicePack : Attribute
	{
		private int servicePack;

		public int ServicePackNumber
		{
			get
			{
				return this.servicePack;
			}
		}

		public AssemblyAttributeServicePack() : this(0)
		{
		}

		public AssemblyAttributeServicePack(int servicePack)
		{
			this.servicePack = servicePack;
		}

		public static int GetServicePackNumber()
		{
			AssemblyAttributeServicePack assemblyAttributeServicePack = (AssemblyAttributeServicePack)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyAttributeServicePack), false).First<object>();
			return assemblyAttributeServicePack.ServicePackNumber;
		}

		public static bool IsServicePack()
		{
			return AssemblyAttributeServicePack.GetServicePackNumber() > 0;
		}
	}
}
