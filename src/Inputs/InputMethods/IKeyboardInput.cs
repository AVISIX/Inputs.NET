using System;

namespace Inputs.InputMethods
{
    public interface IKeyboardInput : IDisposable
    {
        /// <summary>
        /// The Name of the Method.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Press the specific Virtual Keycode down.
        /// </summary>
        /// <param name="key">The key you want to press.</param>
        /// <returns></returns>
        bool Press(VK key);

        /// <summary>
        /// Release the specific key code. This wont have any effect if the key isn't down!
        /// </summary>
        /// <param name="key">The key you want to release.</param>
        /// <returns></returns>
        bool Release(VK key);
    }
}
