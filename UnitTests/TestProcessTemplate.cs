//-----------------------------------------------------------------------
// <copyright file="TestProcessTemplate.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Unittest
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using CreateGUIDVSPlugin.Utility;
    using System.Text.RegularExpressions;

    [TestFixture]
    public class TestProcessTemplate
    {
        private static Dictionary<string, string> keywordMap = new Dictionary<string, string>()
        {
            { "Variable1", "VariableA" },
            { "Variable2", "VariableB" },
            { "Variable3", "VariableC" },
            { "Variable4", "VariableD" },
            { "Variable5", "VariableE" },
            { "Variable6", "VariableF" },
            { "Variable7", "VariableG" },
            { "Variable8", "VariableH" },
            { "Variable9", "VariableI" },
        };

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [TestCase("Variable1", true, true, false, false)]
        [TestCase("Variable_5", true, true, false, false)]
        [TestCase("Variable1(2)", true, true, true, false)]
        [TestCase("Variable2(123)", true, true, true, false)]
        [TestCase("Variable1(a)", true, false, false, true)]
        [TestCase("Variable2(ef)", true, false, false, true)]
        [TestCase("Variable1(123d)", true, false, false, true)]
        [TestCase("Variable2(-123)", true, false, false, true)]
        [TestCase("Variable3(+123)", true, false, false, true)]
        [TestCase("Variable4(d123)", true, false, false, true)]
        [TestCase("Variable-1", true, false, false, true)]
        [TestCase("Variable+1", true, false, false, true)]
        [TestCase("Variable 1", true, false, false, true)]
        [TestCase("Variable,1", true, false, false, true)]
        [TestCase("Variable.1", true, false, false, true)]
        [TestCase("Variable[1", true, false, false, true)]
        [TestCase("Variable(1", true, false, false, true)]
        [TestCase("Variable1)", true, false, false, true)]
        [TestCase("[Variable1]", true, false, false, true)]
        public void Test_RegVariable(
            string inputKeyword,
            bool matchWhole,
            bool matchKeyword,
            bool matchIndex,
            bool matchInvalid
        )
        {
            Console.WriteLine("inputKeyword: " + inputKeyword);

            var testPattern = ProcessTemplate.regexVariable;
            var reg = new Regex(testPattern);
            var input = "{" + inputKeyword + "}";
            Console.WriteLine("regex     : " + testPattern);
            Console.WriteLine("input     : " + input);

            var match = reg.Match(input);
            Console.WriteLine("matchWhole  : " + matchWhole);
            Assert.That(match.Success, Is.EqualTo(matchWhole));
            if (match.Success)
            {
                var groupKeyword   = match.Groups["keyword"];
                var groupIndex     = match.Groups["index"];
                var groupInvalidId = match.Groups["invalidId"];
                Console.WriteLine("matchKeyword: " + matchKeyword + " : " + groupKeyword.Success);
                Assert.That(groupKeyword.Success  , Is.EqualTo(matchKeyword));

                Console.WriteLine("matchIndex  : " + matchIndex + " : " + groupIndex.Success);
                Assert.That(groupIndex.Success    , Is.EqualTo(matchIndex));

                Console.WriteLine("matchInvalid: " + matchInvalid + " : " + groupInvalidId.Success);
                Assert.That(groupInvalidId.Success, Is.EqualTo(matchInvalid));
            }
        }

        [TestCase("{[Variable1]}", true)]
        [TestCase("{[Variable1]}}}", true)]
        public void Test_RegexEach(string input, bool expected)
        {
            var testPattern = ProcessTemplate.regexVariablePar;
            var reg = new Regex(testPattern);
            Console.WriteLine("regex   : " + testPattern);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = reg.Match(input);
            if (output.Success)
            {
                string pattern = @"<([^>]+)>";
                foreach (Match m in Regex.Matches(testPattern, pattern))
                {
                    var keyName = m.Groups[1].Value;
                    var group = output.Groups[keyName];
                    if (group.Success)
                    {
                        Console.WriteLine("group  : " + keyName + " : " + group.Value);
                    }
                    else
                    {
                        Console.WriteLine("group  : " + keyName + " : " + "Not Match");
                    }
                }
            }
            Console.WriteLine("output  : " + output.Success);
            Assert.That(output.Success, Is.EqualTo(expected));
        }

        [TestCase("Variable1", "VariableA")]
        [TestCase("Variable2", "VariableB")]
        [TestCase("Variable3", "VariableC")]
        [TestCase("Variable4", "VariableD")]
        public void Test_ParseVariable(string inputKeyword, string outputKeyword)
        {
            var input = "{" + inputKeyword + "}";
            var expected = outputKeyword;
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("output  : " + output);
            Assert.That(output, Is.EqualTo(expected));
        }

        [TestCase("Variable1", "-", "")]
        [TestCase("Variable1", "+", "")]
        [TestCase("Variable1", "", "-")]
        [TestCase("Variable1", "", "+")]
        [TestCase("Variable1", "-", "-")]
        [TestCase("Variable1", "+", "+")]
        public void Test_ParseInvalidVariable(string inputKeyword, string left, string right)
        {
            var input = "{" + left + inputKeyword + right + "}";
            var expected = input;
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("output  : " + output);
            Assert.That(output, Is.EqualTo(expected));
        }

        [TestCase("Variable2", "{", "", typeof(ProcessTemplate.OrphanedLeftBraceException))]
        [TestCase("Variable3", "", "}", typeof(ProcessTemplate.OrphanedRightBraceException))]
        public void Test_ParseInvalidVariableNoCurlyBracket(string inputKeyword, string left, string right, Type typeException)
        {
            var input = left + inputKeyword + right;
            var expected = input;
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            Assert.That(
                () => { ProcessTemplate.ReplaceVariable(input, delegateGetNewText); },
                Throws.InstanceOf(typeException)
            );
        }

        [TestCase("Variable1", "VariableA", "1")]
        [TestCase("Variable2", "VariableB", "2")]
        [TestCase("Variable3", "VariableC", "3")]
        [TestCase("Variable4", "VariableD", "4")]
        public void Test_ParseVariableIndex(string inputKeyword, string outputKeyword, string index)
        {
            var indexStr = "(" + index + ")";
            var input = "{" + inputKeyword + indexStr + "}";
            var expected = outputKeyword + indexStr;
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("output  : " + output);
            Assert.That(output, Is.EqualTo(expected));
        }


        [TestCase("Variable1", "VariableA", "1", "[", "]")]
        [TestCase("Variable3", "VariableC", "3", "<", ">")]
        [TestCase("Variable4", "VariableD", "4", "[", "")]
        [TestCase("Variable6", "VariableF", "6", "(", "")]
        [TestCase("Variable7", "VariableG", "7", "", "]")]
        [TestCase("Variable9", "VariableI", "9", "", ")")]
        [TestCase("Variable1", "VariableA", "a", "(", ")")]
        [TestCase("Variable1", "VariableA", "ab", "(", ")")]
        [TestCase("Variable1", "VariableA", "123ab", "(", ")")]
        [TestCase("Variable1", "VariableA", "-1", "(", ")")]
        [TestCase("Variable1", "VariableA", "+1", "(", ")")]
        [TestCase("Variable1", "VariableA", "_1", "(", ")")]
        [TestCase("Variable1", "VariableA", "1_", "(", ")")]
        public void Test_ParseVariableInvalidIndex(
            string inputKeyword,
            string outputKeyword,
            string index,
            string left,
            string right)
        {
            var indexStr = left + index + right;
            var input = "{" + inputKeyword + indexStr + "}";
            var expected = input;
            Console.WriteLine("input   : " + input);
            Console.WriteLine("expected: " + expected);

            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("output  : " + output);
            Assert.That(output, Is.EqualTo(expected));
        }

        [TestCase("Variable2", "2", "{", "}", typeof(ProcessTemplate.OrphanedLeftBraceException))]
        [TestCase("Variable8", "8", "" , "}", typeof(ProcessTemplate.OrphanedRightBraceException))]
        public void Test_ParseVariableInvalidBrace(
            string inputKeyword,
            string index,
            string left,
            string right,
            Type typeException)
        {
            var indexStr = left + index + right;
            var input = "{" + inputKeyword + indexStr + "}";
            Console.WriteLine("input   : " + input);

            Assert.That(
                () => { ProcessTemplate.ReplaceVariable(input, delegateGetNewText); },
                Throws.InstanceOf(typeException)
            );
        }

        /// <summary>
        /// delegate to substitute keywords
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal string delegateGetNewText(string keyword, int index)
        {
            if (!keywordMap.ContainsKey(keyword))
            {
                throw new ProcessTemplate.InvalidKeywordException(-1, keyword);
            }
            var newkeyword = keywordMap[keyword];
            if (index < 0)
            {
                return newkeyword;
            }
            else
            {
                return newkeyword + "(" + index.ToString() + ")";
            }
        }
    }
}
