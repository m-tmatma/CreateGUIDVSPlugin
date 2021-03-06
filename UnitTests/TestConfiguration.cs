﻿//-----------------------------------------------------------------------
// <copyright file="TestConfiguration.cs" company="Masaru Tsuchiyama">
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

    /// <summary>
    /// unit test for Configuration
    /// </summary>
    [TestFixture]
    public class TestConfiguration
    {
        /// <summary>
        /// top Sub Key
        /// </summary>
        private const string topSubKey = "Software\\CreateGUIDVSPlugin";

        /// <summary>
        /// input data for test1
        /// </summary>
        private const string Input1 = "CreateGUIDVSPlugin.dll";

        /// <summary>
        /// expected output data for test1
        /// </summary>
        private const string Result1 = "CreateGUIDVSPlugin";

        /// <summary>
        /// input data for test2
        /// </summary>
        private const string Input2 = "CreateGUIDVSPlugin";

        /// <summary>
        /// expected output data for test1
        /// </summary>
        private const string Result2 = "CreateGUIDVSPlugin";

        /// <summary>
        /// input data for test1
        /// </summary>
        private const string Input3 = @"C:\path\to\CreateGUIDVSPlugin.dll";

        /// <summary>
        /// expected output data for test1
        /// </summary>
        private const string Result3 = "CreateGUIDVSPlugin";

        /// <summary>
        /// Format String1
        /// </summary>
        private const string FormatString1 = "// {" + "{" + Template.VariableLowerHyphens + "}" + "}";

        /// <summary>
        /// Format String1
        /// </summary>
        private const string FormatString2 = "// {" + "{" + Template.VariableLowerNoHyphens + "}" + "}";

        /// <summary>
        /// Configuration class
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// User registroy key
        /// </summary>
        private RegistryKey userRegistryRoot;

        /// <summary>
        /// setup method to be called to initialize unit tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            DeleteSubKey();
            InitTargetClass();
        }

        /// <summary>
        /// cleanup method to be called to cleanup unit tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            CleanupTargetClass();
            DeleteSubKey();
        }

        /// <summary>
        /// unit test method
        /// </summary>
        /// <see href="https://github.com/nunit/docs/wiki/TestCase-Attribute">TestCase Attribute</see>
        /// <see href="https://github.com/nunit/docs/wiki/TestCaseSource-Attribute">TestCaseSource Attribute</see>
        /// <see href="https://github.com/nunit/docs/wiki/TestCaseData">TestCaseData</see>
        [TestCase(Input1, Result1)]
        [TestCase(Input2, Result2)]
        [TestCase(Input3, Result3)]
        [Category("GetSubKeyName")]
        public void Test_GetSubKeyName(string input, string expected)
        {
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = this.configuration.GetSubKeyName(input);
            Console.WriteLine("output  : " + output);
            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// Get Default Value
        /// </summary>
        [Test]
        public void Test_DefaultValue()
        {
            this.configuration.Load();
            var expected = Template.DefaultFormatString;
            Console.WriteLine("expected: " + expected);

            var output = this.configuration.FormatString;
            Console.WriteLine("output  : " + output);
            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// Set and Get Value
        /// </summary>
        /// <param name="input">input data</param>
        [TestCase(FormatString1)]
        [TestCase(FormatString2)]
        public void Test_SetAndGetValue(string input)
        {
            SetAndGetValue(input, false);
        }

        /// <summary>
        /// Set and Get Value (Re-Initialize Class)
        /// </summary>
        /// <param name="input">input data</param>
        [TestCase(FormatString1)]
        [TestCase(FormatString2)]
        public void Test_SetAndGetValue_ReInit(string input)
        {
            SetAndGetValue(input, true);
        }

        /// <summary>
        /// Set and Get Value Test function
        /// </summary>
        /// <param name="input">input data</param>
        /// <param name="isRecreate">whether to re-create the instance of Configuration class</param>
        private void SetAndGetValue(string input, bool isRecreate)
        {
            Console.WriteLine("input   : " + input);

            // save
            this.configuration.FormatString = input;
            this.configuration.Save();

            if (isRecreate)
            {
                // Re-create the instance of Configuration class

                // release Configuration Class
                CleanupTargetClass();

                // re-create Configuration Class
                InitTargetClass();
            }
            else
            {
                // clear value of Configuration.FormatString
                this.configuration.FormatString = string.Empty;
            }

            // load
            this.configuration.Load();

            // check the result
            var expected = input;
            Console.WriteLine("expected: " + expected);

            var output = this.configuration.FormatString;
            Console.WriteLine("output  : " + output);
            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// Initialize target class
        /// </summary>
        private void InitTargetClass()
        {
            this.userRegistryRoot = Registry.CurrentUser.CreateSubKey(topSubKey, true);
            this.configuration = new Configuration(userRegistryRoot);
        }

        /// <summary>
        /// cleanup target class
        /// </summary>
        private void CleanupTargetClass()
        {
            this.configuration = null;
            this.userRegistryRoot.Close();
        }

        /// <summary>
        /// Delete Sub Key
        /// </summary>
        private void DeleteSubKey()
        {
            var tempKey = Registry.CurrentUser.OpenSubKey(topSubKey);
            if (tempKey != null)
            {
                tempKey.Close();
                Registry.CurrentUser.DeleteSubKeyTree(topSubKey);
            }
        }
    }
}
