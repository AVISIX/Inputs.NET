using Inputs.Hooks;
using Inputs.InputMethods.Mouse;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Threading;

namespace Inputs.Tests
{
    [TestClass]
    public class MouseTests
    {
        [TestInitialize]
        public void InitMouseTests()
        {
        }

        [TestMethod("Test Method Switching")]
        public void TestMouseMethodSwitching1()
        {
            Mouse.SetMethodFrom<MouseEvent>();
            if (Mouse.MethodObject.Name != nameof(MouseEvent))
                Assert.Fail();

            Mouse.SetMethodFrom<NtUserInjectMouseInput>();
            if (Mouse.MethodObject.Name != nameof(NtUserInjectMouseInput))
                Assert.Fail();

            Mouse.SetMethodFrom<NtUserSendInput>();
            if (Mouse.MethodObject.Name != nameof(NtUserSendInput))
                Assert.Fail();

            Mouse.SetMethodFrom<MouseDD>();
            if (Mouse.MethodObject.Name != nameof(MouseDD))
                Assert.Fail();
        }

        #region Set Cursor Pos
        [TestMethod("Test SetCursorPos")]
        public void MouseTest2()
        {
            Mouse.SetCursorPos(0, 0);

            var newPos = Mouse.GetCursorPos();
            Assert.IsTrue(newPos.X == 0);
            Assert.IsTrue(newPos.Y == 0);
        }
        #endregion

        #region Mouse Move Tests
        private void DoMouseMoveTest()
        {
            Mouse.SetCursorPos(10, 10);
            var origin = Mouse.GetCursorPos();

            Thread.Sleep(250);

            Mouse.Move(origin.X + 50, origin.Y + 50);

            Thread.Sleep(250);

            var destination = Mouse.GetCursorPos();

            Assert.IsTrue(origin.X == destination.X - 50);
            Assert.IsTrue(origin.Y == destination.Y - 50);
        }

        [TestMethod("Test MouseEvent-method")]
        public void TestMouseEvent3()
        {
            Mouse.SetMethodFrom<MouseEvent>();
            DoMouseMoveTest();
        }

        [TestMethod("Test MouseDD-method")]
        public void TestDDDriverMove4()
        {
            Mouse.SetMethodFrom<MouseDD>();
            DoMouseMoveTest();
        }

        [TestMethod("Test NtUserInjectMouseInput-method")]
        public void TestNtUserInjectInput5()
        {
            Mouse.SetMethodFrom<NtUserInjectMouseInput>();
            DoMouseMoveTest();
        }

        [TestMethod("Test NtUserSendInput-method")]
        public void TestNtSendInput6()
        {
            Mouse.SetMethodFrom<Inputs.InputMethods.Mouse.NtUserSendInput>();
            DoMouseMoveTest();
        }
        #endregion

        #region Clicking Tests

        private void TestClick(MouseKey k, bool sim = true)
        {
            Thread.Sleep(250);

            MouseHook hook = new MouseHook();
            
            hook.OnKeyPressed += new MouseHookEventHandler((p, key, type, simulated) =>
            {
                Assert.AreEqual(key, k);
                Assert.AreEqual(type, ClickType.Down);
                Assert.AreEqual(simulated, sim);
            });

            hook.Hook();
            Mouse.Press(k);
            Thread.Sleep(250);
            hook.Unhook();
            Mouse.Release(k);
        }

        [TestMethod("Test MouseEvent-click")]
        public void TestMouseEventClick7()
        {
            Mouse.SetMethodFrom<MouseEvent>();
            TestClick(MouseKey.Left);
            TestClick(MouseKey.Right);
            TestClick(MouseKey.Middle);
        }

        [TestMethod("Test MouseDD-click")]
        public void TestMouseDDClick8()
        {
            Mouse.SetMethodFrom<MouseDD>();
            TestClick(MouseKey.Left, false);
            TestClick(MouseKey.Right, false);
            TestClick(MouseKey.Middle, false);
        }

        [TestMethod("Test NtUserInjectMouseInput-click")]
        public void TestNtUserInjectMouseInputClick9()
        {
            Mouse.SetMethodFrom<NtUserInjectMouseInput>();
            TestClick(MouseKey.Left);
            TestClick(MouseKey.Right);
            TestClick(MouseKey.Middle);
        }

        [TestMethod("Test NtUserSendInput-click")]
        public void TestNtUserSendInputClick10()
        {
            Mouse.SetMethodFrom<NtUserSendInput>();
            TestClick(MouseKey.Left);
            TestClick(MouseKey.Right);
            TestClick(MouseKey.Middle);
        }

        #endregion
    }
}
