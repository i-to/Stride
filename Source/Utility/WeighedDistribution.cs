using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Stride.Utility.Fluent;

namespace Stride.Utility
{
    public static class WeighedDistribution
    {
        public static IEnumerable<double> ComputeCumulativeWeights(IEnumerable<int> absoluteWeights)
        {
            double sum = absoluteWeights.Sum();
            var normalizedWeights = absoluteWeights.Select(w => w / sum);
            return normalizedWeights.Scan(Combinator.SumTwoDoubles);
        }

        public static int BucketIndexOfValue(IEnumerable<int> absoluteWeights, double normalizedValue)
        {
            if (!normalizedValue.IsInRangeInclusiveLower(0.0, 1.0))
                throw new ArgumentException($"Expected value in range [0.0, 1.0), given {normalizedValue}.");
            var cumulativeWeights = ComputeCumulativeWeights(absoluteWeights);
            return cumulativeWeights.FindIndex(upperBound => normalizedValue < upperBound);
        }
    }

    public class TestWeighedDistribution
    {
        public static void Test()
        {
            var weights = new [] {1, 2, 3};
            var cumulativeWeights = WeighedDistribution.ComputeCumulativeWeights(weights); // = [0.1666666667; 0.5; 1.0]
            cumulativeWeights.ToArray();
            WeighedDistribution.BucketIndexOfValue(weights, 0.0); // = 0
            WeighedDistribution.BucketIndexOfValue(weights, 0.49); // = 1
            WeighedDistribution.BucketIndexOfValue(weights, 0.5); // = 2
            WeighedDistribution.BucketIndexOfValue(weights, 1.0); // should throw
            WeighedDistribution.BucketIndexOfValue(weights, 1.7); // should throw
            WeighedDistribution.BucketIndexOfValue(weights, -1.0); // should throw
        }
    }
}
