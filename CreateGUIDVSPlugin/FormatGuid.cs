//-----------------------------------------------------------------------
// <copyright file="FormatGuid.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace CreateGUIDVSPlugin
{
    using System;

   /// <summary>
    /// GUID formatter
    /// </summary>
    public class FormatGuid
    {
        /// <summary>
        /// Offset to byte data part in GUID.
        /// </summary>
        private const int Offset = 8;

        /// <summary>
        /// size of byte data part in GUID.
        /// </summary>
        private const int ArraySize = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatGuid" /> class.
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
            for (var i = 0; i < ArraySize; i++)
            {
                bytesPart[i] = guidBytes[i + Offset];
            }

            this.Bytes = bytesPart;
        }

        /// <summary>
        /// Gets GUID
        /// </summary>
        public Guid OrgData { get; }

        /// <summary>
        /// Gets GUID (first part)
        /// </summary>
        public int Data1 { get; }

        /// <summary>
        /// Gets GUID (second part)
        /// </summary>
        public short Data2 { get; }

        /// <summary>
        /// Gets GUID (third part)
        /// </summary>
        public short Data3 { get; }

        /// <summary>
        /// Gets GUID (Bytes part)
        /// </summary>
        public byte[] Bytes { get; }
    }
}
