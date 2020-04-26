using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Half_Life_Background_Changer
{
    public partial class ULabel : Control
    {
        [Browsable(false)]
        public new bool Enabled { get => base.Enabled; set => base.Enabled = value; }

        private ContentAlignment contentAlignment = ContentAlignment.MiddleLeft;
        public ContentAlignment ContentAlignment { get => contentAlignment; set { contentAlignment = value; Invalidate(); } }

        public override string Text { get => base.Text; set { base.Text = value; Invalidate(); } }

        protected override void OnPaddingChanged(EventArgs e)
        {
            Invalidate();
            base.OnPaddingChanged(e);
        }

        public ULabel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            base.Enabled = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new SolidBrush(ForeColor))
            {
                using (var sf = new StringFormat())
                {
                    switch (contentAlignment)
                    {
                        case ContentAlignment.TopLeft:
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Near;
                            break;

                        case ContentAlignment.TopRight:
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Near;
                            break;

                        case ContentAlignment.TopCenter:
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Near;
                            break;

                        case ContentAlignment.MiddleLeft:
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Center;
                            break;

                        case ContentAlignment.MiddleRight:
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Center;
                            break;

                        case ContentAlignment.MiddleCenter:
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;
                            break;

                        case ContentAlignment.BottomLeft:
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Far;
                            break;

                        case ContentAlignment.BottomRight:
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Far;
                            break;

                        case ContentAlignment.BottomCenter:
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Far;
                            break;
                    }

                    var rect = ClientRectangle;
                    rect.X += Padding.Left;
                    rect.Y += Padding.Top;
                    rect.Width -= Padding.Right;
                    rect.Height -= Padding.Bottom;

                    e.Graphics.DrawString(Text, Font, brush,rect, sf);
                }
            }

            base.OnPaint(e);
        }
    }
}
