//-----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CreateGUIDVSPlugin
{
    using System;
    using Microsoft.Win32;

    /// <summary>
    /// Configuration and setting manager
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Registry SubKey for the setting
        /// </summary>
        private const string SubKeyName = @"CreateGUIDVSPlugin";

        /// <summary>
        /// Registry Value Name for FormatString setting
        /// </summary>
        private const string ValueNameFormatString = "FormatString";

        /// <summary>
        /// root registry key for user setting
        /// </summary>
        private RegistryKey userRegistryRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// Property for getting singleton instance
        /// </summary>
        /// <param name="userRegistryRoot">root Registry for User setting</param>
        public Configuration(RegistryKey userRegistryRoot)
        {
            this.userRegistryRoot = userRegistryRoot;
        }

        /// <summary>
        /// Gets or sets Template string.
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Load setting from registry
        /// </summary>
        public void Load()
        {
            var defaultFormatString = Template.DefaultFormatString;
            this.FormatString = defaultFormatString;
            try
            {
                using (RegistryKey subKey = this.userRegistryRoot.OpenSubKey(SubKeyName))
                {
                    var value = (string)subKey.GetValue(ValueNameFormatString, defaultFormatString);
                    this.FormatString = value;
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// Store setting to registry
        /// </summary>
        public void Save()
        {
            try
            {
                using (RegistryKey subKey = this.userRegistryRoot.CreateSubKey(SubKeyName))
                {
                    subKey.SetValue(ValueNameFormatString, this.FormatString);
                }
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}
