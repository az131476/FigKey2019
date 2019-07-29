using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace MesManager.UI
{
    public partial class ReportCenter : RadForm
    {
        public ReportCenter()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            Init();
            EventHandlers();
        }

        private void EventHandlers()
        {
            this.radButtonElementSN.Click += RadButtonElementSN_Click;
            this.radButtonElementQuanlity.Click += RadButtonElementQuanlity_Click;
            this.radButtonElementPackage.Click += RadButtonElementPackage_Click;
            this.radButtonElementMaterial.Click += RadButtonElementMaterial_Click;
        }

        private void Init()
        {
            this.radDock1.RemoveAllWindows();
            //this.radRibbonBar1.show
        }

        #region Event Handlers
        private void RadButtonElementMaterial_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_material);
            this.radDock1.ActiveWindow = this.dw_material;
        }

        private void RadButtonElementPackage_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_package);
            this.radDock1.ActiveWindow = this.dw_package;
        }

        private void RadButtonElementQuanlity_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_quanlity);
            this.radDock1.ActiveWindow = this.dw_quanlity;
        }

        private void RadButtonElementSN_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_sn);
            this.radDock1.ActiveWindow = this.dw_sn;
        }
        #endregion
    }
}
