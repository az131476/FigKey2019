using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Localization;
using DevExpress.XtraTreeList.Menu;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.ViewInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.UtilityFunctions.XtraTreeList;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.GUI.IncludeFilesPage;
using Vector.VLConfig.GUI.XtraTreeListUtils;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.TriggersPage
{
	internal class TriggerTree : UserControl
	{
		private enum EnumRowMoveAction
		{
			Top,
			Up,
			Down,
			Bottom
		}

		public enum EnumDropPosition
		{
			None = -1,
			AsSiblingBefore,
			AsSiblingAfter
		}

		public enum AddAction
		{
			Condition,
			Trigger,
			Marker
		}

		private const int cDropLineHeight = 2;

		private const int cDropLineMarginLeft = 3;

		private const int cDropLineTriangleHeight = 4;

		private const int cDropLineMarginRight = 5;

		private const Keys cTreeListKeyUp = Keys.Up;

		private const Keys cTreeListKeyDown = Keys.Down;

		private const Keys cTreeListKeyHome = Keys.Home;

		private const Keys cTreeListKeyEnd = Keys.End;

		private const int cMaxTreeLevels = 2;

		private readonly GUIElementManager_ControlGridTree mGuiElementManager;

		private readonly CustomErrorProvider mCustomErrorProvider;

		private readonly PageValidator mPageValidator;

		private DisplayMode mDisplayMode;

		private TriggerMode mCurrentTriggerMode = TriggerMode.OnOff;

		private TriggerConfiguration mTriggerConfiguration;

		private readonly BindingList<EventPresenter> mBindingList = new BindingList<EventPresenter>();

		private readonly Dictionary<ulong, EventPresenter> mPresenterMap = new Dictionary<ulong, EventPresenter>();

		private readonly TreeListGeneralServiceEx mTreeListGeneralServiceEx;

		private readonly KeyboardNavigationService mTreeListNavigationService;

		private readonly TreeNodeCheckStateService mTreeNodeCheckBoxService;

		private TreeListHitInfo mDragHitInfo;

		private readonly Color mDropLineColor = SystemColors.WindowText;

		private Point mDropLineLeft;

		private Point mDropLineRight;

		private bool mDropLineRefreshRequested;

		private readonly Color mLoggingOnColor = Color.FromArgb(240, 255, 240);

		private readonly Color mLoggingOffColor = Color.FromArgb(255, 240, 240);

		private IContainer components;

		private TreeList mTreeList;

		private TreeListColumn mColTriggerType;

		private TreeListColumn mColEvent;

		private TreeListColumn mColChannel;

		private TreeListColumn mColCondition;

		private TreeListColumn mColAction;

		private TreeListColumn mColComment;

		private TreeListColumn mColName;

		private Button mButtonMoveLast;

		private Button mButtonMoveDown;

		private Button mButtonMoveUp;

		private Button mButtonMoveFirst;

		private RepositoryItemComboBox mRepositoryItemComboBoxTriggerEffect;

		private RepositoryItemCheckEdit mRepositoryItemCheckEditIsActive;

		private RepositoryItemComboBox mRepositoryItemComboBoxChannel;

		private RepositoryItemComboBox mRepositoryItemComboBoxAction;

		private RepositoryItemMemoExEdit mRepositoryItemMemoExEditComment;

		private RepositoryItemTextEdit mRepositoryItemTextEditTriggerName;

		private RepositoryItemTextEdit mRepositoryItemTextEditDummy;

		private RepositoryItemButtonEdit mRepositoryItemButtonEditCondition;

		private ErrorProvider mErrorProviderLocalModel;

		private ErrorProvider mErrorProviderFormat;

		private ErrorProvider mErrorProviderGlobalModel;

		private XtraToolTipController mXtraToolTipController;

		private ContextMenuStrip mContextMenuStrip;

		private ToolStripMenuItem mToolStripMenuItemMoveTop;

		private ToolStripMenuItem mToolStripMenuItemMoveUp;

		private ToolStripMenuItem mToolStripMenuItemMoveDown;

		private ToolStripMenuItem mToolStripMenuItemMoveBottom;

		private ToolStripSeparator mToolStripSeparator2;

		private ToolStripMenuItem mToolStripMenuItemRemove;

		private ToolStripMenuItem mToolStripMenuItemResetToDefault;

		private ToolStripSeparator mToolStripSeparator1;

		private ToolStripMenuItem mToolStripMenuItemCut;

		private ToolStripMenuItem mToolStripMenuItemCopy;

		private ToolStripMenuItem mToolStripMenuItemPaste;

		private ToolStripSeparator mToolStripSeparator3;

		private ToolStripMenuItem mToolStripMenuItemExpandAll;

		private ToolStripMenuItem mToolStripMenuItemCollapseAll;

		public event EventHandler SelectionChanged;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public IModelEditor ModelEditor
		{
			get;
			set;
		}

		public PageValidator PageValidator
		{
			get
			{
				return this.mPageValidator;
			}
		}

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get;
			set;
		}

		public DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get;
			set;
		}

		public DisplayMode DisplayMode
		{
			get
			{
				return this.mDisplayMode;
			}
			set
			{
				if (this.mDisplayMode != null)
				{
					this.mTreeList.RefreshDataSource();
				}
				this.mDisplayMode = value;
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public TriggerConfiguration TriggerConfiguration
		{
			get
			{
				return this.mTriggerConfiguration;
			}
			set
			{
				bool flag = value != null && (this.mTriggerConfiguration == null || this.mCurrentTriggerMode != value.TriggerMode.Value);
				this.mTriggerConfiguration = value;
				this.mCurrentTriggerMode = ((this.mTriggerConfiguration != null) ? this.mTriggerConfiguration.TriggerMode.Value : TriggerMode.OnOff);
				if (this.mTriggerConfiguration == null)
				{
					return;
				}
				ulong selectedEventId = this.GetSelectedEventId();
				int selectedIndex = this.GetSelectedIndex();
				this.UpdateBindingList();
				this.FillComboBoxTriggerEffect();
				if (!this.SelectEventId(selectedEventId) && !flag)
				{
					this.SelectIndex(selectedIndex, true);
				}
				this.UpdateUpDownButtons();
				this.UpdateContextMenuStrip();
			}
		}

		public TriggerTree.AddAction NextAddAction
		{
			get
			{
				if (this.TriggerConfiguration == null)
				{
					return TriggerTree.AddAction.Trigger;
				}
				if (this.CanAddCondition)
				{
					return TriggerTree.AddAction.Condition;
				}
				switch (this.TriggerConfiguration.TriggerMode.Value)
				{
				case TriggerMode.Triggered:
					return TriggerTree.AddAction.Trigger;
				case TriggerMode.Permanent:
					return TriggerTree.AddAction.Marker;
				case TriggerMode.OnOff:
					if (!this.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport)
					{
						return TriggerTree.AddAction.Trigger;
					}
					if (this.TriggerConfiguration.OnOffTriggers.All((RecordTrigger t) => t.TriggerEffect.Value != TriggerEffect.LoggingOn))
					{
						return TriggerTree.AddAction.Trigger;
					}
					if (this.TriggerConfiguration.OnOffTriggers.All((RecordTrigger t) => t.TriggerEffect.Value != TriggerEffect.LoggingOff))
					{
						return TriggerTree.AddAction.Trigger;
					}
					return TriggerTree.AddAction.Marker;
				default:
					return TriggerTree.AddAction.Trigger;
				}
			}
		}

		public bool CanAddCombinedEvent
		{
			get
			{
				return this.mTreeList.FocusedNode == null || this.mTreeList.FocusedNode.Level < 2;
			}
		}

		public bool SelectedLevelImpliesConjunction
		{
			get
			{
				return this.mTreeList.FocusedNode == null || this.mTreeList.FocusedNode.Level % 2 == 0;
			}
		}

		public bool CanAddVoCANEvent
		{
			get
			{
				return this.mTreeList.FocusedNode == null || this.mTreeList.FocusedNode.Level == 0;
			}
		}

		public bool CanAddCondition
		{
			get
			{
				EventPresenter eventPresenter;
				return this.TryGetSelectedPresenter(out eventPresenter) && eventPresenter.Parent != null && eventPresenter.Parent.Event is CombinedEvent;
			}
		}

		public bool CanRemoveSelection
		{
			get
			{
				EventPresenter eventPresenter;
				return this.TryGetSelectedPresenter(out eventPresenter) && !(eventPresenter.Event is DummyEvent);
			}
		}

		private bool CanExpand
		{
			get
			{
				return this.mTreeList.Nodes.Cast<TreeListNode>().Any((TreeListNode t) => t.HasChildren && this.HasCollapsed(t));
			}
		}

		private bool CanCollapse
		{
			get
			{
				return this.mTreeList.Nodes.Cast<TreeListNode>().Any((TreeListNode t) => t.HasChildren && this.HasExpanded(t));
			}
		}

		public TriggerTree()
		{
			this.InitializeComponent();
			this.mTreeListGeneralServiceEx = new TreeListGeneralServiceEx(this.mTreeList);
			this.mTreeListGeneralServiceEx.InitAppearance();
			this.mTreeListNavigationService = new KeyboardNavigationService(this.mTreeList);
			this.mTreeNodeCheckBoxService = new TreeNodeCheckStateService(this.mTreeList, EnumCheckStateMode.PseudoTriState);
			this.mGuiElementManager = new GUIElementManager_ControlGridTree();
			this.mCustomErrorProvider = new CustomErrorProvider(this.mErrorProviderFormat);
			this.mCustomErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.mErrorProviderLocalModel);
			this.mCustomErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.mErrorProviderGlobalModel);
			this.mPageValidator = new PageValidator(this.mCustomErrorProvider);
			this.FillComboBoxAction();
			this.mTreeList.SelectImageList = MainImageList.Instance.ImageCollection;
			this.mTreeList.DataSource = this.mBindingList;
		}

		public void Init()
		{
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		private void OnDispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			this.mTreeNodeCheckBoxService.Dispose();
		}

		private void OnItemChanged(object sender, EventArgs e)
		{
			this.ValidateInput(true);
		}

		private void Raise_SelectionChanged()
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(this, EventArgs.Empty);
			}
		}

		public void AddTrigger(RecordTrigger trigger)
		{
			if (trigger == null || trigger.Event == null)
			{
				return;
			}
			this.AddToCurrentDataList(trigger);
			this.ValidateInput(true);
			if (trigger.Event is CombinedEvent)
			{
				EventPresenter eventPresenter;
				if (this.TryGetPresenter(trigger.Event.Id, out eventPresenter) && eventPresenter.GetAllChildPresenters(false).Count == 1)
				{
					this.SelectEventId(eventPresenter.GetAllChildPresenters(false)[0].Id);
					return;
				}
			}
			else
			{
				this.SelectEventId(trigger.Event.Id);
			}
		}

		public void AddCondition(Event ev)
		{
			if (ev == null)
			{
				return;
			}
			EventPresenter eventPresenter;
			if (!this.TryGetSelectedPresenter(out eventPresenter))
			{
				return;
			}
			EventPresenter eventPresenter2;
			CombinedEvent combinedEvent = (eventPresenter.ParentId != 0uL && this.TryGetPresenter(eventPresenter.ParentId, out eventPresenter2)) ? (eventPresenter2.Event as CombinedEvent) : (eventPresenter.Event as CombinedEvent);
			if (combinedEvent == null)
			{
				return;
			}
			combinedEvent.Add(ev);
			this.ValidateInput(true);
			if (ev is CombinedEvent)
			{
				EventPresenter eventPresenter3;
				if (this.TryGetPresenter(ev.Id, out eventPresenter3) && eventPresenter3.GetAllChildPresenters(false).Count == 1)
				{
					this.SelectEventId(eventPresenter3.GetAllChildPresenters(false)[0].Id);
					return;
				}
			}
			else
			{
				this.SelectEventId(ev.Id);
			}
		}

		private void UpdateBindingList()
		{
			using (new TreeListBatchUpdate(this.mTreeList))
			{
				ReadOnlyCollection<RecordTrigger> currentList = this.mTriggerConfiguration.CurrentList;
				List<Event> list = new List<Event>();
				foreach (RecordTrigger current in currentList)
				{
					list.Add(current.Event);
					list.AddRange(current.GetAllChildren());
				}
				List<ulong> dataModelEventIds = (from t in list
				select t.Id).ToList<ulong>();
				List<ulong> list2 = (from t in this.mPresenterMap.Keys
				where this.mPresenterMap[t] is RecordTriggerPresenter && !dataModelEventIds.Contains(t)
				select t).ToList<ulong>();
				foreach (ulong current2 in list2)
				{
					this.RemoveFromPresenterLayer(this.mPresenterMap[current2]);
				}
				List<RecordTriggerPresenter> list3 = this.mPresenterMap.Values.OfType<RecordTriggerPresenter>().ToList<RecordTriggerPresenter>();
				foreach (RecordTriggerPresenter current3 in list3)
				{
					ReadOnlyCollection<Event> allChildren = current3.RecordTrigger.GetAllChildren();
					List<ulong> childEventIds = (from t in allChildren
					select t.Id).ToList<ulong>();
					ReadOnlyCollection<EventPresenter> allChildPresenters = current3.GetAllChildPresenters(true);
					List<EventPresenter> list4 = (from t in allChildPresenters
					where !childEventIds.Contains(t.Id)
					select t).ToList<EventPresenter>();
					foreach (EventPresenter current4 in list4)
					{
						this.RemoveFromPresenterLayer(current4);
					}
				}
				foreach (RecordTriggerPresenter current5 in list3)
				{
					current5.UpdateChildren();
				}
				foreach (RecordTriggerPresenter current6 in list3)
				{
					ReadOnlyCollection<EventPresenter> allChildPresenters2 = current6.GetAllChildPresenters(true);
					List<EventPresenter> list5 = (from t in allChildPresenters2
					where !this.mPresenterMap.ContainsKey(t.Id)
					select t).ToList<EventPresenter>();
					foreach (EventPresenter current7 in list5)
					{
						this.AddToPresenterLayer(current7);
					}
				}
				List<RecordTrigger> list6 = (from t in currentList
				where !this.mPresenterMap.ContainsKey(t.Event.Id)
				select t).ToList<RecordTrigger>();
				foreach (RecordTrigger current8 in list6)
				{
					this.AddToPresenterLayer(new RecordTriggerPresenter(current8, this));
				}
				for (int i = 0; i < dataModelEventIds.Count; i++)
				{
					ulong key = dataModelEventIds[i];
					EventPresenter item = this.mPresenterMap[key];
					int num = this.mBindingList.IndexOf(item);
					if (num != i)
					{
						this.mBindingList.Remove(item);
						this.mBindingList.Insert(i, item);
					}
				}
				List<RecordTriggerPresenter> list7 = this.mBindingList.OfType<RecordTriggerPresenter>().ToList<RecordTriggerPresenter>();
				List<EventPresenter> list8 = new List<EventPresenter>();
				foreach (RecordTriggerPresenter current9 in list7)
				{
					list8.Add(current9);
					list8.AddRange(current9.GetAllChildPresenters(true));
				}
				List<ulong> list9 = (from t in list8
				select t.Id).ToList<ulong>();
				for (int j = 0; j < list9.Count; j++)
				{
					ulong key2 = list9[j];
					EventPresenter item2 = this.mPresenterMap[key2];
					int num2 = this.mBindingList.IndexOf(item2);
					if (num2 != j)
					{
						this.mBindingList.Remove(item2);
						this.mBindingList.Insert(j, item2);
					}
				}
			}
		}

		private void AddToPresenterLayer(EventPresenter pres)
		{
			this.InsertInPresenterLayer(pres, this.mBindingList.Count);
		}

		private void AddToCurrentDataList(RecordTrigger trigger)
		{
			this.mTriggerConfiguration.AddToCurrentList(trigger);
		}

		private void InsertInPresenterLayer(EventPresenter pres, int targetIndex)
		{
			if (pres == null || pres.Event == null || this.mPresenterMap.ContainsKey(pres.Id))
			{
				return;
			}
			if (targetIndex < 0 || targetIndex > this.mBindingList.Count)
			{
				targetIndex = this.mBindingList.Count;
			}
			pres.DataChanged += new EventHandler(this.OnItemChanged);
			this.mPresenterMap.Add(pres.Id, pres);
			this.mBindingList.Insert(targetIndex, pres);
			pres.UpdateChildren();
			ReadOnlyCollection<EventPresenter> allChildPresenters = pres.GetAllChildPresenters(true);
			foreach (EventPresenter current in allChildPresenters)
			{
				targetIndex++;
				current.DataChanged += new EventHandler(this.OnItemChanged);
				this.mPresenterMap.Add(current.Id, current);
				this.mBindingList.Insert(targetIndex, current);
			}
		}

		private void InsertInCurrentDataList(EventPresenter pres, int targetIndex, EventPresenter targetParent = null)
		{
			if (pres == null)
			{
				return;
			}
			RecordTriggerPresenter recordTriggerPresenter = pres as RecordTriggerPresenter;
			CombinedEvent combinedEvent = (targetParent != null) ? (targetParent.Event as CombinedEvent) : null;
			if (recordTriggerPresenter != null)
			{
				this.mTriggerConfiguration.InsertInCurrentList(recordTriggerPresenter.RecordTrigger, targetIndex);
				return;
			}
			if (combinedEvent != null)
			{
				combinedEvent.Insert(targetIndex, pres.Event);
			}
		}

		private void RemoveFromPresenterLayer(EventPresenter pres)
		{
			if (pres == null || !this.mPresenterMap.ContainsKey(pres.Id))
			{
				return;
			}
			List<EventPresenter> list = new List<EventPresenter>(pres.GetAllChildPresenters(true));
			list.Reverse();
			foreach (EventPresenter current in list)
			{
				this.RemoveFromPresenterLayer(current);
			}
			this.mPresenterMap.Remove(pres.Id);
			this.mBindingList.Remove(pres);
			pres.DataChanged -= new EventHandler(this.OnItemChanged);
			pres.Dispose();
		}

		private void RemoveFromCurrentDataList(EventPresenter pres)
		{
			RecordTriggerPresenter recordTriggerPresenter = pres as RecordTriggerPresenter;
			CombinedEvent combinedEvent = null;
			EventPresenter eventPresenter;
			if (pres.ParentId != 0uL && this.TryGetPresenter(pres.ParentId, out eventPresenter))
			{
				combinedEvent = (eventPresenter.Event as CombinedEvent);
			}
			if (recordTriggerPresenter != null)
			{
				this.mTriggerConfiguration.RemoveFromCurrentList(recordTriggerPresenter.RecordTrigger);
				return;
			}
			if (combinedEvent != null)
			{
				combinedEvent.Remove(pres.Event);
			}
		}

		private bool TryGetEventId(TreeListNode node, out ulong eventId)
		{
			eventId = 0uL;
			if (node == null)
			{
				return false;
			}
			EventPresenter eventPresenter = this.mTreeList.GetDataRecordByNode(node) as EventPresenter;
			eventId = ((eventPresenter != null) ? eventPresenter.Id : 0uL);
			return eventId > 0uL;
		}

		private bool TryGetEventId(TreeListNode node, out ulong eventId, out int index)
		{
			index = -1;
			if (!this.TryGetEventId(node, out eventId))
			{
				return false;
			}
			index = this.GetIndex(eventId);
			return index >= 0;
		}

		private bool TryGetPresenter(ulong eventId, out EventPresenter pres)
		{
			pres = null;
			if (!this.mPresenterMap.ContainsKey(eventId))
			{
				return false;
			}
			pres = this.mPresenterMap[eventId];
			return pres != null;
		}

		private bool TryGetPresenter(TreeListNode node, out EventPresenter pres)
		{
			pres = this.GetPresenter(node);
			return pres != null;
		}

		private EventPresenter GetPresenter(TreeListNode node)
		{
			if (node == null)
			{
				return null;
			}
			return this.mTreeList.GetDataRecordByNode(node) as EventPresenter;
		}

		private int GetIndex(ulong eventId)
		{
			if (!this.mPresenterMap.ContainsKey(eventId))
			{
				return -1;
			}
			return this.mBindingList.IndexOf(this.mPresenterMap[eventId]);
		}

		private int GetIndexInCurrentDataList(EventPresenter pres)
		{
			if (pres == null)
			{
				return -1;
			}
			RecordTriggerPresenter recordTriggerPresenter = pres as RecordTriggerPresenter;
			CombinedEvent combinedEvent = (pres.Parent != null) ? (pres.Parent.Event as CombinedEvent) : null;
			if (recordTriggerPresenter != null)
			{
				return this.mTriggerConfiguration.IndexInCurrentList(recordTriggerPresenter.RecordTrigger);
			}
			if (combinedEvent != null)
			{
				return combinedEvent.IndexOf(pres.Event);
			}
			return -1;
		}

		private ulong GetSelectedEventId()
		{
			ulong result;
			if (!this.TryGetEventId(this.mTreeList.FocusedNode, out result))
			{
				return 0uL;
			}
			return result;
		}

		private int GetSelectedIndex()
		{
			ulong num;
			int result;
			if (!this.TryGetEventId(this.mTreeList.FocusedNode, out num, out result))
			{
				return -1;
			}
			return result;
		}

		private bool SelectEventId(ulong eventId)
		{
			if (!this.mPresenterMap.ContainsKey(eventId))
			{
				return false;
			}
			TreeListNode nodeByDataRecord = this.GetNodeByDataRecord(this.mPresenterMap[eventId]);
			if (nodeByDataRecord == null)
			{
				return false;
			}
			this.mTreeList.FocusedNode = nodeByDataRecord;
			this.mTreeList.Selection.Set(nodeByDataRecord);
			this.UpdateUpDownButtons();
			this.UpdateContextMenuStrip();
			return true;
		}

		private void SelectIndex(int index, bool allowIndexAdaption = true)
		{
			if (allowIndexAdaption && this.mBindingList.Any<EventPresenter>())
			{
				if (index < 0)
				{
					index = 0;
				}
				else if (index >= this.mBindingList.Count)
				{
					index = this.mBindingList.Count - 1;
				}
			}
			if (index < 0 || index >= this.mBindingList.Count)
			{
				return;
			}
			TreeListNode nodeByDataRecord = this.GetNodeByDataRecord(this.mBindingList[index]);
			if (nodeByDataRecord == null)
			{
				return;
			}
			this.mTreeList.FocusedNode = nodeByDataRecord;
			this.mTreeList.Selection.Set(nodeByDataRecord);
			this.UpdateUpDownButtons();
			this.UpdateContextMenuStrip();
		}

		public bool TryGetSelectedPresenter(out EventPresenter pres)
		{
			return this.TryGetPresenter(this.mTreeList.FocusedNode, out pres);
		}

		private TreeListNode GetNodeByDataRecord(EventPresenter pres)
		{
			return this.GetNodeByDataRecordInternal(pres, this.mTreeList.Nodes);
		}

		private TreeListNode GetNodeByDataRecordInternal(EventPresenter pres, TreeListNodes nodes)
		{
			if (pres == null)
			{
				return null;
			}
			foreach (TreeListNode treeListNode in nodes)
			{
				EventPresenter eventPresenter;
				if (this.TryGetPresenter(treeListNode, out eventPresenter) && pres == eventPresenter)
				{
					TreeListNode result = treeListNode;
					return result;
				}
				if (treeListNode.Nodes != null && treeListNode.Nodes.Count > 0)
				{
					TreeListNode nodeByDataRecordInternal = this.GetNodeByDataRecordInternal(pres, treeListNode.Nodes);
					if (nodeByDataRecordInternal != null)
					{
						TreeListNode result = nodeByDataRecordInternal;
						return result;
					}
				}
			}
			return null;
		}

		private void AdaptSelectionOnMouseDown(TreeListHitInfo hitInfo, MouseEventArgs e)
		{
			if (hitInfo == null || hitInfo.Node == null)
			{
				return;
			}
			if (e.Button != MouseButtons.Right)
			{
				return;
			}
			ulong eventId;
			if (!this.TryGetEventId(hitInfo.Node, out eventId))
			{
				return;
			}
			if (hitInfo.Column != null)
			{
				this.mTreeList.FocusedColumn = hitInfo.Column;
			}
			this.SelectEventId(eventId);
		}

		public void RemoveSelection()
		{
			if (!this.CanRemoveSelection)
			{
				return;
			}
			EventPresenter eventPresenter;
			if (!this.TryGetSelectedPresenter(out eventPresenter))
			{
				return;
			}
			if (eventPresenter.GetAllChildPresenters(true).Count((EventPresenter t) => !(t.Event is DummyEvent)) > 1 && InformMessageBox.Show(EnumInfoType.Warning, EnumQuestionType.QuestionDefaultNo, Resources_Trigger.WarningDeleteCombinedEvent) != DialogResult.Yes)
			{
				return;
			}
			int selectedIndex = this.GetSelectedIndex();
			this.RemoveFromCurrentDataList(eventPresenter);
			this.ValidateInput(true);
			this.SelectIndex(selectedIndex, true);
		}

		private bool HasCollapsed(TreeListNode node)
		{
			return node != null && node.HasChildren && (!node.Expanded || node.Nodes.Cast<TreeListNode>().Any(new Func<TreeListNode, bool>(this.HasCollapsed)));
		}

		private void ExpandAll()
		{
			this.mTreeList.ExpandAll();
		}

		private bool HasExpanded(TreeListNode node)
		{
			return node != null && node.HasChildren && (node.Expanded || node.Nodes.Cast<TreeListNode>().Any(new Func<TreeListNode, bool>(this.HasExpanded)));
		}

		private void CollapseAll()
		{
			this.mTreeList.CollapseAll();
		}

		private void TreeList_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
		{
			this.mTreeListGeneralServiceEx.CustomDrawNodeCell<EventPresenter>(e, new GeneralService.IsReadOnlyAtAll<EventPresenter>(TriggerTree.IsReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<EventPresenter>(TriggerTree.IsReadOnlyByCellContent), new TreeListGeneralServiceEx.IsInactiveButEditable<EventPresenter>(TriggerTree.IsInactiveButEditable));
			ulong num;
			int dataSourceRowIndex;
			if (!this.TryGetEventId(e.Node, out num, out dataSourceRowIndex))
			{
				return;
			}
			IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(e.Column, dataSourceRowIndex);
			this.mCustomErrorProvider.Tree.DisplayError(gUIElement, e);
			if (this.TriggerConfiguration.TriggerMode.Value != TriggerMode.OnOff)
			{
				return;
			}
			if (e.Node.Selected && (e.Column != this.mTreeList.FocusedColumn || e.Node != this.mTreeList.FocusedNode))
			{
				return;
			}
			EventPresenter eventPresenter;
			if (!this.TryGetPresenter(e.Node, out eventPresenter))
			{
				return;
			}
			RecordTriggerPresenter recordTriggerPresenter = eventPresenter as RecordTriggerPresenter;
			if (recordTriggerPresenter == null)
			{
				return;
			}
			if (recordTriggerPresenter.RecordTrigger.TriggerEffect.Value == TriggerEffect.LoggingOn)
			{
				e.Appearance.BackColor = this.mLoggingOnColor;
				return;
			}
			if (recordTriggerPresenter.RecordTrigger.TriggerEffect.Value == TriggerEffect.LoggingOff)
			{
				e.Appearance.BackColor = this.mLoggingOffColor;
			}
		}

		private void TreeList_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
		{
			EventPresenter pres;
			if (!this.TryGetPresenter(e.Node, out pres))
			{
				return;
			}
			if (TriggerTree.IsReadOnlyAtAll(pres, e.Column) || TriggerTree.IsReadOnlyByCellContent(pres, e.Column))
			{
				e.RepositoryItem = this.mRepositoryItemTextEditDummy;
			}
		}

		private void TreeList_EditorKeyDown(object sender, KeyEventArgs e)
		{
			this.mTreeListNavigationService.EditorKeyDown(e);
		}

		private void TreeList_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			this.mTreeListGeneralServiceEx.FocusedColumnChanged(e);
		}

		private void TreeList_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
		{
			this.mTreeListGeneralServiceEx.FocusedNodeChanged(e);
			this.Raise_SelectionChanged();
			this.UpdateUpDownButtons();
			this.UpdateContextMenuStrip();
		}

		private void TreeList_KeyDown(object sender, KeyEventArgs e)
		{
			this.mTreeListNavigationService.KeyDown(e);
			Keys keyData = e.KeyData;
			if (keyData > Keys.Divide)
			{
				switch (keyData)
				{
				case Keys.LButton | Keys.RButton | Keys.Space | Keys.Control:
					e.Handled = true;
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Bottom);
					return;
				case Keys.MButton | Keys.Space | Keys.Control:
					e.Handled = true;
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Top);
					return;
				case Keys.LButton | Keys.MButton | Keys.Space | Keys.Control:
				case Keys.LButton | Keys.RButton | Keys.MButton | Keys.Space | Keys.Control:
					break;
				case Keys.RButton | Keys.MButton | Keys.Space | Keys.Control:
					e.Handled = true;
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Up);
					return;
				case Keys.Back | Keys.Space | Keys.Control:
					e.Handled = true;
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Down);
					return;
				default:
					if (keyData == (Keys)131139)
					{
						e.Handled = true;
						ConfigClipboardManager.Copy();
						return;
					}
					switch (keyData)
					{
					case (Keys)131158:
						ConfigClipboardManager.Paste();
						return;
					case (Keys)131159:
						break;
					case (Keys)131160:
						ConfigClipboardManager.Copy();
						this.RemoveSelection();
						break;
					default:
						return;
					}
					break;
				}
				return;
			}
			if (keyData != Keys.Delete)
			{
				if (keyData == Keys.Multiply)
				{
					e.Handled = true;
					this.ExpandAll();
					return;
				}
				if (keyData != Keys.Divide)
				{
					return;
				}
				e.Handled = true;
				this.CollapseAll();
				return;
			}
			else
			{
				e.Handled = true;
				if (!this.CanRemoveSelection)
				{
					return;
				}
				this.RemoveSelection();
				return;
			}
		}

		private void TreeList_MouseDown(object sender, MouseEventArgs e)
		{
			base.ActiveControl = this.mTreeList;
			this.mTreeListGeneralServiceEx.MouseDown(e);
			TreeListHitInfo hitInfo = this.mTreeList.CalcHitInfo(e.Location);
			this.AdaptSelectionOnMouseDown(hitInfo, e);
			this.PrepareDragOnMouseDown(hitInfo, e);
		}

		private void TreeList_MouseMove(object sender, MouseEventArgs e)
		{
			this.DragStart(e);
		}

		private void TreeList_MouseUp(object sender, MouseEventArgs e)
		{
			this.mTreeListGeneralServiceEx.MouseUp(e);
			this.CancelDragStart();
		}

		private void TreeList_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.Menu is TreeListColumnMenu)
			{
				this.mTreeListGeneralServiceEx.PopupMenuShowing(e);
				DXMenuItem dXMenuItem = null;
				DXMenuItem dXMenuItem2 = null;
				foreach (DXMenuItem dXMenuItem3 in e.Menu.Items)
				{
					switch ((TreeListStringId)dXMenuItem3.Tag)
					{
					case TreeListStringId.MenuColumnColumnCustomization:
						dXMenuItem = dXMenuItem3;
						break;
					case TreeListStringId.MenuColumnBestFitAllColumns:
						dXMenuItem2 = dXMenuItem3;
						break;
					}
				}
				if (dXMenuItem != null)
				{
					e.Menu.Items.Remove(dXMenuItem);
				}
				if (dXMenuItem2 != null)
				{
					dXMenuItem2.BeginGroup = false;
					return;
				}
			}
			else
			{
				TreeListNodeMenu arg_C7_0 = e.Menu as TreeListNodeMenu;
			}
		}

		private void TreeList_ShowingEditor(object sender, CancelEventArgs e)
		{
			EventPresenter dataRecord;
			if (this.TryGetSelectedPresenter(out dataRecord))
			{
				this.mTreeListGeneralServiceEx.ShowingEditor<EventPresenter>(e, dataRecord, this.mTreeList.FocusedColumn, new GeneralService.IsReadOnlyAtAll<EventPresenter>(TriggerTree.IsReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<EventPresenter>(TriggerTree.IsReadOnlyByCellContent));
				return;
			}
			e.Cancel = true;
		}

		private static bool IsReadOnlyAtAll(EventPresenter pres, TreeListColumn column)
		{
			return (column.FieldName == "Type" && column.ColumnEdit is RepositoryItemComboBox && ((RepositoryItemComboBox)column.ColumnEdit).Items.Count <= 1) || pres.IsReadOnlyAtAll(column.FieldName);
		}

		private static bool IsReadOnlyByCellContent(EventPresenter pres, TreeListColumn column)
		{
			return pres.IsReadOnlyByCellContent(column.FieldName);
		}

		private static bool IsInactiveButEditable(EventPresenter pres, TreeListColumn column)
		{
			return pres.IsInactiveButEditable(column.FieldName);
		}

		private void TreeList_ShownEditor(object sender, EventArgs e)
		{
			this.mTreeListNavigationService.ShownEditor();
			if (this.mTreeList.FocusedColumn == this.mColChannel)
			{
				this.FillComboBoxChannel();
			}
		}

		private void TreeList_CustomDrawEmptyArea(object sender, CustomDrawEmptyAreaEventArgs e)
		{
			if (this == null || this.ModelValidator == null)
			{
				return;
			}
			if (!this.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport && this.mTriggerConfiguration.TriggerMode.Value == TriggerMode.Permanent)
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = (stringFormat.LineAlignment = StringAlignment.Center);
				RectangleF bounds = e.EmptyAreaRegion.GetBounds(e.Graphics);
				e.Graphics.DrawString(Resources_Trigger.CurrentLoggerTypeNotSupportMarkers, e.Appearance.Font, SystemBrushes.WindowText, bounds, stringFormat);
				e.Handled = true;
			}
		}

		private void TreeList_TopVisibleNodeIndexChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void TreeList_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void TriggerTree_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		public void DisplayErrors()
		{
			PageValidatorTreeUtil.StoreNodeMappingForVisibleNodes(this.mTreeList, new PageValidatorTreeUtil.StoreNodeMappingHandler(this.OnPageValidatorTreeUtilStoreNodeMapping));
			this.mPageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			this.mTreeList.Refresh();
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			this.ResetValidationFramework();
			PageValidatorTreeUtil.StoreNodeMappingForVisibleNodes(this.mTreeList, new PageValidatorTreeUtil.StoreNodeMappingHandler(this.OnPageValidatorTreeUtilStoreNodeMapping));
			bool flag = true;
			flag &= this.ModelValidator.Validate(this.mTriggerConfiguration, isDataChanged, this.mPageValidator);
			this.DisplayErrors();
			return flag;
		}

		public bool HasErrors()
		{
			return this.mPageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		public bool HasGlobalErrors()
		{
			return this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
		}

		public bool HasLocalErrors()
		{
			return this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.mPageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		public void ResetValidationFramework()
		{
			this.mPageValidator.General.Reset();
			this.mGuiElementManager.Reset();
		}

		private void OnPageValidatorTreeUtilStoreNodeMapping(TreeListNode treeNode)
		{
			ulong eventId;
			int dataSourceIdx;
			if (!this.TryGetEventId(treeNode, out eventId, out dataSourceIdx))
			{
				return;
			}
			this.StoreMapping4VisibleColumns(eventId, dataSourceIdx);
		}

		private void StoreMapping4VisibleColumns(ulong eventId, int dataSourceIdx)
		{
			EventPresenter eventPresenter;
			if (!this.TryGetPresenter(eventId, out eventPresenter))
			{
				return;
			}
			RecordTriggerPresenter recordTriggerPresenter = eventPresenter as RecordTriggerPresenter;
			RecordTrigger recordTrigger = (recordTriggerPresenter != null) ? recordTriggerPresenter.RecordTrigger : null;
			if (this.mColEvent.Visible)
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(this.mColEvent, dataSourceIdx);
				if (eventPresenter.Event is VoCanRecordingEvent)
				{
					this.mPageValidator.Tree.StoreMapping((eventPresenter.Event as VoCanRecordingEvent).IsUsingCASM2T3L, gUIElement);
				}
				else if (eventPresenter.Event is IncEvent)
				{
					this.mPageValidator.Tree.StoreMapping((eventPresenter.Event as IncEvent).FilePath, gUIElement);
				}
				this.mPageValidator.Tree.StoreMapping(eventPresenter.Event.IsPointInTime, gUIElement);
			}
			if (this.mColChannel.Visible)
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(this.mColChannel, dataSourceIdx);
				IEnumerable<IValidatedProperty> enumerable = eventPresenter.GetValidatedPropertyListChannel();
				foreach (IValidatedProperty current in enumerable)
				{
					this.mPageValidator.Tree.StoreMapping(current, gUIElement);
				}
			}
			if (this.mColCondition.Visible)
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(this.mColCondition, dataSourceIdx);
				IEnumerable<IValidatedProperty> enumerable = eventPresenter.GetValidatedPropertyListCondition();
				foreach (IValidatedProperty current2 in enumerable)
				{
					if (current2 != null)
					{
						this.mPageValidator.Tree.StoreMapping(current2, gUIElement);
					}
				}
			}
			if (this.mColTriggerType.Visible)
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(this.mColTriggerType, dataSourceIdx);
				if (recordTrigger != null)
				{
					this.mPageValidator.Tree.StoreMapping(recordTrigger.TriggerEffect, gUIElement);
				}
			}
			if (this.mColAction.Visible)
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(this.mColAction, dataSourceIdx);
				if (recordTrigger != null)
				{
					this.mPageValidator.Tree.StoreMapping(recordTrigger.Action, gUIElement);
				}
			}
			if (this.mColName.Visible)
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(this.mColName, dataSourceIdx);
				if (recordTrigger != null)
				{
					this.mPageValidator.Tree.StoreMapping(recordTrigger.Name, gUIElement);
				}
			}
			if (this.mColComment.Visible)
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(this.mColComment, dataSourceIdx);
				if (recordTrigger != null)
				{
					this.mPageValidator.Tree.StoreMapping(recordTrigger.Comment, gUIElement);
				}
			}
		}

		private void FillComboBoxChannel()
		{
			ComboBoxEdit comboBoxEdit = this.mTreeList.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit == null)
			{
				return;
			}
			EventPresenter eventPresenter;
			if (!this.TryGetSelectedPresenter(out eventPresenter))
			{
				return;
			}
			if (eventPresenter.Event is CANIdEvent || eventPresenter.Event is CANDataEvent || eventPresenter.Event is CANBusStatisticsEvent)
			{
				this.FillComboBoxChannelCan(comboBoxEdit);
				return;
			}
			if (eventPresenter.Event is LINIdEvent || eventPresenter.Event is LINDataEvent)
			{
				this.FillComboBoxChannelLin(comboBoxEdit);
				return;
			}
			if (eventPresenter.Event is FlexrayIdEvent)
			{
				this.FillComboBoxChannelFlexray(comboBoxEdit);
				return;
			}
			if (eventPresenter.Event is SymbolicMessageEvent)
			{
				switch ((eventPresenter.Event as SymbolicMessageEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					this.FillComboBoxChannelCan(comboBoxEdit);
					return;
				case BusType.Bt_LIN:
					this.FillComboBoxChannelLin(comboBoxEdit);
					return;
				case BusType.Bt_FlexRay:
					this.FillComboBoxChannelFlexray(comboBoxEdit);
					return;
				default:
					return;
				}
			}
			else if (eventPresenter.Event is SymbolicSignalEvent)
			{
				switch ((eventPresenter.Event as ISymbolicSignalEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					this.FillComboBoxChannelCan(comboBoxEdit);
					return;
				case BusType.Bt_LIN:
					this.FillComboBoxChannelLin(comboBoxEdit);
					return;
				case BusType.Bt_FlexRay:
					this.FillComboBoxChannelFlexray(comboBoxEdit);
					return;
				default:
					return;
				}
			}
			else if (eventPresenter.Event is MsgTimeoutEvent)
			{
				switch ((eventPresenter.Event as MsgTimeoutEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					this.FillComboBoxChannelCan(comboBoxEdit);
					return;
				case BusType.Bt_LIN:
					this.FillComboBoxChannelLin(comboBoxEdit);
					return;
				default:
					return;
				}
			}
			else
			{
				if (eventPresenter.Event is DigitalInputEvent)
				{
					this.FillComboBoxDigitalInput(comboBoxEdit);
					return;
				}
				if (eventPresenter.Event is AnalogInputEvent)
				{
					this.FillComboBoxAnalogInput(comboBoxEdit);
					return;
				}
				if (eventPresenter.Event is KeyEvent)
				{
					this.FillComboBoxKey(comboBoxEdit);
				}
				return;
			}
		}

		private void FillComboBoxChannelCan(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
		}

		private void FillComboBoxChannelLin(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapLINChannelNumber2String(num, this.ModelValidator.LoggerSpecifics));
			}
		}

		private void FillComboBoxChannelFlexray(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_FlexRay); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num));
			}
		}

		private void FillComboBoxDigitalInput(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapDigitalInputNumber2String(num));
			}
		}

		private void FillComboBoxAnalogInput(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.IO.NumberOfAnalogInputs; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapAnalogInputNumber2String(num));
			}
		}

		private void FillComboBoxKey(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.IO.NumberOfKeys; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num, false));
			}
			for (uint num2 = 1u; num2 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys; num2 += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num2, true));
			}
			for (uint num3 = 1u + Constants.CasKeyOffset; num3 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfCasKeys + Constants.CasKeyOffset; num3 += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num3, false));
			}
		}

		private void RepositoryItemButtonEditCondition_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			EventPresenter eventPresenter;
			if (!this.TryGetSelectedPresenter(out eventPresenter))
			{
				return;
			}
			RecordTriggerPresenter recordTriggerPresenter = eventPresenter as RecordTriggerPresenter;
			bool flag = recordTriggerPresenter != null && recordTriggerPresenter.HasDefaultName;
			if (!this.EditTriggerCondition(eventPresenter.Event))
			{
				return;
			}
			if (flag)
			{
				eventPresenter.Name = recordTriggerPresenter.DefaultName;
			}
			this.ValidateInput(true);
		}

		private bool EditTriggerCondition(Event ev)
		{
			if (ev is CANIdEvent)
			{
				return this.EditCanIdTriggerCondition(ev as CANIdEvent);
			}
			if (ev is LINIdEvent)
			{
				return this.EditLinIdTriggerCondition(ev as LINIdEvent);
			}
			if (ev is FlexrayIdEvent)
			{
				return this.EditFlexrayIdTriggerCondition(ev as FlexrayIdEvent);
			}
			if (ev is CANDataEvent)
			{
				return this.EditCanDataTriggerCondition(ev as CANDataEvent);
			}
			if (ev is LINDataEvent)
			{
				return this.EditLinDataTriggerCondition(ev as LINDataEvent);
			}
			if (ev is SymbolicMessageEvent)
			{
				return this.EditSymbolicMessageTriggerCondition(ev as SymbolicMessageEvent);
			}
			if (ev is ISymbolicSignalEvent)
			{
				return this.EditSymbolicSignalTriggerCondition(ev as ISymbolicSignalEvent);
			}
			if (ev is MsgTimeoutEvent)
			{
				return this.EditMsgTimeoutCondition(ev as MsgTimeoutEvent);
			}
			if (ev is CANBusStatisticsEvent)
			{
				return this.EditCanBusStatisticsTriggerCondition(ev as CANBusStatisticsEvent);
			}
			if (ev is KeyEvent)
			{
				return this.EditKeyCondition(ev as KeyEvent);
			}
			if (ev is DigitalInputEvent)
			{
				return this.EditDigitalInputCondition(ev as DigitalInputEvent);
			}
			if (ev is AnalogInputEvent)
			{
				return this.EditAnalogInputCondition(ev as AnalogInputEvent);
			}
			if (ev is IgnitionEvent)
			{
				return this.EditIgnitionCondition(ev as IgnitionEvent);
			}
			if (ev is VoCanRecordingEvent)
			{
				return this.EditVoCanRecordingCondition(ev as VoCanRecordingEvent);
			}
			return ev is IncEvent && this.EditIncEventCondition(ev as IncEvent);
		}

		private bool EditCanIdTriggerCondition(CANIdEvent canIdEvent)
		{
			if (canIdEvent == null)
			{
				return false;
			}
			using (CANIdCondition cANIdCondition = new CANIdCondition(this.ModelValidator)
			{
				CANIdEvent = new CANIdEvent(canIdEvent)
			})
			{
				if (DialogResult.OK != cANIdCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (canIdEvent.Equals(cANIdCondition.CANIdEvent))
				{
					bool result = false;
					return result;
				}
				canIdEvent.Assign(cANIdCondition.CANIdEvent);
			}
			return true;
		}

		private bool EditFlexrayIdTriggerCondition(FlexrayIdEvent frIdEvent)
		{
			if (frIdEvent == null)
			{
				return false;
			}
			using (FlexrayIdCondition flexrayIdCondition = new FlexrayIdCondition(this.ModelValidator)
			{
				FlexrayIdEvent = new FlexrayIdEvent(frIdEvent)
			})
			{
				if (DialogResult.OK != flexrayIdCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (frIdEvent.Equals(flexrayIdCondition.FlexrayIdEvent))
				{
					bool result = false;
					return result;
				}
				frIdEvent.Assign(flexrayIdCondition.FlexrayIdEvent);
			}
			return true;
		}

		private bool EditCanBusStatisticsTriggerCondition(CANBusStatisticsEvent busStatEvent)
		{
			if (busStatEvent == null)
			{
				return false;
			}
			using (CANBusStatisticsCondition cANBusStatisticsCondition = new CANBusStatisticsCondition(this.ModelValidator)
			{
				CANBusStatisticsEvent = new CANBusStatisticsEvent(busStatEvent)
			})
			{
				if (DialogResult.OK != cANBusStatisticsCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (busStatEvent.Equals(cANBusStatisticsCondition.CANBusStatisticsEvent))
				{
					bool result = false;
					return result;
				}
				busStatEvent.Assign(cANBusStatisticsCondition.CANBusStatisticsEvent);
			}
			return true;
		}

		private bool EditLinIdTriggerCondition(LINIdEvent linIdEvent)
		{
			if (linIdEvent == null)
			{
				return false;
			}
			using (LINIdCondition lINIdCondition = new LINIdCondition(this.ModelValidator)
			{
				LINIdEvent = new LINIdEvent(linIdEvent)
			})
			{
				if (DialogResult.OK != lINIdCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (linIdEvent.Equals(lINIdCondition.LINIdEvent))
				{
					bool result = false;
					return result;
				}
				linIdEvent.Assign(lINIdCondition.LINIdEvent);
			}
			return true;
		}

		private bool EditCanDataTriggerCondition(CANDataEvent canDataEvent)
		{
			if (canDataEvent == null)
			{
				return false;
			}
			using (CANDataCondition cANDataCondition = new CANDataCondition(this.ModelValidator, null)
			{
				CANDataEvent = new CANDataEvent(canDataEvent)
			})
			{
				if (DialogResult.OK != cANDataCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (canDataEvent.Equals(cANDataCondition.CANDataEvent))
				{
					bool result = false;
					return result;
				}
				canDataEvent.Assign(cANDataCondition.CANDataEvent);
			}
			return true;
		}

		private bool EditLinDataTriggerCondition(LINDataEvent linDataEvent)
		{
			if (linDataEvent == null)
			{
				return false;
			}
			using (LINDataCondition lINDataCondition = new LINDataCondition(this.ModelValidator, null)
			{
				LINDataEvent = new LINDataEvent(linDataEvent)
			})
			{
				if (DialogResult.OK != lINDataCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (linDataEvent.Equals(lINDataCondition.LINDataEvent))
				{
					bool result = false;
					return result;
				}
				linDataEvent.Assign(lINDataCondition.LINDataEvent);
			}
			return true;
		}

		private bool EditSymbolicMessageTriggerCondition(SymbolicMessageEvent symbolicMsgEvent)
		{
			if (symbolicMsgEvent == null)
			{
				return false;
			}
			using (SymbolicMessageCondition symbolicMessageCondition = new SymbolicMessageCondition(this.ModelValidator, this.ApplicationDatabaseManager)
			{
				SymbolicMessageEvent = new SymbolicMessageEvent(symbolicMsgEvent)
			})
			{
				if (DialogResult.OK != symbolicMessageCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (symbolicMsgEvent.Equals(symbolicMessageCondition.SymbolicMessageEvent))
				{
					bool result = false;
					return result;
				}
				symbolicMsgEvent.Assign(symbolicMessageCondition.SymbolicMessageEvent);
			}
			return true;
		}

		private bool EditSymbolicSignalTriggerCondition(ISymbolicSignalEvent symbolicSigEvent)
		{
			if (symbolicSigEvent == null)
			{
				return false;
			}
			ISymbolicSignalEvent signalEvent;
			if (symbolicSigEvent is SymbolicSignalEvent)
			{
				signalEvent = new SymbolicSignalEvent(symbolicSigEvent);
			}
			else if (symbolicSigEvent is CcpXcpSignalEvent)
			{
				signalEvent = new CcpXcpSignalEvent(symbolicSigEvent);
			}
			else
			{
				if (!(symbolicSigEvent is DiagnosticSignalEvent))
				{
					return false;
				}
				signalEvent = new DiagnosticSignalEvent(symbolicSigEvent);
			}
			SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null);
			if (symbolicSigEvent is CcpXcpSignalEvent)
			{
				symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
			}
			else if (symbolicSigEvent is DiagnosticSignalEvent)
			{
				symbolicSignalCondition.DiagnosticActionsConfiguration = this.DiagnosticActionsConfiguration;
				symbolicSignalCondition.DiagnosticsDatabaseConfiguration = this.DiagnosticsDatabaseConfiguration;
			}
			symbolicSignalCondition.SignalEvent = signalEvent;
			if (DialogResult.OK != symbolicSignalCondition.ShowDialog())
			{
				return false;
			}
			if (symbolicSigEvent.Equals(symbolicSignalCondition.SignalEvent))
			{
				return false;
			}
			symbolicSigEvent.Assign(symbolicSignalCondition.SignalEvent);
			return true;
		}

		private bool EditMsgTimeoutCondition(MsgTimeoutEvent msgTimeoutEvent)
		{
			if (msgTimeoutEvent == null)
			{
				return false;
			}
			using (MsgTimeoutCondition msgTimeoutCondition = new MsgTimeoutCondition(this.ModelValidator, this.ApplicationDatabaseManager)
			{
				MsgTimeoutEvent = new MsgTimeoutEvent(msgTimeoutEvent)
			})
			{
				if (DialogResult.OK != msgTimeoutCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (msgTimeoutEvent.Equals(msgTimeoutCondition.MsgTimeoutEvent))
				{
					bool result = false;
					return result;
				}
				msgTimeoutEvent.Assign(msgTimeoutCondition.MsgTimeoutEvent);
			}
			return true;
		}

		private bool EditKeyCondition(KeyEvent keyEvent)
		{
			if (keyEvent == null)
			{
				return false;
			}
			using (KeyCondition keyCondition = new KeyCondition(this.ModelValidator)
			{
				KeyEvent = new KeyEvent(keyEvent)
			})
			{
				if (DialogResult.OK != keyCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (keyEvent.Equals(keyCondition.KeyEvent))
				{
					bool result = false;
					return result;
				}
				keyEvent.Assign(keyCondition.KeyEvent);
			}
			return true;
		}

		private bool EditDigitalInputCondition(DigitalInputEvent digInEvent)
		{
			if (digInEvent == null)
			{
				return false;
			}
			using (DigitalInputCondition digitalInputCondition = new DigitalInputCondition(this.ModelValidator)
			{
				DigitalInputEvent = new DigitalInputEvent(digInEvent)
			})
			{
				if (DialogResult.OK != digitalInputCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (digInEvent.Equals(digitalInputCondition.DigitalInputEvent))
				{
					bool result = false;
					return result;
				}
				digInEvent.Assign(digitalInputCondition.DigitalInputEvent);
			}
			return true;
		}

		private bool EditAnalogInputCondition(AnalogInputEvent analogInputEvent)
		{
			if (analogInputEvent == null)
			{
				return false;
			}
			using (AnalogInputCondition analogInputCondition = new AnalogInputCondition(this.ModelValidator)
			{
				AnalogInputEvent = new AnalogInputEvent(analogInputEvent)
			})
			{
				if (DialogResult.OK != analogInputCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (analogInputEvent.Equals(analogInputCondition.AnalogInputEvent))
				{
					bool result = false;
					return result;
				}
				analogInputEvent.Assign(analogInputCondition.AnalogInputEvent);
			}
			return true;
		}

		private bool EditIgnitionCondition(IgnitionEvent ignitionEvent)
		{
			if (ignitionEvent == null)
			{
				return false;
			}
			using (IgnitionCondition ignitionCondition = new IgnitionCondition
			{
				IgnitionEvent = new IgnitionEvent(ignitionEvent)
			})
			{
				if (DialogResult.OK != ignitionCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (ignitionEvent.Equals(ignitionCondition.IgnitionEvent))
				{
					bool result = false;
					return result;
				}
				ignitionEvent.Assign(ignitionCondition.IgnitionEvent);
			}
			return true;
		}

		private bool EditVoCanRecordingCondition(VoCanRecordingEvent voCanEvent)
		{
			if (voCanEvent == null)
			{
				return false;
			}
			using (VoCanRecordingCondition voCanRecordingCondition = new VoCanRecordingCondition(this.ModelValidator)
			{
				VoCanRecordingEvent = new VoCanRecordingEvent(voCanEvent)
			})
			{
				if (DialogResult.OK != voCanRecordingCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (voCanEvent.Equals(voCanRecordingCondition.VoCanRecordingEvent))
				{
					bool result = false;
					return result;
				}
				voCanEvent.Assign(voCanRecordingCondition.VoCanRecordingEvent);
			}
			return true;
		}

		private bool EditIncEventCondition(IncEvent incEvent)
		{
			if (incEvent == null)
			{
				return false;
			}
			using (IncludeParameterCondition includeParameterCondition = new IncludeParameterCondition
			{
				IncEvent = new IncEvent(incEvent)
			})
			{
				includeParameterCondition.ErrorText = this.GetErrorText(incEvent);
				if (DialogResult.OK != includeParameterCondition.ShowDialog())
				{
					bool result = false;
					return result;
				}
				if (incEvent.Equals(includeParameterCondition.IncEvent))
				{
					bool result = false;
					return result;
				}
				incEvent.Assign(includeParameterCondition.IncEvent);
			}
			return true;
		}

		private string GetErrorText(IncEvent ev)
		{
			IModelValidationResultCollector resultCollector = this.mPageValidator.ResultCollector;
			string errorText;
			if (!string.IsNullOrEmpty(errorText = resultCollector.GetErrorText(ValidationErrorClass.FormatError, ev.FilePath)))
			{
				return errorText;
			}
			if (!string.IsNullOrEmpty(errorText = resultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, ev.FilePath)))
			{
				return errorText;
			}
			if (!string.IsNullOrEmpty(errorText = resultCollector.GetErrorText(ValidationErrorClass.LocalModelError, ev.FilePath)))
			{
				return errorText;
			}
			if (!string.IsNullOrEmpty(errorText = resultCollector.GetErrorText(ValidationErrorClass.FormatError, ev.ParamIndex)))
			{
				return errorText;
			}
			if (!string.IsNullOrEmpty(errorText = resultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, ev.ParamIndex)))
			{
				return errorText;
			}
			if (!string.IsNullOrEmpty(errorText = resultCollector.GetErrorText(ValidationErrorClass.LocalModelError, ev.ParamIndex)))
			{
				return errorText;
			}
			return string.Empty;
		}

		private void FillComboBoxTriggerEffect()
		{
			this.mRepositoryItemComboBoxTriggerEffect.Items.Clear();
			if (this.mTriggerConfiguration != null && this.mTriggerConfiguration.TriggerMode.Value == TriggerMode.OnOff)
			{
				if (this.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport)
				{
					this.mRepositoryItemComboBoxTriggerEffect.Items.Add(GUIUtil.MapTriggerEffect2String(TriggerEffect.Marker));
				}
				this.mRepositoryItemComboBoxTriggerEffect.Items.Add(GUIUtil.MapTriggerEffect2String(TriggerEffect.LoggingOn));
				this.mRepositoryItemComboBoxTriggerEffect.Items.Add(GUIUtil.MapTriggerEffect2String(TriggerEffect.LoggingOff));
				return;
			}
			if (this.mTriggerConfiguration != null && this.mTriggerConfiguration.TriggerMode.Value == TriggerMode.Permanent)
			{
				this.mRepositoryItemComboBoxTriggerEffect.Items.Add(GUIUtil.MapTriggerEffect2String(TriggerEffect.Marker));
				return;
			}
			this.mRepositoryItemComboBoxTriggerEffect.Items.Add(GUIUtil.MapTriggerEffect2String(TriggerEffect.Normal));
			if (this.ModelValidator.LoggerSpecifics.Recording.HasEnhancedTriggerSupport)
			{
				this.mRepositoryItemComboBoxTriggerEffect.Items.Add(GUIUtil.MapTriggerEffect2String(TriggerEffect.Single));
				this.mRepositoryItemComboBoxTriggerEffect.Items.Add(GUIUtil.MapTriggerEffect2String(TriggerEffect.EndMeasurement));
			}
		}

		private void FillComboBoxAction()
		{
			this.mRepositoryItemComboBoxAction.Items.Clear();
			foreach (TriggerAction triggerAction in Enum.GetValues(typeof(TriggerAction)))
			{
				if (triggerAction != TriggerAction.Unknown)
				{
					this.mRepositoryItemComboBoxAction.Items.Add(GUIUtil.MapTriggerAction2String(triggerAction));
				}
			}
		}

		public void ShowColumnComment(bool showhide)
		{
			this.mColComment.Visible = showhide;
			if (this.mTreeList.VisibleColumns.Count == 0)
			{
				return;
			}
			if (this.mColComment.Visible && this.mColComment.VisibleIndex != this.mTreeList.VisibleColumns.Count - 1)
			{
				this.mColComment.VisibleIndex = this.mTreeList.VisibleColumns.Count - 1;
			}
		}

		public void ShowColumnName(bool showhide)
		{
			this.mColName.Visible = showhide;
			if (this.mTreeList.VisibleColumns.Count == 0)
			{
				return;
			}
			int num = this.mColComment.Visible ? 1 : 0;
			if (this.mColName.Visible && this.mColName.VisibleIndex != this.mTreeList.VisibleColumns.Count - 1 - num)
			{
				this.mColName.VisibleIndex = this.mTreeList.VisibleColumns.Count - 1 - num;
			}
		}

		public string CreateMarkerName(RecordTrigger marker)
		{
			RecordTriggerPresenter recordTriggerPresenter = new RecordTriggerPresenter(marker, this);
			return recordTriggerPresenter.DefaultName;
		}

		private void RepositoryItemComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			this.mTreeList.PostEditor();
		}

		private void MoveRow(ulong sourceEventId, ulong targetEventId, TriggerTree.EnumDropPosition dropPos)
		{
			if (!this.CanMoveRow(sourceEventId, targetEventId, dropPos))
			{
				return;
			}
			EventPresenter eventPresenter = this.mPresenterMap[sourceEventId];
			EventPresenter eventPresenter2 = this.mPresenterMap[targetEventId];
			int indexInCurrentDataList = this.GetIndexInCurrentDataList(eventPresenter);
			int num = (eventPresenter2.Event is DummyEvent) ? eventPresenter2.Parent.IndexOf(eventPresenter2) : this.GetIndexInCurrentDataList(eventPresenter2);
			if (eventPresenter.ParentId == eventPresenter2.ParentId)
			{
				if (indexInCurrentDataList < num && dropPos == TriggerTree.EnumDropPosition.AsSiblingBefore)
				{
					num--;
				}
				else if (indexInCurrentDataList > num && dropPos == TriggerTree.EnumDropPosition.AsSiblingAfter)
				{
					num++;
				}
			}
			else if (dropPos == TriggerTree.EnumDropPosition.AsSiblingAfter)
			{
				num++;
			}
			EventPresenter eventPresenter3;
			if (eventPresenter.ParentId == 0uL && eventPresenter2.ParentId != 0uL)
			{
				eventPresenter3 = new EventPresenter(eventPresenter.Event.Clone() as Event, this, eventPresenter2.Parent);
			}
			else if (eventPresenter.ParentId != 0uL && eventPresenter2.ParentId == 0uL)
			{
				RecordTriggerPresenter recordTriggerParentOf = this.GetRecordTriggerParentOf(eventPresenter);
				RecordTriggerPresenter recordTriggerPresenter = new RecordTriggerPresenter(RecordTrigger.Create(eventPresenter.Event.Clone() as Event), this)
				{
					Type = recordTriggerParentOf.Type,
					Action = recordTriggerParentOf.Action,
					Comment = recordTriggerParentOf.Comment
				};
				recordTriggerPresenter.Name = ((!recordTriggerParentOf.HasDefaultName && !string.IsNullOrEmpty(recordTriggerParentOf.Name)) ? recordTriggerParentOf.Name : recordTriggerPresenter.DefaultName);
				eventPresenter3 = recordTriggerPresenter;
			}
			else if (eventPresenter.ParentId != eventPresenter2.ParentId)
			{
				eventPresenter3 = new EventPresenter(eventPresenter.Event.Clone() as Event, this, eventPresenter2.Parent);
			}
			else
			{
				eventPresenter3 = eventPresenter;
			}
			this.RemoveFromCurrentDataList(eventPresenter);
			this.InsertInCurrentDataList(eventPresenter3, num, eventPresenter2.Parent);
			this.ValidateInput(true);
			this.SelectEventId(eventPresenter3.Id);
		}

		private RecordTriggerPresenter GetRecordTriggerParentOf(EventPresenter pres)
		{
			EventPresenter eventPresenter = pres;
			while (eventPresenter.Parent != null)
			{
				eventPresenter = eventPresenter.Parent;
			}
			return eventPresenter as RecordTriggerPresenter;
		}

		private bool CanMoveRow(ulong sourceEventId, ulong targetEventId, TriggerTree.EnumDropPosition dropPos)
		{
			if (!this.mPresenterMap.ContainsKey(sourceEventId) || !this.mPresenterMap.ContainsKey(targetEventId) || dropPos == TriggerTree.EnumDropPosition.None || sourceEventId == targetEventId)
			{
				return false;
			}
			EventPresenter eventPresenter = this.mPresenterMap[sourceEventId];
			EventPresenter eventPresenter2 = this.mPresenterMap[targetEventId];
			if (eventPresenter.Event is DummyEvent)
			{
				return false;
			}
			if (eventPresenter.Event is CombinedEvent && eventPresenter.ParentId != eventPresenter2.ParentId)
			{
				return false;
			}
			if (eventPresenter.Event is VoCanRecordingEvent && eventPresenter2.ParentId != 0uL)
			{
				return false;
			}
			if (eventPresenter2.Event is DummyEvent && dropPos != TriggerTree.EnumDropPosition.AsSiblingBefore)
			{
				return false;
			}
			int index = this.GetIndex(sourceEventId);
			int index2;
			switch (dropPos)
			{
			case TriggerTree.EnumDropPosition.AsSiblingBefore:
				index2 = this.GetIndex(this.GetPreviousSiblingOf(targetEventId));
				break;
			case TriggerTree.EnumDropPosition.AsSiblingAfter:
				index2 = this.GetIndex(this.GetNextSiblingOf(targetEventId));
				break;
			default:
				return false;
			}
			return index2 != index;
		}

		private bool CanMoveSelectedRow(TriggerTree.EnumRowMoveAction action)
		{
			ulong num;
			ulong num2;
			TriggerTree.EnumDropPosition enumDropPosition;
			return this.CanMoveSelectedRow(action, out num, out num2, out enumDropPosition);
		}

		private bool CanMoveSelectedRow(TriggerTree.EnumRowMoveAction action, out ulong selectedEventId, out ulong targetEventId, out TriggerTree.EnumDropPosition dropPos)
		{
			selectedEventId = this.GetSelectedEventId();
			targetEventId = 0uL;
			dropPos = TriggerTree.EnumDropPosition.None;
			if (selectedEventId == 0uL)
			{
				return false;
			}
			switch (action)
			{
			case TriggerTree.EnumRowMoveAction.Top:
				targetEventId = this.GetFirstSiblingOfSelected();
				dropPos = TriggerTree.EnumDropPosition.AsSiblingBefore;
				break;
			case TriggerTree.EnumRowMoveAction.Up:
				targetEventId = this.GetPreviousSiblingOfSelected();
				dropPos = TriggerTree.EnumDropPosition.AsSiblingBefore;
				break;
			case TriggerTree.EnumRowMoveAction.Down:
				targetEventId = this.GetNextSiblingOfSelected();
				dropPos = TriggerTree.EnumDropPosition.AsSiblingAfter;
				break;
			case TriggerTree.EnumRowMoveAction.Bottom:
				targetEventId = this.GetLastSiblingOfSelected();
				dropPos = TriggerTree.EnumDropPosition.AsSiblingAfter;
				break;
			default:
				return false;
			}
			return this.CanMoveRow(selectedEventId, targetEventId, dropPos);
		}

		private void MoveSelectedRow(TriggerTree.EnumRowMoveAction action)
		{
			ulong sourceEventId;
			ulong targetEventId;
			TriggerTree.EnumDropPosition dropPos;
			if (!this.CanMoveSelectedRow(action, out sourceEventId, out targetEventId, out dropPos))
			{
				return;
			}
			this.MoveRow(sourceEventId, targetEventId, dropPos);
		}

		private ulong GetFirstSiblingOfSelected()
		{
			TreeListNode focusedNode = this.mTreeList.FocusedNode;
			if (focusedNode == null || focusedNode.PrevNode == null)
			{
				return 0uL;
			}
			TreeListNode prevNode = focusedNode.PrevNode;
			while (prevNode.PrevNode != null)
			{
				prevNode = prevNode.PrevNode;
			}
			EventPresenter presenter = this.GetPresenter(prevNode);
			if (presenter == null)
			{
				return 0uL;
			}
			return presenter.Id;
		}

		private ulong GetPreviousSiblingOfSelected()
		{
			TreeListNode focusedNode = this.mTreeList.FocusedNode;
			EventPresenter eventPresenter = (focusedNode != null) ? this.GetPresenter(focusedNode.PrevNode) : null;
			if (eventPresenter == null)
			{
				return 0uL;
			}
			return eventPresenter.Id;
		}

		private ulong GetPreviousSiblingOf(ulong eventId)
		{
			TreeListNode treeListNode = this.mTreeList.FindNodeByKeyID(eventId);
			EventPresenter eventPresenter = (treeListNode != null) ? this.GetPresenter(treeListNode.PrevNode) : null;
			if (eventPresenter == null)
			{
				return 0uL;
			}
			return eventPresenter.Id;
		}

		private ulong GetNextSiblingOfSelected()
		{
			TreeListNode focusedNode = this.mTreeList.FocusedNode;
			EventPresenter eventPresenter = (focusedNode != null) ? this.GetPresenter(focusedNode.NextNode) : null;
			if (eventPresenter == null)
			{
				return 0uL;
			}
			return eventPresenter.Id;
		}

		private ulong GetNextSiblingOf(ulong eventId)
		{
			TreeListNode treeListNode = this.mTreeList.FindNodeByKeyID(eventId);
			EventPresenter eventPresenter = (treeListNode != null) ? this.GetPresenter(treeListNode.NextNode) : null;
			if (eventPresenter == null)
			{
				return 0uL;
			}
			return eventPresenter.Id;
		}

		private ulong GetLastSiblingOfSelected()
		{
			TreeListNode focusedNode = this.mTreeList.FocusedNode;
			if (focusedNode == null || focusedNode.NextNode == null)
			{
				return 0uL;
			}
			TreeListNode nextNode = focusedNode.NextNode;
			while (nextNode.NextNode != null)
			{
				nextNode = nextNode.NextNode;
			}
			EventPresenter presenter = this.GetPresenter(nextNode);
			if (presenter == null)
			{
				return 0uL;
			}
			return presenter.Id;
		}

		private void UpdateUpDownButtons()
		{
			this.mButtonMoveFirst.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Top);
			this.mButtonMoveUp.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Up);
			this.mButtonMoveDown.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Down);
			this.mButtonMoveLast.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Bottom);
		}

		private void ButtonMoveFirst_Click(object sender, EventArgs e)
		{
			this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Top);
		}

		private void ButtonMoveUp_Click(object sender, EventArgs e)
		{
			this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Up);
		}

		private void ButtonMoveDown_Click(object sender, EventArgs e)
		{
			this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Down);
		}

		private void ButtonMoveLast_Click(object sender, EventArgs e)
		{
			this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Bottom);
		}

		private void UpdateContextMenuStrip()
		{
			EventPresenter eventPresenter;
			bool visible = this.TryGetSelectedPresenter(out eventPresenter) && eventPresenter.ParentId == 0uL && this.mTreeList.FocusedColumn == this.mColName;
			this.mToolStripMenuItemResetToDefault.Visible = visible;
			this.mToolStripSeparator1.Visible = visible;
			RecordTriggerPresenter recordTriggerPresenter = eventPresenter as RecordTriggerPresenter;
			this.mToolStripMenuItemResetToDefault.Enabled = (recordTriggerPresenter != null && !recordTriggerPresenter.HasDefaultName);
			this.mToolStripMenuItemMoveTop.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Top);
			this.mToolStripMenuItemMoveUp.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Up);
			this.mToolStripMenuItemMoveDown.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Down);
			this.mToolStripMenuItemMoveBottom.Enabled = this.CanMoveSelectedRow(TriggerTree.EnumRowMoveAction.Bottom);
			this.mToolStripMenuItemRemove.Enabled = this.CanRemoveSelection;
			this.mToolStripMenuItemExpandAll.Enabled = this.CanExpand;
			this.mToolStripMenuItemCollapseAll.Enabled = this.CanCollapse;
		}

		private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			this.mContextMenuStrip.Hide();
			if (e.ClickedItem == this.mToolStripMenuItemResetToDefault)
			{
				EventPresenter eventPresenter;
				if (!this.TryGetSelectedPresenter(out eventPresenter))
				{
					return;
				}
				RecordTriggerPresenter recordTriggerPresenter = eventPresenter as RecordTriggerPresenter;
				if (recordTriggerPresenter == null)
				{
					return;
				}
				recordTriggerPresenter.Name = recordTriggerPresenter.DefaultName;
				return;
			}
			else
			{
				if (e.ClickedItem == this.mToolStripMenuItemMoveTop)
				{
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Top);
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemMoveUp)
				{
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Up);
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemMoveDown)
				{
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Down);
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemMoveBottom)
				{
					this.MoveSelectedRow(TriggerTree.EnumRowMoveAction.Bottom);
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemRemove)
				{
					this.RemoveSelection();
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemExpandAll)
				{
					this.ExpandAll();
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemCollapseAll)
				{
					this.CollapseAll();
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemCopy)
				{
					ConfigClipboardManager.Copy();
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemPaste)
				{
					ConfigClipboardManager.Paste();
					return;
				}
				if (e.ClickedItem == this.mToolStripMenuItemCut)
				{
					ConfigClipboardManager.Copy();
					this.RemoveSelection();
				}
				return;
			}
		}

		private void TreeList_AfterCollapse(object sender, NodeEventArgs e)
		{
			this.UpdateUpDownButtons();
			this.UpdateContextMenuStrip();
		}

		private void TreeList_AfterExpand(object sender, NodeEventArgs e)
		{
			this.UpdateUpDownButtons();
			this.UpdateContextMenuStrip();
			this.DisplayErrors();
		}

		private void UpdateCopyAndPasteControls()
		{
			EventPresenter eventPresenter;
			this.mToolStripMenuItemCopy.Enabled = (this.TryGetSelectedPresenter(out eventPresenter) && (eventPresenter is RecordTriggerPresenter || (eventPresenter != null && eventPresenter.Event != null && !(eventPresenter.Event is DummyEvent))));
			this.mToolStripMenuItemCut.Enabled = this.mToolStripMenuItemCopy.Enabled;
			this.mToolStripMenuItemPaste.Enabled = ConfigClipboardManager.PasteApplicable;
		}

		private void mContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			this.UpdateCopyAndPasteControls();
		}

		private void PrepareDragOnMouseDown(TreeListHitInfo hitInfo, MouseEventArgs e)
		{
			if (hitInfo == null || hitInfo.Node == null)
			{
				return;
			}
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			ulong num;
			if (!this.TryGetEventId(hitInfo.Node, out num))
			{
				return;
			}
			this.mDragHitInfo = hitInfo;
			this.mTreeList.OptionsBehavior.DragNodes = false;
		}

		private void CancelDragStart()
		{
			this.mDragHitInfo = null;
			this.mTreeList.OptionsBehavior.DragNodes = true;
			this.SetTreeDropPosition(null, TriggerTree.EnumDropPosition.None);
		}

		private void DragStart(MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || this.mDragHitInfo == null)
			{
				return;
			}
			Size dragSize = SystemInformation.DragSize;
			Rectangle rectangle = new Rectangle(new Point(this.mDragHitInfo.MousePoint.X - dragSize.Width / 2, this.mDragHitInfo.MousePoint.Y - dragSize.Height / 2), dragSize);
			if (rectangle.Contains(e.Location))
			{
				return;
			}
			ulong num;
			if (!this.TryGetEventId(this.mDragHitInfo.Node, out num))
			{
				return;
			}
			this.mTreeList.OptionsBehavior.DragNodes = true;
			this.mTreeList.DoDragDrop(num, DragDropEffects.Move);
			this.mDragHitInfo = null;
		}

		private void TreeList_DragOver(object sender, DragEventArgs e)
		{
			this.mTreeList.PostEditor();
			this.mTreeList.CloseEditor();
			e.Effect = DragDropEffects.None;
			TriggerTree.EnumDropPosition dropPosition = TriggerTree.EnumDropPosition.None;
			TreeListHitInfo treeListHitInfo = this.CalcDropTargetHitInfo();
			if (treeListHitInfo != null && e.Data.GetDataPresent(typeof(ulong)))
			{
				ulong dropData = (ulong)e.Data.GetData(typeof(ulong));
				ulong num;
				e.Effect = this.GetDragDropEffect(treeListHitInfo, dropData, out num, out dropPosition);
			}
			this.SetTreeDropPosition(treeListHitInfo, dropPosition);
			if (e.Effect != DragDropEffects.None)
			{
				base.ActiveControl = this.mTreeList;
			}
		}

		private void TreeList_DragDrop(object sender, DragEventArgs e)
		{
			TreeListHitInfo treeListHitInfo = this.CalcDropTargetHitInfo();
			if (treeListHitInfo == null || !e.Data.GetDataPresent(typeof(ulong)))
			{
				return;
			}
			ulong num = (ulong)e.Data.GetData(typeof(ulong));
			if (num < 1uL)
			{
				return;
			}
			ulong targetEventId;
			TriggerTree.EnumDropPosition dropPos;
			e.Effect = this.GetDragDropEffect(treeListHitInfo, num, out targetEventId, out dropPos);
			if (e.Effect == DragDropEffects.None)
			{
				return;
			}
			this.MoveRow(num, targetEventId, dropPos);
		}

		private TreeListHitInfo CalcDropTargetHitInfo()
		{
			Point pt = this.mTreeList.PointToClient(Control.MousePosition);
			TreeListHitInfo treeListHitInfo = this.mTreeList.CalcHitInfo(pt);
			if (treeListHitInfo == null || treeListHitInfo.Node != null || treeListHitInfo.Column != null || !this.mBindingList.Any<EventPresenter>())
			{
				return treeListHitInfo;
			}
			RecordTrigger recordTrigger = this.mTriggerConfiguration.CurrentList.Last<RecordTrigger>();
			if (recordTrigger == null || !this.mPresenterMap.ContainsKey(recordTrigger.Event.Id))
			{
				return treeListHitInfo;
			}
			TreeListNode nodeByDataRecord = this.GetNodeByDataRecord(this.mPresenterMap[recordTrigger.Event.Id]);
			RowInfo rowInfo = this.mTreeList.ViewInfo.RowsInfo[nodeByDataRecord];
			if (pt.X < rowInfo.CheckBounds.Left)
			{
				return treeListHitInfo;
			}
			Point pt2 = new Point(pt.X, rowInfo.Bounds.Bottom - 1);
			return this.mTreeList.CalcHitInfo(pt2);
		}

		private DragDropEffects GetDragDropEffect(TreeListHitInfo hitInfo, ulong dropData, out ulong targetEventId, out TriggerTree.EnumDropPosition dropPos)
		{
			targetEventId = 0uL;
			dropPos = TriggerTree.EnumDropPosition.None;
			if (hitInfo == null || !this.TryGetEventId(hitInfo.Node, out targetEventId))
			{
				return DragDropEffects.None;
			}
			dropPos = this.GetDropPosition(hitInfo, dropData, targetEventId);
			if (dropPos != TriggerTree.EnumDropPosition.None)
			{
				return DragDropEffects.Move;
			}
			return DragDropEffects.None;
		}

		private TriggerTree.EnumDropPosition GetDropPosition(TreeListHitInfo hitInfo, ulong dropData, ulong targetEventId)
		{
			IEnumerable<TriggerTree.EnumDropPosition> potentialDropPositions = this.GetPotentialDropPositions(hitInfo);
			foreach (TriggerTree.EnumDropPosition current in potentialDropPositions)
			{
				if (this.CanMoveRow(dropData, targetEventId, current))
				{
					return current;
				}
			}
			return TriggerTree.EnumDropPosition.None;
		}

		private IEnumerable<TriggerTree.EnumDropPosition> GetPotentialDropPositions(TreeListHitInfo hitInfo)
		{
			List<TriggerTree.EnumDropPosition> list = new List<TriggerTree.EnumDropPosition>();
			if (hitInfo == null || hitInfo.Node == null)
			{
				return list;
			}
			RowInfo rowInfo = this.mTreeList.ViewInfo.RowsInfo[hitInfo.Node];
			bool flag = hitInfo.MousePoint.Y < rowInfo.Bounds.Y + rowInfo.Bounds.Height / 2;
			bool flag2 = hitInfo.MousePoint.Y >= rowInfo.Bounds.Y + rowInfo.Bounds.Height / 2;
			if (flag)
			{
				list.Add(TriggerTree.EnumDropPosition.AsSiblingBefore);
				list.Add(TriggerTree.EnumDropPosition.AsSiblingAfter);
			}
			else if (flag2)
			{
				list.Add(TriggerTree.EnumDropPosition.AsSiblingAfter);
				list.Add(TriggerTree.EnumDropPosition.AsSiblingBefore);
			}
			return list;
		}

		private void TreeList_CalcNodeDragImageIndex(object sender, CalcNodeDragImageIndexEventArgs e)
		{
			e.ImageIndex = -1;
		}

		private void SetTreeDropPosition(TreeListHitInfo hitInfo, TriggerTree.EnumDropPosition dropPosition)
		{
			Point point = new Point(0, 0);
			Point point2 = new Point(0, 0);
			if (hitInfo != null && hitInfo.Node != null)
			{
				RowInfo rowInfo = this.mTreeList.ViewInfo.RowsInfo[hitInfo.Node];
				int x = rowInfo.Bounds.Left + rowInfo.IndentInfo.LevelWidth * rowInfo.Level;
				int right = rowInfo.Bounds.Right;
				switch (dropPosition)
				{
				case TriggerTree.EnumDropPosition.AsSiblingBefore:
				{
					int y = rowInfo.Bounds.Top - 1;
					point = new Point(x, y);
					point2 = new Point(right, y);
					break;
				}
				case TriggerTree.EnumDropPosition.AsSiblingAfter:
				{
					EventPresenter eventPresenter;
					if (this.TryGetPresenter(hitInfo.Node, out eventPresenter))
					{
						ReadOnlyCollection<EventPresenter> allChildPresenters = eventPresenter.GetAllChildPresenters(true);
						TreeListNode treeListNode = (hitInfo.Node.Expanded && allChildPresenters.Any<EventPresenter>()) ? this.mTreeList.FindNodeByKeyID(allChildPresenters.Last<EventPresenter>().Id) : null;
						RowInfo rowInfo2 = (treeListNode != null) ? this.mTreeList.ViewInfo.RowsInfo[treeListNode] : rowInfo;
						int y2 = rowInfo2.Bounds.Bottom - 1;
						point = new Point(x, y2);
						point2 = new Point(right, y2);
					}
					break;
				}
				}
			}
			this.mDropLineRefreshRequested = (!point.Equals(this.mDropLineLeft) || !point2.Equals(this.mDropLineRight));
			this.mDropLineLeft = point;
			this.mDropLineRight = point2;
			this.DrawTreeDropPosition();
		}

		private void DrawTreeDropPosition()
		{
			if (this.mDropLineRefreshRequested)
			{
				this.mDropLineRefreshRequested = false;
				this.mTreeList.Refresh();
			}
			if (this.mDropLineLeft.Equals(this.mDropLineRight))
			{
				return;
			}
			Graphics graphics = this.mTreeList.CreateGraphics();
			Pen pen = new Pen(this.mDropLineColor, 2f);
			Brush brush = new SolidBrush(this.mDropLineColor);
			Point pt = new Point(this.mDropLineLeft.X + 3, this.mDropLineLeft.Y);
			Point pt2 = new Point(this.mDropLineRight.X - 5, this.mDropLineRight.Y);
			Point[] points = new Point[]
			{
				new Point(pt.X, pt.Y - 4 - 1),
				new Point(pt.X + 4, pt.Y),
				new Point(pt.X, pt.Y + 4)
			};
			Point[] points2 = new Point[]
			{
				new Point(pt2.X + 1, pt2.Y - 4 - 1),
				new Point(pt2.X + 1 - 4 - 1, pt2.Y),
				new Point(pt2.X + 1, pt2.Y + 4)
			};
			graphics.DrawLine(pen, pt, pt2);
			graphics.FillPolygon(brush, points);
			graphics.FillPolygon(brush, points2);
		}

		public bool Serialize(TriggersPage triggersPage)
		{
			if (triggersPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.mTreeList.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				triggersPage.TriggersTreeLayout = Convert.ToBase64String(array, 0, array.Length);
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return result;
		}

		public bool DeSerialize(TriggersPage triggersPage)
		{
			if (triggersPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(triggersPage.TriggersTreeLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(triggersPage.TriggersTreeLayout);
				memoryStream = new MemoryStream(buffer);
				Dictionary<TreeListColumn, string> dictionary = new Dictionary<TreeListColumn, string>();
				foreach (TreeListColumn treeListColumn in this.mTreeList.Columns)
				{
					dictionary.Add(treeListColumn, treeListColumn.Caption);
				}
				this.mTreeList.RestoreLayoutFromStream(memoryStream);
				foreach (TreeListColumn current in dictionary.Keys)
				{
					current.Caption = dictionary[current];
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return result;
		}

		private void XtraToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			TreeListHitInfo treeListHitInfo = this.mTreeList.CalcHitInfo(e.ControlMousePosition);
			if (treeListHitInfo == null)
			{
				return;
			}
			EventPresenter eventPresenter;
			if (!this.TryGetPresenter(treeListHitInfo.Node, out eventPresenter))
			{
				return;
			}
			string text;
			MainImageList.IconIndex baseImgIndex;
			bool error = eventPresenter.GetError(out text, out baseImgIndex);
			if (error && treeListHitInfo.HitInfoType == HitInfoType.SelectImage)
			{
				e.Info = new ToolTipControlInfo(treeListHitInfo.Node, text)
				{
					ToolTipImage = MainImageList.Instance.GetImage(baseImgIndex)
				};
			}
			if (!error && treeListHitInfo.HitInfoType == HitInfoType.Cell && treeListHitInfo.Column == this.mColCondition)
			{
				IncEvent incEvent = eventPresenter.Event as IncEvent;
				if (incEvent != null)
				{
					IncludeFileParameterPresenter includeFileParameterPresenter;
					if (IncludeFileManager.Instance.TryGetOutParameter(incEvent, out includeFileParameterPresenter, false))
					{
						IncludeFileParameterPresenter instanceParameter = includeFileParameterPresenter.Parent.InstanceParameter;
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(incEvent.FilePath.Value);
						if (instanceParameter != null && !string.IsNullOrEmpty(instanceParameter.Value))
						{
							stringBuilder.Append(" (");
							stringBuilder.Append(instanceParameter.Value);
							stringBuilder.Append(")");
						}
						stringBuilder.AppendLine();
						e.Info = new ToolTipControlInfo(treeListHitInfo.Node, stringBuilder.ToString(), eventPresenter.Condition);
						return;
					}
				}
				else if (eventPresenter != null && eventPresenter.Event != null && eventPresenter.Event is DiagnosticSignalEvent)
				{
					DiagnosticSignalEvent diagnosticSignalEvent = eventPresenter.Event as DiagnosticSignalEvent;
					string text2 = Environment.NewLine + string.Format(Resources.SymbolicSignalName, diagnosticSignalEvent.DiagnosticEcuName.Value, diagnosticSignalEvent.DiagnosticServiceName.Value);
					text2 = text2 + Environment.NewLine + this.GetParameterDisplayStrings(diagnosticSignalEvent);
					e.Info = new ToolTipControlInfo(treeListHitInfo.Node, text2, eventPresenter.Condition);
				}
			}
		}

		private string GetParameterDisplayStrings(DiagnosticSignalEvent signal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string variantQualifier;
			IList<KeyValuePair<string, string>> list;
			if (this.ModelValidator.IsDiagECUConfigured(signal.DatabasePath.Value, signal.DiagnosticEcuName.Value, out variantQualifier) && DiagSymbolsManager.Instance().GetDisassembledMessageParams(this.ModelValidator.GetAbsoluteFilePath(signal.DatabasePath.Value), signal.DiagnosticEcuName.Value, variantQualifier, signal.DiagnosticServiceName.Value, signal.DiagnosticMessageData.Value, out list))
			{
				int num = 0;
				foreach (KeyValuePair<string, string> current in list)
				{
					if (num != 0)
					{
						if (num > 1)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.AppendFormat("{0} = {1}", current.Key, current.Value);
					}
					num++;
				}
				return stringBuilder.ToString();
			}
			return string.Empty;
		}

		protected override void Dispose(bool disposing)
		{
			this.OnDispose(disposing);
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TriggerTree));
			this.mTreeList = new TreeList();
			this.mColEvent = new TreeListColumn();
			this.mColChannel = new TreeListColumn();
			this.mRepositoryItemComboBoxChannel = new RepositoryItemComboBox();
			this.mColCondition = new TreeListColumn();
			this.mRepositoryItemButtonEditCondition = new RepositoryItemButtonEdit();
			this.mColTriggerType = new TreeListColumn();
			this.mRepositoryItemComboBoxTriggerEffect = new RepositoryItemComboBox();
			this.mColAction = new TreeListColumn();
			this.mRepositoryItemComboBoxAction = new RepositoryItemComboBox();
			this.mColName = new TreeListColumn();
			this.mRepositoryItemTextEditTriggerName = new RepositoryItemTextEdit();
			this.mColComment = new TreeListColumn();
			this.mRepositoryItemMemoExEditComment = new RepositoryItemMemoExEdit();
			this.mContextMenuStrip = new ContextMenuStrip(this.components);
			this.mToolStripMenuItemResetToDefault = new ToolStripMenuItem();
			this.mToolStripSeparator1 = new ToolStripSeparator();
			this.mToolStripMenuItemMoveTop = new ToolStripMenuItem();
			this.mToolStripMenuItemMoveUp = new ToolStripMenuItem();
			this.mToolStripMenuItemMoveDown = new ToolStripMenuItem();
			this.mToolStripMenuItemMoveBottom = new ToolStripMenuItem();
			this.mToolStripSeparator2 = new ToolStripSeparator();
			this.mToolStripMenuItemRemove = new ToolStripMenuItem();
			this.mToolStripMenuItemCut = new ToolStripMenuItem();
			this.mToolStripMenuItemCopy = new ToolStripMenuItem();
			this.mToolStripMenuItemPaste = new ToolStripMenuItem();
			this.mToolStripSeparator3 = new ToolStripSeparator();
			this.mToolStripMenuItemExpandAll = new ToolStripMenuItem();
			this.mToolStripMenuItemCollapseAll = new ToolStripMenuItem();
			this.mRepositoryItemCheckEditIsActive = new RepositoryItemCheckEdit();
			this.mRepositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.mXtraToolTipController = new XtraToolTipController(this.components);
			this.mButtonMoveLast = new Button();
			this.mButtonMoveDown = new Button();
			this.mButtonMoveUp = new Button();
			this.mButtonMoveFirst = new Button();
			this.mErrorProviderLocalModel = new ErrorProvider(this.components);
			this.mErrorProviderFormat = new ErrorProvider(this.components);
			this.mErrorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.mTreeList).BeginInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxChannel).BeginInit();
			((ISupportInitialize)this.mRepositoryItemButtonEditCondition).BeginInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxTriggerEffect).BeginInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxAction).BeginInit();
			((ISupportInitialize)this.mRepositoryItemTextEditTriggerName).BeginInit();
			((ISupportInitialize)this.mRepositoryItemMemoExEditComment).BeginInit();
			this.mContextMenuStrip.SuspendLayout();
			((ISupportInitialize)this.mRepositoryItemCheckEditIsActive).BeginInit();
			((ISupportInitialize)this.mRepositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.mErrorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.mErrorProviderFormat).BeginInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.mTreeList.AllowDrop = true;
			componentResourceManager.ApplyResources(this.mTreeList, "mTreeList");
			this.mTreeList.Columns.AddRange(new TreeListColumn[]
			{
				this.mColEvent,
				this.mColChannel,
				this.mColCondition,
				this.mColTriggerType,
				this.mColAction,
				this.mColName,
				this.mColComment
			});
			this.mTreeList.ContextMenuStrip = this.mContextMenuStrip;
			this.mTreeList.DragNodesMode = TreeListDragNodesMode.Standard;
			this.mTreeList.KeyFieldName = "Id";
			this.mTreeList.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.mTreeList.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mTreeList.LookAndFeel.UseWindowsXPTheme = true;
			this.mTreeList.Name = "mTreeList";
			this.mTreeList.OptionsBehavior.AutoMoveRowFocus = true;
			this.mTreeList.OptionsBehavior.AutoSelectAllInEditor = false;
			this.mTreeList.OptionsBehavior.CloseEditorOnLostFocus = false;
			this.mTreeList.OptionsBehavior.DragNodes = true;
			this.mTreeList.OptionsBehavior.MoveOnEdit = false;
			this.mTreeList.OptionsBehavior.ResizeNodes = false;
			this.mTreeList.OptionsLayout.RemoveOldColumns = true;
			this.mTreeList.OptionsView.ShowCheckBoxes = true;
			this.mTreeList.OptionsView.ShowIndicator = false;
			this.mTreeList.ParentFieldName = "ParentId";
			this.mTreeList.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.mRepositoryItemComboBoxTriggerEffect,
				this.mRepositoryItemCheckEditIsActive,
				this.mRepositoryItemComboBoxChannel,
				this.mRepositoryItemComboBoxAction,
				this.mRepositoryItemMemoExEditComment,
				this.mRepositoryItemTextEditTriggerName,
				this.mRepositoryItemTextEditDummy,
				this.mRepositoryItemButtonEditCondition
			});
			this.mTreeList.ToolTipController = this.mXtraToolTipController;
			this.mTreeList.CustomNodeCellEdit += new GetCustomNodeCellEditEventHandler(this.TreeList_CustomNodeCellEdit);
			this.mTreeList.AfterExpand += new NodeEventHandler(this.TreeList_AfterExpand);
			this.mTreeList.AfterCollapse += new NodeEventHandler(this.TreeList_AfterCollapse);
			this.mTreeList.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this.TreeList_FocusedNodeChanged);
			this.mTreeList.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this.TreeList_FocusedColumnChanged);
			this.mTreeList.CalcNodeDragImageIndex += new CalcNodeDragImageIndexEventHandler(this.TreeList_CalcNodeDragImageIndex);
			this.mTreeList.ShownEditor += new EventHandler(this.TreeList_ShownEditor);
			this.mTreeList.CustomDrawNodeCell += new CustomDrawNodeCellEventHandler(this.TreeList_CustomDrawNodeCell);
			this.mTreeList.CustomDrawEmptyArea += new CustomDrawEmptyAreaEventHandler(this.TreeList_CustomDrawEmptyArea);
			this.mTreeList.PopupMenuShowing += new PopupMenuShowingEventHandler(this.TreeList_PopupMenuShowing);
			this.mTreeList.LeftCoordChanged += new EventHandler(this.TreeList_LeftCoordChanged);
			this.mTreeList.TopVisibleNodeIndexChanged += new EventHandler(this.TreeList_TopVisibleNodeIndexChanged);
			this.mTreeList.ShowingEditor += new CancelEventHandler(this.TreeList_ShowingEditor);
			this.mTreeList.EditorKeyDown += new KeyEventHandler(this.TreeList_EditorKeyDown);
			this.mTreeList.DragDrop += new DragEventHandler(this.TreeList_DragDrop);
			this.mTreeList.DragOver += new DragEventHandler(this.TreeList_DragOver);
			this.mTreeList.KeyDown += new KeyEventHandler(this.TreeList_KeyDown);
			this.mTreeList.MouseDown += new MouseEventHandler(this.TreeList_MouseDown);
			this.mTreeList.MouseMove += new MouseEventHandler(this.TreeList_MouseMove);
			this.mTreeList.MouseUp += new MouseEventHandler(this.TreeList_MouseUp);
			componentResourceManager.ApplyResources(this.mColEvent, "mColEvent");
			this.mColEvent.FieldName = "EventStr";
			this.mColEvent.Name = "mColEvent";
			this.mColEvent.OptionsColumn.AllowEdit = false;
			this.mColEvent.OptionsColumn.AllowMove = false;
			this.mColEvent.OptionsColumn.AllowSort = false;
			componentResourceManager.ApplyResources(this.mColChannel, "mColChannel");
			this.mColChannel.ColumnEdit = this.mRepositoryItemComboBoxChannel;
			this.mColChannel.FieldName = "Channel";
			this.mColChannel.Name = "mColChannel";
			this.mColChannel.OptionsColumn.AllowMove = false;
			this.mColChannel.OptionsColumn.AllowSort = false;
			componentResourceManager.ApplyResources(this.mRepositoryItemComboBoxChannel, "mRepositoryItemComboBoxChannel");
			this.mRepositoryItemComboBoxChannel.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemComboBoxChannel.Buttons"))
			});
			this.mRepositoryItemComboBoxChannel.Name = "mRepositoryItemComboBoxChannel";
			this.mRepositoryItemComboBoxChannel.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepositoryItemComboBoxChannel.SelectedValueChanged += new EventHandler(this.RepositoryItemComboBox_SelectedValueChanged);
			componentResourceManager.ApplyResources(this.mColCondition, "mColCondition");
			this.mColCondition.ColumnEdit = this.mRepositoryItemButtonEditCondition;
			this.mColCondition.FieldName = "Condition";
			this.mColCondition.Name = "mColCondition";
			this.mColCondition.OptionsColumn.AllowMove = false;
			this.mColCondition.OptionsColumn.AllowSort = false;
			componentResourceManager.ApplyResources(this.mRepositoryItemButtonEditCondition, "mRepositoryItemButtonEditCondition");
			this.mRepositoryItemButtonEditCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.mRepositoryItemButtonEditCondition.Name = "mRepositoryItemButtonEditCondition";
			this.mRepositoryItemButtonEditCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepositoryItemButtonEditCondition.ButtonClick += new ButtonPressedEventHandler(this.RepositoryItemButtonEditCondition_ButtonClick);
			componentResourceManager.ApplyResources(this.mColTriggerType, "mColTriggerType");
			this.mColTriggerType.ColumnEdit = this.mRepositoryItemComboBoxTriggerEffect;
			this.mColTriggerType.FieldName = "Type";
			this.mColTriggerType.Name = "mColTriggerType";
			this.mColTriggerType.OptionsColumn.AllowMove = false;
			this.mColTriggerType.OptionsColumn.AllowSort = false;
			componentResourceManager.ApplyResources(this.mRepositoryItemComboBoxTriggerEffect, "mRepositoryItemComboBoxTriggerEffect");
			this.mRepositoryItemComboBoxTriggerEffect.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemComboBoxTriggerEffect.Buttons"))
			});
			this.mRepositoryItemComboBoxTriggerEffect.Name = "mRepositoryItemComboBoxTriggerEffect";
			this.mRepositoryItemComboBoxTriggerEffect.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepositoryItemComboBoxTriggerEffect.SelectedValueChanged += new EventHandler(this.RepositoryItemComboBox_SelectedValueChanged);
			componentResourceManager.ApplyResources(this.mColAction, "mColAction");
			this.mColAction.ColumnEdit = this.mRepositoryItemComboBoxAction;
			this.mColAction.FieldName = "Action";
			this.mColAction.Name = "mColAction";
			this.mColAction.OptionsColumn.AllowMove = false;
			this.mColAction.OptionsColumn.AllowSort = false;
			componentResourceManager.ApplyResources(this.mRepositoryItemComboBoxAction, "mRepositoryItemComboBoxAction");
			this.mRepositoryItemComboBoxAction.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemComboBoxAction.Buttons"))
			});
			this.mRepositoryItemComboBoxAction.Name = "mRepositoryItemComboBoxAction";
			this.mRepositoryItemComboBoxAction.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepositoryItemComboBoxAction.SelectedValueChanged += new EventHandler(this.RepositoryItemComboBox_SelectedValueChanged);
			componentResourceManager.ApplyResources(this.mColName, "mColName");
			this.mColName.ColumnEdit = this.mRepositoryItemTextEditTriggerName;
			this.mColName.FieldName = "Name";
			this.mColName.Name = "mColName";
			this.mColName.OptionsColumn.AllowMove = false;
			this.mColName.OptionsColumn.AllowSort = false;
			componentResourceManager.ApplyResources(this.mRepositoryItemTextEditTriggerName, "mRepositoryItemTextEditTriggerName");
			this.mRepositoryItemTextEditTriggerName.Name = "mRepositoryItemTextEditTriggerName";
			componentResourceManager.ApplyResources(this.mColComment, "mColComment");
			this.mColComment.ColumnEdit = this.mRepositoryItemMemoExEditComment;
			this.mColComment.FieldName = "Comment";
			this.mColComment.Name = "mColComment";
			this.mColComment.OptionsColumn.AllowMove = false;
			this.mColComment.OptionsColumn.AllowSort = false;
			this.mRepositoryItemMemoExEditComment.AcceptsReturn = false;
			componentResourceManager.ApplyResources(this.mRepositoryItemMemoExEditComment, "mRepositoryItemMemoExEditComment");
			this.mRepositoryItemMemoExEditComment.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemMemoExEditComment.Buttons"))
			});
			this.mRepositoryItemMemoExEditComment.MaxLength = 8190;
			this.mRepositoryItemMemoExEditComment.Name = "mRepositoryItemMemoExEditComment";
			this.mRepositoryItemMemoExEditComment.ShowIcon = false;
			this.mRepositoryItemMemoExEditComment.ValidateOnEnterKey = true;
			this.mContextMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.mToolStripMenuItemResetToDefault,
				this.mToolStripSeparator1,
				this.mToolStripMenuItemMoveTop,
				this.mToolStripMenuItemMoveUp,
				this.mToolStripMenuItemMoveDown,
				this.mToolStripMenuItemMoveBottom,
				this.mToolStripSeparator2,
				this.mToolStripMenuItemRemove,
				this.mToolStripMenuItemCut,
				this.mToolStripMenuItemCopy,
				this.mToolStripMenuItemPaste,
				this.mToolStripSeparator3,
				this.mToolStripMenuItemExpandAll,
				this.mToolStripMenuItemCollapseAll
			});
			this.mContextMenuStrip.Name = "mContextMenu";
			componentResourceManager.ApplyResources(this.mContextMenuStrip, "mContextMenuStrip");
			this.mContextMenuStrip.Opening += new CancelEventHandler(this.mContextMenuStrip_Opening);
			this.mContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.ContextMenuStrip_ItemClicked);
			this.mToolStripMenuItemResetToDefault.Name = "mToolStripMenuItemResetToDefault";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemResetToDefault, "mToolStripMenuItemResetToDefault");
			this.mToolStripSeparator1.Name = "mToolStripSeparator1";
			componentResourceManager.ApplyResources(this.mToolStripSeparator1, "mToolStripSeparator1");
			this.mToolStripMenuItemMoveTop.Image = Resources.ImageMoveFirst;
			this.mToolStripMenuItemMoveTop.Name = "mToolStripMenuItemMoveTop";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveTop, "mToolStripMenuItemMoveTop");
			this.mToolStripMenuItemMoveUp.Image = Resources.ImageMovePrev;
			this.mToolStripMenuItemMoveUp.Name = "mToolStripMenuItemMoveUp";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveUp, "mToolStripMenuItemMoveUp");
			this.mToolStripMenuItemMoveDown.Image = Resources.ImageMoveNext;
			this.mToolStripMenuItemMoveDown.Name = "mToolStripMenuItemMoveDown";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveDown, "mToolStripMenuItemMoveDown");
			this.mToolStripMenuItemMoveBottom.Image = Resources.ImageMoveLast;
			this.mToolStripMenuItemMoveBottom.Name = "mToolStripMenuItemMoveBottom";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveBottom, "mToolStripMenuItemMoveBottom");
			this.mToolStripSeparator2.Name = "mToolStripSeparator2";
			componentResourceManager.ApplyResources(this.mToolStripSeparator2, "mToolStripSeparator2");
			this.mToolStripMenuItemRemove.Image = Resources.ImageDelete;
			this.mToolStripMenuItemRemove.Name = "mToolStripMenuItemRemove";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemRemove, "mToolStripMenuItemRemove");
			this.mToolStripMenuItemCut.Name = "mToolStripMenuItemCut";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemCut, "mToolStripMenuItemCut");
			this.mToolStripMenuItemCopy.Name = "mToolStripMenuItemCopy";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemCopy, "mToolStripMenuItemCopy");
			this.mToolStripMenuItemPaste.Name = "mToolStripMenuItemPaste";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemPaste, "mToolStripMenuItemPaste");
			this.mToolStripSeparator3.Name = "mToolStripSeparator3";
			componentResourceManager.ApplyResources(this.mToolStripSeparator3, "mToolStripSeparator3");
			this.mToolStripMenuItemExpandAll.Image = Resources.ImageExpandAll;
			this.mToolStripMenuItemExpandAll.Name = "mToolStripMenuItemExpandAll";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemExpandAll, "mToolStripMenuItemExpandAll");
			this.mToolStripMenuItemCollapseAll.Image = Resources.ImageCollapseAll;
			this.mToolStripMenuItemCollapseAll.Name = "mToolStripMenuItemCollapseAll";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemCollapseAll, "mToolStripMenuItemCollapseAll");
			componentResourceManager.ApplyResources(this.mRepositoryItemCheckEditIsActive, "mRepositoryItemCheckEditIsActive");
			this.mRepositoryItemCheckEditIsActive.Name = "mRepositoryItemCheckEditIsActive";
			componentResourceManager.ApplyResources(this.mRepositoryItemTextEditDummy, "mRepositoryItemTextEditDummy");
			this.mRepositoryItemTextEditDummy.Name = "mRepositoryItemTextEditDummy";
			this.mXtraToolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.BackColor");
			this.mXtraToolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.ForeColor");
			this.mXtraToolTipController.Appearance.Options.UseBackColor = true;
			this.mXtraToolTipController.Appearance.Options.UseForeColor = true;
			this.mXtraToolTipController.Appearance.Options.UseTextOptions = true;
			this.mXtraToolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.mXtraToolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.BackColor");
			this.mXtraToolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.ForeColor");
			this.mXtraToolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.mXtraToolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.mXtraToolTipController.MaxWidth = 500;
			this.mXtraToolTipController.ShowPrefix = true;
			this.mXtraToolTipController.UseNativeLookAndFeel = true;
			this.mXtraToolTipController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(this.XtraToolTipController_GetActiveObjectInfo);
			componentResourceManager.ApplyResources(this.mButtonMoveLast, "mButtonMoveLast");
			this.mButtonMoveLast.Image = Resources.ImageMoveLast;
			this.mButtonMoveLast.Name = "mButtonMoveLast";
			this.mButtonMoveLast.UseVisualStyleBackColor = true;
			this.mButtonMoveLast.Click += new EventHandler(this.ButtonMoveLast_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveDown, "mButtonMoveDown");
			this.mButtonMoveDown.Image = Resources.ImageMoveNext;
			this.mButtonMoveDown.Name = "mButtonMoveDown";
			this.mButtonMoveDown.UseVisualStyleBackColor = true;
			this.mButtonMoveDown.Click += new EventHandler(this.ButtonMoveDown_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveUp, "mButtonMoveUp");
			this.mButtonMoveUp.Image = Resources.ImageMovePrev;
			this.mButtonMoveUp.Name = "mButtonMoveUp";
			this.mButtonMoveUp.UseVisualStyleBackColor = true;
			this.mButtonMoveUp.Click += new EventHandler(this.ButtonMoveUp_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveFirst, "mButtonMoveFirst");
			this.mButtonMoveFirst.Image = Resources.ImageMoveFirst;
			this.mButtonMoveFirst.Name = "mButtonMoveFirst";
			this.mButtonMoveFirst.UseVisualStyleBackColor = true;
			this.mButtonMoveFirst.Click += new EventHandler(this.ButtonMoveFirst_Click);
			this.mErrorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderLocalModel.ContainerControl = this;
			this.mErrorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderFormat.ContainerControl = this;
			this.mErrorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.mErrorProviderGlobalModel, "mErrorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.mButtonMoveLast);
			base.Controls.Add(this.mButtonMoveDown);
			base.Controls.Add(this.mButtonMoveUp);
			base.Controls.Add(this.mButtonMoveFirst);
			base.Controls.Add(this.mTreeList);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "TriggerTree";
			base.Resize += new EventHandler(this.TriggerTree_Resize);
			((ISupportInitialize)this.mTreeList).EndInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxChannel).EndInit();
			((ISupportInitialize)this.mRepositoryItemButtonEditCondition).EndInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxTriggerEffect).EndInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxAction).EndInit();
			((ISupportInitialize)this.mRepositoryItemTextEditTriggerName).EndInit();
			((ISupportInitialize)this.mRepositoryItemMemoExEditComment).EndInit();
			this.mContextMenuStrip.ResumeLayout(false);
			((ISupportInitialize)this.mRepositoryItemCheckEditIsActive).EndInit();
			((ISupportInitialize)this.mRepositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.mErrorProviderLocalModel).EndInit();
			((ISupportInitialize)this.mErrorProviderFormat).EndInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
