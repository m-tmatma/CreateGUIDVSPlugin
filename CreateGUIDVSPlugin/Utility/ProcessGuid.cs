using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CreateGUIDVSPlugin.Utility
{
    internal class ProcessGuid
    {
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
        };

        /// <summary>
        /// delegate for converting a guid to a string
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        delegate string ConvertGuid(Guid guid);

        /// <summary>
        /// class for converting GUID
        /// </summary>
        private class MapFormat
        {
            public string Key { get; private set; }
            public Format GuidFormat { get; private set; }
            public ConvertGuid delegateConvertGuid { get; private set; }

            public MapFormat(string Key, Format GuidFormat, ConvertGuid delegateConvertGuid)
            {
                this.Key = Key;
                this.GuidFormat = GuidFormat;
                this.delegateConvertGuid = delegateConvertGuid;
            }
        };

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
                builder.Append(bytes[8+i].ToString("x2"));
                if (i != 7)
                {
                    builder.Append(", ");
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/97af8hh4(v=vs.110).aspx
        /// </summary>
        static private MapFormat[] tableFormats = new MapFormat[] {
            new MapFormat("RawHyphenDigits", Format.RawHyphenDigits, delegate (Guid guid) { return guid.ToString("D"); }),
            new MapFormat("Raw32Digits",     Format.Raw32Digits,     delegate (Guid guid) { return guid.ToString("N"); }),
            new MapFormat("GuidVariable",    Format.GuidVariable,    delegate (Guid guid) { return guid.ToString("X"); }),
            new MapFormat("RAW_GUID_DEF",    Format.DEFINE_GUID,     delegate (Guid guid) { return FormatGuidAsRawValues(guid); }),
            new MapFormat("RAW_GUID_IMP",    Format.OLECREATE,       delegate (Guid guid) { return FormatGuidAsRawValues(guid); }),
        };

        private Match m;

        private MapFormat mapFormat;

        /// <summary>
        /// key for Guid dictionary
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// guid format
        /// </summary>
        public Format GuidFormat { get; private set; }

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
                        builder.Replace("0x", "");
                        builder.Replace(",", "");
                        builder.Replace(" ", "");
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
        /// Format the guid in the original format
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string Convert(Guid guid)
        {
            if (this.mapFormat != null)
            {
                return this.mapFormat.delegateConvertGuid(guid);
            }

            // return original string
            return this.m.ToString();
        }
    }
}
