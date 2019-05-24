using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class DeviceListPopupMenuController
	{
		public delegate void DeviceSelectableOnlyForClassicViewDelegate(object sender, string path);

		private const int _MinPopupMenuWithInPixel = 600;

		private const uint _maxCustomFolderEntriesLimit = 10u;

		private const uint _maxPackedImagesEntriesLimit = 10u;

		private bool isUsedForNavigator;

		private IModelValidator modelValidator;

		private LoggerType loggerType;

		private Dictionary<string, BarCheckItem> onlineDeviceBarItems;

		private Dictionary<string, BarCheckItem> memoryCardBarItems;

		private Dictionary<string, BarCheckItem> customFolderBarItems;

		private Dictionary<string, BarCheckItem> packedImageBarItems;

		private Dictionary<string, BarCheckItem> additionalDriveBarItems;

		private BarStaticItem onlineDevicesHeaderItem;

		private BarStaticItem memoryCardsHeaderItem;

		private BarStaticItem customFoldersHeaderItem;

		private BarStaticItem packedImagesHeaderItem;

		private BarButtonItem selectCustomFolderItem;

		private BarButtonItem selectPackedImageItem;

		private BarCheckItem selectedItem;

		private DropDownButton dropdownButton;

		private BarManager barManager;

		private PopupMenu popupMenu;

		private uint maxCustomFolderEntries;

		private uint maxPackedImagesEntries;

		private string onlineDevicesHeaderCaptionNone;

		private string onlineDevicesHeaderCaption;

		private string memoryCardsHeaderCaptionNone;

		private string memoryCardsHeaderCaption;

		public event EventHandler SelectedDeviceChanged;

		public event DeviceListPopupMenuController.DeviceSelectableOnlyForClassicViewDelegate DeviceSelectableOnlyForClassicView;

		public string SelectedDevicePath
		{
			get
			{
				if (this.selectedItem != null)
				{
					return this.selectedItem.Name;
				}
				return string.Empty;
			}
		}

		public IList<string> OnlineDevices
		{
			get
			{
				return new List<string>(this.onlineDeviceBarItems.Keys);
			}
		}

		public IList<string> MemoryCardDevices
		{
			get
			{
				return new List<string>(this.memoryCardBarItems.Keys);
			}
		}

		public IList<string> CustomFolderDevices
		{
			get
			{
				return new List<string>(this.customFolderBarItems.Keys);
			}
		}

		public IList<string> PackedImageDevices
		{
			get
			{
				return new List<string>(this.packedImageBarItems.Keys);
			}
		}

		public List<string> AdditionalDrives
		{
			get
			{
				return new List<string>(this.additionalDriveBarItems.Keys);
			}
		}

		public DeviceListPopupMenuController(DropDownButton dropdownButtonOnForm, BarManager barManagerOnForm, PopupMenu popupMenuOnForm, bool isUsedInNavigatorContext, uint maxNumCustomFolderEntries = 5u, uint maxNumPackedImageEntries = 5u)
		{
			this.dropdownButton = dropdownButtonOnForm;
			this.dropdownButton.Layout += new LayoutEventHandler(this.OnDropdownButtonLayout);
			this.barManager = barManagerOnForm;
			this.popupMenu = popupMenuOnForm;
			this.popupMenu.BeforePopup += new CancelEventHandler(this.OnBeforeMenuPopup);
			this.isUsedForNavigator = isUsedInNavigatorContext;
			this.onlineDeviceBarItems = new Dictionary<string, BarCheckItem>();
			this.memoryCardBarItems = new Dictionary<string, BarCheckItem>();
			this.customFolderBarItems = new Dictionary<string, BarCheckItem>();
			this.packedImageBarItems = new Dictionary<string, BarCheckItem>();
			this.additionalDriveBarItems = new Dictionary<string, BarCheckItem>();
			this.maxCustomFolderEntries = 10u;
			if (maxNumCustomFolderEntries <= 10u)
			{
				this.maxCustomFolderEntries = maxNumCustomFolderEntries;
			}
			this.maxPackedImagesEntries = 10u;
			if (maxNumPackedImageEntries <= 10u)
			{
				this.maxPackedImagesEntries = maxNumPackedImageEntries;
			}
			this.InitPopupMenuSkeletonEntries();
			this.selectedItem = null;
			this.loggerType = LoggerType.Unknown;
			this.modelValidator = null;
		}

		public void Init(IModelValidator validator, bool isScanForAllLoggerTypesEnabled)
		{
			this.modelValidator = validator;
			if (isScanForAllLoggerTypesEnabled)
			{
				this.loggerType = LoggerType.Unknown;
			}
			else
			{
				this.loggerType = this.modelValidator.LoggerSpecifics.Type;
			}
			this.UpdateControls();
		}

		private void InitPopupMenuSkeletonEntries()
		{
			this.onlineDevicesHeaderCaption = "<b>" + Resources.CaptionHeaderOnlineDevices + "</b>";
			this.onlineDevicesHeaderCaptionNone = this.onlineDevicesHeaderCaption + " " + Resources.None;
			this.onlineDevicesHeaderItem = new BarStaticItem();
			this.onlineDevicesHeaderItem.Name = "OnlineDevices";
			this.onlineDevicesHeaderItem.AllowHtmlText = DefaultBoolean.True;
			this.onlineDevicesHeaderItem.Caption = this.onlineDevicesHeaderCaptionNone;
			this.barManager.Items.Add(this.onlineDevicesHeaderItem);
			this.popupMenu.ItemLinks.Add(this.onlineDevicesHeaderItem, true).BeginGroup = true;
			this.memoryCardsHeaderCaption = "<b>" + Resources.CaptionHeaderMemoryCards + "</b>";
			this.memoryCardsHeaderCaptionNone = this.memoryCardsHeaderCaption + " " + Resources.None;
			this.memoryCardsHeaderItem = new BarStaticItem();
			this.memoryCardsHeaderItem.Name = "MemoryCards";
			this.memoryCardsHeaderItem.AllowHtmlText = DefaultBoolean.True;
			this.memoryCardsHeaderItem.Caption = this.memoryCardsHeaderCaptionNone;
			this.barManager.Items.Add(this.memoryCardsHeaderItem);
			this.popupMenu.ItemLinks.Add(this.memoryCardsHeaderItem, true).BeginGroup = true;
			this.customFoldersHeaderItem = new BarStaticItem();
			this.customFoldersHeaderItem.Name = "CustomFolders";
			this.customFoldersHeaderItem.AllowHtmlText = DefaultBoolean.True;
			this.customFoldersHeaderItem.Caption = "<b>" + Resources.CaptionHeaderCustomFolders + "</b>";
			this.barManager.Items.Add(this.customFoldersHeaderItem);
			this.popupMenu.ItemLinks.Add(this.customFoldersHeaderItem, true).BeginGroup = true;
			this.selectCustomFolderItem = new BarButtonItem(this.barManager, "<i>" + Resources.CaptionSelectCustomFolder + "</i>");
			this.selectCustomFolderItem.Name = "SelectCustomFolder";
			this.selectCustomFolderItem.AllowHtmlText = DefaultBoolean.True;
			this.selectCustomFolderItem.ItemClick += new ItemClickEventHandler(this.OnSelectCustomFolderEventHandler);
			this.popupMenu.ItemLinks.Add(this.selectCustomFolderItem, false);
			this.packedImagesHeaderItem = new BarStaticItem();
			this.packedImagesHeaderItem.Name = "PackedImages";
			this.packedImagesHeaderItem.AllowHtmlText = DefaultBoolean.True;
			this.packedImagesHeaderItem.Caption = "<b>" + Resources.CaptionHeaderPackedImages + "</b>";
			this.barManager.Items.Add(this.packedImagesHeaderItem);
			this.popupMenu.ItemLinks.Add(this.packedImagesHeaderItem, true).BeginGroup = true;
			this.selectPackedImageItem = new BarButtonItem(this.barManager, "<i>" + Resources.CaptionSelectPackedImage + "</i>");
			this.selectPackedImageItem.Name = "SelectPackedImage";
			this.selectPackedImageItem.AllowHtmlText = DefaultBoolean.True;
			this.selectPackedImageItem.ItemClick += new ItemClickEventHandler(this.OnSelectPackedImageEventHandler);
			this.popupMenu.ItemLinks.Add(this.selectPackedImageItem);
		}

		public void AddOnlineDevice(string onlineDevicePath, LoggerType type)
		{
			BarCheckItem barCheckItem = new BarCheckItem(this.barManager, false);
			if (this.loggerType != LoggerType.Unknown)
			{
				barCheckItem.Caption = FileSystemServices.GetCardReaderDisplayName(onlineDevicePath);
			}
			else
			{
				barCheckItem.Caption = FileSystemServices.GetCardReaderDisplayName(onlineDevicePath) + string.Format(" [{0}]", type.ToString());
			}
			barCheckItem.Name = onlineDevicePath;
			barCheckItem.CheckedChanged += new ItemClickEventHandler(this.OnDeviceItemCheckChangedEventHandler);
			int num = this.popupMenu.ItemLinks.IndexOf(this.onlineDevicesHeaderItem.Links[0]);
			int position = num + this.onlineDeviceBarItems.Count + 1;
			this.popupMenu.ItemLinks.Insert(position, barCheckItem);
			this.barManager.Items.Add(barCheckItem);
			this.onlineDeviceBarItems.Add(onlineDevicePath, barCheckItem);
			this.UpdateControls();
		}

		public void AddMemoryCardDevice(string memoryCardPath, LoggerType type)
		{
			BarCheckItem barCheckItem = new BarCheckItem(this.barManager, false);
			if (this.loggerType != LoggerType.Unknown)
			{
				barCheckItem.Caption = FileSystemServices.GetCardReaderDisplayName(memoryCardPath);
			}
			else
			{
				barCheckItem.Caption = FileSystemServices.GetCardReaderDisplayName(memoryCardPath) + string.Format(" [{0}]", type.ToString());
			}
			barCheckItem.Name = memoryCardPath;
			barCheckItem.CheckedChanged += new ItemClickEventHandler(this.OnDeviceItemCheckChangedEventHandler);
			int num = this.popupMenu.ItemLinks.IndexOf(this.memoryCardsHeaderItem.Links[0]);
			int position = num + this.memoryCardBarItems.Count + 1;
			this.popupMenu.ItemLinks.Insert(position, barCheckItem);
			this.barManager.Items.Add(barCheckItem);
			this.memoryCardBarItems.Add(memoryCardPath, barCheckItem);
			this.UpdateControls();
		}

		public bool AddCustomFolderOrPackedImageDevice(string path)
		{
			if (!Directory.Exists(path))
			{
				if (File.Exists(path))
				{
					BarCheckItem barCheckItem = this.GetExistingBarItemForPackedImagePath(path);
					if (barCheckItem == null)
					{
						barCheckItem = this.CreateAndInsertPackedImageItem(path);
					}
					GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceZips, barCheckItem.Name);
					if (!this.IsFolderContainingCompatibleLogData(path, true))
					{
						return false;
					}
					this.SelectDeviceItem(barCheckItem);
				}
				return false;
			}
			BarCheckItem barCheckItem2 = this.GetExistingBarItemForPath(path);
			if (barCheckItem2 == null)
			{
				barCheckItem2 = this.CreateAndInsertCustomFolderItem(path);
			}
			GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceFolders, barCheckItem2.Name);
			if (!this.IsFolderContainingCompatibleLogData(path, true))
			{
				return false;
			}
			this.SelectDeviceItem(barCheckItem2);
			return true;
		}

		public void RemoveDevice(string path)
		{
			if (this.onlineDeviceBarItems.ContainsKey(path))
			{
				this.RemoveDeviceItem(this.onlineDeviceBarItems[path]);
			}
			else if (this.memoryCardBarItems.ContainsKey(path))
			{
				this.RemoveDeviceItem(this.memoryCardBarItems[path]);
			}
			this.UpdateControls();
		}

		public bool IsPathAvailableAndSelectable(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (Directory.Exists(path))
			{
				BarCheckItem existingBarItemForPath = this.GetExistingBarItemForPath(path);
				return existingBarItemForPath != null && this.IsFolderContainingCompatibleLogData(path, false);
			}
			if (File.Exists(path))
			{
				BarCheckItem existingBarItemForPackedImagePath = this.GetExistingBarItemForPackedImagePath(path);
				return existingBarItemForPackedImagePath != null && this.IsFolderContainingCompatibleLogData(path, false);
			}
			return false;
		}

		public bool SelectDevice(string path)
		{
			BarCheckItem barCheckItem = this.GetExistingBarItemForPath(path);
			if (barCheckItem == null)
			{
				barCheckItem = this.GetExistingBarItemForPackedImagePath(path);
			}
			if (barCheckItem != null)
			{
				if (this.customFolderBarItems.ContainsKey(barCheckItem.Name))
				{
					GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceFolders, barCheckItem.Name);
				}
				else if (this.packedImageBarItems.ContainsKey(barCheckItem.Name))
				{
					GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceZips, barCheckItem.Name);
				}
				if (this.IsFolderContainingCompatibleLogData(path, true))
				{
					this.SelectDeviceItem(barCheckItem);
				}
				return true;
			}
			return false;
		}

		public void AddAdditionalDrives(List<string> drivePathList)
		{
			foreach (string current in drivePathList)
			{
				if (this.additionalDriveBarItems.ContainsKey(current))
				{
					return;
				}
				BarCheckItem barCheckItem = new BarCheckItem(this.barManager, false);
				barCheckItem.Caption = current;
				barCheckItem.Name = current;
				barCheckItem.CheckedChanged += new ItemClickEventHandler(this.OnDeviceItemCheckChangedEventHandler);
				int position = this.popupMenu.ItemLinks.IndexOf(this.customFoldersHeaderItem.Links[0]);
				this.popupMenu.ItemLinks.Insert(position, barCheckItem);
				barCheckItem.Links[0].Visible = false;
				this.barManager.Items.Add(barCheckItem);
				this.additionalDriveBarItems.Add(current, barCheckItem);
			}
			this.UpdateControls();
		}

		public void RemoveAllAddtionalDrives()
		{
			foreach (string current in this.additionalDriveBarItems.Keys)
			{
				this.popupMenu.ItemLinks.Remove(this.additionalDriveBarItems[current].Links[0]);
				this.barManager.Items.Remove(this.additionalDriveBarItems[current]);
			}
			this.additionalDriveBarItems.Clear();
			this.UpdateControls();
		}

		public void OnDeviceItemCheckChangedEventHandler(object sender, ItemClickEventArgs e)
		{
			BarCheckItem barCheckItem = sender as BarCheckItem;
			if (barCheckItem == null)
			{
				return;
			}
			if (this.customFolderBarItems.ContainsKey(barCheckItem.Name))
			{
				if (!GlobalOptionsManager.FolderPathExists(GlobalOptionsManager.ListSelector.SourceFolders, barCheckItem.Name))
				{
					return;
				}
				GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceFolders, barCheckItem.Name);
			}
			else if (this.packedImageBarItems.ContainsKey(barCheckItem.Name))
			{
				if (!GlobalOptionsManager.FolderPathExists(GlobalOptionsManager.ListSelector.SourceZips, barCheckItem.Name))
				{
					return;
				}
				GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceZips, barCheckItem.Name);
			}
			this.SelectDeviceItem(barCheckItem);
		}

		public void OnSelectCustomFolderEventHandler(object sender, ItemClickEventArgs e)
		{
			using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
			{
				browseFolderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				if (GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.SourceFolders).Count > 0)
				{
					string text = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.SourceFolders)[0];
					if (!string.IsNullOrEmpty(text))
					{
						browseFolderDialog.SelectedPath = Path.GetDirectoryName(text);
					}
				}
				browseFolderDialog.Title = Resources.SelectCustomSourceFolder;
				if (DialogResult.OK == browseFolderDialog.ShowDialog())
				{
					if (!string.IsNullOrEmpty(browseFolderDialog.SelectedPath) && DirectoryProxy.Exists(browseFolderDialog.SelectedPath))
					{
						if (!GUIUtil.FolderAccessible(browseFolderDialog.SelectedPath))
						{
							InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
						}
						else
						{
							BarCheckItem barCheckItem = this.GetExistingBarItemForPath(browseFolderDialog.SelectedPath);
							if (barCheckItem == null)
							{
								barCheckItem = this.CreateAndInsertCustomFolderItem(browseFolderDialog.SelectedPath);
							}
							GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceFolders, barCheckItem.Name);
							this.SelectDeviceItem(barCheckItem);
						}
					}
				}
			}
		}

		public void OnSelectPackedImageEventHandler(object sender, ItemClickEventArgs e)
		{
			GenericOpenFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			if (GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.SourceZips).Count > 0)
			{
				string text = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.SourceZips)[0];
				if (!string.IsNullOrEmpty(text))
				{
					GenericOpenFileDialog.InitialDirectory = Path.GetDirectoryName(text);
				}
			}
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(Resources.SelectCustomSourceFolder, FileType.ZIPFile))
			{
				if (string.IsNullOrEmpty(GenericOpenFileDialog.FileName) || !DirectoryProxy.Exists(GenericOpenFileDialog.FileName))
				{
					return;
				}
				if (!GUIUtil.FolderAccessible(GenericOpenFileDialog.FileName))
				{
					InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
					return;
				}
				BarCheckItem barCheckItem = this.GetExistingBarItemForPackedImagePath(GenericOpenFileDialog.FileName);
				if (barCheckItem == null)
				{
					barCheckItem = this.CreateAndInsertPackedImageItem(GenericOpenFileDialog.FileName);
				}
				GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.SourceZips, barCheckItem.Name);
				this.SelectDeviceItem(barCheckItem);
			}
		}

		public void OnBeforeMenuPopup(object sender, EventArgs args)
		{
			this.popupMenu.MinWidth = Math.Max(this.dropdownButton.Width, 600);
			this.UpdateControls();
		}

		public void OnDropdownButtonResize(object sender, EventArgs args)
		{
			if (this.selectedItem == null)
			{
				return;
			}
			if (this.customFolderBarItems.ContainsKey(this.selectedItem.Name) || this.packedImageBarItems.ContainsKey(this.selectedItem.Name))
			{
				Control control = sender as Control;
				this.selectedItem.Caption = PathUtil.GetShortenedPath(this.selectedItem.Name, this.dropdownButton.Font, control.Width, false);
			}
		}

		public void OnDropdownButtonLayout(object sender, EventArgs e)
		{
			this.ShortenDropdownButtonTextToWidth();
		}

		private void UncheckAllDeviceItemsExcept(BarCheckItem itemToCheck)
		{
			foreach (BarItem barItem in this.barManager.Items)
			{
				if (barItem is BarCheckItem)
				{
					BarCheckItem barCheckItem = barItem as BarCheckItem;
					barCheckItem.CheckedChanged -= new ItemClickEventHandler(this.OnDeviceItemCheckChangedEventHandler);
					barCheckItem.Checked = (barCheckItem == itemToCheck);
					barCheckItem.CheckedChanged += new ItemClickEventHandler(this.OnDeviceItemCheckChangedEventHandler);
				}
			}
		}

		private void SelectDeviceItem(BarCheckItem item)
		{
			if (item != null)
			{
				if ((this.customFolderBarItems.ContainsKey(item.Name) || this.packedImageBarItems.ContainsKey(item.Name) || this.additionalDriveBarItems.ContainsKey(item.Name)) && !this.IsFolderContainingCompatibleLogData(item.Name, true))
				{
					return;
				}
				this.dropdownButton.Text = item.Caption;
				this.dropdownButton.ToolTip = item.Name;
			}
			else
			{
				this.dropdownButton.Text = string.Empty;
				this.dropdownButton.ToolTip = string.Empty;
				if (this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
				{
					this.dropdownButton.Text = this.selectCustomFolderItem.Caption;
				}
			}
			this.UncheckAllDeviceItemsExcept(item);
			BarCheckItem barCheckItem = this.selectedItem;
			this.selectedItem = item;
			this.ShortenDropdownButtonTextToWidth();
			if (((barCheckItem == null && this.selectedItem != null) || (barCheckItem != null && this.selectedItem == null) || (barCheckItem != null && barCheckItem.Name != item.Name)) && this.SelectedDeviceChanged != null)
			{
				this.SelectedDeviceChanged(this, EventArgs.Empty);
			}
		}

		private string GetMostRecentCustomFolderPath()
		{
			int num = this.popupMenu.ItemLinks.IndexOf(this.customFoldersHeaderItem.Links[0]);
			if (num < 0)
			{
				return string.Empty;
			}
			if (num + 1 < this.popupMenu.ItemLinks.Count)
			{
				BarCheckItem barCheckItem = this.popupMenu.ItemLinks[num + 1].Item as BarCheckItem;
				if (barCheckItem != null)
				{
					return barCheckItem.Name;
				}
			}
			return string.Empty;
		}

		private string GetMostRecentPackedImagePath()
		{
			int num = this.popupMenu.ItemLinks.IndexOf(this.packedImagesHeaderItem.Links[0]);
			if (num < 0)
			{
				return string.Empty;
			}
			if (num + 1 < this.popupMenu.ItemLinks.Count)
			{
				BarCheckItem barCheckItem = this.popupMenu.ItemLinks[num + 1].Item as BarCheckItem;
				if (barCheckItem != null)
				{
					return barCheckItem.Name;
				}
			}
			return string.Empty;
		}

		private BarCheckItem CreateAndInsertCustomFolderItem(string folderPath)
		{
			BarCheckItem barCheckItem = new BarCheckItem(this.barManager, false);
			barCheckItem.Caption = this.GetShortenedPath(folderPath);
			barCheckItem.Name = folderPath;
			barCheckItem.CheckedChanged += new ItemClickEventHandler(this.OnDeviceItemCheckChangedEventHandler);
			int num = this.popupMenu.ItemLinks.IndexOf(this.customFoldersHeaderItem.Links[0]);
			int position = num + 1;
			this.popupMenu.ItemLinks.Insert(position, barCheckItem);
			this.barManager.Items.Add(barCheckItem);
			this.customFolderBarItems.Add(folderPath, barCheckItem);
			int num2 = this.popupMenu.ItemLinks.IndexOf(this.selectCustomFolderItem.Links[0]);
			int num3 = num2 - num - 1;
			if ((long)num3 > (long)((ulong)this.maxCustomFolderEntries))
			{
				BarCheckItem barCheckItem2 = this.popupMenu.ItemLinks[num2 - 1].Item as BarCheckItem;
				if (barCheckItem2 != null)
				{
					this.RemoveDeviceItem(barCheckItem2);
				}
			}
			return barCheckItem;
		}

		private BarCheckItem CreateAndInsertPackedImageItem(string zipArchivePath)
		{
			BarCheckItem barCheckItem = new BarCheckItem(this.barManager, false);
			barCheckItem.Caption = this.GetShortenedPath(zipArchivePath);
			barCheckItem.Name = zipArchivePath;
			barCheckItem.CheckedChanged += new ItemClickEventHandler(this.OnDeviceItemCheckChangedEventHandler);
			int num = this.popupMenu.ItemLinks.IndexOf(this.packedImagesHeaderItem.Links[0]);
			int position = num + 1;
			this.popupMenu.ItemLinks.Insert(position, barCheckItem);
			this.barManager.Items.Add(barCheckItem);
			this.packedImageBarItems.Add(zipArchivePath, barCheckItem);
			int num2 = this.popupMenu.ItemLinks.IndexOf(this.selectPackedImageItem.Links[0]);
			int num3 = num2 - num - 1;
			if ((long)num3 > (long)((ulong)this.maxPackedImagesEntries))
			{
				BarCheckItem barCheckItem2 = this.popupMenu.ItemLinks[num2 - 1].Item as BarCheckItem;
				if (barCheckItem2 != null)
				{
					this.RemoveDeviceItem(barCheckItem2);
				}
			}
			return barCheckItem;
		}

		private void RemoveDeviceItem(BarCheckItem itemToRemove)
		{
			if (itemToRemove.Links.Count > 0)
			{
				this.popupMenu.ItemLinks.Remove(itemToRemove.Links[0]);
			}
			this.barManager.Items.Remove(itemToRemove);
			if (this.onlineDeviceBarItems.ContainsKey(itemToRemove.Name))
			{
				this.onlineDeviceBarItems.Remove(itemToRemove.Name);
				return;
			}
			if (this.memoryCardBarItems.ContainsKey(itemToRemove.Name))
			{
				this.memoryCardBarItems.Remove(itemToRemove.Name);
				return;
			}
			if (this.customFolderBarItems.ContainsKey(itemToRemove.Name))
			{
				this.customFolderBarItems.Remove(itemToRemove.Name);
				return;
			}
			if (this.packedImageBarItems.ContainsKey(itemToRemove.Name))
			{
				this.packedImageBarItems.Remove(itemToRemove.Name);
			}
		}

		private void UpdateControls()
		{
			if (this.onlineDeviceBarItems.Count > 0)
			{
				this.onlineDevicesHeaderItem.Caption = this.onlineDevicesHeaderCaption;
			}
			else
			{
				this.onlineDevicesHeaderItem.Caption = this.onlineDevicesHeaderCaptionNone;
			}
			this.onlineDevicesHeaderItem.Links[0].BeginGroup = true;
			if (this.memoryCardBarItems.Count > 0)
			{
				this.memoryCardsHeaderItem.Caption = this.memoryCardsHeaderCaption;
			}
			else
			{
				this.memoryCardsHeaderItem.Caption = this.memoryCardsHeaderCaptionNone;
			}
			this.memoryCardsHeaderItem.Links[0].BeginGroup = true;
			this.customFoldersHeaderItem.Links[0].BeginGroup = true;
			this.packedImagesHeaderItem.Links[0].BeginGroup = true;
			this.barManager.ForceInitialize();
			string text = string.Empty;
			if (this.selectedItem != null)
			{
				text = this.selectedItem.Name;
			}
			this.UpdateAdditionalDrivesDeviceList();
			this.UpdateCustomFolderDeviceList();
			this.UpdatePackedImageDeviceList();
			if (this.loggerType != LoggerType.Unknown && !this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && this.onlineDeviceBarItems.Count == 0 && this.memoryCardBarItems.Count == 0)
			{
				this.SelectDeviceItem(null);
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				this.AutoSelectDefaultEntry();
				return;
			}
			if (this.customFolderBarItems.ContainsKey(text))
			{
				if (this.IsFolderContainingCompatibleLogData(text, false))
				{
					this.SelectDeviceItem(this.customFolderBarItems[text]);
					return;
				}
				this.AutoSelectDefaultEntry();
				return;
			}
			else if (this.packedImageBarItems.ContainsKey(text))
			{
				if (this.IsFolderContainingCompatibleLogData(text, false))
				{
					this.SelectDeviceItem(this.packedImageBarItems[text]);
					return;
				}
				this.AutoSelectDefaultEntry();
				return;
			}
			else
			{
				if (this.onlineDeviceBarItems.ContainsKey(text))
				{
					this.SelectDeviceItem(this.onlineDeviceBarItems[text]);
					return;
				}
				if (this.memoryCardBarItems.ContainsKey(text))
				{
					this.SelectDeviceItem(this.memoryCardBarItems[text]);
					return;
				}
				this.AutoSelectDefaultEntry();
				return;
			}
		}

		private void AutoSelectDefaultEntry()
		{
			int num = this.popupMenu.ItemLinks.IndexOf(this.onlineDevicesHeaderItem.Links[0]);
			int num2 = this.popupMenu.ItemLinks.IndexOf(this.memoryCardsHeaderItem.Links[0]);
			int num3 = this.popupMenu.ItemLinks.IndexOf(this.customFoldersHeaderItem.Links[0]);
			BarCheckItem item = null;
			if (num + 1 < num2)
			{
				item = (this.popupMenu.ItemLinks[num + 1].Item as BarCheckItem);
			}
			else if (num2 + 1 < num3)
			{
				item = (this.popupMenu.ItemLinks[num2 + 1].Item as BarCheckItem);
			}
			this.SelectDeviceItem(item);
		}

		private void UpdateAdditionalDrivesDeviceList()
		{
			foreach (string current in this.additionalDriveBarItems.Keys)
			{
				this.additionalDriveBarItems[current].Links[0].Visible = Directory.Exists(current);
			}
		}

		private void UpdateCustomFolderDeviceList()
		{
			this.customFoldersHeaderItem.Links[0].Visible = true;
			this.selectCustomFolderItem.Links[0].Visible = true;
			if (this.loggerType != LoggerType.Unknown)
			{
				this.customFoldersHeaderItem.Links[0].Visible = this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem;
				this.selectCustomFolderItem.Links[0].Visible = this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem;
			}
			foreach (string current in this.customFolderBarItems.Keys)
			{
				BarCheckItem barCheckItem = this.customFolderBarItems[current];
				this.popupMenu.ItemLinks.Remove(barCheckItem.Links[0]);
				this.barManager.Items.Remove(barCheckItem);
			}
			this.customFolderBarItems.Clear();
			IList<string> recentFolders = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.SourceFolders);
			for (int i = Math.Min((int)this.maxCustomFolderEntries, recentFolders.Count); i > 0; i--)
			{
				BarCheckItem barCheckItem = this.CreateAndInsertCustomFolderItem(recentFolders[i - 1]);
				barCheckItem.Links[0].Visible = true;
				if (this.loggerType != LoggerType.Unknown)
				{
					barCheckItem.Links[0].Visible = this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem;
				}
			}
		}

		private void UpdatePackedImageDeviceList()
		{
			this.packedImagesHeaderItem.Links[0].Visible = true;
			this.selectPackedImageItem.Links[0].Visible = true;
			if (this.loggerType != LoggerType.Unknown)
			{
				this.packedImagesHeaderItem.Links[0].Visible = this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem;
				this.selectPackedImageItem.Links[0].Visible = this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem;
			}
			if (this.loggerType != LoggerType.Unknown)
			{
				this.packedImagesHeaderItem.Links[0].Visible = this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem;
			}
			foreach (string current in this.packedImageBarItems.Keys)
			{
				BarCheckItem barCheckItem = this.packedImageBarItems[current];
				this.popupMenu.ItemLinks.Remove(barCheckItem.Links[0]);
				this.barManager.Items.Remove(barCheckItem);
			}
			this.packedImageBarItems.Clear();
			IList<string> recentFolders = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.SourceZips);
			for (int i = Math.Min((int)this.maxPackedImagesEntries, recentFolders.Count); i > 0; i--)
			{
				BarCheckItem barCheckItem = this.CreateAndInsertPackedImageItem(recentFolders[i - 1]);
				barCheckItem.Links[0].Visible = true;
				if (this.loggerType != LoggerType.Unknown)
				{
					barCheckItem.Links[0].Visible = this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem;
				}
			}
		}

		private string GetShortenedPath(string path)
		{
			return PathUtil.GetShortenedPath(path, this.popupMenu.MenuAppearance.MenuBar.Font, this.popupMenu.MinWidth - 100, false);
		}

		private void ShortenDropdownButtonTextToWidth()
		{
			if (this.selectedItem == null)
			{
				return;
			}
			if (this.customFolderBarItems.ContainsKey(this.selectedItem.Name) || this.packedImageBarItems.ContainsKey(this.selectedItem.Name))
			{
				this.dropdownButton.Text = PathUtil.GetShortenedPath(this.selectedItem.Name, this.dropdownButton.Font, this.dropdownButton.Width - 100, false);
			}
		}

		private BarCheckItem GetExistingBarItemForPath(string pathToFind)
		{
			foreach (string current in this.onlineDeviceBarItems.Keys)
			{
				if (string.Compare(pathToFind, current, true) == 0)
				{
					BarCheckItem result = this.onlineDeviceBarItems[current];
					return result;
				}
				int length = Math.Min(current.Length, pathToFind.Length);
				if (string.Compare(pathToFind.Substring(0, length), current.Substring(0, length), true) == 0)
				{
					BarCheckItem result = this.onlineDeviceBarItems[current];
					return result;
				}
			}
			foreach (string current2 in this.memoryCardBarItems.Keys)
			{
				if (string.Compare(pathToFind, current2, true) == 0)
				{
					BarCheckItem result = this.memoryCardBarItems[current2];
					return result;
				}
				int length2 = Math.Min(current2.Length, pathToFind.Length);
				if (string.Compare(pathToFind.Substring(0, length2), current2.Substring(0, length2), true) == 0)
				{
					BarCheckItem result = this.memoryCardBarItems[current2];
					return result;
				}
			}
			foreach (string current3 in this.customFolderBarItems.Keys)
			{
				if (string.Compare(pathToFind, current3, true) == 0)
				{
					BarCheckItem result = this.customFolderBarItems[current3];
					return result;
				}
			}
			return null;
		}

		private BarCheckItem GetExistingBarItemForPackedImagePath(string pathToFind)
		{
			foreach (string current in this.packedImageBarItems.Keys)
			{
				if (string.Compare(pathToFind, current, true) == 0)
				{
					return this.packedImageBarItems[current];
				}
			}
			return null;
		}

		private bool IsFolderContainingCompatibleLogData(string selectedPath, bool showMessageBox = true)
		{
			if (this.loggerType != LoggerType.Unknown)
			{
				if (!this.IsMemoryCardContentCompatibleToLogger(selectedPath, this.isUsedForNavigator))
				{
					if (showMessageBox)
					{
						InformMessageBox.Error(string.Format(Resources.ErrorDirDoesNotContainDataForLogger, selectedPath));
					}
					return false;
				}
				if (this.isUsedForNavigator && !this.modelValidator.LoggerSpecifics.FileConversion.IsNavigatorViewSupported)
				{
					this.FireDeviceSelectableOnlyForClassicView(selectedPath);
					return false;
				}
			}
			else
			{
				ILoggerSpecifics loggerSpecifics = null;
				if (!this.DoesMemoryCardContainCompatibleLoggingData(selectedPath, out loggerSpecifics))
				{
					if (showMessageBox)
					{
						InformMessageBox.Error(string.Format(Resources.VLExportErrorDirDoesNotContainData, selectedPath));
					}
					return false;
				}
				if (this.isUsedForNavigator && !loggerSpecifics.FileConversion.IsNavigatorViewSupported)
				{
					this.FireDeviceSelectableOnlyForClassicView(selectedPath);
					return false;
				}
			}
			return true;
		}

		private bool IsMemoryCardContentCompatibleToLogger(string memCardPath, bool isExistenceOfAllIniFilesRequired = false)
		{
			if (this.modelValidator.LoggerSpecifics.Type == LoggerType.VN1630log)
			{
				return VN16XXlogScanner.IsMemoryCardContentCompatibleToLogger(memCardPath);
			}
			bool flag = MlRtIniFile.IsMemoryCardContentCompatibleToLogger(memCardPath, this.modelValidator.LoggerSpecifics);
			if (!isExistenceOfAllIniFilesRequired)
			{
				return flag;
			}
			return flag && FileProxy.Exists(Path.Combine(memCardPath, Constants.LogDataIniFileName)) && FileProxy.Exists(Path.Combine(memCardPath, Constants.LogDataIniFile2Name));
		}

		private bool DoesMemoryCardContainCompatibleLoggingData(string memCardPath, out ILoggerSpecifics detectedLoggerSpecifics)
		{
			detectedLoggerSpecifics = MlRtIniFile.LoadLoggerTypeFromMemoryCardContent(memCardPath);
			if (detectedLoggerSpecifics.Type == LoggerType.VN1630log)
			{
				return VN16XXlogScanner.IsMemoryCardContentCompatibleToLogger(memCardPath);
			}
			return detectedLoggerSpecifics.Type == LoggerType.GL1000 || (FileProxy.Exists(Path.Combine(memCardPath, Constants.LogDataIniFileName)) && FileProxy.Exists(Path.Combine(memCardPath, Constants.LogDataIniFile2Name)));
		}

		private void FireDeviceSelectableOnlyForClassicView(string path)
		{
			if (this.DeviceSelectableOnlyForClassicView != null)
			{
				this.DeviceSelectableOnlyForClassicView(this, path);
			}
		}
	}
}
