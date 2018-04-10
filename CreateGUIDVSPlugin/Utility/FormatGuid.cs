using System;

namespace GuidTools
{
    /// <summary>
    /// GUID formatter
    /// </summary>
    public class FormatGuid
    {
        private const int Offset = 8;
        private const int ArraySize = 8;

        /// <summary>
        /// format a GUID
        /// </summary>
        /// <param name="guid">GUID to format</param>
        public FormatGuid(Guid guid)
        {
            this.OrgData = guid;

            var guidBytes = this.OrgData.ToByteArray();
            this.Data1 = ((int)guidBytes[3] << 24) | ((int)guidBytes[2] << 16) | ((int)guidBytes[1] << 8) | guidBytes[0];
            this.Data2 = (short)(((int)guidBytes[5] << 8) | guidBytes[4]);
            this.Data3 = (short)(((int)guidBytes[7] << 8) | guidBytes[6]);

            var bytesPart = new byte[ArraySize];
            for(var i = 0; i < ArraySize; i++)
            {
                bytesPart[i] = guidBytes[i + Offset];
            }
            this.Bytes = bytesPart;
        }

        /// <summary>
        /// GUID
        /// </summary>
        public Guid OrgData { get; }

        /// <summary>
        /// GUID (first part)
        /// </summary>
        public int Data1 { get; }

        /// <summary>
        /// GUID (second part)
        /// </summary>
        public short Data2 { get; }

        /// <summary>
        /// GUID (third part)
        /// </summary>
        public short Data3 { get; }

        /// <summary>
        /// GUID (Bytes part)
        /// </summary>
        public byte[] Bytes { get; }
    }
}
