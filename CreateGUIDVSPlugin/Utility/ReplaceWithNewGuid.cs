﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CreateGUIDVSPlugin.Utility
{
    public class ReplaceWithNewGuid
    {
        /// <summary>
        /// delegate for creating new GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public delegate Guid NewGuid();

        /// <summary>
        /// hold delegate NewGuid
        /// </summary>
        private NewGuid delegateNewGuid;

        /// <summary>
        /// dictionary for storing previous translation of GUIDs.
        /// </summary>
        private Dictionary<string, Guid> dict = new Dictionary<string, Guid>();

        /// <summary>
        /// Variable
        /// </summary>
        static private string raw_variable = @"(<*)\w+(>*)";

        /// <summary>
        /// 00000000-0000-0000-0000-000000000000
        /// </summary>
        static private string raw_guid_hyphens = "([0-9A-Fa-f]{8})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{12})";

        /// <summary>
        /// 00000000000000000000000000000000
        /// </summary>
        static private string raw_guid_no_hyphens = "([0-9A-Fa-f]{32})";

        /// <summary>
        /// 0x00000000
        /// </summary>
        static private string hex_4byte_string = "(0[xX][0-9A-Fa-f]{1,8})";

        /// <summary>
        /// 0x0000
        /// </summary>
        static private string hex_2byte_string = "(0[xX][0-9A-Fa-f]{1,4})";

        /// <summary>
        /// 0x00
        /// </summary>
        static private string hex_1byte_string = "(0[xX][0-9A-Fa-f]{1,2})";
        static private string spaces = @"\s*";
        static private string comma_spaces = spaces + "," + spaces;

        /// <summary>
        /// 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
        /// </summary>
        static private string hex_8_of_1byte = hex_1byte_string
                                     + comma_spaces + hex_1byte_string
                                     + comma_spaces + hex_1byte_string
                                     + comma_spaces + hex_1byte_string
                                     + comma_spaces + hex_1byte_string
                                     + comma_spaces + hex_1byte_string
                                     + comma_spaces + hex_1byte_string
                                     + comma_spaces + hex_1byte_string;

        /// <summary>
        /// {0x00000000, 0x0000, 0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// </summary>
        static private string raw_string_array = "{"
                                               + spaces
                                               + hex_4byte_string + comma_spaces
                                               + hex_2byte_string + comma_spaces
                                               + hex_2byte_string + comma_spaces
                                               + "{"
                                               + spaces
                                               + hex_8_of_1byte
                                               + spaces
                                               + "}"
                                               + spaces
                                               + "}";

        static private string raw_guid_value = hex_4byte_string + comma_spaces
                                               + hex_2byte_string + comma_spaces
                                               + hex_2byte_string + comma_spaces
                                               + hex_8_of_1byte;
        static private string name_guid_value_def = @"(?<RAW_GUID_DEF>" + raw_guid_value + ")";
        static private string name_guid_value_imp = @"(?<RAW_GUID_IMP>" + raw_guid_value + ")";

        /// <summary>
        /// DEFINE_GUID(<<name>>, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
        /// </summary>
        static private string raw_define_guid = @"(?<="
                                               + @"DEFINE_GUID"
                                               + spaces
                                               + @"\("
                                               + spaces
                                               + raw_variable
                                               + comma_spaces
                                               + @")"
                                               + name_guid_value_def
                                               + @"(?="
                                               + spaces
                                               + @"\)"
                                               + @")";

        /// <summary>
        /// IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
        /// </summary>
        static private string raw_impl_olecreate = @"(?<="
                                               + @"IMPLEMENT_OLECREATE"
                                               + spaces
                                               + @"\("
                                               + spaces
                                               + raw_variable
                                               + comma_spaces
                                               + raw_variable
                                               + comma_spaces
                                               + @")"
                                               + name_guid_value_imp
                                               + @"(?="
                                               + spaces
                                               + @"\)"
                                               + @")";

        // For reference
        // https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/regular-expression-language-quick-reference#backreference_constructs
        // https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/backreference-constructs-in-regular-expressions
        static private string word_separater = @"\b";
        static private string name_guid_hyphens    = word_separater + @"(?<RawHyphenDigits>" + raw_guid_hyphens     + ")" + word_separater;
        static private string name_guid_no_hyphens = word_separater + @"(?<Raw32Digits>"     + raw_guid_no_hyphens  + ")" + word_separater;
        static private string name_string_array    = @"(?<GuidVariable>" + raw_string_array    + ")";
        static private string name_define_guid     = word_separater + @"(?<DEFINE_GUID>" + raw_define_guid     + ")" + word_separater;
        static private string name_impl_olecreate  = word_separater + @"(?<OLECREATE>"   + raw_impl_olecreate  + ")" + word_separater;
        static private string[] elements = new string[]
        {
            name_guid_hyphens,
            name_guid_no_hyphens,
            name_string_array,
            name_define_guid,
            name_impl_olecreate,
        };
        static private string[] elements_par = Array.ConvertAll(elements, delegate (string elem) { return "(" + elem + ")"; });
        static private string guid_string = string.Join("|", elements_par);
        static private Regex reg = new Regex(guid_string);

        /// <summary>
        /// constructor
        /// </summary>
        public ReplaceWithNewGuid(NewGuid newGuid = null)
        {
            if(newGuid != null)
            {
                this.delegateNewGuid = newGuid;
            }
            else
            {
                this.delegateNewGuid = delegate () { return Guid.NewGuid(); };
            }
        }

        /// <summary>
        /// utility function to create GUID.
        /// </summary>
        /// <returns></returns>
        private Guid CallNewGuid()
        {
            return this.delegateNewGuid();
        }

        /// <summary>
        /// delegate for ReplaceNewGuid
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public string delegateReplaceNewGuid(Match m)
        {
            var processGuid = new ProcessGuid(m);
            var newGuid = CallNewGuid();

            var guid_str = processGuid.Convert(newGuid);
            return guid_str;
        }

        /// <summary>
        /// delegate for ReplaceSameGuidToSameGuid
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public string delegateReplaceSameGuidToSameGuid(Match m)
        {
            var processGuid = new ProcessGuid(m);
            var key = processGuid.Key;
            var guid = new Guid(key);
            if (!dict.ContainsKey(key))
            {
                dict[key] = CallNewGuid();
            }
            var newGuid = dict[key];

            var guid_str = processGuid.Convert(newGuid);
            return guid_str;
        }

        /// <summary>
        /// replace GUIDs to new GUIDs.
        /// all GUIDS will be replaced with the different GUIDs.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ReplaceNewGuid(string input)
        {
            //Console.WriteLine(guid_string);
            var myEvaluator = new MatchEvaluator(delegateReplaceNewGuid);

            // Replace matched characters using the delegate method.
            var output = reg.Replace(input, myEvaluator);
            return output;
        }

        /// <summary>
        /// replace GUIDs to new GUIDs.
        /// same GUIDS will be replaced with the same GUIDs.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ReplaceSameGuidToSameGuid(string input)
        {
            //Console.WriteLine(guid_string);
            var myEvaluator = new MatchEvaluator(delegateReplaceSameGuidToSameGuid);

            // Replace matched characters using the delegate method.
            var output = reg.Replace(input, myEvaluator);
            return output;
        }

        public void Dump()
        {
            foreach (KeyValuePair<string, Guid> keyvalue in dict)
            {
                Console.WriteLine("{0}:{1}", keyvalue.Key, keyvalue.Value.ToString());
            }
        }
    }
}
