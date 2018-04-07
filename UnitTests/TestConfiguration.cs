//-----------------------------------------------------------------------
// <copyright file="TestConfiguration.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Unittest
{
    using System;
    using System.Text;
    using Microsoft.Win32;
    using NUnit.Framework;
    using GuidTools;

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
            this.userRegistryRoot = Registry.CurrentUser.CreateSubKey(topSubKey, true, RegistryOptions.Volatile);
            this.configuration = new Configuration(userRegistryRoot);
        }

        /// <summary>
        /// cleanup method to be called to cleanup unit tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.userRegistryRoot.Close();
            this.configuration = null;

            Registry.CurrentUser.DeleteSubKeyTree(topSubKey);
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
            var output = this.configuration.GetSubKeyName(input);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }
    }
}
