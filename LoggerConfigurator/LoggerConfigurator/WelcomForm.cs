using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using LoggerConfigurator.Properties;

namespace LoggerConfigurator
{
    public partial class WelcomForm : Telerik.WinControls.UI.RadForm
    {
        public WelcomForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.radPanorama1.PanelImage = Resources.ThViewer_bg;
            this.radPanorama1.PanelImageSize = new Size(this.radPanorama1.Width,this.radPanorama1.Height);
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
