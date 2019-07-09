using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RetrospectiveManager.RadView
{
    public partial class PackageProduct : Telerik.WinControls.UI.RadForm
    {
        public PackageProduct()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void PackageProduct_Load(object sender, EventArgs e)
        {
            
        }
    }
}
