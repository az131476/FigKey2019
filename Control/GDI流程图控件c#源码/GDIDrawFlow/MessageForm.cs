using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GDIDrawFlow
{

	public enum ButtonType
	{
		/// <summary>
		/// �Ǻͷ�ť
		/// </summary>
		YesAndNo,
		/// <summary>
		/// ȷ����ȡ����ť
		/// </summary>
		ConfirmAndCancle,
		/// <summary>
		/// ȷ��
		/// </summary>
		ConfirmOnly
	}

	/// <summary>
	/// MessageForm ��ժҪ˵����
	/// </summary>
	public class MessageForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lbl_Yes;
		private System.Windows.Forms.Label lbl_No;
		private System.Windows.Forms.Label lbl_info;
		private System.Windows.Forms.Label lbl_title;
		private System.Windows.Forms.PictureBox pb_close;
		private System.Windows.Forms.Panel pnl_MoveBar;
		private System.ComponentModel.IContainer components;
		private string info;
		private string title;
		private ButtonType buttonType;
		private int cx,cy;
		private bool isDown;
		private System.Windows.Forms.Label lbl_Cancel;
		private System.Windows.Forms.Label lbl_Confirm;
		private System.Windows.Forms.Timer showForm;
		public MessageForm(string info,string title,ButtonType buttonType)
		{
			this.info=info;
			this.title=title;
			this.buttonType=buttonType;
			InitializeComponent();
			this.BackgroundImage=new Bitmap(GetType(),"images.message.jpg");
		}
		public MessageForm(string info):this(info,"ϵͳ��ʾ",ButtonType.ConfirmOnly)
		{
		}
		public MessageForm(string info,string title):this(info,title,ButtonType.ConfirmOnly)
		{
		}
		public MessageForm(string info,ButtonType buttonType):this(info,"ϵͳ��ʾ",buttonType)
		{
		}
		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MessageForm));
			this.lbl_Yes = new System.Windows.Forms.Label();
			this.lbl_No = new System.Windows.Forms.Label();
			this.lbl_Cancel = new System.Windows.Forms.Label();
			this.lbl_Confirm = new System.Windows.Forms.Label();
			this.lbl_info = new System.Windows.Forms.Label();
			this.lbl_title = new System.Windows.Forms.Label();
			this.pb_close = new System.Windows.Forms.PictureBox();
			this.pnl_MoveBar = new System.Windows.Forms.Panel();
			this.showForm = new System.Windows.Forms.Timer(this.components);
			this.pnl_MoveBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbl_Yes
			// 
			this.lbl_Yes.BackColor = System.Drawing.Color.Transparent;
			this.lbl_Yes.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lbl_Yes.Image = ((System.Drawing.Image)(resources.GetObject("lbl_Yes.Image")));
			this.lbl_Yes.Location = new System.Drawing.Point(132, 150);
			this.lbl_Yes.Name = "lbl_Yes";
			this.lbl_Yes.Size = new System.Drawing.Size(64, 22);
			this.lbl_Yes.TabIndex = 0;
			this.lbl_Yes.Text = "��";
			this.lbl_Yes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbl_Yes.Visible = false;
			this.lbl_Yes.Click += new System.EventHandler(this.lbl_confirm_Click);
			// 
			// lbl_No
			// 
			this.lbl_No.BackColor = System.Drawing.Color.Transparent;
			this.lbl_No.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lbl_No.Image = ((System.Drawing.Image)(resources.GetObject("lbl_No.Image")));
			this.lbl_No.Location = new System.Drawing.Point(204, 150);
			this.lbl_No.Name = "lbl_No";
			this.lbl_No.Size = new System.Drawing.Size(64, 22);
			this.lbl_No.TabIndex = 1;
			this.lbl_No.Text = "��";
			this.lbl_No.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbl_No.Visible = false;
			this.lbl_No.Click += new System.EventHandler(this.lbl_confirm_Click);
			// 
			// lbl_Cancel
			// 
			this.lbl_Cancel.BackColor = System.Drawing.Color.Transparent;
			this.lbl_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lbl_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("lbl_Cancel.Image")));
			this.lbl_Cancel.Location = new System.Drawing.Point(204, 150);
			this.lbl_Cancel.Name = "lbl_Cancel";
			this.lbl_Cancel.Size = new System.Drawing.Size(64, 22);
			this.lbl_Cancel.TabIndex = 3;
			this.lbl_Cancel.Text = "ȡ��";
			this.lbl_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbl_Cancel.Visible = false;
			this.lbl_Cancel.Click += new System.EventHandler(this.lbl_confirm_Click);
			// 
			// lbl_Confirm
			// 
			this.lbl_Confirm.BackColor = System.Drawing.Color.Transparent;
			this.lbl_Confirm.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lbl_Confirm.Image = ((System.Drawing.Image)(resources.GetObject("lbl_Confirm.Image")));
			this.lbl_Confirm.Location = new System.Drawing.Point(132, 150);
			this.lbl_Confirm.Name = "lbl_Confirm";
			this.lbl_Confirm.Size = new System.Drawing.Size(64, 22);
			this.lbl_Confirm.TabIndex = 2;
			this.lbl_Confirm.Text = "ȷ��";
			this.lbl_Confirm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbl_Confirm.Visible = false;
			this.lbl_Confirm.Click += new System.EventHandler(this.lbl_confirm_Click);
			// 
			// lbl_info
			// 
			this.lbl_info.BackColor = System.Drawing.Color.Transparent;
			this.lbl_info.Location = new System.Drawing.Point(100, 56);
			this.lbl_info.Name = "lbl_info";
			this.lbl_info.Size = new System.Drawing.Size(252, 84);
			this.lbl_info.TabIndex = 4;
			this.lbl_info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbl_title
			// 
			this.lbl_title.BackColor = System.Drawing.Color.Transparent;
			this.lbl_title.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.lbl_title.ForeColor = System.Drawing.Color.White;
			this.lbl_title.Location = new System.Drawing.Point(6, 6);
			this.lbl_title.Name = "lbl_title";
			this.lbl_title.Size = new System.Drawing.Size(360, 16);
			this.lbl_title.TabIndex = 5;
			this.lbl_title.Text = "ϵͳ��ʾ";
			this.lbl_title.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnl_MoveBar_MouseUp);
			this.lbl_title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnl_MoveBar_MouseMove);
			this.lbl_title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnl_MoveBar_MouseDown);
			// 
			// pb_close
			// 
			this.pb_close.BackColor = System.Drawing.Color.Transparent;
			this.pb_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pb_close.BackgroundImage")));
			this.pb_close.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pb_close.Location = new System.Drawing.Point(372, 6);
			this.pb_close.Name = "pb_close";
			this.pb_close.Size = new System.Drawing.Size(16, 16);
			this.pb_close.TabIndex = 6;
			this.pb_close.TabStop = false;
			this.pb_close.Click += new System.EventHandler(this.pb_close_Click);
			// 
			// pnl_MoveBar
			// 
			this.pnl_MoveBar.BackColor = System.Drawing.Color.Transparent;
			this.pnl_MoveBar.Controls.Add(this.lbl_title);
			this.pnl_MoveBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnl_MoveBar.Location = new System.Drawing.Point(0, 0);
			this.pnl_MoveBar.Name = "pnl_MoveBar";
			this.pnl_MoveBar.Size = new System.Drawing.Size(398, 28);
			this.pnl_MoveBar.TabIndex = 7;
			this.pnl_MoveBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnl_MoveBar_MouseUp);
			this.pnl_MoveBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnl_MoveBar_MouseMove);
			this.pnl_MoveBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnl_MoveBar_MouseDown);
			// 
			// showForm
			// 
			this.showForm.Enabled = true;
			this.showForm.Interval = 10;
			this.showForm.Tick += new System.EventHandler(this.showForm_Tick);
			// 
			// MessageForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(398, 188);
			this.Controls.Add(this.pb_close);
			this.Controls.Add(this.lbl_info);
			this.Controls.Add(this.lbl_Cancel);
			this.Controls.Add(this.lbl_Confirm);
			this.Controls.Add(this.lbl_No);
			this.Controls.Add(this.lbl_Yes);
			this.Controls.Add(this.pnl_MoveBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MessageForm";
			this.Opacity = 0;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ϵͳ��ʾ";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageForm_KeyDown);
			this.Load += new System.EventHandler(this.MessageForm_Load);
			this.pnl_MoveBar.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void MessageForm_Load(object sender, System.EventArgs e)
		{
			this.lbl_title.Text=this.title;//���ô������
			//this.lbl_title.Text="ϵͳ��ʾ";
			this.lbl_info.Text=this.info;//���ô�����ʾ��Ϣ
			switch(buttonType)
			{
				case ButtonType.ConfirmAndCancle:
					this.lbl_Confirm.Visible =true;
					this.lbl_Cancel.Visible =true;
					break;
				case ButtonType.ConfirmOnly:
					this.lbl_Confirm.Left=this.Width/2-this.lbl_Confirm.Width/2;
					this.lbl_Confirm.Visible =true;
					break;
				case ButtonType.YesAndNo:
					this.lbl_Yes.Visible =true;
					this.lbl_No.Visible =true;
					break;
			}
		}

		#region �����ƶ��¼�
		private void pnl_MoveBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			isDown=true;
			cx=e.X;
			cy=e.Y;
		}

		private void pnl_MoveBar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(isDown)
			{
				this.Left+=e.X-cx;
				this.Top +=e.Y-cy;
			}
		}

		private void pnl_MoveBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			isDown=false;
		}
		#endregion

		private void pb_close_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void lbl_confirm_Click(object sender, System.EventArgs e)
		{
			Label lbl_Button=(Label)sender;
			switch(lbl_Button.Name)
			{
				case "lbl_Yes":
					DialogResult=DialogResult.Yes;
					break;
				case "lbl_No":
					DialogResult=DialogResult.No;
					break;
				case "lbl_Confirm":
					DialogResult=DialogResult.OK;
					break;
				case "lbl_Cancel":
					DialogResult=DialogResult.Cancel;
					break;
			}
		}
		/// <summary>
		/// ��ʾ�Զ���Ի���
		/// </summary>
		/// <param name="info">��Ϣ</param>
		/// <param name="title">����</param>
		/// <param name="buttonType">��ť����</param>
		/// <returns>����DialogResultֵ</returns>
		public static  DialogResult Show(string info,string title,ButtonType buttonType)
		{
			MessageForm mf=new MessageForm(info,title,buttonType);
			mf.ShowDialog();
			return mf.DialogResult;
		}
		/// <summary>
		/// ��ʾ�Զ���Ի���
		/// </summary>
		/// <param name="info">��Ϣ</param>
		/// <returns>����DialogResultֵ</returns>
		public static  DialogResult Show(string info)
		{
			MessageForm mf=new MessageForm(info);
			mf.ShowDialog();
			return mf.DialogResult;
		}
		/// <summary>
		/// ��ʾ�Զ���Ի���
		/// </summary>
		/// <param name="info">��Ϣ</param>
		/// <param name="title">����</param>
		/// <returns>����DialogResultֵ</returns>
		public static  DialogResult Show(string info,string title)
		{
			MessageForm mf=new MessageForm(info,title);
			mf.ShowDialog();
			return mf.DialogResult;
		}
		/// <summary>
		/// ��ʾ�Զ���Ի���
		/// </summary>
		/// <param name="info">��Ϣ</param>
		/// <param name="buttonType">��ť����</param>
		/// <returns>����DialogResultֵ</returns>
		public static  DialogResult Show(string info,ButtonType buttonType)
		{
			MessageForm mf=new MessageForm(info,buttonType);
			mf.ShowDialog();
			return mf.DialogResult;
		}
		private void showForm_Tick(object sender, System.EventArgs e)
		{
			if(this.Opacity<0.8)
			{
				this.Opacity+=0.05;
			}
			else
			{
				this.showForm .Stop();
			}
		}

		private void MessageForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode==Keys.Enter)
			{
				switch(buttonType)
				{
					case ButtonType.ConfirmAndCancle:
					case ButtonType.ConfirmOnly:
						DialogResult=DialogResult.OK;
						break;
					case ButtonType.YesAndNo:
						DialogResult=DialogResult.Yes;
						break;
				}
			}
		}
	}
}
