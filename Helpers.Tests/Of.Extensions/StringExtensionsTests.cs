using Azon.Helpers.Extensions;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Extensions {
    [TestFixture]
    public class StringExtensionsTests {
        [Test]
        public void SubstringAfter_should_return_full_string_if_pattern_not_matched() {
            Assert.AreEqual("abc", "abc".SubstringAfter("d"));
        }

        [Test]
        public void SubstringAfter_should_return_rest_of_the_string_after_first_match() {
            Assert.AreEqual("abcdabc", "abcdabcdabc".SubstringAfter("d"));
        }

        [Test]
        public void SubstringAfter_should_return_empty_string_if_pattern_matched_at_the_end() {
            Assert.AreEqual(string.Empty, "abc".SubstringAfter("bc"));
        }

        [Test]
        public void SubstringAfter_should_not_fail_is_pattern_is_longer_than_string() {
            Assert.AreEqual("abc", "abc".SubstringAfter("defgh"));
        }
    }
}
