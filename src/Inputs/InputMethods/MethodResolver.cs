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
        private static InputType FindInAssembly<T>(Assembly assembly)
        {
            if (assembly == null)
                return null;

            var inputMethod = assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t)).FirstOrDefault();

            if (inputMethod == null)
                return null;

            return Activator.CreateInstance(inputMethod) as InputType;
        }

        public static InputType GetMethodObjectFor<T>()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().ToList())
            {
                var result = FindInAssembly<T>(asm);

                if (result == null)
                    continue;

                return result;
            }

            throw new InputMethodNotFoundException($"The input method of {typeof(T).FullName} does not exist.");
        }
    }
}
