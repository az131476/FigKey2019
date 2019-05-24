using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.McModule;
using Vector.UtilityFunctions;
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CcpXcpSignalRequestsPage
{
	public class CcpXcpSignalRequestsGrid : UserControl
	{
		public enum SignalExport
		{
			All,
			Active,
			Selected
		}

		private DatabaseConfiguration databaseConfiguration;

		private CcpXcpSignalConfiguration signalConfiguration;

		private Dictionary<CcpXcpSignal, uint> signalIconMap;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private KeyboardNavigationService keyboardService;

		private MultiSelectEditService multiSelectEditService;

		private readonly GeneralService mXtraGridService;

		private int mMultiEditCounter;

		private bool mMultiEditActive;

		private bool isGroupRowSelected;

		private bool isExpandingLocked;

		private Dictionary<ActionCcpXcp, bool> expandStates;

		private GridHitInfo downHitInfo;

		private static readonly int iconSize = 16;

		private IContainer components;

		private GridControl gridControlCcpXcpSignalRequests;

		private GridView gridViewCcpXcpSignalRequests;

		private BindingSource ccpXcpDatabaseBindingSource;

		private RepositoryItemComboBox repositoryItemDaqEventsComboBox;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsEnabled;

		private RepositoryItemTextEdit repositoryItemTextEdit;

		private GridColumn colActive;

		private GridColumn colName;

		private GridColumn colEcu;

		private GridColumn colMeasurementMode;

		private GridColumn colCycle;

		private RepositoryItemComboBox repositoryItemEcuComboBox;

		private XtraToolTipController mXtraToolTipController;

		private GridColumn colByteCount;

		private RepositoryItemComboBox repositoryItemMeasurementModeComboBox;

		private GridColumn colEvent;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private RepositoryItemCheckEdit repositoryItemCheckEditDummy;

		public event EventHandler SelectionChanged;

		public DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				return this.databaseConfiguration;
			}
			set
			{
				this.databaseConfiguration = value;
				this.CheckForDisabledDatabases();
				this.PopulateDaqEventLists();
				if (this.signalConfiguration != null)
				{
					this.UpdateSignalIconMap(this.signalConfiguration.Signals);
					this.ResetSignalByteCount(this.signalConfiguration.Signals);
				}
				this.gridViewCcpXcpSignalRequests.RefreshData();
			}
		}

		public CcpXcpSignalConfiguration CcpXcpSignalConfiguration
		{
			get
			{
				return this.signalConfiguration;
			}
			set
			{
				this.signalConfiguration = value;
				this.CheckForDisabledDatabases();
				if (value != null)
				{
					List<int> selectedSignalRows = this.GetSelectedSignalRows();
					List<int> selectedGroupRows = this.GetSelectedGroupRows();
					int idx = this.StoreFocusedRow();
					this.PopulateDaqEventLists();
					int topRowIndex = this.gridViewCcpXcpSignalRequests.TopRowIndex;
					this.ResetValidationFramework();
					this.gridControlCcpXcpSignalRequests.DataSource = this.signalConfiguration.Signals;
					this.SetExpandStates();
					this.gridViewCcpXcpSignalRequests.ClearSelection();
					this.RestoreFocusedRow(idx);
					this.RestoreSelection(selectedSignalRows, selectedGroupRows);
					this.gridViewCcpXcpSignalRequests.TopRowIndex = topRowIndex;
					this.DisplayErrors();
				}
			}
		}

		public IModelEditor ModelEditor
		{
			get;
			set;
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public ISemanticChecker SemanticChecker
		{
			get;
			set;
		}

		public ILoggerSpecifics CurrentLogger
		{
			get;
			set;
		}

		public string ConfigurationFolderPath
		{
			get;
			set;
		}

		public bool IsGroupRowSelected
		{
			get
			{
				return this.isGroupRowSelected;
			}
		}

		public CcpXcpSignalRequestsGrid()
		{
			this.InitializeComponent();
			this.keyboardService = new KeyboardNavigationService(this.gridViewCcpXcpSignalRequests);
			this.mXtraGridService = new GeneralService(this.gridViewCcpXcpSignalRequests);
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.expandStates = new Dictionary<ActionCcpXcp, bool>();
			this.isGroupRowSelected = false;
			this.isExpandingLocked = false;
			foreach (CcpXcpMeasurementMode mode in Enum.GetValues(typeof(CcpXcpMeasurementMode)))
			{
				this.repositoryItemMeasurementModeComboBox.Items.Add(this.CcpXcpMeasurementMode2String(mode));
			}
			this.multiSelectEditService = new MultiSelectEditService(this.gridViewCcpXcpSignalRequests, new GridColumn[]
			{
				this.colEcu,
				this.colMeasurementMode,
				this.colCycle
			});
		}

		private List<int> GetSelectedSignalRows()
		{
			return (from index in this.gridViewCcpXcpSignalRequests.GetSelectedRows()
			where index >= 0
			select this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(index)).ToList<int>();
		}

		private List<int> GetSelectedGroupRows()
		{
			return (from index in this.gridViewCcpXcpSignalRequests.GetSelectedRows()
			where index < 0
			select index).ToList<int>();
		}

		private void RestoreSelection(List<int> selectedSignalRows, List<int> selectedGroupRows)
		{
			this.gridViewCcpXcpSignalRequests.ClearSelection();
			foreach (int current in selectedSignalRows)
			{
				int rowHandle = this.gridViewCcpXcpSignalRequests.GetRowHandle(current);
				if (this.gridViewCcpXcpSignalRequests.IsValidRowHandle(rowHandle))
				{
					this.gridViewCcpXcpSignalRequests.SelectRow(rowHandle);
				}
			}
			foreach (int current2 in selectedGroupRows)
			{
				if (this.gridViewCcpXcpSignalRequests.IsValidRowHandle(current2))
				{
					this.gridViewCcpXcpSignalRequests.SelectRow(current2);
				}
			}
		}

		private int StoreFocusedRow()
		{
			int focusedRowHandle = this.gridViewCcpXcpSignalRequests.FocusedRowHandle;
			int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(focusedRowHandle);
			if (focusedRowHandle < 0)
			{
				return focusedRowHandle;
			}
			return dataSourceRowIndex;
		}

		private void RestoreFocusedRow(int idx)
		{
			int num = (idx >= 0) ? this.gridViewCcpXcpSignalRequests.GetRowHandle(idx) : idx;
			if (this.gridViewCcpXcpSignalRequests.IsValidRowHandle(num))
			{
				this.gridViewCcpXcpSignalRequests.FocusedRowHandle = num;
				this.gridViewCcpXcpSignalRequests.SelectRow(num);
			}
		}

		private void Raise_SelectionChanged(object sender, EventArgs e)
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(sender, e);
			}
		}

		public void Init()
		{
		}

		public void AddSignals(Dictionary<ISignal, IDatabase> signals)
		{
			if (signals == null || signals.Count < 1)
			{
				return;
			}
			this.Cursor = Cursors.WaitCursor;
			this.SaveExpandStates();
			this.gridViewCcpXcpSignalRequests.BeginDataUpdate();
			HashSet<CcpXcpSignalComparer> hashSet = new HashSet<CcpXcpSignalComparer>();
			List<CcpXcpSignal> list = new List<CcpXcpSignal>();
			ActionCcpXcp actionCcpXcp;
			if (!this.TryGetSelectedAction(out actionCcpXcp))
			{
				actionCcpXcp = CcpXcpSignalListSelection.CreateDefaultAction();
				this.CcpXcpSignalConfiguration.AddAction(actionCcpXcp);
			}
			foreach (KeyValuePair<ISignal, IDatabase> current in signals)
			{
				Database database = CcpXcpManager.Instance().GetDatabase(current.Value);
				if (database != null)
				{
					CcpXcpSignal ccpXcpSignal = new CcpXcpSignal(current.Key.Name);
					ccpXcpSignal.EcuName.Value = database.CcpXcpEcuDisplayName.Value;
					ccpXcpSignal.DaqEvents = CcpXcpManager.Instance().CreateDaqEventList(current.Key, database);
					if (ccpXcpSignal.DaqEvents.Count > 0)
					{
						ccpXcpSignal.DaqEventId.Value = (from key in ccpXcpSignal.DaqEvents
						orderby key.Key
						select key).First<KeyValuePair<uint, string>>().Key;
						if (current.Key.HasDaqDefaultEvent && ccpXcpSignal.DaqEvents.Keys.Contains((uint)current.Key.DaqDefaulEventId))
						{
							ccpXcpSignal.DaqEventId.Value = (uint)current.Key.DaqDefaulEventId;
						}
					}
					else
					{
						ccpXcpSignal.MeasurementMode.Value = CcpXcpMeasurementMode.Polling;
					}
					ccpXcpSignal.ActionCcpXcp = actionCcpXcp;
					if (actionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Triggered || actionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Conditional)
					{
						ccpXcpSignal.MeasurementMode.Value = CcpXcpMeasurementMode.Polling;
					}
					if (hashSet.Add(new CcpXcpSignalComparer(ccpXcpSignal)))
					{
						list.Add(ccpXcpSignal);
					}
				}
			}
			this.CcpXcpSignalConfiguration.AddSignals(list, actionCcpXcp);
			this.gridViewCcpXcpSignalRequests.EndDataUpdate();
			bool flag = hashSet.Any<CcpXcpSignalComparer>();
			if (flag)
			{
				this.gridViewCcpXcpSignalRequests.BeginUpdate();
				this.SelectConsecutiveRowsOfSignals(from sig in hashSet
				select sig.Signal);
				this.gridViewCcpXcpSignalRequests.EndUpdate();
			}
			this.ValidateInput(flag);
			this.Cursor = Cursors.Default;
		}

		public CcpXcpSignal AddBlankSignal(string signal, string ecu, ActionCcpXcp action)
		{
			if (!string.IsNullOrEmpty(signal))
			{
				CcpXcpSignal ccpXcpSignal = new CcpXcpSignal(signal);
				this.signalConfiguration.AddSignal(ccpXcpSignal, action);
				this.gridViewCcpXcpSignalRequests.ClearSelection();
				this.gridViewCcpXcpSignalRequests.RefreshData();
				if (!string.IsNullOrEmpty(ecu))
				{
					this.UpdateSignalUsingECU(ccpXcpSignal, ecu);
				}
				return ccpXcpSignal;
			}
			return null;
		}

		public void UpdateSignalUsingECU(CcpXcpSignal signal, string ecu)
		{
			ReadOnlyCollection<Database> readOnlyCollection = new ReadOnlyCollection<Database>((from Database db in this.databaseConfiguration.ActiveCCPXCPDatabases
			where db.CcpXcpEcuList.Count > 0 && db.CcpXcpEcuDisplayName.Value == ecu
			select db).ToList<Database>());
			signal.EcuName.Value = ecu;
			if (readOnlyCollection.Count > 0)
			{
				A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(readOnlyCollection[0]);
				if (a2LDatabase.DeviceConfig == null)
				{
					return;
				}
				ISignal signal2 = a2LDatabase.GetSignal(signal.Name.Value);
				Dictionary<uint, string> daqEvents = CcpXcpManager.Instance().CreateDaqEventList(signal2, readOnlyCollection[0]);
				signal.Refresh(daqEvents);
			}
		}

		public void RemoveSelectedSignals(bool showQuestion = true)
		{
			this.SaveExpandStates();
			List<CcpXcpSignal> list = this.TryGetSelectedSignals();
			int count = list.Count;
			if (count < 1)
			{
				return;
			}
			if (count > 1 && showQuestion && InformMessageBox.Question(Resources.WarningDeleteEvents) != DialogResult.Yes)
			{
				return;
			}
			int num = 0;
			int[] selectedRows = this.gridViewCcpXcpSignalRequests.GetSelectedRows();
			for (int i = 0; i < selectedRows.Length; i++)
			{
				int rowHandle = selectedRows[i];
				int visibleIndex = this.gridViewCcpXcpSignalRequests.GetVisibleIndex(rowHandle);
				if (visibleIndex > num)
				{
					num = visibleIndex;
				}
			}
			num -= count;
			foreach (CcpXcpSignal current in list)
			{
				this.CcpXcpSignalConfiguration.RemoveSignal(current, current.ActionCcpXcp);
			}
			int num2 = num + 1;
			int num3 = this.gridViewCcpXcpSignalRequests.RowCount - 1;
			this.gridViewCcpXcpSignalRequests.ClearSelection();
			if (this.gridViewCcpXcpSignalRequests.RowCount > 0)
			{
				if (num2 > num3)
				{
					num2 = num3;
				}
				else if (num2 < 0)
				{
					num2 = 0;
				}
				this.gridViewCcpXcpSignalRequests.SelectRow(this.gridViewCcpXcpSignalRequests.GetVisibleRowHandle(num2));
				this.gridViewCcpXcpSignalRequests.FocusedRowHandle = this.gridViewCcpXcpSignalRequests.GetVisibleRowHandle(num2);
			}
			this.ValidateInput(true);
		}

		public void AddAction(ActionCcpXcp action)
		{
			this.SaveExpandStates();
			this.CcpXcpSignalConfiguration.AddAction(action);
			this.ValidateInput(true);
			this.SelectRowOfAction(action);
		}

		public void RemoveSelectedActions(bool showQuestion = true)
		{
			this.SaveExpandStates();
			List<ActionCcpXcp> list = this.TryGetSelectedActions();
			int count = list.Count;
			if (count < 1)
			{
				return;
			}
			if (showQuestion && InformMessageBox.Question(Resources.WarningDeleteEvents) != DialogResult.Yes)
			{
				return;
			}
			int num = 0;
			int[] selectedRows = this.gridViewCcpXcpSignalRequests.GetSelectedRows();
			for (int i = 0; i < selectedRows.Length; i++)
			{
				int rowHandle = selectedRows[i];
				int visibleIndex = this.gridViewCcpXcpSignalRequests.GetVisibleIndex(rowHandle);
				if (visibleIndex > num)
				{
					num = visibleIndex;
				}
			}
			num -= count;
			foreach (ActionCcpXcp current in list)
			{
				this.CcpXcpSignalConfiguration.RemoveAction(current);
			}
			int num2 = num + 1;
			int num3 = this.gridViewCcpXcpSignalRequests.RowCount - 1;
			this.gridViewCcpXcpSignalRequests.ClearSelection();
			if (this.gridViewCcpXcpSignalRequests.RowCount > 0)
			{
				if (num2 > num3)
				{
					num2 = num3;
				}
				else if (num2 < 0)
				{
					num2 = 0;
				}
				this.gridViewCcpXcpSignalRequests.SelectRow(this.gridViewCcpXcpSignalRequests.GetVisibleRowHandle(num2));
				this.gridViewCcpXcpSignalRequests.FocusedRowHandle = this.gridViewCcpXcpSignalRequests.GetVisibleRowHandle(num2);
			}
			this.ValidateInput(true);
		}

		public void EditAction()
		{
			this.SaveExpandStates();
			ActionCcpXcp actionCcpXcp;
			if (!this.TryGetSelectedAction(out actionCcpXcp))
			{
				return;
			}
			CcpXcpSignalListSelection ccpXcpSignalListSelection = new CcpXcpSignalListSelection(this.ModelValidator, this.ApplicationDatabaseManager, this.signalConfiguration, actionCcpXcp);
			if (DialogResult.OK == ccpXcpSignalListSelection.ShowDialog())
			{
				ActionCcpXcp resultAction = ccpXcpSignalListSelection.ResultAction;
				actionCcpXcp.Assign(resultAction);
				this.ValidateInput(true);
			}
		}

		private void CheckForDisabledDatabases()
		{
			if (this.CcpXcpSignalConfiguration != null && this.CcpXcpSignalConfiguration.Signals != null)
			{
				using (IEnumerator<CcpXcpSignal> enumerator = this.CcpXcpSignalConfiguration.Signals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CcpXcpSignal sig = enumerator.Current;
						CcpXcpSignal arg_B7_0 = sig;
						bool arg_B7_1;
						if ((from Database db in this.DatabaseConfiguration.CCPXCPDatabases
						where db.CcpXcpEcuDisplayName.Value == sig.EcuName.Value
						select db).Any<Database>())
						{
							arg_B7_1 = !(from Database db in this.DatabaseConfiguration.ActiveCCPXCPDatabases
							where db.CcpXcpEcuDisplayName.Value == sig.EcuName.Value
							select db).Any<Database>();
						}
						else
						{
							arg_B7_1 = false;
						}
						arg_B7_0.DatabaseDisabled = arg_B7_1;
					}
				}
			}
		}

		private string CcpXcpMeasurementMode2String(CcpXcpMeasurementMode mode)
		{
			switch (mode)
			{
			case CcpXcpMeasurementMode.DAQ:
				return Vocabulary.Daq;
			case CcpXcpMeasurementMode.Polling:
				return Resources.Polling;
			default:
				return Resources.Unknown;
			}
		}

		private CcpXcpMeasurementMode String2CcpXcpMeasurementMode(string modeString)
		{
			foreach (CcpXcpMeasurementMode ccpXcpMeasurementMode in Enum.GetValues(typeof(CcpXcpMeasurementMode)))
			{
				if (modeString == this.CcpXcpMeasurementMode2String(ccpXcpMeasurementMode))
				{
					return ccpXcpMeasurementMode;
				}
			}
			return CcpXcpMeasurementMode.DAQ;
		}

		private void ToggleSignalActiveState()
		{
			this.SaveExpandStates();
			List<CcpXcpSignal> list = this.TryGetSelectedSignals();
			bool flag = true;
			foreach (CcpXcpSignal current in list)
			{
				flag &= current.IsActive.Value;
			}
			if (flag)
			{
				using (List<CcpXcpSignal>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CcpXcpSignal current2 = enumerator2.Current;
						current2.IsActive.Value = false;
					}
					goto IL_D1;
				}
			}
			foreach (CcpXcpSignal current3 in list)
			{
				current3.IsActive.Value = true;
			}
			IL_D1:
			this.gridViewCcpXcpSignalRequests.RefreshData();
			this.ValidateInput(true);
		}

		private void PopulateDaqEventLists()
		{
			if (this.CcpXcpSignalConfiguration == null)
			{
				return;
			}
			if (this.databaseConfiguration != null)
			{
				using (IEnumerator<CcpXcpSignal> enumerator = this.CcpXcpSignalConfiguration.Signals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CcpXcpSignal sig = enumerator.Current;
						if (sig.DaqEvents == null || !sig.DaqEvents.Any<KeyValuePair<uint, string>>())
						{
							ReadOnlyCollection<Database> readOnlyCollection = new ReadOnlyCollection<Database>((from Database db in this.databaseConfiguration.ActiveCCPXCPDatabases
							where db.CcpXcpEcuDisplayName.Value == sig.EcuName.Value
							select db).ToList<Database>());
							foreach (Database current in readOnlyCollection)
							{
								A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(current);
								if (a2LDatabase != null && a2LDatabase.DeviceConfig != null)
								{
									ISignal signal = a2LDatabase.GetSignal(sig.Name.Value);
									sig.DaqEvents = CcpXcpManager.Instance().CreateDaqEventList(signal, current);
								}
							}
						}
					}
				}
			}
		}

		private string RemovePollingTimeUnit(string pollingTime)
		{
			int length = Resources.MilliSecsShort.Length;
			pollingTime = pollingTime.Trim();
			if (pollingTime.Length >= length && string.Compare(pollingTime.Substring(pollingTime.Length - length, length), Resources.MilliSecsShort, StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				pollingTime = pollingTime.Substring(0, pollingTime.Length - length);
				pollingTime = pollingTime.Trim();
			}
			return pollingTime;
		}

		public void SelectConsecutiveRowsOfSignals(IEnumerable<CcpXcpSignal> signalsToSelect)
		{
			this.gridViewCcpXcpSignalRequests.ClearSelection();
			if (!(this.gridControlCcpXcpSignalRequests.DataSource is IList<CcpXcpSignal>))
			{
				return;
			}
			Dictionary<GridColumn, int> dictionary = new Dictionary<GridColumn, int>();
			foreach (GridColumn gridColumn in this.gridViewCcpXcpSignalRequests.SortedColumns)
			{
				dictionary.Add(gridColumn, gridColumn.SortIndex);
			}
			Dictionary<GridColumn, ColumnSortOrder> dictionary2 = new Dictionary<GridColumn, ColumnSortOrder>();
			foreach (GridColumn gridColumn2 in this.gridViewCcpXcpSignalRequests.SortedColumns)
			{
				dictionary2.Add(gridColumn2, gridColumn2.SortOrder);
			}
			this.gridViewCcpXcpSignalRequests.ClearSorting();
			this.gridViewCcpXcpSignalRequests.SelectRows(this.gridViewCcpXcpSignalRequests.RowCount - signalsToSelect.Count<CcpXcpSignal>(), this.gridViewCcpXcpSignalRequests.RowCount - 1);
			foreach (KeyValuePair<GridColumn, ColumnSortOrder> current in dictionary2)
			{
				current.Key.SortOrder = current.Value;
			}
			foreach (KeyValuePair<GridColumn, int> current2 in dictionary)
			{
				current2.Key.SortIndex = current2.Value;
			}
		}

		private void UpdateSignalIconMap(ReadOnlyCollection<CcpXcpSignal> signals)
		{
			if (signals == null)
			{
				return;
			}
			this.signalIconMap = new Dictionary<CcpXcpSignal, uint>();
			foreach (CcpXcpSignal current in signals)
			{
				CcpXcpManager.EnumValType enumValType;
				uint signalDimensionAndValueType = CcpXcpManager.Instance().GetSignalDimensionAndValueType(current, out enumValType);
				try
				{
					if (signalDimensionAndValueType > 1u)
					{
						this.signalIconMap.Add(current, 2u);
					}
					else
					{
						this.signalIconMap.Add(current, signalDimensionAndValueType);
					}
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private void ResetSignalByteCount(ReadOnlyCollection<CcpXcpSignal> signals)
		{
			foreach (CcpXcpSignal current in signals)
			{
				current.ResetByteCount();
			}
		}

		private IList<string> TryGetEcuForSignal(string signalName)
		{
			List<string> list = new List<string>();
			foreach (Database current in this.databaseConfiguration.ActiveCCPXCPDatabases)
			{
				A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(current);
				if (!current.IsInconsistent && a2LDatabase != null && a2LDatabase.LoggerEcu != null)
				{
					ISignal signal = a2LDatabase.GetSignal(signalName);
					if (signal != null)
					{
						list.Add(current.CcpXcpEcuDisplayName.Value);
					}
				}
			}
			return list;
		}

		private void TryResolveSignalEcuConflicts(CcpXcpSignal signal, Database db, bool matchCase)
		{
			if (db != null)
			{
				A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(db);
				if (!db.IsInconsistent && a2LDatabase != null && a2LDatabase.LoggerEcu != null)
				{
					StringComparison sc = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
					IEnumerable<ISignal> source = from s in a2LDatabase.McDatabase.AllSignals
					where s.Name.Equals(signal.Name.Value, sc) && s.SignalType != EnumSignalType.kAxis
					select s;
					signal.EcuName.Value = db.CcpXcpEcuDisplayName.Value;
					if (source.Count<ISignal>() == 1)
					{
						ISignal signal2 = source.ElementAt(0);
						signal.Name.Value = signal2.Name;
						Dictionary<uint, string> daqEvents = CcpXcpManager.Instance().CreateDaqEventList(signal2, db);
						signal.Refresh(daqEvents);
					}
				}
			}
		}

		private void gridViewCCPXCPDatabases_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			CcpXcpSignal ccpXcpSignal;
			bool flag = this.TryGetSignal(this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(e.RowHandle), out ccpXcpSignal);
			if (flag)
			{
				if (!ccpXcpSignal.ActionCcpXcp.IsActive.Value)
				{
					e.Appearance.ForeColor = SystemColors.GrayText;
				}
				else
				{
					e.Appearance.ForeColor = SystemColors.ControlText;
				}
				if (e.Appearance.BackColor == SystemColors.Highlight || e.Appearance.BackColor == SystemColors.MenuHighlight)
				{
					e.Appearance.ForeColor = SystemColors.HighlightText;
				}
			}
			if (e.Column == this.colActive)
			{
				CheckEditViewInfo checkEditViewInfo = ((GridCellInfo)e.Cell).ViewInfo as CheckEditViewInfo;
				if (checkEditViewInfo != null)
				{
					if (flag && (ccpXcpSignal.DatabaseDisabled || !ccpXcpSignal.ActionCcpXcp.IsActive.Value))
					{
						checkEditViewInfo.AllowOverridedState = true;
						checkEditViewInfo.OverridedState = ObjectState.Disabled;
						checkEditViewInfo.CalcViewInfo(e.Graphics);
					}
					else
					{
						checkEditViewInfo.AllowOverridedState = true;
						checkEditViewInfo.OverridedState = ObjectState.Normal;
						checkEditViewInfo.CalcViewInfo(e.Graphics);
					}
				}
			}
			if (e.Column == this.colName && flag && !this.pageValidator.General.HasError(ccpXcpSignal.Name))
			{
				uint num = 0u;
				if (this.signalIconMap == null || !this.signalIconMap.TryGetValue(ccpXcpSignal, out num))
				{
					this.UpdateSignalIconMap(this.signalConfiguration.Signals);
					if (this.signalIconMap != null)
					{
						this.signalIconMap.TryGetValue(ccpXcpSignal, out num);
					}
				}
				if (num == 1u)
				{
					GridUtil.DrawImageTextCell(e, Resources.IconSignalDefault.ToBitmap());
					return;
				}
				if (num == 2u)
				{
					GridUtil.DrawImageTextCell(e, Resources.IconArray.ToBitmap());
					return;
				}
			}
			else
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(e.RowHandle));
				this.customErrorProvider.Grid.DisplayError(gUIElement, e);
			}
		}

		private void gridViewCCPXCPDatabases_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				string localizedString = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroupBox);
				string localizedString2 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilter);
				string localizedString3 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilterEditor);
				string localizedString4 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroup);
				string localizedString5 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnUnGroup);
				string localizedString6 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnRemoveColumn);
				string localizedString7 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuGroupPanelShow);
				for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
				{
					string caption = e.Menu.Items[i].Caption;
					if (localizedString4 == caption || localizedString5 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString6 == caption || localizedString7 == caption)
					{
						e.Menu.Items.RemoveAt(i);
					}
				}
			}
		}

		private void gridViewCCPXCPDatabases_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			CcpXcpSignal ccpXcpSignal;
			if (!this.TryGetSignal(e.ListSourceRowIndex, out ccpXcpSignal))
			{
				return;
			}
			if (ccpXcpSignal is CcpXcpSignalDummy && e.Column != this.colEvent && e.IsGetData)
			{
				e.Value = string.Empty;
				return;
			}
			if (e.Column == this.colEvent)
			{
				this.UnboundColumnDataEvent(ccpXcpSignal, e);
				return;
			}
			if (e.Column == this.colActive)
			{
				this.UnboundColumnIsSignalActive(ccpXcpSignal, e);
				return;
			}
			if (e.Column == this.colName)
			{
				this.UnboundColumnName(ccpXcpSignal, e);
				return;
			}
			if (e.Column == this.colEcu)
			{
				this.UnboundColumnEcuName(ccpXcpSignal, e);
				return;
			}
			if (e.Column == this.colByteCount)
			{
				this.UnboundColumnByteCount(ccpXcpSignal, e);
				return;
			}
			if (e.Column == this.colMeasurementMode)
			{
				this.UnboundColumnMeasurementMode(ccpXcpSignal, e);
				return;
			}
			if (e.Column == this.colCycle)
			{
				this.UnboundColumnCycle(ccpXcpSignal, sender, e);
			}
		}

		private void gridViewCCPXCPDatabases_ShowingEditor(object sender, CancelEventArgs e)
		{
			if (this.gridViewCcpXcpSignalRequests.FocusedColumn != this.colActive)
			{
				try
				{
					if (!Convert.ToBoolean(this.gridViewCcpXcpSignalRequests.GetRowCellValue(this.gridViewCcpXcpSignalRequests.FocusedRowHandle, this.colActive)))
					{
						e.Cancel = true;
					}
				}
				catch (Exception)
				{
				}
			}
			CcpXcpSignal ccpXcpSignal;
			if (this.TryGetSelectedSignal(out ccpXcpSignal))
			{
				if (ccpXcpSignal.DatabaseDisabled || ccpXcpSignal is CcpXcpSignalDummy || !ccpXcpSignal.ActionCcpXcp.IsActive.Value)
				{
					e.Cancel = true;
				}
				if (this.gridViewCcpXcpSignalRequests.FocusedColumn == this.colCycle && ccpXcpSignal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ && (ccpXcpSignal.DaqEvents == null || ccpXcpSignal.DaqEvents.Count < 1))
				{
					e.Cancel = true;
				}
				if (this.gridViewCcpXcpSignalRequests.FocusedColumn == this.colCycle && ccpXcpSignal.ActionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Triggered)
				{
					e.Cancel = true;
				}
			}
		}

		private void gridViewCcpXcpSignalRequests_ShownEditor(object sender, EventArgs e)
		{
			GridView gridView = sender as GridView;
			if (gridView == null)
			{
				return;
			}
			CcpXcpSignal ccpXcpSignal;
			if (this.gridViewCcpXcpSignalRequests.FocusedColumn == this.colCycle && this.TryGetSelectedSignal(out ccpXcpSignal) && ccpXcpSignal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling)
			{
				TextEdit textEdit = gridView.ActiveEditor as TextEdit;
				if (textEdit != null)
				{
					textEdit.EditValue = this.RemovePollingTimeUnit(ccpXcpSignal.PollingTime.Value);
					textEdit.SelectAll();
				}
			}
		}

		private void gridViewCcpXcpDescriptions_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			CcpXcpSignal ccpXcpSignal;
			if (this.TryGetSignal(this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(e.RowHandle), out ccpXcpSignal))
			{
				if (ccpXcpSignal is CcpXcpSignalDummy)
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
				}
				if (e.Column == this.colCycle && e.RowHandle == this.gridViewCcpXcpSignalRequests.FocusedRowHandle)
				{
					if (ccpXcpSignal.ActionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Triggered)
					{
						e.RepositoryItem = this.repositoryItemTextEditDummy;
					}
					else if (ccpXcpSignal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling)
					{
						e.RepositoryItem = this.repositoryItemTextEdit;
					}
					else
					{
						this.PopulateDaqEventsComboBox(ccpXcpSignal);
						if (ccpXcpSignal.DaqEvents == null || ccpXcpSignal.DaqEvents.Count < 1)
						{
							e.RepositoryItem = this.repositoryItemTextEdit;
						}
						else
						{
							e.RepositoryItem = this.repositoryItemDaqEventsComboBox;
						}
					}
				}
				else if (e.Column == this.colEcu && e.RowHandle == this.gridViewCcpXcpSignalRequests.FocusedRowHandle)
				{
					this.PopulateECUListComboBox();
					e.RepositoryItem.ReadOnly = false;
					e.RepositoryItem = this.repositoryItemEcuComboBox;
				}
				CcpXcpSignal ccpXcpSignal2;
				if (this.TryGetSignal(this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(this.gridViewCcpXcpSignalRequests.FocusedRowHandle), out ccpXcpSignal2))
				{
					if (!ccpXcpSignal2.IsActive.Value || ccpXcpSignal2.DatabaseDisabled || !ccpXcpSignal2.ActionCcpXcp.IsActive.Value)
					{
						this.repositoryItemMeasurementModeComboBox.ReadOnly = true;
						this.repositoryItemDaqEventsComboBox.ReadOnly = true;
						return;
					}
					this.repositoryItemMeasurementModeComboBox.ReadOnly = false;
					this.repositoryItemDaqEventsComboBox.ReadOnly = false;
				}
			}
		}

		private void gridViewCcpXcpDescriptions_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			try
			{
				if (!this.gridViewCcpXcpSignalRequests.IsValidRowHandle(e.RowHandle))
				{
					return;
				}
				CcpXcpSignal ccpXcpSignal = this.CcpXcpSignalConfiguration.Signals.ElementAt(this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(e.RowHandle));
				if (!ccpXcpSignal.IsActive.Value || ccpXcpSignal.DatabaseDisabled)
				{
					e.Appearance.ForeColor = SystemColors.GrayText;
				}
				else
				{
					e.Appearance.ForeColor = SystemColors.ControlText;
				}
			}
			catch (Exception)
			{
			}
			if (e.Appearance.BackColor == SystemColors.Highlight || e.Appearance.BackColor == SystemColors.MenuHighlight)
			{
				e.Appearance.ForeColor = SystemColors.HighlightText;
			}
		}

		private void gridViewCcpXcpSignalRequests_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
		{
			try
			{
				CcpXcpSignal ccpXcpSignal = this.CcpXcpSignalConfiguration.Signals.ElementAt(this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(e.RowHandle));
				if (!ccpXcpSignal.ActionCcpXcp.IsActive.Value)
				{
					e.Appearance.ForeColor = SystemColors.GrayText;
				}
				else
				{
					e.Appearance.ForeColor = SystemColors.ControlText;
				}
			}
			catch (Exception)
			{
			}
			if (e.Appearance.BackColor == SystemColors.Highlight || e.Appearance.BackColor == SystemColors.MenuHighlight)
			{
				e.Appearance.ForeColor = SystemColors.HighlightText;
			}
			int childRowHandle = this.gridViewCcpXcpSignalRequests.GetChildRowHandle(e.RowHandle, 0);
			int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(childRowHandle);
			if (dataSourceRowIndex < 0 || dataSourceRowIndex >= this.CcpXcpSignalConfiguration.Signals.Count)
			{
				e.Handled = false;
				return;
			}
			GridGroupRowPainter gridGroupRowPainter = e.Painter as GridGroupRowPainter;
			if (gridGroupRowPainter == null)
			{
				return;
			}
			GridGroupRowInfo gridGroupRowInfo = e.Info as GridGroupRowInfo;
			if (gridGroupRowInfo == null)
			{
				return;
			}
			Rectangle buttonBounds = gridGroupRowInfo.ButtonBounds;
			Rectangle bounds = e.Bounds;
			bounds.X = buttonBounds.Right + 4;
			gridGroupRowPainter.DrawGroupRowBackground(e.Info);
			OpenCloseButtonInfoArgs e2 = new OpenCloseButtonInfoArgs(e.Cache, gridGroupRowInfo.ButtonBounds, gridGroupRowInfo.GroupExpanded, gridGroupRowInfo.AppearanceGroupButton, ObjectState.Normal);
			if (!gridGroupRowInfo.ButtonBounds.IsEmpty)
			{
				gridGroupRowPainter.ElementsPainter.OpenCloseButton.DrawObject(e2);
			}
			Point location = bounds.Location;
			int num = (bounds.Height - CcpXcpSignalRequestsGrid.iconSize) / 2;
			CcpXcpSignal ccpXcpSignal2 = this.CcpXcpSignalConfiguration.Signals[dataSourceRowIndex];
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (this.HasEventGlobalModelErrors(ccpXcpSignal2.ActionCcpXcp.Event, out empty))
			{
				Rectangle rect = new Rectangle(location.X, location.Y + num, CcpXcpSignalRequestsGrid.iconSize, CcpXcpSignalRequestsGrid.iconSize);
				e.Graphics.DrawImageUnscaled(Resources.IconGlobalModelError.ToBitmap(), rect);
				location.X += CcpXcpSignalRequestsGrid.iconSize + 2;
			}
			else if (ccpXcpSignal2.ActionCcpXcp.StopType is StopOnEvent && this.HasEventGlobalModelErrors((ccpXcpSignal2.ActionCcpXcp.StopType as StopOnEvent).Event, out empty))
			{
				Rectangle rect2 = new Rectangle(location.X, location.Y + num, CcpXcpSignalRequestsGrid.iconSize, CcpXcpSignalRequestsGrid.iconSize);
				e.Graphics.DrawImageUnscaled(Resources.IconGlobalModelError.ToBitmap(), rect2);
				location.X += CcpXcpSignalRequestsGrid.iconSize + 2;
			}
			else if (this.HasSignalErrors(ccpXcpSignal2.ActionCcpXcp, out empty2, out empty))
			{
				Rectangle rect3 = new Rectangle(location.X, location.Y + num, CcpXcpSignalRequestsGrid.iconSize, CcpXcpSignalRequestsGrid.iconSize);
				e.Graphics.DrawImageUnscaled(Resources.IconGlobalModelError.ToBitmap(), rect3);
				location.X += CcpXcpSignalRequestsGrid.iconSize + 2;
			}
			Rectangle rect4 = new Rectangle(location.X, location.Y + num, CcpXcpSignalRequestsGrid.iconSize, CcpXcpSignalRequestsGrid.iconSize);
			if (ccpXcpSignal2.ActionCcpXcp.Event is CyclicTimerEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageCyclicTimer, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is OnStartEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageStart, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is CANIdEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageIDMessageCAN, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is FlexrayIdEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageIDMessageFlexray, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is LINIdEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageIDMessageLIN, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is CANDataEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDataTriggerCAN, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is LINDataEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDataTriggerLIN, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is SymbolicMessageEvent)
			{
				switch ((ccpXcpSignal2.ActionCcpXcp.Event as SymbolicMessageEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageCAN, rect4);
					break;
				case BusType.Bt_LIN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageLIN, rect4);
					break;
				case BusType.Bt_FlexRay:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageFlexray, rect4);
					break;
				default:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageCAN, rect4);
					break;
				}
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is SymbolicSignalEvent)
			{
				switch ((ccpXcpSignal2.ActionCcpXcp.Event as SymbolicSignalEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalCAN, rect4);
					break;
				case BusType.Bt_LIN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalLIN, rect4);
					break;
				case BusType.Bt_FlexRay:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalFlexRay, rect4);
					break;
				default:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalCAN, rect4);
					break;
				}
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is CcpXcpSignalEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalCcpXcp, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is MsgTimeoutEvent)
			{
				switch ((ccpXcpSignal2.ActionCcpXcp.Event as MsgTimeoutEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutCAN, rect4);
					break;
				case BusType.Bt_LIN:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutLIN, rect4);
					break;
				case BusType.Bt_FlexRay:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutFlexray, rect4);
					break;
				default:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutCAN, rect4);
					break;
				}
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is DigitalInputEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDigitalInput, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is AnalogInputEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageAnalogInputSignal, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is KeyEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageKey, rect4);
			}
			else if (ccpXcpSignal2.ActionCcpXcp.Event is IgnitionEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageOnIgnition, rect4);
			}
			location.X += CcpXcpSignalRequestsGrid.iconSize + 2;
			bounds.Location = location;
			GridView gridView = sender as GridView;
			string text = (gridView != null) ? gridView.GetGroupRowDisplayText(e.RowHandle) : string.Empty;
			e.Appearance.DrawString(e.Cache, text, bounds);
			e.Handled = true;
		}

		private void gridViewCCPXCPDatabases_KeyDown(object sender, KeyEventArgs e)
		{
			this.keyboardService.GridControlProcessGridKey(e);
			if (e.KeyCode == Keys.Space)
			{
				this.ToggleSignalActiveState();
				return;
			}
			if (e.KeyCode == Keys.Delete && InformMessageBox.Question(Resources.WarningDeleteEvents) == DialogResult.Yes)
			{
				this.RemoveSelectedActions(false);
				this.RemoveSelectedSignals(false);
			}
		}

		private void gridViewCcpXcpSignalRequests_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewCcpXcpSignalRequests_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void CcpXcpSignalRequestsGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewCcpXcpSignalRequests_CustomColumnGroup(object sender, CustomColumnSortEventArgs e)
		{
			IList<CcpXcpSignal> signals = this.CcpXcpSignalConfiguration.Signals;
			if (e.Column == this.colEvent && e.ListSourceRowIndex1 < signals.Count && e.ListSourceRowIndex2 < signals.Count)
			{
				CcpXcpSignal ccpXcpSignal = signals[e.ListSourceRowIndex1];
				CcpXcpSignal ccpXcpSignal2 = signals[e.ListSourceRowIndex2];
				if (ccpXcpSignal.ActionCcpXcp == ccpXcpSignal2.ActionCcpXcp)
				{
					e.Result = 0;
				}
				else
				{
					e.Result = 1;
				}
				e.Handled = true;
				return;
			}
			e.Handled = false;
		}

		private void gridViewCcpXcpSignalRequests_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
		{
			IList<CcpXcpSignal> signals = this.CcpXcpSignalConfiguration.Signals;
			if (e.Column == this.colEvent && e.ListSourceRowIndex1 < signals.Count && e.ListSourceRowIndex2 < signals.Count)
			{
				CcpXcpSignal ccpXcpSignal = signals[e.ListSourceRowIndex1];
				CcpXcpSignal ccpXcpSignal2 = signals[e.ListSourceRowIndex2];
				int num = this.CcpXcpSignalConfiguration.Actions.IndexOf(ccpXcpSignal.ActionCcpXcp);
				int num2 = this.CcpXcpSignalConfiguration.Actions.IndexOf(ccpXcpSignal2.ActionCcpXcp);
				if (num < num2)
				{
					e.Result = -1;
				}
				else if (num > num2)
				{
					e.Result = 1;
				}
				else
				{
					e.Result = 0;
				}
				e.Handled = true;
				return;
			}
			e.Handled = false;
		}

		private void gridViewCcpXcpSignalRequests_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.isGroupRowSelected = (e.FocusedRowHandle < 0);
			this.mXtraGridService.FocusedRowChanged(e);
			this.Raise_SelectionChanged(this, EventArgs.Empty);
		}

		private void gridViewCcpXcpSignalRequests_GroupRowCollapsing(object sender, RowAllowEventArgs e)
		{
			e.Allow = !this.isExpandingLocked;
			this.isExpandingLocked = false;
		}

		private void gridViewCcpXcpSignalRequests_GroupRowExpanding(object sender, RowAllowEventArgs e)
		{
			e.Allow = !this.isExpandingLocked;
			this.isExpandingLocked = false;
		}

		private void gridViewCcpXcpSignalRequests_GroupRowExpanded(object sender, RowEventArgs e)
		{
			this.gridControlCcpXcpSignalRequests.Refresh();
			this.DisplayErrors();
		}

		private void gridViewCcpXcpSignalRequests_DoubleClick(object sender, EventArgs e)
		{
			GridHitInfo gridHitInfo = this.gridViewCcpXcpSignalRequests.CalcHitInfo(this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition));
			if (this.isGroupRowSelected && gridHitInfo.HitTest == GridHitTest.Row)
			{
				this.EditAction();
				this.isExpandingLocked = true;
			}
		}

		private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			ToolTipControlInfo info = null;
			try
			{
				GridHitInfo gridHitInfo = this.gridViewCcpXcpSignalRequests.CalcHitInfo(e.ControlMousePosition);
				CcpXcpSignal ccpXcpSignal;
				if (gridHitInfo.RowHandle < 0 && this.TryGetSignal(this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(gridHitInfo.RowHandle), out ccpXcpSignal))
				{
					string text = string.Empty;
					if (this.HasEventGlobalModelErrors(ccpXcpSignal.ActionCcpXcp.Event, out text))
					{
						text = "Start: " + text;
					}
					string empty = string.Empty;
					if (ccpXcpSignal.ActionCcpXcp.StopType is StopOnEvent && this.HasEventGlobalModelErrors((ccpXcpSignal.ActionCcpXcp.StopType as StopOnEvent).Event, out empty))
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += " ";
						}
						text = text + "Stop: " + empty;
					}
					string empty2 = string.Empty;
					string empty3 = string.Empty;
					if (string.IsNullOrEmpty(text) && this.HasSignalErrors(ccpXcpSignal.ActionCcpXcp, out empty3, out empty2))
					{
						text = string.Format(Resources_CcpXcp.CcpXcpSignalErrorInGroup, empty3);
					}
					if (!string.IsNullOrEmpty(text))
					{
						info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), text);
						e.Info = info;
					}
				}
			}
			catch
			{
				e.Info = info;
			}
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewCcpXcpSignalRequests.PostEditor();
		}

		private void repositoryItemMeasurementModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewCcpXcpSignalRequests.PostEditor();
		}

		private void repositoryItemEcuComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBoxEdit comboBoxEdit = sender as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				this.repositoryItemEcuComboBox.Tag = comboBoxEdit.SelectedIndex;
			}
			this.gridViewCcpXcpSignalRequests.PostEditor();
		}

		private void repositoryItemCheckEditIsEnabled_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewCcpXcpSignalRequests.PostEditor();
		}

		private void UnboundColumnDataEvent(CcpXcpSignal signal, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (signal.ActionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Always)
				{
					e.Value = Resources_CcpXcp.SignalActionActivationAlways;
				}
				else if (signal.ActionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Triggered)
				{
					e.Value = Resources_CcpXcp.SignalActionActivationTriggered + ": " + CcpXcpSignalRequestsGrid.CreateNameFromEvent(signal.ActionCcpXcp.Event, this.ModelValidator);
				}
				else if (signal.ActionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Conditional)
				{
					string text = CcpXcpSignalRequestsGrid.CreateNameFromEvent(signal.ActionCcpXcp.Event, this.ModelValidator);
					string text2 = "";
					if (signal.ActionCcpXcp.StopType is StopOnEvent)
					{
						text2 = CcpXcpSignalRequestsGrid.CreateNameFromEvent((signal.ActionCcpXcp.StopType as StopOnEvent).Event, this.ModelValidator);
					}
					else
					{
						StopOnTimer stopOnTimer = signal.ActionCcpXcp.StopType as StopOnTimer;
						if (stopOnTimer != null)
						{
							text2 = string.Concat(new object[]
							{
								Resources.Timer,
								" (",
								stopOnTimer.Duration,
								" ms)"
							});
						}
					}
					e.Value = string.Concat(new string[]
					{
						Resources_CcpXcp.SignalActionActivationConditional,
						": Start: ",
						text,
						"; Stop: ",
						text2
					});
				}
				else
				{
					e.Value = Resources.Unknown;
				}
				if (!signal.ActionCcpXcp.IsActive.Value)
				{
					e.Value = Resources_CcpXcp.SignalActionActivationOff + ": " + e.Value;
				}
			}
		}

		public static string CreateNameFromEvent(Event ev, IModelValidator modelValidator)
		{
			if (ev == null || modelValidator == null)
			{
				return "";
			}
			string result = "";
			if (ev is OnStartEvent)
			{
				double num = Convert.ToDouble((ev as OnStartEvent).Delay.Value) / 1000.0;
				result = string.Format(Resources.DiagEvOnStartGroupRow, num.ToString("F1", CultureInfo.InvariantCulture));
			}
			else if (ev is CyclicTimerEvent)
			{
				CyclicTimerEvent cyclicTimerEvent = ev as CyclicTimerEvent;
				result = string.Format(Resources.DiagEvCyclicTimerGroupRow, new object[]
				{
					cyclicTimerEvent.Interval.Value,
					GUIUtil.GetTimeUnitString(cyclicTimerEvent.TimeUnit.Value),
					cyclicTimerEvent.DelayCycles.Value * cyclicTimerEvent.Interval.Value,
					GUIUtil.GetTimeUnitString(cyclicTimerEvent.TimeUnit.Value)
				});
			}
			else if (ev is IdEvent)
			{
				IdEvent idEvent = ev as IdEvent;
				if (idEvent is CANIdEvent)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(idEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(idEvent));
				}
				else if (idEvent is LINIdEvent)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(idEvent.ChannelNumber.Value, modelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(idEvent));
				}
				else if (idEvent is FlexrayIdEvent)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(idEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(idEvent));
				}
			}
			else if (ev is CANDataEvent)
			{
				CANDataEvent cANDataEvent = ev as CANDataEvent;
				result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(cANDataEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(cANDataEvent));
			}
			else if (ev is LINDataEvent)
			{
				LINDataEvent lINDataEvent = ev as LINDataEvent;
				result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(lINDataEvent.ChannelNumber.Value, modelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(lINDataEvent));
			}
			else if (ev is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = ev as SymbolicMessageEvent;
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_CAN)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicMessageEvent, modelValidator.DatabaseServices));
				}
				else if (symbolicMessageEvent.BusType.Value == BusType.Bt_LIN)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value, modelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(symbolicMessageEvent, modelValidator.DatabaseServices));
				}
				else if (symbolicMessageEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicMessageEvent, modelValidator.DatabaseServices));
				}
			}
			else if (ev is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = ev as SymbolicSignalEvent;
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_CAN)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicSignalEvent, modelValidator.DatabaseServices));
				}
				else if (symbolicSignalEvent.BusType.Value == BusType.Bt_LIN)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value, modelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(symbolicSignalEvent, modelValidator.DatabaseServices));
				}
				else if (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicSignalEvent, modelValidator.DatabaseServices));
				}
			}
			else if (ev is CcpXcpSignalEvent)
			{
				CcpXcpSignalEvent symSigEvent = ev as CcpXcpSignalEvent;
				result = GUIUtil.MapEventCondition2String(symSigEvent, modelValidator.DatabaseServices);
			}
			else if (ev is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = ev as MsgTimeoutEvent;
				if (msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(msgTimeoutEvent, modelValidator.DatabaseServices));
				}
				else if (msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
				{
					result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value, modelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(msgTimeoutEvent, modelValidator.DatabaseServices));
				}
			}
			else if (ev is DigitalInputEvent)
			{
				DigitalInputEvent digitalInputEvent = ev as DigitalInputEvent;
				result = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapDigitalInputNumber2String(digitalInputEvent.DigitalInput.Value), GUIUtil.MapEventCondition2String(digitalInputEvent));
			}
			else if (ev is AnalogInputEvent)
			{
				AnalogInputEvent analogInputEvent = ev as AnalogInputEvent;
				result = string.Format("{0} {1}", GUIUtil.MapAnalogInputNumber2String(analogInputEvent.InputNumber.Value), GUIUtil.MapEventCondition2String(analogInputEvent));
			}
			else if (ev is KeyEvent)
			{
				KeyEvent keyEvent = ev as KeyEvent;
				result = GUIUtil.MapKeyNumber2String(keyEvent.Number.Value, keyEvent.IsOnPanel.Value);
			}
			else if (ev is IgnitionEvent)
			{
				IgnitionEvent ignitionEvent = ev as IgnitionEvent;
				result = GUIUtil.MapEventCondition2String(ignitionEvent);
			}
			else
			{
				result = Resources.Unknown;
			}
			return result;
		}

		private void UnboundColumnIsSignalActive(CcpXcpSignal signal, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = signal.IsActive.Value;
				return;
			}
			this.SaveExpandStates();
			bool flag;
			this.pageValidator.Grid.UpdateModel<bool>((bool)e.Value, signal.IsActive, out flag);
		}

		private void UnboundColumnName(CcpXcpSignal signal, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = signal.Name.Value;
			}
		}

		private void UnboundColumnEcuName(CcpXcpSignal signal, CustomColumnDataEventArgs e)
		{
			if (!e.IsGetData)
			{
				bool flag;
				this.pageValidator.Grid.UpdateModel<string>((string)e.Value, signal.EcuName, out flag);
				if (flag)
				{
					int index = (int)this.repositoryItemEcuComboBox.Tag;
					Database database = this.databaseConfiguration.ActiveCCPXCPDatabases[index];
					A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(database);
					if (database.FileType == DatabaseFileType.A2L && a2LDatabase != null)
					{
						ISignal signal2 = a2LDatabase.GetSignal(signal.Name.Value);
						Dictionary<uint, string> daqEvents = CcpXcpManager.Instance().CreateDaqEventList(signal2, database);
						signal.Refresh(daqEvents);
					}
				}
				return;
			}
			if (!signal.DatabaseDisabled)
			{
				e.Value = signal.EcuName.Value;
				return;
			}
			e.Value = Resources.Disabled + ": " + signal.EcuName.Value;
		}

		private void UnboundColumnByteCount(CcpXcpSignal signal, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				uint num = 0u;
				e.Value = num;
				if (CcpXcpManager.Instance().TryGetByteCountOfSignal(signal, out num))
				{
					e.Value = num;
				}
			}
		}

		private void UnboundColumnCycle(CcpXcpSignal signal, object sender, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (signal.ActionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Triggered)
				{
					e.Value = Resources_CcpXcp.CcpXcpSignalTriggeredCycle;
				}
				else if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
				{
					string value;
					if (signal.DaqEvents != null && signal.DaqEvents.TryGetValue(signal.DaqEventId.Value, out value))
					{
						e.Value = value;
					}
					else
					{
						e.Value = Resources_CcpXcp.CcpXcpDaqEventId + ": " + signal.DaqEventId.Value;
					}
				}
				else
				{
					e.Value = signal.PollingTime + " " + Resources.UnitMilliseconds;
				}
			}
			if (e.IsSetData)
			{
				if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
				{
					if (signal.DaqEvents == null)
					{
						return;
					}
					using (Dictionary<uint, string>.Enumerator enumerator = signal.DaqEvents.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<uint, string> current = enumerator.Current;
							if (e.Value.ToString() == current.Value)
							{
								bool flag;
								this.pageValidator.Grid.UpdateModel<uint>(current.Key, signal.DaqEventId, out flag);
							}
						}
						return;
					}
				}
				string text = this.RemovePollingTimeUnit(e.Value.ToString());
				int num;
				bool flag2 = int.TryParse(text, out num);
				if (flag2)
				{
					bool flag3;
					this.pageValidator.Grid.UpdateModel<string>(text, signal.PollingTime, out flag3);
				}
			}
		}

		private void UnboundColumnMeasurementMode(CcpXcpSignal signal, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.CcpXcpMeasurementMode2String(signal.MeasurementMode.Value);
			}
			if (e.IsSetData)
			{
				bool flag;
				this.pageValidator.Grid.UpdateModel<CcpXcpMeasurementMode>(this.String2CcpXcpMeasurementMode(e.Value.ToString()), signal.MeasurementMode, out flag);
			}
		}

		private void PopulateDaqEventsComboBox(CcpXcpSignal signal)
		{
			this.repositoryItemDaqEventsComboBox.Items.Clear();
			if (signal.DaqEvents != null)
			{
				this.repositoryItemDaqEventsComboBox.Items.AddRange(signal.DaqEvents.Values);
			}
		}

		private void PopulateECUListComboBox()
		{
			this.repositoryItemEcuComboBox.Items.Clear();
			foreach (Database current in this.databaseConfiguration.ActiveCCPXCPDatabases)
			{
				if (current.CcpXcpEcuList.Count > 0)
				{
					this.repositoryItemEcuComboBox.Items.Add(current.CcpXcpEcuDisplayName.Value);
				}
			}
		}

		public void DisplayErrors()
		{
			this.StoreMapping4VisibleCells();
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			this.ResetValidationFramework();
			this.StoreMapping4VisibleCells();
			bool flag = true;
			flag &= this.ModelValidator.Validate(this.signalConfiguration, isDataChanged, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public bool HasErrors()
		{
			return this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		public bool HasGlobalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
		}

		public bool HasDatabaseError(CcpXcpSignal signal)
		{
			return this.pageValidator.General.HasError(signal.EcuName);
		}

		public bool HasLocalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		private bool HasSignalErrors(ActionCcpXcp action, out string signalName, out string errorText)
		{
			signalName = string.Empty;
			errorText = string.Empty;
			foreach (CcpXcpSignal current in action.ActiveSignals)
			{
				signalName = current.Name.Value;
				if (this.pageValidator.General.HasError(current.IsActive))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.IsActive);
					bool result = true;
					return result;
				}
				if (this.pageValidator.General.HasError(current.Name))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.Name);
					bool result = true;
					return result;
				}
				if (this.pageValidator.General.HasError(current.EcuName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.EcuName);
					bool result = true;
					return result;
				}
				if (this.pageValidator.General.HasError(current.MeasurementMode))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.MeasurementMode);
					bool result = true;
					return result;
				}
				if (this.pageValidator.General.HasError(current.DaqEventId))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.DaqEventId);
					bool result = true;
					return result;
				}
				if (this.pageValidator.General.HasError(current.PollingTime))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.PollingTime);
					bool result = true;
					return result;
				}
			}
			return false;
		}

		private bool HasEventGlobalModelErrors(Event ev, out string errorText)
		{
			errorText = string.Empty;
			if (ev is IdEvent)
			{
				if (this.pageValidator.General.HasError((ev as IdEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as IdEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is CANDataEvent)
			{
				if (this.pageValidator.General.HasError((ev as CANDataEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as CANDataEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is LINDataEvent)
			{
				if (this.pageValidator.General.HasError((ev as LINDataEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as LINDataEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is SymbolicMessageEvent)
			{
				if (this.pageValidator.General.HasError((ev as SymbolicMessageEvent).MessageName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicMessageEvent).MessageName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as SymbolicMessageEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicMessageEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is SymbolicSignalEvent)
			{
				if (this.pageValidator.General.HasError((ev as SymbolicSignalEvent).SignalName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicSignalEvent).SignalName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as SymbolicSignalEvent).MessageName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicSignalEvent).MessageName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as SymbolicSignalEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicSignalEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is CcpXcpSignalEvent)
			{
				if (this.pageValidator.General.HasError((ev as CcpXcpSignalEvent).SignalName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as CcpXcpSignalEvent).SignalName);
					return true;
				}
			}
			else if (ev is MsgTimeoutEvent)
			{
				if (this.pageValidator.General.HasError((ev as MsgTimeoutEvent).MessageName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as MsgTimeoutEvent).MessageName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as MsgTimeoutEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as MsgTimeoutEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is DigitalInputEvent)
			{
				if (this.pageValidator.General.HasError((ev as DigitalInputEvent).DigitalInput))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as DigitalInputEvent).DigitalInput);
					return true;
				}
			}
			else if (ev is KeyEvent && this.pageValidator.General.HasError((ev as KeyEvent).IsOnPanel))
			{
				errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as KeyEvent).IsOnPanel);
				return true;
			}
			return false;
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewCcpXcpSignalRequests, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.signalConfiguration.Signals.Count)
			{
				return;
			}
			CcpXcpSignal signal = this.signalConfiguration.Signals[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(signal, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(CcpXcpSignal signal, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colActive, this.gridViewCcpXcpSignalRequests))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colActive, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(signal.IsActive, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colName, this.gridViewCcpXcpSignalRequests))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colName, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(signal.Name, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colEcu, this.gridViewCcpXcpSignalRequests))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colEcu, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(signal.EcuName, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colMeasurementMode, this.gridViewCcpXcpSignalRequests))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colMeasurementMode, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(signal.MeasurementMode, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colCycle, this.gridViewCcpXcpSignalRequests))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colCycle, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(signal.PollingTime, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colCycle, this.gridViewCcpXcpSignalRequests))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colCycle, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(signal.DaqEventId, gUIElement);
			}
		}

		public void ExpandAllGroups()
		{
			this.gridControlCcpXcpSignalRequests.Refresh();
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewCcpXcpSignalRequests.IsValidRowHandle(i))
				{
					return;
				}
				this.gridViewCcpXcpSignalRequests.SetRowExpanded(i, false);
				this.gridViewCcpXcpSignalRequests.SetRowExpanded(i, true);
			}
		}

		public bool TryGetSelectedSignal(out CcpXcpSignal signal)
		{
			int num;
			return this.TryGetSelectedSignal(out signal, out num);
		}

		private bool TryGetSelectedSignal(out CcpXcpSignal signal, out int idx)
		{
			signal = null;
			idx = this.gridViewCcpXcpSignalRequests.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.signalConfiguration.Signals.Count - 1)
			{
				return false;
			}
			signal = this.signalConfiguration.Signals[idx];
			return null != signal;
		}

		private List<CcpXcpSignal> TryGetSelectedSignals()
		{
			List<CcpXcpSignal> list = new List<CcpXcpSignal>();
			int[] selectedRows = this.gridViewCcpXcpSignalRequests.GetSelectedRows();
			for (int i = 0; i < selectedRows.Length; i++)
			{
				int rowHandle = selectedRows[i];
				int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(rowHandle);
				if (!this.gridViewCcpXcpSignalRequests.IsGroupRow(rowHandle) && dataSourceRowIndex >= 0 && dataSourceRowIndex <= this.signalConfiguration.Signals.Count - 1)
				{
					list.Add(this.signalConfiguration.Signals[dataSourceRowIndex]);
				}
			}
			return list;
		}

		private bool TryGetSignal(int listSourceRowIndex, out CcpXcpSignal signal)
		{
			signal = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.signalConfiguration.Signals.Count - 1)
			{
				return false;
			}
			signal = this.signalConfiguration.Signals[listSourceRowIndex];
			return null != signal;
		}

		public void SelectRowOfSignal(CcpXcpSignal signal)
		{
			this.gridViewCcpXcpSignalRequests.ClearSelection();
			for (int i = 0; i < this.gridViewCcpXcpSignalRequests.RowCount; i++)
			{
				IList<CcpXcpSignal> list = this.gridViewCcpXcpSignalRequests.DataSource as IList<CcpXcpSignal>;
				if (list != null)
				{
					CcpXcpSignal ccpXcpSignal = list[this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(i)];
					if (ccpXcpSignal == signal)
					{
						this.gridViewCcpXcpSignalRequests.FocusedRowHandle = i;
						this.gridViewCcpXcpSignalRequests.SelectRow(i);
						return;
					}
				}
			}
		}

		public bool TryGetSelectedAction(out ActionCcpXcp action)
		{
			int num;
			return this.TryGetSelectedAction(out action, out num);
		}

		private bool TryGetSelectedAction(out ActionCcpXcp action, out int groupRowHandle)
		{
			action = null;
			groupRowHandle = -1;
			if (this.isGroupRowSelected)
			{
				groupRowHandle = this.gridViewCcpXcpSignalRequests.FocusedRowHandle;
				if (this.gridViewCcpXcpSignalRequests.GetChildRowCount(groupRowHandle) == 0)
				{
					return false;
				}
				int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(this.gridViewCcpXcpSignalRequests.GetChildRowHandle(groupRowHandle, 0));
				CcpXcpSignal ccpXcpSignal = null;
				if (!this.TryGetSignal(dataSourceRowIndex, out ccpXcpSignal))
				{
					return false;
				}
				action = ccpXcpSignal.ActionCcpXcp;
			}
			else
			{
				int focusedRowHandle = this.gridViewCcpXcpSignalRequests.FocusedRowHandle;
				if (!this.gridViewCcpXcpSignalRequests.IsValidRowHandle(focusedRowHandle))
				{
					return false;
				}
				groupRowHandle = this.gridViewCcpXcpSignalRequests.GetParentRowHandle(focusedRowHandle);
				CcpXcpSignal ccpXcpSignal2 = this.CcpXcpSignalConfiguration.Signals[this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(focusedRowHandle)];
				action = ccpXcpSignal2.ActionCcpXcp;
			}
			return null != action;
		}

		private List<ActionCcpXcp> TryGetSelectedActions()
		{
			List<ActionCcpXcp> list = new List<ActionCcpXcp>();
			int[] selectedRows = this.gridViewCcpXcpSignalRequests.GetSelectedRows();
			for (int i = 0; i < selectedRows.Length; i++)
			{
				int rowHandle = selectedRows[i];
				if (this.gridViewCcpXcpSignalRequests.IsGroupRow(rowHandle) && this.gridViewCcpXcpSignalRequests.GetChildRowCount(rowHandle) != 0)
				{
					int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(this.gridViewCcpXcpSignalRequests.GetChildRowHandle(rowHandle, 0));
					CcpXcpSignal ccpXcpSignal = null;
					if (this.TryGetSignal(dataSourceRowIndex, out ccpXcpSignal))
					{
						list.Add(ccpXcpSignal.ActionCcpXcp);
					}
				}
			}
			return list;
		}

		public void SelectRowOfAction(ActionCcpXcp action)
		{
			for (int i = 0; i < this.gridViewCcpXcpSignalRequests.RowCount; i++)
			{
				IList<CcpXcpSignal> list = this.gridViewCcpXcpSignalRequests.DataSource as IList<CcpXcpSignal>;
				if (list != null)
				{
					ActionCcpXcp actionCcpXcp = list[this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(i)].ActionCcpXcp;
					if (actionCcpXcp == action)
					{
						if (this.gridViewCcpXcpSignalRequests.IsValidRowHandle(i))
						{
							this.gridViewCcpXcpSignalRequests.ClearSelection();
							this.gridViewCcpXcpSignalRequests.FocusedRowHandle = this.gridViewCcpXcpSignalRequests.GetParentRowHandle(i);
							this.gridViewCcpXcpSignalRequests.SelectRow(this.gridViewCcpXcpSignalRequests.GetParentRowHandle(i));
							return;
						}
						break;
					}
				}
			}
		}

		private bool TryGetAction(out ActionCcpXcp action, int groupRowHandle)
		{
			action = null;
			if (this.gridViewCcpXcpSignalRequests.IsGroupRow(groupRowHandle))
			{
				if (this.gridViewCcpXcpSignalRequests.GetChildRowCount(groupRowHandle) == 0)
				{
					return false;
				}
				int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(this.gridViewCcpXcpSignalRequests.GetChildRowHandle(groupRowHandle, 0));
				CcpXcpSignal ccpXcpSignal = null;
				if (!this.TryGetSignal(dataSourceRowIndex, out ccpXcpSignal))
				{
					return false;
				}
				action = ccpXcpSignal.ActionCcpXcp;
			}
			else
			{
				if (!this.gridViewCcpXcpSignalRequests.IsValidRowHandle(groupRowHandle))
				{
					return false;
				}
				CcpXcpSignal ccpXcpSignal2 = this.CcpXcpSignalConfiguration.Signals[this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(groupRowHandle)];
				action = ccpXcpSignal2.ActionCcpXcp;
			}
			return null != action;
		}

		private void SaveExpandStates()
		{
			this.expandStates.Clear();
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewCcpXcpSignalRequests.IsValidRowHandle(i))
				{
					return;
				}
				int childRowHandle = this.gridViewCcpXcpSignalRequests.GetChildRowHandle(i, 0);
				if (this.gridViewCcpXcpSignalRequests.IsValidRowHandle(childRowHandle))
				{
					int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(childRowHandle);
					if (dataSourceRowIndex < this.CcpXcpSignalConfiguration.Signals.Count)
					{
						CcpXcpSignal ccpXcpSignal = this.CcpXcpSignalConfiguration.Signals[dataSourceRowIndex];
						if (!this.expandStates.ContainsKey(ccpXcpSignal.ActionCcpXcp))
						{
							this.expandStates.Add(ccpXcpSignal.ActionCcpXcp, this.gridViewCcpXcpSignalRequests.GetRowExpanded(i));
						}
					}
				}
			}
		}

		private void SetExpandStates()
		{
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewCcpXcpSignalRequests.IsValidRowHandle(i))
				{
					return;
				}
				int childRowHandle = this.gridViewCcpXcpSignalRequests.GetChildRowHandle(i, 0);
				if (this.gridViewCcpXcpSignalRequests.IsValidRowHandle(childRowHandle))
				{
					int dataSourceRowIndex = this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(childRowHandle);
					CcpXcpSignal ccpXcpSignal = this.CcpXcpSignalConfiguration.Signals[dataSourceRowIndex];
					bool rowExpanded = this.gridViewCcpXcpSignalRequests.GetRowExpanded(i);
					bool flag = true;
					if (this.expandStates.ContainsKey(ccpXcpSignal.ActionCcpXcp))
					{
						flag = this.expandStates[ccpXcpSignal.ActionCcpXcp];
					}
					if (rowExpanded != flag)
					{
						this.gridViewCcpXcpSignalRequests.SetRowExpanded(i, flag);
					}
				}
			}
		}

		public bool Serialize(CcpXcpSignalRequestsPage page)
		{
			if (page == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewCcpXcpSignalRequests.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				page.CcpXcpSignalRequestsPageGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(CcpXcpSignalRequestsPage page)
		{
			if (page == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(page.CcpXcpSignalRequestsPageGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(page.CcpXcpSignalRequestsPageGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewCcpXcpSignalRequests.RestoreLayoutFromStream(memoryStream);
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

		public void AddSignalsFromFile(string file)
		{
			string extension = Path.GetExtension(file);
			if (string.IsNullOrEmpty(extension))
			{
				return;
			}
			List<CcpXcpSignal> list = new List<CcpXcpSignal>();
			ActionCcpXcp action = null;
			if (!this.TryGetSelectedAction(out action))
			{
				action = new ActionCcpXcp(null);
				this.AddAction(action);
			}
			if (extension.EndsWith("csv"))
			{
				this.AddSignalsFromFileCSV(file, action, ref list);
			}
			else if (extension.EndsWith("lab"))
			{
				this.AddSignalsFromFileLAB(file, action, ref list);
			}
			else if (extension.EndsWith("cfg"))
			{
				this.AddSignalsFromFileCanapeSignalList(file, action, ref list);
			}
			if (list != null)
			{
				this.gridViewCcpXcpSignalRequests.BeginUpdate();
				this.SelectRowsOfSignals(list);
				this.gridViewCcpXcpSignalRequests.EndUpdate();
			}
		}

		private void AddSignalsFromFileCSV(string file, ActionCcpXcp action, ref List<CcpXcpSignal> signalsToSelect)
		{
			this.gridViewCcpXcpSignalRequests.BeginDataUpdate();
			try
			{
				List<CcpXcpSignal> list = new List<CcpXcpSignal>();
				using (new WaitCursor())
				{
					using (StreamReader streamReader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.Default, true))
					{
						signalsToSelect = new List<CcpXcpSignal>();
						streamReader.ReadLine();
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							string text2 = "";
							string[] array = text.Split(new char[]
							{
								';'
							});
							if (array.Length > 0)
							{
								if (array.Length > 1)
								{
									text2 = array[1];
								}
								else
								{
									IList<string> list2 = this.TryGetEcuForSignal(array[0]);
									if (list2.Count == 1)
									{
										text2 = list2[0];
									}
								}
								CcpXcpSignal ccpXcpSignal = this.AddBlankSignal(array[0], text2, action);
								if (ccpXcpSignal != null)
								{
									signalsToSelect.Add(ccpXcpSignal);
								}
								if (string.IsNullOrEmpty(text2))
								{
									list.Add(ccpXcpSignal);
								}
							}
						}
					}
				}
				this.ConflictedSignalsProcessing(list);
			}
			catch (Exception)
			{
				InformMessageBox.Error(Resources.FileOpenError, new string[]
				{
					file
				});
			}
			this.gridViewCcpXcpSignalRequests.EndDataUpdate();
			this.ValidateInput(true);
		}

		private void AddSignalsFromFileLAB(string file, ActionCcpXcp action, ref List<CcpXcpSignal> signalsToSelect)
		{
			this.gridViewCcpXcpSignalRequests.BeginDataUpdate();
			try
			{
				using (new WaitCursor())
				{
					string ecu = "";
					using (StreamReader streamReader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
					{
						signalsToSelect = new List<CcpXcpSignal>();
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							text = text.Trim();
							if (text.StartsWith("["))
							{
								ecu = text.Replace("[", "").Replace("]", "");
							}
							else
							{
								string[] array = text.Split(new char[]
								{
									';'
								});
								CcpXcpSignal ccpXcpSignal = null;
								if (array.Length > 0)
								{
									ccpXcpSignal = this.AddBlankSignal(array[0], ecu, action);
								}
								if (ccpXcpSignal != null)
								{
									if (array.Length > 1 && !string.IsNullOrEmpty(array[1]))
									{
										ccpXcpSignal.MeasurementMode.Value = CcpXcpMeasurementMode.DAQ;
										ccpXcpSignal.DaqEventId.Value = (uint)ushort.Parse(array[1]);
									}
									else if (array.Length > 3 && !string.IsNullOrEmpty(array[2]) && !string.IsNullOrEmpty(array[3]))
									{
										ccpXcpSignal.MeasurementMode.Value = CcpXcpMeasurementMode.Polling;
										ccpXcpSignal.PollingTime.Value = array[3];
									}
									signalsToSelect.Add(ccpXcpSignal);
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
				InformMessageBox.Error(Resources.FileOpenError, new string[]
				{
					file
				});
			}
			this.gridViewCcpXcpSignalRequests.EndDataUpdate();
			this.ValidateInput(true);
		}

		private void AddSignalsFromFileCanapeSignalList(string file, ActionCcpXcp action, ref List<CcpXcpSignal> signalsToSelect)
		{
			this.gridViewCcpXcpSignalRequests.BeginDataUpdate();
			try
			{
				List<CcpXcpSignal> list = new List<CcpXcpSignal>();
				using (new WaitCursor())
				{
					using (StreamReader streamReader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
					{
						signalsToSelect = new List<CcpXcpSignal>();
						for (int i = 0; i < 4; i++)
						{
							streamReader.ReadLine();
						}
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							string ecu = "";
							string[] array = text.Split(new char[]
							{
								';'
							});
							CcpXcpSignal ccpXcpSignal = null;
							if (array.Length >= 2)
							{
								IList<string> list2 = this.TryGetEcuForSignal(array[0]);
								if (list2.Count == 1)
								{
									ecu = list2[0];
								}
							}
							if (array.Length == 2 && !string.IsNullOrEmpty(array[1]))
							{
								string signal = array[0];
								uint value;
								if (uint.TryParse(array[1], out value))
								{
									ccpXcpSignal = this.AddBlankSignal(signal, ecu, action);
									ccpXcpSignal.MeasurementMode.Value = CcpXcpMeasurementMode.DAQ;
									ccpXcpSignal.DaqEventId.Value = value;
								}
							}
							else if (array.Length == 4 && !string.IsNullOrEmpty(array[2]) && !string.IsNullOrEmpty(array[3]))
							{
								string signal2 = array[0];
								ccpXcpSignal = this.AddBlankSignal(signal2, ecu, action);
								ccpXcpSignal.MeasurementMode.Value = CcpXcpMeasurementMode.Polling;
								ccpXcpSignal.PollingTime.Value = array[3];
							}
							if (ccpXcpSignal != null)
							{
								signalsToSelect.Add(ccpXcpSignal);
							}
							if (ccpXcpSignal != null && string.IsNullOrEmpty(ccpXcpSignal.EcuName.Value))
							{
								list.Add(ccpXcpSignal);
							}
						}
					}
				}
				this.ConflictedSignalsProcessing(list);
			}
			catch (Exception)
			{
				InformMessageBox.Error(Resources.FileOpenError, new string[]
				{
					file
				});
			}
			this.gridViewCcpXcpSignalRequests.EndDataUpdate();
			this.ValidateInput(true);
		}

		private void ConflictedSignalsProcessing(List<CcpXcpSignal> conflictedSignals)
		{
			if (conflictedSignals.Count > 0 && this.databaseConfiguration.ActiveCCPXCPDatabases.Count > 0)
			{
				CcpXcpSignalImportAssistent ccpXcpSignalImportAssistent = new CcpXcpSignalImportAssistent(this.databaseConfiguration.ActiveCCPXCPDatabases);
				if (ccpXcpSignalImportAssistent.ShowDialog() == DialogResult.Yes)
				{
					using (new WaitCursor())
					{
						bool matchCase = ccpXcpSignalImportAssistent.MatchCase;
						string ecu = ccpXcpSignalImportAssistent.SelectedECU;
						Database db = this.databaseConfiguration.CCPXCPDatabases.FirstOrDefault((Database d) => d.CcpXcpEcuDisplayName.Value == ecu);
						foreach (CcpXcpSignal current in conflictedSignals)
						{
							this.TryResolveSignalEcuConflicts(current, db, matchCase);
						}
					}
				}
			}
		}

		public void WriteSignalsToFile(CcpXcpSignalRequestsGrid.SignalExport signalExport)
		{
			IEnumerable<CcpXcpSignal> enumerable = this.signalConfiguration.Signals;
			if (signalExport == CcpXcpSignalRequestsGrid.SignalExport.Active)
			{
				enumerable = from sig in this.signalConfiguration.Signals
				where sig.IsActive.Value && sig.ActionCcpXcp.IsActive.Value
				select sig;
			}
			if (signalExport == CcpXcpSignalRequestsGrid.SignalExport.Selected)
			{
				enumerable = this.TryGetSelectedSignals();
			}
			if (!enumerable.Any<CcpXcpSignal>())
			{
				InformMessageBox.Error(Resources.ErrorNoSignalWasExported);
				return;
			}
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = Resources_Files.FileFilterXCPSignalExport;
				if (DialogResult.OK == saveFileDialog.ShowDialog())
				{
					if (File.Exists(saveFileDialog.FileName))
					{
						File.Delete(saveFileDialog.FileName);
					}
					string extension = Path.GetExtension(saveFileDialog.FileName);
					if (!string.IsNullOrEmpty(extension))
					{
						string value = string.Empty;
						if (extension.EndsWith("lab"))
						{
							value = this.WriteSignalsToFileLab(enumerable);
						}
						else if (extension.EndsWith("cfg"))
						{
							value = this.WriteSignalsToFileCfg(enumerable);
						}
						using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
						{
							streamWriter.Write(value);
						}
					}
				}
			}
		}

		private string WriteSignalsToFileLab(IEnumerable<CcpXcpSignal> signalsToExport)
		{
			string text = string.Empty;
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			foreach (CcpXcpSignal current in signalsToExport)
			{
				string text2 = "";
				text2 += current.Name;
				if (current.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
				{
					text2 = text2 + ";" + current.DaqEventId;
				}
				else
				{
					text2 = text2 + ";;1;" + current.PollingTime;
				}
				if (!dictionary.ContainsKey(current.EcuName.Value))
				{
					dictionary.Add(current.EcuName.Value, new List<string>());
				}
				dictionary[current.EcuName.Value].Add(text2);
			}
			text = text + "[#GLOBAL#]" + Environment.NewLine;
			foreach (KeyValuePair<string, List<string>> current2 in dictionary)
			{
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					"[",
					current2.Key,
					"]",
					Environment.NewLine
				});
				foreach (string current3 in current2.Value)
				{
					text = text + current3 + Environment.NewLine;
				}
			}
			return text;
		}

		private string WriteSignalsToFileCfg(IEnumerable<CcpXcpSignal> signalsToExport)
		{
			string text = "// Selection file" + Environment.NewLine;
			text = text + "// generated with : Vector Logger Configurator Version " + Application.ProductVersion + Environment.NewLine;
			text = text + "* cfg-file-version : 1.0" + Environment.NewLine + Environment.NewLine;
			foreach (CcpXcpSignal current in signalsToExport)
			{
				text += current.Name;
				if (current.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
				{
					text = text + ";" + current.DaqEventId;
				}
				else
				{
					text = text + ";;1;" + current.PollingTime;
				}
				text += Environment.NewLine;
			}
			return text;
		}

		private void CcpXcpSignalRequestsGrid_DragEnter(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void CcpXcpSignalRequestsGrid_DragDrop(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				using (IEnumerator<string> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						this.AddSignalsFromFile(current);
					}
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}

		private bool AcceptFileDrop(DragEventArgs e, out IList<string> acceptedFiles)
		{
			acceptedFiles = null;
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				acceptedFiles = new List<string>();
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				bool result = false;
				string[] array2 = array;
				int i = 0;
				while (i < array2.Length)
				{
					string text = array2[i];
					string extension;
					try
					{
						extension = Path.GetExtension(text);
					}
					catch
					{
						goto IL_9E;
					}
					goto IL_5D;
					IL_9E:
					i++;
					continue;
					IL_5D:
					if (string.Compare(extension, Vocabulary.FileExtensionDotCSV, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotLAB, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotCFG, StringComparison.OrdinalIgnoreCase) == 0)
					{
						acceptedFiles.Add(text);
						result = true;
						goto IL_9E;
					}
					goto IL_9E;
				}
				return result;
			}
			return false;
		}

		private void gridViewCcpXcpSignalRequests_MouseUp(object sender, MouseEventArgs e)
		{
			this.multiSelectEditService.MouseUp(e);
		}

		private void gridViewCcpXcpSignalRequests_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			this.SaveExpandStates();
			if (!this.gridViewCcpXcpSignalRequests.IsGroupRow(e.RowHandle))
			{
				this.multiSelectEditService.CellValueChanged(e);
			}
			if (this.EndMultiEdit())
			{
				this.ValidateInput(true);
			}
			this.DisplayErrors();
		}

		private bool EndMultiEdit()
		{
			if (this.gridViewCcpXcpSignalRequests.SelectedRowsCount <= 1)
			{
				return true;
			}
			if (!this.mMultiEditActive)
			{
				this.mMultiEditActive = true;
				this.mMultiEditCounter = (from CcpXcpSignal sig in this.TryGetSelectedSignals()
				where sig.IsActive.Value
				select sig).Count<CcpXcpSignal>();
			}
			else
			{
				this.mMultiEditCounter--;
				this.mMultiEditActive = (this.mMultiEditCounter > 0);
			}
			return !this.mMultiEditActive;
		}

		private void gridViewCcpXcpSignalRequests_MouseDown(object sender, MouseEventArgs e)
		{
			this.multiSelectEditService.MouseDown(e);
			if ((Control.ModifierKeys & Keys.Control) != Keys.Control && (Control.ModifierKeys & Keys.Shift) != Keys.Shift)
			{
				GridHitInfo gridHitInfo = this.gridViewCcpXcpSignalRequests.CalcHitInfo(e.Location);
				if (gridHitInfo.InRowCell && gridHitInfo.Column.RealColumnEdit is RepositoryItemCheckEdit)
				{
					this.gridViewCcpXcpSignalRequests.ClearSelection();
					this.gridViewCcpXcpSignalRequests.FocusedColumn = gridHitInfo.Column;
					this.gridViewCcpXcpSignalRequests.FocusedRowHandle = gridHitInfo.RowHandle;
					this.gridViewCcpXcpSignalRequests.ShowEditor();
					CheckEdit checkEdit = this.gridViewCcpXcpSignalRequests.ActiveEditor as CheckEdit;
					if (checkEdit != null)
					{
						checkEdit.Toggle();
						DXMouseEventArgs.GetMouseArgs(e).Handled = true;
					}
				}
			}
			this.OnGridMouseDown(e.X, e.Y, e.Button);
		}

		private void gridViewCcpXcpSignalRequests_MouseMove(object sender, MouseEventArgs e)
		{
			this.OnGridMouseMove(e.X, e.Y, e.Button);
		}

		private void repositoryItemDaqEventsComboBox_MouseDown(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseDown(point.X, point.Y, e.Button);
		}

		private void repositoryItemDaqEventsComboBox_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseMove(point.X, point.Y, e.Button);
		}

		private void repositoryItemEcuComboBox_MouseDown(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseDown(point.X, point.Y, e.Button);
		}

		private void repositoryItemEcuComboBox_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseMove(point.X, point.Y, e.Button);
		}

		private void repositoryItemMeasurementModeComboBox_MouseDown(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseDown(point.X, point.Y, e.Button);
		}

		private void repositoryItemMeasurementModeComboBox_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseMove(point.X, point.Y, e.Button);
		}

		private void repositoryItemTextEdit_MouseDown(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseDown(point.X, point.Y, e.Button);
		}

		private void repositoryItemTextEdit_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlCcpXcpSignalRequests.PointToClient(Control.MousePosition);
			this.OnGridMouseMove(point.X, point.Y, e.Button);
		}

		private void OnGridMouseDown(int x, int y, MouseButtons mouseButtons)
		{
			this.downHitInfo = null;
			GridHitInfo gridHitInfo = this.gridViewCcpXcpSignalRequests.CalcHitInfo(new Point(x, y));
			if (Control.ModifierKeys != Keys.None)
			{
				return;
			}
			if (mouseButtons == MouseButtons.Left && gridHitInfo.InRow && gridHitInfo.RowHandle != -2147483647)
			{
				this.downHitInfo = gridHitInfo;
			}
		}

		private void OnGridMouseMove(int x, int y, MouseButtons mouseButtons)
		{
			if (mouseButtons == MouseButtons.Left && this.downHitInfo != null)
			{
				Size dragSize = SystemInformation.DragSize;
				Rectangle rectangle = new Rectangle(new Point(this.downHitInfo.HitPoint.X - dragSize.Width / 2, this.downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);
				if (!rectangle.Contains(new Point(x, y)))
				{
					this.gridControlCcpXcpSignalRequests.DoDragDrop(this.downHitInfo, DragDropEffects.All);
					this.downHitInfo = null;
				}
			}
		}

		private void gridControlCcpXcpSignalRequests_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(GridHitInfo)))
			{
				if (!(e.Data.GetData(typeof(GridHitInfo)) is GridHitInfo))
				{
					return;
				}
				GridHitInfo gridHitInfo = this.gridViewCcpXcpSignalRequests.CalcHitInfo(this.gridControlCcpXcpSignalRequests.PointToClient(new Point(e.X, e.Y)));
				CcpXcpSignal ccpXcpSignal = null;
				if (this.TryGetSignal(this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(this.downHitInfo.RowHandle), out ccpXcpSignal) && ccpXcpSignal is CcpXcpSignalDummy && this.gridViewCcpXcpSignalRequests.IsDataRow(this.downHitInfo.RowHandle))
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				if (gridHitInfo.InRow && gridHitInfo.RowHandle != this.downHitInfo.RowHandle && gridHitInfo.RowHandle != -2147483647)
				{
					if ((!this.gridViewCcpXcpSignalRequests.IsDataRow(this.downHitInfo.RowHandle) && this.gridViewCcpXcpSignalRequests.IsDataRow(gridHitInfo.RowHandle) && this.gridViewCcpXcpSignalRequests.GetParentRowHandle(gridHitInfo.RowHandle) == this.downHitInfo.RowHandle) || (this.gridViewCcpXcpSignalRequests.IsDataRow(this.downHitInfo.RowHandle) && !this.gridViewCcpXcpSignalRequests.IsDataRow(gridHitInfo.RowHandle) && this.gridViewCcpXcpSignalRequests.GetParentRowHandle(this.downHitInfo.RowHandle) == gridHitInfo.RowHandle))
					{
						e.Effect = DragDropEffects.None;
						return;
					}
					e.Effect = DragDropEffects.Move;
					return;
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
		}

		private void gridControlCcpXcpSignalRequests_DragDrop(object sender, DragEventArgs e)
		{
			GridHitInfo gridHitInfo = this.gridViewCcpXcpSignalRequests.CalcHitInfo(this.gridControlCcpXcpSignalRequests.PointToClient(new Point(e.X, e.Y)));
			if (gridHitInfo != null)
			{
				List<CcpXcpSignal> signals = this.TryGetSelectedSignals();
				int rowHandle = gridHitInfo.RowHandle;
				ActionCcpXcp targetAction = null;
				if (this.TryGetAction(out targetAction, rowHandle))
				{
					this.MoveRows(signals, targetAction);
				}
			}
		}

		private void MoveRows(List<CcpXcpSignal> signals, ActionCcpXcp targetAction)
		{
			this.SaveExpandStates();
			this.gridViewCcpXcpSignalRequests.BeginDataUpdate();
			foreach (CcpXcpSignal current in signals)
			{
				this.CcpXcpSignalConfiguration.RemoveSignal(current, current.ActionCcpXcp);
				this.CcpXcpSignalConfiguration.AddSignal(current, targetAction);
			}
			this.gridViewCcpXcpSignalRequests.EndDataUpdate();
			this.ValidateInput(true);
			this.SelectRowsOfSignals(signals);
		}

		public void SelectRowsOfSignals(List<CcpXcpSignal> signalsToSelect)
		{
			this.gridViewCcpXcpSignalRequests.ClearSelection();
			int num = -1;
			ReadOnlyCollection<CcpXcpSignal> signals = this.CcpXcpSignalConfiguration.Signals;
			for (int i = 0; i < this.gridViewCcpXcpSignalRequests.RowCount; i++)
			{
				int visibleRowHandle = this.gridViewCcpXcpSignalRequests.GetVisibleRowHandle(i);
				if (this.gridViewCcpXcpSignalRequests.IsDataRow(visibleRowHandle))
				{
					CcpXcpSignal item = signals[this.gridViewCcpXcpSignalRequests.GetDataSourceRowIndex(visibleRowHandle)];
					if (signalsToSelect.Contains(item))
					{
						if (num < 0)
						{
							num = visibleRowHandle;
						}
						this.gridViewCcpXcpSignalRequests.SelectRow(visibleRowHandle);
						signalsToSelect.Remove(item);
					}
				}
			}
			if (num >= 0)
			{
				this.gridViewCcpXcpSignalRequests.FocusedRowHandle = num;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CcpXcpSignalRequestsGrid));
			this.gridControlCcpXcpSignalRequests = new GridControl();
			this.gridViewCcpXcpSignalRequests = new GridView();
			this.colEvent = new GridColumn();
			this.colActive = new GridColumn();
			this.repositoryItemCheckEditIsEnabled = new RepositoryItemCheckEdit();
			this.colName = new GridColumn();
			this.colEcu = new GridColumn();
			this.repositoryItemEcuComboBox = new RepositoryItemComboBox();
			this.colByteCount = new GridColumn();
			this.colMeasurementMode = new GridColumn();
			this.repositoryItemMeasurementModeComboBox = new RepositoryItemComboBox();
			this.colCycle = new GridColumn();
			this.repositoryItemDaqEventsComboBox = new RepositoryItemComboBox();
			this.repositoryItemTextEdit = new RepositoryItemTextEdit();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.repositoryItemCheckEditDummy = new RepositoryItemCheckEdit();
			this.mXtraToolTipController = new XtraToolTipController(this.components);
			this.ccpXcpDatabaseBindingSource = new BindingSource(this.components);
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.gridControlCcpXcpSignalRequests).BeginInit();
			((ISupportInitialize)this.gridViewCcpXcpSignalRequests).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).BeginInit();
			((ISupportInitialize)this.repositoryItemEcuComboBox).BeginInit();
			((ISupportInitialize)this.repositoryItemMeasurementModeComboBox).BeginInit();
			((ISupportInitialize)this.repositoryItemDaqEventsComboBox).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEdit).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditDummy).BeginInit();
			((ISupportInitialize)this.ccpXcpDatabaseBindingSource).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.gridControlCcpXcpSignalRequests.AllowDrop = true;
			componentResourceManager.ApplyResources(this.gridControlCcpXcpSignalRequests, "gridControlCcpXcpSignalRequests");
			this.gridControlCcpXcpSignalRequests.MainView = this.gridViewCcpXcpSignalRequests;
			this.gridControlCcpXcpSignalRequests.Name = "gridControlCcpXcpSignalRequests";
			this.gridControlCcpXcpSignalRequests.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemDaqEventsComboBox,
				this.repositoryItemCheckEditIsEnabled,
				this.repositoryItemTextEdit,
				this.repositoryItemMeasurementModeComboBox,
				this.repositoryItemEcuComboBox,
				this.repositoryItemTextEditDummy,
				this.repositoryItemCheckEditDummy
			});
			this.gridControlCcpXcpSignalRequests.ToolTipController = this.mXtraToolTipController;
			this.gridControlCcpXcpSignalRequests.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewCcpXcpSignalRequests
			});
			this.gridControlCcpXcpSignalRequests.DragDrop += new DragEventHandler(this.gridControlCcpXcpSignalRequests_DragDrop);
			this.gridControlCcpXcpSignalRequests.DragOver += new DragEventHandler(this.gridControlCcpXcpSignalRequests_DragOver);
			this.gridViewCcpXcpSignalRequests.Columns.AddRange(new GridColumn[]
			{
				this.colEvent,
				this.colActive,
				this.colName,
				this.colEcu,
				this.colByteCount,
				this.colMeasurementMode,
				this.colCycle
			});
			this.gridViewCcpXcpSignalRequests.GridControl = this.gridControlCcpXcpSignalRequests;
			this.gridViewCcpXcpSignalRequests.GroupCount = 1;
			componentResourceManager.ApplyResources(this.gridViewCcpXcpSignalRequests, "gridViewCcpXcpSignalRequests");
			this.gridViewCcpXcpSignalRequests.Name = "gridViewCcpXcpSignalRequests";
			this.gridViewCcpXcpSignalRequests.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewCcpXcpSignalRequests.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewCcpXcpSignalRequests.OptionsBehavior.AutoExpandAllGroups = true;
			this.gridViewCcpXcpSignalRequests.OptionsBehavior.AutoSelectAllInEditor = false;
			this.gridViewCcpXcpSignalRequests.OptionsBehavior.CacheValuesOnRowUpdating = CacheRowValuesMode.Disabled;
			this.gridViewCcpXcpSignalRequests.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.gridViewCcpXcpSignalRequests.OptionsCustomization.AllowColumnMoving = false;
			this.gridViewCcpXcpSignalRequests.OptionsCustomization.AllowFilter = false;
			this.gridViewCcpXcpSignalRequests.OptionsCustomization.AllowGroup = false;
			this.gridViewCcpXcpSignalRequests.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewCcpXcpSignalRequests.OptionsDetail.EnableMasterViewMode = false;
			this.gridViewCcpXcpSignalRequests.OptionsSelection.MultiSelect = true;
			this.gridViewCcpXcpSignalRequests.OptionsView.ShowGroupPanel = false;
			this.gridViewCcpXcpSignalRequests.OptionsView.ShowIndicator = false;
			this.gridViewCcpXcpSignalRequests.PaintStyleName = "WindowsXP";
			this.gridViewCcpXcpSignalRequests.RowHeight = 20;
			this.gridViewCcpXcpSignalRequests.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.colEvent, ColumnSortOrder.Ascending),
				new GridColumnSortInfo(this.colName, ColumnSortOrder.Ascending)
			});
			this.gridViewCcpXcpSignalRequests.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewCCPXCPDatabases_CustomDrawCell);
			this.gridViewCcpXcpSignalRequests.CustomDrawGroupRow += new RowObjectCustomDrawEventHandler(this.gridViewCcpXcpSignalRequests_CustomDrawGroupRow);
			this.gridViewCcpXcpSignalRequests.RowCellStyle += new RowCellStyleEventHandler(this.gridViewCcpXcpDescriptions_RowCellStyle);
			this.gridViewCcpXcpSignalRequests.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewCcpXcpDescriptions_CustomRowCellEdit);
			this.gridViewCcpXcpSignalRequests.LeftCoordChanged += new EventHandler(this.gridViewCcpXcpSignalRequests_LeftCoordChanged);
			this.gridViewCcpXcpSignalRequests.TopRowChanged += new EventHandler(this.gridViewCcpXcpSignalRequests_TopRowChanged);
			this.gridViewCcpXcpSignalRequests.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewCCPXCPDatabases_PopupMenuShowing);
			this.gridViewCcpXcpSignalRequests.GroupRowExpanded += new RowEventHandler(this.gridViewCcpXcpSignalRequests_GroupRowExpanded);
			this.gridViewCcpXcpSignalRequests.GroupRowCollapsing += new RowAllowEventHandler(this.gridViewCcpXcpSignalRequests_GroupRowCollapsing);
			this.gridViewCcpXcpSignalRequests.GroupRowExpanding += new RowAllowEventHandler(this.gridViewCcpXcpSignalRequests_GroupRowExpanding);
			this.gridViewCcpXcpSignalRequests.CustomColumnGroup += new CustomColumnSortEventHandler(this.gridViewCcpXcpSignalRequests_CustomColumnGroup);
			this.gridViewCcpXcpSignalRequests.ShowingEditor += new CancelEventHandler(this.gridViewCCPXCPDatabases_ShowingEditor);
			this.gridViewCcpXcpSignalRequests.ShownEditor += new EventHandler(this.gridViewCcpXcpSignalRequests_ShownEditor);
			this.gridViewCcpXcpSignalRequests.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewCcpXcpSignalRequests_FocusedRowChanged);
			this.gridViewCcpXcpSignalRequests.CellValueChanged += new CellValueChangedEventHandler(this.gridViewCcpXcpSignalRequests_CellValueChanged);
			this.gridViewCcpXcpSignalRequests.CustomColumnSort += new CustomColumnSortEventHandler(this.gridViewCcpXcpSignalRequests_CustomColumnSort);
			this.gridViewCcpXcpSignalRequests.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewCCPXCPDatabases_CustomUnboundColumnData);
			this.gridViewCcpXcpSignalRequests.KeyDown += new KeyEventHandler(this.gridViewCCPXCPDatabases_KeyDown);
			this.gridViewCcpXcpSignalRequests.MouseDown += new MouseEventHandler(this.gridViewCcpXcpSignalRequests_MouseDown);
			this.gridViewCcpXcpSignalRequests.MouseUp += new MouseEventHandler(this.gridViewCcpXcpSignalRequests_MouseUp);
			this.gridViewCcpXcpSignalRequests.MouseMove += new MouseEventHandler(this.gridViewCcpXcpSignalRequests_MouseMove);
			this.gridViewCcpXcpSignalRequests.DoubleClick += new EventHandler(this.gridViewCcpXcpSignalRequests_DoubleClick);
			componentResourceManager.ApplyResources(this.colEvent, "colEvent");
			this.colEvent.FieldName = "colEvent";
			this.colEvent.Name = "colEvent";
			this.colEvent.OptionsColumn.AllowEdit = false;
			this.colEvent.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colEvent.SortMode = ColumnSortMode.Custom;
			this.colEvent.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colActive, "colActive");
			this.colActive.ColumnEdit = this.repositoryItemCheckEditIsEnabled;
			this.colActive.FieldName = "colActive";
			this.colActive.Name = "colActive";
			this.colActive.UnboundType = UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsEnabled, "repositoryItemCheckEditIsEnabled");
			this.repositoryItemCheckEditIsEnabled.Name = "repositoryItemCheckEditIsEnabled";
			this.repositoryItemCheckEditIsEnabled.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.colName, "colName");
			this.colName.FieldName = "colName";
			this.colName.Name = "colName";
			this.colName.OptionsColumn.AllowEdit = false;
			this.colName.OptionsColumn.AllowGroup = DefaultBoolean.False;
			this.colName.OptionsColumn.AllowMerge = DefaultBoolean.False;
			this.colName.OptionsColumn.AllowMove = false;
			this.colName.OptionsColumn.ReadOnly = true;
			this.colName.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colEcu, "colEcu");
			this.colEcu.ColumnEdit = this.repositoryItemEcuComboBox;
			this.colEcu.FieldName = "colEcu";
			this.colEcu.Name = "colEcu";
			this.colEcu.OptionsColumn.AllowGroup = DefaultBoolean.False;
			this.colEcu.OptionsColumn.AllowMerge = DefaultBoolean.False;
			this.colEcu.OptionsColumn.AllowMove = false;
			this.colEcu.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemEcuComboBox, "repositoryItemEcuComboBox");
			this.repositoryItemEcuComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemEcuComboBox.Buttons"))
			});
			this.repositoryItemEcuComboBox.Name = "repositoryItemEcuComboBox";
			this.repositoryItemEcuComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemEcuComboBox.SelectedIndexChanged += new EventHandler(this.repositoryItemEcuComboBox_SelectedIndexChanged);
			this.repositoryItemEcuComboBox.MouseDown += new MouseEventHandler(this.repositoryItemEcuComboBox_MouseDown);
			this.repositoryItemEcuComboBox.MouseMove += new MouseEventHandler(this.repositoryItemEcuComboBox_MouseMove);
			this.colByteCount.AppearanceCell.Options.UseTextOptions = true;
			this.colByteCount.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
			componentResourceManager.ApplyResources(this.colByteCount, "colByteCount");
			this.colByteCount.FieldName = "colByteCount";
			this.colByteCount.Name = "colByteCount";
			this.colByteCount.OptionsColumn.AllowEdit = false;
			this.colByteCount.OptionsColumn.AllowMove = false;
			this.colByteCount.OptionsColumn.ReadOnly = true;
			this.colByteCount.UnboundType = UnboundColumnType.Integer;
			componentResourceManager.ApplyResources(this.colMeasurementMode, "colMeasurementMode");
			this.colMeasurementMode.ColumnEdit = this.repositoryItemMeasurementModeComboBox;
			this.colMeasurementMode.FieldName = "colMeasurementMode";
			this.colMeasurementMode.Name = "colMeasurementMode";
			this.colMeasurementMode.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemMeasurementModeComboBox, "repositoryItemMeasurementModeComboBox");
			this.repositoryItemMeasurementModeComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemMeasurementModeComboBox.Buttons"))
			});
			this.repositoryItemMeasurementModeComboBox.Name = "repositoryItemMeasurementModeComboBox";
			this.repositoryItemMeasurementModeComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemMeasurementModeComboBox.SelectedIndexChanged += new EventHandler(this.repositoryItemMeasurementModeComboBox_SelectedIndexChanged);
			this.repositoryItemMeasurementModeComboBox.MouseDown += new MouseEventHandler(this.repositoryItemMeasurementModeComboBox_MouseDown);
			this.repositoryItemMeasurementModeComboBox.MouseMove += new MouseEventHandler(this.repositoryItemMeasurementModeComboBox_MouseMove);
			componentResourceManager.ApplyResources(this.colCycle, "colCycle");
			this.colCycle.FieldName = "colCycle";
			this.colCycle.Name = "colCycle";
			this.colCycle.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemDaqEventsComboBox, "repositoryItemDaqEventsComboBox");
			this.repositoryItemDaqEventsComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemDaqEventsComboBox.Buttons"))
			});
			this.repositoryItemDaqEventsComboBox.Name = "repositoryItemDaqEventsComboBox";
			this.repositoryItemDaqEventsComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemDaqEventsComboBox.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			this.repositoryItemDaqEventsComboBox.MouseDown += new MouseEventHandler(this.repositoryItemDaqEventsComboBox_MouseDown);
			this.repositoryItemDaqEventsComboBox.MouseMove += new MouseEventHandler(this.repositoryItemDaqEventsComboBox_MouseMove);
			componentResourceManager.ApplyResources(this.repositoryItemTextEdit, "repositoryItemTextEdit");
			this.repositoryItemTextEdit.Mask.EditMask = componentResourceManager.GetString("repositoryItemTextEdit.Mask.EditMask");
			this.repositoryItemTextEdit.Mask.MaskType = (MaskType)componentResourceManager.GetObject("repositoryItemTextEdit.Mask.MaskType");
			this.repositoryItemTextEdit.Name = "repositoryItemTextEdit";
			this.repositoryItemTextEdit.MouseDown += new MouseEventHandler(this.repositoryItemTextEdit_MouseDown);
			this.repositoryItemTextEdit.MouseMove += new MouseEventHandler(this.repositoryItemTextEdit_MouseMove);
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
			this.repositoryItemTextEditDummy.ReadOnly = true;
			this.repositoryItemCheckEditDummy.AllowGrayed = true;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditDummy, "repositoryItemCheckEditDummy");
			this.repositoryItemCheckEditDummy.Name = "repositoryItemCheckEditDummy";
			this.repositoryItemCheckEditDummy.NullStyle = StyleIndeterminate.Inactive;
			this.repositoryItemCheckEditDummy.ReadOnly = true;
			this.mXtraToolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.BackColor");
			this.mXtraToolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.ForeColor");
			this.mXtraToolTipController.Appearance.Options.UseBackColor = true;
			this.mXtraToolTipController.Appearance.Options.UseForeColor = true;
			this.mXtraToolTipController.Appearance.Options.UseTextOptions = true;
			this.mXtraToolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.None;
			this.mXtraToolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.BackColor");
			this.mXtraToolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.ForeColor");
			this.mXtraToolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.mXtraToolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.None;
			this.mXtraToolTipController.MaxWidth = 500;
			this.mXtraToolTipController.ShowPrefix = false;
			this.mXtraToolTipController.UseNativeLookAndFeel = true;
			this.mXtraToolTipController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipController_GetActiveObjectInfo);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			this.AllowDrop = true;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.gridControlCcpXcpSignalRequests);
			base.Name = "CcpXcpSignalRequestsGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.DragDrop += new DragEventHandler(this.CcpXcpSignalRequestsGrid_DragDrop);
			base.DragEnter += new DragEventHandler(this.CcpXcpSignalRequestsGrid_DragEnter);
			base.Resize += new EventHandler(this.CcpXcpSignalRequestsGrid_Resize);
			((ISupportInitialize)this.gridControlCcpXcpSignalRequests).EndInit();
			((ISupportInitialize)this.gridViewCcpXcpSignalRequests).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).EndInit();
			((ISupportInitialize)this.repositoryItemEcuComboBox).EndInit();
			((ISupportInitialize)this.repositoryItemMeasurementModeComboBox).EndInit();
			((ISupportInitialize)this.repositoryItemDaqEventsComboBox).EndInit();
			((ISupportInitialize)this.repositoryItemTextEdit).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditDummy).EndInit();
			((ISupportInitialize)this.ccpXcpDatabaseBindingSource).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
