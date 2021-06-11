namespace Nc2.Wpf.Core.Tests.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Nc2.Wpf.Base.Commands;

    [TestClass]
    public class DeferredCommandTest
    {
        [TestMethod]
        public async Task DeferredCommand_ExecuteOnce()
        {
            
            var waiter = new TaskCompletionSource<Int32>();
            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                var counter = 0;
                cancellationTokenSource.Token.Register(waiter.SetCanceled);
                var command = new DeferredCommand(TimeSpan.FromSeconds(1), cancellation =>
                {
                    counter++;
                    if (counter == 1)
                    {
                        waiter.SetResult(counter);
                    }

                    return Task.CompletedTask;
                });

                command.Execute(null);
                var result = await waiter.Task;
                Assert.AreEqual(1, result);
            }
        }

        [TestMethod]
        public async Task DeferredCommand_MultipleTimes()
        {
            var waiter = new TaskCompletionSource<Int32>();
            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                var callState = 0;
                var executedCounter = 0;
                cancellationTokenSource.Token.Register(waiter.SetCanceled);
                var command = new DeferredCommand(TimeSpan.FromSeconds(1), cancellation =>
                {
                    executedCounter++;
                    if (executedCounter == 2 && callState == 5)
                    {
                        waiter.SetResult(executedCounter);
                    }

                    return Task.CompletedTask;
                });

                callState = 1;
                command.Execute(null);
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
                Assert.AreEqual(1, executedCounter);

                callState = 5;
                command.Execute(null);
                
                await waiter.Task;
                Assert.AreEqual(2, executedCounter);
            }
        }
    }
}
