using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace CodeGenerator
{
    public partial class FrmConfig : Form
    {
        public FrmConfig()
        {
            InitializeComponent();
            this.InitConfig();
        }

        #region 初始化默认配置
        public void InitConfig()
        {
            this.txtCopyRight.Text = GlobalConfig.Item.CopyRight;
            this.txtAuthor.Text = GlobalConfig.Item.Author;
            this.txtAuthorEmail.Text = GlobalConfig.Item.AuthorEmail;
            this.txtOnline.Text = GlobalConfig.Item.Online;
            this.txtEntityNameSpace.Text = GlobalConfig.Item.EntityNameSpace;
            this.txtEntityMapperFile.Text = GlobalConfig.Item.EntityMapperFile;
            this.txtComponentNameSpace.Text = GlobalConfig.Item.ComponentNameSpace;
            this.txtDAONameSpace.Text = GlobalConfig.Item.DAONameSpace;
            this.txtBIZNameSpace.Text = GlobalConfig.Item.BIZNameSpace;
            this.txtDAOClassPostFix.Text = GlobalConfig.Item.DAOClassPostFix;
            this.txtBIZClassPostFix.Text = GlobalConfig.Item.BIZClassPostFix;
            this.txtOutputPath.Text = GlobalConfig.Item.OutputPath;
            this.cbIsAllowView.Checked = GlobalConfig.Item.IsAllowView;
        }
        #endregion

        #region 刷新配置
        private void RefreshConfig()
        {
            GlobalConfig.Item.CopyRight = this.txtCopyRight.Text;
            GlobalConfig.Item.Author = this.txtAuthor.Text;
            GlobalConfig.Item.AuthorEmail = this.txtAuthorEmail.Text;
            GlobalConfig.Item.Online = this.txtOnline.Text;
            GlobalConfig.Item.EntityNameSpace = this.txtEntityNameSpace.Text;
            GlobalConfig.Item.EntityMapperFile = this.txtEntityMapperFile.Text;
            GlobalConfig.Item.ComponentNameSpace = this.txtComponentNameSpace.Text;
            GlobalConfig.Item.DAONameSpace = this.txtDAONameSpace.Text;
            GlobalConfig.Item.BIZNameSpace = this.txtBIZNameSpace.Text;
            GlobalConfig.Item.DAOClassPostFix = this.txtDAOClassPostFix.Text;
            GlobalConfig.Item.BIZClassPostFix = this.txtBIZClassPostFix.Text;
            GlobalConfig.Item.OutputPath = this.txtOutputPath.Text;
            GlobalConfig.Item.IsAllowView = this.cbIsAllowView.Checked;
        }
        #endregion

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                this.RefreshConfig();

                #region 保存配置到config.txt文件中

                string file = Application.ExecutablePath;
                file = Path.GetDirectoryName(file) + "/config.txt";
                if (File.Exists(file)) File.Delete(file);
                ConfigFileUtil<ConfigItem>.SaveToFile(GlobalConfig.Item, file);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.RefreshConfig();
                int count = GenerateCodeUtil.GenerateEntityClass();
                count += GenerateCodeUtil.GenerateEntityMapperFile();
                count += GenerateCodeUtil.GenerateComponents();
                count += GenerateCodeUtil.GenerateDaoLayer();
                count += GenerateCodeUtil.GenerateBusinessLayer();
                count += GenerateCodeUtil.GenerateAppConfig();
                count += GenerateCodeUtil.GenerateWebConfig();
                MessageBox.Show("本次共生成" + count + "个文件!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.RefreshConfig();
            Thread t = new Thread(new ThreadStart(DoTask));
            t.Start();
            this.Close();
        }

        public void DoTask()
        {
            FrmStart fs = new FrmStart();
            Application.Run(fs);
        }
    }
}