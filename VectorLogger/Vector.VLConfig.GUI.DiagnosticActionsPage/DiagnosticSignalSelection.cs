using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.DiagSymbols;
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.GUI.DiagnosticActionsPage
{
	public class DiagnosticSignalSelection : Form
	{
		public delegate void FocusedSignalApplied(DiagnosticSignalRequest signal, bool insertAtEnd);

		internal class SessionTypePresenter
		{
			public SessionType SessionType
			{
				get;
				private set;
			}

			public SessionTypePresenter(SessionType sessionType)
			{
				this.SessionType = sessionType;
			}

			public override string ToString()
			{
				SessionType sessionType = this.SessionType;
				switch (sessionType)
				{
				case SessionType.kST_Undefined:
					return Vocabulary.DiagnosticSessionTypeUndefined;
				case SessionType.kST_Default:
					return Vocabulary.DiagnosticSessionTypeDefault;
				case SessionType.kST_Extended:
					return Vocabulary.DiagnosticSessionTypeExtended;
				case SessionType.kST_Dynamic:
					return Vocabulary.DiagnosticSessionTypeDynamic;
				default:
					if (sessionType != SessionType.kST_Unknown)
					{
						return string.Empty;
					}
					return Vocabulary.DiagnosticSessionTypeUnknown;
				}
			}

			public DiagSessionType GetDiagSessionType()
			{
				switch (this.SessionType)
				{
				case SessionType.kST_Default:
					return DiagSessionType.Default;
				case SessionType.kST_Extended:
					return DiagSessionType.Extended;
				default:
					return DiagSessionType.Unknown;
				}
			}
		}

		internal class ServicePresenter
		{
			private IDsGenericService mService;

			public string Qualifier
			{
				get
				{
					if (this.mService == null)
					{
						return string.Empty;
					}
					return this.mService.Qualifier;
				}
			}

			public ServicePresenter(IDsGenericService service)
			{
				this.mService = service;
			}

			public override string ToString()
			{
				return this.mService.Name + " (" + this.mService.Qualifier + ")";
			}

			public SessionType GetMinRequiredSession(ref byte[] byteStream)
			{
				return this.mService.GetMinRequiredSession(ref byteStream);
			}

			public SessionType GetMinRequiredSessionDefault()
			{
				return this.mService.GetMinRequiredSessionDefault();
			}
		}

		internal class DidPresenter
		{
			private IDsDid mDid;

			public string Identifier
			{
				get
				{
					if (this.mDid == null)
					{
						return string.Empty;
					}
					return this.mDid.GetIdentifier();
				}
			}

			public byte[] ReadPdu
			{
				get
				{
					byte[] result;
					if (this.mDid.ComputeReadPdu(out result) == DsResult.Ok)
					{
						return result;
					}
					return new byte[0];
				}
			}

			public DidPresenter(IDsDid did)
			{
				this.mDid = did;
			}

			public override string ToString()
			{
				return this.mDid.GetIdentifier();
			}
		}

		internal class SignalPresenter
		{
			private DiagnosticsECU mDiagnosticEcu;

			private int mHashCode;

			public IDsDid Did;

			public IDsGenericService GenericService;

			public string Ecu
			{
				get;
				set;
			}

			public string Name
			{
				get;
				set;
			}

			public string Qualifier
			{
				get;
				set;
			}

			public string Path
			{
				get;
				set;
			}

			public string DidId
			{
				get;
				private set;
			}

			public string ServiceName
			{
				get;
				private set;
			}

			public string RequestParameterValues
			{
				get;
				private set;
			}

			public SignalPresenter(DiagnosticsECU diagnosticEcu, IDsStaticSignal signal, IDsDid did, IDsGenericService service, string requestParameterValues)
			{
				this.mDiagnosticEcu = diagnosticEcu;
				this.Ecu = diagnosticEcu.Qualifier.Value;
				this.Name = signal.GetName();
				this.Qualifier = signal.GetQualifier();
				this.Path = signal.GetSignalPath();
				this.Did = did;
				this.DidId = did.GetIdentifier();
				this.GenericService = service;
				this.ServiceName = service.Name;
				this.RequestParameterValues = requestParameterValues;
			}

			public override int GetHashCode()
			{
				if (this.mHashCode != 0)
				{
					return this.mHashCode;
				}
				return this.mHashCode = (this.DidId.GetHashCode() ^ this.Qualifier.GetHashCode() ^ this.Name.GetHashCode() ^ this.Ecu.GetHashCode());
			}

			public override bool Equals(object obj)
			{
				if (!(obj is DiagnosticSignalSelection.SignalPresenter))
				{
					return false;
				}
				DiagnosticSignalSelection.SignalPresenter signalPresenter = obj as DiagnosticSignalSelection.SignalPresenter;
				return this.GetHashCode() == (obj as DiagnosticSignalSelection.SignalPresenter).GetHashCode() && this.DidId == signalPresenter.DidId && this.Qualifier == signalPresenter.Qualifier && this.Name == signalPresenter.Name && this.Ecu == signalPresenter.Ecu;
			}

			internal string GetEcuPath()
			{
				if (this.mDiagnosticEcu != null && this.mDiagnosticEcu.Database != null && this.mDiagnosticEcu.Database.FilePath != null)
				{
					return this.mDiagnosticEcu.Database.FilePath.Value;
				}
				return string.Empty;
			}
		}

		private IDiagSymbolsManager mDiagSymbolsManager;

		private DiagnosticsDatabaseConfiguration mDatabaseConfiguration;

		private DiagnosticActionsPage mActionsPageLayout;

		private DiagnosticSignalSelection.FocusedSignalApplied mOnFocusedSignalApplied;

		private bool mInsertAtEnd;

		private readonly GeneralService mGridViewGeneralService;

		private IContainer components;

		private ImageList mImageList1;

		private Button mButtonOk;

		private Button mButtonCancel;

		private Button mButtonHelp;

		private TextBox mTextBoxSignalFilter;

		private GridControl mSignalGrid;

		private GridView mSignalGridView;

		private GridColumn mGridColumnEcu;

		private GridColumn mGridColumnName;

		private GridColumn mGridColumnQualifier;

		private GridColumn mGridColumnPath;

		private Label mLabelSignal;

		private ComboBox mComboBoxSessionType;

		private Label mLabelSessionType;

		private CheckBox mCheckBoxUsePreset;

		private Button mButtonApply;

		private FlowLayoutPanel mFlowLayoutPanel1;

		private GridColumn mGridColumnService;

		private GridColumn mGridColumnRequestParameter;

		private GridColumn mGridColumnDid;

		public DiagnosticSignalRequest FocusedSignal
		{
			get;
			private set;
		}

		public DiagnosticSignalRequest LastSignal
		{
			get;
			private set;
		}

		public DiagnosticSignalSelection(IDiagSymbolsManager diagSymbolsManager, DiagnosticsDatabaseConfiguration databaseConfiguration, DiagnosticActionsPage actionsPageLayout, DiagnosticSignalSelection.FocusedSignalApplied onFocusedSignalApplied, bool insertAtEnd = true)
		{
			this.InitializeComponent();
			this.mGridViewGeneralService = new GeneralService(this.mSignalGridView);
			this.mGridViewGeneralService.InitAppearance();
			this.mDiagSymbolsManager = diagSymbolsManager;
			this.mDatabaseConfiguration = databaseConfiguration;
			this.mActionsPageLayout = actionsPageLayout;
			this.InitializeGrid();
			this.InitializeSessionType();
			this.mOnFocusedSignalApplied = onFocusedSignalApplied;
			this.mInsertAtEnd = insertAtEnd;
			if (this.mOnFocusedSignalApplied == null)
			{
				this.mButtonApply.Visible = false;
			}
			this.mTextBoxSignalFilter.Select();
		}

		public DiagnosticSignalSelection(IDiagSymbolsManager diagSymbolsManager, DiagnosticsDatabaseConfiguration databaseConfiguration, DiagnosticActionsPage actionsPageLayout, DiagnosticSignalRequest focusedSignal, DiagnosticSignalSelection.FocusedSignalApplied focusedSignalApplied) : this(diagSymbolsManager, databaseConfiguration, actionsPageLayout, focusedSignalApplied, true)
		{
			this.FocusedSignal = focusedSignal;
		}

		private void InitializeGrid()
		{
			this.mSignalGrid.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mSignalGrid.LookAndFeel.UseWindowsXPTheme = true;
			this.mSignalGrid.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			Cursor.Current = Cursors.WaitCursor;
			HashSet<DiagnosticSignalSelection.SignalPresenter> hashSet = new HashSet<DiagnosticSignalSelection.SignalPresenter>();
			foreach (DiagnosticsECU current in from ecu in this.mDatabaseConfiguration.ECUs
			orderby ecu.Qualifier.Value
			select ecu)
			{
				IList<IDsDid> list;
				if (this.mDiagSymbolsManager.GetDids(current.Qualifier.Value, current.Variant.Value, out list))
				{
					foreach (IDsDid current2 in list)
					{
						byte[] array;
						IList<IDsGenericService> source;
						if (current2.ComputeReadPdu(out array) == DsResult.Ok && array.Any<byte>() && current2.GetServices(out source) == DsResult.Ok && source.Any<IDsGenericService>())
						{
							List<IDsGenericService> list2 = source.ToList<IDsGenericService>();
							list2.RemoveAll((IDsGenericService ser) => ser.IsWriteService());
							if (list2.Any<IDsGenericService>())
							{
								foreach (IDsGenericService current3 in list2)
								{
									IList<IDsStaticSignal> list3;
									if (current2.GetParams(out list3) == DsResult.Ok)
									{
										foreach (IDsStaticSignal current4 in list3)
										{
											if (current4.IsPositiveResponseParam())
											{
												StringBuilder stringBuilder = new StringBuilder();
												IList<KeyValuePair<string, string>> list4;
												if (this.mDiagSymbolsManager.GetDisassembledMessageParams(current3, array, out list4))
												{
													int num = 0;
													foreach (KeyValuePair<string, string> current5 in list4)
													{
														if (num != 0)
														{
															if (num > 1)
															{
																stringBuilder.Append("; ");
															}
															stringBuilder.Append(current5.Value);
														}
														num++;
													}
												}
												DiagnosticSignalSelection.SignalPresenter item = new DiagnosticSignalSelection.SignalPresenter(current, current4, current2, current3, stringBuilder.ToString());
												hashSet.Add(item);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.mSignalGrid.DataSource = new BindingList<DiagnosticSignalSelection.SignalPresenter>((from sig in hashSet
			orderby sig.Name
			select sig).ToList<DiagnosticSignalSelection.SignalPresenter>());
			Cursor.Current = Cursors.Default;
		}

		private void TextBoxSignalFilter_TextChanged(object sender, EventArgs e)
		{
			if (!this.mTextBoxSignalFilter.Focused)
			{
				return;
			}
			char[] separator = new char[]
			{
				' '
			};
			string[] array = this.mTextBoxSignalFilter.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			List<string> list = new List<string>();
			for (int i = 0; i < this.mSignalGridView.Columns.Count; i++)
			{
				if (this.mSignalGridView.Columns[i].Visible && this.mSignalGridView.Columns[i].FieldName != this.mGridColumnPath.FieldName && this.mSignalGridView.Columns[i].FieldName != this.mGridColumnEcu.FieldName)
				{
					list.Add(this.mSignalGridView.Columns[i].FieldName);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < array.Length; j++)
			{
				if (j > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.Append("(");
				for (int k = 0; k < list.Count; k++)
				{
					if (k > 0)
					{
						stringBuilder.Append("OR");
					}
					stringBuilder.Append("[");
					stringBuilder.Append(list[k]);
					stringBuilder.Append("] LIKE '%");
					stringBuilder.Append(array[j]);
					stringBuilder.Append("%'");
				}
				stringBuilder.Append(")");
			}
			this.mSignalGridView.ActiveFilterString = stringBuilder.ToString();
		}

		private void SignalGridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.mGridViewGeneralService.FocusedRowChanged(e);
			DiagnosticSignalSelection.SignalPresenter signalPresenter = null;
			BindingList<DiagnosticSignalSelection.SignalPresenter> bindingList = this.mSignalGrid.DataSource as BindingList<DiagnosticSignalSelection.SignalPresenter>;
			if (bindingList == null)
			{
				return;
			}
			if (e.FocusedRowHandle >= 0)
			{
				int dataSourceRowIndex = this.mSignalGridView.GetDataSourceRowIndex(e.FocusedRowHandle);
				if (dataSourceRowIndex >= 0 && dataSourceRowIndex < bindingList.Count<DiagnosticSignalSelection.SignalPresenter>())
				{
					signalPresenter = bindingList[dataSourceRowIndex];
				}
			}
			this.mButtonOk.Enabled = (signalPresenter != null);
			if (this.mSignalGrid.Focused && signalPresenter != null)
			{
				this.mTextBoxSignalFilter.Text = signalPresenter.Name;
			}
			this.UpdateSessionType();
			this.mButtonApply.Enabled = (this.mButtonOk.Enabled && this.mOnFocusedSignalApplied != null);
		}

		private void CheckBoxUsePreset_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateSessionType();
		}

		private void ButtonOk_Click(object sender, EventArgs e)
		{
			DiagnosticSignalSelection.SignalPresenter signalPresenter;
			if (this.TryGetFocusedSignalPresenter(out signalPresenter))
			{
				this.FocusedSignal = new DiagnosticSignalRequest(signalPresenter.Qualifier, signalPresenter.DidId, signalPresenter.GenericService.Qualifier, signalPresenter.Ecu, signalPresenter.GetEcuPath(), new DiagnosticSignalSelection.DidPresenter(signalPresenter.Did).ReadPdu, this.GetDiagSessionType());
				if (this.FocusedSignal.Equals(this.LastSignal))
				{
					base.DialogResult = DialogResult.Cancel;
				}
				this.LastSignal = this.FocusedSignal;
			}
			this.Serialize();
		}

		private void ButtonApply_Click(object sender, EventArgs e)
		{
			if (this.mOnFocusedSignalApplied == null)
			{
				return;
			}
			DiagnosticSignalSelection.SignalPresenter signalPresenter;
			if (this.TryGetFocusedSignalPresenter(out signalPresenter))
			{
				this.FocusedSignal = new DiagnosticSignalRequest(signalPresenter.Qualifier, signalPresenter.DidId, signalPresenter.GenericService.Qualifier, signalPresenter.Ecu, signalPresenter.GetEcuPath(), new DiagnosticSignalSelection.DidPresenter(signalPresenter.Did).ReadPdu, this.GetDiagSessionType());
				if (!this.FocusedSignal.Equals(this.LastSignal))
				{
					this.LastSignal = this.FocusedSignal;
					this.mOnFocusedSignalApplied(this.FocusedSignal, this.mInsertAtEnd);
				}
			}
		}

		private void SignalGrid_DoubleClick(object sender, EventArgs e)
		{
			this.ButtonApply_Click(sender, e);
		}

		private void DiagnosticSignalSelection_Load(object sender, EventArgs e)
		{
			this.mSignalGrid.ForceInitialize();
			this.DeSerialize();
			if (this.FocusedSignal == null)
			{
				return;
			}
			this.FocusSignal(this.FocusedSignal);
			this.mTextBoxSignalFilter.Select();
			this.UpdateSessionType();
			this.SelectSessionType(this.FocusedSignal.SessionType.Value);
		}

		private void ButtonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void DiagnosticSignalSelection_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SignalGridView_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			this.mGridViewGeneralService.CustomDrawCell(e, true, false);
		}

		private void SignalGridView_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			this.mGridViewGeneralService.FocusedColumnChanged(e);
		}

		private void SignalGridView_MouseDown(object sender, MouseEventArgs e)
		{
			this.mGridViewGeneralService.MouseDown(e);
		}

		private void SignalGridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				List<GridStringId> list = new List<GridStringId>
				{
					GridStringId.MenuColumnRemoveColumn,
					GridStringId.MenuColumnColumnCustomization,
					GridStringId.MenuColumnBestFit,
					GridStringId.MenuColumnBestFitAllColumns
				};
				foreach (DXMenuItem dXMenuItem in e.Menu.Items)
				{
					if (dXMenuItem.Tag is GridStringId && (GridStringId)dXMenuItem.Tag == GridStringId.MenuColumnRemoveColumn)
					{
						dXMenuItem.BeginGroup = false;
					}
					dXMenuItem.Visible = (dXMenuItem.Tag is GridStringId && list.Contains((GridStringId)dXMenuItem.Tag));
				}
			}
		}

		private bool TryGetFocusedSignalPresenter(out DiagnosticSignalSelection.SignalPresenter presenter)
		{
			presenter = null;
			if (!this.mSignalGridView.IsGroupRow(this.mSignalGridView.FocusedRowHandle))
			{
				int focusedDataSourceRowIndex = this.mSignalGridView.GetFocusedDataSourceRowIndex();
				BindingList<DiagnosticSignalSelection.SignalPresenter> bindingList = this.mSignalGrid.DataSource as BindingList<DiagnosticSignalSelection.SignalPresenter>;
				if (bindingList != null && focusedDataSourceRowIndex >= 0 && focusedDataSourceRowIndex < bindingList.Count<DiagnosticSignalSelection.SignalPresenter>())
				{
					presenter = bindingList[focusedDataSourceRowIndex];
				}
			}
			return presenter != null;
		}

		private void FocusSignal(DiagnosticSignalRequest signal)
		{
			BindingList<DiagnosticSignalSelection.SignalPresenter> bindingList = this.mSignalGrid.DataSource as BindingList<DiagnosticSignalSelection.SignalPresenter>;
			if (bindingList == null)
			{
				return;
			}
			int num = 2147483647;
			int num2 = 2147483647;
			for (int i = 0; i < bindingList.Count<DiagnosticSignalSelection.SignalPresenter>(); i++)
			{
				if (bindingList[i].Qualifier == signal.SignalQualifier.Value && bindingList[i].Ecu == signal.EcuQualifier.Value && bindingList[i].GenericService.Qualifier == signal.ServiceQualifier.Value)
				{
					if (num2 == 2147483647)
					{
						num2 = i;
					}
					if (bindingList[i].Did.GetIdentifier() == signal.DidId.Value)
					{
						num = i;
						break;
					}
				}
			}
			if (num == 2147483647 && num2 != 2147483647)
			{
				num = num2;
			}
			if (num != 2147483647)
			{
				this.mSignalGridView.FocusedRowHandle = this.mSignalGridView.GetRowHandle(num);
				this.mTextBoxSignalFilter.Text = bindingList[num].Name;
			}
		}

		private void SelectSessionType(SessionType sessionType)
		{
			foreach (object current in this.mComboBoxSessionType.Items)
			{
				if (current is DiagnosticSignalSelection.SessionTypePresenter && (current as DiagnosticSignalSelection.SessionTypePresenter).SessionType == sessionType)
				{
					this.mComboBoxSessionType.SelectedItem = (current as DiagnosticSignalSelection.SessionTypePresenter);
					break;
				}
			}
		}

		private void SelectSessionType(DiagSessionType sessionType)
		{
			this.mCheckBoxUsePreset.Checked = (sessionType == DiagSessionType.DynamicFromDB);
			switch (sessionType)
			{
			case DiagSessionType.Default:
				this.SelectSessionType(SessionType.kST_Default);
				return;
			case DiagSessionType.Extended:
				this.SelectSessionType(SessionType.kST_Extended);
				return;
			default:
				return;
			}
		}

		private void InitializeSessionType()
		{
			this.mComboBoxSessionType.Items.Add(new DiagnosticSignalSelection.SessionTypePresenter(SessionType.kST_Default));
			this.mComboBoxSessionType.Items.Add(new DiagnosticSignalSelection.SessionTypePresenter(SessionType.kST_Extended));
			this.mComboBoxSessionType.SelectedIndex = 0;
		}

		private void UpdateSessionType()
		{
			DiagnosticSignalSelection.SignalPresenter signalPresenter;
			if (!this.TryGetFocusedSignalPresenter(out signalPresenter))
			{
				this.mCheckBoxUsePreset.Enabled = false;
				this.mComboBoxSessionType.Enabled = false;
				this.mComboBoxSessionType.Text = string.Empty;
				return;
			}
			byte[] readPdu = new DiagnosticSignalSelection.DidPresenter(signalPresenter.Did).ReadPdu;
			byte[] destinationArray = new byte[readPdu.Length];
			Array.Copy(readPdu, destinationArray, readPdu.Length);
			SessionType minRequiredSession = signalPresenter.GenericService.GetMinRequiredSession(ref destinationArray);
			SessionType minRequiredSessionDefault = signalPresenter.GenericService.GetMinRequiredSessionDefault();
			this.mCheckBoxUsePreset.Enabled = (minRequiredSession == SessionType.kST_Default || minRequiredSession == SessionType.kST_Extended);
			if (!this.mCheckBoxUsePreset.Enabled)
			{
				this.mCheckBoxUsePreset.Checked = false;
			}
			if (this.mCheckBoxUsePreset.Checked)
			{
				this.SelectSessionType(minRequiredSessionDefault);
			}
			this.mComboBoxSessionType.Enabled = (!this.mCheckBoxUsePreset.Enabled || !this.mCheckBoxUsePreset.Checked);
		}

		private DiagSessionType GetDiagSessionType()
		{
			if (this.mCheckBoxUsePreset.Enabled && this.mCheckBoxUsePreset.Checked)
			{
				return DiagSessionType.DynamicFromDB;
			}
			if (this.mComboBoxSessionType.SelectedItem is DiagnosticSignalSelection.SessionTypePresenter)
			{
				return (this.mComboBoxSessionType.SelectedItem as DiagnosticSignalSelection.SessionTypePresenter).GetDiagSessionType();
			}
			return DiagSessionType.Unknown;
		}

		public bool Serialize()
		{
			if (this.mActionsPageLayout == null)
			{
				return false;
			}
			this.mActionsPageLayout.SignalSelectionDialogHeight = base.Height;
			this.mActionsPageLayout.SignalSelectionDialogWidth = base.Width;
			this.mActionsPageLayout.SignalSelectionDialogX = base.Location.X;
			this.mActionsPageLayout.SignalSelectionDialogY = base.Location.Y;
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.mSignalGridView.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				this.mActionsPageLayout.SignalSelectionDialogGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize()
		{
			if (this.mActionsPageLayout == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(this.mActionsPageLayout.SignalSelectionDialogGridLayout))
			{
				return false;
			}
			base.Height = this.mActionsPageLayout.SignalSelectionDialogHeight;
			base.Width = this.mActionsPageLayout.SignalSelectionDialogWidth;
			base.Location = new Point(this.mActionsPageLayout.SignalSelectionDialogX, this.mActionsPageLayout.SignalSelectionDialogY);
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(this.mActionsPageLayout.SignalSelectionDialogGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.mSignalGridView.RestoreLayoutFromStream(memoryStream);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DiagnosticSignalSelection));
			this.mImageList1 = new ImageList(this.components);
			this.mButtonOk = new Button();
			this.mButtonCancel = new Button();
			this.mButtonHelp = new Button();
			this.mTextBoxSignalFilter = new TextBox();
			this.mSignalGrid = new GridControl();
			this.mSignalGridView = new GridView();
			this.mGridColumnEcu = new GridColumn();
			this.mGridColumnName = new GridColumn();
			this.mGridColumnService = new GridColumn();
			this.mGridColumnRequestParameter = new GridColumn();
			this.mGridColumnQualifier = new GridColumn();
			this.mGridColumnDid = new GridColumn();
			this.mGridColumnPath = new GridColumn();
			this.mLabelSignal = new Label();
			this.mComboBoxSessionType = new ComboBox();
			this.mLabelSessionType = new Label();
			this.mCheckBoxUsePreset = new CheckBox();
			this.mButtonApply = new Button();
			this.mFlowLayoutPanel1 = new FlowLayoutPanel();
			((ISupportInitialize)this.mSignalGrid).BeginInit();
			((ISupportInitialize)this.mSignalGridView).BeginInit();
			this.mFlowLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.mImageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("mImageList1.ImageStream");
			this.mImageList1.TransparentColor = Color.Transparent;
			this.mImageList1.Images.SetKeyName(0, "IconA2lDatabases.ico");
			componentResourceManager.ApplyResources(this.mButtonOk, "mButtonOk");
			this.mButtonOk.DialogResult = DialogResult.OK;
			this.mButtonOk.Name = "mButtonOk";
			this.mButtonOk.UseVisualStyleBackColor = true;
			this.mButtonOk.Click += new EventHandler(this.ButtonOk_Click);
			componentResourceManager.ApplyResources(this.mButtonCancel, "mButtonCancel");
			this.mButtonCancel.DialogResult = DialogResult.Cancel;
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mButtonHelp, "mButtonHelp");
			this.mButtonHelp.Name = "mButtonHelp";
			this.mButtonHelp.UseVisualStyleBackColor = true;
			this.mButtonHelp.Click += new EventHandler(this.ButtonHelp_Click);
			componentResourceManager.ApplyResources(this.mTextBoxSignalFilter, "mTextBoxSignalFilter");
			this.mTextBoxSignalFilter.Name = "mTextBoxSignalFilter";
			this.mTextBoxSignalFilter.TextChanged += new EventHandler(this.TextBoxSignalFilter_TextChanged);
			componentResourceManager.ApplyResources(this.mSignalGrid, "mSignalGrid");
			this.mSignalGrid.MainView = this.mSignalGridView;
			this.mSignalGrid.Name = "mSignalGrid";
			this.mSignalGrid.ViewCollection.AddRange(new BaseView[]
			{
				this.mSignalGridView
			});
			this.mSignalGrid.DoubleClick += new EventHandler(this.SignalGrid_DoubleClick);
			this.mSignalGridView.Columns.AddRange(new GridColumn[]
			{
				this.mGridColumnEcu,
				this.mGridColumnName,
				this.mGridColumnService,
				this.mGridColumnRequestParameter,
				this.mGridColumnQualifier,
				this.mGridColumnDid,
				this.mGridColumnPath
			});
			this.mSignalGridView.FocusRectStyle = DrawFocusRectStyle.None;
			this.mSignalGridView.GridControl = this.mSignalGrid;
			this.mSignalGridView.GroupCount = 1;
			this.mSignalGridView.Name = "mSignalGridView";
			this.mSignalGridView.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.mSignalGridView.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.mSignalGridView.OptionsBehavior.AutoExpandAllGroups = true;
			this.mSignalGridView.OptionsBehavior.Editable = false;
			this.mSignalGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.mSignalGridView.OptionsBehavior.ImmediateUpdateRowPosition = false;
			this.mSignalGridView.OptionsBehavior.ReadOnly = true;
			this.mSignalGridView.OptionsCustomization.AllowFilter = false;
			this.mSignalGridView.OptionsCustomization.AllowGroup = false;
			this.mSignalGridView.OptionsCustomization.AllowSort = false;
			this.mSignalGridView.OptionsFilter.AllowFilterEditor = false;
			this.mSignalGridView.OptionsFind.AllowFindPanel = false;
			this.mSignalGridView.OptionsLayout.Columns.StoreAppearance = true;
			this.mSignalGridView.OptionsLayout.StoreAppearance = true;
			this.mSignalGridView.OptionsLayout.StoreDataSettings = false;
			this.mSignalGridView.OptionsNavigation.UseTabKey = false;
			this.mSignalGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.mSignalGridView.OptionsView.RowAutoHeight = true;
			this.mSignalGridView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mSignalGridView.OptionsView.ShowGroupPanel = false;
			this.mSignalGridView.OptionsView.ShowHorizontalLines = DefaultBoolean.False;
			this.mSignalGridView.OptionsView.ShowIndicator = false;
			this.mSignalGridView.OptionsView.ShowVerticalLines = DefaultBoolean.False;
			this.mSignalGridView.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.mGridColumnEcu, ColumnSortOrder.Ascending)
			});
			this.mSignalGridView.CustomDrawCell += new RowCellCustomDrawEventHandler(this.SignalGridView_CustomDrawCell);
			this.mSignalGridView.PopupMenuShowing += new PopupMenuShowingEventHandler(this.SignalGridView_PopupMenuShowing);
			this.mSignalGridView.FocusedRowChanged += new FocusedRowChangedEventHandler(this.SignalGridView_FocusedRowChanged);
			this.mSignalGridView.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this.SignalGridView_FocusedColumnChanged);
			this.mSignalGridView.MouseDown += new MouseEventHandler(this.SignalGridView_MouseDown);
			componentResourceManager.ApplyResources(this.mGridColumnEcu, "mGridColumnEcu");
			this.mGridColumnEcu.FieldName = "Ecu";
			this.mGridColumnEcu.Name = "mGridColumnEcu";
			componentResourceManager.ApplyResources(this.mGridColumnName, "mGridColumnName");
			this.mGridColumnName.FieldName = "Name";
			this.mGridColumnName.Name = "mGridColumnName";
			componentResourceManager.ApplyResources(this.mGridColumnService, "mGridColumnService");
			this.mGridColumnService.FieldName = "ServiceName";
			this.mGridColumnService.Name = "mGridColumnService";
			componentResourceManager.ApplyResources(this.mGridColumnRequestParameter, "mGridColumnRequestParameter");
			this.mGridColumnRequestParameter.FieldName = "RequestParameterValues";
			this.mGridColumnRequestParameter.Name = "mGridColumnRequestParameter";
			componentResourceManager.ApplyResources(this.mGridColumnQualifier, "mGridColumnQualifier");
			this.mGridColumnQualifier.FieldName = "Qualifier";
			this.mGridColumnQualifier.Name = "mGridColumnQualifier";
			componentResourceManager.ApplyResources(this.mGridColumnDid, "mGridColumnDid");
			this.mGridColumnDid.FieldName = "DidId";
			this.mGridColumnDid.Name = "mGridColumnDid";
			componentResourceManager.ApplyResources(this.mGridColumnPath, "mGridColumnPath");
			this.mGridColumnPath.FieldName = "Path";
			this.mGridColumnPath.Name = "mGridColumnPath";
			componentResourceManager.ApplyResources(this.mLabelSignal, "mLabelSignal");
			this.mLabelSignal.Name = "mLabelSignal";
			componentResourceManager.ApplyResources(this.mComboBoxSessionType, "mComboBoxSessionType");
			this.mComboBoxSessionType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxSessionType.Name = "mComboBoxSessionType";
			componentResourceManager.ApplyResources(this.mLabelSessionType, "mLabelSessionType");
			this.mLabelSessionType.Name = "mLabelSessionType";
			componentResourceManager.ApplyResources(this.mCheckBoxUsePreset, "mCheckBoxUsePreset");
			this.mCheckBoxUsePreset.Checked = true;
			this.mCheckBoxUsePreset.CheckState = CheckState.Checked;
			this.mCheckBoxUsePreset.Name = "mCheckBoxUsePreset";
			this.mCheckBoxUsePreset.UseVisualStyleBackColor = true;
			this.mCheckBoxUsePreset.CheckedChanged += new EventHandler(this.CheckBoxUsePreset_CheckedChanged);
			componentResourceManager.ApplyResources(this.mButtonApply, "mButtonApply");
			this.mButtonApply.Name = "mButtonApply";
			this.mButtonApply.UseVisualStyleBackColor = true;
			this.mButtonApply.Click += new EventHandler(this.ButtonApply_Click);
			componentResourceManager.ApplyResources(this.mFlowLayoutPanel1, "mFlowLayoutPanel1");
			this.mFlowLayoutPanel1.Controls.Add(this.mButtonOk);
			this.mFlowLayoutPanel1.Controls.Add(this.mButtonApply);
			this.mFlowLayoutPanel1.Controls.Add(this.mButtonCancel);
			this.mFlowLayoutPanel1.Controls.Add(this.mButtonHelp);
			this.mFlowLayoutPanel1.Name = "mFlowLayoutPanel1";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.mButtonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.mCheckBoxUsePreset);
			base.Controls.Add(this.mComboBoxSessionType);
			base.Controls.Add(this.mLabelSessionType);
			base.Controls.Add(this.mLabelSignal);
			base.Controls.Add(this.mSignalGrid);
			base.Controls.Add(this.mTextBoxSignalFilter);
			base.Controls.Add(this.mFlowLayoutPanel1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DiagnosticSignalSelection";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.Load += new EventHandler(this.DiagnosticSignalSelection_Load);
			base.HelpRequested += new HelpEventHandler(this.DiagnosticSignalSelection_HelpRequested);
			((ISupportInitialize)this.mSignalGrid).EndInit();
			((ISupportInitialize)this.mSignalGridView).EndInit();
			this.mFlowLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
