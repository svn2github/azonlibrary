using System;
using System.Linq.Expressions;
using System.Reflection;

using Moq;

namespace Azon.Helpers.Tests {
    public class BaseTestFixture {
        protected T Mocked<T>(Action<Mock<T>> setupAction = null)
            where T : class
        {
            var mock = new Mock<T>();
            if (setupAction != null)
                setupAction(mock);
            return mock.Object;
        }
    }
}