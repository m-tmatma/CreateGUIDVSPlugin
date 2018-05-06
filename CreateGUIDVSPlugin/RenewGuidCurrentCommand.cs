using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using CreateGUIDVSPlugin.Utility;

namespace CreateGUIDVSPlugin
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RenewGuidCurrentCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4131;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("21faf2f8-6789-4d5a-bda3-c063d68e1b7d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly VSPackage package;

        /// <summary>
        /// default command text
        /// </summary>
        private const string DefaultCommandText = "Renew Guid for current document";

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
                if (activeDocument != null)
                {
                    command.Enabled = true;
                    command.Text = DefaultCommandText;
                }
                else
                {
                    command.Enabled = false;
                    command.Text = DefaultCommandText + " (activate document to enable this command)";
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewGuidCurrentCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private RenewGuidCurrentCommand(VSPackage package)
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
        public static RenewGuidCurrentCommand Instance
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
            Instance = new RenewGuidCurrentCommand(package);
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
                var configuration = this.package.GetConfiguration();
                var formatString = configuration.FormatString;

                try
                {
                    var copyString = Template.Process(formatString);
                    var textView = this.package.GetTextView();
                    if (textView != null)
                    {
                        if (textView.HasAggregateFocus)
                        {
                            // Insert GUID
                            textView.TextBuffer.Insert(textView.Caret.Position.BufferPosition, copyString);
                        }
                    }
                }
                catch (ProcessTemplate.OrphanedLeftBraceException ex)
                {
                    var message = "found orphaned '{' at " + ex.Index.ToString() + " in template";
                    var caption = "Error";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK);
                }
                catch (ProcessTemplate.OrphanedRightBraceException ex)
                {
                    var message = "found orphaned '}' at " + ex.Index.ToString() + " in template";
                    var caption = "Error";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK);
                }
            }
        }
    }
}
