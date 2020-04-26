using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CSCSCH
{
    public partial class Divider : Control
    {
        [Browsable(false)]
        public new Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        private Color darkBorderColor = Color.DarkGray;
        private Color lightBorderColor = Color.White;

        [Description("This Divider's Dark Border Color."), Category("Color Manipulation")]
        public Color DarkBorderColor { get => darkBorderColor; set { darkBorderColor = value; Invalidate(); } }
        [Description("This Divider's Light Border Color."), Category("Color Manipulation")]
        public Color LightBorderColor { get => lightBorderColor; set { lightBorderColor = value; Invalidate(); } }

        public Divider()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);

            base.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var pen = new Pen(darkBorderColor))
            {
                e.Graphics.DrawLine(pen, 0, 0, this.Width, 0);

                pen.Color = lightBorderColor;
                e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
            }

            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Height = 2;
            base.OnSizeChanged(e);
        }
    }
}
