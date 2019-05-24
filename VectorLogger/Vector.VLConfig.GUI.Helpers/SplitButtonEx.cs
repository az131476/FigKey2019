using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.UtilityFunctions;

namespace Vector.VLConfig.GUI.Helpers
{
	public class SplitButtonEx
	{
		private readonly ISplitButtonExClient mClient;

		private readonly Dictionary<string, ToolStripSeparator> mGroupSeparators = new Dictionary<string, ToolStripSeparator>();

		private SplitButton SplitButton
		{
			get
			{
				return this.mClient.SplitButton;
			}
		}

		public ContextMenuStrip SplitMenu
		{
			get
			{
				return this.SplitButton.SplitMenuStrip;
			}
		}

		public string DefaultAction
		{
			get
			{
				return this.SplitButton.Text;
			}
			set
			{
				this.SplitButton.Text = value;
			}
		}

		public SplitButtonEx(ISplitButtonExClient client)
		{
			this.mClient = client;
			if (this.SplitButton.SplitMenuStrip == null)
			{
				this.SplitButton.SplitMenuStrip = new ContextMenuStrip();
			}
			this.SplitMenu.Opening += new CancelEventHandler(this.SplitMenu_Opening);
			this.SplitButton.Click += new EventHandler(this.SplitButton_Click);
			this.DefaultAction = this.mClient.SplitButtonEmptyDefault;
		}

		public ToolStripItem AddItem(string itemText, Image itemImage = null, string groupName = "")
		{
			ToolStripSeparator orCreateGroupSeparator = this.GetOrCreateGroupSeparator(groupName);
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(itemText, itemImage, new EventHandler(this.OnSplitButtonItem_Click));
			this.SplitMenu.Items.Insert(this.SplitMenu.Items.IndexOf(orCreateGroupSeparator), toolStripMenuItem);
			return toolStripMenuItem;
		}

		public void UpdateSplitMenu()
		{
			ToolStripItem toolStripItem = null;
			foreach (ToolStripItem toolStripItem2 in this.SplitMenu.Items)
			{
				bool flag = (toolStripItem2 is ToolStripSeparator) ? (toolStripItem != null && !(toolStripItem is ToolStripSeparator)) : this.mClient.IsItemVisible(toolStripItem2);
				toolStripItem2.Visible = flag;
				if (flag)
				{
					toolStripItem = toolStripItem2;
				}
				else if (this.DefaultAction == toolStripItem2.Text)
				{
					this.DefaultAction = this.mClient.SplitButtonEmptyDefault;
				}
			}
			if (toolStripItem is ToolStripSeparator)
			{
				toolStripItem.Visible = false;
			}
		}

		private void SplitButton_Click(object sender, EventArgs e)
		{
			if (string.Equals(this.SplitButton.Text, this.mClient.SplitButtonEmptyDefault, StringComparison.Ordinal))
			{
				this.SplitMenu.Show(this.SplitButton, new Point(0, this.SplitButton.Bottom - this.SplitButton.Top));
				return;
			}
			this.mClient.DefaultActionClicked();
		}

		private void SplitMenu_Opening(object sender, CancelEventArgs e)
		{
			if (sender != this.SplitMenu)
			{
				return;
			}
			this.UpdateSplitMenu();
		}

		private void OnSplitButtonItem_Click(object sender, EventArgs e)
		{
			ToolStripItem toolStripItem = sender as ToolStripItem;
			if (toolStripItem == null)
			{
				return;
			}
			this.DefaultAction = toolStripItem.Text;
			this.mClient.ItemClicked(toolStripItem);
		}

		private ToolStripSeparator GetOrCreateGroupSeparator(string groupName)
		{
			if (!this.mGroupSeparators.ContainsKey(groupName))
			{
				ToolStripSeparator value = new ToolStripSeparator
				{
					Tag = groupName
				};
				this.mGroupSeparators.Add(groupName, value);
				this.SplitMenu.Items.Add(value);
			}
			return this.mGroupSeparators[groupName];
		}
	}
}
