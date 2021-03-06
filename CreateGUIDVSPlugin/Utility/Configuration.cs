﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace CreateGUIDVSPlugin.Utility
{
    /// <summary>
    /// Configuration and setting manager
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// root registry key for user setting
        /// </summary>
        private RegistryKey UserRegistryRoot;

        /// <summary>
        /// Registry SubKey for the setting
        /// </summary>
        private string subKeyName;

        /// <summary>
        /// Registry Value Name for FormatString setting
        /// </summary>
        private const string ValueNameFormatString = "FormatString";

        /// <summary>
        /// Property for Template
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Property for getting singleton instance
        /// </summary>
        /// <param name="userRegistryRoot">root Registry for User setting</param>
        public Configuration(RegistryKey userRegistryRoot)
        {
            var assembly = Assembly.GetAssembly(typeof(Configuration));
            var manifestModule = assembly.ManifestModule.Name;
            var subKeyName = GetSubKeyName(manifestModule);
            this.subKeyName = subKeyName;
            this.UserRegistryRoot = userRegistryRoot;
        }
 
        /// <summary>
        /// Load setting from registry
        /// </summary>
        public void Load()
        {
            var DefaultFormatString = Template.DefaultFormatString;
            this.FormatString = DefaultFormatString;
            try
            {
                using (RegistryKey subKey = this.UserRegistryRoot.OpenSubKey(this.subKeyName))
                {
                    var value = (string)subKey.GetValue(ValueNameFormatString, DefaultFormatString);
                    this.FormatString = value;
                }
            }
            catch (NullReferenceException)
            {
                ;
            }
        }

        /// <summary>
        /// Store setting to registry
        /// </summary>
        public void Save()
        {
            try
            {
                using (RegistryKey subKey = this.UserRegistryRoot.CreateSubKey(this.subKeyName))
                {
                    subKey.SetValue(ValueNameFormatString, this.FormatString);
                }
            }
            catch (NullReferenceException)
            {
                ;
            }
        }

        /// <summary>
        /// Get SubKeyName from Assembly Path name
        /// </summary>
        /// <param name="manifestModule"></param>
        /// <returns>subKey Name</returns>
        internal string GetSubKeyName(string manifestModule)
        {
            var baseName = Path.GetFileName(manifestModule);
            var index = baseName.LastIndexOf('.');
            if (index >= 0)
            {
                var subKeyName = baseName.Substring(0, index);
                return subKeyName;
            }
            else
            {
                return baseName;
            }
        }
    }
}
