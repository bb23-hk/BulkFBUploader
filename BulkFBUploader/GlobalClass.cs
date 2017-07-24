using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using Facebook;
using System.Collections.Generic;

namespace BulkFBUploader
{

    class GlobalClass
    {
        public const string MyName = "Bulk Photo Uploader";
        public const string FACEBOOK_APP_ID = "107029389893584";
        public const string FACEBOOK_APP_SECURITY = "107d575dc1644a070bc3b8b4d272c2f5";
        public const string MyConfigFile = "BulkFBUploader.config.xml";
        public const int MaxOverlayText = 4;
        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        //public static string[] PERMISSIONS = new string[] { "user_about_me", "publish_actions", "user_photos", "manage_pages", "pages_show_list", "publish_pages" };
        public static string[] PERMISSIONS = new string[] { "publish_actions", "user_photos", "manage_pages", "pages_show_list" };

        private static string _FBAccessToken = "";
        public static string FBAccessToken { get => _FBAccessToken; set => _FBAccessToken = value; }

        private static string _FBID = "";
        public static string FBID { get => _FBID; set => _FBID = value; }

        private static string _FBPageID = "";
        public static string FBPageID { get => _FBPageID; set => _FBPageID = value; }

        private static string _AlbumID = "";
        public static string AlbumID { get => _AlbumID; set => _AlbumID = value; }

        private static FacebookClient fb;
        public static FacebookClient FB { get => fb; set => fb = value; }

        private static string _MyDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string MyDocPath { get => _MyDocPath; }
        public static string MyDocPathXML { get => _MyDocPath + "\\" + _MyAppName(); }
        public const string XMLFile = "BulkFBUploader.config.XML";

        public static bool SaveExportJPG = false;   // save the processed file
        public static string ExportJPGPath = "";    // path to save the processed file
        public static bool UploadToFB = true;       // really upload the processed file to Facebook

        // local storage of XML setting file
        public static PicSettingClass MySetting = null;
        // editing setting, copy to/from for saving/retrieve setting data
        public static PicSettingClass MyEdit = null;

        // list of file to be load into facebook
        public static List<string> FileList;

        // this application's name
        private static string _MyAppName()
        {
            string fullPath = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            return fullPath.Substring(0, fullPath.Length - 4);
        }

        // create application XML directory
        public static void CreateMyDocPath()
        {
            // check if my XML directory exist
            if (!Directory.Exists(MyDocPathXML))
            {
                try
                {
                    // create dicectory
                    DirectoryInfo result = Directory.CreateDirectory(MyDocPathXML);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(),"Error");
                    Application.Exit();
                }
            }
        }



        public static void GetSetting()
        {
            // use XML setting file
            // setup the detail overlay text/picture details
            MySetting.OLInfos = new List<PicSettingOLClass>();
            MyEdit.OLInfos = new List<PicSettingOLClass>();
            for (int i = 0; i < GlobalClass.MaxOverlayText; i++)
            {
                MySetting.OLInfos.Add(new PicSettingOLClass());
                MyEdit.OLInfos.Add(new PicSettingOLClass());
            }

            // read data from XML file
            GetSettingFromXML();

            // if text field is null, assign an empty string
            if (MySetting.MessageText == null)
                MySetting.MessageText = "";
            for (int i = 0; i < GlobalClass.MaxOverlayText; i++)
            {
                if (MySetting.OLInfos[i].Text == null)
                    MySetting.OLInfos[i].Text = "";
            }

            // copy MySetting to MyEdit
            string tmpString = SettingToXml(MySetting);
            FromXml(tmpString, MyEdit);
        }

        // load setting from XML file
        private static void GetSettingFromXML()
        {
            MySetting = XML_Filer.ReadFromXmlFile<PicSettingClass>(GlobalClass.MyDocPathXML + "\\" + GlobalClass.XMLFile);

            // copy MySetting to MyEdit
            string tmpString = SettingToXml(MySetting);
            FromXml(tmpString, MyEdit);
        }

        // save default value to XML
        public static void SaveDefault()
        {
            // copy MyEdit to MySetting
            string tmpString = SettingToXml(MyEdit);
            FromXml(tmpString, MySetting);
            XML_Filer.WriteToXmlFile<PicSettingClass>(GlobalClass.MyDocPathXML + "\\" + GlobalClass.XMLFile, MySetting, false);
        }

        //
        // the following 2 routine is for copying of objects
        //

        // save MySetting to XML string, and return the string
        public static string SettingToXml(Object o)
        {
            try
            {
                //object psc = new PicSettingClass();
                XmlSerializer s = new XmlSerializer(o.GetType());
                using (StringWriter writer = new StringWriter())
                {
                    s.Serialize(writer, o);
                    return writer.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        // read the XML string and store it to MySetting
        // data is the XML string
        public static void FromXml(string data, object o)
        {
            try
            {

                XmlSerializer s = new XmlSerializer(typeof(PicSettingClass));
                using (StringReader reader = new StringReader(data))
                {
                    o = s.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

}
