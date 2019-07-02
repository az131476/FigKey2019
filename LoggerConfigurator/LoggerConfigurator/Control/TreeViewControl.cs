using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using System.Data;
using FigKeyLoggerConfigurator.Model;

namespace FigKeyLoggerConfigurator.Control
{
    public class TreeViewControl
    {
        RadTreeView radTreeView;
        public TreeViewControl(RadTreeView treeView)
        {
            this.radTreeView = treeView;
        }
        /// <summary>
        /// 初始化treeview
        /// </summary>
        public void InitTreeView()
        {
            #region hardWard tree
            RadTreeNode hardWardRoot = radTreeView.AddNodeByPath(TreeViewData.HardWare.ROOT);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.COMMENT);
            RadTreeNode nodeCanChannel = hardWardRoot.Nodes.Add(TreeViewData.HardWare.CAN_CHANNELS);
            hardWardRoot.Nodes.Add(TreeViewData.HardWare.LIN_CHANNELS);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.FLEXRAY_CHANNELS);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.MOST150_CHANNELS);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.ANALOG_INPUTS);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.DIGITAL_INPUTS);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.CAN_GPS);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.MONITORING);
            //hardWardRoot.Nodes.Add(TreeViewData.HardWare.WLAN_3G);

            RadTreeNode nodeCan1 = nodeCanChannel.Nodes.Add(TreeViewData.HardWare.CAN_CHILD + 1);
            nodeCan1.Nodes.Add(TreeViewData.HardWare.CAN_HARDWARE_CONFIG);
            nodeCan1.Nodes.Add(TreeViewData.HardWare.CAN_1_DATA);
            RadTreeNode nodeCan2 = nodeCanChannel.Nodes.Add(TreeViewData.HardWare.CAN_CHILD + 2);
            nodeCan2.Nodes.Add(TreeViewData.HardWare.CAN_HARDWARE_CONFIG);
            nodeCan2.Nodes.Add(TreeViewData.HardWare.CAN_2_DATA);
            #endregion

            #region hardWard tree
            RadTreeNode generalRoot = radTreeView.AddNodeByPath(TreeViewData.General.ROOT);
            generalRoot.Nodes.Add(TreeViewData.General.DATABASE);
            generalRoot.Nodes.Add(TreeViewData.General.SPECIAL_FEATURES);
            generalRoot.Nodes.Add(TreeViewData.General.INCLUDE_FILES);
            #endregion

            #region LoggingMemory1
            RadTreeNode loggingMemory = radTreeView.AddNodeByPath(TreeViewData.LoggingMemory1.ROOT);
            loggingMemory.Nodes.Add(TreeViewData.LoggingMemory1.TRIGGERS);
            loggingMemory.Nodes.Add(TreeViewData.LoggingMemory1.FILTERS);
            #endregion

            #region LoggingMemory2
            RadTreeNode loggingMemory2 = radTreeView.AddNodeByPath(TreeViewData.LoggingMemory2.ROOT);
            loggingMemory2.Nodes.Add(TreeViewData.LoggingMemory2.TRIGGERS);
            loggingMemory2.Nodes.Add(TreeViewData.LoggingMemory2.FILTERS);
            #endregion

            #region XCP/CCP
            RadTreeNode xcpccp = radTreeView.AddNodeByPath(TreeViewData.CcpOrXcp.ROOT);
            xcpccp.Nodes.Add(TreeViewData.CcpOrXcp.DESCRIPTIONS);
            xcpccp.Nodes.Add(TreeViewData.CcpOrXcp.SIGNAL_REQUESTS);
            #endregion

            #region diagnotics
            RadTreeNode diagnotics = radTreeView.AddNodeByPath(TreeViewData.Diagnostics.ROOT);
            diagnotics.Nodes.Add(TreeViewData.Diagnostics.DIAGNOSTIC_DESCRIPTIONS);
            diagnotics.Nodes.Add(TreeViewData.Diagnostics.REQUESTS);
            #endregion

            #region output
            RadTreeNode output = radTreeView.AddNodeByPath(TreeViewData.Output.ROOT);
            output.Nodes.Add(TreeViewData.Output.LEDS);
            output.Nodes.Add(TreeViewData.Output.TRANSMIT_MESSAGE);
            output.Nodes.Add(TreeViewData.Output.SET_IGITAL_OUTPUT);
            #endregion

            #region filemanager
            RadTreeNode fileManager = radTreeView.AddNodeByPath(TreeViewData.FileManager1.ROOT);
            RadTreeNode loggerDevice = fileManager.Nodes.Add(TreeViewData.FileManager1.LOGGER_DEVICE);
            RadTreeNode cardReader = fileManager.Nodes.Add(TreeViewData.FileManager1.CARD_READER);

            loggerDevice.Nodes.Add(TreeViewData.FileManager1.DEVICE_INFORMATION);
            loggerDevice.Nodes.Add(TreeViewData.FileManager1.CLASSIC_VIEW);
            loggerDevice.Nodes.Add(TreeViewData.FileManager1.NAVIGATOR_VIEW);

            cardReader.Nodes.Add(TreeViewData.FileManager1.CLASSIC_VIEW);
            cardReader.Nodes.Add(TreeViewData.FileManager1.NAVIGATOR_VIEW);
            #endregion
        }
    }
}
