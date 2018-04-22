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

        [TestCase("Variable1", true)]
        [TestCase("Variable2", true)]
        [TestCase("Variable3", true)]
        [TestCase("Variable4", true)]
        [TestCase("Variable_5", true)]
        [TestCase("Variable1(2)", true)]
        [TestCase("Variable2(3)", true)]
        [TestCase("Variable3(4)", true)]
        [TestCase("Variable4(5)", true)]
        [TestCase("Variable5(123)", true)]
        [TestCase("Variable1(a)", false)]
        [TestCase("Variable2(b)", false)]
        [TestCase("Variable3(c)", false)]
        [TestCase("Variable4(d)", false)]
        [TestCase("Variable5(ef)", false)]
        [TestCase("Variable1(123d)", false)]
        [TestCase("Variable2(-123)", false)]
        [TestCase("Variable3(+123)", false)]
        [TestCase("Variable4(d123)", false)]
        [TestCase("Variable-1", false)]
        [TestCase("Variable+1", false)]
        [TestCase("Variable 1", false)]
        [TestCase("Variable,1", false)]
        [TestCase("Variable.1", false)]
        [TestCase("Variable[1", false)]
        [TestCase("Variable[1", false)]
        [TestCase("Variable(1", false)]
        [TestCase("Variable1)", false)]
        [TestCase("[Variable1]", false)]
        public void Test_RegEx(string inputKeyword, bool expected)
        {
            var input = "{" + inputKeyword + "}";
            var output = ProcessTemplate.reg.Match(input);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
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
            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }

        public void Test_ParseInvalidVariable(string inputKeyword, string outputKeyword, string left, string right)
        {
            var input = "{" + left + inputKeyword + right + "}";
            var expected = input;
            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }

        [TestCase("Variable1", "VariableA", "", "")]
        [TestCase("Variable2", "VariableB", "{", "")]
        [TestCase("Variable3", "VariableC", "", "}")]
        public void Test_ParseInvalidVariableNoCurlyBracket(string inputKeyword, string outputKeyword, string left, string right)
        {
            var input = left + inputKeyword + right;
            var expected = input;
            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }

        [TestCase("Variable1", "VariableA", 1)]
        [TestCase("Variable2", "VariableB", 2)]
        [TestCase("Variable3", "VariableC", 3)]
        [TestCase("Variable4", "VariableD", 4)]
        public void Test_ParseVariableIndex(string inputKeyword, string outputKeyword, int index)
        {
            var indexStr = "(" + index.ToString() + ")";
            var input = "{" + inputKeyword + indexStr + "}";
            var expected = outputKeyword + indexStr;
            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
        }


        [TestCase("Variable1", "VariableA", 1, "[", "]")]
        [TestCase("Variable2", "VariableB", 2, "{", "}")]
        [TestCase("Variable3", "VariableC", 3, "<", ">")]
        [TestCase("Variable4", "VariableD", 4, "[", "")]
        [TestCase("Variable5", "VariableE", 5, "{", "")]
        [TestCase("Variable6", "VariableF", 6, "(", "")]
        [TestCase("Variable7", "VariableG", 7, "", "]")]
        [TestCase("Variable8", "VariableH", 8, "", "}")]
        [TestCase("Variable9", "VariableI", 9, "", ")")]
        public void Test_ParseVariableInvalidIndex(string inputKeyword, string outputKeyword, int index, string left, string right)
        {
            var indexStr = left + index.ToString() + right;
            var input = "{" + inputKeyword + indexStr + "}";
            var expected = input;
            var output = ProcessTemplate.ReplaceVariable(input, delegateGetNewText);
            Console.WriteLine("input   : " + input);
            Console.WriteLine("output  : " + output);
            Console.WriteLine("expected: " + expected);
            Assert.That(output, Is.EqualTo(expected));
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
                throw new ProcessTemplate.InvalidKeywordException(keyword);
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
