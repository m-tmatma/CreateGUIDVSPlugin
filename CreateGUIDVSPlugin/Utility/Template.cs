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
        internal const string VariableLowerHyphens = "LowerHyphens";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        internal const string VariableUpperHyphens = "UpperHyphens";

        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        internal const string VariableLowerNoHyphens = "LowerNoHyphens";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        internal const string VariableUpperNoHyphens = "UpperNoHyphens";
        /// <summary>
        /// Template Name for 1st part of Lower Case of GUID
        /// </summary>
        internal const string VariableLowerPart1 = "LowerPart1";

        /// <summary>
        /// Template Name for 1st part of Upper Case of GUID
        /// </summary>
        internal const string VariableUpperPart1 = "UpperPart1";

        /// <summary>
        /// Template Name for 2nd part of Lower Case of GUID
        /// </summary>
        internal const string VariableLowerPart2 = "LowerPart2";

        /// <summary>
        /// Template Name for 2nd part of Upper Case of GUID
        /// </summary>
        internal const string VariableUpperPart2 = "UpperPart2";

        /// <summary>
        /// Template Name for 3rd part of Lower Case of GUID
        /// </summary>
        internal const string VariableLowerPart3 = "LowerPart3";

        /// <summary>
        /// Template Name for 3rd part of Upper Case of GUID
        /// </summary>
        internal const string VariableUpperPart3 = "UpperPart3";

        /// <summary>
        /// Template Name for Lower Case 1st byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes1 = "LowerBytes1";

        /// <summary>
        /// Template Name for Upper Case 1st byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes1 = "UpperBytes1";

        /// <summary>
        /// Template Name for Lower Case 2nd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes2 = "LowerBytes2";

        /// <summary>
        /// Template Name for Upper Case 2nd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes2 = "UpperBytes2";

        /// <summary>
        /// Template Name for Lower Case 3rd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes3 = "LowerBytes3";

        /// <summary>
        /// Template Name for Upper Case 3rd byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes3 = "UpperBytes3";

        /// <summary>
        /// Template Name for Lower Case 4th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes4 = "LowerBytes4";

        /// <summary>
        /// Template Name for Upper Case 4th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes4 = "UpperBytes4";

        /// <summary>
        /// Template Name for Lower Case 5th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes5 = "LowerBytes5";

        /// <summary>
        /// Template Name for Upper Case 5th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes5 = "UpperBytes5";

        /// <summary>
        /// Template Name for Lower Case 6th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes6 = "LowerBytes6";

        /// <summary>
        /// Template Name for Upper Case 6th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes6 = "UpperBytes6";

        /// <summary>
        /// Template Name for Lower Case 7th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes7 = "LowerBytes7";

        /// <summary>
        /// Template Name for Upper Case 7th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes7 = "UpperBytes7";

        /// <summary>
        /// Template Name for Lower Case 8th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableLowerBytes8 = "LowerBytes8";

        /// <summary>
        /// Template Name for Upper Case 8th byte of Bytes Part of GUID
        /// </summary>
        internal const string VariableUpperBytes8 = "UpperBytes8";

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
            /// <exception cref="ProcessTemplate.InvalidKeywordException">Thrown when a keyword is invalid</exception>
            internal string delegateGetNewText(string keyword, int index)
            {
                switch (keyword)
                {
                    case Template.VariableLowerHyphens:
                    case Template.VariableLowerNoHyphens:

                    case Template.VariableLowerPart1:
                    case Template.VariableLowerPart2:
                    case Template.VariableLowerPart3:
                    case Template.VariableLowerBytes1:
                    case Template.VariableLowerBytes2:
                    case Template.VariableLowerBytes3:
                    case Template.VariableLowerBytes4:
                    case Template.VariableLowerBytes5:
                    case Template.VariableLowerBytes6:
                    case Template.VariableLowerBytes7:
                    case Template.VariableLowerBytes8:

                    case Template.VariableUpperHyphens:
                    case Template.VariableUpperNoHyphens:

                    case Template.VariableUpperPart1:
                    case Template.VariableUpperPart2:
                    case Template.VariableUpperPart3:
                    case Template.VariableUpperBytes1:
                    case Template.VariableUpperBytes2:
                    case Template.VariableUpperBytes3:
                    case Template.VariableUpperBytes4:
                    case Template.VariableUpperBytes5:
                    case Template.VariableUpperBytes6:
                    case Template.VariableUpperBytes7:
                    case Template.VariableUpperBytes8:
                        break;
                    default:
                        throw new ProcessTemplate.InvalidKeywordException(-1, keyword);
                }

                // see https://msdn.microsoft.com/library/97af8hh4(v=vs.110).aspx about the parameter of Guid.ToString().
                // see https://msdn.microsoft.com/library/system.guid(v=vs.110).aspx
                // see 'Reference Source' link in the above site.
                var guid = GetGuid(index);
                var formatGuid = new FormatGuid(guid);
                switch (keyword)
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
                        throw new ProcessTemplate.InvalidKeywordException(-1, keyword);
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
            new VariableManager(VariableLowerHyphens, "Lower-case Full GUID with hypens"),
            new VariableManager(VariableUpperHyphens, "Upper-case Full GUID with hypens"),
            new VariableManager(VariableLowerNoHyphens, "Lower-case Full GUID without hypens"),
            new VariableManager(VariableUpperNoHyphens, "Upper-case Full GUID without hypens"),
            new VariableManager(VariableLowerPart1, "Lower Case 1st Part"),
            new VariableManager(VariableUpperPart1, "Upper Case 1st Part"),
            new VariableManager(VariableLowerPart2, "Lower Case 2nd Part"),
            new VariableManager(VariableUpperPart2, "Upper Case 2nd Part"),
            new VariableManager(VariableLowerPart3, "Lower Case 3rd Part"),
            new VariableManager(VariableUpperPart3, "Upper Case 3rd Part"),
            new VariableManager(VariableLowerBytes1, "1st byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes1, "1st byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerBytes2, "2nd byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes2, "2nd byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerBytes3, "3rd byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes3, "3rd byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerBytes4, "4th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes4, "4th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerBytes5, "5th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes5, "5th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerBytes6, "6th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes6, "6th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerBytes7, "7th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes7, "7th byte of Upper Case Bytes Part"),
            new VariableManager(VariableLowerBytes8, "8th byte of Lower Case Bytes Part"),
            new VariableManager(VariableUpperBytes8, "8th byte of Upper Case Bytes Part"),
        };
        /// <summary>
        /// Default Template String
        /// </summary>
#if ORIGINAL
        internal readonly static string DefaultFormatString =
              "// {{" + "{" + VariableLowerHyphens + "}" + "}}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerPart1 + "},"
            + "0x{" + VariableLowerPart2 + "},"
            + "0x{" + VariableLowerPart3 + "},"
            + "0x{" + VariableLowerBytes1 + "},"
            + "0x{" + VariableLowerBytes2 + "},"
            + "0x{" + VariableLowerBytes3 + "},"
            + "0x{" + VariableLowerBytes4 + "},"
            + "0x{" + VariableLowerBytes5 + "},"
            + "0x{" + VariableLowerBytes6 + "},"
            + "0x{" + VariableLowerBytes7 + "},"
            + "0x{" + VariableLowerBytes8 + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "<Guid(\""
            + "{" + VariableLowerPart1 + "}-"
            + "{" + VariableLowerPart2 + "}-"
            + "{" + VariableLowerPart3 + "}-"
            + "{" + VariableLowerBytes1 + "}"
            + "{" + VariableLowerBytes2 + "}-"
            + "{" + VariableLowerBytes2 + "}"
            + "{" + VariableLowerBytes3 + "}"
            + "{" + VariableLowerBytes4 + "}"
            + "{" + VariableLowerBytes5 + "}"
            + "{" + VariableLowerBytes6 + "}"
            + "{" + VariableLowerBytes7 + "}"
            + "{" + VariableLowerBytes8 + "}"
            + "\")>"
            + Environment.NewLine;
