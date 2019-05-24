using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface ISymbolicSignalEvent : ISymbolicSignal
	{
		ValidatedProperty<CondRelation> Relation
		{
			get;
			set;
		}

		ValidatedProperty<double> LowValue
		{
			get;
			set;
		}

		ValidatedProperty<double> HighValue
		{
			get;
			set;
		}

		ValidatedProperty<bool> IsFlexrayPDU
		{
			get;
			set;
		}

		ValidatedProperty<string> CcpXcpEcuName
		{
			get;
			set;
		}

		void Assign(ISymbolicSignalEvent other);
	}
}
