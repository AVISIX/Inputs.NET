using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inputs.InputMethods
{
    internal class MethodResolver<InputType> where InputType : class
    {
        public static InputType GetMethodObjectFor<T>()
        {
            var asm = Assembly.GetExecutingAssembly();
            var inputMethod = asm.GetTypes().Where(t => typeof(T).IsAssignableFrom(t)).FirstOrDefault();

            if (inputMethod == null)
                throw new InputMethodNotFoundException("Failed to find the supplied keyboard-input-method.");

            return Activator.CreateInstance(inputMethod) as InputType;
        }
    }
}