#else
        internal readonly static string DefaultFormatString =
              "// {{" + "{" + VariableLowerHyphens + "}" + "}}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerPart1 + "}," + " "
            + "0x{" + VariableLowerPart2 + "}," + " "
            + "0x{" + VariableLowerPart3 + "}," + " "
            + "0x{" + VariableLowerBytes1 + "}," + " "
            + "0x{" + VariableLowerBytes2 + "}," + " "
            + "0x{" + VariableLowerBytes3 + "}," + " "
            + "0x{" + VariableLowerBytes4 + "}," + " "
            + "0x{" + VariableLowerBytes5 + "}," + " "
            + "0x{" + VariableLowerBytes6 + "}," + " "
            + "0x{" + VariableLowerBytes7 + "}," + " "
            + "0x{" + VariableLowerBytes8 + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "// {{" + "{" + VariableLowerHyphens + "(1)" + "}" + "}}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerPart1 + "(1)" + "}," + " "
            + "0x{" + VariableLowerPart2 + "(1)" + "}," + " "
            + "0x{" + VariableLowerPart3 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes1 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes2 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes3 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes4 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes5 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes6 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes7 + "(1)" + "}," + " "
            + "0x{" + VariableLowerBytes8 + "(1)" + "}"
            + ");"
            + Environment.NewLine
            + Environment.NewLine
            + "// {{" + "{" + VariableLowerHyphens + "(2)" + "}" + "}}"
            + Environment.NewLine
            + "DEFINE_GUID(<<name>>, "
            + "0x{" + VariableLowerPart1 + "(2)" + "}," + " "
            + "0x{" + VariableLowerPart2 + "(2)" + "}," + " "
            + "0x{" + VariableLowerPart3 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes1 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes2 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes3 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes4 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes5 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes6 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes7 + "(2)" + "}," + " "
            + "0x{" + VariableLowerBytes8 + "(2)" + "}"
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
        /// <exception cref="ProcessTemplate.OrphanedLeftBraceException">Thrown when an orphaned '{' is found</exception>
        /// <exception cref="ProcessTemplate.OrphanedRightBraceException">Thrown when an orphaned '}' is found</exception>
        internal static string Process(string template, NewGuid newGuid = null)
        {
            if (newGuid == null)
            {
                newGuid = delegate () { return Guid.NewGuid(); };
            }

            // the internal method ProcessTemplate.MatchEvaluatorHandler.delegateReplace() catches
            // the exception 'ProcessTemplate.InvalidKeywordException' thrown by
            // GuidTranslationHandler.delegateGetNewText()
            var handler = new GuidTranslationHandler(newGuid);
            var output = ProcessTemplate.ReplaceVariable(template, handler.delegateGetNewText);
            return output;
        }
    }
}
