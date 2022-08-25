using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inputs
{
    /// <summary>
    /// A point with an X and Y coordinate.
    /// </summary>
    /// <typeparam name="T">The type of the Point.</typeparam>
    public struct Point<T> where T : IComparable<T>
    {
        public Point(T X = default, T Y = default)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>
        /// The X coordinate of this point.
        /// </summary>
        public T X { get; set; }

        /// <summary>
        /// The Y coordinate of this point.
        /// </summary>
        public T Y { get; set; }
    }
}
