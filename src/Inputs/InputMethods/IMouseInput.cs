using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inputs.InputMethods
{
    public interface IMouseInput : IDisposable
    {
        /// <summary>
        /// The Name of the Method.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Press a specific Mouse Key down.
        /// </summary>
        /// <param name="key">The key you want to press.</param>
        bool Press(MouseKey key = MouseKey.Left);

        /// <summary>
        /// Release a specific Mouse key.
        /// 
        /// Has no effect if the key wasn't pressed to begin with.
        /// </summary>
        /// <param name="key">The key you want to release.</param>
        bool Release(MouseKey key = MouseKey.Left);

        /// <summary>
        /// Moves the Mouse by X and Y.
        /// </summary>
        /// <param name="x">The amount of pixels you want to move the Mouse on the X axis.</param>
        /// <param name="y">The amount of pixels you want to move the Mouse on the Y axis.</param>
        bool MoveBy(int x = 0, int y = 0);
    }
}
