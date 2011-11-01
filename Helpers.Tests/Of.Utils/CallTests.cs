using System;

using Azon.Helpers.Generators;
using Azon.Helpers.Utils;

using MbUnit.Framework;

using Moq;

namespace Azon.Helpers.Tests.Of.Utils {
    [TestFixture]
    public class CallTests : BaseTestFixture {
        public interface IFoo {
            void Do<T>(int structParameter, object valueParameter);

            TResult Count<T, TResult>(int structParameter, object valueParameter);
        }

        [Test]
        public void ShouldCallGenericMethodWithGivenTypeParameter() {
            var foo = this.Mocked<IFoo>(
                mock => mock.Setup(
                    f => f.Do<double>(It.IsAny<int>(), It.IsAny<object>())
                )
            );

            Call.Generic(() => foo.Do<Type>(0, null), typeof(double));
        }

        [Test]
        public void ShouldCallGenericMethodWithConstants() {
            var objectParameter = new object();
            var foo = this.Mocked<IFoo>(
                mock => mock.Setup(
                    f => f.Do<double>(5, objectParameter)
                )
            );

            Call.Generic(() => foo.Do<Type>(5, objectParameter), typeof(double));
        }

        [Test]
        [Row(5, "string")]
        public void ShouldCallGenericMethodWithParameters(int structParameter, string valueParameter) {
            var foo = this.Mocked<IFoo>(
                mock => mock.Setup(
                    f => f.Do<double>(5, valueParameter)
                )
            );

            Call.Generic(() => foo.Do<Type>(structParameter, valueParameter), typeof(double));
        }

        [Test]
        public void ShouldReturnValueFromCalledGenericMethod() {
            var expected = Any.String();
            var foo = this.Mocked<IFoo>(
                mock => mock.Setup(
                    f => f.Count<double, string>(It.IsAny<int>(), It.IsAny<object>())
                ).Returns(expected)
            );

            var result = Call.Generic(
                () => foo.Count<Type, object>(0, null),
                typeof(double), typeof(string)
            );

            Assert.AreSame(expected, result);
        }
    }
}
