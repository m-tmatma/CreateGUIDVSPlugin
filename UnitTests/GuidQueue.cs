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
    internal class GuidQueue
    {
        /// <summary>
        /// delegate for creating new GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal delegate Guid NewGuid();

        /// <summary>
        /// seed to create a new GUID.
        /// </summary>
        private Queue<Guid> fifo;

        /// <summary>
        /// delegate for creating brand-new GUID
        /// </summary>
        private NewGuid newGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidQueue" /> class.
        /// </summary>
        public GuidQueue(NewGuid newGuid = null)
        {
            this.fifo = new Queue<Guid>();
            if (newGuid == null)
            {
                this.newGuid = delegate () { return Guid.NewGuid(); };
            }
            else
            {
                this.newGuid = newGuid;
            }
        }

        /// <summary>
        /// create brand-new GUID
        /// </summary>
        /// <returns>new GUID</returns>
        public Guid BrandNewGuid()
        {
            var guid = this.newGuid();
            this.fifo.Enqueue(guid);
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
