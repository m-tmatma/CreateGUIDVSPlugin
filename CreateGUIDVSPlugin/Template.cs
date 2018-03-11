﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreateGUIDVSPlugin
{
    /// <summary>
    /// Manager for Variable Name, Description, and Regular Expressioin
    /// </summary>
    internal class VariableManager
    {
        /// <summary>
        /// Variable
        /// </summary>
        internal string Variable { get; set; }

        /// <summary>
        /// Description for Variable
        /// </summary>
        internal string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Variable">Variable</param>
        /// <param name="Description">Variable Descriotion</param>
        internal VariableManager(string Variable, string Description)
        {
            this.Variable = Variable;
            this.Description = this.Variable + " : " + Description;
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
        static internal string VariableLowerCaseGuidWithHyphens = "{LowerCaseGuidWithHyphens}";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        static internal string VariableUpperCaseGuidWithHyphens = "{UpperCaseGuidWithHyphens}";

        /// <summary>
        /// Template Name for Lower Case GUID with Hyphens
        /// </summary>
        static internal string VariableLowerCaseGuidWithoutHyphens = "{LowerCaseGuidWithoutHyphens}";

        /// <summary>
        /// Template Name for Upper Case GUID with Hyphens
        /// </summary>
        static internal string VariableUpperCaseGuidWithoutHyphens = "{UpperCaseGuidWithoutHyphens}";
        /// <summary>
        /// Template Name for 1st part of Lower Case of GUID
        /// </summary>
        static internal string VariableLowerCase1stPart = "{LowerCase1stPart}";

        /// <summary>
        /// Template Name for 1st part of Upper Case of GUID
        /// </summary>
        static internal string VariableUpperCase1stPart = "{UpperCase1stPart}";

        /// <summary>
        /// Template Name for 2nd part of Lower Case of GUID
        /// </summary>
        static internal string VariableLowerCase2ndPart = "{LowerCase2ndPart}";

        /// <summary>
        /// Template Name for 2nd part of Upper Case of GUID
        /// </summary>
        static internal string VariableUpperCase2ndPart = "{UpperCase2ndPart}";

        /// <summary>
        /// Template Name for 3rd part of Lower Case of GUID
        /// </summary>
        static internal string VariableLowerCase3rdPart = "{LowerCase3rdPart}";

        /// <summary>
        /// Template Name for 3rd part of Upper Case of GUID
        /// </summary>
        static internal string VariableUpperCase3rdPart = "{UpperCase3rdPart}";

        /// <summary>
        /// Template Name for Lower Case 1st byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte1 = "{LowerCaseBytesPartByte1}";

        /// <summary>
        /// Template Name for Upper Case 1st byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte1 = "{UpperCaseBytesPartByte1}";

        /// <summary>
        /// Template Name for Lower Case 2nd byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte2 = "{LowerCaseBytesPartByte2}";

        /// <summary>
        /// Template Name for Upper Case 2nd byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte2 = "{UpperCaseBytesPartByte2}";

        /// <summary>
        /// Template Name for Lower Case 3rd byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte3 = "{LowerCaseBytesPartByte3}";

        /// <summary>
        /// Template Name for Upper Case 3rd byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte3 = "{UpperCaseBytesPartByte3}";

        /// <summary>
        /// Template Name for Lower Case 4th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte4 = "{LowerCaseBytesPartByte4}";

        /// <summary>
        /// Template Name for Upper Case 4th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte4 = "{UpperCaseBytesPartByte4}";

        /// <summary>
        /// Template Name for Lower Case 5th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte5 = "{LowerCaseBytesPartByte5}";

        /// <summary>
        /// Template Name for Upper Case 5th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte5 = "{UpperCaseBytesPartByte5}";

        /// <summary>
        /// Template Name for Lower Case 6th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte6 = "{LowerCaseBytesPartByte6}";

        /// <summary>
        /// Template Name for Upper Case 6th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte6 = "{UpperCaseBytesPartByte6}";

        /// <summary>
        /// Template Name for Lower Case 7th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte7 = "{LowerCaseBytesPartByte7}";

        /// <summary>
        /// Template Name for Upper Case 7th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte7 = "{UpperCaseBytesPartByte7}";

        /// <summary>
        /// Template Name for Lower Case 8th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableLowerCaseBytesPartByte8 = "{LowerCaseBytesPartByte8}";

        /// <summary>
        /// Template Name for Upper Case 8th byte of Bytes Part of GUID
        /// </summary>
        static internal string VariableUpperCaseBytesPartByte8 = "{UpperCaseBytesPartByte8}";

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
        internal readonly static string DefaultFormatString = VariableUpperCaseGuidWithHyphens;

        /// <summary>
        /// Table between variables of Upper Case and variables of Lower Case
        /// </summary>
        internal readonly static Dictionary<string, string> CaseMap = new Dictionary<string, string>
        {
            {Template.VariableUpperCaseGuidWithHyphens,    Template.VariableLowerCaseGuidWithHyphens},
            {Template.VariableUpperCaseGuidWithoutHyphens, Template.VariableLowerCaseGuidWithoutHyphens},
            {Template.VariableUpperCase1stPart, Template.VariableLowerCase1stPart},
            {Template.VariableUpperCase2ndPart, Template.VariableLowerCase2ndPart},
            {Template.VariableUpperCase3rdPart, Template.VariableLowerCase3rdPart},
            {Template.VariableUpperCaseBytesPartByte1, Template.VariableLowerCaseBytesPartByte1},
            {Template.VariableUpperCaseBytesPartByte2, Template.VariableLowerCaseBytesPartByte2},
            {Template.VariableUpperCaseBytesPartByte3, Template.VariableLowerCaseBytesPartByte3},
            {Template.VariableUpperCaseBytesPartByte4, Template.VariableLowerCaseBytesPartByte4},
            {Template.VariableUpperCaseBytesPartByte5, Template.VariableLowerCaseBytesPartByte5},
            {Template.VariableUpperCaseBytesPartByte6, Template.VariableLowerCaseBytesPartByte6},
            {Template.VariableUpperCaseBytesPartByte7, Template.VariableLowerCaseBytesPartByte7},
            {Template.VariableUpperCaseBytesPartByte8, Template.VariableLowerCaseBytesPartByte8},
        };

        /// <summary>
        /// Create a dictionary for the template values
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, string> CreateValuesDictionary(Guid guid)
        {
            var values = new Dictionary<string, string>();
            foreach (VariableManager variableManager in Template.Variables)
            {
                values[variableManager.Variable] = string.Empty;
            }

            //
            // see https://msdn.microsoft.com/library/97af8hh4(v=vs.110).aspx about the parameter of Guid.ToString().
            // see https://msdn.microsoft.com/library/system.guid(v=vs.110).aspx
            // see 'Reference Source' link in the above site.
            //
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
        internal static string ProcessTemplate(string template, Dictionary<string, string> values)
        {
            const string pattern = @"({\w+})";
            string[] substrings = Regex.Split(template, pattern);

            string[] replacedStrings = new string[substrings.Length];

            int index = 0;
            foreach (string match in substrings)
            {
                replacedStrings[index] = match;
                if (values.ContainsKey(match))
                {
                    replacedStrings[index] = values[match];
                }
                index++;
            }
            return string.Join(string.Empty, replacedStrings);
        }
    }
}
