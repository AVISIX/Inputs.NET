using Inputs.Hooks;
using Inputs.InputMethods.Keyboard;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inputs.Tests
{
    [TestClass]
    public class KeyboardTests
    {
        [TestInitialize]
        public void InitKeyboardTests()
        {

        }

        #region Test Clicking

        private void TestClick(VK k, bool sim = true)
        {
            KeyboardHook hook = new KeyboardHook();

            hook.OnKeyPressed += new KeyboardHookEventHandler((key, simulated) =>
            {
                Assert.AreEqual(key, k);
                Assert.AreEqual(simulated, sim);
            });

            Keyboard.Press(k);
            hook.Unhook();
            Keyboard.Release(k);
            Keyboard.Click(k);
        }

        [TestMethod("Test KeyboardEvent-clicking")]
        public void TestKeyboardEvent1()
        {
            Keyboard.SetMethodFrom<KeyboardEvent>();
            TestClick(VK.KEY_LWIN, true);
        }

        [TestMethod("Test NtUserInjectKeyboardInput-clicking")]
        public void TestUserInjectKeyboardInput2()
        {
            Keyboard.SetMethodFrom<NtUserInjectKeyboardInput>();
            TestClick(VK.KEY_LWIN, true);
        }

        [TestMethod("Test NtUserSendInput-clicking")]
        public void TestntUserSendInput3()
        {
            Keyboard.SetMethodFrom<NtUserSendInput>();
            TestClick(VK.KEY_LWIN, true);
        }

        [TestMethod("Test KeyboardDD-click")]
        public void TestKeyboardDD4()
        {
            Keyboard.SetMethodFrom<KeyboardDD>();
            TestClick(VK.KEY_LWIN, true);
        }
        #endregion
    }
}
