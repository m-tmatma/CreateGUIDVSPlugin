//-----------------------------------------------------------------------
// <copyright file="MultipleFormat.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitTests
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using CreateGUIDVSPlugin.Utility;

    /// <summary>
    /// unit test for ReplaceWithNewGuid
    /// </summary>
    [TestFixture]
    public class MultipleFormat
    {
        /// <summary>
        /// GUID generator class to make unit-testing easier
        /// </summary>
        private GuidQueue dstGuidQueue;

        /// <summary>
        /// GUID generator class to make unit-testing easier
        /// </summary>
        private GuidQueue srcGuidQueue;

        /// <summary>
        /// enum of GUID Format
        /// </summary>
        /// <see href="https://msdn.microsoft.com/en-us/library/97af8hh4(v=vs.110).aspx">Guid.ToString Method (String)</see>
        private enum ValidFormat
        {
            /// <summary>
            /// enum definition for hyphen separated GUID
            /// "D": 00000000-0000-0000-0000-000000000000
            /// </summary>
            TypeD,

            /// <summary>
            /// enum definition for hyphen separated GUID
            /// "B": {00000000-0000-0000-0000-000000000000}
            /// </summary>
            TypeB,

            /// <summary>
            /// enum definition for hyphen separated GUID
            /// "P": (00000000-0000-0000-0000-000000000000)
            /// </summary>
            TypeP,

            /// <summary>
            /// enum definition for raw 32-digit GUID
            /// "N": 00000000000000000000000000000000
            /// </summary>
            TypeN,

            /// <summary>
            /// enum definition for GUID structure
            /// "X": {0x00000000, 0x0000, 0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
            /// </summary>
            TypeX,

            /// <summary>
            /// enum definition for DEFINE_GUID
            /// DEFINE_GUID(&lt;&lt;name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
            /// </summary>
            TypeDEFINE_GUID,

            /// <summary>
            /// enum definition for IMPLEMENT_OLECREATE
            /// IMPLEMENT_OLECREATE(&lt;&lt;class&gt;&gt;, &lt;&lt;external_name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
            /// </summary>
            TypeOLECREATE,
        }

        /// <summary>
        /// enum of Invalid GUID Format
        /// </summary>
        private enum InvalidFormat
        {
            /// <summary>
            /// enum definition for wrong DEFINE_GUID
            /// DEFINE_GUID2(&lt;&lt;name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
            /// </summary>
            TypeWrongDEFINE_GUID,

            /// <summary>
            /// enum definition for wrong IMPLEMENT_OLECREATE
            /// IMPLEMENT_OLECREATE2(&lt;&lt;class&gt;&gt;, &lt;&lt;external_name&gt;&gt;, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
            /// </summary>
            TypeWrongOLECREATE,
        }

        /// <summary>
        /// target method in ReplaceWithNewGuid
        /// </summary>
        public enum TestMethod
        {
            /// <summary>
            /// ReplaceWithNewGuid.ReplaceNewGuid
            /// </summary>
            ReplaceNewGuid,

            /// <summary>
            /// ReplaceWithNewGuid.ReplaceSameGuidToSameGuid
            /// </summary>
            ReplaceSameGuidToSameGuid,
        }

        /// <summary>
        /// setup method to be called to initialize unit tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.dstGuidQueue = new GuidQueue();
            this.srcGuidQueue = new GuidQueue();
        }

        /// <summary>
        /// cleanup method to be called to cleanup unit tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.dstGuidQueue = null;
            this.srcGuidQueue = null;
        }

        /// <summary>
        /// delegate for formatting GUID string (valid format)
        /// </summary>
        /// <param name="guid">GUID to be formatted</param>
        /// <param name="destFormat">format type</param>
        delegate void ValidFormatGuid(StringBuilder builder, Guid guid, ValidFormat destFormat);

        /// <summary>
        /// delegate for formatting GUID string (invalid format)
        /// </summary>
        /// <param name="guid">GUID to be formatted</param>
        /// <param name="destFormat">format type</param>
        delegate void InvalidFormatGuid(StringBuilder builder, Guid guid, InvalidFormat destFormat);

        /// <summary>
        /// utility function for ReplaceSameGuidToSameGuid
        /// </summary>
        /// <param name="count">loop count</param>
        /// <param name="repeat">repeat count</param>
        /// <param name="srcMethod">GUID generation method for source GUIDs</param>
        /// <param name="dstMethod">GUID generation method for destination GUIDs</param>
        [Test, Pairwise]
        public void TestGuidByRandomValidFormat(
            [Values(3, 5, 7, 10, 20, 50, 100)] int count,
            [Values(2, 5)] int repeat,
            [Values] TestMethod testMethod,
            [Values] GuidQueue.GuidGeneratorMethod srcMethod,
            [Values] GuidQueue.GuidGeneratorMethod dstMethod)
        {
            this.srcGuidQueue.GuidMethod = srcMethod;
            this.dstGuidQueue.GuidMethod = dstMethod;

            var builderInput = new StringBuilder();
            var builderResult = new StringBuilder();

            Array values = Enum.GetValues(typeof(ValidFormat));
            var random = new Random();

            // create source data
            for (int i = 0; i < count; i++)
            {
                // choose enum 'Format' randomly
                var format = (ValidFormat)values.GetValue(random.Next(values.Length));

                var separator = string.Format("------ {0} ------", i);
#if PRINTF_DEBUG
                Console.WriteLine(i.ToString() + ": " + format.ToString());
#endif // PRINTF_DEBUG

                var srcGuid = this.srcGuidQueue.BrandNewGuidNoCache();
                var dstGuid = Guid.Empty;
                var validDstGuid = false;
                if (testMethod == TestMethod.ReplaceSameGuidToSameGuid)
                {
                    dstGuid = this.dstGuidQueue.BrandNewGuid();
                    validDstGuid = true;
                }

                ValidFormatGuid formatGuid = delegate(StringBuilder builder, Guid guid, ValidFormat destFormat)
                {
                    builder.Append(separator);
                    builder.Append(Environment.NewLine);
                    builder.Append(FormatGuidString(guid, destFormat));
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);
                };

                for(int j = 0; j < repeat; j++)
                {
                    // create input data
                    formatGuid(builderInput, srcGuid, format);

                    if (testMethod == TestMethod.ReplaceNewGuid)
                    {
                        dstGuid = this.dstGuidQueue.BrandNewGuid();
                        validDstGuid = true;
                    }

                    // check whether dstGuid was set to valid value.
                    Assert.That(validDstGuid, Is.EqualTo(true));

                    // create expected data
                    formatGuid(builderResult, dstGuid, format);
                }
            }

            var input = builderInput.ToString();
            var expected = builderResult.ToString();

            var replaceWithGuid = new ReplaceWithNewGuid(this.dstGuidQueue.NewGuidFromCache);
            var output = CallMethodOfReplaceWithNewGuid(replaceWithGuid, testMethod, input);

