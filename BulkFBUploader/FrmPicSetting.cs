using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace BulkFBUploader
{
    public partial class FrmPicSetting : Form
    {

        public FrmPicSetting()
        {
            InitializeComponent();
        }

        private void FrmPicSetting_Load(object sender, EventArgs e)
        {
            // load default value from local
            chkResize.Checked = GlobalClass.MySetting.ResizePic;

            radioButtonSetLong.Checked = true;
            txtLongSide.Text = GlobalClass.MySetting.LongSize.ToString();
            if (Int32.Parse(txtLongSide.Text) == 0)
            {
                radioButtonSetLong.Checked = false;
                txtLongSide.Text = "0";
            }
            else
                radioButtonSetLong.Checked = true;

            radioButtonSetShort.Checked = false;
            txtShortSide.Text = GlobalClass.MySetting.ShortSide.ToString();
            if (Int32.Parse(txtShortSide.Text) == 0)
            {
                radioButtonSetShort.Checked = false;
                txtShortSide.Text = "0";
            }
            else
                radioButtonSetShort.Checked = true;

            if((Int32.Parse(txtLongSide.Text) == 0) && (Int32.Parse(txtShortSide.Text) == 0))
            {
                // cannot both be 0, setup some default here
                radioButtonSetLong.Checked = true;
                txtLongSide.Text = "1280";
            }

            txtMaxSize.Text = GlobalClass.MySetting.MaxSize.ToString();
            if (txtMaxSize.Text.Trim() == "")
                txtMaxSize.Text = "0";

            chkSameCanvas.Checked = GlobalClass.MySetting.SameCanvas;
            txtCanvasTop.Text = GlobalClass.MySetting.CanvasTop.ToString();
            txtCanvasLeft.Text = GlobalClass.MySetting.CanvasLeft.ToString();
            txtCanvasRight.Text = GlobalClass.MySetting.CanvasRight.ToString();
            txtCanvasBottom.Text = GlobalClass.MySetting.CanvasBottom.ToString();
            if (chkSameCanvas.Checked)
            {
                txtCanvasLeft.Enabled = false;
                txtCanvasLeft.Text = txtCanvasTop.Text;
                txtCanvasRight.Enabled = false;
                txtCanvasRight.Text = txtCanvasTop.Text;
                txtCanvasBottom.Enabled = false;
                txtCanvasBottom.Text = txtCanvasTop.Text;
            }
            else
            {
                txtCanvasLeft.Enabled = true;
                txtCanvasRight.Enabled = true;
                txtCanvasBottom.Enabled = true;
            }
            colorDialog1.Color = Color.FromArgb(GlobalClass.MySetting.CanvasColor);
            pictureBoxColor.BackColor = Color.FromArgb(GlobalClass.MySetting.CanvasColor);
            pictureBoxColor.Refresh();

            txtBrightness.Text = GlobalClass.MySetting.Brightness.ToString();
            txtContrast.Text = GlobalClass.MySetting.Contrast.ToString();
            txtSharp.Text = GlobalClass.MySetting.GaussianSharpen.ToString();
            txtSaturation.Text = GlobalClass.MySetting.Saturation.ToString();

            chkAddOverlayText.Checked = GlobalClass.MySetting.AddOverlayText;
            txtMessageText.Text = GlobalClass.MySetting.MessageText.Replace("\n", Environment.NewLine); ;

            comboTextPosition.BeginUpdate();
            Dictionary<string,string> comboSource1 = new Dictionary<string,string>();
            comboSource1.Add("UL", "Upper Left");
            comboSource1.Add("UM", "Upper Middle");
            comboSource1.Add("UR", "Upper Right");
            comboSource1.Add("ML", "Middle Left");
            comboSource1.Add("MM", "Middle Middle");
            comboSource1.Add("MR", "Middle Right");
            comboSource1.Add("LL", "Lower Left");
            comboSource1.Add("LM", "Lower Middle");
            comboSource1.Add("LR", "Lower Right");
            comboTextPosition.DataSource = new BindingSource(comboSource1, null);
            comboTextPosition.DisplayMember = "Value";
            comboTextPosition.ValueMember = "Key";
            comboTextPosition.EndUpdate();
            comboTextPosition.Refresh();

            comboTextOrientation.BeginUpdate();
            Dictionary<string, string> comboSource2 = new Dictionary<string, string>();
            comboSource2.Add("LR", "Left to Right");
            comboSource2.Add("BT", "Buttom to Top");
            comboSource2.Add("TB", "Top to Buttom");
            comboSource2.Add("RL", "Right to Left");
            comboTextOrientation.DataSource = new BindingSource(comboSource2, null);
            comboTextOrientation.DisplayMember = "Value";
            comboTextOrientation.ValueMember = "Key";
            comboTextOrientation.EndUpdate();
            comboTextOrientation.Enabled = false; // not in use for now
            comboTextOrientation.Refresh();

            // update font label's background color
            lblFont.BackColor = pictureBoxColor.BackColor;
            lblFontSize.BackColor = pictureBoxColor.BackColor;
            lblFontColor.BackColor = pictureBoxColor.BackColor;
            lblFontAttr.BackColor = pictureBoxColor.BackColor;

            // the rest of data init is in the Shown method
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // save setting for this session only
            SaveLocalSetting();
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // exit without save anything
            this.Close();
        }

        private void BtnSaveDefault_Click(object sender, EventArgs e)
        {
            SaveLocalSetting();
            GlobalClass.SaveDefault();
            //GlobalClass.SettingToXml();
        }

        private void BtnFont_Click(object sender, EventArgs e)
        {
            // Show the dialog.
            fontDialog1.ShowColor = true;
            if (lblFont.Text != "")
            {
                // setup dialog default value
                FontStyle fs = new FontStyle();
                fs = FontStyle.Regular;
                if (lblFontAttr.Text.IndexOf("Bold") >= 0)
                    fs = FontStyle.Bold;
                if (lblFontAttr.Text.IndexOf("Italic") >= 0)
                    fs = fs | FontStyle.Italic;
                if (lblFontAttr.Text.IndexOf("Strikeout") >= 0)
                    fs = fs | FontStyle.Strikeout;
                if (lblFontAttr.Text.IndexOf("Underline") >= 0)
                    fs = fs | FontStyle.Underline;
                GraphicsUnit gu = GraphicsUnit.Display; ;
                switch (lblFontUnit.Text.ToUpper())
                {
                    case "DISPLAY":
                        gu = GraphicsUnit.Display;
                        break;
                    case "DOCUMENT":
                        gu = GraphicsUnit.Document;
                        break;
                    case "INCH":
                        gu = GraphicsUnit.Inch;
                        break;
                    case "MILLIMETER":
                        gu = GraphicsUnit.Millimeter;
                        break;
                    case "PIXEL":
                        gu = GraphicsUnit.Pixel;
                        break;
                    case "POINT":
                        gu = GraphicsUnit.Point;
                        break;
                    case "WORLD":
                        gu = GraphicsUnit.World;
                        break;
                }
                byte b = (byte)int.Parse(lblFontCharset.Text);
                Font f = new Font(lblFont.Text, float.Parse(lblFontSize.Text), fs, gu, b);
                fontDialog1.Font = f;
                fontDialog1.Color= Color.FromName(lblFontColor.Text);
            }

            // get font data
            DialogResult result = fontDialog1.ShowDialog();
            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                // Get Font.
                Font font = fontDialog1.Font;
                Color color = fontDialog1.Color;
                
                // Set TextBox properties.
                lblFont.Text = font.Name;
                lblFont.ForeColor = color;
                lblFontSize.Text = Math.Round(font.Size,0,MidpointRounding.AwayFromZero).ToString();
                lblFontSize.ForeColor = color;
                lblFontColor.Text = color.ToKnownColor().ToString();
                lblFontColor.ForeColor = color;
                lblFontAttr.Text = font.Style.ToString();
                lblFontAttr.ForeColor = color;
                lblFontCharset.Text = font.GdiCharSet.ToString();
                lblFontUnit.Text = font.Unit.ToString();
            }
        }

        private void RadioButtonSetLong_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSetLong.Checked)
            {
                txtLongSide.Enabled = true;
            }
            else
            {
                txtLongSide.Enabled = false;
                txtLongSide.Text = "0";
            }

        }

        private void RadioButtonSetShort_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSetShort.Checked)
            {
                txtShortSide.Enabled = true;
            }
            else
            {
                txtShortSide.Enabled = false;
                txtShortSide.Text = "0";
            }
        }

        private void FrmPicSetting_Shown(object sender, EventArgs e)
        {
            // this is the second part of loading data into edit grid
            // the first part is on form load event
            // add items data to listview data source
            lstOverlayText.Items.Clear();
            int i = 0;
            for (i = 0; i < GlobalClass.MaxOverlayText; i++)
            {
                GlobalClass.MyEdit.OLInfos[i].InUse = GlobalClass.MySetting.OLInfos[i].InUse;
                GlobalClass.MyEdit.OLInfos[i].Pos = GlobalClass.MySetting.OLInfos[i].Pos;
                GlobalClass.MyEdit.OLInfos[i].Ori = GlobalClass.MySetting.OLInfos[i].Ori;
                GlobalClass.MyEdit.OLInfos[i].Font = GlobalClass.MySetting.OLInfos[i].Font;
                GlobalClass.MyEdit.OLInfos[i].FontSize = GlobalClass.MySetting.OLInfos[i].FontSize;
                GlobalClass.MyEdit.OLInfos[i].FontAttr = GlobalClass.MySetting.OLInfos[i].FontAttr;
                GlobalClass.MyEdit.OLInfos[i].FontColor = GlobalClass.MySetting.OLInfos[i].FontColor;
                GlobalClass.MyEdit.OLInfos[i].FontCharset = GlobalClass.MySetting.OLInfos[i].FontCharset;
                GlobalClass.MyEdit.OLInfos[i].FontUnit = GlobalClass.MySetting.OLInfos[i].FontUnit;
                GlobalClass.MyEdit.OLInfos[i].Xoffset = GlobalClass.MySetting.OLInfos[i].Xoffset;
                GlobalClass.MyEdit.OLInfos[i].Yoffset = GlobalClass.MySetting.OLInfos[i].Yoffset;
                GlobalClass.MyEdit.OLInfos[i].IsFile = GlobalClass.MySetting.OLInfos[i].IsFile;
                GlobalClass.MyEdit.OLInfos[i].Opacity = GlobalClass.MySetting.OLInfos[i].Opacity;
                GlobalClass.MyEdit.OLInfos[i].Text = GlobalClass.MySetting.OLInfos[i].Text;
                // add listview data source to the listview, only show 5 columes of data
                ListViewItem item = new ListViewItem(i.ToString());
                item.SubItems.Add(GlobalClass.MyEdit.OLInfos[i].InUse ? "Y" : "N");
                item.SubItems.Add(GlobalClass.MyEdit.OLInfos[i].Pos);
                item.SubItems.Add(GlobalClass.MyEdit.OLInfos[i].Ori);
                item.SubItems.Add(GlobalClass.MyEdit.OLInfos[i].Text);
                lstOverlayText.Items.Add(item);
            }

            // select the first item, index=0
            lstOverlayText.Items[0].Selected = true;

            // setup combo default
            comboTextPosition.SelectedValue = GlobalClass.MyEdit.OLInfos[0].Pos;
            comboTextOrientation.SelectedValue = GlobalClass.MyEdit.OLInfos[0].Ori;
        }

        // save input panel to local storage
        private void SaveLocalSetting()
        {
            // same picture setting to this session only
            GlobalClass.MySetting.ResizePic = chkResize.Checked;
            GlobalClass.MySetting.LongSize = Int32.Parse(txtLongSide.Text);
            GlobalClass.MySetting.ShortSide = Int32.Parse(txtShortSide.Text);
            GlobalClass.MySetting.MaxSize = Int32.Parse(txtMaxSize.Text);
            GlobalClass.MySetting.SameCanvas = chkSameCanvas.Checked;
            GlobalClass.MySetting.CanvasTop = Int32.Parse(txtCanvasTop.Text);
            GlobalClass.MySetting.CanvasLeft = Int32.Parse(txtCanvasLeft.Text);
            GlobalClass.MySetting.CanvasRight = Int32.Parse(txtCanvasRight.Text);
            GlobalClass.MySetting.CanvasBottom = Int32.Parse(txtCanvasBottom.Text);
            GlobalClass.MySetting.CanvasColor = pictureBoxColor.BackColor.ToArgb();
            GlobalClass.MySetting.Brightness = int.Parse(txtBrightness.Text);
            GlobalClass.MySetting.Contrast = int.Parse(txtContrast.Text);
            GlobalClass.MySetting.GaussianSharpen = int.Parse(txtSharp.Text);
            GlobalClass.MySetting.Saturation = int.Parse(txtSaturation.Text);
            GlobalClass.MySetting.AddOverlayText = chkAddOverlayText.Checked;
            GlobalClass.MySetting.MessageText = txtMessageText.Text;
            for (int i = 0; i < GlobalClass.MaxOverlayText; i++)
            {
                GlobalClass.MySetting.OLInfos[i].InUse = GlobalClass.MyEdit.OLInfos[i].InUse;
                GlobalClass.MySetting.OLInfos[i].Pos = GlobalClass.MyEdit.OLInfos[i].Pos;
                GlobalClass.MySetting.OLInfos[i].Ori = GlobalClass.MyEdit.OLInfos[i].Ori;
                GlobalClass.MySetting.OLInfos[i].Font = GlobalClass.MyEdit.OLInfos[i].Font;
                GlobalClass.MySetting.OLInfos[i].FontSize = GlobalClass.MyEdit.OLInfos[i].FontSize;
                GlobalClass.MySetting.OLInfos[i].FontAttr = GlobalClass.MyEdit.OLInfos[i].FontAttr;
                GlobalClass.MySetting.OLInfos[i].FontColor = GlobalClass.MyEdit.OLInfos[i].FontColor;
                GlobalClass.MySetting.OLInfos[i].FontCharset = GlobalClass.MyEdit.OLInfos[i].FontCharset;
                GlobalClass.MySetting.OLInfos[i].FontUnit = GlobalClass.MyEdit.OLInfos[i].FontUnit;
                GlobalClass.MySetting.OLInfos[i].Xoffset = GlobalClass.MyEdit.OLInfos[i].Xoffset;
                GlobalClass.MySetting.OLInfos[i].Yoffset = GlobalClass.MyEdit.OLInfos[i].Yoffset;
                GlobalClass.MySetting.OLInfos[i].IsFile = GlobalClass.MyEdit.OLInfos[i].IsFile;
                GlobalClass.MySetting.OLInfos[i].Opacity = GlobalClass.MyEdit.OLInfos[i].Opacity;
                GlobalClass.MySetting.OLInfos[i].Text = GlobalClass.MyEdit.OLInfos[i].Text;
            }
        }

        private void ChkSameCanvas_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSameCanvas.Checked)
            {
                txtCanvasTop.Enabled = true;
                txtCanvasLeft.Text = txtCanvasTop.Text;
                txtCanvasLeft.Enabled = false;
                txtCanvasRight.Text = txtCanvasTop.Text;
                txtCanvasRight.Enabled = false;
                txtCanvasBottom.Text = txtCanvasTop.Text;
                txtCanvasBottom.Enabled = false;
            }
            else
            {
                txtCanvasTop.Enabled = true;
                txtCanvasLeft.Enabled = true;
                txtCanvasRight.Enabled = true;
                txtCanvasBottom.Enabled = true;
            }
        }

        private void TxtCanvasTop_Leave(object sender, EventArgs e)
        {
            if (chkSameCanvas.Checked)
            {
                txtCanvasLeft.Text = txtCanvasTop.Text;
                txtCanvasRight.Text = txtCanvasTop.Text;
                txtCanvasBottom.Text = txtCanvasTop.Text;
            }
        }

        private void BtnCanvasColor_Click(object sender, EventArgs e)
        {
            // select canvas color
            DialogResult result = colorDialog1.ShowDialog();
            if (result==DialogResult.OK)
            {
                pictureBoxColor.BackColor = colorDialog1.Color;
            }
        }

        private void LstOverlayText_SelectedIndexChanged(object sender, EventArgs e)
        {
            // bring selected item to edit pad, forfeit changes
            if (lstOverlayText.SelectedItems.Count > 0)
            {
                // cencel changes
                BtnCancelOL_Click(sender, e);
            }
        }

        private void BtnCancelOL_Click(object sender, EventArgs e)
        {
            // cancel change, bring selected item to edit pad again
            int i = lstOverlayText.SelectedItems[0].Index;

            chkOverlayInUse.Checked = GlobalClass.MyEdit.OLInfos[i].InUse;
            comboTextPosition.SelectedValue = GlobalClass.MyEdit.OLInfos[i].Pos;
            comboTextOrientation.SelectedValue = GlobalClass.MyEdit.OLInfos[i].Ori;
            lblFont.Text = GlobalClass.MyEdit.OLInfos[i].Font;
            lblFont.ForeColor = Color.FromName(GlobalClass.MyEdit.OLInfos[i].FontColor);
            lblFontSize.Text = GlobalClass.MyEdit.OLInfos[i].FontSize;
            lblFontSize.ForeColor = Color.FromName(GlobalClass.MyEdit.OLInfos[i].FontColor);
            lblFontAttr.Text = GlobalClass.MyEdit.OLInfos[i].FontAttr;
            lblFontAttr.ForeColor = Color.FromName(GlobalClass.MyEdit.OLInfos[i].FontColor);
            lblFontColor.Text = GlobalClass.MyEdit.OLInfos[i].FontColor;
            lblFontColor.ForeColor = Color.FromName(GlobalClass.MyEdit.OLInfos[i].FontColor);
            lblFontCharset.Text = GlobalClass.MyEdit.OLInfos[i].FontCharset; // hidden label
            lblFontUnit.Text = GlobalClass.MyEdit.OLInfos[i].FontUnit; // hidden label
            txtXoffset.Text = GlobalClass.MyEdit.OLInfos[i].Xoffset.ToString();
            txtYoffset.Text = GlobalClass.MyEdit.OLInfos[i].Yoffset.ToString();
            chkIsFile.Checked = GlobalClass.MyEdit.OLInfos[i].IsFile;
            txtOpacity.Text = GlobalClass.MyEdit.OLInfos[i].Opacity.ToString();
            txtOverlayText.Text = GlobalClass.MyEdit.OLInfos[i].Text.Replace("\n", Environment.NewLine);
        }

        private void BtnSaveOL_Click(object sender, EventArgs e)
        {
            // save selected listview items to local setting
            int i = lstOverlayText.SelectedItems[0].Index; // only 1 selected item only

            GlobalClass.MyEdit.OLInfos[i].InUse = chkOverlayInUse.Checked;
            GlobalClass.MyEdit.OLInfos[i].Pos = comboTextPosition.SelectedValue.ToString();
            GlobalClass.MyEdit.OLInfos[i].Ori = comboTextOrientation.SelectedValue.ToString();
            GlobalClass.MyEdit.OLInfos[i].Font = lblFont.Text;
            GlobalClass.MyEdit.OLInfos[i].FontSize = lblFontSize.Text;
            GlobalClass.MyEdit.OLInfos[i].FontAttr = lblFontAttr.Text;
            GlobalClass.MyEdit.OLInfos[i].FontColor = lblFontColor.Text;
            GlobalClass.MyEdit.OLInfos[i].FontCharset = lblFontCharset.Text;
            GlobalClass.MyEdit.OLInfos[i].FontUnit = lblFontUnit.Text;
            GlobalClass.MyEdit.OLInfos[i].Xoffset = Int16.Parse(txtXoffset.Text);
            GlobalClass.MyEdit.OLInfos[i].Yoffset = Int16.Parse(txtYoffset.Text);
            GlobalClass.MyEdit.OLInfos[i].IsFile = chkIsFile.Checked;
            GlobalClass.MyEdit.OLInfos[i].Opacity = Int16.Parse(txtOpacity.Text);
            GlobalClass.MyEdit.OLInfos[i].Text = txtOverlayText.Text;

            // update listview
            lstOverlayText.Items[i].SubItems[1].Text = (chkOverlayInUse.Checked ? "Y" : "N");
            lstOverlayText.Items[i].SubItems[2].Text = GlobalClass.MyEdit.OLInfos[i].Pos;
            lstOverlayText.Items[i].SubItems[3].Text = GlobalClass.MyEdit.OLInfos[i].Ori;
            lstOverlayText.Items[i].SubItems[4].Text = GlobalClass.MyEdit.OLInfos[i].Text;
            lstOverlayText.Refresh();
        }

        private void PictureBoxColor_Click(object sender, EventArgs e)
        {
            // select canvas color
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBoxColor.BackColor = colorDialog1.Color;
                lblFont.BackColor= colorDialog1.Color;
                lblFontSize.BackColor= colorDialog1.Color;
                lblFontColor.BackColor = colorDialog1.Color;
                lblFontAttr.BackColor= colorDialog1.Color; 
            }
        }

        private void TxtOpacity_Leave(object sender, EventArgs e)
        {
            int i = 0;
            float f = 0;

            try
            {
                f = float.Parse(txtOpacity.Text);
                i = (int)Math.Round(f, 0, MidpointRounding.AwayFromZero);
                if (i < 0)
                {
                    i = 0;
                    txtOpacity.Text = "0";
                }
                else if (i > 100)
                {
                    i = 100;
                    txtOpacity.Text = "100";
                }
                else
                    txtOpacity.Text = i.ToString("D"); // decimal format
            }
            catch (Exception err)
            {
                txtOpacity.Text = "0";
                Console.WriteLine("FrmPicSetting.TxtOpacity_Leave error:" + err.Message);

            }
        }

        private void TxtBrightness_Leave(object sender, EventArgs e)
        {
            int i = 0;
            float f = 0;

            try
            {
                f = float.Parse(txtBrightness.Text);
                i = (int)Math.Round(f, 0, MidpointRounding.AwayFromZero);
                if (i < -100)
                {
                    i = -100;
                    txtBrightness.Text = "-100";
                }
                else if (i > 100)
                {
                    i = 100;
                    txtBrightness.Text = "100";
                }
                else
                    txtBrightness.Text = i.ToString("D"); // decimal format
            }
            catch (Exception err)
            {
                txtBrightness.Text = "0";
                Console.WriteLine("FrmPicSetting.TxtBrightness_Leave error:" + err.Message);
            }
        }

        private void TxtContrast_Leave(object sender, EventArgs e)
        {
            int i = 0;
            float f = 0;

            try
            {
                f = float.Parse(txtContrast.Text);
                i = (int)Math.Round(f, 0, MidpointRounding.AwayFromZero);
                if (i < -100)
                {
                    i = -100;
                    txtContrast.Text = "-100";
                }
                else if (i > 100)
                {
                    i = 100;
                    txtContrast.Text = "100";
                }
                else
                    txtContrast.Text = i.ToString("D"); // decimal format
            }
            catch (Exception err)
            {
                txtContrast.Text = "0";
                Console.WriteLine("FrmPicSetting.TxtContrast_Leave error:" + err.Message);
            }
        }

        private void TxtSharp_Leave(object sender, EventArgs e)
        {
            int i = 0;
            float f = 0;

            try
            {
                f = float.Parse(txtSharp.Text);
                i = (int)Math.Round(f, 0, MidpointRounding.AwayFromZero);
                if (i < 0)
                {
                    i = 0;
                    txtSharp.Text = "0";
                }
                else if (i > 30)
                {
                    i = 30;
                    txtSharp.Text = "30";
                }
                else
                    txtSharp.Text = i.ToString("D"); // decimal format
            }
            catch (Exception err)
            {
                txtSharp.Text = "0";
                Console.WriteLine("FrmPicSetting.TxtSharp_Leave error:" + err.Message);
            }
        }

        private void TxtSaturation_Leave(object sender, EventArgs e)
        {
            int i = 0;
            float f = 0;

            try
            {
                f = float.Parse(txtSaturation.Text);
                i = (int)Math.Round(f, 0, MidpointRounding.AwayFromZero);
                if (i < -100)
                {
                    i = -100;
                    txtSaturation.Text = "-100";
                }
                else if (i > 100)
                {
                    i = 100;
                    txtSaturation.Text = "100";
                }
                else
                    txtSaturation.Text = i.ToString("D"); // decimal format
            }
            catch (Exception err)
            {
                txtSaturation.Text = "0";
                Console.WriteLine("FrmPicSetting.TxtSaturation_Leave error:" + err.Message);
            }
        }

        private void ChkIsFile_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsFile.Checked)
            {
                btnSelectImageFile.Visible = true;
                txtOverlayText.Enabled = false;
            }
            else
            {
                btnSelectImageFile.Visible = false;
                txtOverlayText.Enabled = true;
            }
        }

        private void BtnSelectImageFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOverlayText.Text = openFileDialog1.FileName;
            }
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
}
