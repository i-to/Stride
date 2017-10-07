using System;
using System.Collections.Generic;
using System.Linq;
using Stride.Utility;

namespace Stride.Persistence
{
    public class SessionRecord
    {
        public readonly DateTime Time;
        public readonly IReadOnlyList<double> Weights;

        public SessionRecord(DateTime time, IReadOnlyList<double> weights)
        {
            Time = time;
            Weights = weights;
        }

        public static string Serialize(SessionRecord record) => 
            record.Time.Ticks + 
            ' ' + 
            record.Weights.Select(w => w.ToString("R")).ConcatSpaceSeparated();

        public static SessionRecord Parse(string str)
        {
            var array = str.Split(' ');
            var ticks = long.Parse(array[0]);
            var date = new DateTime(ticks);
            var weights = array.Skip(1).Select(double.Parse).ToArray();
            return new SessionRecord(date, weights);
        }
    }
}