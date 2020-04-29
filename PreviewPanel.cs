using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using DmitryBrant.ImageFormats;

namespace CSCSCH
{
    public partial class PreviewPanel : Panel
    {
        private Bitmap[,] tga;
        private Bitmap loadedImage= null;

        private string backgroundLocation;
        private string backgroundLayoutLocation;

        public string[] NewBitmapLocations = null;

        public string BackgroundLocation { get => backgroundLocation;
            set
            {
                backgroundLocation = value;

                if (backgroundLocation == null || backgroundLocation == string.Empty)
                    return;

                LoadBitmaps();
                Invalidate();
            }
        }

        public string BackgroundLayoutLocation { get => backgroundLayoutLocation;
            set
            {
                backgroundLayoutLocation = value;

                if (backgroundLayoutLocation == null || backgroundLayoutLocation == string.Empty)
                    return;

                LoadBitmaps();
                Invalidate();
            }
        }

        public PreviewPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void LoadBitmaps()
        {
            if (!Directory.Exists(backgroundLocation))
                return;
                //throw new Exception("Path not found: " + backgroundLocation);

            if (!File.Exists(backgroundLayoutLocation))
                return;
                //throw new Exception("File not found: " + backgroundLayoutLocation);

            string[] layoutContents = File.ReadAllLines(backgroundLayoutLocation);

            if (layoutContents.Length == 0)
                throw new Exception("BackgroundLayout.txt is empty.");

            if (!layoutContents[0].Contains("resolution"))
                throw new Exception("BackgroundLayout.txt: resolution keyword must be in the first line.");

            string resolutionLine = RemoveExtraSpaces(layoutContents[0]);

            string[] splitted;
            if ((splitted = resolutionLine.Split()).Length < 3)
                throw new Exception("BackgroundLayout.txt: Resolution has incomplete parameter.");

            Size resolution;
            try
            {
                resolution = new Size(int.Parse(splitted[1]), int.Parse(splitted[2]));
            }
            catch
            {
                throw new Exception("BackgroundLayout.txt: Resolution parameter(s) must be a number.");
            }

            Size sz = resolution;

            int widthChopCount = 0;
            int heightChopCount = 0;

            while (sz.Height > 0)
            {
                while (sz.Width > 0)
                {
                    sz.Width -= 256;
                    widthChopCount++;
                }

                sz.Height -= 256;
                heightChopCount++;
            }

            List<string> chopSizes = new List<string>();

            for (int y = 0; y < heightChopCount; y++)
            {
                for (int x = 0; x < widthChopCount; x++)
                {
                    string chopLine = (x * 256) + " " + (y * 256);
                    chopSizes.Add(chopLine);
                }
            }

            List<string> imageLocations = new List<string>();

            foreach (string dim in chopSizes)
            {
                foreach(string line in layoutContents)
                {
                    string lineT = RemoveExtraSpaces(line);

                    if (lineT.Contains(dim))
                    {
                        string location = lineT.Split()[0];
                        imageLocations.Add(backgroundLocation.Remove(backgroundLocation.Length - 9, 9) + "\\" + location);
                        break;
                    }
                }
            }

            if (imageLocations.Count < widthChopCount * heightChopCount)
                throw new Exception("BackgroundLayout.txt: Incomplete image list.");

            tga = new Bitmap[widthChopCount, heightChopCount];

            int imgLocIndex = 0;
            string missingFiles = string.Empty;
            for (int y = 0; y < heightChopCount; y++)
            {
                for (int x = 0; x < widthChopCount; x++)
                {
                    try
                    {
                        tga[x, y] = TgaReader.Load(imageLocations[imgLocIndex++]);
                    }
                    catch
                    {
                        missingFiles += "File Not Found: " + imageLocations[imgLocIndex - 1] + "\n";
                    }
                }
            }

            if (missingFiles != string.Empty)
            {
                FindForm().Activate();
                MessageBox.Show(missingFiles, "Dependency Error");
            }

            loadedImage = new Bitmap(resolution.Width, resolution.Height);

            using (var g = Graphics.FromImage(loadedImage))
            {
                int tgaX = 0;
                int tgaY = 0;

                foreach (var dim in chopSizes)
                {
                    string[] dimT = dim.Split();

                    int x = int.Parse(dimT[0]);
                    int y = int.Parse(dimT[1]);

                    if (tga[tgaX, tgaY] != null)
                        g.DrawImage(tga[tgaX, tgaY], x, y, tga[tgaX, tgaY].Width, tga[tgaX, tgaY].Height);

                    if (++tgaX >= widthChopCount)
                    {
                        tgaX = 0;

                        if (++tgaY >= heightChopCount)
                            break;
                    }
                }
            }
        }

