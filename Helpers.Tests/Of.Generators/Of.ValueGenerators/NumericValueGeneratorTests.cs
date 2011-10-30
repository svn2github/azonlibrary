using System;

using Azon.Helpers.Generators.ValueGenerators;

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
                    typeof(byte),
                    typeof(ushort),
                    typeof(uint),
                    typeof(float),
                    typeof(double),
                    typeof(decimal)
                }; 
            }
        }
    }
}
