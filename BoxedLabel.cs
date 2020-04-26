using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CSCSCH
{
    public partial class BoxedLabel : Label
    {
        private Color highlightColor = Color.Orange;
        private bool highlighted = false;

        [Description("This Label's Highlight Color."), Category("Highlights")]
        public Color HighlightColor { get => highlightColor; set { highlightColor = value; Invalidate(); } }
        [Description("This Label's Highlight State."), Category("Highlights")]
        public bool Highlighted { get => highlighted; set { highlighted = value; Invalidate(); } }

        public BoxedLabel()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (highlighted)
            {
                using (var pen = new Pen(HighlightColor, 1.5f))
                    e.Graphics.DrawRectangle(pen, 0, 0, Width - 1.5f, Height - 1.5f);
            }

            base.OnPaint(e);
        }
    }
}
