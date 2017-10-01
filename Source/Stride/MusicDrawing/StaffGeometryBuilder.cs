using System.Windows;
using System.Windows.Media;
using Stride.Music;

namespace Stride.MusicDrawing
{
    public class StaffGeometryBuilder
    {
        public Geometry CreateGrandStaffGeometry(
            StavesMetrics metrics,
            double length,
            GrandStaffLedgerLines ledgerLines,
            double noteX)
        {
            var geometry = new GeometryGroup();
            AddGrandStaffLines(metrics, length, geometry);
            AddLedgerLines(
                geometry,
                ledgerLines,
                noteX,
                metrics.LedgerLineLength,
                metrics.Origin.Y,
                metrics.StaffLinesDistance,
                metrics.GrandStaffOffset);
            return geometry;
        }

        void AddGrandStaffLines(StavesMetrics metrics, double length, GeometryGroup geometry)
        {
            var treebleStaffOrigin = metrics.Origin;
            AddLines(geometry, treebleStaffOrigin, metrics.StaffLinesDistance, length, Const.LinesInStaff);
            var bassStaffOrigin = treebleStaffOrigin + new Vector(0.0, metrics.GrandStaffOffset);
            AddLines(geometry, bassStaffOrigin, metrics.StaffLinesDistance, length, Const.LinesInStaff);
        }

        void AddLedgerLines(
            GeometryGroup geometry,
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
                AddLines(geometry, origin, distanceBetweenLines, length, treebleClefTop);
            }

            var treebleClefBottom = ledgerLines.TreebleClef.Bottom;
            if (treebleClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines;
                var origin = new Point(noteX, y);
                AddLines(geometry, origin, distanceBetweenLines, length, treebleClefBottom);
            }

            var bassClefTop = ledgerLines.BassClef.Top;
            if (bassClefTop > 0)
            {
                var y = staffOriginY - bassClefTop * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(noteX, y);
                AddLines(geometry, origin, distanceBetweenLines, length, bassClefTop);
            }

            var bassClefBottom = ledgerLines.BassClef.Bottom;
            if (bassClefBottom > 0)
            {
                var y = staffOriginY + Const.LinesInStaff * distanceBetweenLines + grandStaffOffset;
                var origin = new Point(noteX, y);
                AddLines(geometry, origin, distanceBetweenLines, length, bassClefBottom);
            }
        }

        void AddLines(GeometryGroup geometry, Point origin, double distanceBetweenLines, double length, int count)
        {
            for (int i = 0; i != count; ++i)
            {
                var offset = distanceBetweenLines * i;
                var line = new LineGeometry
                {
                    StartPoint = origin + new Vector(0.0, offset),
                    EndPoint = origin + new Vector(length, offset)
                };
                geometry.Children.Add(line);
            }
        }
    }
}