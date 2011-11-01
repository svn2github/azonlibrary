using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Azon.Helpers.Generators.ValueGenerators;

using MbUnit.Framework;

namespace Azon.Helpers.Tests.Of.Generators.Of.ValueGenerators {
    [TestFixture]
    public class CollectionValueGeneratorTests : ValueGeneratorTest<CollectionValueGenerator> {
        protected override Type[] SupportedTypes {
            get { return new[] { typeof(ICollection<>) }; }
        }

        protected override Type[] SampleTypes {
            get {
                return new[] {
                    typeof(List<int>),
                    typeof(Collection<int>),
                    typeof(HashSet<int>),

                    typeof(IList<float>),
                    typeof(ICollection<float>),
                    typeof(ISet<float>),
                };
            }
        }
    }
}
