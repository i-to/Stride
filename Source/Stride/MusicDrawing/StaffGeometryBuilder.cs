using System.Windows;
using System.Windows.Media;
using Stride.Music;

namespace Stride.MusicDrawing
{
    public class StaffGeometryBuilder
    {
        public Geometry CreateGrandStaffGeometry(StavesMetrics metrics, double length)
        {
            var geometry = new GeometryGroup();
            AddStaffGeometry(geometry, metrics, length, false);
            AddStaffGeometry(geometry, metrics, length, true);
            return geometry;
        }

        void AddStaffGeometry(GeometryGroup geometry, StavesMetrics metrics, double length, bool grandStaffOffset)
        {
            for (int i = 0; i != Const.LinesInStaff; ++i)
            {
                var yOffset = metrics.StaffLinesDistance * i;
                if (grandStaffOffset)
                    yOffset += metrics.GrandStaffOffset;
                var line = new LineGeometry
                {
                    StartPoint = metrics.Origin + new Vector(0.0, yOffset),
                    EndPoint = metrics.Origin + new Vector(length, yOffset)
                };
                geometry.Children.Add(line);
            }
        }
    }
}