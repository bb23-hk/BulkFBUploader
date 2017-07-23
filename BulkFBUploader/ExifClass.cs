using System;

namespace BulkFBUploader
{
    class ExifClass
    {
        public const Int32 exif_Orientation = 0x0112; //274
        public const Int32 exif_Datetime = 0x9003; //36867
        public const Int32 exif_ISO = 0x8827; //34855
        public const Int32 exif_F = 0x829d; //33437
        public const Int32 exif_Exposuretime = 0x829a; //33434
        public const Int32 exif_ExposureProgram = 0x8822; //34850
        public const Int32 exif_Flash = 0x9209; //37385
        public const Int32 exif_WhiteBalance = 0xa403; //41987
        public const Int32 exif_CameraModel = 0x0110; //272
        public const Int32 exif_FocalLength = 0x920a; //37386
        public const Int32 exif_MakerNote = 0x927c; //37500

        // the folowing 3 items is for easy data access only, has nothing to do with EXIF
        public static DateTime FileDateTime { get; set; }
        public static string FilePath { get; set; }
        public static string FileName { get; set; }

        private static int _orientation = 0;
        public static int Orientation { get => _orientation; set => _orientation = value; }

        private static DateTime _datetime = DateTime.Parse("1990-01-01 12:00:00");
        public static DateTime DateTime { get => _datetime; set => _datetime = value; }
        public static string DateTimeStr { get => Convert.ToString(_datetime); }

        private static Int32 _iso = 0;
        public static Int32 Iso { get => _iso; set => _iso = value; }

        private static float _f = 0; // f-stop
        public static float F { get => _f; set => _f = value; }

        private static Int32 _exposuretime1 = 0;
        private static Int32 _exposuretime2 = 1;
        public static float Exposuretime { get => _exposuretime1/_exposuretime2;} // shutter speed
        public static Int32 Exposuretime1 { get => _exposuretime1; set => _exposuretime1 = value; }
        public static Int32 Exposuretime2 { get => _exposuretime2; set => _exposuretime2 = value; }
        public static string ExposuretimeStr { get => ConvertExprosure(_exposuretime1,_exposuretime2); }

        private static string ConvertExprosure(int a, int b)
        {
            if (Math.Floor((decimal)a/10)*10 == a)
            {
                // can be divid by 10
                a = (int)Math.Floor((decimal)a / 10);
                b = (int)Math.Floor((decimal)b / 10);
                return a.ToString() + "/" + b.ToString();
            }
            return a.ToString() + "/" + b.ToString(); 
        }

        private static float _focalLength = 0;
        public static float FocalLength { get => _focalLength; set => _focalLength = value; }

        //0 = Not defined
        //1 = Manual
        //2 = Normal program
        //3 = Aperture priority
        //4 = Shutter priority
        //5 = Creative program(biased toward depth of field)
        //6 = Action program(biased toward fast shutter speed)
        //7 = Portrait mode(for closeup photos with the background out of focus)
        //8 = Landscape mode(for landscape photos with the background in focus)
        private static int _exposureProgram = 0;
        public static int ExposureProgram { get => _exposureProgram; set => _exposureProgram = value; }

        //hex 0000 = Flash did not fire
        //hex 0001 = Flash fired
        //hex 0005 = Strobe return light not detected
        //hex 0007 = Strobe return light detected
        //hex 0009 = Flash fired, compulsory flash mode
        //hex 000D = Flash fired, compulsory flash mode, return light not detected
        //hex 000F = Flash fired, compulsory flash mode, return light detected
        //hex 0010 = Flash did not fire, compulsory flash mode
        //hex 0018 = Flash did not fire, auto mode
        //hex 0019 = Flash fired, auto mode
        //hex 001D = Flash fired, auto mode, return light not detected
        //hex 001F = Flash fired, auto mode, return light detected
        //hex 0020 = No flash function
        //hex 0041 = Flash fired, red-eye reduction mode
        //hex 0045 = Flash fired, red-eye reduction mode, return light not detected
        //hex 0047 = Flash fired, red-eye reduction mode, return light detected
        //hex 0049 = Flash fired, compulsory flash mode, red-eye reduction mode
        //hex 004D = Flash fired, compulsory flash mode, red-eye reduction mode, return light not detected
        //hex 004F = Flash fired, compulsory flash mode, red-eye reduction mode, return light detected
        //hex 0059 = Flash fired, auto mode, red-eye reduction mode
        //hex 005D = Flash fired, auto mode, return light not detected, red-eye reduction mode
        //hex 005F = Flash fired, auto mode, return light detected, red-eye reduction mode

        private static byte _flashData = 0;
        public static byte FlashData { get => _flashData; set => _flashData = value; }
        public static bool Flash { get => (RightStr(Convert.ToString(_flashData,2).PadLeft(8, '0'),1)=="1"); set => _flashData = (value ? (byte) 1 : (byte) 0); }

        //0 = Auto white balance
        //1 = Manual white balance
        private static int _whiteBalance = 0;
        public static int WhiteBalance { get => _whiteBalance; set => _whiteBalance = value; }

        private static string _cameraModel = "";
        public static string CameraModel { get => _cameraModel; set => _cameraModel = value; }

        //private static string _lensModel = "";
        //public static string lensModel { get => _lensModel; set => _lensModel = value; }

        private static string RightStr(string s, int i)
        {
            return s.Substring(s.Length - i);
        }

        public static void ClearEXIF()
        {
            FileDateTime = DateTime.Now;
            FilePath = "";
            FileName = "";
            _orientation = 0;
            _datetime = DateTime.Parse("1990-01-01 12:00:00");
            _iso = 0;
            _f = 0;
            _exposuretime1 = 0;
            _exposuretime2 = 1;
            _focalLength = 0;
            _exposureProgram = 0;
            _flashData = 0;
            _whiteBalance = 0;
            _cameraModel = "";
            //_lensModel = "";
        }
    }
}
