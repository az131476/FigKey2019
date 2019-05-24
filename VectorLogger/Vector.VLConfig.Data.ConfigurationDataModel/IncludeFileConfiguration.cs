using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "IncludeFileConfiguration")]
	public class IncludeFileConfiguration : Feature, IFeatureReferencedFiles
	{
		[DataMember(Name = "IncludeFileConfigurationFileList")]
		private List<IncludeFile> fileList;

		private List<IFile> referencedFiles;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return this;
			}
		}

		public override IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureTransmitMessages TransmitMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return null;
			}
		}

		IList<IFile> IFeatureReferencedFiles.ReferencedFiles
		{
			get
			{
				if (this.referencedFiles == null)
				{
					this.referencedFiles = new List<IFile>();
				}
				this.referencedFiles.Clear();
				foreach (IncludeFile current in this.fileList)
				{
					this.referencedFiles.Add(current);
				}
				return this.referencedFiles;
			}
		}

		public ReadOnlyCollection<IncludeFile> IncludeFiles
		{
			get
			{
				return new ReadOnlyCollection<IncludeFile>(this.fileList);
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is IncludeFileConfiguration)
			{
				updateService.Notify<IncludeFileConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<IncludeFileConfiguration>(this);
		}

		public IncludeFileConfiguration()
		{
			this.fileList = new List<IncludeFile>();
		}

		public void AddIncludeFile(IncludeFile includeFile)
		{
			this.fileList.Add(includeFile);
		}

		public bool InsertIncludeFile(int idx, IncludeFile includeFile)
		{
			if (idx >= 0 && idx < this.fileList.Count)
			{
				this.fileList.Insert(idx, includeFile);
				return true;
			}
			if (idx == this.fileList.Count)
			{
				this.fileList.Add(includeFile);
				return true;
			}
			return false;
		}

		public bool RemoveIncludeFile(IncludeFile includeFile)
		{
			if (this.fileList.Contains(includeFile))
			{
				this.fileList.Remove(includeFile);
				return true;
			}
			return false;
		}

		public void ClearIncludeFiles()
		{
			this.fileList.Clear();
		}
	}
}
