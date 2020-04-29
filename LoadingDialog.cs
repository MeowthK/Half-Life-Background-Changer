using System.Windows.Forms;

namespace Half_Life_Background_Changer
{
    public partial class LoadingDialog : Form
    {
        public int LoadPercent { get => Progress.Value;
            set {
                Progress.Value = value;

                if (value >= 100)
                    Close();
            } }

        public LoadingDialog()
        {
            InitializeComponent();
        }
    }
}
