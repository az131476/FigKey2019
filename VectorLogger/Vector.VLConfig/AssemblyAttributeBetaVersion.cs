using System;
using System.Linq;
using System.Reflection;

namespace Vector.VLConfig
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class AssemblyAttributeBetaVersion : Attribute
	{
		private bool isBeta;

		public AssemblyAttributeBetaVersion() : this(false)
		{
		}

		public AssemblyAttributeBetaVersion(bool isBeta)
		{
			this.isBeta = isBeta;
		}

		public static bool IsBeta()
		{
			AssemblyAttributeBetaVersion assemblyAttributeBetaVersion = (AssemblyAttributeBetaVersion)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyAttributeBetaVersion), false).First<object>();
			return assemblyAttributeBetaVersion.isBeta;
		}
	}
}
