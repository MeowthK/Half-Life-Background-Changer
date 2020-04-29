using CSCSCH;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TeximpNet;

namespace Half_Life_Background_Changer
{
    public partial class Form1 : Form
    {
        private LoadingDialog loaddlg;

        private static void ChangeProgress(LoadingDialog ld, int progress)
        {
            if (ld == null)
                return;

            if (ld.InvokeRequired)
                ld.Invoke(new Action(() =>
                {
                    ld.LoadPercent = progress;
                }));
            else
                ld.LoadPercent = progress;
        }

        private void StartProgress()
        {
            var tstart = new ThreadStart(() => {
                loaddlg = new LoadingDialog();
                loaddlg.LoadPercent = 0;
                loaddlg.ShowDialog();
            });
            var thread = new Thread(tstart);
            thread.Start();
        }

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
                        StartProgress();

                        if (File.Exists(fbd.SelectedPath + "\\BackgroundLayout.txt"))
                        {
                            PreviewPane.BackgroundLocation = fbd.SelectedPath;
                            ChangeProgress(loaddlg, 50);

                            PreviewPane.BackgroundLayoutLocation = fbd.SelectedPath + "\\BackgroundLayout.txt";
                            ChangeProgress(loaddlg, 99);
                        }
                        else
                        {
                            ChangeProgress(loaddlg, 25);

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
                                    ChangeProgress(loaddlg, 50);

                                    PreviewPane.BackgroundLocation = fbd.SelectedPath;
                                    ChangeProgress(loaddlg, 75);
                                    PreviewPane.BackgroundLayoutLocation = resourcePath + "\\BackgroundLayout.txt";
                                    ChangeProgress(loaddlg, 99);
                                }
                                else
                                {
                                    MessageBox.Show("'valve' folder was not found. Cannot resolve missing 'BackgroundLayout.txt'", "Resolve Error");
                                    Application.Exit();
                                }
                            }
                        }

                        ChangeProgress(loaddlg, 100);
                        Activate();
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

                PreviewPane.BackgroundImageChanged += (obj, ev) =>
                {
                    if (PreviewPane.BackgroundImage != null)
                        bxLblDim.Text = "Image Size: " + PreviewPane.BackgroundImage.Width + " x " + PreviewPane.BackgroundImage.Height;
                    else
                        bxLblDim.Text = "No Image Loaded.";
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

            ACResult result = ApplyConfirmation.ShowConfirmation();

            if (result == ACResult.IGNORE)
                return;

            StartProgress();

            int width = int.Parse(tbCWidth.Text);
            int height = int.Parse(tbCHeight.Text);

            if (width < 640 || width > 7680)
                width = 800;

            if (height < 480 || height > 4320)
                height = 600;

            string newBGPath = PreviewPane.BackgroundLocation + "\\background";

            ChangeProgress(loaddlg, 25);

            if (!Directory.Exists(newBGPath))
                Directory.CreateDirectory(newBGPath);

            newBGPath += "\\HLBC";

            if (!Directory.Exists(newBGPath))
                Directory.CreateDirectory(newBGPath);

            Bitmap[,] newBmps = PreviewPane.ChopNewImage(width, height);
            ChangeProgress(loaddlg, 50);

            string[] newBmpsLocations = PreviewPane.NewBitmapLocations;
            ChangeProgress(loaddlg, 75);

            int bmpWCount = newBmps.GetLength(0);
            int bmpHCount = newBmps.Length;
            int x = 0;
            int y = 0;

            string backgroundLayoutContents = "resolution\t" + width + "\t" + height + "\n";
            string errorMsg = string.Empty;
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
                }
                catch
                {
                    errorMsg += "File has been probably corrupted: " + bmpPath + "\n";
                }

                // cleanup!!!
                File.Delete(bmpPath + ".bmp");

                if (++x >= bmpWCount)
                {
                    x = 0;

                    if (++y >= bmpHCount)
                        break;
                }
            }

            ChangeProgress(loaddlg, 100);

            if (errorMsg.Length > 0)
            {
                Activate();
                MessageBox.Show("Background Change Failed! Permission to write to file was denied.\n" + errorMsg, "File IO Error");
                return;
            }

            switch (result)
            {
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
            Activate();
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
                    StartProgress();
                    ChangeProgress(loaddlg, 50);

                    PreviewPane.BackgroundImage = Image.FromFile(ofd.FileName);
                    ChangeProgress(loaddlg, 100);
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
            }
            catch
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