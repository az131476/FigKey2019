using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vector.VLConfig.GUI.IncludeFilesPage
{
	internal class IncludeFileParser
	{
		private const int MinParamNumber = 1;

		private const int MaxParamNumber = 9;

		private readonly string mAbsoluteIncludeFilePath;

		private readonly string[] mParameterNumberTokens;

		private bool mIsFirstNonEmptyLine = true;

		private bool mIsInsideHeader;

		private bool mHeaderScanCompleted;

		private bool mIsInsideFileDescription;

		private bool mIsInsideParameterDescription;

		private readonly List<IncludeFileParameter> mOutParameters = new List<IncludeFileParameter>();

		private readonly StringBuilder mMultilineText = new StringBuilder();

		public bool HasFileNotFoundError
		{
			get;
			private set;
		}

		public string FileDescription
		{
			get;
			private set;
		}

		public List<IncludeFileParameter> Parameters
		{
			get;
			private set;
		}

		public IncludeFileParser(string absoluteIncludeFilePath)
		{
			this.mAbsoluteIncludeFilePath = absoluteIncludeFilePath;
			this.HasFileNotFoundError = !File.Exists(this.mAbsoluteIncludeFilePath);
			this.FileDescription = string.Empty;
			this.Parameters = new List<IncludeFileParameter>();
			List<string> list = new List<string>();
			for (int i = 0; i <= 9; i++)
			{
				list.Add("%" + i + "%");
			}
			this.mParameterNumberTokens = list.ToArray();
			if (!this.HasFileNotFoundError)
			{
				this.Parse();
			}
		}

		private void Parse()
		{
			this.mIsFirstNonEmptyLine = true;
			this.mIsInsideHeader = false;
			this.mHeaderScanCompleted = false;
			this.mIsInsideFileDescription = false;
			this.mIsInsideParameterDescription = false;
			this.mMultilineText.Clear();
			using (StreamReader streamReader = new StreamReader(this.mAbsoluteIncludeFilePath, Encoding.Default))
			{
				while (!streamReader.EndOfStream && !this.mHeaderScanCompleted)
				{
					string line;
					if (this.GetNextNonEmptyLine(streamReader, out line) && !this.TryParseHeaderTag(line) && !this.TryParseInParameterDefinition(line) && !this.TryParseOutParameterDefinition(line) && !this.TryParseInstanceParameterDefinition(line) && !this.TryParseParameterDescription(line))
					{
						this.TryParseFileDescription(line);
					}
				}
			}
			using (StreamReader streamReader2 = new StreamReader(this.mAbsoluteIncludeFilePath, Encoding.Default))
			{
				while (!streamReader2.EndOfStream)
				{
					string line2;
					if (this.GetNextNonEmptyLine(streamReader2, out line2))
					{
						this.ScanForUndefinedParameters(line2);
					}
				}
			}
			this.Parameters.Sort((IncludeFileParameter ifp1, IncludeFileParameter ifp2) => ifp1.ParamNumber.CompareTo(ifp2.ParamNumber));
			int num = this.Parameters.Any<IncludeFileParameter>() ? this.Parameters.Last<IncludeFileParameter>().ParamNumber : 0;
			int paramNumber;
			for (paramNumber = 1; paramNumber <= num; paramNumber++)
			{
				if (this.Parameters.All((IncludeFileParameter param) => param.ParamNumber != paramNumber))
				{
					this.Parameters.Add(IncludeFileParameter.CreateDummyParameter(paramNumber));
				}
			}
			this.Parameters.Sort((IncludeFileParameter ifp1, IncludeFileParameter ifp2) => ifp1.ParamNumber.CompareTo(ifp2.ParamNumber));
			foreach (IncludeFileParameter current in this.mOutParameters)
			{
				num++;
				this.Parameters.Add(new IncludeFileParameter(current.Type, num, current.LtlName, current.Name, current.Description));
			}
			if (!this.Parameters.Any<IncludeFileParameter>())
			{
				this.Parameters.Add(IncludeFileParameter.CreateDummyParameter(1));
			}
		}

		private bool GetNextNonEmptyLine(StreamReader incFile, out string line)
		{
			line = string.Empty;
			while (!incFile.EndOfStream)
			{
				line = incFile.ReadLine();
				if (line != null)
				{
					if (this.mIsInsideFileDescription || this.mIsInsideParameterDescription)
					{
						return true;
					}
					line = line.Trim();
					if (!string.IsNullOrEmpty(line))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool TryParseHeaderTag(string line)
		{
			if (this.mIsFirstNonEmptyLine)
			{
				this.mIsFirstNonEmptyLine = false;
				if (line.Trim().StartsWith("{", StringComparison.OrdinalIgnoreCase))
				{
					this.mIsInsideHeader = true;
					return true;
				}
				this.mHeaderScanCompleted = true;
				return true;
			}
			else
			{
				if (this.mIsInsideHeader && line.Trim().StartsWith("}", StringComparison.OrdinalIgnoreCase))
				{
					this.FinishMultilineText();
					this.mIsInsideHeader = false;
					this.mHeaderScanCompleted = true;
					return true;
				}
				return false;
			}
		}

		private bool TryParseFileDescription(string line)
		{
			if (!this.mIsInsideHeader)
			{
				return false;
			}
			if (this.mIsInsideFileDescription)
			{
				if (line.Trim().StartsWith("purpose end", StringComparison.OrdinalIgnoreCase))
				{
					this.FinishFileDescription();
					return true;
				}
				if (this.mMultilineText.Length > 0)
				{
					this.mMultilineText.Append(Environment.NewLine);
				}
				this.mMultilineText.Append(line);
				return true;
			}
			else
			{
				if (line.Trim().StartsWith("purpose", StringComparison.OrdinalIgnoreCase))
				{
					this.FinishMultilineText();
					this.mIsInsideFileDescription = true;
					this.mMultilineText.Clear();
					return true;
				}
				return false;
			}
		}

		private bool TryParseInParameterDefinition(string line)
		{
			if (!this.mIsInsideHeader)
			{
				return false;
			}
			if (!line.Trim().StartsWith("parameter", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			string[] array = line.Trim().Split(new char[]
			{
				':'
			});
			if (array.Length < 1)
			{
				return false;
			}
			string[] array2 = (from part in array[0].Trim().Split(new char[]
			{
				' '
			})
			where !string.IsNullOrEmpty(part)
			select part).ToArray<string>();
			if (array2.Length < 2)
			{
				return false;
			}
			string s = array2[1].Trim().Trim(new char[]
			{
				'%'
			});
			int num;
			if (!int.TryParse(s, out num) || num < 1 || num > 9)
			{
				return false;
			}
			this.FinishMultilineText();
			if (array.Length < 2)
			{
				this.AddParameter(IncludeFileParameter.CreateInParam(num, "", ""));
				return true;
			}
			string paramName = array[1].Trim();
			if (array.Length < 3)
			{
				this.AddParameter(IncludeFileParameter.CreateInParam(num, paramName, ""));
				return true;
			}
			List<string> list = new List<string>();
			for (int i = 2; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
			string description = string.Join(":", list.ToArray()).Trim();
			this.AddParameter(IncludeFileParameter.CreateInParam(num, paramName, description));
			return true;
		}

		private bool TryParseOutParameterDefinition(string line)
		{
			if (!this.mIsInsideHeader)
			{
				return false;
			}
			if (!line.Trim().StartsWith("return", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			string[] array = line.Trim().Split(new char[]
			{
				':'
			});
			if (array.Length < 1)
			{
				return false;
			}
			string[] array2 = (from part in array[0].Trim().Split(new char[]
			{
				' '
			})
			where !string.IsNullOrEmpty(part)
			select part).ToArray<string>();
			if (array2.Length < 2)
			{
				return false;
			}
			string ltlName = array2[1].Trim();
			this.FinishMultilineText();
			if (array.Length < 2)
			{
				this.AddParameter(IncludeFileParameter.CreateOutParam(ltlName, "", ""));
				return true;
			}
			string paramName = array[1].Trim();
			if (array.Length < 3)
			{
				this.AddParameter(IncludeFileParameter.CreateOutParam(ltlName, paramName, ""));
				return true;
			}
			List<string> list = new List<string>();
			for (int i = 2; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
			string description = string.Join(":", list.ToArray()).Trim();
			this.AddParameter(IncludeFileParameter.CreateOutParam(ltlName, paramName, description));
			return true;
		}

		private bool TryParseInstanceParameterDefinition(string line)
		{
			if (!this.mIsInsideHeader)
			{
				return false;
			}
			if (!line.Trim().StartsWith("instance", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			string[] array = line.Trim().Split(new char[]
			{
				':'
			});
			if (array.Length < 1)
			{
				return false;
			}
			string[] array2 = (from part in array[0].Trim().Split(new char[]
			{
				' '
			})
			where !string.IsNullOrEmpty(part)
			select part).ToArray<string>();
			if (array2.Length < 2)
			{
				return false;
			}
			string s = array2[1].Trim().Trim(new char[]
			{
				'%'
			});
			int num;
			if (!int.TryParse(s, out num) || num < 1 || num > 9)
			{
				return false;
			}
			this.FinishMultilineText();
			if (array.Length < 2)
			{
				this.AddParameter(IncludeFileParameter.CreateInstanceParam(num, "", ""));
				return true;
			}
			string paramName = array[1].Trim();
			if (array.Length < 3)
			{
				this.AddParameter(IncludeFileParameter.CreateInstanceParam(num, paramName, ""));
				return true;
			}
			List<string> list = new List<string>();
			for (int i = 2; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
			string description = string.Join(":", list.ToArray()).Trim();
			this.AddParameter(IncludeFileParameter.CreateInstanceParam(num, paramName, description));
			return true;
		}

		private bool TryParseParameterDescription(string line)
		{
			if (!this.mIsInsideHeader)
			{
				return false;
			}
			if (this.mIsInsideParameterDescription)
			{
				if (line.Trim().StartsWith("description end", StringComparison.OrdinalIgnoreCase))
				{
					this.FinishParameterDescription();
					return true;
				}
				if (this.mMultilineText.Length > 0)
				{
					this.mMultilineText.Append(Environment.NewLine);
				}
				this.mMultilineText.Append(line);
				return true;
			}
			else
			{
				if (line.Trim().StartsWith("description", StringComparison.OrdinalIgnoreCase))
				{
					this.FinishMultilineText();
					this.mIsInsideParameterDescription = true;
					this.mMultilineText.Clear();
					return true;
				}
				return false;
			}
		}

		private void ScanForUndefinedParameters(string line)
		{
			int paramNumber;
			for (paramNumber = 1; paramNumber <= 9; paramNumber++)
			{
				if (line.Contains(this.mParameterNumberTokens[paramNumber]))
				{
					if (this.Parameters.All((IncludeFileParameter param) => param.ParamNumber != paramNumber))
					{
						this.AddParameter(IncludeFileParameter.CreateInParam(paramNumber, "", ""));
					}
				}
			}
		}

		private void FinishMultilineText()
		{
			this.FinishFileDescription();
			this.FinishParameterDescription();
		}

		private void FinishFileDescription()
		{
			if (!this.mIsInsideFileDescription)
			{
				return;
			}
			this.mIsInsideFileDescription = false;
			this.FileDescription = this.mMultilineText.ToString();
			this.mMultilineText.Clear();
		}

		private void FinishParameterDescription()
		{
			if (!this.mIsInsideParameterDescription)
			{
				return;
			}
			this.mIsInsideParameterDescription = false;
			if (this.Parameters.Any<IncludeFileParameter>())
			{
				this.Parameters.Last<IncludeFileParameter>().ExtendedDescription = this.mMultilineText.ToString();
			}
			this.mMultilineText.Clear();
		}

		private void AddParameter(IncludeFileParameter param)
		{
			if (param == null)
			{
				return;
			}
			if (param.Type == IncludeFileParameter.ParamType.Out)
			{
				this.mOutParameters.Add(param);
				return;
			}
			this.Parameters.Add(param);
		}
	}
}
