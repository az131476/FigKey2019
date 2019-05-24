using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionCombined : DpTriggerCondition
	{
		private readonly List<DpTriggerCondition> mChildConditionListStructured = new List<DpTriggerCondition>();

		private readonly CombinedEvent mEvent;

		private bool mSimulateGlBehaviour;

		private string mSimulateGlBehaviourCondition = string.Empty;

		public override string ChannelString
		{
			get
			{
				return string.Empty;
			}
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return string.Empty;
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				return string.Empty;
			}
		}

		public override string ORCondOfPointInTimeConditions
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				foreach (DpTriggerCondition current in this.mChildConditionListFlat)
				{
					if (current.IsPointInTime)
					{
						if (!flag)
						{
							stringBuilder.Append(" || ");
						}
						stringBuilder.Append(current.CombinedCondition);
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}
		}

		public override bool SimulateGlBehaviour
		{
			get
			{
				return this.mSimulateGlBehaviour;
			}
		}

		public override string SimulateGlBehaviourCondition
		{
			get
			{
				return this.mSimulateGlBehaviourCondition;
			}
		}

		public DpTriggerConditionCombined(uint triggerIdent, uint conditionIdent, Event conditionEvent, bool isTrigger) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as CombinedEvent);
			base.IsSingleCondition = false;
			base.IsCombinedConditionRoot = isTrigger;
			this.CreateChildConditions();
			this.CreateCombinedCondition();
			this.CreateSimulateGlBehaviour();
		}

		private void CreateChildConditions()
		{
			uint num = base.Ident * 100u;
			foreach (Event current in this.mEvent)
			{
				if (this.mEvent.ChildIsActive(current))
				{
					DpTriggerCondition dpTriggerCondition = DpTriggerCondition.Create(base.TriggerIdent, num, current, false);
					if (dpTriggerCondition != null)
					{
						dpTriggerCondition.IsSingleCondition = false;
						this.mChildConditionListStructured.Add(dpTriggerCondition);
						if (dpTriggerCondition.ChildConditionListFlat.Any<DpTriggerCondition>())
						{
							this.mChildConditionListFlat.AddRange(dpTriggerCondition.ChildConditionListFlat);
						}
						else
						{
							this.mChildConditionListFlat.Add(dpTriggerCondition);
						}
					}
					num += 1u;
				}
			}
		}

		private void CreateCombinedCondition()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			bool flag = true;
			foreach (DpTriggerCondition current in this.mChildConditionListStructured)
			{
				if (!flag)
				{
					stringBuilder.Append(this.mEvent.IsConjunction.Value ? " && " : " || ");
				}
				stringBuilder.Append(current.CombinedCondition);
				if (flag)
				{
					flag = false;
				}
			}
			stringBuilder.Append(")");
			base.CombinedCondition = stringBuilder.ToString();
		}

		private void CreateSimulateGlBehaviour()
		{
			List<DpTriggerCondition> list = new List<DpTriggerCondition>();
			if (this.mEvent.IsConjunction.Value)
			{
				List<DpTriggerCondition> list2 = (from child in this.mChildConditionListStructured
				where !child.IsSingleCondition
				select child).ToList<DpTriggerCondition>();
				foreach (DpTriggerCondition current in list2)
				{
					list.AddRange(from child in current.ChildConditionListFlat
					where child.IsPointInTime
					select child);
				}
			}
			this.mSimulateGlBehaviour = list.Any<DpTriggerCondition>();
			if (this.mSimulateGlBehaviour)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("(");
				bool flag = true;
				foreach (DpTriggerCondition current2 in list)
				{
					if (!flag)
					{
						stringBuilder.Append(" || ");
					}
					stringBuilder.Append(current2.CombinedCondition);
					if (flag)
					{
						flag = false;
					}
				}
				stringBuilder.Append(")");
				this.mSimulateGlBehaviourCondition = stringBuilder.ToString();
			}
		}
	}
}
