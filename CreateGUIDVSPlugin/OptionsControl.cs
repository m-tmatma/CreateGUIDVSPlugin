using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CreateGUIDVSPlugin.Utility;

namespace CreateGUIDVSPlugin
{
    public partial class OptionsControl : UserControl
    {
        private OptionsPageSetting settingOptionsPage;
        private int insertPos;

        public OptionsControl()
        {
            InitializeComponent();

            foreach(VariableManager variableName in Template.Variables)
            {
                this.comboBoxVariable.Items.Add(variableName.Description);
            }
            this.comboBoxVariable.SelectedIndex = 0;

            this.insertPos = 0;
            this.textBoxTemplate.LostFocus += new EventHandler(textBoxTemplate_LostFocus);
        }

        /// <summary>
        /// Gets or sets the reference to the underlying OptionsPage object.
        /// </summary>
        public OptionsPageSetting OptionsPage
        {
            get
            {
                return settingOptionsPage;
            }
            set
            {
                settingOptionsPage = value;
            }
        }

        /// <summary>
        /// Save the setting to the registry
        /// </summary>
        /// <param name="configuration"></param>
        public void SaveSetting(Configuration configuration)
        {
            configuration.FormatString = this.textBoxTemplate.Text;
            configuration.Save();
        }

        /// <summary>
        /// Load the setting from the registry
        /// </summary>
        /// <param name="configuration"></param>
        public void LoadSetting(Configuration configuration)
        {
            SetText(configuration.FormatString);
        }

        /// <summary>
        /// handler clicking 'Insert' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            var guidIndex = decimal.ToInt32(this.numericUpDownGUID.Value);
            var keyname = Template.Variables[this.comboBoxVariable.SelectedIndex];
            var variable = (guidIndex > 0 )? keyname.GetVariable(guidIndex) : keyname.GetVariable();
            InsertText(variable);
        }

        /// <summary>
        /// handler clicking 'EOL' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInsertLineEnding_Click(object sender, EventArgs e)
        {
            InsertText(Environment.NewLine);
        }

        /// <summary>
        /// handler clicking '{' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLeftBrace_Click(object sender, EventArgs e)
        {
            InsertText("{{");
        }

        /// <summary>
        /// handler clicking '}' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRightBrace_Click(object sender, EventArgs e)
        {
            InsertText("}}");
        }

        /// <summary>
        /// hander clicking 'Default' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetDefault_Click(object sender, EventArgs e)
        {
            SetText(Template.DefaultFormatString);
        }

        /// <summary>
        /// handler that the textbox loses focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxTemplate_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this.insertPos = textBox.SelectionStart;
        }

        /// <summary>
        /// insert a text to the textbox
        /// </summary>
        /// <param name="text"></param>
        private void InsertText(string text)
        {
            var textBox = this.textBoxTemplate;
            textBox.Text = textBox.Text.Insert(this.insertPos, text);
            this.insertPos += text.Length;

            ScrollToCarect();
        }

        /// <summary>
        /// overwrite textbox content
        /// </summary>
        /// <param name="text"></param>
        private void SetText(string text)
        {
            var textBox = this.textBoxTemplate;
            textBox.Text = text;
            this.insertPos = text.Length;

            ScrollToCarect();
        }

        /// <summary>
        /// function to scroll the textbox to the caret position
        /// </summary>
        private void ScrollToCarect()
        {
            var textBox = this.textBoxTemplate;
            textBox.SelectionStart = this.insertPos;
            textBox.Focus();
            textBox.ScrollToCaret();
        }
    }
}
