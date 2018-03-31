using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CreateGUIDVSPlugin
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RenewGuidCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4130;

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
        private const string DefaultCommandText = "Renew Guid";

        /// <summary>
        /// control whether an menu item is displayed or not.
        /// </summary>
        private void BeforeQueryStatus(object sender, EventArgs e)
        {
            bool enabled = false;
            OleMenuCommand command = sender as OleMenuCommand;
            if (command != null)
            {
                var dte = this.package.GetDTE();
                var activeDocument = dte.ActiveDocument;
                var selection = (EnvDTE.TextSelection)activeDocument.Selection;
                var seltext = selection.Text;
                if (!string.IsNullOrEmpty(seltext))
                {
                    enabled = true;
                    command.Text = DefaultCommandText;
                }
                else
                {
                    command.Text = DefaultCommandText + " (Select text that you want to replace with new GUIDs.)";
                }

                command.Enabled = enabled;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewGuidCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private RenewGuidCommand(VSPackage package)
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
        public static RenewGuidCommand Instance
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
            Instance = new RenewGuidCommand(package);
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
                var selection = (EnvDTE.TextSelection)activeDocument.Selection;
                var seltext = selection.Text;
                if (!string.IsNullOrEmpty(seltext))
                {
                    var replaceWithNewGuid = new ReplaceWithNewGuid();

                    // Renew GUIDs in the selected text.
                    var output = replaceWithNewGuid.ReplaceNewGuid(seltext);
#if DEBUG
                    this.ClearOutout();
                    this.ActivateOutout();
                    this.OutputString(seltext);
                    this.OutputString(output);
#endif
                    selection.Delete();
                    selection.Insert(output);
                }
            }
        }
    }
}
