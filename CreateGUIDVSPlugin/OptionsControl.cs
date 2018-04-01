﻿//-----------------------------------------------------------------------
// <copyright file="OptionsControl.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CreateGUIDVSPlugin
{
    using System;
    using System.Windows.Forms;

    public partial class OptionsControl : UserControl
    {
        private OptionsPageSetting settingOptionsPage;
 
        public OptionsControl()
        {
            this.InitializeComponent();

            foreach (Template.VariableManager variableName in Template.Variables)
            {
                this.comboBoxVariable.Items.Add(variableName.Description);
            }

            this.comboBoxVariable.SelectedIndex = 0;
        }

        /// <summary>
        /// Gets or sets the reference to the underlying OptionsPage object.
        /// </summary>
        public OptionsPageSetting OptionsPage
        {
            get
            {
                return this.settingOptionsPage;
            }

            set
            {
                this.settingOptionsPage = value;
            }
        }

        /// <summary>
        /// Save the setting to the registry
        /// </summary>
        /// <param name="configuration">instance of Configuration</param>
        public void SaveSetting(Configuration configuration)
        {
            configuration.FormatString = this.textBoxTemplate.Text;
            configuration.Save();
        }

        /// <summary>
        /// Load the setting from the registry
        /// </summary>
        /// <param name="configuration">instance of Configuration</param>
        public void LoadSetting(Configuration configuration)
        {
            this.textBoxTemplate.Text = configuration.FormatString;
        }

        private void ButtonInsert_Click(object sender, EventArgs e)
        {
            var guidIndex = decimal.ToInt32(this.numericUpDownGUID.Value);
            var keyname = Template.Variables[this.comboBoxVariable.SelectedIndex];
            var variable = (guidIndex > 0) ? keyname.GetVariable(guidIndex) : keyname.GetVariable();
            var textBox = this.textBoxTemplate;
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, variable);
        }

        private void ButtonInsertLineEnding_Click(object sender, EventArgs e)
        {
            var textBox = this.textBoxTemplate;
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, Environment.NewLine);
        }

        private void ButtonSetDefault_Click(object sender, EventArgs e)
        {
            var textBox = this.textBoxTemplate;
            textBox.Text = Template.DefaultFormatString;
        }
    }
}
