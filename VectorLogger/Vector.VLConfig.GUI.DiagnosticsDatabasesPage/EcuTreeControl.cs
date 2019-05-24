using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DiagnosticsDatabasesPage
{
	internal class EcuTreeControl : UserControl
	{
		private readonly string _ImageKeyEcu = "IconECU";

		private readonly string _ImageKeyError = "IconECUWarning";

		private readonly string _ImageKeyDb = "Database";

		private Dictionary<TreeNode, string> toolTipErrorTexts;

		public EventHandler OnSelectEcu;

		public EventHandler<TreeViewCancelEventArgs> OnBeforeSelect;

		private IContainer components;

		private TreeView treeView;

		private ImageList imageList;

		private ToolTip toolTip;

		public int SelectedEcuIndex
		{
			get
			{
				int num = 0;
				foreach (TreeNode treeNode in this.treeView.Nodes)
				{
					foreach (TreeNode treeNode2 in treeNode.Nodes)
					{
						if (this.treeView.SelectedNode == treeNode2)
						{
							return num;
						}
						num++;
					}
				}
				return -1;
			}
		}

		public DiagnosticsDatabaseConfigurationInternal DiagnosticsDatabaseConfiguration
		{
			get;
			set;
		}

		public EcuTreeControl()
		{
			this.InitializeComponent();
			this.toolTipErrorTexts = new Dictionary<TreeNode, string>();
		}

		public void Init()
		{
			this.treeView.Nodes.Clear();
			this.treeView.SelectedNode = null;
			if (this.DiagnosticsDatabaseConfiguration != null)
			{
				foreach (DiagnosticsDatabase current in this.DiagnosticsDatabaseConfiguration.Databases)
				{
					string value = current.FilePath.Value;
					TreeNode treeNode = this.treeView.Nodes.Add(value, value, this._ImageKeyDb, this._ImageKeyDb);
					foreach (DiagnosticsECU current2 in current.ECUs)
					{
						treeNode.Nodes.Add(current2.Qualifier.Value, current2.Qualifier.Value, this._ImageKeyEcu, this._ImageKeyEcu);
					}
				}
				this.RenderDatabasePathsForCurrentWidth();
				this.treeView.ExpandAll();
			}
		}

		public void SelectEcu(string ecuQualifier)
		{
			foreach (TreeNode treeNode in this.treeView.Nodes)
			{
				foreach (TreeNode treeNode2 in treeNode.Nodes)
				{
					if (treeNode2.Name == ecuQualifier)
					{
						this.treeView.SelectedNode = treeNode2;
						return;
					}
				}
			}
			this.treeView.SelectedNode = this.treeView.Nodes[0].Nodes[0];
		}

		public void SetNodesErrorStateAndTooltip(PageValidator pageValidator)
		{
			this.toolTipErrorTexts.Clear();
			foreach (DiagnosticsECU current in this.DiagnosticsDatabaseConfiguration.ECUs)
			{
				TreeNode[] array = this.treeView.Nodes.Find(current.Qualifier.Value, true);
				if (array.Count<TreeNode>() > 0)
				{
					TreeNode treeNode = array[0];
					string errorText = pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.DiagnosticCommParamsECU.PhysRequestMsgId);
					string errorText2 = pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.DiagnosticCommParamsECU.ResponseMsgId);
					if (string.IsNullOrEmpty(errorText) && string.IsNullOrEmpty(errorText2))
					{
						treeNode.ImageKey = this._ImageKeyEcu;
						treeNode.SelectedImageKey = this._ImageKeyEcu;
					}
					else
					{
						treeNode.ImageKey = this._ImageKeyError;
						treeNode.SelectedImageKey = this._ImageKeyError;
						if (!string.IsNullOrEmpty(errorText) && string.IsNullOrEmpty(errorText2))
						{
							this.toolTipErrorTexts.Add(treeNode, errorText);
						}
						else if (string.IsNullOrEmpty(errorText) && !string.IsNullOrEmpty(errorText2))
						{
							this.toolTipErrorTexts.Add(treeNode, errorText2);
						}
						else if (errorText != errorText2)
						{
							this.toolTipErrorTexts.Add(treeNode, errorText + "\n" + errorText2);
						}
						else
						{
							this.toolTipErrorTexts.Add(treeNode, errorText);
						}
					}
					string errorText3 = pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.DiagnosticCommParamsECU.DefaultSessionId);
					string errorText4 = pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, current.DiagnosticCommParamsECU.ExtendedSessionId);
					if (!string.IsNullOrEmpty(errorText3) && !string.IsNullOrEmpty(errorText4) && errorText3 == errorText4)
					{
						if (this.toolTipErrorTexts.ContainsKey(array[0]))
						{
							string text = this.toolTipErrorTexts[array[0]];
							text = text + "\n" + Resources.ErrorSessionBytesMustDiffer;
							this.toolTipErrorTexts[treeNode] = text;
						}
						else
						{
							this.toolTipErrorTexts.Add(treeNode, Resources.ErrorSessionBytesMustDiffer);
						}
						treeNode.ImageKey = this._ImageKeyError;
						treeNode.SelectedImageKey = this._ImageKeyError;
					}
				}
			}
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.treeView.Nodes.Contains(e.Node))
			{
				e.Node.Expand();
				this.treeView.SelectedNode = e.Node.FirstNode;
				if (this.OnSelectEcu != null)
				{
					this.OnSelectEcu(this, EventArgs.Empty);
					return;
				}
			}
			else if (this.OnSelectEcu != null)
			{
				this.OnSelectEcu(this, EventArgs.Empty);
			}
		}

		private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (this.OnBeforeSelect != null)
			{
				this.OnBeforeSelect(this, e);
			}
		}

		private void EcuTreeControl_Resize(object sender, EventArgs e)
		{
			this.RenderDatabasePathsForCurrentWidth();
		}

		private void RenderDatabasePathsForCurrentWidth()
		{
			Size proposedSize = new Size(this.treeView.Width - 70, (int)Math.Ceiling((double)this.treeView.Font.GetHeight()));
			foreach (TreeNode treeNode in this.treeView.Nodes)
			{
				treeNode.Text = GUIUtil.FilePathToShortendDisplayPath(treeNode.Name, this.treeView.Font, proposedSize);
			}
		}

		private void treeView_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
		{
			if (e.Node.ImageKey == this._ImageKeyError && this.toolTipErrorTexts.ContainsKey(e.Node))
			{
				this.toolTip.SetToolTip(this.treeView, this.toolTipErrorTexts[e.Node]);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EcuTreeControl));
			this.treeView = new TreeView();
			this.imageList = new ImageList(this.components);
			this.toolTip = new ToolTip(this.components);
			base.SuspendLayout();
			this.treeView.Dock = DockStyle.Fill;
			this.treeView.HideSelection = false;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.Size = new Size(190, 245);
			this.treeView.TabIndex = 0;
			this.treeView.NodeMouseHover += new TreeNodeMouseHoverEventHandler(this.treeView_NodeMouseHover);
			this.treeView.BeforeSelect += new TreeViewCancelEventHandler(this.treeView_BeforeSelect);
			this.treeView.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
			this.imageList.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList.ImageStream");
			this.imageList.TransparentColor = Color.Transparent;
			this.imageList.Images.SetKeyName(0, "Database");
			this.imageList.Images.SetKeyName(1, "IconECU");
			this.imageList.Images.SetKeyName(2, "IconECUWarning");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.treeView);
			this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "EcuTreeControl";
			base.Size = new Size(190, 245);
			base.Resize += new EventHandler(this.EcuTreeControl_Resize);
			base.ResumeLayout(false);
		}
	}
}
