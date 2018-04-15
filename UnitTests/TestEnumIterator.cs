//-----------------------------------------------------------------------
// <copyright file="TestEnumIterator.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    /// unit test for Template
    /// </summary>
    [TestFixture]
    public class TestEnumIterator
    {
        /// <summary>
        /// enum type1
        /// </summary>
        public enum TestEnumType1
        {
            Type1,
            Type2,
            Type3,
            Type4,
        }

        /// <summary>
        /// enum type2
        /// </summary>
        public enum TestEnumType2
        {
            Type1,
            Type2,
            Type3,
            Type4,
        }

        /// <summary>
        /// setup method to be called to initialize unit tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }

        /// <summary>
        /// cleanup method to be called to cleanup unit tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// Test for TestEnumType1
        /// </summary>
        /// <param name="returnMethod">returnMethod</param>
        [Test, Pairwise]
        public void TestForTestEnumType1(
            [Values] EnumIterator<TestEnumType1>.ReturnMethodType returnMethod)
        {
            TestEnumTypeGeneric<TestEnumType1>(returnMethod);
        }

        /// <summary>
        /// Test for TestEnumType2
        /// </summary>
        /// <param name="returnMethod">returnMethod</param>
        [Test, Pairwise]
        public void TestForTestEnumType2(
            [Values] EnumIterator<TestEnumType2>.ReturnMethodType returnMethod)
        {
            TestEnumTypeGeneric<TestEnumType2>(returnMethod);
        }

        /// <summary>
        /// utility function to test EnumIterator.Enumerable
        /// </summary>
        /// <typeparam name="T">enum type</typeparam>
        /// <param name="returnMethod">returnMethod</param>
        private void TestEnumTypeGeneric<T>(EnumIterator<T>.ReturnMethodType returnMethod)
        {
            Array values = Enum.GetValues(typeof(T));
            var length = values.Length;

            int count = 0;
            IEnumerable<T> iterater = EnumIterator<T>.Enumerable(returnMethod);
            foreach (T targetType in iterater)
            {
                Console.WriteLine(targetType.ToString());
                count++;
            }
            Assert.That(count, Is.EqualTo(length));
        }
    }
}
