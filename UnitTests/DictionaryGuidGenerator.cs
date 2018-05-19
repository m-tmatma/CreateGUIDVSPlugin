//-----------------------------------------------------------------------
// <copyright file="DictionaryGuidGenerator.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitTests
{
    using System;
    using System.Text;
    using Microsoft.Win32;
    using NUnit.Framework;
    using CreateGUIDVSPlugin.Utility;
    using System.Collections.Generic;

    /// <summary>
    /// DictionaryGuidGenerator
    /// </summary>
    internal class DictionaryGuidGenerator
    {
        /// <summary>
        /// guid generator
        /// </summary>
        private TestTemplate.NewGuid newGuid;

        /// <summary>
        /// dictionary to manage GUIDs.
        /// </summary>
        private Dictionary<int, Guid> guidDictionary;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="newGuid"></param>
        internal DictionaryGuidGenerator(TestTemplate.NewGuid newGuid)
        {
            this.newGuid = newGuid;
            this.guidDictionary = new Dictionary<int, Guid>();
        }

        /// <summary>
        /// Get GUID
        /// </summary>
        /// <param name="index">GUID index</param>
        /// <returns></returns>
        internal Guid GetGuid(int index)
        {
            if (!this.guidDictionary.ContainsKey(index))
            {
                this.guidDictionary[index] = this.newGuid();
            }
            return this.guidDictionary[index];
        }
    }
}
