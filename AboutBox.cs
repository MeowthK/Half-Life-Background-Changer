using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Half_Life_Background_Changer
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            Load += (o, e) =>
            {
                memberLink.LinkClicked += (o1, e1) =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(memberLink.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to open link. Reason: " + ex.Message);
                    }
                };
            };
        }

        public static void ShowBox()
        {
            using (var ab = new AboutBox())
            {
                ab.ShowDialog();
                ab.Dispose();
            }
        }
    }
}
