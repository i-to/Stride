using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Score;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class StaffLinesLayoutAlgorithm
    {
        public IEnumerable<LineObject> CreateGrandStaffLines(StavesMetrics metrics)
        {
            var treebleStaffOrigin = metrics.Origin;
            var treebleLines = CreateLines(
                treebleStaffOrigin,
                metrics.StaffLinesDistance,
                metrics.StaffLinesLength,
                Const.LinesInStaff,
                metrics.StaffLineThickness);
            var bassStaffOrigin = treebleStaffOrigin + new Vector(0.0, metrics.GrandStaffOffset);
            var bassLines = CreateLines(
                bassStaffOrigin,
                metrics.StaffLinesDistance,
                metrics.StaffLinesLength,
                Const.LinesInStaff,
                metrics.StaffLineThickness);
            return treebleLines.Concat(bassLines);
        }

        public IReadOnlyList<LineObject> CreateLedgerLines(
            StavesMetrics metrics,
            IReadOnlyDictionary<Beat, (GrandStaffLedgerLines, bool)> ledgerLinesForBeats,
            IReadOnlyDictionary<Beat, double> beatPositions)
        {
            var result = new List<LineObject>();
            foreach (var kv in ledgerLinesForBeats)
            {
                var beat = kv.Key;
                var (ledgerLines, isWholeNote) = kv.Value;
                var noteWidth = isWholeNote
                    ? metrics.WholeNoteheadWidth
                    : metrics.OtherNoteheadWidth;
                var length = 2.0 * metrics.LedgerLineLip + noteWidth;
                var x = beatPositions[beat] - metrics.LedgerLineLip;
                AddLedgerLinesForNote(
                    result,
                    ledgerLines,
                    x,
                    length,
                    metrics.Origin.Y,
                    metrics.StaffLinesDistance,
                    metrics.GrandStaffOffset,
                    metrics.StaffLineThickness);
            }
            return result;
        }

        void AddLedgerLinesForNote(
            List<LineObject> lines,
            GrandStaffLedgerLines ledgerLines,
            double originX,
            double length,
            double staffOriginY,
            double distanceBetweenLines,
            double grandStaffOffset,
            double thickness)
        {

            var treebleClefTop = ledgerLines.TreebleClef.Top;
            if (treebleClefTop > 0)
            {
                var y = staffOriginY - treebleClefTop * distanceBetweenLines;
                var origin = new Point(originX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, treebleClefTop, thickness));
            }

            var treebleClefBottom = ledgerLines.TreebleClef.Bottom;
            if (treebleClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines;
                var origin = new Point(originX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, treebleClefBottom, thickness));
            }

            var bassClefTop = ledgerLines.BassClef.Top;
            if (bassClefTop > 0)
            {
                var y = staffOriginY - bassClefTop * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(originX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, bassClefTop, thickness));
            }

            var bassClefBottom = ledgerLines.BassClef.Bottom;
            if (bassClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(originX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, bassClefBottom, thickness));
            }
        }

        IEnumerable<LineObject> CreateLines(
            Point origin,
            double distanceBetweenLines,
            double length,
            int count,
            double thickness)
        {
            for (int i = 0; i != count; ++i)
            {
                var offset = distanceBetweenLines * i;
                var begin = origin + new Vector(0.0, offset);
                var end = origin + new Vector(length, offset);
                yield return new LineObject(begin, end, thickness);
            }
        }
    }
}