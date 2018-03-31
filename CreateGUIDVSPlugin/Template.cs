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
            { Template.VariableUpperCaseGuidWithHyphens,    Template.VariableLowerCaseGuidWithHyphens },
            { Template.VariableUpperCaseGuidWithoutHyphens, Template.VariableLowerCaseGuidWithoutHyphens },
            { Template.VariableUpperCase1stPart, Template.VariableLowerCase1stPart },
            { Template.VariableUpperCase2ndPart, Template.VariableLowerCase2ndPart },
            { Template.VariableUpperCase3rdPart, Template.VariableLowerCase3rdPart },
            { Template.VariableUpperCaseBytesPartByte1, Template.VariableLowerCaseBytesPartByte1 },
            { Template.VariableUpperCaseBytesPartByte2, Template.VariableLowerCaseBytesPartByte2 },
            { Template.VariableUpperCaseBytesPartByte3, Template.VariableLowerCaseBytesPartByte3 },
            { Template.VariableUpperCaseBytesPartByte4, Template.VariableLowerCaseBytesPartByte4 },
            { Template.VariableUpperCaseBytesPartByte5, Template.VariableLowerCaseBytesPartByte5 },
            { Template.VariableUpperCaseBytesPartByte6, Template.VariableLowerCaseBytesPartByte6 },
            { Template.VariableUpperCaseBytesPartByte7, Template.VariableLowerCaseBytesPartByte7 },
            { Template.VariableUpperCaseBytesPartByte8, Template.VariableLowerCaseBytesPartByte8 },
        };

        /// <summary>
        /// Default Template String
        /// </summary>
#if ORIGINAL
        internal static readonly string DefaultFormatString =
              "// {" + "{" + VariableLowerCaseGuidWithHyphens + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerCase1stPart + "},"
            + "0x{" + VariableLowerCase2ndPart + "},"
            + "0x{" + VariableLowerCase3rdPart + "},"
            + "0x{" + VariableLowerCaseBytesPartByte1 + "},"
            + "0x{" + VariableLowerCaseBytesPartByte2 + "},"
            + "0x{" + VariableLowerCaseBytesPartByte3 + "},"
            + "0x{" + VariableLowerCaseBytesPartByte4 + "},"
            + "0x{" + VariableLowerCaseBytesPartByte5 + "},"
            + "0x{" + VariableLowerCaseBytesPartByte6 + "},"
            + "0x{" + VariableLowerCaseBytesPartByte7 + "},"
            + "0x{" + VariableLowerCaseBytesPartByte8 + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "<Guid(\""
            + "{" + VariableLowerCase1stPart + "}-"
            + "{" + VariableLowerCase2ndPart + "}-"
            + "{" + VariableLowerCase3rdPart + "}-"
            + "{" + VariableLowerCaseBytesPartByte1 + "}"
            + "{" + VariableLowerCaseBytesPartByte2 + "}-"
            + "{" + VariableLowerCaseBytesPartByte2 + "}"
            + "{" + VariableLowerCaseBytesPartByte3 + "}"
            + "{" + VariableLowerCaseBytesPartByte4 + "}"
            + "{" + VariableLowerCaseBytesPartByte5 + "}"
            + "{" + VariableLowerCaseBytesPartByte6 + "}"
            + "{" + VariableLowerCaseBytesPartByte7 + "}"
            + "{" + VariableLowerCaseBytesPartByte8 + "}"
            + "\")>"
            + Environment.NewLine;
#else
        internal static readonly string DefaultFormatString =
              "// {" + "{" + VariableLowerCaseGuidWithHyphens + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerCase1stPart + "}," + " "
            + "0x{" + VariableLowerCase2ndPart + "}," + " "
            + "0x{" + VariableLowerCase3rdPart + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte1 + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte2 + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte3 + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte4 + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte5 + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte6 + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte7 + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte8 + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "// {" + "{" + VariableLowerCaseGuidWithHyphens + "(1)" + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerCase1stPart + "(1)" + "}," + " "
            + "0x{" + VariableLowerCase2ndPart + "(1)" + "}," + " "
            + "0x{" + VariableLowerCase3rdPart + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte1 + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte2 + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte3 + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte4 + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte5 + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte6 + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte7 + "(1)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte8 + "(1)" + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "// {" + "{" + VariableLowerCaseGuidWithHyphens + "(2)" + "}" + "}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerCase1stPart + "(2)" + "}," + " "
            + "0x{" + VariableLowerCase2ndPart + "(2)" + "}," + " "
            + "0x{" + VariableLowerCase3rdPart + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte1 + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte2 + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte3 + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte4 + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte5 + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte6 + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte7 + "(2)" + "}," + " "
            + "0x{" + VariableLowerCaseBytesPartByte8 + "(2)" + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine;
