using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace BulkFBUploader
{
    public partial class FrmFiler : Form
    {
        public FrmFiler()
        {
            InitializeComponent();
            lstFileList.Items.AddRange(GlobalClass.FileList.ToArray());
            LblTotalFile_Update();
        }

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            if (this.txtFolder.Text != "")
                folderBrowserDialog1.SelectedPath = txtFolder.Text;
            folderBrowserDialog1.ShowDialog();
            this.txtFolder.Text = folderBrowserDialog1.SelectedPath;
            GetDirectoryList();
            Properties.Settings.Default.LastDir = this.txtFolder.Text;
            Properties.Settings.Default.Save();
        }


        private void GetDirectoryList()
        {
            string targetDirectory = this.txtFolder.Text;
            if (!Directory.Exists(targetDirectory))
            {
                // default folder not exist
                // user MyPictures folder
                this.txtFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                targetDirectory = this.txtFolder.Text;
            }
            try
            {
                // only support JPG file type
                string[] fileEntries = Directory.GetFiles(targetDirectory, "*.JPG");

                if (fileEntries != null)
                {
                    lstDirList.Items.Clear();
                    lstDirList.Items.AddRange(fileEntries);
                    lstDirList.Refresh();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading directory: " + this.txtFolder.Text + "\n" + e.Message, "Error");
                Application.Exit();
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            GlobalClass.FileList.Clear();
            foreach (string s in lstFileList.Items)
                GlobalClass.FileList.Add(s);
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAddToList_Click(object sender, EventArgs e)
        {
            foreach (int i in lstDirList.SelectedIndices)
            {
                if (!AddToList(lstDirList.Items[i].ToString()))
                    break; // over list limit
            }
            lstFileList.Refresh();
            LblTotalFile_Update();
        }

        private void BtnAddAllToList_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstDirList.Items.Count; i++)
            {
                if (!AddToList(lstDirList.Items[i].ToString()))
                    break; // over list limit
            }
            lstFileList.Refresh();
            LblTotalFile_Update();
        }

        private bool AddToList(string s)
        {
            // add s to lstFileList
            if (lstFileList.Items.Count < GlobalClass.MaxFileList)
            {
                // check if s exist in list already
                int idx = lstFileList.FindStringExact(s);
                if (idx < 0)
                    // not match find, otherwise, don't add the item
                    lstFileList.Items.Add(s);
                return true;
            }
            else
            {
                MessageBox.Show("Cannot add more than " + GlobalClass.MaxFileList.ToString() + " pictures!", "Error");
                return false;
            }
        }

        private void BtnClearList_Click(object sender, EventArgs e)
        {
            lstFileList.Items.Clear();
            GlobalClass.FileList.Clear();
            lstFileList.Refresh();
            LblTotalFile_Update();
        }

        private void BtnRemoveFromList_Click(object sender, EventArgs e)
        {
            List<string> sList = new List<string>();

            // get remove list
            foreach (int i in lstFileList.SelectedIndices)
                sList.Add(lstFileList.Items[i].ToString());
            // remove item from the list
            foreach (string s in sList)
                lstFileList.Items.Remove(s);
            lstFileList.Refresh();
            LblTotalFile_Update();
        }

        // update lblTotalFile.Text to number of items in lstFileList
        private void LblTotalFile_Update()
        {
            string msg1 = ""; // no file in the list
            string msg2 = ""; // num of file in the list

            if (Thread.CurrentThread.CurrentUICulture.ToString().ToUpper() == "EN")
            {
                msg1 = "No file";
                msg2 = "Total " + lstFileList.Items.Count.ToString() + " file" + (lstFileList.Items.Count == 1 ? "." : "s.");
            }
            else
            {
                msg1 = "沒有選擇檔案";
                msg2 = string.Format("共有{0}個檔案", lstFileList.Items.Count.ToString());
            }

            if (lstFileList.Items.Count == 0)
                lblTotalFile.Text = msg1;
            else
                lblTotalFile.Text = msg2;
        }

        private void FrmFiler_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.LastDir != "")
            {
                this.txtFolder.Text = Properties.Settings.Default.LastDir;
                GetDirectoryList();
            }
        }

        private void lstFileList_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.lstFileList.SelectedItem == null)
                return;
            this.lstFileList.DoDragDrop(this.lstFileList.SelectedItem, DragDropEffects.Move);
        }

        private void lstFileList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lstFileList_DragDrop(object sender, DragEventArgs e)
        {
            Point point = lstFileList.PointToClient(new Point(e.X, e.Y));
            int index = this.lstFileList.IndexFromPoint(point);
            if (index < 0) index = this.lstFileList.Items.Count - 1;
            object data = lstFileList.SelectedItem;
            this.lstFileList.Items.Remove(data);
            this.lstFileList.Items.Insert(index, data);
        }

    }
}
