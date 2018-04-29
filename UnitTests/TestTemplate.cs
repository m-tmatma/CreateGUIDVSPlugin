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
        /// invalid variable type
        /// </summary>
        public enum InvalidVariableType
        {
            /// <summary>
            /// "{" is missing in a variable
            /// </summary>
            MissingFormerBlacket,

            /// <summary>
            /// "}" is missing in a variable
            /// </summary>
            MissingLatterBlacket,

            /// <summary>
            /// missing first charator in valid variable
            /// </summary>
            MissingFirstCharactor,

            /// <summary>
            /// missing last charator in valid variable
            /// </summary>
            MissingLastCharactor,

            /// <summary>
            /// missing first and last charator in valid variable
            /// </summary>
            MissingFirstLastCharactor,
        }

        /// <summary>
        /// escape variable type
        /// </summary>
        public enum EscapeVariableType
        {
            /// <summary>
            /// "{{variable}
            /// </summary>
            LeftEscape,

            /// <summary>
            /// "{variable}}
            /// </summary>
            RightEscape,
        }

        /// <summary>
        /// delegate for creating new GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal delegate Guid NewGuid();

        /// <summary>
        /// GUID generator class to make unit-testing easier
        /// </summary>
        private GuidQueue guidQueue;

        /// <summary>
        /// All Variable
        /// </summary>
        private static readonly string[] AllVariableNames = new string[]
        {
            Template.VariableLowerHyphens,
            Template.VariableUpperHyphens,
            Template.VariableLowerNoHyphens,
            Template.VariableUpperNoHyphens,
            Template.VariableLowerPart1,
            Template.VariableLowerPart2,
            Template.VariableLowerPart3,
            Template.VariableLowerBytes1,
            Template.VariableLowerBytes2,
            Template.VariableLowerBytes3,
            Template.VariableLowerBytes4,
            Template.VariableLowerBytes5,
            Template.VariableLowerBytes6,
            Template.VariableLowerBytes7,
            Template.VariableLowerBytes8,
            Template.VariableUpperPart1,
            Template.VariableUpperPart2,
            Template.VariableUpperPart3,
            Template.VariableUpperBytes1,
            Template.VariableUpperBytes2,
            Template.VariableUpperBytes3,
            Template.VariableUpperBytes4,
            Template.VariableUpperBytes5,
            Template.VariableUpperBytes6,
            Template.VariableUpperBytes7,
            Template.VariableUpperBytes8,
        };

        /// <summary>
        /// setup method to be called to initialize unit tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.guidQueue = new GuidQueue();

            // https://github.com/nunit/docs/wiki/TestContext
            Console.WriteLine(TestContext.CurrentContext.Test.Name);
            Console.WriteLine(TestContext.CurrentContext.Test.ClassName);
            Console.WriteLine(TestContext.CurrentContext.Test.FullName);
        }

        /// <summary>
        /// cleanup method to be called to cleanup unit tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.guidQueue = null;
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
            var builderInput = new StringBuilder();
            var builderExpected = new StringBuilder();

            var guid = this.guidQueue.BrandNewGuid();
            for (int i = 0; i < count; i++)
            {
                foreach (string variable in AllVariableNames)
                {
                    builderInput.Append(FormVariable(variable));
                    builderInput.Append(' ');

                    builderExpected.Append(ExpandGuidValue(guid, variable));
                    builderExpected.Append(' ');
                }
                builderInput.Append(Environment.NewLine);
                builderExpected.Append(Environment.NewLine);
            }
            var input = builderInput.ToString();
            var expected = builderExpected.ToString();
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = Template.Process(input, this.guidQueue.NewGuidFromCache);
            Console.WriteLine("output  : " + output);
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
            var builderInput = new StringBuilder();
            var builderExpected = new StringBuilder();

            for (int j = 0; j < numGUIDs; j++)
            {
                var guid = this.guidQueue.BrandNewGuid();
                for (int i = 0; i < count; i++)
                {
                    foreach (string variable in AllVariableNames)
                    {
                        builderInput.Append(FormVariable(variable, j));
                        builderInput.Append(' ');

                        builderExpected.Append(ExpandGuidValue(guid, variable));
                        builderExpected.Append(' ');
                    }
                    builderInput.Append(Environment.NewLine);
                    builderExpected.Append(Environment.NewLine);
                }
            }
            var input = builderInput.ToString();
            var expected = builderExpected.ToString();
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = Template.Process(input, this.guidQueue.NewGuidFromCache);
            Console.WriteLine("output  : " + output);
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
        [TestCase(3, 5)]
        [TestCase(10, 5)]
        [TestCase(50, 3)]
        [TestCase(100, 2)]
        public void TestAllVariableForManyGuidsAtRandom(int numGUIDs, int count)
        {
            var builderInput = new StringBuilder();
            var builderExpected = new StringBuilder();
            var random = new Random();

            var dictionaryGuid = new DictionaryGuidGenerator(this.guidQueue.BrandNewGuid);
            for (int j = 0; j < numGUIDs; j++)
            {
                for (int i = 0; i < count; i++)
                {
                    int guidIndex = random.Next(-1, numGUIDs - 1);
                    foreach (string variable in AllVariableNames)
                    {
                        builderInput.Append(FormVariable(variable, guidIndex));
                        builderInput.Append(' ');

                        builderExpected.Append(ExpandGuidValue(dictionaryGuid.GetGuid(guidIndex), variable));
                        builderExpected.Append(' ');
                    }
                    builderInput.Append(Environment.NewLine);
                    builderExpected.Append(Environment.NewLine);
                }
            }
            var input = builderInput.ToString();
            var expected = builderExpected.ToString();
            var output = Template.Process(input, this.guidQueue.NewGuidFromCache);

            //Console.WriteLine("output  : " + output);
            //Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// Test Empty Variable
        /// </summary>
        [Test]
        public void TestAllEmptyVariable()
        {
            var input = "{}";
            var expected = input;
            var output = Template.Process(input);
            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// Test All Variables which don't have both blackets.
        /// </summary>
        /// <param name="count">loop count</param>
        /// <param name="invalidType">type of broken variable</param>
        [Test, Pairwise]
        public void TestAllVariableInvalid(
            [Values(3, 10)] int count,
            [Values] InvalidVariableType invalidType)
        {
            Type typeException = null;
            var builderInput = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                foreach (string variable in AllVariableNames)
                {
                    switch(invalidType)
                    {
                        case InvalidVariableType.MissingFormerBlacket:
                            builderInput.Append(variable + "}");
                            typeException = typeof(ProcessTemplate.OrphanedRightBraceException);
                            break;
                        case InvalidVariableType.MissingLatterBlacket:
                            builderInput.Append("{" + variable);
                            typeException = typeof(ProcessTemplate.OrphanedLeftBraceException);
                            break;
                        case InvalidVariableType.MissingFirstCharactor:
                            builderInput.Append(FormVariable(variable.Substring(1)));
                            break;
                        case InvalidVariableType.MissingLastCharactor:
                            builderInput.Append(FormVariable(variable.Substring(0, variable.Length - 1)));
                            break;
                        case InvalidVariableType.MissingFirstLastCharactor:
                            builderInput.Append(FormVariable(variable.Substring(1, variable.Length - 1)));
                            break;
                        default:
                            throw new ArgumentException(invalidType.ToString());
                    }
                }
            }
            var input = builderInput.ToString();
            if (typeException != null)
            {
                Assert.That(
                    () => {
                        var output = Template.Process(input, this.guidQueue.NewGuidFromCache);
                    },
                    Throws.InstanceOf(typeException)
                );
            }
            else
            {
                var expected = input;
                var output = Template.Process(input, this.guidQueue.NewGuidFromCache);
                Assert.That(output, Is.EqualTo(expected));
            }
        }

        /// <summary>
        /// Test escaped dummy variable
        /// </summary>
        /// <param name="count">loop count</param>
        /// <param name="escapeVariableType">escape type</param>
        [TestCase(3, EscapeVariableType.LeftEscape, typeof(ProcessTemplate.OrphanedRightBraceException))]
        [TestCase(5, EscapeVariableType.RightEscape, typeof(ProcessTemplate.OrphanedRightBraceException))]
        public void TestAllEscapedInvalidVariable(
            int count,
            EscapeVariableType escapeVariableType,
            Type typeException
        )
        {
            var builderInput = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                foreach (string variable in AllVariableNames)
                {
                    switch (escapeVariableType)
                    {
                        case EscapeVariableType.LeftEscape:
                            builderInput.Append("{");
                            break;
                        default:
                            break;
                    }
                    builderInput.Append(FormVariable(variable));
                    switch (escapeVariableType)
                    {
                        case EscapeVariableType.RightEscape:
                            builderInput.Append("}");
                            break;
                        default:
                            break;
                    }
                }
            }
            var input = builderInput.ToString();
            Console.WriteLine("input   : " + input);
            //var builderExpected = new StringBuilder(input);
            //builderExpected.Replace("{{", "{");
            //builderExpected.Replace("}}", "}");

            //var expected = builderExpected.ToString();
            //Console.WriteLine("expected: " + expected);

            Assert.That(
                () => {
                    var output = Template.Process(input, this.guidQueue.BrandNewGuid);
                    Console.WriteLine("output  : " + output);
                },
                Throws.InstanceOf(typeException)
            );
        }

        /// <summary>
        /// Test escaped valid Variable
        /// </summary>
        /// <param name="count">loop count</param>
        [Test, Pairwise]
        public void TestAllEscapedValidVariable(
            [Values(3, 10)] int count
        )
        {
            var builderInput = new StringBuilder();
            var builderExpected = new StringBuilder();

            var guid = this.guidQueue.BrandNewGuid();
            for (int i = 0; i < count; i++)
            {
                foreach (string variable in AllVariableNames)
                {
                    builderInput.Append("{{");
                    builderInput.Append(FormVariable(variable));
                    builderInput.Append("}}");
                    builderInput.Append(' ');

                    builderExpected.Append('{');
                    builderExpected.Append(ExpandGuidValue(guid, variable));
                    builderExpected.Append('}');
                    builderExpected.Append(' ');
                }
                builderInput.Append(Environment.NewLine);
                builderExpected.Append(Environment.NewLine);
            }
            var input = builderInput.ToString();
            var expected = builderExpected.ToString();
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = Template.Process(input, this.guidQueue.NewGuidFromCache);
            Console.WriteLine("output  : " + output);
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
                case Template.VariableLowerHyphens: return guid.ToString("D");
                case Template.VariableLowerNoHyphens: return guid.ToString("N");

                case Template.VariableLowerPart1: return formatGuid.Data1.ToString("x8");
                case Template.VariableLowerPart2: return formatGuid.Data2.ToString("x4");
                case Template.VariableLowerPart3: return formatGuid.Data3.ToString("x4");
                case Template.VariableLowerBytes1: return formatGuid.Bytes[0].ToString("x2");
                case Template.VariableLowerBytes2: return formatGuid.Bytes[1].ToString("x2");
                case Template.VariableLowerBytes3: return formatGuid.Bytes[2].ToString("x2");
                case Template.VariableLowerBytes4: return formatGuid.Bytes[3].ToString("x2");
                case Template.VariableLowerBytes5: return formatGuid.Bytes[4].ToString("x2");
                case Template.VariableLowerBytes6: return formatGuid.Bytes[5].ToString("x2");
                case Template.VariableLowerBytes7: return formatGuid.Bytes[6].ToString("x2");
                case Template.VariableLowerBytes8: return formatGuid.Bytes[7].ToString("x2");

                case Template.VariableUpperHyphens: return guid.ToString("D").ToUpper();
                case Template.VariableUpperNoHyphens: return guid.ToString("N").ToUpper();

                case Template.VariableUpperPart1: return formatGuid.Data1.ToString("X8");
                case Template.VariableUpperPart2: return formatGuid.Data2.ToString("X4");
                case Template.VariableUpperPart3: return formatGuid.Data3.ToString("X4");
                case Template.VariableUpperBytes1: return formatGuid.Bytes[0].ToString("X2");
                case Template.VariableUpperBytes2: return formatGuid.Bytes[1].ToString("X2");
                case Template.VariableUpperBytes3: return formatGuid.Bytes[2].ToString("X2");
                case Template.VariableUpperBytes4: return formatGuid.Bytes[3].ToString("X2");
                case Template.VariableUpperBytes5: return formatGuid.Bytes[4].ToString("X2");
                case Template.VariableUpperBytes6: return formatGuid.Bytes[5].ToString("X2");
                case Template.VariableUpperBytes7: return formatGuid.Bytes[6].ToString("X2");
                case Template.VariableUpperBytes8: return formatGuid.Bytes[7].ToString("X2");
                default:
                    throw new ArgumentException(variableName);
            }
        }
    }
}
