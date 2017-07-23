namespace BulkFBUploader
{
    partial class FrmUploadOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUploadOption));
            this.chkSaveFile = new System.Windows.Forms.CheckBox();
            this.btnSelectSavePath = new System.Windows.Forms.Button();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.chkUploadToFB = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // chkSaveFile
            // 
            resources.ApplyResources(this.chkSaveFile, "chkSaveFile");
            this.chkSaveFile.Name = "chkSaveFile";
            this.chkSaveFile.UseVisualStyleBackColor = true;
            this.chkSaveFile.CheckedChanged += new System.EventHandler(this.chkSaveFile_CheckedChanged);
            // 
            // btnSelectSavePath
            // 
            resources.ApplyResources(this.btnSelectSavePath, "btnSelectSavePath");
            this.btnSelectSavePath.Name = "btnSelectSavePath";
            this.btnSelectSavePath.UseVisualStyleBackColor = true;
            this.btnSelectSavePath.Click += new System.EventHandler(this.BtnSelectSavePath_Click);
            // 
            // lblFilePath
            // 
            resources.ApplyResources(this.lblFilePath, "lblFilePath");
            this.lblFilePath.Name = "lblFilePath";
            // 
            // chkUploadToFB
            // 
            resources.ApplyResources(this.chkUploadToFB, "chkUploadToFB");
            this.chkUploadToFB.Checked = true;
            this.chkUploadToFB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUploadToFB.Name = "chkUploadToFB";
            this.chkUploadToFB.UseVisualStyleBackColor = true;
            this.chkUploadToFB.CheckedChanged += new System.EventHandler(this.ChkUploadToFB_CheckedChanged);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // folderBrowserDialog1
            // 
            resources.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
            // 
            // FrmUploadOption
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkUploadToFB);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.btnSelectSavePath);
            this.Controls.Add(this.chkSaveFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmUploadOption";
            this.Load += new System.EventHandler(this.FrmUploadOption_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSaveFile;
        private System.Windows.Forms.Button btnSelectSavePath;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.CheckBox chkUploadToFB;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}