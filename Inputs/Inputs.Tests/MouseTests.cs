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

        [TestMethod("Test SetCursorPos")]
        public void MouseTest1()
        {
            Mouse.SetCursorPos(0, 0);

            var newPos = Mouse.GetCursorPos();
            Assert.IsTrue(newPos.X == 0);
            Assert.IsTrue(newPos.Y == 0);
        }

        private void DoMouseMoveTest()
        {
            Mouse.SetCursorPos(0, 0);
            var origin = Mouse.GetCursorPos();

            Thread.Sleep(250);

            Mouse.Move(origin.X + 50, origin.Y + 50);

            Thread.Sleep(250);

            var destination = Mouse.GetCursorPos();

            Assert.IsTrue(origin.X == destination.X - 50);
            Assert.IsTrue(origin.Y == destination.Y - 50);
        }

        [TestMethod("Test MouseEvent-method")]
        public void MouseTest2()
        {
            Mouse.SetMethodFrom<MouseEvent>();
            DoMouseMoveTest();
        }

        [TestMethod("Test MouseDD-method")]
        public void MouseTest3()
        {
            Mouse.SetMethodFrom<MouseDD>();
            DoMouseMoveTest();
        }

        [TestMethod("Test NtUserInjectMouseInput-method")]
        public void MouseTest4()
        {
            Mouse.SetMethodFrom<NtUserInjectMouseInput>();
            DoMouseMoveTest();
        }

        [TestMethod("Test NtUserSendInput-method")]
        public void MouseTest5()
        {
            Mouse.SetMethodFrom<Inputs.InputMethods.Mouse.NtUserSendInput>();
            DoMouseMoveTest();
        }
    }
}
