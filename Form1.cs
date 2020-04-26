using CSCSCH;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TeximpNet;
using System.Linq;
using System.Collections.Generic;

namespace Half_Life_Background_Changer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Load += (o, e) =>
            {
                cbResolutions.SelectedIndex = 1;

                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Go into your mod directory and select 'resource' folder";
                    fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                    fbd.ShowNewFolderButton = false;
                    fbd.ShowDialog();

                    if (fbd.SelectedPath != string.Empty)
                    {
                        if (File.Exists(fbd.SelectedPath + "\\BackgroundLayout.txt"))
                        {
                            PreviewPane.BackgroundLocation = fbd.SelectedPath;
                            PreviewPane.BackgroundLayoutLocation = fbd.SelectedPath + "\\BackgroundLayout.txt";
                        }
                        else
                        {
                            // get half-life root directory
                            string resourcePath = Path.GetDirectoryName(fbd.SelectedPath);
                            resourcePath = Path.GetDirectoryName(resourcePath);

                            if (resourcePath == Path.GetPathRoot(resourcePath))
                            {
                                MessageBox.Show("Invalid Path Folder.", "Root Drive Detected");
                                Application.Exit();
                            }
                            else
                            {
                                resourcePath += "\\valve\\resource";

                                if (File.Exists(resourcePath + "\\BackgroundLayout.txt"))
                                {
                                    PreviewPane.BackgroundLocation = fbd.SelectedPath;
                                    PreviewPane.BackgroundLayoutLocation = resourcePath + "\\BackgroundLayout.txt";
                                }
                                else
                                {
                                    MessageBox.Show("'valve' folder was not found. Cannot resolve missing 'BackgroundLayout.txt'", "Resolve Error");
                                    Application.Exit();
                                }
                            }
                        }
                    }
                    else
                        Application.Exit();
                }

                // update textboxes whenever combobox item is changed
                cbResolutions.TextChanged += RespectCBChanges;

                // fix input (removes alpha/symbolic characters)
                tbCWidth.TextChanged += InspectInput;
                tbCHeight.TextChanged += InspectInput;

                // fix input (check if input is within the allowable values (t >= 640 && t <= 7680))
                tbCWidth.Leave += VerifyInput;
                tbCHeight.Leave += VerifyInput;

                mbReplaceImage.Click += ReplaceImage;
                mbApply.Click += ApplyChanges;

                mbAbout.Click += (obj, ev) =>
                {
                    AboutBox.ShowBox();
                };
            };
        }

        private void ApplyChanges(object sender, EventArgs e)
        {
            if (PreviewPane.BackgroundImage == null)
            {
                MessageBox.Show("No changes were made.", "Operation Aborted");
                return;
            }

            int width = int.Parse(tbCWidth.Text);
            int height = int.Parse(tbCHeight.Text);

            if (width < 640 || width > 7680)
                width = 800;

            if (height < 480 || height > 4320)
                height = 600;

            string newBGPath = PreviewPane.BackgroundLocation + "\\background";

            if (!Directory.Exists(newBGPath))
                Directory.CreateDirectory(newBGPath);

            newBGPath += "\\HLBC";

            if (!Directory.Exists(newBGPath))
                Directory.CreateDirectory(newBGPath);

            Bitmap[,] newBmps = PreviewPane.ChopNewImage(width, height);

            string[] newBmpsLocations = PreviewPane.NewBitmapLocations;

            int bmpWCount = newBmps.GetLength(0);
            int bmpHCount = newBmps.Length;
            int x = 0;
            int y = 0;

            string backgroundLayoutContents = "resolution\t" + width + "\t" + height + "\n";
            foreach (var bmpLoc in newBmpsLocations)
            {
                string bmpRelPath = PreviewPanel.RemoveExtraSpaces(bmpLoc).Split()[0];
                string bmpPath = PreviewPane.BackgroundLocation.Remove(PreviewPane.BackgroundLocation.Length - 9, 9) + "\\" + bmpRelPath;
                backgroundLayoutContents += bmpLoc + "\n";

                try
                {
                    newBmps[x, y].Save(bmpPath + ".bmp");
                    Surface surf = Surface.LoadFromFile(bmpPath + ".bmp", ImageLoadFlags.TARGA_LoadRGB888);
                    surf.SaveToFile(ImageFormat.TARGA, bmpPath, ImageSaveFlags.Default);

                    // cleanup!!!
                    File.Delete(bmpPath + ".bmp");
                } catch
                {
                    MessageBox.Show("File IO Error. File has been probably corrupted: " + bmpPath);
                }

                if (++x >= bmpWCount)
                {
                    x = 0;

                    if (++y >= bmpHCount)
                        break;
                }
            }

            ACResult result = ApplyConfirmation.ShowConfirmation();

            switch (result)
            {
                case ACResult.IGNORE:
                    return;

                case ACResult.BACKUP:
                    if (File.Exists(PreviewPane.BackgroundLocation + "\\BackgroundLayout.txt"))
                    {
                        int cr = 1;

                        while (File.Exists(PreviewPane.BackgroundLocation + "\\BackgroundLayout_v" + cr + ".txt"))
                            cr++;

                        string bgLines = File.ReadAllText(PreviewPane.BackgroundLocation + "\\BackgroundLayout.txt");
                        File.WriteAllText(PreviewPane.BackgroundLocation + "\\BackgroundLayout_v" + cr + ".txt", bgLines);

                        MessageBox.Show("Backup saved as: BackgroundLayout_v" + cr + ".txt", "Backup Saved");
                    }
                    else
                        MessageBox.Show("BackgroundLayout.txt was not found inside the resources folder. Backup is unnecessary.", "Backup Skipped");

                    File.WriteAllText(PreviewPane.BackgroundLocation + "\\BackgroundLayout.txt", backgroundLayoutContents);
                    break;

                case ACResult.OVERWRITE:
                    File.WriteAllText(PreviewPane.BackgroundLocation + "\\BackgroundLayout.txt", backgroundLayoutContents);
                    break;
            }

            MessageBox.Show("Successfully Changed Background Layout!\r\nDon't forget to match the in-game resolution with your newly created background.", "Changes Saved");
        }

        private void ReplaceImage(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.png";
                ofd.Title = "Select An Image";
                ofd.Multiselect = false;

                ofd.FileOk += (o, ev) =>
                {
                    PreviewPane.BackgroundImage = Image.FromFile(ofd.FileName);
                };

                ofd.ShowDialog();
            }
        }

        private void InspectInput(object sender, EventArgs e)
        {
            var tb = sender as TextBox;

            try
            {
                int size = int.Parse(tb.Text);
                tb.Text = size.ToString();
            } catch
            {
                tb.Text = tb == tbCWidth ? "800" : "600";
            }
        }

        private void VerifyInput(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            int size = int.Parse(tb.Text);

            if (tb == tbCWidth && (size < 640 || size > 7680))
                tb.Text = "800";
            else if (tb == tbCHeight && (size < 480 || size > 4320))
                tb.Text = "600";
        }

        private void RespectCBChanges(object sender, EventArgs e)
        {
            tlpResContainer.Enabled = cbResolutions.Text == "Custom";

            string[] res = cbResolutions.Text.Split(new char[] { 'x', 'X' });

            if (res.Length < 2)
                return;

            try
            {
                tbCWidth.Text = int.Parse(res[0]).ToString();
                tbCHeight.Text = int.Parse(res[1]).ToString();
            }
            catch
            {
                MessageBox.Show("Invalid resolution data. Ignoring...", "Malformed Data");
            }
        }
    }
}
