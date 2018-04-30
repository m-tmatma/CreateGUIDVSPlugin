namespace CreateGUIDVSPlugin
{
    partial class OptionsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxTemplate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVariable = new System.Windows.Forms.ComboBox();
            this.buttonInsert = new System.Windows.Forms.Button();
            this.buttonInsertLineEnding = new System.Windows.Forms.Button();
            this.buttonSetDefault = new System.Windows.Forms.Button();
            this.numericUpDownGUID = new System.Windows.Forms.NumericUpDown();
            this.buttonLeftBrace = new System.Windows.Forms.Button();
            this.buttonRightBrace = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGUID)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxTemplate
            // 
            this.textBoxTemplate.Location = new System.Drawing.Point(40, 120);
            this.textBoxTemplate.Multiline = true;
            this.textBoxTemplate.Name = "textBoxTemplate";
            this.textBoxTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxTemplate.Size = new System.Drawing.Size(611, 257);
            this.textBoxTemplate.TabIndex = 0;
            this.textBoxTemplate.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Template for copying string";
            // 
            // comboBoxVariable
            // 
            this.comboBoxVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVariable.FormattingEnabled = true;
            this.comboBoxVariable.Location = new System.Drawing.Point(40, 26);
            this.comboBoxVariable.Name = "comboBoxVariable";
            this.comboBoxVariable.Size = new System.Drawing.Size(425, 29);
            this.comboBoxVariable.TabIndex = 2;
            // 
            // buttonInsert
            // 
            this.buttonInsert.Location = new System.Drawing.Point(576, 26);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(75, 29);
            this.buttonInsert.TabIndex = 3;
            this.buttonInsert.Text = "Insert";
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // buttonInsertLineEnding
            // 
            this.buttonInsertLineEnding.Location = new System.Drawing.Point(551, 77);
            this.buttonInsertLineEnding.Name = "buttonInsertLineEnding";
            this.buttonInsertLineEnding.Size = new System.Drawing.Size(100, 29);
            this.buttonInsertLineEnding.TabIndex = 4;
            this.buttonInsertLineEnding.Text = "EOL";
            this.buttonInsertLineEnding.UseVisualStyleBackColor = true;
            this.buttonInsertLineEnding.Click += new System.EventHandler(this.buttonInsertLineEnding_Click);
            // 
            // buttonSetDefault
            // 
            this.buttonSetDefault.Location = new System.Drawing.Point(433, 77);
            this.buttonSetDefault.Name = "buttonSetDefault";
            this.buttonSetDefault.Size = new System.Drawing.Size(100, 29);
            this.buttonSetDefault.TabIndex = 5;
            this.buttonSetDefault.Text = "Default";
            this.buttonSetDefault.UseVisualStyleBackColor = true;
            this.buttonSetDefault.Click += new System.EventHandler(this.buttonSetDefault_Click);
            // 
            // numericUpDownGUID
            // 
            this.numericUpDownGUID.Location = new System.Drawing.Point(471, 26);
            this.numericUpDownGUID.Name = "numericUpDownGUID";
            this.numericUpDownGUID.Size = new System.Drawing.Size(99, 29);
            this.numericUpDownGUID.TabIndex = 6;
            // 
            // buttonLeftBrace
            // 
            this.buttonLeftBrace.Location = new System.Drawing.Point(294, 77);
            this.buttonLeftBrace.Name = "buttonLeftBrace";
            this.buttonLeftBrace.Size = new System.Drawing.Size(55, 29);
            this.buttonLeftBrace.TabIndex = 7;
            this.buttonLeftBrace.Text = "{";
            this.buttonLeftBrace.UseVisualStyleBackColor = true;
            this.buttonLeftBrace.Click += new System.EventHandler(this.buttonLeftBrace_Click);
            // 
            // buttonRightBrace
            // 
            this.buttonRightBrace.Location = new System.Drawing.Point(372, 77);
            this.buttonRightBrace.Name = "buttonRightBrace";
            this.buttonRightBrace.Size = new System.Drawing.Size(55, 29);
            this.buttonRightBrace.TabIndex = 8;
            this.buttonRightBrace.Text = "}";
            this.buttonRightBrace.UseVisualStyleBackColor = true;
            this.buttonRightBrace.Click += new System.EventHandler(this.buttonRightBrace_Click);
            // 
            // OptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonRightBrace);
            this.Controls.Add(this.buttonLeftBrace);
            this.Controls.Add(this.numericUpDownGUID);
            this.Controls.Add(this.buttonSetDefault);
            this.Controls.Add(this.buttonInsertLineEnding);
            this.Controls.Add(this.buttonInsert);
            this.Controls.Add(this.comboBoxVariable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTemplate);
            this.Name = "OptionsControl";
            this.Size = new System.Drawing.Size(679, 405);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGUID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxVariable;
        private System.Windows.Forms.Button buttonInsert;
        private System.Windows.Forms.Button buttonInsertLineEnding;
        private System.Windows.Forms.Button buttonSetDefault;
        private System.Windows.Forms.NumericUpDown numericUpDownGUID;
        private System.Windows.Forms.Button buttonLeftBrace;
        private System.Windows.Forms.Button buttonRightBrace;
    }
}
