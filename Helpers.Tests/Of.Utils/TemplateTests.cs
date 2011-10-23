using System;

using Azon.Helpers.Tests.Internal.Asserts;
using Azon.Helpers.Utils;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Utils {
    [TestFixture]
    public class TemplateTests {
        [Test]
        public void ApplyShouldThrowIfGivenTemplateIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Template.Apply(null, new object()),
                ex => Assert.AreEqual("template", ex.ParamName)
            );
        }

        [Test]
        public void ApplyShouldThrowIfGivenParameterValuesIsNull() {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Template.Apply(string.Empty, null),
                ex => Assert.AreEqual("params", ex.ParamName)
            );
        }

        [Test]
        public void ApplyShouldThrowIfGivenTemplateHasNotGivenParameter() {
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Template.Apply("{param}", new object()),
                "A value for parameter {param} was not supplied."
            );
        }

        [Test]
        public void ApplyShouldReturnOriginalStringIfGivenTemplateHasNoParameters() {
            var result = Template.Apply("original", new { param = "test" });

            Assert.AreEqual("original", result);
        }

        [Test]
        public void ApplyShouldInsertParameterValue() {
            var result = Template.Apply("{param}", new { param = "test" });

            Assert.AreEqual("test", result);
        }

        [Test]
        public void ApplyShouldInsertEmptyStringIfParameterIsNull() {
            var result = Template.Apply("{param}", new { param = (string)null });

            Assert.AreEqual("", result);
        }

        [Test]
        public void ApplyShouldConvertParameterValueToString() {
            var result = Template.Apply("{param}", new { param = 50 });

            Assert.AreEqual("50", result);
        }
    }
}