        public static string RemoveExtraSpaces(string line)
        {
            if (line.Trim() == string.Empty)
                return string.Empty;

            string trimmed = line.Trim();
            string newline = string.Empty;

            for (int i = 0; i < trimmed.Length; i++)
            {
                while (i < trimmed.Length && char.IsWhiteSpace(trimmed[i]))
                    i++;

                while (i < trimmed.Length && !char.IsWhiteSpace(trimmed[i]))
                    newline += trimmed[i++];
                newline += " ";
            }

            return newline.Trim();
        }

        public string CardinalAlpha(int input)
        {
            if (input <= 0)
                return "0";

            string retVal = string.Empty;

            while (input > 0)
            {
                var c = (char)(input % 27 + 'a' - 1);

                if (c < 'a')
                    c = '0';

                retVal = retVal.Insert(0, c + "");

                input /= 27;
            }

            return retVal;
        }

        public Bitmap[,] ChopNewImage(int NewWidth, int NewHeight)
        {
            Size sz = new Size(NewWidth, NewHeight);

            int widthChopCount = 0;
            int heightChopCount = 0;

            while (sz.Height > 0)
            {
                while (sz.Width > 0)
                {
                    sz.Width -= 256;
                    widthChopCount++;
                }

                sz.Height -= 256;
                heightChopCount++;
            }

            List<string> chopSizes = new List<string>();

            for (int y = 0; y < heightChopCount; y++)
            {
                for (int x = 0; x < widthChopCount; x++)
                {
                    string chopLine = "resource/background/HLBC/" + NewHeight + "_" + (y + 1) + "_" + CardinalAlpha(x + 1) + ".tga" + "\tscaled\t" + (x * 256) + "\t" + (y * 256);
                    chopSizes.Add(chopLine);
                }
            }

            NewBitmapLocations = chopSizes.ToArray();

            Bitmap newBmp = new Bitmap(BackgroundImage, NewWidth, NewHeight);
            Bitmap[,] tgaE = new Bitmap[widthChopCount, heightChopCount];

            for (int y = 0; y < heightChopCount; y++)
            {
                for (int x = 0; x < widthChopCount; x++)
                {
                    int width = 256;
                    int height = 256;

                    if (x == widthChopCount - 1 && 256 * widthChopCount - NewWidth > 0)
                        width = 256 * widthChopCount - NewWidth;

                    if (y == heightChopCount - 1 && 256 * heightChopCount - NewHeight > 0)
                        height = 256 * heightChopCount - NewHeight;

                    tgaE[x, y] = new Bitmap(width, height);

                    using (var g = Graphics.FromImage(tgaE[x, y]))
                    {
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        g.DrawImage(newBmp, 0, 0, new Rectangle(x * 256, y * 256, width, height), GraphicsUnit.Pixel);
                    }
                }
            }

            return tgaE;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (BackgroundImage == null && loadedImage != null)
                e.Graphics.DrawImage(loadedImage, 0, 0, Width, Height);

            base.OnPaint(e);
        }

        protected override void OnSizeChanged( EventArgs e )
        {
            Invalidate();
            base.OnSizeChanged( e );
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            Invalidate();
            base.OnScroll(se);
        }
    }
}
