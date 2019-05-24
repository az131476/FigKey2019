using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI
{
	internal class TreeControl : UserControl, IUpdateObserver<LoggerType>, IUpdateObserver
	{
		public delegate void SelectTreeNode(object sender, IPropertyWindow pw);

		public delegate void BeforeChangeSelect(TreeViewCancelEventArgs e);

		private class ErrorState
		{
			public bool State;

			public ErrorState(bool state)
			{
				this.State = state;
			}
		}

		private const string overlayPrefix = "_Overlay_";

		public TreeControl.SelectTreeNode OnSelectTreeNode;

		public TreeControl.BeforeChangeSelect OnBeforeChangeSelect;

		public EventHandler OnTreeViewAndSelectedNodeChange;

		public bool isFireBeforeAfterSelectEnabled;

		private Dictionary<IPropertyWindow, TreeControl.ErrorState> lastErrorState = new Dictionary<IPropertyWindow, TreeControl.ErrorState>();

		private IContainer components;

		private TreeView treeViewGL1000;

		private ImageList imageList;

		private TreeView treeViewGL3Plus;

		private TreeView treeViewGL2000;

		private TreeView treeViewGL1020FTE;

		private TreeView treeViewVN16XXlog;

		public IUpdateService UpdateService
		{
			get;
			set;
		}

		public TreeControl()
		{
			this.InitializeComponent();
			base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.treeViewGL1000.ExpandAll();
			this.treeViewGL1020FTE.ExpandAll();
			this.treeViewGL2000.ExpandAll();
			this.treeViewGL3Plus.ExpandAll();
			this.treeViewVN16XXlog.ExpandAll();
			this.treeViewGL1000.Visible = false;
			this.treeViewGL1020FTE.Visible = false;
			this.treeViewGL2000.Visible = false;
			this.treeViewGL3Plus.Visible = false;
			this.treeViewVN16XXlog.Visible = false;
			this.isFireBeforeAfterSelectEnabled = true;
			ImageList imageList = new ImageList();
			for (int i = 0; i < this.imageList.Images.Count; i++)
			{
				Image image = ImageUtils.MakeOverlay(this.imageList.Images[i], ImageUtils.ScaleImageForOverlay(MainImageList.Instance.GetImage(MainImageList.IconIndex.Warning), ImageUtils.OverlayPos.BottomRight, 3), ImageUtils.OverlayAligment.TopLeft);
				string key = "_Overlay_" + this.imageList.Images.Keys[i];
				imageList.Images.Add(key, image);
			}
			for (int j = 0; j < imageList.Images.Count; j++)
			{
				this.imageList.Images.Add(imageList.Images.Keys[j], imageList.Images[j]);
			}
		}

		public void Init()
		{
			if (this.UpdateService != null)
			{
				this.UpdateService.AddUpdateObserver(this, UpdateContext.TreeControl);
			}
		}

		public void BindPropertyWindow2Node(IPropertyWindow propertyWindow)
		{
			List<TreeNode> list = this.treeViewGL1000.Nodes.Find(propertyWindow.Type.ToString(), true).ToList<TreeNode>();
			list.AddRange(this.treeViewGL1020FTE.Nodes.Find(propertyWindow.Type.ToString(), true).ToList<TreeNode>());
			list.AddRange(this.treeViewGL2000.Nodes.Find(propertyWindow.Type.ToString(), true).ToList<TreeNode>());
			list.AddRange(this.treeViewGL3Plus.Nodes.Find(propertyWindow.Type.ToString(), true).ToList<TreeNode>());
			list.AddRange(this.treeViewVN16XXlog.Nodes.Find(propertyWindow.Type.ToString(), true).ToList<TreeNode>());
			if (propertyWindow.Type == PageType.CANgps)
			{
				list.AddRange(this.treeViewGL2000.Nodes.Find(PageType.GPS.ToString(), true).ToList<TreeNode>());
			}
			foreach (TreeNode current in list)
			{
				current.Tag = propertyWindow;
			}
			this.lastErrorState.Add(propertyWindow, new TreeControl.ErrorState(false));
		}

		private void treeViewGL1000_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.treeView_AfterSelect(this.treeViewGL1000, e);
		}

		private void treeViewGL1020FTE_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.treeView_AfterSelect(this.treeViewGL1020FTE, e);
		}

		private void treeViewGL2000_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.treeView_AfterSelect(this.treeViewGL2000, e);
		}

		private void treeViewGL3Plus_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.treeView_AfterSelect(this.treeViewGL3Plus, e);
		}

		private void treeViewVN16XXlog_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.treeView_AfterSelect(this.treeViewVN16XXlog, e);
		}

		private void treeView_AfterSelect(TreeView treeView, TreeViewEventArgs e)
		{
			if (!this.isFireBeforeAfterSelectEnabled)
			{
				return;
			}
			if (e.Node.Parent == null || e.Node.Tag == null)
			{
				e.Node.Expand();
				TreeNode firstNode = e.Node.FirstNode;
				while (firstNode.GetNodeCount(false) > 0)
				{
					firstNode.Expand();
					firstNode = firstNode.FirstNode;
				}
				treeView.SelectedNode = firstNode;
				this.FireAfterSelectEvent(e.Node.FirstNode);
				return;
			}
			this.FireAfterSelectEvent(e.Node);
		}

		private void treeViewGL1000_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (!this.isFireBeforeAfterSelectEnabled)
			{
				return;
			}
			if (this.OnBeforeChangeSelect != null)
			{
				this.OnBeforeChangeSelect(e);
			}
		}

		private void treeViewGL1020FTE_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (!this.isFireBeforeAfterSelectEnabled)
			{
				return;
			}
			if (this.OnBeforeChangeSelect != null)
			{
				this.OnBeforeChangeSelect(e);
			}
		}

		private void treeViewGL2000_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (!this.isFireBeforeAfterSelectEnabled)
			{
				return;
			}
			if (this.OnBeforeChangeSelect != null)
			{
				this.OnBeforeChangeSelect(e);
			}
		}

		private void treeViewGL3Plus_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (!this.isFireBeforeAfterSelectEnabled)
			{
				return;
			}
			if (this.OnBeforeChangeSelect != null)
			{
				this.OnBeforeChangeSelect(e);
			}
		}

		private void treeViewVN16XXlog_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (!this.isFireBeforeAfterSelectEnabled)
			{
				return;
			}
			if (this.OnBeforeChangeSelect != null)
			{
				this.OnBeforeChangeSelect(e);
			}
		}

		private void treeViewVN16XXlog_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			this.treeView_DrawNode(sender, e);
			e.DrawDefault = true;
		}

		private void treeViewGL1020FTE_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			this.treeView_DrawNode(sender, e);
			e.DrawDefault = true;
		}

		private void treeViewGL2000_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			this.treeView_DrawNode(sender, e);
			e.DrawDefault = true;
		}

		private void treeViewGL3Plus_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			this.treeView_DrawNode(sender, e);
			e.DrawDefault = true;
		}

		private void treeViewGL1000_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			this.treeView_DrawNode(sender, e);
			e.DrawDefault = true;
		}

		private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			if (e.Node != null && e.Node.Tag != null && e.Node.Tag is IPropertyWindow)
			{
				IPropertyWindow propertyWindow = e.Node.Tag as IPropertyWindow;
				if (propertyWindow.HasGlobalErrors())
				{
					if (!e.Node.ImageKey.StartsWith("_Overlay_"))
					{
						e.Node.ImageKey = "_Overlay_" + e.Node.ImageKey;
						e.Node.SelectedImageKey = e.Node.ImageKey;
						return;
					}
				}
				else if (e.Node.ImageKey.StartsWith("_Overlay_"))
				{
					e.Node.ImageKey = e.Node.ImageKey.Remove(0, "_Overlay_".Length);
					e.Node.SelectedImageKey = e.Node.ImageKey;
				}
			}
		}

		private void FireAfterSelectEvent(TreeNode treeNode)
		{
			if (treeNode == null || treeNode.Tag == null || !(treeNode.Tag is IPropertyWindow))
			{
				return;
			}
			if (this.OnSelectTreeNode != null)
			{
				this.OnSelectTreeNode(this, treeNode.Tag as IPropertyWindow);
			}
		}

		public void SelectDefaultPropertyPage()
		{
			TreeNode[] array = this.treeViewGL1000.Nodes.Find(PageType.DeviceInformation.ToString(), true);
			this.treeViewGL1000.SelectedNode = array[0];
			array = this.treeViewGL1020FTE.Nodes.Find(PageType.DeviceInformation.ToString(), true);
			this.treeViewGL1020FTE.SelectedNode = array[0];
			array = this.treeViewGL2000.Nodes.Find(PageType.DeviceInformation.ToString(), true);
			this.treeViewGL2000.SelectedNode = array[0];
			array = this.treeViewGL3Plus.Nodes.Find(PageType.DeviceInformation.ToString(), true);
			this.treeViewGL3Plus.SelectedNode = array[0];
			array = this.treeViewVN16XXlog.Nodes.Find(PageType.DeviceInformation.ToString(), true);
			this.treeViewVN16XXlog.SelectedNode = array[0];
		}

		public void SelectPropertyPage(PageType type)
		{
			if (this.treeViewGL1000.Visible)
			{
				TreeNode[] array = this.treeViewGL1000.Nodes.Find(type.ToString(), true);
				if (array != null && array.Length > 0)
				{
					this.treeViewGL1000.SelectedNode = array[0];
					return;
				}
			}
			else if (this.treeViewGL1020FTE.Visible)
			{
				TreeNode[] array2 = this.treeViewGL1020FTE.Nodes.Find(type.ToString(), true);
				if (array2 != null && array2.Length > 0)
				{
					this.treeViewGL1020FTE.SelectedNode = array2[0];
					return;
				}
			}
			else if (this.treeViewGL2000.Visible)
			{
				TreeNode[] array3 = this.treeViewGL2000.Nodes.Find(type.ToString(), true);
				if (array3 != null && array3.Length > 0)
				{
					this.treeViewGL2000.SelectedNode = array3[0];
					return;
				}
			}
			else if (this.treeViewGL3Plus.Visible)
			{
				TreeNode[] array4 = this.treeViewGL3Plus.Nodes.Find(type.ToString(), true);
				if (array4 != null && array4.Length > 0)
				{
					this.treeViewGL3Plus.SelectedNode = array4[0];
					return;
				}
			}
			else if (this.treeViewVN16XXlog.Visible)
			{
				TreeNode[] array5 = this.treeViewVN16XXlog.Nodes.Find(type.ToString(), true);
				if (array5 != null && array5.Length > 0)
				{
					this.treeViewVN16XXlog.SelectedNode = array5[0];
				}
			}
		}

		public string GetPageNameForPageType(PageType pageType)
		{
			if (this.treeViewGL1000.Visible)
			{
				TreeNode[] array = this.treeViewGL1000.Nodes.Find(pageType.ToString(), true);
				if (array.Count<TreeNode>() > 0)
				{
					return array[0].Text;
				}
			}
			else if (this.treeViewGL1020FTE.Visible)
			{
				TreeNode[] array2 = this.treeViewGL1020FTE.Nodes.Find(pageType.ToString(), true);
				if (array2.Count<TreeNode>() > 0)
				{
					return array2[0].Text;
				}
			}
			else if (this.treeViewGL2000.Visible)
			{
				TreeNode[] array3 = this.treeViewGL2000.Nodes.Find(pageType.ToString(), true);
				if (array3.Count<TreeNode>() > 0)
				{
					return array3[0].Text;
				}
			}
			else if (this.treeViewGL3Plus.Visible)
			{
				TreeNode[] array4 = this.treeViewGL3Plus.Nodes.Find(pageType.ToString(), true);
				if (array4.Count<TreeNode>() > 0)
				{
					return array4[0].Text;
				}
			}
			else if (this.treeViewVN16XXlog.Visible)
			{
				TreeNode[] array5 = this.treeViewVN16XXlog.Nodes.Find(pageType.ToString(), true);
				if (array5.Count<TreeNode>() > 0)
				{
					return array5[0].Text;
				}
			}
			return string.Empty;
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			TreeView treeView = null;
			if (this.treeViewGL1000.Visible)
			{
				treeView = this.treeViewGL1000;
			}
			else if (this.treeViewGL1020FTE.Visible)
			{
				treeView = this.treeViewGL1020FTE;
			}
			else if (this.treeViewGL2000.Visible)
			{
				treeView = this.treeViewGL2000;
			}
			else if (this.treeViewGL3Plus.Visible)
			{
				treeView = this.treeViewGL3Plus;
			}
			else if (this.treeViewVN16XXlog.Visible)
			{
				treeView = this.treeViewVN16XXlog;
			}
			switch (data)
			{
			case LoggerType.GL1000:
				if (this.treeViewGL1000.Visible)
				{
					return;
				}
				this.SelectCurrentOrDefaultPropertyPage(ref treeView, ref this.treeViewGL1000);
				return;
			case LoggerType.GL1020FTE:
				if (this.treeViewGL1020FTE.Visible)
				{
					return;
				}
				this.SelectCurrentOrDefaultPropertyPage(ref treeView, ref this.treeViewGL1020FTE);
				return;
			case LoggerType.GL2000:
				if (this.treeViewGL2000.Visible)
				{
					return;
				}
				this.SelectCurrentOrDefaultPropertyPage(ref treeView, ref this.treeViewGL2000);
				return;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				if (this.treeViewGL3Plus.Visible)
				{
					return;
				}
				this.SelectCurrentOrDefaultPropertyPage(ref treeView, ref this.treeViewGL3Plus);
				return;
			case LoggerType.VN1630log:
				if (this.treeViewVN16XXlog.Visible)
				{
					return;
				}
				this.SelectCurrentOrDefaultPropertyPage(ref treeView, ref this.treeViewVN16XXlog);
				return;
			default:
				return;
			}
		}

		private void SelectCurrentOrDefaultPropertyPage(ref TreeView oldActiveTreeView, ref TreeView newActiveTreeView)
		{
			string selectedNodeName = string.Empty;
			if (oldActiveTreeView != null && oldActiveTreeView.SelectedNode != null)
			{
				selectedNodeName = oldActiveTreeView.SelectedNode.Name;
			}
			bool flag = false;
			TreeNode treeNode = null;
			TreeNode treeNode2 = null;
			foreach (TreeNode currentNode in newActiveTreeView.Nodes)
			{
				flag = TreeControl.IsCurrentlySelectedNodeFound(selectedNodeName, currentNode, out treeNode2, ref treeNode);
				if (flag)
				{
					break;
				}
			}
			if (!flag && treeNode != null)
			{
				treeNode2 = treeNode;
				if (this.OnTreeViewAndSelectedNodeChange != null)
				{
					this.OnTreeViewAndSelectedNodeChange(this, EventArgs.Empty);
				}
			}
			this.isFireBeforeAfterSelectEnabled = false;
			newActiveTreeView.Visible = true;
			newActiveTreeView.SelectedNode = null;
			newActiveTreeView.SelectedNode = treeNode2;
			this.FireAfterSelectEvent(treeNode2);
			this.isFireBeforeAfterSelectEnabled = true;
			if (oldActiveTreeView != null)
			{
				oldActiveTreeView.SelectedNode = null;
				oldActiveTreeView.Visible = false;
			}
		}

		private static bool IsCurrentlySelectedNodeFound(string selectedNodeName, TreeNode currentNode, out TreeNode newSelectedTreeNode, ref TreeNode defaultTreeNode)
		{
			if (currentNode.Nodes.Count > 0)
			{
				IEnumerator enumerator = currentNode.Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						TreeNode currentNode2 = (TreeNode)enumerator.Current;
						if (TreeControl.IsCurrentlySelectedNodeFound(selectedNodeName, currentNode2, out newSelectedTreeNode, ref defaultTreeNode))
						{
							return true;
						}
					}
					goto IL_98;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			if (currentNode.Name == PageType.DeviceInformation.ToString())
			{
				defaultTreeNode = currentNode;
			}
			if (currentNode.Name == selectedNodeName)
			{
				newSelectedTreeNode = currentNode;
				return true;
			}
			IL_98:
			newSelectedTreeNode = null;
			return false;
		}

		public void RefreshView()
		{
			bool flag = false;
			foreach (IPropertyWindow current in this.lastErrorState.Keys)
			{
				if (current.HasGlobalErrors() != this.lastErrorState[current].State)
				{
					this.lastErrorState[current].State = current.HasGlobalErrors();
					flag = true;
				}
			}
			if (flag)
			{
				this.Refresh();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TreeControl));
			this.treeViewGL1000 = new TreeView();
			this.imageList = new ImageList(this.components);
			this.treeViewGL3Plus = new TreeView();
			this.treeViewGL2000 = new TreeView();
			this.treeViewGL1020FTE = new TreeView();
			this.treeViewVN16XXlog = new TreeView();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.treeViewGL1000, "treeViewGL1000");
			this.treeViewGL1000.DrawMode = TreeViewDrawMode.OwnerDrawAll;
			this.treeViewGL1000.HideSelection = false;
			this.treeViewGL1000.ImageList = this.imageList;
			this.treeViewGL1000.Name = "treeViewGL1000";
			this.treeViewGL1000.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("treeViewGL1000.Nodes"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1000.Nodes1"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1000.Nodes2"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1000.Nodes3"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1000.Nodes4"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1000.Nodes5"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1000.Nodes6")
			});
			this.treeViewGL1000.DrawNode += new DrawTreeNodeEventHandler(this.treeViewGL1000_DrawNode);
			this.treeViewGL1000.BeforeSelect += new TreeViewCancelEventHandler(this.treeViewGL1000_BeforeSelect);
			this.treeViewGL1000.AfterSelect += new TreeViewEventHandler(this.treeViewGL1000_AfterSelect);
			this.imageList.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList.ImageStream");
			this.imageList.TransparentColor = Color.Transparent;
			this.imageList.Images.SetKeyName(0, "IconVLConfigSmall.ico");
			this.imageList.Images.SetKeyName(1, "IconAvailableDevices.ico");
			this.imageList.Images.SetKeyName(2, "IconCANChannels.ico");
			this.imageList.Images.SetKeyName(3, "IconCCPXCP.ico");
			this.imageList.Images.SetKeyName(4, "IconCLFExport.ico");
			this.imageList.Images.SetKeyName(5, "IconDatabases.ico");
			this.imageList.Images.SetKeyName(6, "IconDeviceInformation.ico");
			this.imageList.Images.SetKeyName(7, "IconFileManager.ico");
			this.imageList.Images.SetKeyName(8, "IconFilters.ico");
			this.imageList.Images.SetKeyName(9, "IconGeneral.ico");
			this.imageList.Images.SetKeyName(10, "IconIncludeFiles.ico");
			this.imageList.Images.SetKeyName(11, "IconLEDs.ico");
			this.imageList.Images.SetKeyName(12, "IconLINChannels.ico");
			this.imageList.Images.SetKeyName(13, "IconLogConfiguration.ico");
			this.imageList.Images.SetKeyName(14, "IconLoggerDevice.ico");
			this.imageList.Images.SetKeyName(15, "IconMainConfiguration.ico");
			this.imageList.Images.SetKeyName(16, "IconTriggers.ico");
			this.imageList.Images.SetKeyName(17, "IconSpecialFeatures.ico");
			this.imageList.Images.SetKeyName(18, "IconCardReader.ico");
			this.imageList.Images.SetKeyName(19, "IconHardwareSettings.ico");
			this.imageList.Images.SetKeyName(20, "IconWLANSettings.ico");
			this.imageList.Images.SetKeyName(21, "IconFlexrayChannels.ico");
			this.imageList.Images.SetKeyName(22, "IconInterfaceMode.ico");
			this.imageList.Images.SetKeyName(23, "IconConfigDiag.ico");
			this.imageList.Images.SetKeyName(24, "IconDiagDescription.ico");
			this.imageList.Images.SetKeyName(25, "IconDiagAction.ico");
			this.imageList.Images.SetKeyName(26, "IconComment.ico");
			this.imageList.Images.SetKeyName(27, "IconAnalogInputs.ico");
			this.imageList.Images.SetKeyName(28, "IconDigitalInputs.ico");
			this.imageList.Images.SetKeyName(29, "IconMOSTChannels.ico");
			this.imageList.Images.SetKeyName(30, "IconDigitalOutputs.ico");
			this.imageList.Images.SetKeyName(31, "IconOutputConfiguration.ico");
			this.imageList.Images.SetKeyName(32, "IconGPS.ico");
			this.imageList.Images.SetKeyName(33, "IconSendMsg.ico");
			this.imageList.Images.SetKeyName(34, "IconCardReaderNavi.ico");
			this.imageList.Images.SetKeyName(35, "IconLoggerDeviceNavi.ico");
			this.imageList.Images.SetKeyName(36, "IconA2lDatabases.ico");
			this.imageList.Images.SetKeyName(37, "IconMultibusChannels.ico");
			componentResourceManager.ApplyResources(this.treeViewGL3Plus, "treeViewGL3Plus");
			this.treeViewGL3Plus.DrawMode = TreeViewDrawMode.OwnerDrawAll;
			this.treeViewGL3Plus.HideSelection = false;
			this.treeViewGL3Plus.ImageList = this.imageList;
			this.treeViewGL3Plus.Name = "treeViewGL3Plus";
			this.treeViewGL3Plus.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes1"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes2"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes3"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes4"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes5"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes6"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL3Plus.Nodes7")
			});
			this.treeViewGL3Plus.DrawNode += new DrawTreeNodeEventHandler(this.treeViewGL3Plus_DrawNode);
			this.treeViewGL3Plus.BeforeSelect += new TreeViewCancelEventHandler(this.treeViewGL3Plus_BeforeSelect);
			this.treeViewGL3Plus.AfterSelect += new TreeViewEventHandler(this.treeViewGL3Plus_AfterSelect);
			componentResourceManager.ApplyResources(this.treeViewGL2000, "treeViewGL2000");
			this.treeViewGL2000.DrawMode = TreeViewDrawMode.OwnerDrawAll;
			this.treeViewGL2000.HideSelection = false;
			this.treeViewGL2000.ImageList = this.imageList;
			this.treeViewGL2000.Name = "treeViewGL2000";
			this.treeViewGL2000.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("treeViewGL2000.Nodes"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL2000.Nodes1"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL2000.Nodes2"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL2000.Nodes3"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL2000.Nodes4"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL2000.Nodes5"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL2000.Nodes6")
			});
			this.treeViewGL2000.DrawNode += new DrawTreeNodeEventHandler(this.treeViewGL2000_DrawNode);
			this.treeViewGL2000.BeforeSelect += new TreeViewCancelEventHandler(this.treeViewGL2000_BeforeSelect);
			this.treeViewGL2000.AfterSelect += new TreeViewEventHandler(this.treeViewGL2000_AfterSelect);
			componentResourceManager.ApplyResources(this.treeViewGL1020FTE, "treeViewGL1020FTE");
			this.treeViewGL1020FTE.DrawMode = TreeViewDrawMode.OwnerDrawAll;
			this.treeViewGL1020FTE.HideSelection = false;
			this.treeViewGL1020FTE.ImageList = this.imageList;
			this.treeViewGL1020FTE.Name = "treeViewGL1020FTE";
			this.treeViewGL1020FTE.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("treeViewGL1020FTE.Nodes"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1020FTE.Nodes1"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1020FTE.Nodes2"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1020FTE.Nodes3"),
				(TreeNode)componentResourceManager.GetObject("treeViewGL1020FTE.Nodes4")
			});
			this.treeViewGL1020FTE.DrawNode += new DrawTreeNodeEventHandler(this.treeViewGL1020FTE_DrawNode);
			this.treeViewGL1020FTE.BeforeSelect += new TreeViewCancelEventHandler(this.treeViewGL1020FTE_BeforeSelect);
			this.treeViewGL1020FTE.AfterSelect += new TreeViewEventHandler(this.treeViewGL1020FTE_AfterSelect);
			componentResourceManager.ApplyResources(this.treeViewVN16XXlog, "treeViewVN16XXlog");
			this.treeViewVN16XXlog.DrawMode = TreeViewDrawMode.OwnerDrawAll;
			this.treeViewVN16XXlog.ImageList = this.imageList;
			this.treeViewVN16XXlog.Name = "treeViewVN16XXlog";
			this.treeViewVN16XXlog.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("treeViewVN16XXlog.Nodes"),
				(TreeNode)componentResourceManager.GetObject("treeViewVN16XXlog.Nodes1"),
				(TreeNode)componentResourceManager.GetObject("treeViewVN16XXlog.Nodes2"),
				(TreeNode)componentResourceManager.GetObject("treeViewVN16XXlog.Nodes3")
			});
			this.treeViewVN16XXlog.DrawNode += new DrawTreeNodeEventHandler(this.treeViewVN16XXlog_DrawNode);
			this.treeViewVN16XXlog.BeforeSelect += new TreeViewCancelEventHandler(this.treeViewVN16XXlog_BeforeSelect);
			this.treeViewVN16XXlog.AfterSelect += new TreeViewEventHandler(this.treeViewVN16XXlog_AfterSelect);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.treeViewVN16XXlog);
			base.Controls.Add(this.treeViewGL1020FTE);
			base.Controls.Add(this.treeViewGL2000);
			base.Controls.Add(this.treeViewGL3Plus);
			base.Controls.Add(this.treeViewGL1000);
			this.DoubleBuffered = true;
			base.Name = "TreeControl";
			base.ResumeLayout(false);
		}
	}
}
