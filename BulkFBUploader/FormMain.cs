using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;
using System.Globalization;
using System.Threading;
using System.Drawing;
using System.Collections.Concurrent;
using System.Drawing.Imaging;
using Facebook;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using System.Linq;

namespace BulkFBUploader
{
    public partial class frmMain : Form
    {


        public frmMain()
        {
            string langStr = Properties.Settings.Default.Language;
            if ((langStr.ToUpper()!="ZH-HANT") && (langStr.ToUpper()!="EN"))
            {
                // no or wrong default
                langStr = "en";
                Properties.Settings.Default.Language = langStr;
                Properties.Settings.Default.Save();
            }
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langStr);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(langStr);

            InitializeComponent();
            
            GlobalClass.CreateMyDocPath();

            GlobalClass.MySetting = new PicSettingClass();
            GlobalClass.MyEdit = new PicSettingClass();
            GlobalClass.FileList = new List<string>();

            lblUserInfo.Text = "";
            lblImageFile.Text = "";
            progressBar1.Visible = false;
            chkBoxUploadToPage.Checked = false;
            listBoxPageList.Visible = false;
            listBoxAlbum.Visible = false;
            lblTotalFiles.Text = "";

            GlobalClass.FB = new FacebookClient();

            GlobalClass.GetSetting();
            //FromXml(GlobalClass.MyDocPathXML + "\\" + GlobalClass.XMLFile);
            XML_Filer.WriteToXmlFile(GlobalClass.MyDocPathXML + "\\" + GlobalClass.XMLFile, GlobalClass.MySetting, false);

            this.Text = this.Text + " " + Application.ProductVersion;

            Bitmap bmp;
            if (Thread.CurrentThread.CurrentUICulture.ToString().ToUpper() == "EN")
                bmp = new Bitmap(Properties.Resources.ZH);
            else
                bmp = new Bitmap(Properties.Resources.en);
            pboxLanguage.Image = bmp;
        }


        private void BtnFBLogin_Click(object sender, EventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            var result = frm.ShowDialog();
            if (result == DialogResult.OK)
            {
                btnFBLogin.Enabled = false;
                btnFBLogout.Enabled = true;
                btnUpload.Enabled = false;
                chkBoxUploadToPage.Checked = false;
                listBoxPageList.Visible = false;
                listBoxAlbum.Visible = false;
                GetUserInfo();
            }
            else
            {
                btnFBLogin.Enabled = true;
                btnFBLogout.Enabled = false;
                btnUpload.Enabled = false;
                chkBoxUploadToPage.Checked = false;
                listBoxPageList.Visible = false;
                listBoxAlbum.Visible = false;
            }

            GlobalClass.FB = new FacebookClient(GlobalClass.FBAccessToken);
            dynamic gresult = GlobalClass.FB.Get("me/groups");
        }


