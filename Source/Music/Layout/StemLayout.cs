using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class StemLayout
    {
        readonly StavesMetrics Metrics;
        readonly VerticalLayout VerticalLayout;

        public StemLayout(StavesMetrics metrics, VerticalLayout verticalLayout)
        {
            Metrics = metrics;
            VerticalLayout = verticalLayout;
        }

        double StemLength => 7.0 * Metrics.HalfSpace;

        public IEnumerable<LineObject> CreateStems(
            IEnumerable<NoteOnPage> pagePageNotes,
            IReadOnlyDictionary<Tick, double> tickPositions) => 
            pagePageNotes
                .Where(note => note.Duration.IsHalf() || note.Duration.IsQuarter())
                .Select(note => CreateStem(tickPositions, note));

        LineObject CreateStem(IReadOnlyDictionary<Tick, double> tickPositions, NoteOnPage note)
        {
            var position = note.StaffPosition;
            var (xOffset, yOffset, length) = position.VerticalOffset >= 0
                ? (0, 2, StemLength)
                : (Metrics.OtherNoteheadWidth - Metrics.StemLineThickness, 0, - StemLength);
            var x = xOffset + tickPositions[note.Tick];
            var y = yOffset + VerticalLayout.StaffPositionToYOffset(position);
            var origin = new Point(x, y);
            var end = new Point(x, y + length);
            return new LineObject(origin, end, Metrics.StemLineThickness);
        }
    }
}