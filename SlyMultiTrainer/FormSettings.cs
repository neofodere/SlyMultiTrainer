namespace SlyMultiTrainer
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            var properties = typeof(Util.Controller_t).GetProperties().Select(p => p.Name).ToArray();
            cmbFlyUp.Items.AddRange(properties);
            cmbFlyDown.Items.AddRange(properties);
            cmbFlyAccelerate.Items.AddRange(properties);
            ReadSettings();
        }

        private void ReadSettings()
        {
            cmbFlyUp.SelectedItem = Properties.Settings.Default.FlyButtonUp;
            cmbFlyDown.SelectedItem = Properties.Settings.Default.FlyButtonDown;
            cmbFlyAccelerate.SelectedItem = Properties.Settings.Default.FlyButtonAccelerate;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.FlyButtonUp = cmbFlyUp.SelectedItem?.ToString();
            Properties.Settings.Default.FlyButtonDown = cmbFlyDown.SelectedItem?.ToString();
            Properties.Settings.Default.FlyButtonAccelerate = cmbFlyAccelerate.SelectedItem?.ToString();
            Properties.Settings.Default.Save();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MessageBox.Show("The settings were successfully saved!", "Sly Multi Trainer - Settings saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                return;
            }
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to reset the settings to their default values?", "Sly Multi Trainer - Reset Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                ReadSettings();
                MessageBox.Show("The settings have been reset to their default values.", "Sly Multi Trainer - Reset Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
