using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CreateGUIDVSPlugin.Utility
{
    /// <summary>
    /// Manager for Variable Name, Description, and Regular Expressioin
    /// </summary>
    internal class VariableManager
    {
        /// <summary>
        /// Keyword
        /// </summary>
        internal string Keyword { get; set; }

        /// <summary>
        /// Description for Variable
        /// </summary>
        internal string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Keyword">Variable</param>
        /// <param name="Description">Variable Descriotion</param>
        internal VariableManager(string Keyword, string Description)
        {
            this.Keyword = Keyword;
            this.Description = GetVariable() + ": " + Description;
        }
        internal string GetVariable()
        {
            return "{" + this.Keyword + "}";
        }
        internal string GetVariable(int index)
        {
            return string.Format("{{{0}({1})}}", this.Keyword, index);
        }
    }

    /// <summary>
    /// Template Processing Class
    /// </summary>
    internal class Template
    {
        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        internal const string VariableLowerCaseGuidWithHyphens = "LowerCaseGuidWithHyphens";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        internal const string VariableUpperCaseGuidWithHyphens = "UpperCaseGuidWithHyphens";

        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        internal const string VariableLowerCaseGuidWithoutHyphens = "LowerCaseGuidWithoutHyphens";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        internal const string VariableUpperCaseGuidWithoutHyphens = "UpperCaseGuidWithoutHyphens";
        /// <summary>
        /// Template Name for 1st part of Lower Case of GUID
        /// </summary>
        internal const string VariableLowerCase1stPart = "LowerCase1stPart";

        /// <summary>
        /// Template Name for 1st part of Upper Case of GUID
        /// </summary>
        internal const string VariableUpperCase1stPart = "UpperCase1stPart";

        /// <summary>
        /// Template Name for 2nd part of Lower Case of GUID
        /// </summary>
        internal const string VariableLowerCase2ndPart = "LowerCase2ndPart";

        /// <summary>
        /// Template Name for 2nd part of Upper Case of GUID
        /// </summary>
        internal const string VariableUpperCase2ndPart = "UpperCase2ndPart";

        /// <summary>
        /// Template Name for 3rd part of Lower Case of GUID
        /// </summary>
        internal const string VariableLowerCase3rdPart = "LowerCase3rdPart";

        /// <summary>
        /// Template Name for 3rd part of Upper Case of GUID
        /// </summary>
        internal const string VariableUpperCase3rdPart = "UpperCase3rdPart";

        /// <summary>
        /// Template Name for Lower Case 1st byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte1 = "LowerCaseBytesPartByte1";

        /// <summary>
        /// Template Name for Upper Case 1st byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte1 = "UpperCaseBytesPartByte1";

        /// <summary>
        /// Template Name for Lower Case 2nd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte2 = "LowerCaseBytesPartByte2";

        /// <summary>
        /// Template Name for Upper Case 2nd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte2 = "UpperCaseBytesPartByte2";

        /// <summary>
        /// Template Name for Lower Case 3rd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte3 = "LowerCaseBytesPartByte3";

        /// <summary>
        /// Template Name for Upper Case 3rd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte3 = "UpperCaseBytesPartByte3";

        /// <summary>
        /// Template Name for Lower Case 4th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte4 = "LowerCaseBytesPartByte4";

        /// <summary>
        /// Template Name for Upper Case 4th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte4 = "UpperCaseBytesPartByte4";

        /// <summary>
        /// Template Name for Lower Case 5th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte5 = "LowerCaseBytesPartByte5";

        /// <summary>
        /// Template Name for Upper Case 5th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte5 = "UpperCaseBytesPartByte5";

        /// <summary>
        /// Template Name for Lower Case 6th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte6 = "LowerCaseBytesPartByte6";

        /// <summary>
        /// Template Name for Upper Case 6th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte6 = "UpperCaseBytesPartByte6";

        /// <summary>
        /// Template Name for Lower Case 7th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte7 = "LowerCaseBytesPartByte7";

        /// <summary>
        /// Template Name for Upper Case 7th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte7 = "UpperCaseBytesPartByte7";

        /// <summary>
        /// Template Name for Lower Case 8th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerCaseBytesPartByte8 = "LowerCaseBytesPartByte8";

        /// <summary>
        /// Template Name for Upper Case 8th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperCaseBytesPartByte8 = "UpperCaseBytesPartByte8";

        /// <summary>
        /// class to provide delegate for replacing variable to GUID string.
        /// </summary>
        internal class GuidTranslationHandler
        {
            /// <summary>
            /// GUID cache
            /// </summary>
            private Dictionary<int, Guid> dictGuid = new Dictionary<int, Guid>();

            /// <summary>
            /// delegate for getting a new GUID
            /// </summary>
            private Template.NewGuid newGuid;

            /// <summary>
            /// constructor
            /// </summary>
            internal GuidTranslationHandler(Template.NewGuid newGuid)
            {
                this.newGuid = newGuid;
            }

            /// <summary>
            /// delegate to substitute keywords
            /// </summary>
            /// <param name="keyword">keyword</param>
            /// <param name="index">index</param>
            /// <returns></returns>
            internal string delegateGetNewText(string keyword, int index)
            {
                switch (keyword)
                {
                    case Template.VariableLowerCaseGuidWithHyphens:
                    case Template.VariableLowerCaseGuidWithoutHyphens:

                    case Template.VariableLowerCase1stPart:
                    case Template.VariableLowerCase2ndPart:
                    case Template.VariableLowerCase3rdPart:
                    case Template.VariableLowerCaseBytesPartByte1:
                    case Template.VariableLowerCaseBytesPartByte2:
                    case Template.VariableLowerCaseBytesPartByte3:
                    case Template.VariableLowerCaseBytesPartByte4:
                    case Template.VariableLowerCaseBytesPartByte5:
                    case Template.VariableLowerCaseBytesPartByte6:
                    case Template.VariableLowerCaseBytesPartByte7:
                    case Template.VariableLowerCaseBytesPartByte8:

                    case Template.VariableUpperCaseGuidWithHyphens:
                    case Template.VariableUpperCaseGuidWithoutHyphens:

                    case Template.VariableUpperCase1stPart:
                    case Template.VariableUpperCase2ndPart:
                    case Template.VariableUpperCase3rdPart:
                    case Template.VariableUpperCaseBytesPartByte1:
                    case Template.VariableUpperCaseBytesPartByte2:
                    case Template.VariableUpperCaseBytesPartByte3:
                    case Template.VariableUpperCaseBytesPartByte4:
                    case Template.VariableUpperCaseBytesPartByte5:
                    case Template.VariableUpperCaseBytesPartByte6:
                    case Template.VariableUpperCaseBytesPartByte7:
                    case Template.VariableUpperCaseBytesPartByte8:
                        break;
                    default:
                        throw new ProcessTemplate.InvalidKeywordException(keyword);
                }

                // see https://msdn.microsoft.com/library/97af8hh4(v=vs.110).aspx about the parameter of Guid.ToString().
                // see https://msdn.microsoft.com/library/system.guid(v=vs.110).aspx
                // see 'Reference Source' link in the above site.
                var guid = GetGuid(index);
                var formatGuid = new FormatGuid(guid);
                switch (keyword)
                {
                    case Template.VariableLowerCaseGuidWithHyphens: return guid.ToString("D");
                    case Template.VariableLowerCaseGuidWithoutHyphens: return guid.ToString("N");

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

                    case Template.VariableUpperCaseGuidWithHyphens: return guid.ToString("D").ToUpper();
                    case Template.VariableUpperCaseGuidWithoutHyphens: return guid.ToString("N").ToUpper();

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
                        throw new ProcessTemplate.InvalidKeywordException(keyword);
                }
            }

            /// <summary>
            /// fetch a GUID
            /// </summary>
            /// <param name="index">index</param>
            /// <returns>GUID</returns>
            private Guid GetGuid(int index)
            {
                Guid guid = Guid.Empty;
                if (!dictGuid.ContainsKey(index))
                {
                    guid = newGuid();
                    dictGuid[index] = guid;
                }
                else
                {
                    guid = dictGuid[index];
                }
                return guid;
            }
        }

        /// <summary>
        /// Template Variables
        /// </summary>
        internal readonly static VariableManager[] Variables = new VariableManager[]
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
        /// Default Template String
        /// </summary>
#if ORIGINAL
        internal readonly static string DefaultFormatString =
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
        internal readonly static string DefaultFormatString =
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
        /// delegate for creating new GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal delegate Guid NewGuid();

        /// <summary>
        /// Replace the template variable
        /// </summary>
        /// <param name="template">template string to be replace</param>
        /// <param name="newGuid">delegate to create a GUID</param>
        /// <returns></returns>
        internal static string Process(string template, NewGuid newGuid = null)
        {
            if (newGuid == null)
            {
                newGuid = delegate () { return Guid.NewGuid(); };
            }
            var handler = new GuidTranslationHandler(newGuid);
            var output = ProcessTemplate.ReplaceVariable(template, handler.delegateGetNewText);
            return output;
        }
    }
}
