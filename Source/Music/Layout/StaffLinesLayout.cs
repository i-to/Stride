using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Stride.Music.Score;
using Stride.Music.Theory;
using Stride.Utility;

namespace Stride.Music.Layout
{
    public class StaffLinesLayout
    {
        public IReadOnlyList<LineObject> CreateGrandStaffLines(StavesMetrics metrics, double length)
        {
            var treebleStaffOrigin = metrics.Origin;
            var treebleLines = CreateLines(treebleStaffOrigin, metrics.StaffLinesDistance, length, Const.LinesInStaff);
            var bassStaffOrigin = treebleStaffOrigin + new Vector(0.0, metrics.GrandStaffOffset);
            var bassLines = CreateLines(bassStaffOrigin, metrics.StaffLinesDistance, length, Const.LinesInStaff);
            return treebleLines.Concat(bassLines).ToReadOnlyList();
        }

        public IReadOnlyList<LineObject> CreateLedgerLines(
            StavesMetrics metrics,
            IEnumerable<(Tick, GrandStaffLedgerLines)> ledgerLinesForTicks,
            IReadOnlyDictionary<Tick, double> tickPositions)
        {
            var result = new List<LineObject>();
            foreach (var (tick, ledgerLines) in ledgerLinesForTicks)
            {
                var x = tickPositions[tick];
                AddLedgerLinesForNote(
                    result,
                    ledgerLines,
                    x,
                    metrics.LedgerLineLength,
                    metrics.Origin.Y,
                    metrics.StaffLinesDistance,
                    metrics.GrandStaffOffset);
            }
            return result;
        }

        void AddLedgerLinesForNote(
            List<LineObject> lines,
            GrandStaffLedgerLines ledgerLines,
            double noteX,
            double length,
            double staffOriginY,
            double distanceBetweenLines,
            double grandStaffOffset)
        {

            var treebleClefTop = ledgerLines.TreebleClef.Top;
            if (treebleClefTop > 0)
            {
                var y = staffOriginY - treebleClefTop * distanceBetweenLines;
                var origin = new Point(noteX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, treebleClefTop));
            }

            var treebleClefBottom = ledgerLines.TreebleClef.Bottom;
            if (treebleClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines;
                var origin = new Point(noteX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, treebleClefBottom));
            }

            var bassClefTop = ledgerLines.BassClef.Top;
            if (bassClefTop > 0)
            {
                var y = staffOriginY - bassClefTop * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(noteX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, bassClefTop));
            }

            var bassClefBottom = ledgerLines.BassClef.Bottom;
            if (bassClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(noteX, y);
                lines.AddRange(CreateLines(origin, distanceBetweenLines, length, bassClefBottom));
            }
        }

        IEnumerable<LineObject> CreateLines(Point origin, double distanceBetweenLines, double length, int count)
        {
            for (int i = 0; i != count; ++i)
            {
                var offset = distanceBetweenLines * i;
                var begin = origin + new Vector(0.0, offset);
                var end = origin + new Vector(length, offset);
                yield return new LineObject(begin, end);
            }
        }
    }
}