using System;
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
        /// Template Variables
        /// </summary>
        internal readonly static VariableManager[] Variables = new VariableManager[]
        {
            new VariableManager(VariableLowerCaseGuidWithHyphens, "Lower-case Full GUID with hypens"),
            new VariableManager(VariableUpperCaseGuidWithHyphens, "Upper-case Full GUID with hypens"),
            new VariableManager(VariableLowerCaseGuidWithoutHyphens, "Lower-case Full GUID without hypens"),
            new VariableManager(VariableUpperCaseGuidWithoutHyphens, "Upper-case Full GUID without hypens"),
        };

        /// <summary>
        /// Default Template String
        /// </summary>
        internal readonly static string DefaultFormatString = VariableUpperCaseGuidWithHyphens;

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
