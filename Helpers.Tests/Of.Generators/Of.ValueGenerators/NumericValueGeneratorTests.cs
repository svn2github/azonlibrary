using System;

using Azon.Helpers.Generators.ValueGenerators;
using Azon.Helpers.Generators.ValueGenerators.Constraints;
using Azon.Helpers.Tests.Internal.Asserts;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class NumericValueGeneratorTests : ValueGeneratorTest<NumericValueGenerator> {
        protected override Type[] SupportedTypes {
            get {
                return new[] {
                    typeof(sbyte),
                    typeof(short),
                    typeof(int),
                    typeof(long),
                    typeof(byte),
                    typeof(ushort),
                    typeof(uint),
                    typeof(ulong)
                }; 
            }
        }

        protected override Type[] SampleTypes {
            get { return this.SupportedTypes; }
        }

        [Test]
        [Row(typeof(sbyte),  sbyte.MinValue + 40,  sbyte.MinValue + 50)]
        [Row(typeof(short),  short.MinValue + 40,  short.MinValue + 50)]
        [Row(typeof(int),    int.MinValue + 40,    int.MinValue + 50)]
        [Row(typeof(long),   long.MinValue + 40,   long.MinValue + 50)]
        [Row(typeof(byte),   byte.MinValue + 40,   byte.MinValue + 50)]
        [Row(typeof(ushort), ushort.MinValue + 40, ushort.MinValue + 50)]
        [Row(typeof(uint),   uint.MinValue + 40,   uint.MinValue + 50)]
        [Row(typeof(ulong),  ulong.MinValue + 40,  ulong.MinValue + 50)]
        public void ShouldReturnValueFromNarrowInterval<T>(T minValue, T maxValue)
            where T : struct, IComparable<T>
        {
            var constraints = new IConstraint[] {
                new MaxValueConstraint<T>(maxValue),
                new MinValueConstraint<T>(minValue)
            };

            var value = this.Generator.GetRandomValue(typeof(T), constraints);

            Assert.Between(value, minValue, maxValue);
        }

        [Test]
        public void ShouldThrowIfGivenMaxValueIsLessThanMinValue() {
            var constraints = new IConstraint[] {
                new MinValueConstraint<int>(5),
                new MaxValueConstraint<int>(0)
            };

            ExceptionAssert.Throws<InvalidOperationException>(
                () => this.Generator.GetRandomValue(typeof(int), constraints),
                "minValue (5) should not be greater than maxValue (0)."
            );
        }
    }
}
