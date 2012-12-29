using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Azon.Helpers.Extensions;
using Azon.Helpers.Utils;

using Gallio.Framework;

using MbUnit.Framework;

using Moq;

namespace Azon.Helpers.Tests.Of.Utils {
    [TestFixture]
    public class GuardTest : BaseTestFixture {
        public interface IFoo {
            void Run();
            void Execute();
        }

        [Test]
        public void ShouldNotAllowRecursionIfSameTarget() {
            var mock = this.Mocked<IDisposable>(
                m => m.Setup(d => d.Dispose())
                      .Callback(() => Guard.Block(m.Object.Dispose))
            );

            Guard.Block(mock.Dispose);

            Mock.Get(mock).Verify(d => d.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldAllowReentrancyIfDifferentTarget() {
            var firstMock = this.Mocked<IDisposable>();
            var secondMock = this.Mocked<IDisposable>(
                m => m.Setup(d => d.Dispose())
                      .Callback(() => Guard.Block(firstMock.Dispose))
            );

            Guard.Block(secondMock.Dispose);

            Mock.Get(firstMock).Verify(d => d.Dispose(), Times.Once());
            Mock.Get(secondMock).Verify(d => d.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldAllowNonRecursiveReentrancy() {
            var key = new object();
            var mock = this.Mocked<IDisposable>();

            Guard.Block(key, mock.Dispose);
            Guard.Block(key, mock.Dispose);

            Mock.Get(mock).Verify(d => d.Dispose(), Times.Exactly(2));
        }

        [Test]
        public void ShouldBindExecutionToTargetAndMethod() {
            var mock = this.Mocked<IFoo>(m => {
                m.Setup(f => f.Run()).Callback(() => Guard.Block(m.Object.Execute));
                m.Setup(f => f.Execute()).Callback(() => Guard.Block(m.Object.Run));
            });

            Guard.Block(mock.Run);

            Mock.Get(mock).Verify(d => d.Execute(), Times.Once());
            Mock.Get(mock).Verify(d => d.Run(), Times.Once());
        }

        #region Performance Tests

        [Test]
        public void ShouldBeOfAcceptablePerformance() {
            var iteration = 0;
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < 100.Milliseconds()) {
                iteration++;
                Guard.Block(this, this.Method);
            }

            Assert.GreaterThanOrEqualTo(iteration, 40000);
            TestLog.Write("Number of iterations: {0}", iteration);
        }

        private void Method() {}

        #endregion
    }
}
