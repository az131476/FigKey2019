using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vector.VLConfig.GUI.XtraTreeListUtils
{
	internal class TreeListStateService
	{
		private readonly TreeList mTreeList;

		private readonly Dictionary<object, TreeListStateEntry> mTreeListStates = new Dictionary<object, TreeListStateEntry>();

		public bool ExpandNewItems
		{
			get;
			set;
		}

		public TreeListStateService(TreeList treeList, bool expandNewItems = false)
		{
			this.mTreeList = treeList;
			this.ExpandNewItems = expandNewItems;
		}

		private bool TryGetKeyFieldValue(TreeListNode node, out object keyFieldValue)
		{
			keyFieldValue = null;
			object dataRecordByNode = this.mTreeList.GetDataRecordByNode(node);
			if (dataRecordByNode == null)
			{
				return false;
			}
			Type type = dataRecordByNode.GetType();
			PropertyInfo property = type.GetProperty(this.mTreeList.KeyFieldName);
			if (property != null)
			{
				keyFieldValue = property.GetValue(dataRecordByNode, null);
				return keyFieldValue != null;
			}
			FieldInfo field = type.GetField(this.mTreeList.KeyFieldName);
			if (field != null)
			{
				keyFieldValue = field.GetValue(dataRecordByNode);
				return keyFieldValue != null;
			}
			return false;
		}

		public void StoreExpandState()
		{
			foreach (TreeListStateEntry current in this.mTreeListStates.Values)
			{
				current.PrepareForUpdate();
			}
			this.StoreExpandState(this.mTreeList.Nodes);
			List<object> list = (from t in this.mTreeListStates.Keys
			where !this.mTreeListStates[t].Updated
			select t).ToList<object>();
			foreach (object current2 in list)
			{
				this.mTreeListStates.Remove(current2);
			}
		}

		private void StoreExpandState(TreeListNodes nodes)
		{
			foreach (TreeListNode treeListNode in nodes)
			{
				object key;
				if (this.TryGetKeyFieldValue(treeListNode, out key))
				{
					if (!this.mTreeListStates.ContainsKey(key))
					{
						this.mTreeListStates.Add(key, new TreeListStateEntry(this.ExpandNewItems));
					}
					this.mTreeListStates[key].Expanded = treeListNode.Expanded;
					this.StoreExpandState(treeListNode.Nodes);
				}
			}
		}

		public void RestoreExpandState()
		{
			this.RestoreExpandState(this.mTreeList.Nodes);
		}

		private void RestoreExpandState(TreeListNodes nodes)
		{
			foreach (TreeListNode treeListNode in nodes)
			{
				object key;
				if (this.TryGetKeyFieldValue(treeListNode, out key))
				{
					if (!this.mTreeListStates.ContainsKey(key))
					{
						this.mTreeListStates.Add(key, new TreeListStateEntry(this.ExpandNewItems));
					}
					treeListNode.Expanded = this.mTreeListStates[key].Expanded;
					this.RestoreExpandState(treeListNode.Nodes);
				}
			}
		}
	}
}
