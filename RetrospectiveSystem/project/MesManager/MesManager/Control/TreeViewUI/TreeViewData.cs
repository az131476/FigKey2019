using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using CommonUtils.FileHelper;
using System.IO;

namespace MesManager.Control.TreeViewUI
{
    class TreeViewData
    {
        static DataTable dt;
        public static void LoadTreeView(TreeView treeView1)
        {
            treeView1.Nodes.Clear();

            TreeNode root1 = new TreeNode("General");
            root1.Name = "general_";
            treeView1.Nodes.Add(root1);
            DirOperate dirOperate = new DirOperate();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string slop_name = dt.Rows[i]["slop_name"].ToString();
                string station = dt.Rows[i]["station_name"].ToString();

                TreeNode unitNode = new TreeNode();
                unitNode.Name = slop_name;
                unitNode.Text = slop_name;
                if (!root1.Nodes.ContainsKey(unitNode.Name))
                    root1.Nodes.Add(unitNode);
                NodeStation(slop_name, unitNode);
            }
            root1.Expand();
            root1.ExpandAll();
            dt.Clear();
        }
        public static void NodeStation(string slop_name, TreeNode node)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string stationName = "";
                string slopName = dt.Rows[i]["slop_name"].ToString();
                if (slopName.Equals(slop_name))
                {
                    stationName = dt.Rows[i]["station_name"].ToString();
                    TreeNode snode = new TreeNode(stationName);
                    node.Nodes.Add(snode);
                }
            }
        }

        public static void PopulateTreeView(string path,TreeView treeView)
        {
            TreeNode rootNode;
            DirectoryInfo info = new DirectoryInfo(path);
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView.Nodes.Add(rootNode);
            }
        }

        private static void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }
    }
}
