using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulkFBUploader
{
    public partial class FrmUploadOption : Form
    {
        public FrmUploadOption()
        {
            InitializeComponent();
        }

        private void FrmUploadOption_Load(object sender, EventArgs e)
        {
            // initial value from global
            chkSaveFile.Checked = GlobalClass.SaveExportJPG;
            btnSelectSavePath.Enabled = chkSaveFile.Checked;
            lblFilePath.Text = GlobalClass.ExportJPGPath;
            chkUploadToFB.Checked = GlobalClass.UploadToFB;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSaveFile_CheckedChanged(object sender, EventArgs e)
        {
            GlobalClass.SaveExportJPG = chkSaveFile.Checked;
            btnSelectSavePath.Enabled = chkSaveFile.Checked;
        }

        private void BtnSelectSavePath_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalClass.ExportJPGPath != "")
                    folderBrowserDialog1.SelectedPath = GlobalClass.ExportJPGPath;
                folderBrowserDialog1.ShowDialog();
                GlobalClass.ExportJPGPath = folderBrowserDialog1.SelectedPath;
                lblFilePath.Text = GlobalClass.ExportJPGPath;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }

        private void ChkUploadToFB_CheckedChanged(object sender, EventArgs e)
        {
            GlobalClass.UploadToFB = chkUploadToFB.Checked;
        }
    }
}
