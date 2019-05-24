using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IFeatureSymbolicDefinitions
	{
		IList<ISymbolicMessage> SymbolicMessages
		{
			get;
		}

		IList<ISymbolicSignal> SymbolicSignals
		{
			get;
		}

		IList<DiagnosticAction> SymbolicDiagnosticActions
		{
			get;
		}
	}
}
