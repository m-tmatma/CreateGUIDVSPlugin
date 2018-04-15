//-----------------------------------------------------------------------
// <copyright file="TestGuidQueue.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitTests
{
    using System;
    using System.Text;
    using Microsoft.Win32;
    using NUnit.Framework;
    using CreateGUIDVSPlugin.Utility;
    using System.Collections.Generic;

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
        [TestCase(3)]
        [TestCase(10)]
        public void TestRepeatOneByOne(int count)
        {
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
        [TestCase(3)]
        [TestCase(10)]
        public void TestRepeatAllTogether(int count)
        {
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
    }
}
