using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//+5+1+a+s+p+x
namespace 毕业设计_农产品追溯_
{
    public partial class DataEditUI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string action = Request["action"];
                if (action == "edit")
                {
                    FormView1.ChangeMode(FormViewMode.Edit);
                }
                else if (action == "addnew")
                {
                    FormView1.ChangeMode(FormViewMode.Insert);
                }
                else if (action == "view")
                {
                    FormView1.ChangeMode(FormViewMode.ReadOnly);
                }
                else
                {
                    throw new Exception("action错误!");
                }
            }
        }

        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            Response.Redirect("InformationEdit.aspx");

        }

        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            //FileUpload myfile = new FileUpload();
            //myfile = (FileUpload)FormView1.FindControl("FileUpload1");
            //TextBox ImageTextBox = (TextBox)FormView1.FindControl("ImageTextBox");
            //ImageTextBox.Text = "~/图片/" + myfile.FileName;
          
            Response.Redirect("InformationEdit.aspx");
        }

        protected void FormView1_ItemCreated(object sender, EventArgs e)
        {
            if (FormView1.CurrentMode == FormViewMode.Insert)
            {
                TextBox PluckingTimeTextBox = (TextBox)FormView1.FindControl("PluckingTimeTextBox");
                Literal litId = (Literal)FormView1.FindControl("litId");
                litId.Text = PluckingTimeTextBox.ClientID;
            }

            if (FormView1.CurrentMode == FormViewMode.Edit)
            {
                TextBox PluckingTimeTextBox = (TextBox)FormView1.FindControl("PluckingTimeTextBox");
                Literal litId = (Literal)FormView1.FindControl("litId");
                litId.Text = PluckingTimeTextBox.ClientID;
           
            }

            //TextBox ImageTextBox = (TextBox)FormView1.FindControl("ImageTextBox");
            //ImageTextBox.Text = "~/图片/";
        
            
           
        }

        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            
            string mypath = Request.PhysicalApplicationPath + "\\图片\\";        //抓實體主機目錄的位置
            string tempfileName = "";

            //這個4是因為這邊有四個FileUpload控制項
            FileUpload myfile = new FileUpload();
            myfile = (FileUpload)FormView1.FindControl("FileUpload1");              //把formview的fileupload放入新的myfile裡
            if (myfile.HasFile)
            {
                //檢查有沒有檔案
                if (myfile.PostedFile.ContentType == "image/pjpeg" | myfile.PostedFile.ContentType == "image/jpeg" | myfile.PostedFile.ContentType == "image/gif" | myfile.PostedFile.ContentType == "image/x-png")
                {                    //因為我只需要圖案而以~再加一個判斷
                    string pathToCheck = mypath + myfile.FileName;
                    string fileName = myfile.FileName;
                    if (System.IO.File.Exists(pathToCheck))
                    {                    //判斷檔案有沒有存在~有的話就改名啦~
                        int my_counter = 2;
                        while (System.IO.File.Exists(pathToCheck))
                        {
                            tempfileName = my_counter.ToString() + "_" + fileName;
                            pathToCheck = mypath + tempfileName;
                            my_counter = my_counter + 1;
                        }
                        fileName = tempfileName;
                    }
                    myfile.SaveAs(mypath + fileName);

                }
                else
                {
                    e.Cancel = true;                      //如果遇到不是圖檔的話~就直接跳出顯示錯誤

                   
                }

            }
            e.Values["Image"] = "~/图片/" + myfile.FileName;
         
       
            
       
        }

        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            string mypath = Request.PhysicalApplicationPath + "\\图片\\";        //抓實體主機目錄的位置
            string tempfileName = "";

            //這個4是因為這邊有四個FileUpload控制項
            FileUpload myfile = new FileUpload();
            myfile = (FileUpload)FormView1.FindControl("FileUpload2");              //把formview的fileupload放入新的myfile裡
            if (myfile.HasFile)
            {
                //檢查有沒有檔案
                if (myfile.PostedFile.ContentType == "image/pjpeg" | myfile.PostedFile.ContentType == "image/jpeg" | myfile.PostedFile.ContentType == "image/gif" | myfile.PostedFile.ContentType == "image/x-png")
                {                    //因為我只需要圖案而以~再加一個判斷
                    string pathToCheck = mypath + myfile.FileName;
                    string fileName = myfile.FileName;
                    if (System.IO.File.Exists(pathToCheck))
                    {                    //判斷檔案有沒有存在~有的話就改名啦~
                        int my_counter = 2;
                        while (System.IO.File.Exists(pathToCheck))
                        {
                            tempfileName = my_counter.ToString() + "_" + fileName;
                            pathToCheck = mypath + tempfileName;
                            my_counter = my_counter + 1;
                        }
                        fileName = tempfileName;
                    }
                    myfile.SaveAs(mypath + fileName);

                }
                else
                {
                    e.Cancel = true;                      //如果遇到不是圖檔的話~就直接跳出顯示錯誤

                   
                }

            }


            e.NewValues["Image"] = "~/图片/" + myfile.FileName;

        }
        }
    
}
