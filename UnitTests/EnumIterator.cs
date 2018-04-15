//-----------------------------------------------------------------------
// <copyright file="EnumIterator.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitTests
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// sequential iterator for enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SequentialEnumIterator<T>
    {
        public SequentialEnumIterator()
        {
        }

        /// <summary>
        /// sequential iterator
        /// </summary>
        public IEnumerable<T> Enumerable
        {
            get
            {
                foreach (T data in Enum.GetValues(typeof(T)))
                {
                    yield return data;
                }
            }
        }
    }

    /// <summary>
    /// random iterator for enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RandomEnumIterator<T>
    {
        public RandomEnumIterator()
        {
        }

        /// <summary>
        /// random iterator
        /// </summary>
        public IEnumerable<T> Enumerable
        {
            get
            {
                var random = new Random();
                Array values = Enum.GetValues(typeof(T));
                var length = values.Length;

                for(int i = 0; i < length; i++)
                {
                    T data = (T)values.GetValue(random.Next(length - 1));
                    yield return data;
                }
            }
        }
    }

    /// <summary>
    /// enum iterator selector
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumIterator<T>
    {
        /// <summary>
        /// iterator method
        /// </summary>
        public enum ReturnMethodType
        {
            Sequential,
            Random,
        }

        /// <summary>
        /// enum iterator selector
        /// </summary>
        /// <param name="method">iterator method</param>
        /// <returns></returns>
        static public IEnumerable<T> Enumerable(ReturnMethodType method)
        {
            switch(method)
            {
                case ReturnMethodType.Sequential:
                    {
                        var iterator = new SequentialEnumIterator<T>();
                        return iterator.Enumerable;
                    }
                case ReturnMethodType.Random:
                    {
                        var iterator = new RandomEnumIterator<T>();
                        return iterator.Enumerable;
                    }
                default:
                    throw new ArgumentException(method.ToString());
            }
        }
    }
}
