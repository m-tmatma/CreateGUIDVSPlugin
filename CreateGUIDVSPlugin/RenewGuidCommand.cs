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
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
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
                if ( !string.IsNullOrEmpty(seltext))
                {
                    var replaceWithNewGuid = new ReplaceWithNewGuid();

                    // Renew GUIDs in the selected text.
                    var output = replaceWithNewGuid.ReplaceNewGuid(seltext);
                    selection.Delete();
                    selection.Insert(output);
                }
            }
        }
    }
}
