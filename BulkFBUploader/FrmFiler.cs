using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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
                this.txtFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                targetDirectory = this.txtFolder.Text;
            }
            try
            {
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
                MessageBox.Show(e.Message, "Error");
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
                if (lstFileList.Items.Count <= 1000)
                    lstFileList.Items.Add(lstDirList.Items[i].ToString());
                else
                {
                    MessageBox.Show("Cannot add more than 1000 pictures!", "Error");
                    break;
                }

            }
            lstFileList.Refresh();
            LblTotalFile_Update();
        }

        private void BtnAddAllToList_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstDirList.Items.Count; i++)
            {
                if (lstFileList.Items.Count <= 1000)
                    lstFileList.Items.Add(lstDirList.Items[i].ToString());
                else
                {
                    MessageBox.Show("Cannot add more than 1000 pictures!", "Error");
                    break;
                }
            }
            lstFileList.Refresh();
            LblTotalFile_Update();
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
            if (lstFileList.Items.Count == 0)
                lblTotalFile.Text = "No file";
            else
                lblTotalFile.Text = "Total " + lstFileList.Items.Count.ToString() + " file" + (lstFileList.Items.Count == 1? ".": "s.");
        }

        private void FrmFiler_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.LastDir != "")
            {
                this.txtFolder.Text = Properties.Settings.Default.LastDir;
                GetDirectoryList();
            }
        }
    }
}
