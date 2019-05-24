using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class MDF4FinalizerTool : GenericToolInterface
	{
		public static readonly string SearchPatternAllUnfinishedMf4Files = "*.mf4u";

		public static readonly string SearchPatternAllFinishedMf4Files = "*.mf4";

		private readonly FileConversionParameters mConversionParameters;

		private static readonly Dictionary<int, string> sMdfLibErrorCodes = new Dictionary<int, string>
		{
			{
				-1,
				"unspecified error code"
			},
			{
				0,
				"OK"
			},
			{
				1,
				"general error"
			},
			{
				2,
				"functionality is not implemented / not supported (yet)"
			},
			{
				3,
				"functionality / element is available for this format / this file / this object"
			},
			{
				4,
				"out of memory (new failed)"
			},
			{
				5,
				"invalid argument"
			},
			{
				6,
				"index is out of boundary"
			},
			{
				7,
				"method not called from same thread"
			},
			{
				8,
				"data type overflow error"
			},
			{
				9,
				"operation was canceled by user"
			},
			{
				10,
				"file or file path not found"
			},
			{
				11,
				"file could not be opened"
			},
			{
				12,
				"unable to read from file"
			},
			{
				13,
				"file could not be created"
			},
			{
				14,
				"unable to write to file"
			},
			{
				15,
				"file already is open (warning)"
			},
			{
				16,
				"file already exists, must not overwrite!"
			},
			{
				17,
				"file already was sorted"
			},
			{
				18,
				"file could not be sorted"
			},
			{
				19,
				"file is empty"
			},
			{
				20,
				"format in file not supported (no MDF, or future MDF version)"
			},
			{
				21,
				"file has invalid format"
			},
			{
				22,
				"some part of the format or the formula could not be parsed (format or IO error)"
			},
			{
				23,
				"XML parser not available"
			},
			{
				24,
				"no converter DLL available"
			},
			{
				25,
				"reverse time stamp detected during Flush or SetTime (warning only)"
			},
			{
				30,
				"no data available"
			},
			{
				31,
				"reached end of data"
			},
			{
				32,
				"data type cannot be used for this operation"
			},
			{
				33,
				"result buffer too small"
			},
			{
				34,
				"given channel does not apply to data pointer"
			},
			{
				35,
				"not initialized"
			},
			{
				36,
				"no data available because before first data point (value of first data point is returned)"
			},
			{
				37,
				"channel group contains a VLSD channel"
			},
			{
				38,
				"not all channels have been written yet (detected during FlushRecord)"
			},
			{
				39,
				"channel value already has been written before (Write/SetTime)"
			},
			{
				40,
				"result type does not match"
			},
			{
				41,
				"conversion result is not valid"
			},
			{
				42,
				"conversion could not be evaluated"
			},
			{
				43,
				"division by zero"
			},
			{
				44,
				"logarithm of zero"
			},
			{
				45,
				"status string table entry found for raw value (numerical value returned as phys value might not make sense). note: numerical value returned is raw value converted by main conversion"
			},
			{
				46,
				"no inverse conversion available"
			},
			{
				50,
				"log sink was not registered"
			},
			{
				51,
				"no Log sink registered for this logging level"
			},
			{
				52,
				"library is not licensed, must call GetFileManager with valid license info"
			},
			{
				53,
				"a required Mdf4Lib dll is not found in the installed folder. Please copy all corresponding dlls in same folder"
			},
			{
				54,
				"interface compatibility check has failed. The installed libraries are not compatible with each other"
			},
			{
				55,
				"object is already closed"
			},
			{
				60,
				"cycle counters increased during sorting (can only occur for MDF3 and eFixMdf3AddRecords)"
			},
			{
				61,
				"cycle counters decreased during sorting (can only occur for MDF3, possibly indicates corrupt file)"
			},
			{
				66,
				"a data pointer object (IDataPointer or IGroupDataPointer) uses this channel group. Operation only possible after release of the data pointer object."
			},
			{
				67,
				"a data writer object (IDataWriter) uses this channel group. Operation only possible after release of the data writer object."
			},
			{
				68,
				"file is read-only and cannot be modified (not newly created nor opened with eReadWriteExclusive)"
			},
			{
				69,
				"a given interface pointer object is not contained in this file"
			},
			{
				70,
				"channel cannot be added because the Byte range of the parent element is fixed (cannot be exteded) and is smaller than the required Byte range of channel to be added"
			},
			{
				71,
				"channel or channel group with same identification already contained"
			},
			{
				73,
				"master channel with same synchronization type already contained in channel group, cannot add a second one"
			},
			{
				72,
				"no master channel with required synchronization type found"
			},
			{
				74,
				"unable to add child channels because parent channel cannot be a structure (array or no fixed length data type eByteArray)"
			},
			{
				75,
				"size for data type not allowed in MDF format or not supported by MDF4 Lib"
			},
			{
				76,
				"conversion type not allowed here"
			},
			{
				77,
				"input buffer too small, rest filled with zeros"
			},
			{
				78,
				"input buffer too large, data truncated"
			},
			{
				79,
				"context does not allow this operation, see log file for details"
			},
			{
				80,
				"conversion may not be monotone"
			},
			{
				81,
				"input string was too long and has been truncated (warning)"
			},
			{
				82,
				"invalid channel type for operation"
			},
			{
				83,
				"object instance still exists and cannot be removed"
			},
			{
				84,
				"not found"
			},
			{
				85,
				"unfinalized MDF file, must be finalized first (e.g. using IFileManager::FinalizeFile)"
			},
			{
				86,
				"file already is finalized"
			},
			{
				87,
				"file is unfinalized, but cannot be finalized due to some unknown finalization step (custom step of unknown application or new standard step)"
			},
			{
				88,
				"file is unfinalized, but cannot be finalized because a required finalization step is not implemented / not supported (yet)"
			},
			{
				100,
				"invalid event type for operation"
			},
			{
				101,
				"invalid event range type for operation"
			},
			{
				102,
				"invalid event scope for operation"
			},
			{
				103,
				"invalid sync type"
			},
			{
				104,
				"invalid event properties for operation"
			},
			{
				105,
				"invalid event hierarchy for operation"
			}
		};

		public MDF4FinalizerTool(FileConversionParameters conversionParameters)
		{
			this.mConversionParameters = conversionParameters;
			base.FileName = "MdfFinalize.exe";
		}

		public Result FinalizeFile(string sourceFileName, out string errorText)
		{
			errorText = string.Empty;
			if (!File.Exists(sourceFileName))
			{
				errorText = string.Format(Resources.InternalError, "File does not exist: " + sourceFileName);
				return Result.Error;
			}
			base.DeleteCommandLineArguments();
			if (this.mConversionParameters.OverwriteDestinationFiles)
			{
				base.AddCommandLineArgument("-O");
			}
			base.AddCommandLineArgument("-R");
			base.AddCommandLineArgument("-A");
			base.AddCommandLineArgument("-W");
			base.AddCommandLineArgument(FileSystemHelpers.MakeNicePath(sourceFileName));
			DummyProgressIndicator progressIndicator = new DummyProgressIndicator();
			WarningParser warningParser = new WarningParser();
			base.RunSynchronousWithProgressBar(progressIndicator, warningParser);
			if (base.LastExitCode != 0)
			{
				errorText = this.LastExitCodeText();
				return Result.Error;
			}
			if (warningParser.Errors.Contains(Resources.MDFFinalizeTimeStampErrorText))
			{
				errorText = string.Format(Resources.MDFFinalizeTimeStampErrorText, Path.GetFileName(sourceFileName));
				return Result.Error;
			}
			return Result.OK;
		}

		private string LastExitCodeText()
		{
			string text = MDF4FinalizerTool.sMdfLibErrorCodes.ContainsKey(base.LastExitCode) ? MDF4FinalizerTool.sMdfLibErrorCodes[base.LastExitCode] : MDF4FinalizerTool.sMdfLibErrorCodes[-1];
			string str = string.Concat(new object[]
			{
				"MdfFinalize error ",
				base.LastExitCode,
				": ",
				text
			});
			return str + GenerationUtilVN.DebugInfo(base.LastStdOut);
		}
	}
}
