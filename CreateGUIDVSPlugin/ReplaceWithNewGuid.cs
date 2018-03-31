//-----------------------------------------------------------------------
// <copyright file="ReplaceWithNewGuid.cs" company="Masaru Tsuchiyama">
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
    /// Renew GUIDs
    /// </summary>
    public class ReplaceWithNewGuid
    {
        /// <summary>
        /// regular expression for space and comma.
        /// </summary>
        private static string commaSpaces = spaces + "," + spaces;

        /// <summary>
        /// array of regular expression elements
        /// </summary>
        private static string[] elements = new string[]
        {
            guidString1,
            guidString2,
            nameGuidString3,
            nameGuidString4,
            nameGuidString5,
        };

        /// <summary>
        /// combination of 'elements'
        /// </summary>
        private static string[] elementsPar = Array.ConvertAll(elements, delegate(string elem) { return "(" + elem + ")"; });

        /// <summary>
        /// final regular expression
        /// </summary>
        private static string guidString = string.Join("|", elementsPar);

        /// <summary>
        /// regular expression for hyphen separated GUID
        /// </summary>
        private static string guidString1 = wordSeparator + nameGuidString1 + wordSeparator;

        /// <summary>
        /// regular expression for raw 32-digit GUID
        /// </summary>
        private static string guidString2 = wordSeparator + nameGuidString2 + wordSeparator;

        /// <summary>
        /// regular expression of 1 Byte hex string
        /// 0x00
        /// </summary>
        private static string hex1ByteString = "(0[xX][0-9A-Fa-f]{1,2})";

        /// <summary>
        /// regular expression of 2 Bytes hex string
        /// example: 0x0000
        /// </summary>
        private static string hex2ByteString = "(0[xX][0-9A-Fa-f]{1,4})";

        /// <summary>
        /// regular expression of 4 Bytes hex string
        /// 0x00000000
        /// </summary>
        private static string hex4ByteString = "(0[xX][0-9A-Fa-f]{1,8})";

        /// <summary>
        /// 8 comma separated Byte data
        /// for example: 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
        /// </summary>
        private static string hex8of1Byte = hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString;

        /// <summary>
        /// regular expression for hyphen separated GUID by name-capture
        /// </summary>
        private static string nameGuidString1 = @"(?<RawHyphenDigits>" + rawGuidString1 + ")";

        /// <summary>
        /// regular expression for raw 32-digit GUID by name-capture
        /// </summary>
        private static string nameGuidString2 = @"(?<Raw32Digits>" + rawGuidString2 + ")";

        /// <summary>
        /// regular expression for definition of GUID structure
        /// </summary>
        private static string nameGuidString3 = @"(?<GuidVariable>" + rawStringArray + ")";

        /// <summary>
        /// regular expression for DEFINE_GUID by name-capture
        /// </summary>
        private static string nameGuidString4 = @"(?<DEFINE_GUID>" + rawDefineGuid + ")";

        /// <summary>
        /// regular expression for IMPLEMENT_OLECREATE by name-capture
        /// </summary>
        private static string nameGuidString5 = @"(?<OLECREATE>" + rawImpOlecreate + ")";

        /// <summary>
        /// regular expression for DEFINE_GUID by name-capture
        /// </summary>
        private static string nameGuidValueDef = @"(?<RAW_GUID_DEF>" + rawGuidValue + ")";

        /// <summary>
        /// regular expression for IMPLEMENT_OLECREATE by name-capture
        /// </summary>
        private static string nameGuidValueImp = @"(?<RAW_GUID_IMP>" + rawGuidValue + ")";

        /// <summary>
        /// regular expression for DEFINE_GUID
        /// DEFINE_GUID(&lt;&lt;name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
        /// </summary>
        private static string rawDefineGuid = @"(?<="
                                               + @"DEFINE_GUID"
                                               + spaces
                                               + @"\("
                                               + spaces
                                               + rawVariable
                                               + commaSpaces
                                               + @")"
                                               + nameGuidValueDef
                                               + @"(?="
                                               + spaces
                                               + @"\)"
                                               + @")";

        /// <summary>
        /// hyphen separated GUID
        /// for example: 00000000-0000-0000-0000-000000000000
        /// </summary>
        private static string rawGuidString1 = "([0-9A-Fa-f]{8})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{12})";

        /// <summary>
        /// regular expression for raw 32-digit GUID
        /// for example: 00000000000000000000000000000000
        /// </summary>
        private static string rawGuidString2 = "([0-9A-Fa-f]{32})";

        /// <summary>
        /// regular expression for GUID array.
        /// </summary>
        private static string rawGuidValue = hex4ByteString + commaSpaces
                                                       + hex2ByteString + commaSpaces
                                                       + hex2ByteString + commaSpaces
                                                       + hex8of1Byte;

        /// <summary>
        /// IMPLEMENT_OLECREATE(&lt;&lt;class&gt;&gt;, &lt;&lt;external_name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
        /// </summary>
        private static string rawImpOlecreate = @"(?<="
                                               + @"IMPLEMENT_OLECREATE"
                                               + spaces
                                               + @"\("
                                               + spaces
                                               + rawVariable
                                               + commaSpaces
                                               + rawVariable
                                               + commaSpaces
                                               + @")"
                                               + nameGuidValueImp
                                               + @"(?="
                                               + spaces
                                               + @"\)"
                                               + @")";

        /// <summary>
        /// regular expression for definition of GUID structure
        /// {0x00000000, 0x0000, 0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// </summary>
        private static string rawStringArray = "{"
                                               + spaces
                                               + hex4ByteString + commaSpaces
                                               + hex2ByteString + commaSpaces
                                               + hex2ByteString + commaSpaces
                                               + "{"
                                               + spaces
                                               + hex8of1Byte
                                               + spaces
                                               + "}"
                                               + spaces
                                               + "}";

        /// <summary>
        /// regular expression for Variable 
        /// </summary>
        private static string rawVariable = @"(<*)\w+(>*)";

        /// <summary>
        /// regular expression class to parse
        /// </summary>
        private static Regex reg = new Regex(guidString);

        /// <summary>
        /// regular expression for spaces.
        /// </summary>
        private static string spaces = @"\s*";

        // For reference
        // https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/regular-expression-language-quick-reference#backreference_constructs
        // https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/backreference-constructs-in-regular-expressions

        /// <summary>
        /// regular expression for word separator
        /// </summary>
        private static string wordSeparator = @"\b";

        /// <summary>
        /// hold delegate NewGuid
        /// </summary>
        private NewGuid delegateNewGuid;

        /// <summary>
        /// dictionary for storing previous translation of GUIDs.
        /// </summary>
        private Dictionary<string, Guid> dict = new Dictionary<string, Guid>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceWithNewGuid" /> class.
        /// </summary>
        /// <param name="newGuid">delegate for creating new GUID</param>
        public ReplaceWithNewGuid(NewGuid newGuid = null)
        {
            if (newGuid != null)
            {
                this.delegateNewGuid = newGuid;
            }
            else
            {
                this.delegateNewGuid = delegate { return Guid.NewGuid(); };
            }
        }

        /// <summary>
        /// delegate for creating new GUID
        /// </summary>
        /// <returns>new GUID</returns>
        public delegate Guid NewGuid();

        /// <summary>
        /// debug print method
        /// </summary>
        public void Dump()
        {
            foreach (KeyValuePair<string, Guid> keyvalue in this.dict)
            {
                Console.WriteLine("{0}:{1}", keyvalue.Key, keyvalue.Value.ToString());
            }
        }

        /// <summary>
        /// replace GUIDs to new GUIDs.
        /// all GUIDS will be replaced with the different GUIDs.
        /// </summary>
        /// <param name="input">source text whose GUIDs will be renewed</param>
        /// <returns>converted text whose GUIDs were renewed</returns>
        public string ReplaceNewGuid(string input)
        {
            var myEvaluator = new MatchEvaluator(this.DelegateReplaceNewGuid);

            // Replace matched characters using the delegate method.
            var output = reg.Replace(input, myEvaluator);
            return output;
        }

        /// <summary>
        /// replace GUIDs to new GUIDs.
        /// same GUIDS will be replaced with the same GUIDs.
        /// </summary>
        /// <param name="input">source text whose GUIDs will be renewed</param>
        /// <returns>converted text whose GUIDs were renewed</returns>
        public string ReplaceSameGuidToSameGuid(string input)
        {
            var myEvaluator = new MatchEvaluator(this.DelegateReplaceSameGuidToSameGuid);

            // Replace matched characters using the delegate method.
            var output = reg.Replace(input, myEvaluator);
            return output;
        }

        /// <summary>
        /// utility function to create GUID.
        /// </summary>
        /// <returns>new GUID</returns>
        private Guid CallNewGuid()
        {
            return this.delegateNewGuid();
        }

        /// <summary>
        /// delegate for ReplaceNewGuid
        /// </summary>
        /// <param name="m">Matched information</param>
        /// <returns>return replaced GUID string</returns>
        private string DelegateReplaceNewGuid(Match m)
        {
            var processGuid = new ProcessGuid(m);
            var newGuid = this.CallNewGuid();

            var guid_str = processGuid.Convert(newGuid);
            return guid_str;
        }

        /// <summary>
        /// delegate for ReplaceSameGuidToSameGuid
        /// </summary>
        /// <param name="m">Matched information</param>
        /// <returns>return replaced GUID string</returns>
        private string DelegateReplaceSameGuidToSameGuid(Match m)
        {
            var processGuid = new ProcessGuid(m);
            var key = processGuid.Key;
            var guid = new Guid(key);
            if (!this.dict.ContainsKey(key))
            {
                this.dict[key] = this.CallNewGuid();
            }

            var newGuid = this.dict[key];

            var guid_str = processGuid.Convert(newGuid);
            return guid_str;
        }

        /// <summary>
        /// GUID parser and formatter
        /// </summary>
        private class ProcessGuid
        {
            /// https://msdn.microsoft.com/ja-jp/library/97af8hh4(v=vs.110).aspx
            /// <summary>
            /// table for GUID formatters
            /// </summary>
            private static MapFormat[] tableFormats = new MapFormat[]
            {
                new MapFormat("RawHyphenDigits", Format.RawHyphenDigits, delegate(Guid guid) { return guid.ToString("D"); }),
                new MapFormat("Raw32Digits",     Format.Raw32Digits,     delegate(Guid guid) { return guid.ToString("N"); }),
                new MapFormat("GuidVariable",    Format.GuidVariable,    delegate(Guid guid) { return guid.ToString("X"); }),
                new MapFormat("RAW_GUID_DEF",    Format.DEFINE_GUID,     delegate(Guid guid) { return FormatGuidAsRawValues(guid); }),
                new MapFormat("RAW_GUID_IMP",    Format.OLECREATE,       delegate(Guid guid) { return FormatGuidAsRawValues(guid); }),
            };

            /// <summary>
            /// save parameter of MatchEvaluator delegate
            /// </summary>
            private Match m;

            /// <summary>
            /// save matching GUID format
            /// </summary>
            private MapFormat mapFormat;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProcessGuid" /> class.
            /// </summary>
            /// <param name="m">Matched information</param>
            public ProcessGuid(Match m)
            {
                this.m = m;

                this.Key = string.Empty;
                this.mapFormat = null;
                this.GuidFormat = Format.Unknown;
                foreach (MapFormat mapFormat in tableFormats)
                {
                    if (m.Groups[mapFormat.Key].Success)
                    {
                        Guid guid;
                        var value = m.Groups[mapFormat.Key].Value;

                        // try converting GUID.
                        if (!Guid.TryParse(value, out guid))
                        {
                            var builder = new StringBuilder(value);
                            builder.Replace("0x", string.Empty);
                            builder.Replace(",", string.Empty);
                            builder.Replace(" ", string.Empty);
                            value = builder.ToString();
                        }

                        // retry converting GUID.
                        if (Guid.TryParse(value, out guid))
                        {
                            this.Key = guid.ToString("D");
                        }

                        this.mapFormat = mapFormat;
                        this.GuidFormat = mapFormat.GuidFormat;
                        break;
                    }
                }
            }

            /// <summary>
            /// delegate for converting a guid to a string
            /// </summary>
            /// <param name="guid">GUID to be formatted</param>
            /// <returns>return replaced GUID string</returns>
            private delegate string ConvertGuid(Guid guid);

            /// https://msdn.microsoft.com/ja-jp/library/97af8hh4(v=vs.110).aspx
            /// <summary>
            /// enum of GUID Format
            /// </summary>
            public enum Format
            {
                /// <summary>
                /// reserved. not used.
                /// </summary>
                Unknown,

                /// <summary>
                /// enum definition for hyphen separated GUID
                /// "D": 00000000-0000-0000-0000-000000000000
                /// </summary>
                RawHyphenDigits,

                /// <summary>
                /// enum definition for raw 32-digit GUID
                /// "N": 00000000000000000000000000000000
                /// </summary>
                Raw32Digits,

                /// <summary>
                /// enum definition for GUID structure
                /// "X": {0x00000000, 0x0000, 0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
                /// </summary>
                GuidVariable,

                /// <summary>
                /// enum definition for DEFINE_GUID
                /// DEFINE_GUID(&lt;&lt;name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
                /// </summary>
                DEFINE_GUID,

                /// <summary>
                /// enum definition for IMPLEMENT_OLECREATE
                /// IMPLEMENT_OLECREATE(&lt;&lt;class&gt;&gt;, &lt;&lt;external_name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
                /// </summary>
                OLECREATE,
            }

            /// <summary>
            /// Gets guid format
            /// </summary>
            public Format GuidFormat { get; private set; }

            /// <summary>
            /// Gets key for Guid dictionary
            /// </summary>
            public string Key { get; private set; }

            /// <summary>
            /// Format the guid in the original format
            /// </summary>
            /// <param name="guid">GUID to be formatted</param>
            /// <returns>return replaced GUID string</returns>
            public string Convert(Guid guid)
            {
                if (this.mapFormat != null)
                {
                    return this.mapFormat.DelegateConvertGuid(guid);
                }

                // return original string
                return this.m.ToString();
            }

            /// <summary>
            /// format a GUID to GUID definition string for DEFINE_GUID or IMPLEMENT_OLECREATE
            /// 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
            /// </summary>
            /// <param name="guid">GUID to be formatted</param>
            /// <returns>return replaced GUID string</returns>
            private static string FormatGuidAsRawValues(Guid guid)
            {
                var bytes = guid.ToByteArray();
                var builder = new StringBuilder();
                int i = 0;

                int guid_1st = ((int)bytes[3] << 24) | ((int)bytes[2] << 16) | ((int)bytes[1] << 8) | bytes[0];
                short guid_2nd = (short)(((int)bytes[5] << 8) | bytes[4]);
                short guid_3rd = (short)(((int)bytes[7] << 8) | bytes[6]);

                // 0x00000000
                builder.Append("0x");
                builder.Append(guid_1st.ToString("x8"));
                builder.Append(", ");

                // 0x0000
                builder.Append("0x");
                builder.Append(guid_2nd.ToString("x4"));
                builder.Append(", ");

                // 0x0000
                builder.Append("0x");
                builder.Append(guid_3rd.ToString("x4"));
                builder.Append(", ");

                // 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                for (i = 0; i < 8; i++)
                {
                    builder.Append("0x");
                    builder.Append(bytes[8 + i].ToString("x2"));
                    if (i != 7)
                    {
                        builder.Append(", ");
                    }
                }

                return builder.ToString();
            }

            /// <summary>
            /// class for converting GUID
            /// </summary>
            private class MapFormat
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="MapFormat" /> class.
                /// </summary>
                /// <param name="key">Key Name for Regular expression</param>
                /// <param name="guidFormat">GUID format (currently not used)</param>
                /// <param name="delegateConvertGuid">delegate for Guid formatter</param>
                public MapFormat(string key, Format guidFormat, ConvertGuid delegateConvertGuid)
                {
                    this.Key = key;
                    this.GuidFormat = guidFormat;
                    this.DelegateConvertGuid = delegateConvertGuid;
                }

                /// <summary>
                /// Gets delegate for Guid formatter
                /// </summary>
                public ConvertGuid DelegateConvertGuid { get; private set; }

                /// <summary>
                /// Gets GUID format (currently not used)
                /// </summary>
                public Format GuidFormat { get; private set; }

                /// <summary>
                /// Gets Key Name for Regular expression
                /// </summary>
                public string Key { get; private set; }
            }
        }
    }
}
