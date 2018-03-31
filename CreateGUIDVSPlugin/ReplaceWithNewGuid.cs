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
        private static string commaSpaces = spaces + "," + spaces;

        private static string[] elements = new string[]
        {
            guidString1,
            guidString2,
            nameGuidString3,
            nameGuidString4,
            nameGuidString5,
        };

        private static string[] elementsPar = Array.ConvertAll(elements, delegate (string elem) { return "(" + elem + ")"; });

        private static string guidString = string.Join("|", elementsPar);

        private static string guidString1 = wordSeparater + nameGuidString1 + wordSeparater;

        private static string guidString2 = wordSeparater + nameGuidString2 + wordSeparater;

        /// <summary>
        /// 0x00
        /// </summary>
        private static string hex1ByteString = "(0[xX][0-9A-Fa-f]{1,2})";

        /// <summary>
        /// 0x0000
        /// </summary>
        private static string hex2ByteString = "(0[xX][0-9A-Fa-f]{1,4})";

        /// <summary>
        /// 0x00000000
        /// </summary>
        private static string hex4ByteString = "(0[xX][0-9A-Fa-f]{1,8})";

        /// <summary>
        /// 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
        /// </summary>
        private static string hex8of1Byte = hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString
                                     + commaSpaces + hex1ByteString;

        private static string nameGuidString1 = @"(?<RawHyphenDigits>" + rawGuidString1 + ")";

        private static string nameGuidString2 = @"(?<Raw32Digits>" + rawGuidString2 + ")";

        private static string nameGuidString3 = @"(?<GuidVariable>" + rawStringArray + ")";

        private static string nameGuidString4 = @"(?<DEFINE_GUID>" + rawDefineGuid + ")";

        private static string nameGuidString5 = @"(?<OLECREATE>" + rawImpOlecreate + ")";

        private static string nameGuidValueDef = @"(?<RAW_GUID_DEF>" + rawGuidValue + ")";

        private static string nameGuidValueImp = @"(?<RAW_GUID_IMP>" + rawGuidValue + ")";

        /// <summary>
        /// DEFINE_GUID(<<name>>, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
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
        /// 00000000-0000-0000-0000-000000000000
        /// </summary>
        private static string rawGuidString1 = "([0-9A-Fa-f]{8})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{4})-([0-9A-Fa-f]{12})";

        /// <summary>
        /// 00000000000000000000000000000000
        /// </summary>
        private static string rawGuidString2 = "([0-9A-Fa-f]{32})";

        private static string rawGuidValue = hex4ByteString + commaSpaces
                                                       + hex2ByteString + commaSpaces
                                                       + hex2ByteString + commaSpaces
                                                       + hex8of1Byte;

        /// <summary>
        /// IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
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
        /// Variable
        /// </summary>
        private static string rawVariable = @"(<*)\w+(>*)";

        private static Regex reg = new Regex(guidString);

        private static string spaces = @"\s*";

        // For reference
        // https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/regular-expression-language-quick-reference#backreference_constructs
        // https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/backreference-constructs-in-regular-expressions
        private static string wordSeparater = @"\b";

        /// <summary>
        /// hold delegate NewGuid
        /// </summary>
        private NewGuid delegateNewGuid;

        /// <summary>
        /// dictionary for storing previous translation of GUIDs.
        /// </summary>
        private Dictionary<string, Guid> dict = new Dictionary<string, Guid>();

        /// <summary>
        /// constructor
        /// </summary>
        public ReplaceWithNewGuid(NewGuid newGuid = null)
        {
            if (newGuid != null)
            {
                this.delegateNewGuid = newGuid;
            }
            else
            {
                this.delegateNewGuid = delegate () { return Guid.NewGuid(); };
            }
        }

        /// <summary>
        /// delegate for creating new GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public delegate Guid NewGuid();

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
        /// <param name="input"></param>
        /// <returns></returns>
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
        /// <param name="input"></param>
        /// <returns></returns>
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
        /// <param name="m"></param>
        /// <returns></returns>
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

        private class ProcessGuid
        {
            /// <summary>
            /// https://msdn.microsoft.com/ja-jp/library/97af8hh4(v=vs.110).aspx
            /// </summary>
            private static MapFormat[] tableFormats = new MapFormat[] {
                new MapFormat("RawHyphenDigits", Format.RawHyphenDigits, delegate (Guid guid) { return guid.ToString("D"); }),
                new MapFormat("Raw32Digits",     Format.Raw32Digits,     delegate (Guid guid) { return guid.ToString("N"); }),
                new MapFormat("GuidVariable",    Format.GuidVariable,    delegate (Guid guid) { return guid.ToString("X"); }),
                new MapFormat("RAW_GUID_DEF",    Format.DEFINE_GUID,     delegate (Guid guid) { return FormatGuidAsRawValues(guid); }),
                new MapFormat("RAW_GUID_IMP",    Format.OLECREATE,       delegate (Guid guid) { return FormatGuidAsRawValues(guid); }),
            };

            private Match m;

            private MapFormat mapFormat;

            /// <summary>
            /// constructor
            /// </summary>
            /// <param name="m"></param>
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
            /// <param name="guid"></param>
            /// <returns></returns>
            delegate string ConvertGuid(Guid guid);

            /// <summary>
            /// https://msdn.microsoft.com/ja-jp/library/97af8hh4(v=vs.110).aspx
            /// </summary>
            public enum Format
            {
                Unknown,

                /// <summary>
                /// "D": 00000000-0000-0000-0000-000000000000
                /// </summary>
                RawHyphenDigits,

                /// <summary>
                /// "N": 00000000000000000000000000000000
                /// </summary>
                Raw32Digits,

                /// <summary>
                /// "X": {0x00000000, 0x0000, 0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
                /// </summary>
                GuidVariable,

                /// <summary>
                /// DEFINE_GUID(<<name>>, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
                /// </summary>
                DEFINE_GUID,

                /// <summary>
                /// IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
                /// </summary>
                OLECREATE,
            }

            /// <summary>
            /// guid format
            /// </summary>
            public Format GuidFormat { get; private set; }

            /// <summary>
            /// key for Guid dictionary
            /// </summary>
            public string Key { get; private set; }

            /// <summary>
            /// Format the guid in the original format
            /// </summary>
            /// <param name="guid"></param>
            /// <returns></returns>
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
            /// 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
            /// </summary>
            /// <param name="guid"></param>
            /// <returns></returns>
            static string FormatGuidAsRawValues(Guid guid)
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
                public MapFormat(string Key, Format GuidFormat, ConvertGuid delegateConvertGuid)
                {
                    this.Key = Key;
                    this.GuidFormat = GuidFormat;
                    this.DelegateConvertGuid = delegateConvertGuid;
                }

                public ConvertGuid DelegateConvertGuid { get; private set; }

                public Format GuidFormat { get; private set; }

                public string Key { get; private set; }
            }
        }
    }
}
