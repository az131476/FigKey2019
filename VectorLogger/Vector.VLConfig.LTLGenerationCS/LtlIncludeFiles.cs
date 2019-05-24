using System;
using System.IO;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LtlIncludeFiles : LTLGenericCodePart
	{
		private IncludeFileConfiguration includeFileConfiguration;

		private string configurationFolderPath;

		public LtlIncludeFiles(IncludeFileConfiguration includeFileConfiguration, string configurationFolderPath)
		{
			this.includeFileConfiguration = includeFileConfiguration;
			this.configurationFolderPath = configurationFolderPath;
		}

		public void GenerateIncludeFileCode()
		{
			if (this.includeFileConfiguration == null)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.IncludeFileError);
			}
			if (this.includeFileConfiguration.IncludeFiles.Count<IncludeFile>() > 0)
			{
				base.LtlCode = new StringBuilder();
				base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("INCLUDE FILES"));
				base.LtlCode.AppendLine();
				for (int i = 0; i < this.includeFileConfiguration.IncludeFiles.Count<IncludeFile>(); i++)
				{
					IncludeFile includeFile = this.includeFileConfiguration.IncludeFiles[i];
					string text;
					if (FileSystemServices.IsAbsolutePath(includeFile.FilePath.Value))
					{
						text = string.Format("{0}", FileSystemServices.GetAbsolutePath(includeFile.FilePath.Value, this.configurationFolderPath));
					}
					else
					{
						text = includeFile.FilePath.Value;
						text = text.TrimStart(new char[]
						{
							Path.DirectorySeparatorChar
						});
					}
					text = text.Replace("\\\\", "\\\\\\\\");
					base.LtlCode.AppendFormat("#include(\"{0}\" ", text);
					int num = includeFile.HighestInParameterIndex + 1;
					if (includeFile.Parameters.Count < num)
					{
						throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.IncludeFileError);
					}
					for (int j = 0; j < num; j++)
					{
						base.LtlCode.AppendFormat(" \"{0}\"", includeFile.Parameters[j].Value);
					}
					base.LtlCode.AppendLine(")");
				}
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine();
			}
		}
	}
}
