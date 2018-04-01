//-----------------------------------------------------------------------
// <copyright file="Template.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CreateGUIDVSPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Template Processing Class
    /// </summary>
    internal class Template
    {
        /// <summary>
        /// Table between variables of Upper Case and variables of Lower Case
        /// </summary>
        internal static readonly Dictionary<string, string> CaseMap = new Dictionary<string, string>
        {
            { Template.variableUpperCaseGuidWithHyphens,    Template.variableLowerCaseGuidWithHyphens },
            { Template.variableUpperCaseGuidWithoutHyphens, Template.variableLowerCaseGuidWithoutHyphens },
            { Template.variableUpperCase1stPart, Template.variableLowerCase1stPart },
            { Template.variableUpperCase2ndPart, Template.variableLowerCase2ndPart },
            { Template.variableUpperCase3rdPart, Template.variableLowerCase3rdPart },
            { Template.variableUpperCaseBytesPartByte1, Template.variableLowerCaseBytesPartByte1 },
            { Template.variableUpperCaseBytesPartByte2, Template.variableLowerCaseBytesPartByte2 },
            { Template.variableUpperCaseBytesPartByte3, Template.variableLowerCaseBytesPartByte3 },
            { Template.variableUpperCaseBytesPartByte4, Template.variableLowerCaseBytesPartByte4 },
            { Template.variableUpperCaseBytesPartByte5, Template.variableLowerCaseBytesPartByte5 },
            { Template.variableUpperCaseBytesPartByte6, Template.variableLowerCaseBytesPartByte6 },
            { Template.variableUpperCaseBytesPartByte7, Template.variableLowerCaseBytesPartByte7 },
            { Template.variableUpperCaseBytesPartByte8, Template.variableLowerCaseBytesPartByte8 },
        };

        /// <summary>
        /// Default Template String
        /// </summary>
#if ORIGINAL
        internal static readonly string DefaultFormatString =
              "// {" + "{" + variableLowerCaseGuidWithHyphens + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + variableLowerCase1stPart + "},"
            + "0x{" + variableLowerCase2ndPart + "},"
            + "0x{" + variableLowerCase3rdPart + "},"
            + "0x{" + variableLowerCaseBytesPartByte1 + "},"
            + "0x{" + variableLowerCaseBytesPartByte2 + "},"
            + "0x{" + variableLowerCaseBytesPartByte3 + "},"
            + "0x{" + variableLowerCaseBytesPartByte4 + "},"
            + "0x{" + variableLowerCaseBytesPartByte5 + "},"
            + "0x{" + variableLowerCaseBytesPartByte6 + "},"
            + "0x{" + variableLowerCaseBytesPartByte7 + "},"
            + "0x{" + variableLowerCaseBytesPartByte8 + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "<Guid(\""
            + "{" + variableLowerCase1stPart + "}-"
            + "{" + variableLowerCase2ndPart + "}-"
            + "{" + variableLowerCase3rdPart + "}-"
            + "{" + variableLowerCaseBytesPartByte1 + "}"
            + "{" + variableLowerCaseBytesPartByte2 + "}-"
            + "{" + variableLowerCaseBytesPartByte2 + "}"
            + "{" + variableLowerCaseBytesPartByte3 + "}"
            + "{" + variableLowerCaseBytesPartByte4 + "}"
            + "{" + variableLowerCaseBytesPartByte5 + "}"
            + "{" + variableLowerCaseBytesPartByte6 + "}"
            + "{" + variableLowerCaseBytesPartByte7 + "}"
            + "{" + variableLowerCaseBytesPartByte8 + "}"
            + "\")>"
            + Environment.NewLine;
#else
        internal static readonly string DefaultFormatString =
              "// {" + "{" + variableLowerCaseGuidWithHyphens + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + variableLowerCase1stPart + "}," + " "
            + "0x{" + variableLowerCase2ndPart + "}," + " "
            + "0x{" + variableLowerCase3rdPart + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte1 + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte2 + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte3 + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte4 + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte5 + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte6 + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte7 + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte8 + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "// {" + "{" + variableLowerCaseGuidWithHyphens + "(1)" + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + variableLowerCase1stPart + "(1)" + "}," + " "
            + "0x{" + variableLowerCase2ndPart + "(1)" + "}," + " "
            + "0x{" + variableLowerCase3rdPart + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte1 + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte2 + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte3 + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte4 + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte5 + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte6 + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte7 + "(1)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte8 + "(1)" + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "// {" + "{" + variableLowerCaseGuidWithHyphens + "(2)" + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + variableLowerCase1stPart + "(2)" + "}," + " "
            + "0x{" + variableLowerCase2ndPart + "(2)" + "}," + " "
            + "0x{" + variableLowerCase3rdPart + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte1 + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte2 + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte3 + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte4 + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte5 + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte6 + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte7 + "(2)" + "}," + " "
            + "0x{" + variableLowerCaseBytesPartByte8 + "(2)" + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine;
#endif

        /// <summary>
        /// Template Variables
        /// </summary>
        internal static readonly VariableManager[] Variables = new VariableManager[]
        {
            new VariableManager(variableLowerCaseGuidWithHyphens, "Lower-case Full GUID with hypens"),
            new VariableManager(variableUpperCaseGuidWithHyphens, "Upper-case Full GUID with hypens"),
            new VariableManager(variableLowerCaseGuidWithoutHyphens, "Lower-case Full GUID without hypens"),
            new VariableManager(variableUpperCaseGuidWithoutHyphens, "Upper-case Full GUID without hypens"),
            new VariableManager(variableLowerCase1stPart, "Lower Case 1st Part"),
            new VariableManager(variableUpperCase1stPart, "Upper Case 1st Part"),
            new VariableManager(variableLowerCase2ndPart, "Lower Case 2nd Part"),
            new VariableManager(variableUpperCase2ndPart, "Upper Case 2nd Part"),
            new VariableManager(variableLowerCase3rdPart, "Lower Case 3rd Part"),
            new VariableManager(variableUpperCase3rdPart, "Upper Case 3rd Part"),
            new VariableManager(variableLowerCaseBytesPartByte1, "1st byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte1, "1st byte of Upper Case Bytes Part"),
            new VariableManager(variableLowerCaseBytesPartByte2, "2nd byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte2, "2nd byte of Upper Case Bytes Part"),
            new VariableManager(variableLowerCaseBytesPartByte3, "3rd byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte3, "3rd byte of Upper Case Bytes Part"),
            new VariableManager(variableLowerCaseBytesPartByte4, "4th byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte4, "4th byte of Upper Case Bytes Part"),
            new VariableManager(variableLowerCaseBytesPartByte5, "5th byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte5, "5th byte of Upper Case Bytes Part"),
            new VariableManager(variableLowerCaseBytesPartByte6, "6th byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte6, "6th byte of Upper Case Bytes Part"),
            new VariableManager(variableLowerCaseBytesPartByte7, "7th byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte7, "7th byte of Upper Case Bytes Part"),
            new VariableManager(variableLowerCaseBytesPartByte8, "8th byte of Lower Case Bytes Part"),
            new VariableManager(variableUpperCaseBytesPartByte8, "8th byte of Upper Case Bytes Part"),
        };

        /// <summary>
        /// Template Name for 1st part of Lower Case of GUID
        /// </summary>
        private static string variableLowerCase1stPart = "LowerCase1stPart";

        /// <summary>
        /// Template Name for 2nd part of Lower Case of GUID
        /// </summary>
        private static string variableLowerCase2ndPart = "LowerCase2ndPart";

        /// <summary>
        /// Template Name for 3rd part of Lower Case of GUID
        /// </summary>
        private static string variableLowerCase3rdPart = "LowerCase3rdPart";

        /// <summary>
        /// Template Name for Lower Case 1st byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte1 = "LowerCaseBytesPartByte1";

        /// <summary>
        /// Template Name for Lower Case 2nd byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte2 = "LowerCaseBytesPartByte2";

        /// <summary>
        /// Template Name for Lower Case 3rd byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte3 = "LowerCaseBytesPartByte3";

        /// <summary>
        /// Template Name for Lower Case 4th byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte4 = "LowerCaseBytesPartByte4";

        /// <summary>
        /// Template Name for Lower Case 5th byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte5 = "LowerCaseBytesPartByte5";

        /// <summary>
        /// Template Name for Lower Case 6th byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte6 = "LowerCaseBytesPartByte6";

        /// <summary>
        /// Template Name for Lower Case 7th byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte7 = "LowerCaseBytesPartByte7";

        /// <summary>
        /// Template Name for Lower Case 8th byte of Bytes Part of GUID
        /// </summary>
        private static string variableLowerCaseBytesPartByte8 = "LowerCaseBytesPartByte8";

        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        private static string variableLowerCaseGuidWithHyphens = "LowerCaseGuidWithHyphens";

        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        private static string variableLowerCaseGuidWithoutHyphens = "LowerCaseGuidWithoutHyphens";

        /// <summary>
        /// Template Name for 1st part of Upper Case of GUID
        /// </summary>
        private static string variableUpperCase1stPart = "UpperCase1stPart";

        /// <summary>
        /// Template Name for 2nd part of Upper Case of GUID
        /// </summary>
        private static string variableUpperCase2ndPart = "UpperCase2ndPart";

        /// <summary>
        /// Template Name for 3rd part of Upper Case of GUID
        /// </summary>
        private static string variableUpperCase3rdPart = "UpperCase3rdPart";

        /// <summary>
        /// Template Name for Upper Case 1st byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte1 = "UpperCaseBytesPartByte1";

        /// <summary>
        /// Template Name for Upper Case 2nd byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte2 = "UpperCaseBytesPartByte2";

        /// <summary>
        /// Template Name for Upper Case 3rd byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte3 = "UpperCaseBytesPartByte3";

        /// <summary>
        /// Template Name for Upper Case 4th byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte4 = "UpperCaseBytesPartByte4";

        /// <summary>
        /// Template Name for Upper Case 5th byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte5 = "UpperCaseBytesPartByte5";

        /// <summary>
        /// Template Name for Upper Case 6th byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte6 = "UpperCaseBytesPartByte6";

        /// <summary>
        /// Template Name for Upper Case 7th byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte7 = "UpperCaseBytesPartByte7";

        /// <summary>
        /// Template Name for Upper Case 8th byte of Bytes Part of GUID
        /// </summary>
        private static string variableUpperCaseBytesPartByte8 = "UpperCaseBytesPartByte8";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        private static string variableUpperCaseGuidWithHyphens = "UpperCaseGuidWithHyphens";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        private static string variableUpperCaseGuidWithoutHyphens = "UpperCaseGuidWithoutHyphens";

        /// <summary>
        /// divided element type
        /// </summary>
        internal enum Type
        {
            None,
            NormalString,
            TemplateNoIndex,
            TemplateIndex,
        }

        /// <summary>
        /// Create a dictionary for the template values
        /// </summary>
        /// <param name="guid">source GUID</param>
        /// <returns>dictionary of formatted GUID strings.</returns>
        internal static Dictionary<string, string> CreateValuesDictionary(Guid guid)
        {
            var values = new Dictionary<string, string>();
            foreach (VariableManager variableManager in Template.Variables)
            {
                values[variableManager.Keyword] = string.Empty;
            }

            // see https://msdn.microsoft.com/library/97af8hh4(v=vs.110).aspx about the parameter of Guid.ToString().
            // see https://msdn.microsoft.com/library/system.guid(v=vs.110).aspx
            // see 'Reference Source' link in the above site.
            var lowerWithHyphens = guid.ToString("D");
            values[Template.variableLowerCaseGuidWithHyphens] = lowerWithHyphens;

            var lowerWithoutHyphens = guid.ToString("N");
            values[Template.variableLowerCaseGuidWithoutHyphens] = lowerWithoutHyphens;

            var formatGuid = new FormatGuid(guid);
            values[Template.variableLowerCase1stPart] = formatGuid.Data1.ToString("x8");
            values[Template.variableLowerCase2ndPart] = formatGuid.Data2.ToString("x4");
            values[Template.variableLowerCase3rdPart] = formatGuid.Data3.ToString("x4");
            values[Template.variableLowerCaseBytesPartByte1] = formatGuid.Bytes[0].ToString("x2");
            values[Template.variableLowerCaseBytesPartByte2] = formatGuid.Bytes[1].ToString("x2");
            values[Template.variableLowerCaseBytesPartByte3] = formatGuid.Bytes[2].ToString("x2");
            values[Template.variableLowerCaseBytesPartByte4] = formatGuid.Bytes[3].ToString("x2");
            values[Template.variableLowerCaseBytesPartByte5] = formatGuid.Bytes[4].ToString("x2");
            values[Template.variableLowerCaseBytesPartByte6] = formatGuid.Bytes[5].ToString("x2");
            values[Template.variableLowerCaseBytesPartByte7] = formatGuid.Bytes[6].ToString("x2");
            values[Template.variableLowerCaseBytesPartByte8] = formatGuid.Bytes[7].ToString("x2");

            // set variables for Upper Case
            foreach (KeyValuePair<string, string> element in CaseMap)
            {
                // ex. Template.variableUpperCaseGuidWithHyphens
                var targetKeyName = element.Key;

                // ex. Template.variableLowerCaseGuidWithHyphens
                var sourceKeyName = element.Value;

                var value = values[sourceKeyName];
                values[targetKeyName] = value.ToUpper();
            }

            return values;
        }

        /// <summary>
        /// Replace the template variable to the values defined by dictionary
        /// </summary>
        /// <param name="template">template string to be replaced</param>
        /// <returns>formatted string</returns>
        internal static string ProcessTemplate(string template)
        {
            const int NoIndex = -1;
            const string PatternDivide = @"(?<variable>{\w+(\(\d+\))?})";
            const string PatternParse = @"{(?<keyword>\w+)(\((?<index>\d+)\))?}";
            var substrings = Regex.Split(template, PatternDivide, RegexOptions.ExplicitCapture);
            var regex = new Regex(PatternParse, RegexOptions.Compiled);
            Dictionary<int, Dictionary<string, string>> dataBase = new Dictionary<int, Dictionary<string, string>>();

            var builder = new StringBuilder();
            foreach (string str in substrings)
            {
                int guidIndex = NoIndex;
                Type elementType = Type.None;
                string keywordOrData = string.Empty;

                Dictionary<string, string> values = null;
                var match = regex.Match(str);
                if (match.Success)
                {
                    var groupKeyword = match.Groups["keyword"];
                    var groupIndex = match.Groups["index"];
                    if (groupKeyword.Success)
                    {
                        keywordOrData = groupKeyword.Value;
                    }
                    else
                    {
                        keywordOrData = string.Empty;
                    }

                    if (groupIndex.Success)
                    {
                        elementType = Type.TemplateIndex;
                        guidIndex = int.Parse(groupIndex.Value);
                    }
                    else
                    {
                        elementType = Type.TemplateNoIndex;
                        guidIndex = NoIndex;
                    }

                    if (!dataBase.ContainsKey(guidIndex))
                    {
                        var guid = Guid.NewGuid();
                        dataBase[guidIndex] = CreateValuesDictionary(guid);
                    }

                    values = dataBase[guidIndex];
                }
                else
                {
                    elementType = Type.NormalString;
                    keywordOrData = str;
                }

                switch (elementType)
                {
                    case Type.None:
                        break;
                    case Type.NormalString:
                        builder.Append(keywordOrData);
                        break;
                    case Type.TemplateIndex:
                        builder.Append(values[keywordOrData]);
                        break;
                    case Type.TemplateNoIndex:
                        builder.Append(values[keywordOrData]);
                        break;
                }
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// Manager for Variable Name, Description, and Regular expression
    /// </summary>
    internal class VariableManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableManager" /> class.
        /// </summary>
        /// <param name="keyword">Variable name</param>
        /// <param name="description">Variable description</param>
        internal VariableManager(string keyword, string description)
        {
            this.Keyword = keyword;
            this.Description = this.GetVariable() + ": " + description;
        }

        /// <summary>
        /// Gets or sets description for Variable
        /// </summary>
        internal string Description { get; set; }

        /// <summary>
        /// Gets or sets variable keyword
        /// </summary>
        internal string Keyword { get; set; }

        /// <summary>
        /// Get Template variable
        /// </summary>
        /// <returns>Template variable</returns>
        internal string GetVariable()
        {
            return "{" + this.Keyword + "}";
        }

        /// <summary>
        /// Get Template variable with index
        /// </summary>
        /// <param name="index">variable index</param>
        /// <returns>Template variable with index</returns>
        internal string GetVariable(int index)
        {
            return string.Format("{{{0}({1})}}", this.Keyword, index);
        }
    }
}