#if PRINTF_DEBUG
            Console.WriteLine("input: ");
            Console.WriteLine(input);

            Console.WriteLine("output: ");
            Console.WriteLine(output);

            Console.WriteLine("expected: ");
            Console.WriteLine(expected);
#endif // PRINTF_DEBUG

            Assert.That(output, Is.EqualTo(expected));
        }

        /// <summary>
        /// utility function for ReplaceSameGuidToSameGuid
        /// </summary>
        /// <param name="count">loop count</param>
        /// <param name="repeat">repeat count</param>
        /// <param name="testMethod">test method</param>
        /// <param name="newSrcGuid">delegate for creating new source GUIDs</param>
        [Test, Pairwise]
        public void TestGuidByRandomInvalidFormat(
            [Values(3, 5, 7, 10, 20, 50, 100)] int count,
            [Values(2, 5)] int repeat,
            [Values] TestMethod testMethod,
            [Values] GuidQueue.GuidGeneratorMethod srcMethod,
            [Values] GuidQueue.GuidGeneratorMethod dstMethod)
        {
            this.srcGuidQueue.GuidMethod = srcMethod;
            this.dstGuidQueue.GuidMethod = dstMethod;

            var builderInput = new StringBuilder();

            Array values = Enum.GetValues(typeof(InvalidFormat));
            var random = new Random();

            // create source data
            for (int i = 0; i < count; i++)
            {
                // choose enum 'Format' randomly
                var format = (InvalidFormat)values.GetValue(random.Next(values.Length));

                var separator = string.Format("------ {0} ------", i);
#if PRINTF_DEBUG
                Console.WriteLine(i.ToString() + ": " + format.ToString());
#endif // PRINTF_DEBUG

                var srcGuid = this.srcGuidQueue.BrandNewGuidNoCache();
                var dstGuid = this.dstGuidQueue.BrandNewGuid();

                InvalidFormatGuid formatGuid = delegate (StringBuilder builder, Guid guid, InvalidFormat destFormat)
                {
                    builder.Append(separator);
                    builder.Append(Environment.NewLine);
                    builder.Append(FormatGuidString(guid, destFormat));
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);
                };

                for (int j = 0; j < repeat; j++)
                {
                    // create input data
                    formatGuid(builderInput, srcGuid, format);
                }
            }

            var input = builderInput.ToString();
            var replaceWithGuid = new ReplaceWithNewGuid(this.dstGuidQueue.NewGuidFromCache);
            var output = CallMethodOfReplaceWithNewGuid(replaceWithGuid, testMethod, input);

#if PRINTF_DEBUG
            Console.WriteLine("input: ");
            Console.WriteLine(input);

            Console.WriteLine("output: ");
            Console.WriteLine(output);
