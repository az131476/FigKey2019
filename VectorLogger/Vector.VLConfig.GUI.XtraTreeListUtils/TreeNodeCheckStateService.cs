using DevExpress.Utils.Drawing;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.XtraTreeListUtils
{
	internal class TreeNodeCheckStateService : IDisposable
	{
		private readonly TreeList mTreeList;

		private readonly bool mOriginalAllowIndeterminateCheckState;

		public EnumCheckStateMode CheckStateMode
		{
			get;
			private set;
		}

		public bool UseSubnodeDeactivation
		{
			get;
			set;
		}

		public TreeNodeCheckStateService(TreeList treeList, EnumCheckStateMode checkStateMode = EnumCheckStateMode.PseudoTriState)
		{
			this.mTreeList = treeList;
			this.CheckStateMode = checkStateMode;
			this.UseSubnodeDeactivation = false;
			if (this.mTreeList != null)
			{
				this.mOriginalAllowIndeterminateCheckState = this.mTreeList.OptionsBehavior.AllowIndeterminateCheckState;
				this.mTreeList.OptionsBehavior.AllowIndeterminateCheckState = (this.CheckStateMode != EnumCheckStateMode.DualState);
			}
			this.RegisterEventHandlers();
		}

		private void RegisterEventHandlers()
		{
			if (this.mTreeList == null)
			{
				return;
			}
			this.mTreeList.CustomDrawNodeCheckBox += new CustomDrawNodeCheckBoxEventHandler(this.TreeList_CustomDrawNodeCheckBox);
			this.mTreeList.BeforeCheckNode += new CheckNodeEventHandler(this.TreeList_BeforeCheckNode);
			this.mTreeList.GetNodeDisplayValue += new GetNodeDisplayValueEventHandler(this.TreeList_GetNodeDisplayValue);
		}

		private void UnregisterEventHandlers()
		{
			if (this.mTreeList == null)
			{
				return;
			}
			this.mTreeList.CustomDrawNodeCheckBox -= new CustomDrawNodeCheckBoxEventHandler(this.TreeList_CustomDrawNodeCheckBox);
			this.mTreeList.BeforeCheckNode -= new CheckNodeEventHandler(this.TreeList_BeforeCheckNode);
			this.mTreeList.GetNodeDisplayValue -= new GetNodeDisplayValueEventHandler(this.TreeList_GetNodeDisplayValue);
		}

		public void Dispose()
		{
			if (this.mTreeList != null)
			{
				this.mTreeList.OptionsBehavior.AllowIndeterminateCheckState = this.mOriginalAllowIndeterminateCheckState;
			}
			this.UnregisterEventHandlers();
		}

		private void TreeList_BeforeCheckNode(object sender, CheckNodeEventArgs e)
		{
			e.CanCheck = (this.CanCheckNode(e.Node) && (!this.UseSubnodeDeactivation || !this.IsInactive(e.Node)));
			if (!e.CanCheck)
			{
				return;
			}
			this.SetCheckStateInDataRecord(e.Node, e.State);
		}

		private void TreeList_CustomDrawNodeCheckBox(object sender, CustomDrawNodeCheckBoxEventArgs e)
		{
			if (!this.CanCheckNode(e.Node))
			{
				e.ObjectArgs.State = ObjectState.Disabled;
				e.Handled = true;
				return;
			}
			if (this.UseSubnodeDeactivation && this.IsInactive(e.Node))
			{
				e.ObjectArgs.State = ObjectState.Disabled;
			}
		}

		private void TreeList_GetNodeDisplayValue(object sender, GetNodeDisplayValueEventArgs e)
		{
			if (e.Column.VisibleIndex != 0)
			{
				return;
			}
			if (!this.CanCheckNode(e.Node))
			{
				return;
			}
			e.Node.CheckState = this.GetCheckStateFromDataRecord(e.Node);
		}

		private ICheckStateDataRecord GetDataRecord(TreeListNode node)
		{
			if (node == null || this.mTreeList == null)
			{
				return null;
			}
			return this.mTreeList.GetDataRecordByNode(node) as ICheckStateDataRecord;
		}

		private bool CanCheckNode(TreeListNode node)
		{
			ICheckStateDataRecord dataRecord = this.GetDataRecord(node);
			return dataRecord != null && dataRecord.CanCheck;
		}

		private CheckState GetCheckStateFromDataRecord(TreeListNode node)
		{
			if (!this.CanCheckNode(node))
			{
				return CheckState.Checked;
			}
			ICheckStateDataRecord dataRecord = this.GetDataRecord(node);
			if (dataRecord.CheckState == CheckState.Unchecked)
			{
				return CheckState.Unchecked;
			}
			switch (this.CheckStateMode)
			{
			case EnumCheckStateMode.DualState:
				return CheckState.Checked;
			case EnumCheckStateMode.PseudoTriState:
				if (!this.ChildNodesAreChecked(node))
				{
					return CheckState.Indeterminate;
				}
				return CheckState.Checked;
			case EnumCheckStateMode.TriState:
				return dataRecord.CheckState;
			default:
				return CheckState.Checked;
			}
		}

		private bool ChildNodesAreChecked(TreeListNode parent)
		{
			return parent == null || (this.CheckStateMode == EnumCheckStateMode.DualState || this.CheckStateMode == EnumCheckStateMode.TriState) || parent.Nodes.Cast<TreeListNode>().All((TreeListNode child) => this.GetCheckStateFromDataRecord(child) == CheckState.Checked);
		}

		private void SetCheckStateInDataRecord(TreeListNode node, CheckState state)
		{
			if (!this.CanCheckNode(node))
			{
				return;
			}
			ICheckStateDataRecord dataRecord = this.GetDataRecord(node);
			switch (this.CheckStateMode)
			{
			case EnumCheckStateMode.DualState:
				dataRecord.CheckState = state;
				return;
			case EnumCheckStateMode.PseudoTriState:
				dataRecord.CheckState = ((state == CheckState.Indeterminate) ? CheckState.Checked : CheckState.Unchecked);
				return;
			case EnumCheckStateMode.TriState:
				dataRecord.CheckState = state;
				return;
			default:
				return;
			}
		}

		private bool IsInactive(TreeListNode node)
		{
			return node != null && node.ParentNode != null && (this.GetCheckStateFromDataRecord(node.ParentNode) == CheckState.Unchecked || this.IsInactive(node.ParentNode));
		}
	}
}
