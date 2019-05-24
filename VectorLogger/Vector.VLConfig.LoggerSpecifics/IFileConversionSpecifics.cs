using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IFileConversionSpecifics
	{
		IEnumerable<FileConversionDestFormat> DestFormats
		{
			get;
		}

		IList<FileConversionFilenameFormat> FilenameFormats
		{
			get;
		}

		FileConversionDestFormat DefaultDestFormat
		{
			get;
		}

		string RawFileFormat
		{
			get;
		}

		bool HasAdvancedSettings
		{
			get;
		}

		bool HasChannelMapping
		{
			get;
		}

		bool HasSplittingOptions
		{
			get;
		}

		bool HasSelectableLogFiles
		{
			get;
		}

		bool HasExportDatabases
		{
			get;
		}

		uint NumberOfCanMappingChannels
		{
			get;
		}

		uint NumberOfLinMappingChannels
		{
			get;
		}

		uint NumberOfFlexRayMappingChannels
		{
			get;
		}

		bool IsNavigatorViewSupported
		{
			get;
		}

		bool IsQuickViewSupported
		{
			get;
		}
	}
}