#endif // PRINTF_DEBUG

            Assert.That(output, Is.EqualTo(input));
        }

        /// <summary>
        /// call a method of ReplaceWithNewGuid
        /// </summary>
        /// <param name="replaceWithGuid">instance of ReplaceWithNewGuid</param>
        /// <param name="testMethod">invoke method</param>
        /// <param name="input">input data</param>
        /// <returns>result of ReplaceWithNewGuid</returns>
        private string CallMethodOfReplaceWithNewGuid(ReplaceWithNewGuid replaceWithGuid, TestMethod testMethod, string input)
        {
            var output = string.Empty;
            switch (testMethod)
            {
                case TestMethod.ReplaceNewGuid:
                    output = replaceWithGuid.ReplaceNewGuid(input);
                    break;
                case TestMethod.ReplaceSameGuidToSameGuid:
                    output = replaceWithGuid.ReplaceSameGuidToSameGuid(input);
                    break;
                default:
                    throw new ArgumentException(testMethod.ToString());
            }
            return output;
        }

        /// <summary>
        /// return GUID as string (valid)
        /// </summary>
        /// <param name="guid">a GUID to be formated</param>
        /// <param name="format">valid format</param>
        /// <returns>formatted GUID string</returns>
        /// <see href="https://msdn.microsoft.com/en-us/library/97af8hh4(v=vs.110).aspx">Guid.ToString Method (String)</see>
        private static string FormatGuidString(Guid guid, ValidFormat format)
        {
            switch (format)
            {
                case ValidFormat.TypeN:
                    return guid.ToString("N");
                case ValidFormat.TypeD:
                    return guid.ToString("D");
                case ValidFormat.TypeB:
                    return guid.ToString("B");
                case ValidFormat.TypeP:
                    return guid.ToString("P");
                case ValidFormat.TypeX:
                    return guid.ToString("X");
                case ValidFormat.TypeOLECREATE:
                    return FormatGuidAsImplementOleCreate(guid);
                case ValidFormat.TypeDEFINE_GUID:
                    return FormatGuidAsDefineGuid(guid);
                default:
                    throw new ArgumentException(format.ToString());
            }
        }

        /// <summary>
        /// return GUID as string (invalid)
        /// </summary>
        /// <param name="guid">a GUID to be formated</param>
        /// <param name="format">invalid format</param>
        /// <returns>formatted GUID string</returns>
        /// <see href="https://msdn.microsoft.com/en-us/library/97af8hh4(v=vs.110).aspx">Guid.ToString Method (String)</see>
        private static string FormatGuidString(Guid guid, InvalidFormat format)
        {
            switch (format)
            {
                case InvalidFormat.TypeWrongDEFINE_GUID:
                    return FormatGuidAsImplementOleCreate(guid, "DEFINE_GUID2");
                case InvalidFormat.TypeWrongOLECREATE:
                    return FormatGuidAsDefineGuid(guid, "IMPLEMENT_OLECREATE2");
                default:
                    throw new ArgumentException(format.ToString());
            }
        }

        /// <summary>
        /// IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, 0x00000000,0x0000,0x0000,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
        /// </summary>
        /// <param name="guid">a GUID to be formated</param>
        /// <param name="defineGuid">IMPLEMENT_OLECREATE tag</param>
        /// <param name="variableName1">class tag</param>
        /// <param name="variableName2">external_name tag</param>
        /// <returns>formatted GUID string</returns>
        private static string FormatGuidAsImplementOleCreate(Guid guid, string macroName = "IMPLEMENT_OLECREATE", string variableName1 = "class", string variableName2 = "external_name")
        {
            var builder = new StringBuilder();
            builder.Append(macroName);
            builder.Append("(");
            builder.Append(variableName1);
            builder.Append(",");
            builder.Append(variableName2);
            builder.Append(",");
            builder.Append(FormatGuidAsRawValues(guid));
            builder.Append(");");
            return builder.ToString();
        }

        /// <summary>
        /// DEFINE_GUID(name, 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00);
        /// </summary>
        /// <param name="guid">a GUID to be formated</param>
        /// <param name="macroName">DEFINE_GUID tag</param>
        /// <param name="variableName">variable tag</param>
        /// <returns>formatted GUID string</returns>
        private static string FormatGuidAsDefineGuid(Guid guid, string macroName = "DEFINE_GUID", string variableName = "name")
        {
            var builder = new StringBuilder();
            builder.Append(macroName);
            builder.Append("(");
            builder.Append(variableName);
            builder.Append(",");
            builder.Append(FormatGuidAsRawValues(guid));
            builder.Append(");");
            return builder.ToString();
        }

        /// <summary>
        /// 0x00000000,0x0000,0x0000, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
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
    }
}
