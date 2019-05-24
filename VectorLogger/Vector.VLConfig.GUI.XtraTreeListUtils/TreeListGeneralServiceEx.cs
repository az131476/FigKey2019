using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using System;
using System.Drawing;
using Vector.UtilityFunctions.XtraTreeList;

namespace Vector.VLConfig.GUI.XtraTreeListUtils
{
	internal class TreeListGeneralServiceEx : GeneralService
	{
		public delegate bool IsInactiveButEditable<in T>(T dataRecord, TreeListColumn column) where T : class;

		private readonly TreeList mTreeList;

		public TreeListGeneralServiceEx(TreeList treeList) : base(treeList)
		{
			this.mTreeList = treeList;
		}

		public void CustomDrawNodeCell<T>(CustomDrawNodeCellEventArgs e, GeneralService.IsReadOnlyAtAll<T> callbackIsReadOnlyAtAll, GeneralService.IsReadOnlyByCellContent<T> callbackIsReadOnlyByCellContent, TreeListGeneralServiceEx.IsInactiveButEditable<T> callbackIsInactiveButEditable) where T : class
		{
			if (e.Node == null)
			{
				return;
			}
			if (e.Column == null)
			{
				return;
			}
			T t = this.mTreeList.GetDataRecordByNode(e.Node) as T;
			if (t == null)
			{
				return;
			}
			bool isReadOnlyAtAll = callbackIsReadOnlyAtAll(t, e.Column);
			bool isReadOnlyByCellContent = callbackIsReadOnlyByCellContent(t, e.Column);
			bool isInactiveButEditable = callbackIsInactiveButEditable(t, e.Column);
			this.CustomDrawCellReadOnlyInactiveState(e, isReadOnlyAtAll, isReadOnlyByCellContent, isInactiveButEditable);
		}

		public void CustomDrawCellReadOnlyInactiveState(CustomDrawNodeCellEventArgs e, bool isReadOnlyAtAll, bool isReadOnlyByCellContent, bool isInactiveButEditable)
		{
			if (e.Column == null)
			{
				return;
			}
			if (e.Node == null)
			{
				return;
			}
			if (e.EditViewInfo == null)
			{
				return;
			}
			if (e.Appearance == null)
			{
				return;
			}
			if (isReadOnlyAtAll || isReadOnlyByCellContent)
			{
				RepositoryItem item = e.EditViewInfo.Item;
				if (item != null)
				{
					if (item is RepositoryItemComboBox)
					{
						ButtonEditViewInfo buttonEditViewInfo = e.EditViewInfo as ButtonEditViewInfo;
						if (buttonEditViewInfo != null && buttonEditViewInfo.RightButtons.Count > 0)
						{
							buttonEditViewInfo.RightButtons[0].State = ObjectState.Disabled;
						}
					}
					if (item is RepositoryItemTextEdit)
					{
						TextEditViewInfo textEditViewInfo = e.EditViewInfo as TextEditViewInfo;
						if (textEditViewInfo != null)
						{
							textEditViewInfo.State = ObjectState.Disabled;
						}
					}
					if (item is RepositoryItemButtonEdit)
					{
						ButtonEditViewInfo buttonEditViewInfo2 = e.EditViewInfo as ButtonEditViewInfo;
						if (buttonEditViewInfo2 != null && buttonEditViewInfo2.RightButtons.Count > 0)
						{
							buttonEditViewInfo2.RightButtons[0].State = ObjectState.Disabled;
						}
					}
					else if (item is RepositoryItemTrackBar)
					{
						TrackBarViewInfo trackBarViewInfo = e.EditViewInfo as TrackBarViewInfo;
						if (trackBarViewInfo != null)
						{
							trackBarViewInfo.State = ObjectState.Disabled;
						}
					}
					else if (item is RepositoryItemCheckEdit)
					{
						CheckEditViewInfo checkEditViewInfo = e.EditViewInfo as CheckEditViewInfo;
						if (checkEditViewInfo != null)
						{
							checkEditViewInfo.AllowOverridedState = true;
							checkEditViewInfo.OverridedState = ObjectState.Disabled;
							checkEditViewInfo.CalcViewInfo(e.Graphics);
						}
					}
				}
				else
				{
					Type columnType = e.Column.ColumnType;
					if (columnType == typeof(bool))
					{
						CheckEditViewInfo checkEditViewInfo2 = e.EditViewInfo as CheckEditViewInfo;
						if (checkEditViewInfo2 != null)
						{
							checkEditViewInfo2.AllowOverridedState = true;
							checkEditViewInfo2.OverridedState = ObjectState.Disabled;
							checkEditViewInfo2.CalcViewInfo(e.Graphics);
						}
					}
					else
					{
						TextEditViewInfo textEditViewInfo2 = e.EditViewInfo as TextEditViewInfo;
						if (textEditViewInfo2 != null)
						{
							textEditViewInfo2.State = ObjectState.Disabled;
						}
					}
				}
			}
			if (!isReadOnlyByCellContent && !isInactiveButEditable)
			{
				return;
			}
			bool flag = this.mTreeList.Selection.Contains(e.Node) || this.mTreeList.FocusedNode == e.Node;
			bool flag2 = this.mTreeList.Focused || this.mTreeList.ActiveEditor != null;
			bool flag3 = this.mTreeList.FocusedColumn != e.Column || this.mTreeList.FocusedNode != e.Node;
			if (!flag)
			{
				e.Appearance.ForeColor = SystemColors.GrayText;
				return;
			}
			if (!flag2)
			{
				e.Appearance.ForeColor = SystemColors.ControlDarkDark;
				return;
			}
			if (flag3)
			{
				return;
			}
			e.Appearance.ForeColor = SystemColors.GrayText;
		}
	}
}
