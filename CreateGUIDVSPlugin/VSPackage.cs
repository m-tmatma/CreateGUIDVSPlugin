//-----------------------------------------------------------------------
// <copyright file="VSPackage.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CreateGUIDVSPlugin
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids.SolutionExists)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(VSPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPageAttribute(typeof(OptionsPageSetting), "CreateGUIDVSPlugin", "Setting", 0, 0, true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class VSPackage : Package
    {
        /// <summary>
        /// VSPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "bf2a5c33-ab8b-45f6-9ab0-1e3288dd0fe5";

        /// <summary>
        /// configuration class
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="VSPackage"/> class.
        /// </summary>
        public VSPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        /// <summary>
        /// Gets instance of OutputWindow
        /// </summary>
        public EnvDTE.OutputWindowPane OutputPane { get; private set; }

        /// <summary>
        /// Get DTE Object
        /// </summary>
        /// <returns>DTE Object</returns>
        public EnvDTE.DTE GetDTE()
        {
            return (EnvDTE.DTE)GetService(typeof(SDTE));
        }

        public IVsTextView GetCurrentNativeTextView()
        {
            var textManager = (IVsTextManager)GetService(typeof(SVsTextManager));

            ErrorHandler.ThrowOnFailure(textManager.GetActiveView(1, null, out IVsTextView activeView));
            return activeView;
        }

        public IWpfTextView GetTextView()
        {
            var compService = GetService(typeof(SComponentModel)) as IComponentModel;
            var editorAdapter = compService.GetService<IVsEditorAdaptersFactoryService>();
            return editorAdapter.GetWpfTextView(GetCurrentNativeTextView());
        }

#if DEBUG
        /// <summary>
        /// Get OutputWindow
        /// </summary>
        /// <param name="dte">DTE object</param>
        /// <returns>instance of OutputWindow</returns>
        public EnvDTE.Window GetOutputWindow(EnvDTE.DTE dte)
        {
            return dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
        }

        /// <summary>
        /// Add an item to OutputWindow
        /// </summary>
        /// <param name="paneName">window pane name</param>
        public void AddOutputWindow(string paneName)
        {
            var outputWindow = (EnvDTE.OutputWindow)GetOutputWindow(GetDTE()).Object;
            this.OutputPane = outputWindow.OutputWindowPanes.Add(paneName);
        }
#endif

        /// <summary>
        /// Get Instance of Configuration
        /// </summary>
        /// <returns>instance of Configuration</returns>
        public Configuration GetConfiguration()
        {
            return configuration;
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            CopyGuidCommand.Initialize(this);
            InsertGuidCommand.Initialize(this);
            RenewGuidCommand.Initialize(this);

#if DEBUG
            AddOutputWindow("Create GUID");
#endif

            this.configuration = new Configuration(this.UserRegistryRoot);
            this.configuration.Load();
        }

        #endregion
    }
}
