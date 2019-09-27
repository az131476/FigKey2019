using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Spire.Pdf;
using CommonUtils.FileHelper;

namespace ConvertTool
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        private string sourchFilePath;
        private string currentSaveFilePath;
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void PDFToWord(string sourchFilePath)
        {
            if (sourchFilePath.Contains(".pdf"))
            {
                currentSaveFilePath = sourchFilePath.Replace(".pdf", "") + ".doc";
            }
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(sourchFilePath);
            doc.SaveToFile(currentSaveFilePath, FileFormat.DOCX);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.menu_addFile.Click += Menu_addFile_Click;
            this.btn_startConvert.Click += Btn_startConvert_Click;
        }

        private void Btn_startConvert_Click(object sender, EventArgs e)
        {
            //是否启动
            PDFToWord(sourchFilePath);

            if (MessageBox.Show("是否立即打开转换后的文件", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;
            System.Diagnostics.Process.Start(currentSaveFilePath);
        }

        private void Menu_addFile_Click(object sender, EventArgs e)
        {
            FileContent fileContent = FileSelect.GetSelectFileContent("(*.*)|*.*","选择文件");
            sourchFilePath = fileContent.FileName;
        }
    }
}
