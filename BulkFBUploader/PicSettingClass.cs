using System;
using System.Collections.Generic;


namespace BulkFBUploader
{

    public class PicSettingClass
    {
        public Boolean ResizePic { get; set; }
        public int LongSize { get; set; }
        public int ShortSide { get; set; }
        public int MaxSize { get; set; }
        public Boolean SameCanvas { get; set; }
        public int CanvasTop { get; set; }
        public int CanvasLeft { get; set; }
        public int CanvasRight { get; set; }
        public int CanvasBottom { get; set; }
        public int CanvasColor { get; set; }
        public int Brightness { get; set; } // -100 to 100
        public int Contrast { get; set; } // -100 to 100
        public int GaussianSharpen { get; set; } // 0 to 50
        public int HueDegrees { get; set; } // 0 to 360
        public bool HueRotate { get; set; } 
        public int Saturation { get; set; } // -100 to 100


        public Boolean AddOverlayText { get; set; }

        // overlay detail
        public List<PicSettingOLClass> OLInfos { get; set; }

        public string MessageText { get; set; }
    }

    public class PicSettingOLClass
    {
        public int Index { get; set; }
        public bool InUse { get; set; }
        public string Pos { get; set; }
        public string Ori { get; set; }
        public string Font { get; set; }
        public string FontSize { get; set; }
        public string FontAttr { get; set; }
        public string FontColor { get; set; }
        public string FontCharset { get; set; }
        public string FontUnit { get; set; }
        public int Xoffset { get; set; }
        public int Yoffset { get; set; }
        public bool IsFile { get; set; }
        public int Opacity { get; set; }
        public string Text { get; set; }

        public PicSettingOLClass()
        {
        }

        public PicSettingOLClass(int aIndex, bool aInUse, string aPos, string aOri, 
            string aFont, string aFontSize, string aFontAttr, string aFontColor, string aFontCharset, string aFontUnit, 
            int aXoffset, int aYoffset,
            bool aIsFile, int aOpacity, string aText)
        {
            this.Index = aIndex;
            this.InUse = aInUse;
            this.Pos = aPos;
            this.Ori = aOri;
            this.Font = aFont;
            this.FontSize = aFontSize;
            this.FontAttr = aFontAttr;
            this.FontColor = aFontColor;
            this.FontCharset = aFontCharset;
            this.FontUnit = aFontUnit;
            this.IsFile = aIsFile;
            this.Text = aText;
        }
    }

}