#endif

        /// <summary>
        /// Template Variables
        /// </summary>
        internal static readonly VariableManager[] Variables = new VariableManager[]
        {
            new VariableManager(VariableLowerCaseGuidWithHyphens, "Lower-case Full GUID with hypens"),
            new VariableManager(VariableUpperCaseGuidWithHyphens, "Upper-case Full GUID with hypens"),
            new VariableManager(VariableLowerCaseGuidWithoutHyphens, "Lower-case Full GUID without hypens"),
            new VariableManager(VariableUpperCaseGuidWithoutHyphens, "Upper-case Full GUID without hypens"),
            new VariableManager(VariableLowerCase1stPart, "Lower Case 1st Part"),
            new VariableManager(VariableUpperCase1stPart, "Upper Case 1st Part"),
            new VariableManager(VariableLowerCase2ndPart, "Lower Case 2nd Part"),
            new VariableManager(VariableUpperCase2ndPart, "Upper Case 2nd Part"),
            new VariableManager(VariableLowerCase3rdPart, "Lower Case 3rd Part"),
            new VariableManager(VariableUpperCase3rdPart, "Upper Case 3rd Part"),
            new VariableManager(VariableLowerCaseBytesPartByte1, "1st byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte1, "1st byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerCaseBytesPartByte2, "2nd byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte2, "2nd byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerCaseBytesPartByte3, "3rd byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte3, "3rd byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerCaseBytesPartByte4, "4th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte4, "4th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerCaseBytesPartByte5, "5th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte5, "5th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerCaseBytesPartByte6, "6th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte6, "6th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerCaseBytesPartByte7, "7th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte7, "7th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerCaseBytesPartByte8, "8th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperCaseBytesPartByte8, "8th byte of Upper Case Bytes Part"),
        };

        /// <summary>
        /// Template Name for 1st part of Lower Case of GUID
        /// </summary>
        internal static string VariableLowerCase1stPart = "LowerCase1stPart";

        /// <summary>
        /// Template Name for 2nd part of Lower Case of GUID
        /// </summary>
        internal static string VariableLowerCase2ndPart = "LowerCase2ndPart";

        /// <summary>
        /// Template Name for 3rd part of Lower Case of GUID
        /// </summary>
        internal static string VariableLowerCase3rdPart = "LowerCase3rdPart";

        /// <summary>
        /// Template Name for Lower Case 1st byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte1 = "LowerCaseBytesPartByte1";

        /// <summary>
        /// Template Name for Lower Case 2nd byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte2 = "LowerCaseBytesPartByte2";

        /// <summary>
        /// Template Name for Lower Case 3rd byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte3 = "LowerCaseBytesPartByte3";

        /// <summary>
        /// Template Name for Lower Case 4th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte4 = "LowerCaseBytesPartByte4";

        /// <summary>
        /// Template Name for Lower Case 5th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte5 = "LowerCaseBytesPartByte5";

        /// <summary>
        /// Template Name for Lower Case 6th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte6 = "LowerCaseBytesPartByte6";

        /// <summary>
        /// Template Name for Lower Case 7th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte7 = "LowerCaseBytesPartByte7";

        /// <summary>
        /// Template Name for Lower Case 8th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableLowerCaseBytesPartByte8 = "LowerCaseBytesPartByte8";

        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        internal static string VariableLowerCaseGuidWithHyphens = "LowerCaseGuidWithHyphens";

        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        internal static string VariableLowerCaseGuidWithoutHyphens = "LowerCaseGuidWithoutHyphens";

        /// <summary>
        /// Template Name for 1st part of Upper Case of GUID
        /// </summary>
        internal static string VariableUpperCase1stPart = "UpperCase1stPart";

        /// <summary>
        /// Template Name for 2nd part of Upper Case of GUID
        /// </summary>
        internal static string VariableUpperCase2ndPart = "UpperCase2ndPart";

        /// <summary>
        /// Template Name for 3rd part of Upper Case of GUID
        /// </summary>
        internal static string VariableUpperCase3rdPart = "UpperCase3rdPart";

        /// <summary>
        /// Template Name for Upper Case 1st byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte1 = "UpperCaseBytesPartByte1";

        /// <summary>
        /// Template Name for Upper Case 2nd byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte2 = "UpperCaseBytesPartByte2";

        /// <summary>
        /// Template Name for Upper Case 3rd byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte3 = "UpperCaseBytesPartByte3";

        /// <summary>
        /// Template Name for Upper Case 4th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte4 = "UpperCaseBytesPartByte4";

        /// <summary>
        /// Template Name for Upper Case 5th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte5 = "UpperCaseBytesPartByte5";

        /// <summary>
        /// Template Name for Upper Case 6th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte6 = "UpperCaseBytesPartByte6";

        /// <summary>
        /// Template Name for Upper Case 7th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte7 = "UpperCaseBytesPartByte7";

        /// <summary>
        /// Template Name for Upper Case 8th byte of Bytes Part of GUID
        /// </summary>
        internal static string VariableUpperCaseBytesPartByte8 = "UpperCaseBytesPartByte8";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        internal static string VariableUpperCaseGuidWithHyphens = "UpperCaseGuidWithHyphens";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        internal static string VariableUpperCaseGuidWithoutHyphens = "UpperCaseGuidWithoutHyphens";

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
        /// <returns></returns>
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
            values[Template.VariableLowerCaseGuidWithHyphens] = lowerWithHyphens;

            var lowerWithoutHyphens = guid.ToString("N");
            values[Template.VariableLowerCaseGuidWithoutHyphens] = lowerWithoutHyphens;

            var formatGuid = new FormatGuid(guid);
            values[Template.VariableLowerCase1stPart] = formatGuid.Data1.ToString("x8");
            values[Template.VariableLowerCase2ndPart] = formatGuid.Data2.ToString("x4");
            values[Template.VariableLowerCase3rdPart] = formatGuid.Data3.ToString("x4");
            values[Template.VariableLowerCaseBytesPartByte1] = formatGuid.Bytes[0].ToString("x2");
            values[Template.VariableLowerCaseBytesPartByte2] = formatGuid.Bytes[1].ToString("x2");
            values[Template.VariableLowerCaseBytesPartByte3] = formatGuid.Bytes[2].ToString("x2");
            values[Template.VariableLowerCaseBytesPartByte4] = formatGuid.Bytes[3].ToString("x2");
            values[Template.VariableLowerCaseBytesPartByte5] = formatGuid.Bytes[4].ToString("x2");
            values[Template.VariableLowerCaseBytesPartByte6] = formatGuid.Bytes[5].ToString("x2");
            values[Template.VariableLowerCaseBytesPartByte7] = formatGuid.Bytes[6].ToString("x2");
            values[Template.VariableLowerCaseBytesPartByte8] = formatGuid.Bytes[7].ToString("x2");

            // set variables for Upper Case
            foreach (KeyValuePair<string, string> element in CaseMap)
            {
                // ex. Template.VariableUpperCaseGuidWithHyphens
                var targetKeyName = element.Key;

                // ex. Template.VariableLowerCaseGuidWithHyphens
                var sourceKeyName = element.Value;

                var value = values[sourceKeyName];
                values[targetKeyName] = value.ToUpper();
            }

            return values;
        }

        /// <summary>
        /// Replace the template variable to the values defined by dictionary
        /// </summary>
        /// <param name="template">template string to be replace</param>
        /// <param name="values">the values to replace</param>
        /// <returns></returns>
        internal static string ProcessTemplate(string template)
        {
            const int NoIndex = -1;
            const string patternDivide = @"(?<variable>{\w+(\(\d+\))?})";
            const string patternParse = @"{(?<keyword>\w+)(\((?<index>\d+)\))?}";
            var substrings = Regex.Split(template, patternDivide, RegexOptions.ExplicitCapture);
            var regex = new Regex(patternParse, RegexOptions.Compiled);
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
    /// Manager for Variable Name, Description, and Regular Expressioin
    /// </summary>
    internal class VariableManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Keyword">Variable name</param>
        /// <param name="Description">Variable Descriotion</param>
        internal VariableManager(string Keyword, string Description)
        {
            this.Keyword = Keyword;
            this.Description = this.GetVariable() + ": " + Description;
        }

        /// <summary>
        /// Description for Variable
        /// </summary>
        internal string Description { get; set; }

        /// <summary>
        /// Keyword
        /// </summary>
        internal string Keyword { get; set; }

        internal string GetVariable()
        {
            return "{" + this.Keyword + "}";
        }
        internal string GetVariable(int index)
        {
            return string.Format("{{{0}({1})}}", this.Keyword, index);
        }
    }
}
