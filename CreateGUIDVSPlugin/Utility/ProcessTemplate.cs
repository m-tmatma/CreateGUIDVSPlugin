//-----------------------------------------------------------------------
// <copyright file="ProcessTemplate.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CreateGUIDVSPlugin.Utility
{
    using System;
    using System.Text.RegularExpressions;

    public class ProcessTemplate
    {
        /// <summary>
        /// regular expression of left brace for variable
        /// </summary>
        const string regexStrEscapeLeftSep = @"(?<escape_left>{{)";

        /// <summary>
        /// regular expression of right brace for variable
        /// </summary>
        const string regexStrEscapeRightSep = @"(?<escape_right>}})";

        /// <summary>
        /// regular expression of left separator for variable
        /// </summary>
        const string regexStrLeftSep = @"{";

        /// <summary>
        /// regular expression of keyword in variable
        /// </summary>
        const string regexStrkeyword = @"(?<keyword>\w+)";

        /// <summary>
        /// regular expression of index in variable
        /// </summary>
        const string regexStrIndex = @"(?<index>\d+)";

        /// <summary>
        /// regular expression of right separator for variable
        /// </summary>
        const string regexStrRightSep = @"}";

        /// <summary>
        /// regular expression for variable
        /// </summary>
        const string regexVariable = regexStrLeftSep + regexStrkeyword + @"(" + @"\(" + regexStrIndex + @"\)" + @")?" + regexStrRightSep;

        /// <summary>
        /// regular expression of left separator for variable with parentheses
        /// </summary>
        const string regexStrEscapeLeftSepPar = @"(" + regexStrEscapeLeftSep + @")";

        /// <summary>
        /// regular expression of right separator for variable with parentheses
        /// </summary>
        const string regexStrEscapeRightSepPar = @"(" + regexStrEscapeRightSep + @")";

        /// <summary>
        /// regular expression for variable with parentheses
        /// </summary>
        const string regexVariablePar = "(" + regexVariable + ")";

        /// <summary>
        /// regular expression
        /// </summary>
        const string regexStr = "("
            + regexStrEscapeLeftSepPar  + "|"
            + regexStrEscapeRightSepPar + "|"
            + regexVariablePar
            + ")";

        /// <summary>
        /// instance of regular expression
        /// </summary>
        static internal Regex reg = new Regex(regexStr);

        /// <summary>
        /// definition of delegate
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public delegate string GetNewText(string keyword, int index);

        /// <summary>
        /// exception when keyword is not valid
        /// </summary>
        internal class InvalidKeywordException : Exception
        {
            internal InvalidKeywordException(string message)
            : base(message)
            {
            }
        }

        /// <summary>
        /// class to provide a delegate of MatchEvaluator
        /// </summary>
        internal class MatchEvaluatorHandler
        {
            /// <summary>
            /// delegate for processing variables
            /// </summary>
            private GetNewText delegateTranslate;

            /// <summary>
            /// constructor
            /// </summary>
            /// <param name="delegateTranslate"></param>
            public MatchEvaluatorHandler(GetNewText delegateTranslate)
            {
                this.delegateTranslate = delegateTranslate;
            }

            /// <summary>
            /// delegate to be called from MatchEvaluator
            /// </summary>
            /// <param name="m">matched information</param>
            /// <returns>replaced text</returns>
            public string delegateReplace(Match m)
            {
                var groupEscapeLeft  = m.Groups["escape_left"];
                var groupEscapeRight = m.Groups["escape_right"];
                var groupKeyword     = m.Groups["keyword"];
                var groupIndex       = m.Groups["index"];
                if (groupEscapeLeft.Success)
                {
                    return "{";
                }
                else if (groupEscapeRight.Success)
                {
                    return "}";
                }
                else if (groupKeyword.Success)
                {
                    // get matched keyword
                    var keyword = groupKeyword.Value;
                    var outData = keyword;
                    var index = -1;

                    if (groupIndex.Success)
                    {
                        // with index
                        index = int.Parse(groupIndex.Value);
                    }
                    try
                    {
                        outData = this.delegateTranslate(keyword, index);
                    }
                    catch (InvalidKeywordException)
                    {
                        outData = m.Groups[0].Value;
                    }
                    return outData;
                }
                else
                {
                    return m.Groups[0].Value;
                }
            }
        }

        /// <summary>
        /// substitute template variable
        /// </summary>
        /// <param name="input">input text to be processed</param>
        /// <param name="delegateTranslate">delegate for processing variables</param>
        /// <returns>converted text</returns>
        public static string ReplaceVariable(string input, GetNewText delegateTranslate)
        {
            var evaluateHander = new MatchEvaluatorHandler(delegateTranslate);
            var myEvaluator = new MatchEvaluator(evaluateHander.delegateReplace);

            var output = reg.Replace(input, myEvaluator);
            return output;
        }
    }
}
