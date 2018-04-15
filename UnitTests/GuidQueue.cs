//-----------------------------------------------------------------------
// <copyright file="GuidQueue.cs" company="Masaru Tsuchiyama">
//     Copyright (c) Masaru Tsuchiyama. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitTests
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Tester Class for providing custom GUID generator 
    /// </summary>
    public class GuidQueue
    {
        /// <summary>
        /// Guid Method
        /// </summary>
        public enum GuidGeneratorMethod
        {
            Normal,
            GuidGenerater,
        }

        /// <summary>
        /// delegate for creating new GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public delegate Guid NewGuid();

        /// <summary>
        /// seed to create a new GUID.
        /// </summary>
        private Queue<Guid> fifo;

        /// <summary>
        /// delegate for creating brand-new GUID
        /// </summary>
        private NewGuid newGuid;

        /// <summary>
        /// guid Generator Method
        /// </summary>
        private GuidGeneratorMethod guidGeneratorMethod;

        /// <summary>
        /// Set and Get guid generation method
        /// </summary>
        public GuidGeneratorMethod GuidMethod
        {
            get
            {
                return this.guidGeneratorMethod;
            }
            set
            {
                switch(value)
                {
                    case GuidGeneratorMethod.Normal:
                        this.newGuid = delegate () { return Guid.NewGuid(); };
                        break;
                    case GuidGeneratorMethod.GuidGenerater:
                        var guidGenerator = new GuidGenerater();
                        this.newGuid = guidGenerator.NewGuid;
                        break;
                }
                this.guidGeneratorMethod = value; 
                this.fifo.Clear();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidQueue" /> class.
        /// </summary>
        public GuidQueue()
        {
            this.fifo = new Queue<Guid>();
            this.GuidMethod = GuidGeneratorMethod.Normal;
        }

        /// <summary>
        /// create brand-new GUID with queueing
        /// </summary>
        /// <returns>new GUID</returns>
        public Guid BrandNewGuid()
        {
            var guid = this.newGuid();
            this.fifo.Enqueue(guid);
            return guid;
        }

        /// <summary>
        /// create brand-new GUID without queueing
        /// </summary>
        /// <returns>new GUID</returns>
        public Guid BrandNewGuidNoCache()
        {
            var guid = this.newGuid();
            this.fifo.Clear();
            return guid;
        }

        /// <summary>
        /// create new GUID from cache.
        /// </summary>
        /// <returns>new GUID</returns>
        public Guid NewGuidFromCache()
        {
            var guid = this.fifo.Dequeue();
            return guid;
        }
    }
}
