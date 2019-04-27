namespace VisualDump.Controls
{
    partial class OptionControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupDumpWindow = new System.Windows.Forms.GroupBox();
            this.checkClear = new System.Windows.Forms.CheckBox();
            this.labelClear = new System.Windows.Forms.Label();
            this.buttOpen = new System.Windows.Forms.Button();
            this.comboThemes = new System.Windows.Forms.ComboBox();
            this.labelTheme = new System.Windows.Forms.Label();
            this.groupDumpWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupDumpWindow
            // 
            this.groupDumpWindow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDumpWindow.Controls.Add(this.checkClear);
            this.groupDumpWindow.Controls.Add(this.labelClear);
            this.groupDumpWindow.Controls.Add(this.buttOpen);
            this.groupDumpWindow.Controls.Add(this.comboThemes);
            this.groupDumpWindow.Controls.Add(this.labelTheme);
            this.groupDumpWindow.Location = new System.Drawing.Point(4, 4);
            this.groupDumpWindow.Name = "groupDumpWindow";
            this.groupDumpWindow.Size = new System.Drawing.Size(397, 79);
            this.groupDumpWindow.TabIndex = 0;
            this.groupDumpWindow.TabStop = false;
            this.groupDumpWindow.Text = "Tool Window";
            // 
            // checkClear
            // 
            this.checkClear.AutoSize = true;
            this.checkClear.Checked = true;
            this.checkClear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkClear.Location = new System.Drawing.Point(97, 55);
            this.checkClear.Name = "checkClear";
            this.checkClear.Size = new System.Drawing.Size(15, 14);
            this.checkClear.TabIndex = 5;
            this.checkClear.UseVisualStyleBackColor = true;
            this.checkClear.CheckedChanged += new System.EventHandler(this.CheckClear_CheckedChanged);
            // 
            // labelClear
            // 
            this.labelClear.AutoSize = true;
            this.labelClear.Location = new System.Drawing.Point(33, 55);
            this.labelClear.Name = "labelClear";
            this.labelClear.Size = new System.Drawing.Size(58, 13);
            this.labelClear.TabIndex = 4;
            this.labelClear.Text = "Auto clear:";
            // 
            // buttOpen
            // 
            this.buttOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttOpen.Location = new System.Drawing.Point(370, 22);
            this.buttOpen.Name = "buttOpen";
            this.buttOpen.Size = new System.Drawing.Size(21, 21);
            this.buttOpen.TabIndex = 2;
            this.buttOpen.Text = "📂";
            this.buttOpen.UseVisualStyleBackColor = true;
            this.buttOpen.Click += new System.EventHandler(this.ButtOpen_Click);
            // 
            // comboThemes
            // 
            this.comboThemes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboThemes.FormattingEnabled = true;
            this.comboThemes.Location = new System.Drawing.Point(97, 22);
            this.comboThemes.Name = "comboThemes";
            this.comboThemes.Size = new System.Drawing.Size(267, 21);
            this.comboThemes.TabIndex = 1;
            this.comboThemes.SelectedIndexChanged += new System.EventHandler(this.ComboThemes_SelectedIndexChanged);
            // 
            // labelTheme
            // 
            this.labelTheme.AutoSize = true;
            this.labelTheme.Location = new System.Drawing.Point(7, 25);
            this.labelTheme.Name = "labelTheme";
            this.labelTheme.Size = new System.Drawing.Size(84, 13);
            this.labelTheme.TabIndex = 0;
            this.labelTheme.Text = "Selected theme:";
            // 
            // OptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupDumpWindow);
            this.Name = "OptionControl";
            this.Size = new System.Drawing.Size(404, 263);
            this.VisibleChanged += new System.EventHandler(this.OptionControl_VisibleChanged);
            this.groupDumpWindow.ResumeLayout(false);
            this.groupDumpWindow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupDumpWindow;
        private System.Windows.Forms.Button buttOpen;
        private System.Windows.Forms.ComboBox comboThemes;
        private System.Windows.Forms.Label labelTheme;
        private System.Windows.Forms.CheckBox checkClear;
        private System.Windows.Forms.Label labelClear;
    }
}
