using System.Windows.Forms;

public enum ACResult
{
    BACKUP,
    OVERWRITE,
    IGNORE
};

namespace Half_Life_Background_Changer
{
    public partial class ApplyConfirmation : Form
    {
        public ACResult Result { get; set; } = ACResult.IGNORE;

        public ApplyConfirmation()
        {
            InitializeComponent();
        }

        public static ACResult ShowConfirmation()
        {
            var form = new ApplyConfirmation();
            form.mbBackup.Click += (o, e) =>
            {
                form.Result = ACResult.BACKUP;
                form.Close();
            };

            form.mbOverwrite.Click += (o, e) =>
            {
                form.Result = ACResult.OVERWRITE;
                form.Close();
            };

            form.ShowDialog();

            return form.Result;
        }
    }
}
