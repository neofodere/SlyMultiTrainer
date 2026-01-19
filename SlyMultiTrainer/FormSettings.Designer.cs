namespace SlyMultiTrainer
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOK = new Button();
            btnCancel = new Button();
            cmbFlyUp = new ComboBox();
            label1 = new Label();
            grpClock = new GroupBox();
            label3 = new Label();
            cmbFlyAccelerate = new ComboBox();
            label2 = new Label();
            cmbFlyDown = new ComboBox();
            btnResetSettings = new Button();
            tabControl1 = new TabControl();
            tabSettingsMain = new TabPage();
            grpClock.SuspendLayout();
            tabControl1.SuspendLayout();
            tabSettingsMain.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.AutoSize = true;
            btnOK.Location = new Point(477, 316);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 25);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.AutoSize = true;
            btnCancel.Location = new Point(558, 316);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 25);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cmbFlyUp
            // 
            cmbFlyUp.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFlyUp.Location = new Point(74, 25);
            cmbFlyUp.Name = "cmbFlyUp";
            cmbFlyUp.Size = new Size(86, 21);
            cmbFlyUp.TabIndex = 36;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 28);
            label1.Name = "label1";
            label1.Size = new Size(21, 13);
            label1.TabIndex = 37;
            label1.Text = "Up";
            // 
            // grpClock
            // 
            grpClock.BackColor = Color.Transparent;
            grpClock.Controls.Add(label3);
            grpClock.Controls.Add(cmbFlyAccelerate);
            grpClock.Controls.Add(label2);
            grpClock.Controls.Add(cmbFlyDown);
            grpClock.Controls.Add(label1);
            grpClock.Controls.Add(cmbFlyUp);
            grpClock.Location = new Point(6, 6);
            grpClock.Name = "grpClock";
            grpClock.Size = new Size(169, 118);
            grpClock.TabIndex = 38;
            grpClock.TabStop = false;
            grpClock.Text = "Fly";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 86);
            label3.Name = "label3";
            label3.Size = new Size(58, 13);
            label3.TabIndex = 41;
            label3.Text = "Accelerate";
            // 
            // cmbFlyAccelerate
            // 
            cmbFlyAccelerate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFlyAccelerate.Location = new Point(74, 83);
            cmbFlyAccelerate.Name = "cmbFlyAccelerate";
            cmbFlyAccelerate.Size = new Size(86, 21);
            cmbFlyAccelerate.TabIndex = 40;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 57);
            label2.Name = "label2";
            label2.Size = new Size(35, 13);
            label2.TabIndex = 39;
            label2.Text = "Down";
            // 
            // cmbFlyDown
            // 
            cmbFlyDown.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFlyDown.Location = new Point(74, 54);
            cmbFlyDown.Name = "cmbFlyDown";
            cmbFlyDown.Size = new Size(86, 21);
            cmbFlyDown.TabIndex = 38;
            // 
            // btnResetSettings
            // 
            btnResetSettings.AutoSize = true;
            btnResetSettings.BackColor = SystemColors.Control;
            btnResetSettings.FlatAppearance.BorderSize = 0;
            btnResetSettings.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnResetSettings.ForeColor = SystemColors.ControlText;
            btnResetSettings.Location = new Point(12, 316);
            btnResetSettings.Name = "btnResetSettings";
            btnResetSettings.Size = new Size(89, 25);
            btnResetSettings.TabIndex = 39;
            btnResetSettings.Text = "Reset settings";
            btnResetSettings.UseVisualStyleBackColor = true;
            btnResetSettings.Click += btnResetSettings_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabSettingsMain);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(645, 310);
            tabControl1.TabIndex = 44;
            // 
            // tabSettingsMain
            // 
            tabSettingsMain.Controls.Add(grpClock);
            tabSettingsMain.Location = new Point(4, 22);
            tabSettingsMain.Name = "tabSettingsMain";
            tabSettingsMain.Padding = new Padding(3);
            tabSettingsMain.Size = new Size(637, 284);
            tabSettingsMain.TabIndex = 0;
            tabSettingsMain.Text = "Main";
            tabSettingsMain.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            ClientSize = new Size(645, 351);
            Controls.Add(tabControl1);
            Controls.Add(btnResetSettings);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Font = new Font("Microsoft Sans Serif", 8F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sly Multi Trainer - Settings";
            FormClosing += FormSettings_FormClosing;
            Load += FormSettings_Load;
            grpClock.ResumeLayout(false);
            grpClock.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabSettingsMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        public ComboBox cmbFlyUp;
        private Label label1;
        private GroupBox grpClock;
        private Label label3;
        public ComboBox cmbFlyAccelerate;
        private Label label2;
        public ComboBox cmbFlyDown;
        public Button btnResetSettings;
        private TabControl tabControl1;
        private TabPage tabSettingsMain;
    }
}