﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CreateGUIDVSPlugin
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CopyGuidCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("21faf2f8-6789-4d5a-bda3-c063d68e1b7d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly VSPackage package;


        /// <summary>
        /// control whether an menu item is displayed or not.
        /// </summary>
        private void BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand command = sender as OleMenuCommand;
            if (command != null)
            {
                var dte = this.package.GetDTE();
                var activeDocument = dte.ActiveDocument;

                command.Visible = true;
            }
        }
 
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyGuidCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private CopyGuidCommand(VSPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += this.BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CopyGuidCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(VSPackage package)
        {
            Instance = new CopyGuidCommand(package);
        }

#if DEBUG
        /// <summary>
        /// Print to Output Window
        /// </summary>
        internal void OutputString(string output)
        {
            var outPutPane = this.package.OutputPane;
            outPutPane.OutputString(output);
        }

        /// <summary>
        /// Print to Output Window with Line Ending
        /// </summary>
        internal void OutputStringLine(string output)
        {
            OutputString(output + Environment.NewLine);
        }

        /// <summary>
        /// Clear Output Window
        /// </summary>
        internal void ClearOutout()
        {
            var outPutPane = this.package.OutputPane;
            outPutPane.Clear();
        }

        /// <summary>
        /// Clear Output Window
        /// </summary>
        internal void ActivateOutout()
        {
            var outPutPane = this.package.OutputPane;
            outPutPane.Activate();
        }
#endif

        /// <summary>
        /// Create a dictionary for the template values
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateValuesDictionary(Guid guid)
        {
            var values = new Dictionary<string, string>();
            foreach (VariableManager variableManager in Template.Variables)
            {
                values[variableManager.Variable] = string.Empty;
            }

            //
            // see https://msdn.microsoft.com/library/97af8hh4(v=vs.110).aspx about the parameter of Guid.ToString().
            // see https://msdn.microsoft.com/library/system.guid(v=vs.110).aspx
            // see 'Reference Source' link in the above site.
            //
            var lowerWithHyphens = guid.ToString("D");
            values[Template.VariableLowerCaseGuidWithHyphens] = lowerWithHyphens;
            values[Template.VariableUpperCaseGuidWithHyphens] = lowerWithHyphens.ToUpper();

            var lowerWithoutHyphens = guid.ToString("N");
            values[Template.VariableLowerCaseGuidWithoutHyphens] = lowerWithoutHyphens;
            values[Template.VariableUpperCaseGuidWithoutHyphens] = lowerWithoutHyphens.ToUpper();
            return values;
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var dte = this.package.GetDTE();
            var activeDocument = dte.ActiveDocument;

            if (activeDocument != null)
            {
                var guid = Guid.NewGuid();
                var values = CreateValuesDictionary(guid);
                var configuration = this.package.GetConfiguration();
                var formatString = configuration.FormatString;

                var copyString = Template.ProcessTemplate(formatString, values);
#if DEBUG
                this.ClearOutout();
                this.ActivateOutout();
                this.OutputString(copyString);
#endif
                Clipboard.SetDataObject(copyString);
            }
        }
    }
}
