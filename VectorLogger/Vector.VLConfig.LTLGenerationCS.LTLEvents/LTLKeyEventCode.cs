using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLKeyEventCode : LTLGenericEventCode
	{
		private KeyEvent keyEvent;

		public LTLKeyEventCode(string eventFlagName, KeyEvent ev) : base(eventFlagName)
		{
			this.keyEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			base.PreEventCode = null;
			base.TriggerEvent = string.Format("SET ({0})", this.GetKeyVariableName());
			base.WhenCondition = null;
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			if (this.keyEvent.IsOnPanel.Value)
			{
				return string.Format("Event on panel key {0}", this.keyEvent.Number.Value);
			}
			return string.Format("Event on key {0}", this.keyEvent.Number.Value);
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = this.GetKeyVariableName();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string GetKeyVariableName()
		{
			if (this.keyEvent.IsOnPanel.Value)
			{
				return string.Format("PanelKey{0:D}", this.keyEvent.Number.Value);
			}
			if (this.keyEvent.IsCasKey)
			{
				GlobalSettings.UseAuxSwitchBox();
			}
			return string.Format("Key{0:D}", this.keyEvent.Number.Value);
		}
	}
}
