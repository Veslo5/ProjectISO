using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Engine.Helpers
{
    public class DoubleBuffer<T>
    {
        public T Buffer1 { get; set; }
        public T Buffer2 { get; set; }
        public bool IsBuffer1Active { get; set; }

        public DoubleBuffer(T buffer)
        {
            Buffer1 = buffer;
            Buffer2 = buffer;
        }

        /// <summary>
        /// Retrives active buffer
        /// </summary>
        /// <returns></returns>
        public T GetActiveBuffer()
        {
            return IsBuffer1Active ? Buffer1 : Buffer2;
        }
    }
}