        private void BtnFBLogout_Click(object sender, EventArgs e)
        {
            var logoutUrl = GlobalClass.FB.GetLogoutUrl(new { access_token = GlobalClass.FBAccessToken, next = "https://www.facebook.com/connect/login_success.html" });

            try
            {
                WebBrowser wb = new WebBrowser();
                wb.Navigate(logoutUrl);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            Application.Exit();
        }

        private void GetUserInfo()
        {
            GlobalClass.FB = new FacebookClient(GlobalClass.FBAccessToken);

            dynamic me = GlobalClass.FB.Get("me");

            string me_name = me.name;
            string me_id = me.id;

            lblUserInfo.Text = me_name + ",(" + me_id + ")";

            string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", "me", "large", GlobalClass.FBAccessToken);
            pBoxUserInfo.Load(profilePictureUrl);

            this.Refresh();
        }


        private void ChkBoxUploadToPage_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBoxUploadToPage.Checked)
            {
                // display listBoxPageList
                listBoxPageList.Visible = true;
                GetUserPage();
            }
            else
            {
                listBoxPageList.Items.Clear();
                listBoxPageList.Visible = false;
            }
        }


        private void GetUserPage()
        {
            GlobalClass.FB = new FacebookClient(GlobalClass.FBAccessToken);

            string pageAfter = "";

            do
            {
                dynamic pages;
                if (pageAfter == "")
                {
                    // first page
                    pages = GlobalClass.FB.Get("me/accounts");
                }
                else
                {
                    // more pages
                    var pageParameters = new Dictionary<string, object>();
                    pageParameters["after"] = pageAfter;
                    pages = GlobalClass.FB.Get("me/accounts", pageParameters);
                }

                foreach (dynamic pageInfo in pages.data)
                {
                    string pageName = pageInfo.name;
                    string pageID = pageInfo.id;
                    string pageItem = pageName + "/" + pageID;

                    listBoxPageList.Items.Add(pageItem);
                }

                try
                {
                    string nextString = pages.paging.next;
                    int nextStringIdx = nextString.IndexOf("after");

                    if (nextStringIdx > 0)
                    {
                        pageAfter = nextString.Substring(nextStringIdx + 6);
                    }
                }
                catch
                {
                    // no more page
                    break;
                }
            } while (true);
            listBoxPageList.SelectedIndex = 0;

        }

        private void ChkBoxNewAlbum_CheckStateChanged(object sender, EventArgs e)
        {
            string albumAfter = "";

            if (GlobalClass.FBAccessToken == "")
            {
                MessageBox.Show("Please login to Facebook first!!!", "Error");
                chkBoxNewAlbum.Checked = true;
            }
            else
            {

                // new album?
                if (chkBoxNewAlbum.Checked)
                {
                    // it is new album
                    listBoxAlbum.Visible = false;
                    lblAlbumName.Visible = true;
                    txtAlbumName.Visible = true;
                    lblAlbumDesc.Visible = true;
                    txtAlbumDesc.Visible = true;
                }
                else
                {
                    // use existing album
                    listBoxAlbum.Visible = true;
                    lblAlbumName.Visible = false;
                    txtAlbumName.Visible = false;
                    lblAlbumDesc.Visible = false;
                    txtAlbumDesc.Visible = false;
                    try
                    {
                        // get list of album
                        GlobalClass.FB = new FacebookClient(GlobalClass.FBAccessToken);

                        do
                        {
                            // get album
                            dynamic albums;
                            if (albumAfter == "")
                            {
                                // first round, no after now
                                if (chkBoxUploadToPage.Checked)
                                {
                                    // page album
                                    string tempItem = listBoxPageList.SelectedItem.ToString();
                                    string tempID = tempItem.Substring(tempItem.LastIndexOf("/") + 1);
                                    albums = GlobalClass.FB.Get(tempID + "/albums");
                                }
                                else
                                {
                                    // personal album
                                    albums = GlobalClass.FB.Get("me/albums");
                                }
                            }
                            else
                            {
                                // has more album info to retrieve
                                if (chkBoxUploadToPage.Checked)
                                {
                                    // page album
                                    string tempItem = listBoxPageList.SelectedItem.ToString();
                                    string tempID = tempItem.Substring(tempItem.LastIndexOf("/") + 1);
                                    var albumParameters = new Dictionary<string, object>();
                                    albumParameters["after"] = albumAfter;
                                    albums = GlobalClass.FB.Get(tempID + "/albums", albumParameters);
                                }
                                else
                                {
                                    // personal album
                                    var albumParameters = new Dictionary<string, object>();
                                    albumParameters["after"] = albumAfter;
                                    albums = GlobalClass.FB.Get("me/albums", albumParameters);
                                }
                            }

                            foreach (dynamic albumInfo in albums.data)
                            {
                                try
                                {
                                    String item = albumInfo.name + "/" + albumInfo.id;
                                    listBoxAlbum.Items.Add(item);
                                }
                                catch (Exception exception1)
                                {

                                    listBoxAlbum.Visible = false;
                                    lblAlbumName.Visible = true;
                                    txtAlbumName.Visible = true;
                                    lblAlbumDesc.Visible = true;
                                    txtAlbumDesc.Visible = true;
                                    //chkBoxNewAlbum.Checked = true;
                                    throw exception1;
                                }
                            }

                            listBoxAlbum.SelectedIndex = 0;
                            listBoxAlbum.Visible = true;

                            try
                            {
                                // check if next is available
                                string nextString = albums.paging.next;
                                int nextStringIdx = nextString.IndexOf("after");

                                if (nextStringIdx > 0)
                                {
                                    albumAfter = nextString.Substring(nextStringIdx + 6);
                                }
                            }
                            catch // (Exception exception2)
                            {
                                // check new album
                                //listBoxAlbum.Visible = false;
                                //txtAlbumName.Enabled = true;
                                //txtAlbumDesc.Enabled = true;
                                //chkBoxNewAlbum.Checked = true;

                                break;
                            }
                        } while (true);
                    }
                    catch (Exception exception3)
                    {
                        // check new album
                        listBoxAlbum.Visible = false;
                        lblAlbumName.Visible = true;
                        txtAlbumName.Visible = true;
                        lblAlbumDesc.Visible = true;
                        txtAlbumDesc.Visible = true;
                        //chkBoxNewAlbum.Checked = true;

                        MessageBox.Show(exception3.Message, "Error");
                        throw exception3;
                    }
                }
            }
        }



        private async void BtnUpload_Click(object sender, EventArgs e)
        {
            // perform pre-upload process            
            //if (GlobalClass.AlbumDir == "")
            if (GlobalClass.FileList.Count == 0)
                MessageBox.Show("Please select some file first !!", "Error");
            else
            {
                if (chkBoxNewAlbum.Checked)
                {
                    // create new album
                    GlobalClass.FB = new FacebookClient(GlobalClass.FBAccessToken);
                    var albumParameters = new Dictionary<string, object>();
                    albumParameters["message"] = txtAlbumDesc.Text;
                    albumParameters["name"] = txtAlbumName.Text;
                    try
                    {
                        if (chkBoxUploadToPage.Checked)
                        {
                            // use new album in page
                            string tempItem = listBoxPageList.SelectedItem.ToString();
                            GlobalClass.FBPageID = tempItem.Substring(tempItem.LastIndexOf("/") + 1);
                            dynamic result = GlobalClass.FB.Post("/" + GlobalClass.FBPageID + "/albums", albumParameters);
                            GlobalClass.AlbumID = result.id;
                        }
                        else
                        {
                            // create new user album
                            dynamic result = GlobalClass.FB.Post("/me/albums", albumParameters);
                            GlobalClass.AlbumID = result.id;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        btnUpload.Enabled = false;
                        // error, break out
                        return;
                    }
                }
                else
                {
                    if (chkBoxUploadToPage.Checked)
                    {
                        // use existing page album
                        string tempItem = listBoxAlbum.SelectedItem.ToString();
                        GlobalClass.AlbumID = tempItem.Substring(tempItem.LastIndexOf("/") + 1);
                    }
                    else
                    {
                        // use existing personal album
                        string tempItem = listBoxAlbum.SelectedItem.ToString();
                        GlobalClass.AlbumID = tempItem.Substring(tempItem.LastIndexOf("/") + 1);

                    }
                }

                //lblDirectory.Text = GlobalClass.AlbumDir + " (" + GlobalClass.AlbumID + ")";
                btnUpload.Enabled = false;

                // perform upload process
                await DoQUpload();
            }
        }




        private void BtnPicSetting_Click(object sender, EventArgs e)
        {
            FrmPicSetting frm = new FrmPicSetting();
            var result = frm.ShowDialog();
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private static Point CalcStringPos(string txt, string posStr, Font f, int w, int h, int offsetX, int offsetY)
        {

            // calculate font dimension
            Image _fakeImage = new Bitmap(1, 1);
            Graphics _graphics = Graphics.FromImage(_fakeImage); // get overlay text size
            SizeF sSize = _graphics.MeasureString(txt, f);
            Point p = new Point(); // get start coordinate

            switch (posStr)
            {
                case "UL":
                    // top left
                    p.X = offsetX;
                    p.Y = offsetY;
                    break;
                case "UM":
                    // top middle
                    p.X = (int)Math.Round((w - sSize.Width) / 2, 0, MidpointRounding.AwayFromZero)+offsetX;
                    p.Y = offsetY;
                    break;
                case "UR":
                    // top right
                    p.X = w - (int)Math.Round(sSize.Width, 0, MidpointRounding.AwayFromZero) + 1 + offsetX;
                    p.Y = offsetY;
                    break;
                case "ML":
                    p.X = offsetX;
                    p.Y = (int)Math.Round((h - sSize.Height) / 2, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "MM":
                    p.X = (int)Math.Round((w - sSize.Width) / 2, 0, MidpointRounding.AwayFromZero) + offsetX;
                    p.Y = (int)Math.Round((h - sSize.Height) / 2, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "MR":
                    p.X = w - (int)Math.Round(sSize.Width, 0, MidpointRounding.AwayFromZero) + 1 + offsetX;
                    p.Y = (int)Math.Round((h - sSize.Height) / 2, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "LL":
                    p.X = offsetX;
                    p.Y = h - (int)Math.Round(sSize.Height, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "LM":
                    p.X = (int)Math.Round((w - sSize.Width) / 2, 0, MidpointRounding.AwayFromZero) + offsetX;
                    p.Y = h - (int)Math.Round(sSize.Height, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "LR":
                    p.X = w - (int)Math.Round(sSize.Width, 0, MidpointRounding.AwayFromZero) + 1;
                    p.Y = h - (int)Math.Round(sSize.Height, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
            }
            return p;
        }

        private static Point CalcImagePos(ref Image olImage, ref Image mImage, string posStr, int offsetX, int offsetY)
        {
            int olw = olImage.Width;
            int olh = olImage.Height;
            int mw = mImage.Width;
            int mh = mImage.Height;

            Point p = new Point();

            switch (posStr)
            {
                case "UL":
                    // top left
                    p.X = offsetX;
                    p.Y = offsetY;
                    break;
                case "UM":
                    // top middle
                    p.X = (int)Math.Round((double)(mw - olw) / 2, 0, MidpointRounding.AwayFromZero) + offsetX;
                    p.Y = offsetY;
                    break;
                case "UR":
                    // top right
                    p.X = mw - olw + 1 + offsetX;
                    p.Y = offsetY;
                    break;
                case "ML":
                    p.X = offsetX;
                    p.Y = (int)Math.Round((double)(mh - olh) / 2, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "MM":
                    p.X = (int)Math.Round((double)(mw - olw) / 2, 0, MidpointRounding.AwayFromZero) + offsetX;
                    p.Y = (int)Math.Round((double)(mh - olh) / 2, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "MR":
                    p.X = mw - olw + 1 + offsetX;
                    p.Y = (int)Math.Round((double)(mh - olh) / 2, 0, MidpointRounding.AwayFromZero) + offsetY;
                    break;
                case "LL":
                    p.X = offsetX;
                    p.Y = mh - olh + offsetY;
                    break;
                case "LM":
                    p.X = (int)Math.Round((double)(mw - olw) / 2, 0, MidpointRounding.AwayFromZero) + offsetX;
                    p.Y = mh - olh + offsetY;
                    break;
                case "LR":
                    p.X = mw - olw + 1 + offsetX;
                    p.Y = mh - olh + offsetY;
                    break;
            }
            return p;

        }

        private void BtnSelectFiles_Click(object sender, EventArgs e)
        {
            FrmFiler frm = new FrmFiler();
            frm.ShowDialog();
            if (GlobalClass.FileList.Count == 0)
            {
                lblTotalFiles.Text = "No file";
                btnUpload.Enabled = false;
            }
            else
            {
                lblTotalFiles.Text = "Total " + GlobalClass.FileList.Count.ToString() + " file" + (GlobalClass.FileList.Count == 1 ? "." : "s.");
                btnUpload.Enabled = true;
            }
        }

        private async Task DoQUpload()
        {
            // use own facebook client locally here. becasue might need to login using page token
            FacebookClient FBpage = new FacebookClient(GlobalClass.FBAccessToken);

            // build file list array
            // Process the list of files found in the directory.
            if (chkBoxUploadToPage.Checked)
            {
                // use page album
                string myPageItem = listBoxPageList.SelectedItem.ToString();
                string myPageID = myPageItem.Substring(myPageItem.LastIndexOf("/") + 1);
                dynamic pages = GlobalClass.FB.Get("me/accounts/id=" + myPageID);
                dynamic PageAccessToken = pages.data[0].access_token;
                string PageAccessTokenString = PageAccessToken.ToString();
                // relogin by using page access token
                FBpage = new FacebookClient(PageAccessTokenString);
            }

            //string targetDirectory = GlobalClass.AlbumDir;
            string myAlbum = "/" + GlobalClass.AlbumID + "/photos";

            // setup progress bar
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            if (GlobalClass.FileList.Count > 1000)
                progressBar1.Maximum = 1000;
            else
                progressBar1.Maximum = GlobalClass.FileList.Count;

            dynamic parameters = new ExpandoObject();

            //int i = 0;
            //foreach (string fileListName in fileEntries)
            string fileListName = "";
            for (int i = 0; i < GlobalClass.FileList.Count; i++)
            {
                Application.DoEvents();

                fileListName = GlobalClass.FileList[i];

                // display processing file
                lblImageFile.Text = fileListName;
                lblImageFile.Refresh();
                Application.DoEvents();

                // update progress bar
                progressBar1.Value = i+1;
                progressBar1.Refresh();
                Application.DoEvents();

                // read file into memory
                byte[] photo1 = File.ReadAllBytes(fileListName);
                byte[] photo2 = new byte[0];
                ImageFactory imageFactory = new ImageFactory(preserveExifData: true);
                imageFactory.Quality(95);
                MemoryStream inStream = new MemoryStream(photo1);
                MemoryStream outStream = new MemoryStream();
                imageFactory.Load(inStream);
                Application.DoEvents();

                // save exif data to EXIFClass
                ExifClass.Orientation = 1; // reset orientation first, nothing to change
                ConcurrentDictionary<int, PropertyItem> exifItems = new ConcurrentDictionary<int, PropertyItem>();
                exifItems = imageFactory.ExifPropertyItems;
                ExifClass.ClearEXIF();
                // read file info into EXIF for later easy access
                ExifClass.FilePath = Path.GetDirectoryName(fileListName);
                ExifClass.FileName = Path.GetFileName(fileListName);
                ExifClass.FileDateTime = File.GetCreationTime(fileListName);

                foreach (var exifItem in exifItems)
                {
                    ProcessEXIF(exifItem);
                    Application.DoEvents();
                }

                // rotate the picture if necessary
                RotateImage(ref imageFactory);
                Application.DoEvents();

                // update new exif data to photo1
                imageFactory.Save(outStream);
                photo2 = outStream.ToArray();
                photo1 = photo2;
                
                if (GlobalClass.MySetting.ResizePic)
                {
                    ResizeImage(ref imageFactory, ref outStream);

                    photo2 = outStream.ToArray();
                    photo1 = photo2;
                    Application.DoEvents();
                }

                // perform additional picture turning
                if (GlobalClass.MySetting.Brightness !=0 || GlobalClass.MySetting.Contrast != 0 || GlobalClass.MySetting.GaussianSharpen != 0 || GlobalClass.MySetting.Saturation != 0)
                    imageFactory.Brightness(GlobalClass.MySetting.Brightness)
                        .Contrast(GlobalClass.MySetting.Contrast)
                        .GaussianSharpen(GlobalClass.MySetting.GaussianSharpen)
                        .Saturation(GlobalClass.MySetting.Saturation);
                Application.DoEvents();

                // process canvas
                if ((GlobalClass.MySetting.CanvasTop > 0) || (GlobalClass.MySetting.CanvasLeft > 0) || (GlobalClass.MySetting.CanvasRight > 0) || (GlobalClass.MySetting.CanvasBottom > 0))
                {
                    // process canvas
                    int picW = 0;
                    int picH = 0;
                    int pic1W = 0;
                    int pic1H = 0;

                    // get new canvas size
                    picW = imageFactory.Image.Width;
                    picH = imageFactory.Image.Height;
                    pic1W = picW + GlobalClass.MySetting.CanvasLeft + GlobalClass.MySetting.CanvasRight;
                    pic1H = picH + GlobalClass.MySetting.CanvasTop + GlobalClass.MySetting.CanvasBottom;

                    Size size = new Size(pic1W, pic1H);
                    ISupportedImageFormat format = new JpegFormat { Quality = 95 };
                    ResizeMode rmode = ResizeMode.BoxPad;
                    Color bgColor = Color.FromArgb(GlobalClass.MySetting.CanvasColor);
                    ResizeLayer resizeLayer = new ResizeLayer(size, rmode, AnchorPosition.Center, true);

                    // resize action, photo1 contain the resized image
                    // Load, resize, set the format and quality and save an image.
                    imageFactory.Resize(resizeLayer)
                                .Format(format)
                                .BackgroundColor(bgColor)
                                .Save(outStream);
                    // Do something with the stream.
                    photo2 = outStream.ToArray();
                    photo1 = photo2;
                    Application.DoEvents();
                }

                // add overlay text, GlobalClass.MaxOverlayText overlay text only
                if (GlobalClass.MySetting.AddOverlayText)
                {
                    for (int ot = 0; ot < GlobalClass.MaxOverlayText; ot++)
                    {
                        if (GlobalClass.MySetting.OLInfos[ot].InUse)
                        {
                            if (GlobalClass.MySetting.OLInfos[ot].IsFile)
                            {
                                // picture overlay
                                try
                                {
                                    ImageLayer ilayer = new ImageLayer();
                                    //byte[] photoil = File.ReadAllBytes(GlobalClass.MySetting.OLInfos[ot].Text);
                                    Image tmpImage1 = Image.FromFile(GlobalClass.MySetting.OLInfos[ot].Text);
                                    Image tmpImage2 = imageFactory.Image;
                                    ilayer.Image = tmpImage1;
                                    ilayer.Opacity = GlobalClass.MySetting.OLInfos[ot].Opacity;
                                    if (GlobalClass.MySetting.OLInfos[0].Pos == "MM")
                                        ilayer.Position = null;
                                    else
                                    {
                                        Point p = CalcImagePos(ref tmpImage1, ref tmpImage2, GlobalClass.MySetting.OLInfos[ot].Pos,
                                            GlobalClass.MySetting.OLInfos[ot].Xoffset, GlobalClass.MySetting.OLInfos[ot].Yoffset);
                                        ilayer.Position = p;
                                    }
                                    ilayer.Size = new Size { Height = tmpImage1.Height, Width = tmpImage1.Width };

                                    // process the overlay watermark image
                                    imageFactory.Overlay(ilayer)
                                        .Save(outStream);
                                    // Do something with the stream.
                                    photo2 = outStream.ToArray();
                                    photo1 = photo2;
                                } catch (Exception err)
                                {
                                    MessageBox.Show(err.Message, "Error");
                                }
                            }
                            else
                            {
                                // text overlay
                                TextLayer textLayer = new TextLayer();
                                string str = GlobalClass.MySetting.OLInfos[ot].Text;
                                textLayer.Text = ProcessSpecialChar(str); // convert special command char to data string
                                textLayer.FontColor = Color.FromName(GlobalClass.MySetting.OLInfos[ot].FontColor);
                                textLayer.FontFamily = new FontFamily(GlobalClass.MySetting.OLInfos[ot].Font);
                                textLayer.FontSize = (int)Math.Round(Convert.ToDouble(GlobalClass.MySetting.OLInfos[ot].FontSize), 0, MidpointRounding.AwayFromZero);
                                FontStyle fs = new FontStyle();
                                fs = FontStyle.Regular;
                                if (GlobalClass.MySetting.OLInfos[ot].FontAttr.IndexOf("Bold") >= 0)
                                    fs = fs | FontStyle.Bold;
                                if (GlobalClass.MySetting.OLInfos[ot].FontAttr.IndexOf("Italic") >= 0)
                                    fs = fs | FontStyle.Italic;
                                if (GlobalClass.MySetting.OLInfos[ot].FontAttr.IndexOf("Strikeout") >= 0)
                                    fs = fs | FontStyle.Strikeout;
                                if (GlobalClass.MySetting.OLInfos[ot].FontAttr.IndexOf("Underline") >= 0)
                                    fs = fs | FontStyle.Underline;
                                textLayer.Style = fs;
                                textLayer.Opacity = 100;
                                textLayer.DropShadow = false;

                                if (GlobalClass.MySetting.OLInfos[ot].Pos == "MM")
                                    // centre for now
                                    textLayer.Position = null;
                                else
                                {
                                    Font stringFont = new Font(textLayer.FontFamily, textLayer.FontSize, fs, GraphicsUnit.Pixel);
                                    Point p = CalcStringPos(GlobalClass.MySetting.OLInfos[ot].Text, GlobalClass.MySetting.OLInfos[ot].Pos, 
                                        stringFont, imageFactory.Image.Width, imageFactory.Image.Height,
                                        GlobalClass.MySetting.OLInfos[ot].Xoffset, GlobalClass.MySetting.OLInfos[ot].Yoffset);
                                    textLayer.Position = p;
                                }

                                // process the watermark text
                                imageFactory.Watermark(textLayer)
                                            .Save(outStream);
                                // Do something with the stream.
                                photo2 = outStream.ToArray();
                                photo1 = photo2;
                            }
                        }
                        Application.DoEvents();
                    }
                }

                // write fileListName to disk if necessary
                if (GlobalClass.SaveExportJPG)
                {
                    string writeFile = GlobalClass.ExportJPGPath + "\\" + ExifClass.FileName;
                    try
                    {
                        var fs = new BinaryWriter(new FileStream(writeFile, FileMode.Create, FileAccess.Write));
                        fs.Write(photo1);
                        fs.Close();
                        Application.DoEvents();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Error createing " + ExifClass.FileName + Environment.NewLine + err.Message, "Error");
                    }
                }

                // upload fileListName to album
                //parameters.message = fileInfo;
                parameters.message = ProcessSpecialChar(GlobalClass.MySetting.MessageText.Replace("\n", Environment.NewLine)); 
                parameters.source = new FacebookMediaObject
                {
                    ContentType = "image/jpeg",
                    FileName = System.IO.Path.GetFileName(fileListName)
                }.SetValue(photo1);

                // upload to facebook here
                if (GlobalClass.UploadToFB)
                {
                    try
                    {
                        dynamic result = await FBpage.PostTaskAsync(myAlbum, parameters);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(fileListName + "error. " + e.Message, "Error");
                    }
                }
                Application.DoEvents();
            }

            // clear the progress bar and file name label
            progressBar1.Visible = false;
            if (lblImageFile.Text.IndexOf("Error") < 0)
                lblImageFile.Text = GlobalClass.FileList.Count.ToString() + " file" + (GlobalClass.FileList.Count == 1 ? "" : "s") + " uploaded.";

            btnUpload.Enabled = true;
        }

        private void ResizeImage(ref ImageFactory imageFactory, ref MemoryStream outStream)
        {
            // get new size
            Size size = new Size(1024, 1024);
            ISupportedImageFormat format = new JpegFormat { Quality = 95 };
            ResizeMode rmode = ResizeMode.Max;
            if (GlobalClass.MySetting.LongSize > 0)
            {
                size = new Size(GlobalClass.MySetting.LongSize, GlobalClass.MySetting.LongSize);
                rmode = ResizeMode.Max;
            }
            else if (GlobalClass.MySetting.ShortSide > 0)
            {
                size = new Size(GlobalClass.MySetting.ShortSide, GlobalClass.MySetting.ShortSide);
                rmode = ResizeMode.Min;
            }
            ResizeLayer resizeLayer = new ResizeLayer(size, rmode, AnchorPosition.Center, false);

            // resize, photo1 contain the resized image
            // Load, resize, set the format and quality and save an image.
            imageFactory.Resize(resizeLayer)
                        .Format(format)
                        .GaussianSharpen(10)
                        .Save(outStream);

        }

        private void RotateImage(ref ImageFactory imageFactory)
        {
            try
            {
                // if tag exist, rotate, otherwise, no change and return
                imageFactory.ExifPropertyItems[ExifClass.exif_Orientation].Value[0] = 1;
                switch (ExifClass.Orientation)
                {
                    case 1:
                        // nothing to change
                        break;
                    case 2:
                        {
                            ExifClass.Orientation = 1;
                            imageFactory.Flip(false);
                            break;
                        }
                    case 3:
                        {
                            ExifClass.Orientation = 1;
                            imageFactory.Rotate(180);
                            break;
                        }

                    case 4:
                        {
                            ExifClass.Orientation = 1;
                            imageFactory.Flip(true);
                            break;
                        }
                    case 5:
                        {
                            ExifClass.Orientation = 1;
                            imageFactory.Flip(false);
                            imageFactory.Rotate(90);
                            break;
                        }
                    case 6:
                        {
                            ExifClass.Orientation = 1;
                            imageFactory.Rotate(90);
                            break;
                        }
                    case 7:
                        {
                            ExifClass.Orientation = 1;
                            imageFactory.Flip(false);
                            imageFactory.Rotate(-90);
                            break;
                        }
                    case 8:
                        {
                            ExifClass.Orientation = 1;
                            imageFactory.Rotate(-90);
                            break;
                        }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }

        }

        private void ProcessEXIF(dynamic exifItem)
        {
            try
            {
                switch (exifItem.Key)
                {
                    case ExifClass.exif_Orientation:
                        {
                            ExifClass.Orientation = (int)exifItem.Value.Value[0];
                            break;
                        }
                    case ExifClass.exif_Datetime:
                        {
                            string s0, s1, s2;
                            s0 = System.Text.Encoding.Default.GetString(exifItem.Value.Value);
                            s1 = s0.Substring(0, 10).Replace(":", "-");
                            s2 = s0.Substring(11, 8);
                            DateTime d = Convert.ToDateTime(s1 + " " + s2);
                            ExifClass.DateTime = d;
                            break;
                        }
                    case ExifClass.exif_ISO:
                        {
                            Int16 t1;
                            t1 = System.BitConverter.ToInt16(exifItem.Value.Value, 0);
                            ExifClass.Iso = t1;
                            break;
                        }
                    case ExifClass.exif_F:
                        {
                            Int32 t1, t2;
                            float t1a, t2a;
                            t1 = System.BitConverter.ToInt32(exifItem.Value.Value, 0);
                            t1a = (float)t1;
                            t2 = System.BitConverter.ToInt32(exifItem.Value.Value, 4);
                            t2a = (float)t2;
                            ExifClass.F = t1a / t2a;
                            break;
                        }
                    case ExifClass.exif_Exposuretime:
                        {
                            Int32 t1, t2;
                            t1 = System.BitConverter.ToInt32(exifItem.Value.Value, 0);
                            t2 = System.BitConverter.ToInt32(exifItem.Value.Value, 4);
                            ExifClass.Exposuretime1 = t1;
                            ExifClass.Exposuretime2 = t2;
                            break;
                        }
                    case ExifClass.exif_FocalLength:
                        {
                            Int32 t1, t2;
                            t1 = System.BitConverter.ToInt32(exifItem.Value.Value, 0);
                            t2 = System.BitConverter.ToInt32(exifItem.Value.Value, 4);
                            ExifClass.FocalLength = t1 / t2;
                            break;
                        }
                    case ExifClass.exif_ExposureProgram:
                        {
                            Int16 t1;
                            t1 = System.BitConverter.ToInt16(exifItem.Value.Value, 0);
                            ExifClass.ExposureProgram = t1;
                            break;
                        }
                    case ExifClass.exif_Flash:
                        {
                            ExifClass.FlashData = exifItem.Value.Value[0];
                            break;
                        }
                    case ExifClass.exif_WhiteBalance:
                        {
                            Int16 t1;
                            t1 = System.BitConverter.ToInt16(exifItem.Value.Value, 0);
                            ExifClass.WhiteBalance = t1;
                            break;
                        }
                    case ExifClass.exif_CameraModel:
                        {
                            string camera = System.Text.Encoding.Default.GetString(exifItem.Value.Value).Substring(0, exifItem.Value.Len - 1);
                            ExifClass.CameraModel = camera;
                            break;
                        }
                    //case ExifClass.exif_MakerNote:
                    //    {
                    //        Int16 t1;
                    //        t1 = (Int16)exifItem.Value.Len;
                    //        byte[] bs = Enumerable.Repeat((byte)0x00, t1).ToArray(); 
                    //        Buffer.BlockCopy(exifItem.Value.Value, 0, bs, 0, t1);
                    //        ExifClass.FlashData = exifItem.Value.Value[0];
                    //        break;
                    //    }

                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }

        private string ProcessSpecialChar(string s)
        {
            string ss = s;
            string[] toSearch = new string[] { "$D", "$F", "$N", "$O", "$U", "$t", "$i", "$l", "$s", "$f", "$h", "$c", "$n", "$$" };

            string f = "";

            for (int i = 0; i < toSearch.Length; i++)
            {
                int idx = ss.IndexOf(toSearch[i]);
                while (idx >= 0)
                {
                    switch (toSearch[i])
                    {
                        case "$D": //file directory/folder
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.FilePath);
                            break;
                        case "$F": //file name (with extension)
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.FileName);
                            break;
                        case "$N": //file name (without extension)
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.FileName.Substring(0, ExifClass.FileName.Length - 4));
                            break;
                        case "$O": //file extension
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.FileName.Substring(ExifClass.FileName.Length - 3, 3));
                            break;
                        case "$U": //current system date/time
                            f = GlobalClass.DefaultDateTimeFormat; // format string
                            if (ss.Substring(idx + 2, 1) == "{")
                            {
                                // it has a format string
                                int idx1 = ss.IndexOf("}", idx + 3);
                                if (idx1 > 0)
                                {
                                    // correct format
                                    f = ss.Substring(idx + 3, idx1 - idx - 3); // copy out the format string
                                    ss = ss.Remove(idx + 2, idx1 - idx - 1); // remove the format string
                                }
                            }
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, DateTime.Now.ToString(f));
                            break;
                        case "$t": //exif date/time
                            DateTime dt = ExifClass.DateTime;
                            f = GlobalClass.DefaultDateTimeFormat; // format string
                            if (ss.Substring(idx + 2, 1) == "{")
                            {
                                // it has a format string
                                int idx1 = ss.IndexOf("}", idx + 3);
                                if (idx1 > 0)
                                {
                                    // correct format
                                    f = ss.Substring(idx + 3, idx1 - idx - 3); // copy out the format string
                                    ss = ss.Remove(idx + 2, idx1 - idx - 1); // remove the format string
                                }
                            }
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.DateTime.ToString(f));
                            break;
                        case "$i": //ISO
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.Iso.ToString());
                            break;
                        case "$l": //focal length
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.FocalLength.ToString());
                            break;
                        case "$s": //shutter speed
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.ExposuretimeStr);
                            break;
                        case "$f": //f-stop
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.F.ToString());
                            break;
                        case "$h": // flash on/off
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, (ExifClass.Flash ? "On" : "Off"));
                            break;
                        case "$c": // camera model
                            ss = ss.Remove(idx, 2);
                            ss = ss.Insert(idx, ExifClass.CameraModel);
                            break;
                        //case "$n": // lens model
                        //    ss = ss.Remove(idx, 2);
                        //    ss = ss.Insert(idx, ExifClass.lensModel);
                        //    break;
                        case "$$": //$
                            ss = ss.Remove(idx, 1);
                            break;
                    }
                    idx = ss.IndexOf(toSearch[i]);
                }
            }
            return ss;
        }

        private void PboxLanguage_Click(object sender, EventArgs e)
        {

            if (Thread.CurrentThread.CurrentUICulture.ToString().ToUpper() == "EN")
            {
                Properties.Settings.Default.Language = "zh-Hant";
                MessageBox.Show("請重新啟動程式!", "信息");
            }
            else
            {
                Properties.Settings.Default.Language = "en";
                MessageBox.Show("Please restart this application!","Message");
            }
            Properties.Settings.Default.Save();
        }

        private void BtnUploadSetting_Click(object sender, EventArgs e)
        {
            FrmUploadOption frm = new FrmUploadOption();
            frm.Show();
        }

        private void ListBoxPageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get list of albums from personal page
            if (!chkBoxNewAlbum.Checked)
            {

            }
        }
    }
}