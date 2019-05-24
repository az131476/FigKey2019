using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GUI;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.BusinessLogic
{
	public class FileConversionProfile
	{
		private string mDisplayName = string.Empty;

		private XmlDocument mDoc;

		private FileConversionParameters mLoadedParameters;

		private List<string> mMarkers = new List<string>();

		private List<string> mTriggers = new List<string>();

		private readonly List<FileConversionProfileError> mErrors = new List<FileConversionProfileError>();

		public LoggerType LoggerTypeForImageIndex
		{
			get;
			set;
		}

		public int ImageIndex
		{
			get
			{
				return this.GetStateImageIndex(EnumViewType.Common, this.LoggerTypeForImageIndex);
			}
		}

		public string DisplayName
		{
			get
			{
				return this.mDisplayName;
			}
			set
			{
				this.HasChanged |= !this.mDisplayName.Equals(value);
				this.mDisplayName = value;
			}
		}

		public string File
		{
			get
			{
				if (string.IsNullOrEmpty(this.FilePath))
				{
					return this.FilePath;
				}
				return Path.GetFileName(this.FilePath);
			}
		}

		public string FilePath
		{
			get;
			private set;
		}

		public bool HasMarkers
		{
			get;
			set;
		}

		public bool HasTriggers
		{
			get;
			set;
		}

		public bool HasChanged
		{
			get;
			private set;
		}

		public EnumViewType ViewType
		{
			get;
			private set;
		}

		public LoggerType LoggerType
		{
			get;
			private set;
		}

		public FileConversionProfile(string filePath)
		{
			this.FilePath = filePath;
			this.LoadFromFile();
			this.HasChanged = false;
		}

		public void LoadFromFile()
		{
			this.mDoc = new XmlDocument();
			this.mLoadedParameters = FileConversionHelper.GetDefaultParameters(LoggerType.Unknown);
			this.DisplayName = string.Empty;
			this.HasMarkers = false;
			this.HasTriggers = false;
			this.ViewType = EnumViewType.Unknown;
			this.mErrors.Clear();
			FileInfo fileInfo = new FileInfo(this.FilePath);
			if (!fileInfo.Exists)
			{
				this.AddError(EnumViewType.Common, EnumProfileError.FileDoesNotExist, "");
				return;
			}
			try
			{
				this.mDoc.Load(this.FilePath);
			}
			catch (Exception)
			{
				this.AddError(EnumViewType.Common, EnumProfileError.InvalidFileContent, "");
				return;
			}
			if (this.mDoc.GetElementsByTagName("FileConversionProfile").Count != 1)
			{
				this.AddError(EnumViewType.Common, EnumProfileError.InvalidFileContent, "");
			}
			if (this.mDoc.GetElementsByTagName("FileConversionParameters").Count != 1)
			{
				this.AddError(EnumViewType.Common, EnumProfileError.InvalidFileContent, "");
			}
			this.ViewType = this.GetViewType();
			this.LoggerType = this.GetLoggerType();
			this.LoggerTypeForImageIndex = this.LoggerType;
			this.DisplayName = this.GetNodeInnerText("DisplayName", string.Empty, EnumViewType.Common);
			if (!this.HasErrors(this.ViewType))
			{
				this.mLoadedParameters = FileConversionHelper.GetDefaultParameters(this.LoggerType);
				this.mLoadedParameters.DestinationFormat = this.GetNodeValueEnum<FileConversionDestFormat>("DestinationFormat", this.mLoadedParameters.DestinationFormat, EnumViewType.Common);
				this.mLoadedParameters.DestinationFormatVersionStrings = this.GetNodeValueStringArray("DestinationFormatVersionStrings", this.mLoadedParameters.DestinationFormatVersionStrings, EnumViewType.Common);
				this.mLoadedParameters.DestinationFormatExtensions = this.GetNodeValueStringArray("DestinationFormatExtensions", this.mLoadedParameters.DestinationFormatExtensions, EnumViewType.Common);
				this.mLoadedParameters.OverwriteDestinationFiles = this.GetNodeValueBool("OverwriteDestinationFiles", this.mLoadedParameters.OverwriteDestinationFiles, EnumViewType.Common);
				this.mLoadedParameters.FilenameFormat = this.GetNodeValueEnum<FileConversionFilenameFormat>("FilenameFormat", this.mLoadedParameters.FilenameFormat, EnumViewType.Common);
				this.mLoadedParameters.Prefix = this.GetNodeInnerText("Prefix", this.mLoadedParameters.Prefix, EnumViewType.Common);
				this.mLoadedParameters.CustomFilename = this.GetNodeInnerText("CustomFilename", this.mLoadedParameters.CustomFilename, EnumViewType.Common);
				this.mLoadedParameters.HexadecimalNotation = this.GetNodeValueBool("HexadecimalNotation", this.mLoadedParameters.HexadecimalNotation, EnumViewType.Common);
				this.mLoadedParameters.SingleFile = this.GetNodeValueBool("SingleFile", this.mLoadedParameters.SingleFile, EnumViewType.Common);
				this.mLoadedParameters.GlobalTimestamps = this.GetNodeValueBool("GlobalTimestamps", this.mLoadedParameters.GlobalTimestamps, EnumViewType.Common);
				this.mLoadedParameters.RelativeTimestamps = this.GetNodeValueBool("RelativeTimestamps", this.mLoadedParameters.RelativeTimestamps, EnumViewType.Common);
				this.mLoadedParameters.GermanMSExcelFormat = this.GetNodeValueBool("GermanMSExcelFormat", this.mLoadedParameters.GermanMSExcelFormat, EnumViewType.Common);
				this.mLoadedParameters.UseMinimumDigitsForTriggerIndex = this.GetNodeValueBool("UseMinimumDigitsForTriggerIndex", this.mLoadedParameters.UseMinimumDigitsForTriggerIndex, EnumViewType.Common);
				this.mLoadedParameters.MinimumDigitsForTriggerIndex = this.GetNodeValueInt("MinimumDigitsForTriggerIndex", this.mLoadedParameters.MinimumDigitsForTriggerIndex, EnumViewType.Common);
				this.mLoadedParameters.CopyMediaFiles = this.GetNodeValueBool("CopyMediaFiles", this.mLoadedParameters.CopyMediaFiles, EnumViewType.Common);
				this.mLoadedParameters.WriteRawValues = this.GetNodeValueBool("WriteRawValues", this.mLoadedParameters.WriteRawValues, EnumViewType.Common);
				this.mLoadedParameters.MDF3SignalFormat = this.GetNodeValueEnum<FileConversionMDF3SignalFormat>("MDF3SignalFormat", this.mLoadedParameters.MDF3SignalFormat, EnumViewType.Common);
				this.mLoadedParameters.MDF3SignalFormatDelimiter = this.GetNodeInnerText("MDF3SignalFormatDelimiter", this.mLoadedParameters.MDF3SignalFormatDelimiter, EnumViewType.Common);
				this.mLoadedParameters.UseUnlimitedFileSize = this.GetNodeValueBool("UseUnlimitedFileSize", this.mLoadedParameters.UseUnlimitedFileSize, EnumViewType.Common);
				this.mLoadedParameters.SplitFilesBySize = this.GetNodeValueBool("SplitFilesBySize", this.mLoadedParameters.SplitFilesBySize, EnumViewType.Common);
				this.mLoadedParameters.FileFractionSize = this.GetNodeValueInt("FileFractionSize", this.mLoadedParameters.FileFractionSize, EnumViewType.Common);
				this.mLoadedParameters.SplitFilesByTime = this.GetNodeValueBool("SplitFilesByTime", this.mLoadedParameters.SplitFilesByTime, EnumViewType.Common);
				this.mLoadedParameters.FileFractionTime = this.GetNodeValueInt("FileFractionTime", this.mLoadedParameters.FileFractionTime, EnumViewType.Common);
				this.mLoadedParameters.TimeBase = this.GetNodeValueEnum<FileConversionTimeBase>("TimeBase", this.mLoadedParameters.TimeBase, EnumViewType.Common);
				this.mLoadedParameters.UseRealTimeRaster = this.GetNodeValueBool("UseRealTimeRaster", this.mLoadedParameters.UseRealTimeRaster, EnumViewType.Common);
				this.mLoadedParameters.UseDateTimeFromMeasurementStart = this.GetNodeValueBool("UseDateTimeFromMeasurementStart", this.mLoadedParameters.UseDateTimeFromMeasurementStart, EnumViewType.Common);
				this.mLoadedParameters.UseChannelMapping = this.GetNodeValueBool("UseChannelMapping", this.mLoadedParameters.UseChannelMapping, EnumViewType.Common);
				this.mLoadedParameters.HideChannelMappingIdentities = this.GetNodeValueBool("HideChannelMappingIdentities", this.mLoadedParameters.HideChannelMappingIdentities, EnumViewType.Common);
				this.mLoadedParameters.CanChannelMapping = this.GetNodeValueUIntArray("CanChannelMapping", this.mLoadedParameters.CanChannelMapping, EnumViewType.Common);
				this.mLoadedParameters.LinChannelMapping = this.GetNodeValueUIntArray("LinChannelMapping", this.mLoadedParameters.LinChannelMapping, EnumViewType.Common);
				this.mLoadedParameters.FlexRayChannelMapping = this.GetNodeValueUIntArray("FlexRayChannelMapping", this.mLoadedParameters.FlexRayChannelMapping, EnumViewType.Common);
				if (this.ViewType != EnumViewType.ClfExport)
				{
					this.AddError(EnumViewType.ClfExport, EnumProfileError.ViewTypeMissmatch, FileConversionHelper.GetName(this.ViewType));
				}
				if (this.mLoadedParameters.DestinationFormat == FileConversionDestFormat.CLF)
				{
					this.AddError(EnumViewType.ClfExport, EnumProfileError.InvalidValue, "DestinationFormat");
				}
				this.mLoadedParameters.RecoveryMode = this.GetNodeValueBool("RecoveryMode", this.mLoadedParameters.RecoveryMode, EnumViewType.ClfExport);
				this.mLoadedParameters.JumpOverSleepTime = this.GetNodeValueBool("JumpOverSleepTime", this.mLoadedParameters.JumpOverSleepTime, EnumViewType.ClfExport);
				if (this.ViewType != EnumViewType.Classic)
				{
					this.AddError(EnumViewType.Classic, EnumProfileError.ViewTypeMissmatch, FileConversionHelper.GetName(this.ViewType));
				}
				this.mLoadedParameters.DeleteSourceFilesWhenDone = this.GetNodeValueBool("DeleteSourceFilesWhenDone", this.mLoadedParameters.DeleteSourceFilesWhenDone, EnumViewType.Classic);
				this.mLoadedParameters.SaveRawFile = this.GetNodeValueBool("SaveRawFile", this.mLoadedParameters.SaveRawFile, EnumViewType.Classic);
				this.mLoadedParameters.GenerateVsysvarFile = this.GetNodeValueBool("GenerateVsysvarFile", this.mLoadedParameters.GenerateVsysvarFile, EnumViewType.Classic);
				this.mLoadedParameters.RecoveryMode = this.GetNodeValueBool("RecoveryMode", this.mLoadedParameters.RecoveryMode, EnumViewType.Classic);
				this.mLoadedParameters.JumpOverSleepTime = this.GetNodeValueBool("JumpOverSleepTime", this.mLoadedParameters.JumpOverSleepTime, EnumViewType.Classic);
				this.mLoadedParameters.SuppressBufferConcat = this.GetNodeValueBool("SuppressBufferConcat", this.mLoadedParameters.SuppressBufferConcat, EnumViewType.Classic);
				if (this.ViewType != EnumViewType.Navigator)
				{
					this.AddError(EnumViewType.Navigator, EnumProfileError.ViewTypeMissmatch, FileConversionHelper.GetName(this.ViewType));
				}
				this.mLoadedParameters.SaveRawFile = this.GetNodeValueBool("SaveRawFile", this.mLoadedParameters.SaveRawFile, EnumViewType.Navigator);
				this.mLoadedParameters.GenerateVsysvarFile = this.GetNodeValueBool("GenerateVsysvarFile", this.mLoadedParameters.GenerateVsysvarFile, EnumViewType.Navigator);
				this.mLoadedParameters.SuppressBufferConcat = this.GetNodeValueBool("SuppressBufferConcat", this.mLoadedParameters.SuppressBufferConcat, EnumViewType.Navigator);
				XmlNodeList elementsByTagName = this.mDoc.GetElementsByTagName("Marker");
				this.mMarkers = (from XmlNode tmpNode in elementsByTagName
				select tmpNode.InnerText).ToList<string>();
				this.HasMarkers = this.mMarkers.Any<string>();
				XmlNodeList elementsByTagName2 = this.mDoc.GetElementsByTagName("Trigger");
				this.mTriggers = (from XmlNode tmpNode in elementsByTagName2
				select tmpNode.InnerText).ToList<string>();
				this.HasTriggers = this.mTriggers.Any<string>();
				return;
			}
		}

		public void Save()
		{
			if (this.HasErrors(this.ViewType) || !this.HasChanged)
			{
				return;
			}
			this.SetNodeInnerText("DisplayName", this.DisplayName);
			this.mDoc.Save(this.FilePath);
			this.HasChanged = false;
		}

		public void SaveFrom(IFileConversionParametersClient client)
		{
			if (client == null)
			{
				return;
			}
			FileConversionParameters fileConversionParameters = client.FileConversionParameters;
			if (fileConversionParameters == null)
			{
				return;
			}
			string directoryName = Path.GetDirectoryName(this.FilePath);
			if (string.IsNullOrEmpty(directoryName))
			{
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			this.mDoc = new XmlDocument();
			XmlNode newChild = this.mDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
			this.mDoc.AppendChild(newChild);
			XmlNode parentNode = this.AppendChild(this.mDoc, "FileConversionProfile", null);
			this.AppendChild(parentNode, "ViewType", Enum.GetName(typeof(EnumViewType), client.ViewType));
			this.AppendChild(parentNode, "LoggerType", Enum.GetName(typeof(LoggerType), client.LoggerType));
			this.AppendChild(parentNode, "DisplayName", this.DisplayName);
			XmlNode parentNode2 = this.AppendChild(parentNode, "FileConversionParameters", null);
			this.AppendChild(parentNode2, "DestinationFormat", Enum.GetName(typeof(FileConversionDestFormat), fileConversionParameters.DestinationFormat));
			this.AppendChild(parentNode2, "DestinationFormatVersionStrings", fileConversionParameters.DestinationFormatVersionStrings);
			this.AppendChild(parentNode2, "DestinationFormatExtensions", fileConversionParameters.DestinationFormatExtensions);
			this.AppendChild(parentNode2, "DeleteSourceFilesWhenDone", fileConversionParameters.DeleteSourceFilesWhenDone);
			this.AppendChild(parentNode2, "OverwriteDestinationFiles", fileConversionParameters.OverwriteDestinationFiles);
			this.AppendChild(parentNode2, "SaveRawFile", fileConversionParameters.SaveRawFile);
			this.AppendChild(parentNode2, "DestinationFolder", fileConversionParameters.DestinationFolder);
			this.AppendChild(parentNode2, "FilenameFormat", Enum.GetName(typeof(FileConversionFilenameFormat), fileConversionParameters.FilenameFormat));
			this.AppendChild(parentNode2, "Prefix", fileConversionParameters.Prefix);
			this.AppendChild(parentNode2, "CustomFilename", fileConversionParameters.CustomFilename);
			this.AppendChild(parentNode2, "HexadecimalNotation", fileConversionParameters.HexadecimalNotation);
			this.AppendChild(parentNode2, "SingleFile", fileConversionParameters.SingleFile);
			this.AppendChild(parentNode2, "GlobalTimestamps", fileConversionParameters.GlobalTimestamps);
			this.AppendChild(parentNode2, "RelativeTimestamps", fileConversionParameters.RelativeTimestamps);
			this.AppendChild(parentNode2, "RecoveryMode", fileConversionParameters.RecoveryMode);
			this.AppendChild(parentNode2, "GermanMSExcelFormat", fileConversionParameters.GermanMSExcelFormat);
			this.AppendChild(parentNode2, "JumpOverSleepTime", fileConversionParameters.JumpOverSleepTime);
			this.AppendChild(parentNode2, "GenerateVsysvarFile", fileConversionParameters.GenerateVsysvarFile);
			this.AppendChild(parentNode2, "UseMinimumDigitsForTriggerIndex", fileConversionParameters.UseMinimumDigitsForTriggerIndex);
			this.AppendChild(parentNode2, "MinimumDigitsForTriggerIndex", fileConversionParameters.MinimumDigitsForTriggerIndex);
			this.AppendChild(parentNode2, "CopyMediaFiles", fileConversionParameters.CopyMediaFiles);
			this.AppendChild(parentNode2, "WriteRawValues", fileConversionParameters.WriteRawValues);
			this.AppendChild(parentNode2, "MDF3SignalFormat", Enum.GetName(typeof(FileConversionMDF3SignalFormat), fileConversionParameters.MDF3SignalFormat));
			this.AppendChild(parentNode2, "MDF3SignalFormatDelimiter", fileConversionParameters.MDF3SignalFormatDelimiter);
			this.AppendChild(parentNode2, "UseUnlimitedFileSize", fileConversionParameters.UseUnlimitedFileSize);
			this.AppendChild(parentNode2, "SplitFilesBySize", fileConversionParameters.SplitFilesBySize);
			this.AppendChild(parentNode2, "FileFractionSize", fileConversionParameters.FileFractionSize);
			this.AppendChild(parentNode2, "SplitFilesByTime", fileConversionParameters.SplitFilesByTime);
			this.AppendChild(parentNode2, "FileFractionTime", fileConversionParameters.FileFractionTime);
			this.AppendChild(parentNode2, "TimeBase", Enum.GetName(typeof(FileConversionTimeBase), fileConversionParameters.TimeBase));
			this.AppendChild(parentNode2, "UseRealTimeRaster", fileConversionParameters.UseRealTimeRaster);
			this.AppendChild(parentNode2, "SuppressBufferConcat", fileConversionParameters.SuppressBufferConcat);
			this.AppendChild(parentNode2, "UseDateTimeFromMeasurementStart", fileConversionParameters.UseDateTimeFromMeasurementStart);
			this.AppendChild(parentNode2, "UseChannelMapping", fileConversionParameters.UseChannelMapping);
			this.AppendChild(parentNode2, "HideChannelMappingIdentities", fileConversionParameters.HideChannelMappingIdentities);
			this.AppendChild(parentNode2, "CanChannelMapping", fileConversionParameters.CanChannelMapping);
			this.AppendChild(parentNode2, "LinChannelMapping", fileConversionParameters.LinChannelMapping);
			this.AppendChild(parentNode2, "FlexRayChannelMapping", fileConversionParameters.FlexRayChannelMapping);
			IFileConversionParametersExClient fileConversionParametersExClient = client as IFileConversionParametersExClient;
			if (fileConversionParametersExClient != null)
			{
				if (this.HasMarkers)
				{
					XmlNode parentNode3 = this.AppendChild(parentNode, "MarkerTypeSelection", null);
					IEnumerable<string> markerTypeSelection = fileConversionParametersExClient.GetMarkerTypeSelection();
					foreach (string current in markerTypeSelection)
					{
						this.AppendChild(parentNode3, "Marker", current);
					}
				}
				if (this.HasTriggers)
				{
					XmlNode parentNode4 = this.AppendChild(parentNode, "TriggerTypeSelection", null);
					IEnumerable<string> triggerTypeSelection = fileConversionParametersExClient.GetTriggerTypeSelection();
					foreach (string current2 in triggerTypeSelection)
					{
						this.AppendChild(parentNode4, "Trigger", current2);
					}
				}
			}
			this.mDoc.Save(this.FilePath);
			this.LoadFromFile();
		}

		public void ApplyTo(IFileConversionParametersClient client)
		{
			this.LoadFromFile();
			if (this.HasErrors(client.ViewType))
			{
				return;
			}
			FileConversionParameters fileConversionParameters = new FileConversionParameters(client.FileConversionParameters);
			this.ApplyCommon(fileConversionParameters, client.ViewType, client.LoggerType);
			switch (client.ViewType)
			{
			case EnumViewType.ClfExport:
				this.ApplyClfExportSpecific(fileConversionParameters);
				break;
			case EnumViewType.Classic:
				this.ApplyClassicSpecific(fileConversionParameters);
				break;
			case EnumViewType.Navigator:
				this.ApplyNavigatorSpecific(fileConversionParameters, client as IFileConversionParametersExClient);
				break;
			}
			client.FileConversionParameters = fileConversionParameters;
		}

		private void ApplyCommon(FileConversionParameters targetParams, EnumViewType clientViewType, LoggerType clientLoggerType)
		{
			targetParams.DestinationFormat = this.GetValue<FileConversionDestFormat>(clientViewType, "DestinationFormat", this.mLoadedParameters.DestinationFormat, targetParams.DestinationFormat);
			targetParams.DestinationFormatVersionStrings = this.GetValue<string[]>(clientViewType, "DestinationFormatVersionStrings", this.mLoadedParameters.DestinationFormatVersionStrings, targetParams.DestinationFormatVersionStrings);
			targetParams.DestinationFormatExtensions = this.GetValue<string[]>(clientViewType, "DestinationFormatExtensions", this.mLoadedParameters.DestinationFormatExtensions, targetParams.DestinationFormatExtensions);
			targetParams.OverwriteDestinationFiles = this.GetValue<bool>(clientViewType, "OverwriteDestinationFiles", this.mLoadedParameters.OverwriteDestinationFiles, targetParams.OverwriteDestinationFiles);
			targetParams.FilenameFormat = this.GetValue<FileConversionFilenameFormat>(clientViewType, "FilenameFormat", this.mLoadedParameters.FilenameFormat, targetParams.FilenameFormat);
			targetParams.Prefix = this.GetValue<string>(clientViewType, "Prefix", this.mLoadedParameters.Prefix, targetParams.Prefix);
			targetParams.CustomFilename = this.GetValue<string>(clientViewType, "CustomFilename", this.mLoadedParameters.CustomFilename, targetParams.CustomFilename);
			targetParams.HexadecimalNotation = this.GetValue<bool>(clientViewType, "HexadecimalNotation", this.mLoadedParameters.HexadecimalNotation, targetParams.HexadecimalNotation);
			targetParams.SingleFile = this.GetValue<bool>(clientViewType, "SingleFile", this.mLoadedParameters.SingleFile, targetParams.SingleFile);
			targetParams.GlobalTimestamps = this.GetValue<bool>(clientViewType, "GlobalTimestamps", this.mLoadedParameters.GlobalTimestamps, targetParams.GlobalTimestamps);
			targetParams.RelativeTimestamps = this.GetValue<bool>(clientViewType, "RelativeTimestamps", this.mLoadedParameters.RelativeTimestamps, targetParams.RelativeTimestamps);
			targetParams.GermanMSExcelFormat = this.GetValue<bool>(clientViewType, "GermanMSExcelFormat", this.mLoadedParameters.GermanMSExcelFormat, targetParams.GermanMSExcelFormat);
			targetParams.UseMinimumDigitsForTriggerIndex = this.GetValue<bool>(clientViewType, "UseMinimumDigitsForTriggerIndex", this.mLoadedParameters.UseMinimumDigitsForTriggerIndex, targetParams.UseMinimumDigitsForTriggerIndex);
			targetParams.MinimumDigitsForTriggerIndex = this.GetValue<int>(clientViewType, "MinimumDigitsForTriggerIndex", this.mLoadedParameters.MinimumDigitsForTriggerIndex, targetParams.MinimumDigitsForTriggerIndex);
			targetParams.CopyMediaFiles = this.GetValue<bool>(clientViewType, "CopyMediaFiles", this.mLoadedParameters.CopyMediaFiles, targetParams.CopyMediaFiles);
			targetParams.WriteRawValues = this.GetValue<bool>(clientViewType, "WriteRawValues", this.mLoadedParameters.WriteRawValues, targetParams.WriteRawValues);
			targetParams.MDF3SignalFormat = this.GetValue<FileConversionMDF3SignalFormat>(clientViewType, "MDF3SignalFormat", this.mLoadedParameters.MDF3SignalFormat, targetParams.MDF3SignalFormat);
			targetParams.MDF3SignalFormatDelimiter = this.GetValue<string>(clientViewType, "MDF3SignalFormatDelimiter", this.mLoadedParameters.MDF3SignalFormatDelimiter, targetParams.MDF3SignalFormatDelimiter);
			targetParams.UseUnlimitedFileSize = this.GetValue<bool>(clientViewType, "UseUnlimitedFileSize", this.mLoadedParameters.UseUnlimitedFileSize, targetParams.UseUnlimitedFileSize);
			targetParams.SplitFilesBySize = this.GetValue<bool>(clientViewType, "SplitFilesBySize", this.mLoadedParameters.SplitFilesBySize, targetParams.SplitFilesBySize);
			targetParams.FileFractionSize = this.GetValue<int>(clientViewType, "FileFractionSize", this.mLoadedParameters.FileFractionSize, targetParams.FileFractionSize);
			targetParams.SplitFilesByTime = this.GetValue<bool>(clientViewType, "SplitFilesByTime", this.mLoadedParameters.SplitFilesByTime, targetParams.SplitFilesByTime);
			targetParams.FileFractionTime = this.GetValue<int>(clientViewType, "FileFractionTime", this.mLoadedParameters.FileFractionTime, targetParams.FileFractionTime);
			targetParams.TimeBase = this.GetValue<FileConversionTimeBase>(clientViewType, "TimeBase", this.mLoadedParameters.TimeBase, targetParams.TimeBase);
			targetParams.UseRealTimeRaster = this.GetValue<bool>(clientViewType, "UseRealTimeRaster", this.mLoadedParameters.UseRealTimeRaster, targetParams.UseRealTimeRaster);
			targetParams.UseDateTimeFromMeasurementStart = this.GetValue<bool>(clientViewType, "UseDateTimeFromMeasurementStart", this.mLoadedParameters.UseDateTimeFromMeasurementStart, targetParams.UseDateTimeFromMeasurementStart);
			targetParams.UseChannelMapping = this.GetValue<bool>(clientViewType, "UseChannelMapping", this.mLoadedParameters.UseChannelMapping, targetParams.UseChannelMapping);
			targetParams.HideChannelMappingIdentities = this.GetValue<bool>(clientViewType, "HideChannelMappingIdentities", this.mLoadedParameters.HideChannelMappingIdentities, targetParams.HideChannelMappingIdentities);
			targetParams.CanChannelMapping = this.GetChannelMapping(clientViewType, clientLoggerType, BusType.Bt_CAN, "CanChannelMapping", this.mLoadedParameters.CanChannelMapping, targetParams.CanChannelMapping);
			targetParams.LinChannelMapping = this.GetChannelMapping(clientViewType, clientLoggerType, BusType.Bt_LIN, "LinChannelMapping", this.mLoadedParameters.LinChannelMapping, targetParams.LinChannelMapping);
			targetParams.FlexRayChannelMapping = this.GetChannelMapping(clientViewType, clientLoggerType, BusType.Bt_FlexRay, "FlexRayChannelMapping", this.mLoadedParameters.FlexRayChannelMapping, targetParams.FlexRayChannelMapping);
		}

		private void ApplyClfExportSpecific(FileConversionParameters targetParams)
		{
			targetParams.RecoveryMode = this.GetValue<bool>(EnumViewType.ClfExport, "RecoveryMode", this.mLoadedParameters.RecoveryMode, targetParams.RecoveryMode);
			targetParams.JumpOverSleepTime = this.GetValue<bool>(EnumViewType.ClfExport, "JumpOverSleepTime", this.mLoadedParameters.JumpOverSleepTime, targetParams.JumpOverSleepTime);
		}

		private void ApplyClassicSpecific(FileConversionParameters targetParams)
		{
			targetParams.DeleteSourceFilesWhenDone = this.GetValue<bool>(EnumViewType.Classic, "DeleteSourceFilesWhenDone", this.mLoadedParameters.DeleteSourceFilesWhenDone, targetParams.DeleteSourceFilesWhenDone);
			targetParams.SaveRawFile = this.GetValue<bool>(EnumViewType.Classic, "SaveRawFile", this.mLoadedParameters.SaveRawFile, targetParams.SaveRawFile);
			targetParams.GenerateVsysvarFile = this.GetValue<bool>(EnumViewType.Classic, "GenerateVsysvarFile", this.mLoadedParameters.GenerateVsysvarFile, targetParams.GenerateVsysvarFile);
			targetParams.RecoveryMode = this.GetValue<bool>(EnumViewType.Classic, "RecoveryMode", this.mLoadedParameters.RecoveryMode, targetParams.RecoveryMode);
			targetParams.JumpOverSleepTime = this.GetValue<bool>(EnumViewType.Classic, "JumpOverSleepTime", this.mLoadedParameters.JumpOverSleepTime, targetParams.JumpOverSleepTime);
			targetParams.SuppressBufferConcat = this.GetValue<bool>(EnumViewType.Classic, "SuppressBufferConcat", this.mLoadedParameters.SuppressBufferConcat, targetParams.SuppressBufferConcat);
		}

		private void ApplyNavigatorSpecific(FileConversionParameters targetParams, IFileConversionParametersExClient exClient)
		{
			targetParams.SaveRawFile = this.GetValue<bool>(EnumViewType.Navigator, "SaveRawFile", this.mLoadedParameters.SaveRawFile, targetParams.SaveRawFile);
			targetParams.GenerateVsysvarFile = this.GetValue<bool>(EnumViewType.Navigator, "GenerateVsysvarFile", this.mLoadedParameters.GenerateVsysvarFile, targetParams.GenerateVsysvarFile);
			targetParams.SuppressBufferConcat = this.GetValue<bool>(EnumViewType.Navigator, "SuppressBufferConcat", this.mLoadedParameters.SuppressBufferConcat, targetParams.SuppressBufferConcat);
			if (exClient == null)
			{
				return;
			}
			if (this.HasMarkers)
			{
				List<string> source = new List<string>(exClient.RestoreMarkerTypeSelection(this.mMarkers));
				if (source.Any<string>())
				{
					this.AddError(EnumViewType.Navigator, EnumProfileError.MarkerRestoration, "");
				}
			}
			if (this.HasTriggers)
			{
				List<string> source2 = new List<string>(exClient.RestoreTriggerTypeSelection(this.mTriggers));
				if (source2.Any<string>())
				{
					this.AddError(EnumViewType.Navigator, EnumProfileError.TriggerRestoration, "");
				}
			}
		}

		private T GetValue<T>(EnumViewType viewType, string tagName, T loadedValue, T clientValue)
		{
			if (!this.HasErrorOrWarning(viewType, tagName))
			{
				return loadedValue;
			}
			return clientValue;
		}

		private uint[] GetChannelMapping(EnumViewType viewType, LoggerType clientLoggerType, BusType busType, string tagName, uint[] loadedValue, uint[] clientValue)
		{
			if (this.HasErrorOrWarning(viewType, tagName))
			{
				return clientValue;
			}
			uint numMappingChannels = this.GetNumMappingChannels(clientLoggerType, busType);
			uint numMappingChannels2 = this.GetNumMappingChannels(this.LoggerType, busType);
			uint num = Math.Min(numMappingChannels2, numMappingChannels);
			uint[] array = new uint[clientValue.Length];
			uint num2;
			for (num2 = 0u; num2 < num; num2 += 1u)
			{
				array[(int)((UIntPtr)num2)] = loadedValue[(int)((UIntPtr)num2)];
			}
			while ((ulong)num2 < (ulong)((long)array.Length))
			{
				array[(int)((UIntPtr)num2)] = clientValue[(int)((UIntPtr)num2)];
				num2 += 1u;
			}
			return array;
		}

		private uint GetNumMappingChannels(LoggerType loggerType, BusType busType)
		{
			ILoggerSpecifics loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(loggerType);
			switch (busType)
			{
			case BusType.Bt_CAN:
				return loggerSpecifics.CAN.NumberOfChannels + loggerSpecifics.CAN.NumberOfVirtualChannels;
			case BusType.Bt_LIN:
				return loggerSpecifics.LIN.NumberOfChannels + Constants.MaximumNumberOfLINprobeChannels;
			case BusType.Bt_FlexRay:
				return loggerSpecifics.Flexray.NumberOfChannels;
			default:
				return 0u;
			}
		}

		private bool HasErrorOrWarning(EnumViewType viewType, string tagName)
		{
			return this.mErrors.Any((FileConversionProfileError t) => t.TagName.Equals(tagName, StringComparison.Ordinal) && (t.ViewType == viewType || t.ViewType == EnumViewType.Common) && (t.IsError || t.IsWarning));
		}

		private XmlNode AppendChild(XmlNode parentNode, string tagName, string innerText = null)
		{
			if (parentNode == null)
			{
				return null;
			}
			XmlNode xmlNode = this.mDoc.CreateElement(tagName);
			if (innerText != null)
			{
				xmlNode.InnerText = innerText;
			}
			parentNode.AppendChild(xmlNode);
			return xmlNode;
		}

		private void AppendChild(XmlNode parentNode, string tagName, bool value)
		{
			this.AppendChild(parentNode, tagName, value ? bool.TrueString : bool.FalseString);
		}

		private void AppendChild(XmlNode parentNode, string tagName, int value)
		{
			this.AppendChild(parentNode, tagName, value.ToString(CultureInfo.InvariantCulture));
		}

		private void AppendChild(XmlNode parentNode, string tagName, uint value)
		{
			this.AppendChild(parentNode, tagName, value.ToString(CultureInfo.InvariantCulture));
		}

		private void AppendChild(XmlNode parentNode, string tagName, uint[] value)
		{
			XmlNode parentNode2 = this.AppendChild(parentNode, tagName, null);
			for (int i = 0; i < value.Length; i++)
			{
				this.AppendChild(parentNode2, "uint", value[i]);
			}
		}

		private void AppendChild(XmlNode parentNode, string tagName, double value)
		{
			this.AppendChild(parentNode, tagName, value.ToString(CultureInfo.InvariantCulture));
		}

		private void AppendChild(XmlNode parentNode, string tagName, double[] value)
		{
			XmlNode parentNode2 = this.AppendChild(parentNode, tagName, null);
			for (int i = 0; i < value.Length; i++)
			{
				this.AppendChild(parentNode2, "double", value[i]);
			}
		}

		private void AppendChild(XmlNode parentNode, string tagName, string[] value)
		{
			XmlNode parentNode2 = this.AppendChild(parentNode, tagName, null);
			for (int i = 0; i < value.Length; i++)
			{
				this.AppendChild(parentNode2, "string", value[i]);
			}
		}

		private void SetNodeInnerText(string tagName, string value)
		{
			XmlNodeList elementsByTagName = this.mDoc.GetElementsByTagName(tagName);
			if (elementsByTagName.Count >= 1)
			{
				elementsByTagName[0].InnerText = value;
			}
		}

		private string GetNodeInnerText(string tagName, string defaultValue, EnumViewType viewType)
		{
			XmlNodeList elementsByTagName = this.mDoc.GetElementsByTagName(tagName);
			if (elementsByTagName.Count >= 1)
			{
				return elementsByTagName[0].InnerText;
			}
			this.AddError(viewType, EnumProfileError.TagMissing, tagName);
			return defaultValue;
		}

		private bool GetNodeValueBool(string tagName, bool defaultValue, EnumViewType viewType)
		{
			string nodeInnerText = this.GetNodeInnerText(tagName, defaultValue ? bool.TrueString : bool.FalseString, viewType);
			bool result;
			if (bool.TryParse(nodeInnerText, out result))
			{
				return result;
			}
			this.AddError(viewType, EnumProfileError.InvalidValue, tagName);
			return defaultValue;
		}

		private int GetNodeValueInt(string tagName, int defaultValue, EnumViewType viewType)
		{
			string nodeInnerText = this.GetNodeInnerText(tagName, defaultValue.ToString(CultureInfo.InvariantCulture), viewType);
			int result;
			if (int.TryParse(nodeInnerText, out result))
			{
				return result;
			}
			this.AddError(viewType, EnumProfileError.InvalidValue, tagName);
			return defaultValue;
		}

		private T GetNodeValueEnum<T>(string tagName, T defaultValue, EnumViewType viewType)
		{
			string nodeInnerText = this.GetNodeInnerText(tagName, Enum.GetName(typeof(T), defaultValue), viewType);
			if (!typeof(T).IsEnum)
			{
				return defaultValue;
			}
			foreach (object current in Enum.GetValues(typeof(T)))
			{
				if (nodeInnerText.Equals(Enum.GetName(typeof(T), current)))
				{
					return (T)((object)current);
				}
			}
			this.AddError(viewType, EnumProfileError.InvalidValue, tagName);
			return defaultValue;
		}

		private EnumViewType GetViewType()
		{
			string nodeInnerText = this.GetNodeInnerText("ViewType", Enum.GetName(typeof(EnumViewType), EnumViewType.Unknown), EnumViewType.Common);
			foreach (EnumViewType enumViewType in Enum.GetValues(typeof(EnumViewType)))
			{
				if (nodeInnerText.Equals(Enum.GetName(typeof(EnumViewType), enumViewType)) && enumViewType != EnumViewType.Unknown && enumViewType != EnumViewType.Common)
				{
					return enumViewType;
				}
			}
			this.AddError(EnumViewType.Common, EnumProfileError.InvalidFileContent, "");
			return EnumViewType.Unknown;
		}

		private LoggerType GetLoggerType()
		{
			string nodeInnerText = this.GetNodeInnerText("LoggerType", Enum.GetName(typeof(LoggerType), LoggerType.Unknown), EnumViewType.Common);
			foreach (LoggerType loggerType in Enum.GetValues(typeof(LoggerType)))
			{
				if (nodeInnerText.Equals(Enum.GetName(typeof(LoggerType), loggerType)) && loggerType != LoggerType.Unknown)
				{
					return loggerType;
				}
			}
			this.AddError(EnumViewType.Common, EnumProfileError.InvalidFileContent, "");
			return LoggerType.Unknown;
		}

		private uint[] GetNodeValueUIntArray(string tagName, uint[] defaultValue, EnumViewType viewType)
		{
			XmlNodeList elementsByTagName = this.mDoc.GetElementsByTagName(tagName);
			if (elementsByTagName.Count < 1)
			{
				this.AddError(viewType, EnumProfileError.TagMissing, tagName);
				return defaultValue;
			}
			List<uint> list = new List<uint>();
			int num = 0;
			while (num < elementsByTagName[0].ChildNodes.Count && list.Count != defaultValue.Length)
			{
				XmlNode xmlNode = elementsByTagName[0].ChildNodes[num];
				if ("uint".Equals(xmlNode.Name))
				{
					uint item;
					if (!uint.TryParse(xmlNode.InnerText, out item))
					{
						this.AddError(viewType, EnumProfileError.InvalidValue, tagName);
						list.Add(defaultValue[num]);
					}
					else
					{
						list.Add(item);
					}
				}
				num++;
			}
			if (list.Count < defaultValue.Length)
			{
				this.AddError(viewType, EnumProfileError.ArrayValueMissing, tagName);
			}
			while (list.Count < defaultValue.Length)
			{
				list.Add(defaultValue[list.Count]);
			}
			return list.ToArray();
		}

		private double[] GetNodeValueDoubleArray(string tagName, double[] defaultValue, EnumViewType viewType)
		{
			XmlNodeList elementsByTagName = this.mDoc.GetElementsByTagName(tagName);
			if (elementsByTagName.Count < 1)
			{
				this.AddError(viewType, EnumProfileError.TagMissing, tagName);
				return defaultValue;
			}
			List<double> list = new List<double>();
			int num = 0;
			while (num < elementsByTagName[0].ChildNodes.Count && list.Count != defaultValue.Length)
			{
				XmlNode xmlNode = elementsByTagName[0].ChildNodes[num];
				if ("double".Equals(xmlNode.Name))
				{
					double item;
					if (!double.TryParse(xmlNode.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out item))
					{
						this.AddError(viewType, EnumProfileError.InvalidValue, tagName);
						list.Add(defaultValue[num]);
					}
					else
					{
						list.Add(item);
					}
				}
				num++;
			}
			if (list.Count < defaultValue.Length)
			{
				this.AddError(viewType, EnumProfileError.ArrayValueMissing, tagName);
			}
			while (list.Count < defaultValue.Length)
			{
				list.Add(defaultValue[list.Count]);
			}
			return list.ToArray();
		}

		private string[] GetNodeValueStringArray(string tagName, string[] defaultValue, EnumViewType viewType)
		{
			XmlNodeList elementsByTagName = this.mDoc.GetElementsByTagName(tagName);
			if (elementsByTagName.Count < 1)
			{
				this.AddError(viewType, EnumProfileError.TagMissing, tagName);
				return defaultValue;
			}
			List<string> list = new List<string>();
			int num = 0;
			while (num < elementsByTagName[0].ChildNodes.Count && list.Count != defaultValue.Length)
			{
				XmlNode xmlNode = elementsByTagName[0].ChildNodes[num];
				if ("string".Equals(xmlNode.Name))
				{
					list.Add(xmlNode.InnerText);
				}
				num++;
			}
			if (list.Count < defaultValue.Length)
			{
				this.AddError(viewType, EnumProfileError.ArrayValueMissing, tagName);
			}
			while (list.Count < defaultValue.Length)
			{
				list.Add(defaultValue[list.Count]);
			}
			return list.ToArray();
		}

		private void AddError(EnumViewType viewType, EnumProfileError error, string propertyName = "")
		{
			this.mErrors.Add(new FileConversionProfileError(viewType, error, propertyName));
		}

		public Image GetStateImage(EnumViewType viewType, LoggerType loggerType)
		{
			int stateImageIndex = this.GetStateImageIndex(viewType, loggerType);
			return MainImageList.Instance.ImageList.Images[stateImageIndex];
		}

		public int GetStateImageIndex(EnumViewType viewType, LoggerType loggerType)
		{
			if (this.HasErrors(viewType))
			{
				return MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.Error, false);
			}
			if (this.HasWarnings(viewType))
			{
				return MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.Warning, false);
			}
			if (this.HasInfos(viewType, loggerType))
			{
				return MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.Info, false);
			}
			return MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.NoImage, false);
		}

		public bool HasErrors(EnumViewType viewType)
		{
			return this.GetErrors(viewType).Any<FileConversionProfileError>();
		}

		private IEnumerable<FileConversionProfileError> GetErrors(EnumViewType viewType)
		{
			if (viewType == EnumViewType.Unknown)
			{
				viewType = EnumViewType.Common;
			}
			return (from t in this.mErrors
			where t.IsError && (t.ViewType == EnumViewType.Common || t.ViewType == viewType)
			select t).ToList<FileConversionProfileError>();
		}

		public bool HasWarnings(EnumViewType viewType)
		{
			return this.GetWarnings(viewType).Any<FileConversionProfileError>();
		}

		private IEnumerable<FileConversionProfileError> GetWarnings(EnumViewType viewType)
		{
			if (viewType == EnumViewType.Unknown)
			{
				viewType = EnumViewType.Common;
			}
			return (from t in this.mErrors
			where t.IsWarning && (t.ViewType == EnumViewType.Common || t.ViewType == viewType)
			select t).ToList<FileConversionProfileError>();
		}

		public bool HasInfos(EnumViewType viewType, LoggerType loggerType)
		{
			return this.GetInfos(viewType, loggerType).Any<FileConversionProfileError>();
		}

		private IEnumerable<FileConversionProfileError> GetInfos(EnumViewType viewType, LoggerType loggerType)
		{
			if (viewType == EnumViewType.Unknown)
			{
				viewType = EnumViewType.Common;
			}
			List<FileConversionProfileError> list = (from t in this.mErrors
			where t.IsInfo && (t.ViewType == EnumViewType.Common || t.ViewType == viewType)
			select t).ToList<FileConversionProfileError>();
			if (this.LoggerType != loggerType)
			{
				list.Add(new FileConversionProfileError(EnumViewType.Common, EnumProfileError.LoggerTypeMissmatch, Enum.GetName(typeof(LoggerType), this.LoggerType)));
			}
			return list;
		}

		public string GetErrorsText(EnumViewType viewType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<FileConversionProfileError> errors = this.GetErrors(viewType);
			foreach (FileConversionProfileError current in errors)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("- ");
				stringBuilder.Append(current.Text);
			}
			return stringBuilder.ToString();
		}

		public string GetWarningsText(EnumViewType viewType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<FileConversionProfileError> warnings = this.GetWarnings(viewType);
			foreach (FileConversionProfileError current in warnings)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("- ");
				stringBuilder.Append(current.Text);
			}
			return stringBuilder.ToString();
		}

		public string GetInfosText(EnumViewType viewType, LoggerType loggerType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<FileConversionProfileError> infos = this.GetInfos(viewType, loggerType);
			foreach (FileConversionProfileError current in infos)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("- ");
				stringBuilder.Append(current.Text);
			}
			return stringBuilder.ToString();
		}

		private IEnumerable<FileConversionProfileError> GetVolatileErrors()
		{
			return (from t in this.mErrors
			where t.IsVolatile
			select t).ToList<FileConversionProfileError>();
		}

		public void RemoveVolatileErrors()
		{
			IEnumerable<FileConversionProfileError> volatileErrors = this.GetVolatileErrors();
			foreach (FileConversionProfileError current in volatileErrors)
			{
				this.mErrors.Remove(current);
			}
		}
	}
}
