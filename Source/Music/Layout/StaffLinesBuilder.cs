using System;
using System.Collections.Generic;
using System.Windows;
using Stride.Music.Presentation;
using Stride.Music.Theory;

namespace Stride.Music.Layout
{
    public class StaffLinesBuilder
    {
        public IReadOnlyList<LineObject> CreateGrandStaffGeometry(
            StavesMetrics metrics,
            double length,
            GrandStaffLedgerLines ledgerLines,
            double noteX)
        {
            var lines = new List<LineObject>();
            void AddLine(Point begin, Point end) => lines.Add(new LineObject(begin, end));
            AddGrandStaffLines(metrics, length, AddLine);
            AddLedgerLines(
                AddLine,
                ledgerLines,
                noteX,
                metrics.LedgerLineLength,
                metrics.Origin.Y,
                metrics.StaffLinesDistance,
                metrics.GrandStaffOffset);
            return lines;
        }

        void AddGrandStaffLines(StavesMetrics metrics, double length, Action<Point, Point> addLine)
        {
            var treebleStaffOrigin = metrics.Origin;
            AddLines(addLine, treebleStaffOrigin, metrics.StaffLinesDistance, length, Const.LinesInStaff);
            var bassStaffOrigin = treebleStaffOrigin + new Vector(0.0, metrics.GrandStaffOffset);
            AddLines(addLine, bassStaffOrigin, metrics.StaffLinesDistance, length, Const.LinesInStaff);
        }

        void AddLedgerLines(
            Action<Point, Point> addLine,
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
                AddLines(addLine, origin, distanceBetweenLines, length, treebleClefTop);
            }

            var treebleClefBottom = ledgerLines.TreebleClef.Bottom;
            if (treebleClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines;
                var origin = new Point(noteX, y);
                AddLines(addLine, origin, distanceBetweenLines, length, treebleClefBottom);
            }

            var bassClefTop = ledgerLines.BassClef.Top;
            if (bassClefTop > 0)
            {
                var y = staffOriginY - bassClefTop * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(noteX, y);
                AddLines(addLine, origin, distanceBetweenLines, length, bassClefTop);
            }

            var bassClefBottom = ledgerLines.BassClef.Bottom;
            if (bassClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(noteX, y);
                AddLines(addLine, origin, distanceBetweenLines, length, bassClefBottom);
            }
        }

        void AddLines(Action<Point, Point> addLine, Point origin, double distanceBetweenLines, double length, int count)
        {
            for (int i = 0; i != count; ++i)
            {
                var offset = distanceBetweenLines * i;
                var begin = origin + new Vector(0.0, offset);
                var end = origin + new Vector(length, offset);
                addLine(begin, end);
            }
        }
    }
}