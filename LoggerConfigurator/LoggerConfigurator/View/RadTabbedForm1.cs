using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace FigKeyLoggerConfigurator.View
{
    public partial class RadTabbedForm1 : Telerik.WinControls.UI.RadTabbedForm
    {
        public RadTabbedForm1()
        {
            InitializeComponent();

            this.AllowAero = false;
        }
    }
}
