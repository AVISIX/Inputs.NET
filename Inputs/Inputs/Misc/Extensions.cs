using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Inputs.Misc.Native.User32;

namespace Inputs.Misc
{
    internal static class Extensions
    {
        internal static MOUSEEVENTF_FLAGS MapMouseKey(this MouseKey key, bool down)
        {
            MOUSEEVENTF_FLAGS keyType;

            if (down == true)
            {
                switch (key)
                {
                    case MouseKey.Left:
                        keyType = MOUSEEVENTF_FLAGS.MOUSEEVENTF_LEFTDOWN;
                        break;

                    case MouseKey.Right:
                        keyType = MOUSEEVENTF_FLAGS.MOUSEEVENTF_RIGHTDOWN;
                        break;

                    case MouseKey.Middle:
                        keyType = MOUSEEVENTF_FLAGS.MOUSEEVENTF_MIDDLEDOWN;
                        break;

                    default: throw new Exception("Unsupported Key Type.");
                }
            }
            else
            {
                switch (key)
                {
                    case MouseKey.Left:
                        keyType = MOUSEEVENTF_FLAGS.MOUSEEVENTF_LEFTUP;
                        break;

                    case MouseKey.Right:
                        keyType = MOUSEEVENTF_FLAGS.MOUSEEVENTF_RIGHTUP;
                        break;

                    case MouseKey.Middle:
                        keyType = MOUSEEVENTF_FLAGS.MOUSEEVENTF_MIDDLEUP;
                        break;

                    default: throw new Exception("Unsupported Key Type.");
                }
            }

            return keyType;
        }
    }
}
