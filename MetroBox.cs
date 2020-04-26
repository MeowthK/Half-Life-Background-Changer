using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CSCSCH
{
    public partial class MetroBox : Panel
    {
        private Color darkBorderColor = Color.DarkGray;
        private Color lightBorderColor = Color.White;
        private Color fillColor = Color.LightGray;
        private bool pressed = false;

        [Description("This Border's Dark Part Color."), Category("Color Manipulation")]
        public Color DarkBorderColor { get => darkBorderColor; set { darkBorderColor = value; Invalidate(); } } 
        [Description("This Border's Light Part Color."), Category("Color Manipulation")]
        public Color LightBorderColor { get => lightBorderColor; set { lightBorderColor = value; Invalidate(); } }
        [Description("This Box's Fill Part Color."), Category("Color Manipulation")]
        public Color FillColor { get => fillColor; set { fillColor = value; Invalidate(); } }

        [Description("This Box's Pressed State."), Category("State")]
        public bool Inset { get => pressed; set { pressed = value; Invalidate(); } }

        [Browsable(false)]
        public new Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        public MetroBox()
        {
            InitializeComponent();
            DoubleBuffered = true;

            base.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle rect = ClientRectangle;
            rect.X = 1;
            rect.Y = 1;
            rect.Width -= 2;
            rect.Height -= 2;

            e.Graphics.FillRectangle(new SolidBrush(FillColor), rect);

            rect.Width += 2;
            rect.Height += 2;

            var bord1 = pressed ? DarkBorderColor : LightBorderColor;
            var bord2 = pressed ? LightBorderColor : DarkBorderColor;

            using (var pen = new Pen(bord1, 2))
            {
                e.Graphics.DrawRectangle(pen, rect);

                rect.Width -= 1;
                rect.Height -= 1;

                pen.Color = bord2;
                pen.Width = 1;

                e.Graphics.DrawLine(pen, 0, rect.Height, rect.Width, rect.Height);
                e.Graphics.DrawLine(pen, 1, rect.Height - 1, rect.Width - 1, rect.Height - 1);

                e.Graphics.DrawLine(pen, rect.Width, 0, rect.Width, rect.Height);
                e.Graphics.DrawLine(pen, rect.Width - 1, 1, rect.Width - 1, rect.Height - 1);
            }

            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Inset = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Inset = false;
            base.OnMouseUp(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            Inset = true;
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            Inset = false;
            base.OnLeave(e);
        }

        private Color ParseColorFromString( string RGBColor )
        {
            string[] splitted = RGBColor.Split(new char[] { ',' });

            if (splitted.Length < 3 || splitted.Length > 4)
                throw new ArgumentException("Color values provided is not valid.");

            int[] color = new int[4];

            if (splitted.Length == 3)
            {
                for (int i = 0; i < splitted.Length; i++)
                {
                    try {  color[i] = int.Parse(splitted[i].Trim()); }
                    catch { throw new ArgumentException( "RGB values is invalid." ); }
                }

                return Color.FromArgb(color[0], color[1], color[2]);
            }
            else
            {
                for (int i = 0; i < splitted.Length; i++)
                {
                    try { color[i] = int.Parse(splitted[i].Trim()); }
                    catch { throw new ArgumentException("RGB values is invalid."); }
                }

                return Color.FromArgb(color[0], color[1], color[2], color[3]);
            }
        }
    }
}
