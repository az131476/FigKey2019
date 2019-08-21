using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NetronLight;
namespace LightUI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		
		private System.Windows.Forms.Panel RightPanel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Panel MainPanel;
		private NetronLight.GraphControl graphControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			#region Creation of a sample diagram

			#region The shapes
			TextLabel label = this.graphControl1.AddShape(ShapeTypes.TextLabel, new Point(100,33)) as TextLabel;

			label.Text = "This is the class hierarchy of Netron-Light.";
			label.Width = 350;
			label.Height =33;

			SimpleRectangle ent = this.graphControl1.AddShape(ShapeTypes.Rectangular,new Point(200,100)) as SimpleRectangle;
			ent.Text = "Entity";
			ent.Height = 33;
			ent.ShapeColor = Color.SteelBlue;

			SimpleRectangle conn = this.graphControl1.AddShape(ShapeTypes.Rectangular,new Point(100,200)) as SimpleRectangle;
			conn.Text = "Connection";
			conn.Height = 33;
			conn.ShapeColor = Color.LightSteelBlue;

			SimpleRectangle shbase = this.graphControl1.AddShape(ShapeTypes.Rectangular,new Point(250,200)) as SimpleRectangle;
			shbase.Text = "ShapeBase";
			shbase.Height = 33;
			shbase.ShapeColor = Color.LightSteelBlue;

			SimpleRectangle con = this.graphControl1.AddShape(ShapeTypes.Rectangular,new Point(400,200)) as SimpleRectangle;
			con.Text = "Connector";
			con.Height = 33;
			con.ShapeColor = Color.LightSteelBlue;

			OvalShape oval = this.graphControl1.AddShape(ShapeTypes.Oval,new Point(100,300)) as OvalShape;
			oval.Text = "Oval";
			oval.Height = 33;
			oval.ShapeColor = Color.AliceBlue;

			OvalShape rec = this.graphControl1.AddShape(ShapeTypes.Oval,new Point(225,350)) as OvalShape;
			rec.Text = "SimpleRectangle";
			rec.Height = 33;
			rec.Width = 150;
			rec.ShapeColor = Color.AliceBlue;

			OvalShape tl = this.graphControl1.AddShape(ShapeTypes.Oval,new Point(400,300)) as OvalShape;
			tl.Text = "TextLabel";
			tl.Height = 33;
			tl.ShapeColor = Color.AliceBlue;

			#endregion

			#region The connections
			this.graphControl1.AddConnection(ent.Connectors[0], conn.Connectors[3]);
			this.graphControl1.AddConnection(ent.Connectors[0], con.Connectors[3]);
			this.graphControl1.AddConnection(ent.Connectors[0], shbase.Connectors[3]);
			this.graphControl1.AddConnection(shbase.Connectors[0], oval.Connectors[3]);
			this.graphControl1.AddConnection(shbase.Connectors[0], rec.Connectors[3]);
			this.graphControl1.AddConnection(shbase.Connectors[0], tl.Connectors[3]);

			

			#endregion

			//finally, some publicity


			TextLabel pub = this.graphControl1.AddShape(ShapeTypes.TextLabel, new Point(100,450)) as TextLabel;

			pub.Text = "Visit the Netron Project for a complete (open source) graph library \n and advanced graph applications: http://netron.sf.net";
			pub.Width = 500;
			pub.Height =60;


			#endregion

			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.RightPanel = new System.Windows.Forms.Panel();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.graphControl1 = new NetronLight.GraphControl();
			this.RightPanel.SuspendLayout();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// RightPanel
			// 
			this.RightPanel.Controls.Add(this.propertyGrid1);
			this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.RightPanel.Location = new System.Drawing.Point(648, 0);
			this.RightPanel.Name = "RightPanel";
			this.RightPanel.Size = new System.Drawing.Size(200, 614);
			this.RightPanel.TabIndex = 0;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.BackColor = System.Drawing.Color.Gainsboro;
			this.propertyGrid1.CommandsBackColor = System.Drawing.Color.Gainsboro;
			this.propertyGrid1.CommandsForeColor = System.Drawing.Color.Gray;
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.HelpBackColor = System.Drawing.Color.Gainsboro;
			this.propertyGrid1.HelpForeColor = System.Drawing.Color.SlateGray;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.Color.Gainsboro;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(200, 614);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.Color.SlateGray;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(645, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 614);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.graphControl1);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(645, 614);
			this.MainPanel.TabIndex = 4;
			// 
			// graphControl1
			// 
			this.graphControl1.BackColor = System.Drawing.Color.Gainsboro;
			this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphControl1.Location = new System.Drawing.Point(0, 0);
			this.graphControl1.Name = "graphControl1";
			this.graphControl1.ShowGrid = false;
			this.graphControl1.Size = new System.Drawing.Size(645, 614);
			this.graphControl1.TabIndex = 1;
			this.graphControl1.OnShowProps += new NetronLight.GraphControl.ShowProps(this.graphControl1_OnShowProps);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(848, 614);
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.RightPanel);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Netron Light";
			this.RightPanel.ResumeLayout(false);
			this.MainPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void graphControl1_OnShowProps(object ent)
		{
			this.propertyGrid1.SelectedObject = ent;
		}


	}
}
