using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetronLight;

namespace LightUI
{
    public partial class TestNetronLight : Form
    {
        public TestNetronLight()
        {
            InitializeComponent();
        }

        private void TestNetronLight_Load(object sender, EventArgs e)
        {

            #region The shapes
            TextLabel label = this.graphControl1.AddShape(ShapeTypes.TextLabel, new Point(100, 33)) as TextLabel;

            label.Text = "This is the class hierarchy of Netron-Light.";
            label.Width = 350;
            label.Height = 33;

            SimpleRectangle ent = this.graphControl1.AddShape(ShapeTypes.Rectangular, new Point(100, 100)) as SimpleRectangle;
            ent.Text = "Entity";
            ent.Height = 33;
            ent.ShapeColor = Color.SteelBlue;

            SimpleRectangle conn = this.graphControl1.AddShape(ShapeTypes.Rectangular, new Point(250, 100)) as SimpleRectangle;
            conn.Text = "Connection";
            conn.Height = 33;
            conn.ShapeColor = Color.LightSteelBlue;

            SimpleRectangle station_3 = this.graphControl1.AddShape(ShapeTypes.Rectangular, new Point(400, 100)) as SimpleRectangle;
            station_3.Text = "station_3";
            station_3.Height = 33;
            station_3.ShapeColor = Color.BurlyWood;

            #endregion

            #region The connections
            this.graphControl1.AddConnection(ent.Connectors[2], conn.Connectors[1]);
            this.graphControl1.AddConnection(conn.Connectors[2], station_3.Connectors[1]);
            //this.graphControl1.AddConnection(ent.Connectors[0], shbase.Connectors[3]);
            #endregion
            //this.graphControl1.Enabled = false;
            this.graphControl1.ShowGrid = false;
            this.graphControl1.BackColor = Color.DarkTurquoise;
            this.graphControl1.MouseDown += GraphControl1_MouseDown;

        }

        private void GraphControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
            }
        }

    }
}
