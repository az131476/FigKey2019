namespace MesManager.RadView
{
    partial class MESMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MESMainForm));
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.toolsGroup = new Telerik.WinControls.UI.TileGroupElement();
            this.mainBasicInfo = new Telerik.WinControls.UI.RadTileElement();
            this.mainMaterialManager = new Telerik.WinControls.UI.RadTileElement();
            this.mainGraphView = new Telerik.WinControls.UI.RadTileElement();
            this.mainProcess = new Telerik.WinControls.UI.RadTileElement();
            this.mainQuanlityAnomaly = new Telerik.WinControls.UI.RadTileElement();
            this.mainReportData = new Telerik.WinControls.UI.RadTileElement();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanorama1
            // 
            this.radPanorama1.AutoArrangeNewTiles = false;
            this.radPanorama1.BackColor = System.Drawing.Color.White;
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanorama1.Groups.AddRange(new Telerik.WinControls.RadItem[] {
            this.toolsGroup});
            this.radPanorama1.Location = new System.Drawing.Point(0, 0);
            this.radPanorama1.Name = "radPanorama1";
            this.radPanorama1.PanelImageSize = new System.Drawing.Size(1024, 768);
            this.radPanorama1.RowsCount = 2;
            this.radPanorama1.ShowGroups = true;
            this.radPanorama1.Size = new System.Drawing.Size(1108, 869);
            this.radPanorama1.TabIndex = 1;
            ((Telerik.WinControls.UI.RadPanoramaElement)(this.radPanorama1.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(23)))), ((int)(((byte)(117)))));
            // 
            // toolsGroup
            // 
            this.toolsGroup.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolsGroup.BorderBottomWidth = 1F;
            this.toolsGroup.CellSize = new System.Drawing.Size(155, 155);
            this.toolsGroup.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.toolsGroup.Font = new System.Drawing.Font("Segoe UI Light", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolsGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(172)))), ((int)(((byte)(255)))));
            this.toolsGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mainBasicInfo,
            this.mainMaterialManager,
            this.mainGraphView,
            this.mainProcess,
            this.mainQuanlityAnomaly,
            this.mainReportData});
            this.toolsGroup.Margin = new System.Windows.Forms.Padding(300, 150, 200, 0);
            this.toolsGroup.Name = "toolsGroup";
            this.toolsGroup.RowsCount = 2;
            this.toolsGroup.Text = "主功能";
            this.toolsGroup.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.toolsGroup.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.toolsGroup.UseCompatibleTextRendering = false;
            // 
            // mainBasicInfo
            // 
            this.mainBasicInfo.AccessibleDescription = "mainBasicInfo";
            this.mainBasicInfo.AccessibleName = "mainBasicInfo";
            this.mainBasicInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(126)))), ((int)(((byte)(216)))));
            this.mainBasicInfo.CellPadding = new System.Windows.Forms.Padding(5);
            this.mainBasicInfo.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainBasicInfo.DrawBorder = true;
            this.mainBasicInfo.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.mainBasicInfo.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainBasicInfo.Name = "mainBasicInfo";
            this.mainBasicInfo.Padding = new System.Windows.Forms.Padding(15, 15, 0, 10);
            this.mainBasicInfo.Text = "<html>基础资料 <br>";
            this.mainBasicInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainBasicInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.mainBasicInfo.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainBasicInfo.UseCompatibleTextRendering = false;
            // 
            // mainMaterialManager
            // 
            this.mainMaterialManager.AccessibleDescription = "mainMaterialManager";
            this.mainMaterialManager.AccessibleName = "mainMaterialManager";
            this.mainMaterialManager.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.mainMaterialManager.CellPadding = new System.Windows.Forms.Padding(5);
            this.mainMaterialManager.Column = 1;
            this.mainMaterialManager.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainMaterialManager.DrawBorder = true;
            this.mainMaterialManager.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.mainMaterialManager.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainMaterialManager.Name = "mainMaterialManager";
            this.mainMaterialManager.Padding = new System.Windows.Forms.Padding(15, 15, 0, 10);
            this.mainMaterialManager.Text = "<html>物料绑定 <br>";
            this.mainMaterialManager.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainMaterialManager.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.mainMaterialManager.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainMaterialManager.UseCompatibleTextRendering = false;
            // 
            // mainGraphView
            // 
            this.mainGraphView.AccessibleDescription = "mainGraphView";
            this.mainGraphView.AccessibleName = "mainGraphView";
            this.mainGraphView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.mainGraphView.CellPadding = new System.Windows.Forms.Padding(5);
            this.mainGraphView.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainGraphView.DrawBorder = true;
            this.mainGraphView.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.mainGraphView.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainGraphView.Name = "mainGraphView";
            this.mainGraphView.Padding = new System.Windows.Forms.Padding(15, 15, 0, 10);
            this.mainGraphView.Row = 1;
            this.mainGraphView.Text = "<html>报表查询 <br>";
            this.mainGraphView.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainGraphView.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.mainGraphView.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainGraphView.UseCompatibleTextRendering = false;
            // 
            // mainProcess
            // 
            this.mainProcess.AccessibleDescription = "mainProductPackage";
            this.mainProcess.AccessibleName = "mainProductPackage";
            this.mainProcess.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.mainProcess.CellPadding = new System.Windows.Forms.Padding(5);
            this.mainProcess.Column = 2;
            this.mainProcess.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainProcess.DrawBorder = true;
            this.mainProcess.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.mainProcess.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainProcess.Name = "mainProcess";
            this.mainProcess.Padding = new System.Windows.Forms.Padding(15, 15, 0, 10);
            this.mainProcess.Text = "<html>工艺流程 <br>";
            this.mainProcess.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.mainProcess.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainProcess.UseCompatibleTextRendering = false;
            // 
            // mainQuanlityAnomaly
            // 
            this.mainQuanlityAnomaly.AccessibleDescription = "mainQuanlityAnomaly";
            this.mainQuanlityAnomaly.AccessibleName = "mainQuanlityAnomaly";
            this.mainQuanlityAnomaly.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.mainQuanlityAnomaly.CellPadding = new System.Windows.Forms.Padding(5);
            this.mainQuanlityAnomaly.Column = 2;
            this.mainQuanlityAnomaly.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainQuanlityAnomaly.DrawBorder = true;
            this.mainQuanlityAnomaly.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.mainQuanlityAnomaly.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainQuanlityAnomaly.Name = "mainQuanlityAnomaly";
            this.mainQuanlityAnomaly.Padding = new System.Windows.Forms.Padding(15, 15, 0, 10);
            this.mainQuanlityAnomaly.Row = 1;
            this.mainQuanlityAnomaly.Text = "<html>品质异常管理 <br>";
            this.mainQuanlityAnomaly.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainQuanlityAnomaly.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.mainQuanlityAnomaly.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainQuanlityAnomaly.UseCompatibleTextRendering = false;
            // 
            // mainReportData
            // 
            this.mainReportData.AccessibleDescription = "mainReportData";
            this.mainReportData.AccessibleName = "mainReportData";
            this.mainReportData.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.mainReportData.CellPadding = new System.Windows.Forms.Padding(5);
            this.mainReportData.Column = 1;
            this.mainReportData.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainReportData.DrawBorder = true;
            this.mainReportData.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.mainReportData.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainReportData.Name = "mainReportData";
            this.mainReportData.Padding = new System.Windows.Forms.Padding(15, 15, 0, 10);
            this.mainReportData.Row = 1;
            this.mainReportData.Text = "<html>追溯管理 <br>";
            this.mainReportData.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainReportData.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.mainReportData.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainReportData.UseCompatibleTextRendering = false;
            // 
            // MESMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1108, 869);
            this.Controls.Add(this.radPanorama1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MESMainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.MESMainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.TileGroupElement toolsGroup;
        private Telerik.WinControls.UI.RadTileElement mainBasicInfo;//基础信息
        private Telerik.WinControls.UI.RadTileElement mainMaterialManager;//物料管理
        private Telerik.WinControls.UI.RadTileElement mainProcess;//成品装箱

        private Telerik.WinControls.UI.RadTileElement mainGraphView;//看板中心
        private Telerik.WinControls.UI.RadTileElement mainReportData;//报表中心
        private Telerik.WinControls.UI.RadTileElement mainQuanlityAnomaly;//品质异常管理
    }
}
