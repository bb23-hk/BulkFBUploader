namespace BulkFBUploader
{
    partial class FrmFiler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFiler));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lstDirList = new System.Windows.Forms.ListBox();
            this.btnAddToList = new System.Windows.Forms.Button();
            this.btnAddAllToList = new System.Windows.Forms.Button();
            this.btnClearList = new System.Windows.Forms.Button();
            this.btnRemoveFromList = new System.Windows.Forms.Button();
            this.lstFileList = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            resources.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
            // 
            // btnSelectFolder
            // 
            resources.ApplyResources(this.btnSelectFolder, "btnSelectFolder");
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.BtnSelectFolder_Click);
            // 
            // txtFolder
            // 
            resources.ApplyResources(this.txtFolder, "txtFolder");
            this.txtFolder.Name = "txtFolder";
            // 
            // lstDirList
            // 
            resources.ApplyResources(this.lstDirList, "lstDirList");
            this.lstDirList.FormattingEnabled = true;
            this.lstDirList.Name = "lstDirList";
            this.lstDirList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // btnAddToList
            // 
            resources.ApplyResources(this.btnAddToList, "btnAddToList");
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.UseVisualStyleBackColor = true;
            this.btnAddToList.Click += new System.EventHandler(this.BtnAddToList_Click);
            // 
            // btnAddAllToList
            // 
            resources.ApplyResources(this.btnAddAllToList, "btnAddAllToList");
            this.btnAddAllToList.Name = "btnAddAllToList";
            this.btnAddAllToList.UseVisualStyleBackColor = true;
            this.btnAddAllToList.Click += new System.EventHandler(this.BtnAddAllToList_Click);
            // 
            // btnClearList
            // 
            resources.ApplyResources(this.btnClearList, "btnClearList");
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.UseVisualStyleBackColor = true;
            this.btnClearList.Click += new System.EventHandler(this.BtnClearList_Click);
            // 
            // btnRemoveFromList
            // 
            resources.ApplyResources(this.btnRemoveFromList, "btnRemoveFromList");
            this.btnRemoveFromList.Name = "btnRemoveFromList";
            this.btnRemoveFromList.UseVisualStyleBackColor = true;
            this.btnRemoveFromList.Click += new System.EventHandler(this.BtnRemoveFromList_Click);
            // 
            // lstFileList
            // 
            resources.ApplyResources(this.lstFileList, "lstFileList");
            this.lstFileList.FormattingEnabled = true;
            this.lstFileList.Name = "lstFileList";
            this.lstFileList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblTotalFile
            // 
            resources.ApplyResources(this.lblTotalFile, "lblTotalFile");
            this.lblTotalFile.Name = "lblTotalFile";
            // 
            // FrmFiler
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTotalFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lstFileList);
            this.Controls.Add(this.btnRemoveFromList);
            this.Controls.Add(this.btnClearList);
            this.Controls.Add(this.btnAddAllToList);
            this.Controls.Add(this.btnAddToList);
            this.Controls.Add(this.lstDirList);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.btnSelectFolder);
            this.Name = "FrmFiler";
            this.Load += new System.EventHandler(this.FrmFiler_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.Button btnAddAllToList;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.Button btnRemoveFromList;
        private System.Windows.Forms.ListBox lstFileList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListBox lstDirList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalFile;
    }
}