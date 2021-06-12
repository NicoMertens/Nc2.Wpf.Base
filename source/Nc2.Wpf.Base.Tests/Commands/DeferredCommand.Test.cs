namespace Nc2.Wpf.Core.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Nc2.Wpf.Base.Commands;

    [TestClass]
    public class DeferredCommandTest
    {
        [TestMethod]
        public async Task DeferredCommand_ExecuteOnce()
        {
            var executionList = new List<Boolean>();
            var command = new DeferredCommand(TimeSpan.FromSeconds(1), cancellation =>
            {
                executionList.Add(true);
                return Task.CompletedTask;
            });

            command.Execute(null);
            await Task.Delay(TimeSpan.FromSeconds(2));
            Assert.AreEqual(1, executionList.Count);
        }

        [TestMethod]
        public async Task DeferredCommand_MultipleTimes()
        {
            var callState = 0;
            var executionList = new List<Int32>();
            var command = new DeferredCommand(TimeSpan.FromSeconds(1), cancellation =>
            {
                executionList.Add(callState);
                return Task.CompletedTask;
            });

            var executingEventCounter = 0;
            var executedEventCounter = 0;
            command.CommandExecuting += (s, e) => executingEventCounter++;
            command.CommandExecuted += (s, e) => executedEventCounter++;

            Assert.IsFalse(command.IsRunning);
            callState = 1;
            command.Execute(null);
            Assert.IsTrue(command.IsRunning);
            await Task.Delay(100);

            callState = 2;
            command.Execute(null);
            await Task.Delay(500);
                
            callState = 3;
            command.Execute(null);
            await Task.Delay(900);

            callState = 4;
            command.Execute(null);
            await Task.Delay(1100);
            Assert.IsFalse(command.IsRunning);
            Assert.AreEqual(1, executingEventCounter);
            Assert.AreEqual(1, executedEventCounter);
            Assert.AreEqual(1, executionList.Count);
            Assert.AreEqual(4, executionList[0]);

            callState = 5;
            command.Execute(null);
            await Task.Delay(1100);
            Assert.IsFalse(command.IsRunning);

            Assert.AreEqual(2, executingEventCounter);
            Assert.AreEqual(2, executedEventCounter);
            Assert.AreEqual(2, executionList.Count);
            Assert.AreEqual(5, executionList[1]);
        }

        [TestMethod]
        public async Task DeferredCommand_Exception()
        {
            Exception actualException = null;
            var command = new DeferredCommand(TimeSpan.FromSeconds(1), _ => throw new InvalidOperationException());
            command.ExceptionThrowed += (s, e) => actualException = e;
            command.Execute(null);
            await Task.Delay(TimeSpan.FromSeconds(2));
            Assert.AreEqual(typeof(InvalidOperationException), actualException.GetType());
        }

        [TestMethod]
        public async Task DeferredCommand_CancellationBeforeTimeout()
        {
            var isExecuted = false;
            var command = new DeferredCommand(TimeSpan.FromSeconds(2), _ =>
            {
                isExecuted = true;
                return Task.CompletedTask;
            });

            command.Execute(null);
            await Task.Delay(TimeSpan.FromSeconds(1));
            command.Cancel();
            Assert.IsFalse(isExecuted);
        }

        [TestMethod]
        public async Task DeferredCommand_CancellationAfterTimeout()
        {
            var isCancellationIsRequested = false;
            var command = new DeferredCommand(TimeSpan.FromMilliseconds(500), async cancellationToken =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                isCancellationIsRequested = cancellationToken.IsCancellationRequested;
            });

            command.Execute(null);
            await Task.Delay(TimeSpan.FromSeconds(1));
            command.Cancel();
            await Task.Delay(TimeSpan.FromSeconds(1));
            Assert.IsTrue(isCancellationIsRequested);
        }

        [TestMethod]
        public async Task DeferredCommand_CancellationAfterDispose()
        {
            var command = new DeferredCommand(TimeSpan.FromMilliseconds(1), _ => Task.CompletedTask);
            command.Execute(null);
            await Task.Delay(TimeSpan.FromSeconds(1));
            Assert.IsFalse(command.IsRunning);
            command.Cancel();
        }

        [TestMethod]
        public async Task DeferredCommand_CanExecute()
        {
            var isExecuted = false;
            var canExecute = false;
            var command = new DeferredCommand(TimeSpan.FromMilliseconds(1), _ => 
            {
                isExecuted = true;
                return Task.CompletedTask;
            }, () => canExecute);

            command.Execute(null);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            Assert.IsFalse(isExecuted);

            canExecute = true;
            command.Execute(null);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            Assert.IsTrue(isExecuted);
        }
    }
}
