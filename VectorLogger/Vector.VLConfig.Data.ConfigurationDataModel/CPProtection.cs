using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CPProtection")]
	public class CPProtection : IFile
	{
		[DataMember(Name = "CPProtectionECUName")]
		public ValidatedProperty<string> ECUName
		{
			get;
			set;
		}

		[DataMember(Name = "CPProtectionHasSeedAndKey")]
		public ValidatedProperty<bool> HasSeedAndKey
		{
			get;
			set;
		}

		[DataMember(Name = "CPProtectionSeedAndKeyFilePath")]
		public ValidatedProperty<string> SeedAndKeyFilePath
		{
			get;
			set;
		}

		public ValidatedProperty<string> FilePath
		{
			get
			{
				return this.SeedAndKeyFilePath;
			}
			set
			{
				this.SeedAndKeyFilePath = value;
			}
		}

		public CPProtection()
		{
			this.ECUName = new ValidatedProperty<string>("");
			this.HasSeedAndKey = new ValidatedProperty<bool>(false);
			this.SeedAndKeyFilePath = new ValidatedProperty<string>("");
		}

		public CPProtection(CPProtection other)
		{
			this.ECUName = new ValidatedProperty<string>(other.ECUName.Value);
			this.HasSeedAndKey = new ValidatedProperty<bool>(other.HasSeedAndKey.Value);
			this.SeedAndKeyFilePath = new ValidatedProperty<string>(other.SeedAndKeyFilePath.Value);
		}

		public CPProtection(string ecuName, bool hasSeedAndKeyProt)
		{
			this.ECUName = new ValidatedProperty<string>(ecuName);
			this.HasSeedAndKey = new ValidatedProperty<bool>(hasSeedAndKeyProt);
			this.SeedAndKeyFilePath = new ValidatedProperty<string>("");
		}

		public CPProtection(string ecuName) : this(ecuName, false)
		{
		}
	}
}
