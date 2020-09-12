namespace Nc2.Wpf.Base.Tests.Commands
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Nc2.Wpf.Base.Commands;

    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void Command_CanExecuteChanged()
        {
            var canExecuteChanged = false;
            var command = new Command(p => { }, p => true);
            command.CanExecuteChanged += (sender, args) => canExecuteChanged = true;
            command.Invalidate(); // Invalidate löst CanExecuteChanged aus
            Assert.IsTrue(canExecuteChanged);
        }

        [TestMethod]
        public void Command_CanExecute()
        {
            var command = new Command(p => { }, p => (Boolean)p);
            Assert.IsTrue(command.CanExecute(true));
            Assert.IsFalse(command.CanExecute(false));
        }

        [TestMethod]
        public void Command_Execute()
        {
            Object result = null;
            var command = new Command(p => result = p, p => true);
            
            Assert.IsNull(result);
            
            command.Execute(123);
            Assert.AreEqual(123, result);
            
            command.Execute("ABC");
            Assert.AreEqual("ABC", result);
        }
    }
}
