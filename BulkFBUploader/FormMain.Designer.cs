using System.Windows.Forms;

namespace BulkFBUploader
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.btnFBLogin = new System.Windows.Forms.Button();
            this.btnFBLogout = new System.Windows.Forms.Button();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblAlbumName = new System.Windows.Forms.Label();
            this.txtAlbumName = new System.Windows.Forms.TextBox();
            this.lblAlbumDesc = new System.Windows.Forms.Label();
            this.txtAlbumDesc = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblImageFile = new System.Windows.Forms.Label();
            this.chkBoxNewAlbum = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.listBoxAlbum = new System.Windows.Forms.ListBox();
            this.chkBoxUploadToPage = new System.Windows.Forms.CheckBox();
            this.listBoxPageList = new System.Windows.Forms.ListBox();
            this.btnSelectFiles = new System.Windows.Forms.Button();
            this.lblTotalFiles = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUploadSetting = new System.Windows.Forms.Button();
            this.pboxLanguage = new System.Windows.Forms.PictureBox();
            this.btnPicSetting = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.pBoxUserInfo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pboxLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxUserInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnFBLogin
            // 
            resources.ApplyResources(this.btnFBLogin, "btnFBLogin");
            this.btnFBLogin.Name = "btnFBLogin";
            this.btnFBLogin.UseVisualStyleBackColor = true;
            this.btnFBLogin.Click += new System.EventHandler(this.BtnFBLogin_Click);
            // 
            // btnFBLogout
            // 
            resources.ApplyResources(this.btnFBLogout, "btnFBLogout");
            this.btnFBLogout.Name = "btnFBLogout";
            this.btnFBLogout.UseVisualStyleBackColor = true;
            this.btnFBLogout.Click += new System.EventHandler(this.BtnFBLogout_Click);
            // 
            // lblUserInfo
            // 
            resources.ApplyResources(this.lblUserInfo, "lblUserInfo");
            this.lblUserInfo.Name = "lblUserInfo";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnUpload
            // 
            resources.ApplyResources(this.btnUpload, "btnUpload");
            this.btnUpload.Image = global::BulkFBUploader.Properties.Resources.fbupload;
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // lblAlbumName
            // 
            resources.ApplyResources(this.lblAlbumName, "lblAlbumName");
            this.lblAlbumName.Name = "lblAlbumName";
            // 
            // txtAlbumName
            // 
            resources.ApplyResources(this.txtAlbumName, "txtAlbumName");
            this.txtAlbumName.Name = "txtAlbumName";
            // 
            // lblAlbumDesc
            // 
            resources.ApplyResources(this.lblAlbumDesc, "lblAlbumDesc");
            this.lblAlbumDesc.Name = "lblAlbumDesc";
            // 
            // txtAlbumDesc
            // 
            this.txtAlbumDesc.AcceptsReturn = true;
            resources.ApplyResources(this.txtAlbumDesc, "txtAlbumDesc");
            this.txtAlbumDesc.Name = "txtAlbumDesc";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // lblImageFile
            // 
            resources.ApplyResources(this.lblImageFile, "lblImageFile");
            this.lblImageFile.Name = "lblImageFile";
            // 
            // chkBoxNewAlbum
            // 
            resources.ApplyResources(this.chkBoxNewAlbum, "chkBoxNewAlbum");
            this.chkBoxNewAlbum.Checked = true;
            this.chkBoxNewAlbum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxNewAlbum.Name = "chkBoxNewAlbum";
            this.chkBoxNewAlbum.UseVisualStyleBackColor = true;
            this.chkBoxNewAlbum.CheckStateChanged += new System.EventHandler(this.ChkBoxNewAlbum_CheckStateChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // listBoxAlbum
            // 
            this.listBoxAlbum.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxAlbum, "listBoxAlbum");
            this.listBoxAlbum.Name = "listBoxAlbum";
            // 
            // chkBoxUploadToPage
            // 
            resources.ApplyResources(this.chkBoxUploadToPage, "chkBoxUploadToPage");
            this.chkBoxUploadToPage.Name = "chkBoxUploadToPage";
            this.chkBoxUploadToPage.UseVisualStyleBackColor = true;
            this.chkBoxUploadToPage.CheckedChanged += new System.EventHandler(this.ChkBoxUploadToPage_CheckStateChanged);
            // 
            // listBoxPageList
            // 
            this.listBoxPageList.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxPageList, "listBoxPageList");
            this.listBoxPageList.Name = "listBoxPageList";
            this.listBoxPageList.SelectedIndexChanged += new System.EventHandler(this.ListBoxPageList_SelectedIndexChanged);
            // 
            // btnSelectFiles
            // 
            resources.ApplyResources(this.btnSelectFiles, "btnSelectFiles");
            this.btnSelectFiles.Name = "btnSelectFiles";
            this.btnSelectFiles.UseVisualStyleBackColor = true;
            this.btnSelectFiles.Click += new System.EventHandler(this.BtnSelectFiles_Click);
            // 
            // lblTotalFiles
            // 
            resources.ApplyResources(this.lblTotalFiles, "lblTotalFiles");
            this.lblTotalFiles.Name = "lblTotalFiles";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnUploadSetting
            // 
            this.btnUploadSetting.Image = global::BulkFBUploader.Properties.Resources.ulsetting;
            resources.ApplyResources(this.btnUploadSetting, "btnUploadSetting");
            this.btnUploadSetting.Name = "btnUploadSetting";
            this.btnUploadSetting.UseVisualStyleBackColor = true;
            this.btnUploadSetting.Click += new System.EventHandler(this.BtnUploadSetting_Click);
            // 
            // pboxLanguage
            // 
            this.pboxLanguage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.pboxLanguage, "pboxLanguage");
            this.pboxLanguage.Name = "pboxLanguage";
            this.pboxLanguage.TabStop = false;
            this.pboxLanguage.Click += new System.EventHandler(this.PboxLanguage_Click);
            // 
            // btnPicSetting
            // 
            resources.ApplyResources(this.btnPicSetting, "btnPicSetting");
            this.btnPicSetting.Name = "btnPicSetting";
            this.btnPicSetting.UseVisualStyleBackColor = true;
            this.btnPicSetting.Click += new System.EventHandler(this.BtnPicSetting_Click);
            // 
            // BtnExit
            // 
            resources.ApplyResources(this.BtnExit, "BtnExit");
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // pBoxUserInfo
            // 
            resources.ApplyResources(this.pBoxUserInfo, "pBoxUserInfo");
            this.pBoxUserInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBoxUserInfo.Image = global::BulkFBUploader.Properties.Resources.user;
            this.pBoxUserInfo.Name = "pBoxUserInfo";
            this.pBoxUserInfo.TabStop = false;
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnUploadSetting);
            this.Controls.Add(this.pboxLanguage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotalFiles);
            this.Controls.Add(this.btnSelectFiles);
            this.Controls.Add(this.btnPicSetting);
            this.Controls.Add(this.listBoxPageList);
            this.Controls.Add(this.chkBoxUploadToPage);
            this.Controls.Add(this.listBoxAlbum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkBoxNewAlbum);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.lblImageFile);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtAlbumDesc);
            this.Controls.Add(this.lblAlbumDesc);
            this.Controls.Add(this.txtAlbumName);
            this.Controls.Add(this.lblAlbumName);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pBoxUserInfo);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.btnFBLogout);
            this.Controls.Add(this.btnFBLogin);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            ((System.ComponentModel.ISupportInitialize)(this.pboxLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxUserInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Label label1;
        private Button btnFBLogin;
        private Button btnFBLogout;
        private Label lblUserInfo;
        private PictureBox pBoxUserInfo;
        private Label label2;
        private Button btnUpload;
        private Label lblAlbumName;
        private TextBox txtAlbumName;
        private Label lblAlbumDesc;
        private TextBox txtAlbumDesc;
        private ProgressBar progressBar1;
        private Label lblImageFile;
        private Button BtnExit;
        private CheckBox chkBoxNewAlbum;
        private Label label5;
        private ListBox listBoxAlbum;
        private CheckBox chkBoxUploadToPage;
        private ListBox listBoxPageList;
        private Button btnPicSetting;
        private Button btnSelectFiles;
        private Label lblTotalFiles;
        private Label label3;
        private PictureBox pboxLanguage;
        private Button btnUploadSetting;
    }
}

