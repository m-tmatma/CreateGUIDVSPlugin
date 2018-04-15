//-----------------------------------------------------------------------
// <copyright file="TestTemplate.cs" company="Masaru Tsuchiyama">
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
    /// unit test for Template
    /// </summary>
    [TestFixture]
    public class TestTemplate
    {
        /// <summary>
        /// delegate for creating new GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal delegate Guid NewGuid();

        /// <summary>
        /// GUID generator class to make unit-testing easier
        /// </summary>
        private GuidGenerater guidGenerator;

        /// <summary>
        /// All Variable
        /// </summary>
        private static readonly string[] AllVariableNames = new string[]
        {
            Template.VariableLowerCase1stPart,
            Template.VariableLowerCase2ndPart,
            Template.VariableLowerCase3rdPart,
            Template.VariableLowerCaseBytesPartByte1,
            Template.VariableLowerCaseBytesPartByte2,
            Template.VariableLowerCaseBytesPartByte3,
            Template.VariableLowerCaseBytesPartByte4,
            Template.VariableLowerCaseBytesPartByte5,
            Template.VariableLowerCaseBytesPartByte6,
            Template.VariableLowerCaseBytesPartByte7,
            Template.VariableLowerCaseBytesPartByte8,
            Template.VariableUpperCase1stPart,
            Template.VariableUpperCase2ndPart,
            Template.VariableUpperCase3rdPart,
            Template.VariableUpperCaseBytesPartByte1,
            Template.VariableUpperCaseBytesPartByte2,
            Template.VariableUpperCaseBytesPartByte3,
            Template.VariableUpperCaseBytesPartByte4,
            Template.VariableUpperCaseBytesPartByte5,
            Template.VariableUpperCaseBytesPartByte6,
            Template.VariableUpperCaseBytesPartByte7,
            Template.VariableUpperCaseBytesPartByte8,
        };

        /// <summary>
        /// setup method to be called to initialize unit tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.guidGenerator = new GuidGenerater();
        }

        /// <summary>
        /// cleanup method to be called to cleanup unit tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.guidGenerator = null;
        }

        /// <summary>
        /// Test All Variable
        /// </summary>
        /// <param name="count">loop count</param>
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(20)]
        public void TestAllVariable(int count)
        {
            var guidGenerator = new GuidGenerater();
            var builderInput = new StringBuilder();
            var builderExpected = new StringBuilder();

            var guid = guidGenerator.NewGuid();
            for (int i = 0; i < count; i++)
            {
                foreach (string variable in AllVariableNames)
                {
                    builderInput.Append(FormVariable(variable));
                    builderInput.Append(Environment.NewLine);

                    builderExpected.Append(ExpandGuidValue(guid, variable));
                    builderExpected.Append(Environment.NewLine);
                }
            }
            var input = builderInput.ToString();
            var expected = builderExpected.ToString();
            var output = Template.ProcessTemplate(input, this.guidGenerator.NewGuid);

            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }


        /// <summary>
        /// Test All Variable for many GUIDs
        /// </summary>
        /// <param name="numGUIDs">number of GUIDs</param>
        /// <param name="count">loop count</param>
        [TestCase(2, 1)]
        [TestCase(4, 3)]
        [TestCase(5, 10)]
        [TestCase(6, 20)]
        public void TestAllVariableForManyGuids(int numGUIDs, int count)
        {
            var guidGenerator = new GuidGenerater();
            var builderInput = new StringBuilder();
            var builderExpected = new StringBuilder();

            for (int j = 0; j < numGUIDs; j++)
            {
                var guid = guidGenerator.NewGuid();
                for (int i = 0; i < count; i++)
                {
                    foreach (string variable in AllVariableNames)
                    {
                        builderInput.Append(FormVariable(variable, j));
                        builderInput.Append(Environment.NewLine);

                        builderExpected.Append(ExpandGuidValue(guid, variable));
                        builderExpected.Append(Environment.NewLine);
                    }
                }
            }
            var input = builderInput.ToString();
            var expected = builderExpected.ToString();
            var output = Template.ProcessTemplate(input, this.guidGenerator.NewGuid);

            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// DictionaryGuidGenerator
        /// </summary>
        internal class DictionaryGuidGenerator
        {
            /// <summary>
            /// guid generator
            /// </summary>
            private NewGuid newGuid;

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

        /// <summary>
        /// Test All Variable for many GUIDs at random
        /// </summary>
        /// <param name="numGUIDs">number of GUIDs</param>
        /// <param name="count">loop count</param>
        [TestCase(10, 5)]
        [TestCase(50, 3)]
        [TestCase(100, 2)]
        public void TestAllVariableForManyGuidsAtRandom(int numGUIDs, int count)
        {
            var guidGenerator = new GuidGenerater();
            var builderInput = new StringBuilder();
            var builderExpected = new StringBuilder();
            var random = new Random();

            var dictionaryGuid = new DictionaryGuidGenerator(guidGenerator.NewGuid);
            for (int j = 0; j < numGUIDs; j++)
            {
                for (int i = 0; i < count; i++)
                {
                    int guidIndex = random.Next(-1, numGUIDs - 1);
                    foreach (string variable in AllVariableNames)
                    {
                        builderInput.Append(FormVariable(variable, guidIndex));
                        builderInput.Append(Environment.NewLine);

                        builderExpected.Append(ExpandGuidValue(dictionaryGuid.GetGuid(guidIndex), variable));
                        builderExpected.Append(Environment.NewLine);
                    }
                }
            }
            var input = builderInput.ToString();
            var expected = builderExpected.ToString();
            var output = Template.ProcessTemplate(input, this.guidGenerator.NewGuid);

            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// create Variable
        /// </summary>
        /// <param name="variableName">variable name</param>
        /// <returns>variable</returns>
        private string FormVariable(string variableName)
        {
            return "{" + variableName + "}";
        }

        /// <summary>
        /// create Variable with index
        /// </summary>
        /// <param name="variableName">variable name</param>
        /// <param name="index">GUID index</param>
        /// <returns>variable</returns>
        private string FormVariable(string variableName, int index)
        {
            if (index >= 0)
            {
                return "{" + variableName + "(" + index.ToString() + ")" + "}";
            }
            else
            {
                return FormVariable(variableName);
            }
        }

        /// <summary>
        /// expand GUID value
        /// </summary>
        /// <param name="guid">GUID value to be expanded</param>
        /// <param name="variableName">variable name</param>
        /// <returns>variable</returns>
        private string ExpandGuidValue(Guid guid, string variableName)
        {
            var formatGuid = new FormatGuid(guid);

            switch (variableName)
            {
                case Template.VariableLowerCase1stPart: return formatGuid.Data1.ToString("x8");
                case Template.VariableLowerCase2ndPart: return formatGuid.Data2.ToString("x4");
                case Template.VariableLowerCase3rdPart: return formatGuid.Data3.ToString("x4");
                case Template.VariableLowerCaseBytesPartByte1: return formatGuid.Bytes[0].ToString("x2");
                case Template.VariableLowerCaseBytesPartByte2: return formatGuid.Bytes[1].ToString("x2");
                case Template.VariableLowerCaseBytesPartByte3: return formatGuid.Bytes[2].ToString("x2");
                case Template.VariableLowerCaseBytesPartByte4: return formatGuid.Bytes[3].ToString("x2");
                case Template.VariableLowerCaseBytesPartByte5: return formatGuid.Bytes[4].ToString("x2");
                case Template.VariableLowerCaseBytesPartByte6: return formatGuid.Bytes[5].ToString("x2");
                case Template.VariableLowerCaseBytesPartByte7: return formatGuid.Bytes[6].ToString("x2");
                case Template.VariableLowerCaseBytesPartByte8: return formatGuid.Bytes[7].ToString("x2");

                case Template.VariableUpperCase1stPart: return formatGuid.Data1.ToString("X8");
                case Template.VariableUpperCase2ndPart: return formatGuid.Data2.ToString("X4");
                case Template.VariableUpperCase3rdPart: return formatGuid.Data3.ToString("X4");
                case Template.VariableUpperCaseBytesPartByte1: return formatGuid.Bytes[0].ToString("X2");
                case Template.VariableUpperCaseBytesPartByte2: return formatGuid.Bytes[1].ToString("X2");
                case Template.VariableUpperCaseBytesPartByte3: return formatGuid.Bytes[2].ToString("X2");
                case Template.VariableUpperCaseBytesPartByte4: return formatGuid.Bytes[3].ToString("X2");
                case Template.VariableUpperCaseBytesPartByte5: return formatGuid.Bytes[4].ToString("X2");
                case Template.VariableUpperCaseBytesPartByte6: return formatGuid.Bytes[5].ToString("X2");
                case Template.VariableUpperCaseBytesPartByte7: return formatGuid.Bytes[6].ToString("X2");
                case Template.VariableUpperCaseBytesPartByte8: return formatGuid.Bytes[7].ToString("X2");
                default:
                    throw new ArgumentException(variableName);
            }
        }
    }
}
