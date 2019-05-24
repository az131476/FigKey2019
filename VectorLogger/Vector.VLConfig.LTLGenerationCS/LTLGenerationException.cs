using System;
using System.Text;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLGenerationException : Exception
	{
		public LTLGenerator.LTLGenerationResult Result
		{
			get;
			private set;
		}

		public string AdditionalInfo
		{
			get;
			private set;
		}

		public LTLGenerationException()
		{
			this.Result = LTLGenerator.LTLGenerationResult.Error;
			this.AdditionalInfo = string.Empty;
		}

		public LTLGenerationException(string message) : base(message)
		{
			this.Result = LTLGenerator.LTLGenerationResult.Error;
			this.AdditionalInfo = string.Empty;
		}

		public LTLGenerationException(string message, Exception inner) : base(message, inner)
		{
			this.Result = LTLGenerator.LTLGenerationResult.Error;
			this.AdditionalInfo = string.Empty;
		}

		public LTLGenerationException(LTLGenerator.LTLGenerationResult result)
		{
			this.Result = result;
			this.AdditionalInfo = string.Empty;
		}

		public LTLGenerationException(LTLGenerator.LTLGenerationResult result, string addInfo)
		{
			this.Result = result;
			this.AdditionalInfo = addInfo;
		}

		public void InsertAdditionalInfo(string addInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(addInfo);
			this.AdditionalInfo.Insert(0, stringBuilder.ToString());
		}
	}
}
