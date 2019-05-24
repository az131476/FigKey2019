using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CANoe
{
	[CompilerGenerated, Guid("CD866FB7-44BF-11D3-8538-00105A3E017B"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
	public interface _IMeasurementEvents
	{
		[DispId(1)]
		void OnInit();

		[DispId(2)]
		void OnStart();

		[DispId(3)]
		void OnStop();

		[DispId(4)]
		void OnExit();
	}
}
