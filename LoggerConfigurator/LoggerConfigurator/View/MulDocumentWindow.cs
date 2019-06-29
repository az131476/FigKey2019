using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using Telerik.WinControls.UI.Docking;

namespace LoggerConfigurator.View
{
    class MulDocumentWindow : RadForm
    {
        public void CreateDocumentWindowCan1()
        {

        }

        public void CreateDocumentWindowCan2()
        {

        }

        public void CreateDocumentWindowHardWareConfig(RadDock radDock)
        {
            DocumentWindow documentWindowHardWare = new DocumentWindow();
            documentWindowHardWare.Name = "dwHardWare";
            documentWindowHardWare.Text = "硬件配置";

            RadGroupBox radGroupBox = new RadGroupBox();
            radGroupBox.Text = "通道配置";

            RadLabel radLabel = new RadLabel();
            radLabel.Text = "波特率";
            radLabel.Left = 3;
            radLabel.Top = 3;

            RadTextBox radTextBox1 = new RadTextBox();
            radTextBox1.Left = 2;


            RadLabel lbx_protocol = new RadLabel();
            lbx_protocol.Text = "协议";
            lbx_protocol.Top = 3;
            lbx_protocol.Left = 3;

            RadTextBox radTextBox2 = new RadTextBox();
            radTextBox2.Left = 2;

            radGroupBox.Controls.Add(radLabel);
            radGroupBox.Controls.Add(radTextBox1);
            radGroupBox.Controls.Add(lbx_protocol);
            radGroupBox.Controls.Add(radTextBox2);

            documentWindowHardWare.Controls.Add(radGroupBox);
            radDock.AddDocument(documentWindowHardWare);
        }
    }
}
