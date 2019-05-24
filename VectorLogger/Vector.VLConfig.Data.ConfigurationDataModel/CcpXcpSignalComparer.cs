using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public class CcpXcpSignalComparer
	{
		public CcpXcpSignal Signal;

		public CcpXcpSignalComparer(CcpXcpSignal signal)
		{
			this.Signal = signal;
		}

		public override int GetHashCode()
		{
			return this.Signal.Name.Value.ToLowerInvariant().GetHashCode() * 17 + this.Signal.EcuName.Value.ToLowerInvariant().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			CcpXcpSignalComparer ccpXcpSignalComparer;
			if (obj is CcpXcpSignalComparer)
			{
				ccpXcpSignalComparer = (obj as CcpXcpSignalComparer);
			}
			else
			{
				if (!(obj is CcpXcpSignal))
				{
					return false;
				}
				ccpXcpSignalComparer = new CcpXcpSignalComparer(obj as CcpXcpSignal);
			}
			return this.Signal.Name.Value.ToLowerInvariant() == ccpXcpSignalComparer.Signal.Name.Value.ToLowerInvariant() && this.Signal.EcuName.Value.ToLowerInvariant() == ccpXcpSignalComparer.Signal.EcuName.Value.ToLowerInvariant();
		}
	}
}
