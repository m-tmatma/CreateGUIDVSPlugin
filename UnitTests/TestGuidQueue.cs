//-----------------------------------------------------------------------
// <copyright file="TestGuidQueue.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitTests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// unit test for GuidQueue
    /// </summary>
    [TestFixture]
    public class TestGuidQueue
    {
        /// <summary>
        /// GUID queue
        /// </summary>
        private GuidQueue guidQueue;

        /// <summary>
        /// Guid Method
        /// </summary>
        public enum GuidGeneratorMethod
        {
            Normal,
            GuidGenerater,
        }

        /// <summary>
        /// setup method to be called to initialize unit tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.guidQueue = new GuidQueue();
        }

        /// <summary>
        /// cleanup method to be called to cleanup unit tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.guidQueue = null;
        }

        /// <summary>
        /// Test for repeating GUID generation One by One
        /// </summary>
        /// <param name="count">loop count</param>
        /// <param name="method">GUID generation method</param>
        [TestCase(3, GuidGeneratorMethod.Normal)]
        [TestCase(3, GuidGeneratorMethod.GuidGenerater)]
        [TestCase(10, GuidGeneratorMethod.Normal)]
        [TestCase(10, GuidGeneratorMethod.GuidGenerater)]
        public void TestRepeatOneByOne(int count, GuidGeneratorMethod method)
        {
            SetGuidGenerationMethod(method);

            for (int i = 0; i < count; i++)
            {
                var guid1 = this.guidQueue.BrandNewGuid();
                var guid2 = this.guidQueue.NewGuidFromCache();
                Assert.That(guid1, Is.EqualTo(guid2));
            }
            Assert.That(() => { this.guidQueue.NewGuidFromCache(); }, Throws.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Test for repeating GUID generation all together
        /// </summary>
        /// <param name="count">loop count</param>
        /// <param name="method">GUID generation method</param>
        [TestCase(3, GuidGeneratorMethod.Normal)]
        [TestCase(3, GuidGeneratorMethod.GuidGenerater)]
        [TestCase(10, GuidGeneratorMethod.Normal)]
        [TestCase(10, GuidGeneratorMethod.GuidGenerater)]
        public void TestRepeatAllTogether(int count, GuidGeneratorMethod method)
        {
            SetGuidGenerationMethod(method);

            var guids = new Guid[count];
            for (int i = 0; i < count; i++)
            {
                guids[i] = this.guidQueue.BrandNewGuid();
            }
            for (int i = 0; i < count; i++)
            {
                var guid2 = this.guidQueue.NewGuidFromCache();
                Assert.That(guids[i], Is.EqualTo(guid2));
            }
            Assert.That(() => { this.guidQueue.NewGuidFromCache(); }, Throws.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Set Guid Generation method
        /// </summary>
        /// <param name="method"></param>
        private void SetGuidGenerationMethod(GuidGeneratorMethod method)
        {
            if (method == GuidGeneratorMethod.Normal)
            {
                ;
            }
            else if (method == GuidGeneratorMethod.GuidGenerater)
            {
                var guidGenerator = new GuidGenerater();
                this.guidQueue = new GuidQueue(guidGenerator.NewGuid);
            }
        }
    }
}
