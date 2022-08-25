using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

using static Inputs.Misc.Native.User32;
using System.Windows.Media;

namespace Inputs.Misc
{
    internal static class Extensions
    {
        internal static Size GetElementPixelSize(this UIElement element)
        {
            Matrix transformToDevice;

            var source = PresentationSource.FromVisual(element);
            if (source != null)
                transformToDevice = source.CompositionTarget.TransformToDevice;
            else
            {
                using (var source2 = new HwndSource(new HwndSourceParameters()))
                    transformToDevice = source2.CompositionTarget.TransformToDevice;
            }

            if (element.DesiredSize == new Size())
                element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return (Size)transformToDevice.Transform((Vector)element.DesiredSize);
        }

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

                    default: throw new NotSupportedException("Unsupported Key Type.");
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

                    default: throw new NotSupportedException("Unsupported Key Type.");
                }
            }

            return keyType;
        }
    }
}
