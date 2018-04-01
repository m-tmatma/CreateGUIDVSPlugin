//-----------------------------------------------------------------------
// <copyright file="OptionsControl.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CreateGUIDVSPlugin
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// class for Control of user settings
    /// </summary>
    public partial class OptionsControl : UserControl
    {
        /// <summary>
        /// instance of dialog page
        /// </summary>
        private OptionsPageSetting settingOptionsPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsControl" /> class.
        /// Control for user settings.
        /// </summary>
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

        /// <summary>
        /// handler for clicking 'Insert' Button
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args.</param>
        private void ButtonInsert_Click(object sender, EventArgs e)
        {
            var guidIndex = decimal.ToInt32(this.numericUpDownGUID.Value);
            var keyname = Template.Variables[this.comboBoxVariable.SelectedIndex];
            var variable = (guidIndex > 0) ? keyname.GetVariable(guidIndex) : keyname.GetVariable();
            var textBox = this.textBoxTemplate;
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, variable);
        }

        /// <summary>
        /// handler for clicking 'End Of line' Button
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args.</param>
        private void ButtonInsertLineEnding_Click(object sender, EventArgs e)
        {
            var textBox = this.textBoxTemplate;
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, Environment.NewLine);
        }

        /// <summary>
        /// handler for clicking 'Set Default' Button
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args.</param>
        private void ButtonSetDefault_Click(object sender, EventArgs e)
        {
            var textBox = this.textBoxTemplate;
            textBox.Text = Template.DefaultFormatString;
        }
    }
}
