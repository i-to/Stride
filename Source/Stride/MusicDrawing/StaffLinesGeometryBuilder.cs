using System.Windows;
using System.Windows.Media;

namespace Stride.MusicDrawing
{
    public class StaffLinesGeometryBuilder
    {
        public Geometry CreateStaffLinesGeometry(Point origin, double length, double staffLinesDistance)
        {
            var StaffLineCount = 5;
            var geometry = new GeometryGroup();
            for (int i = 0; i != StaffLineCount; ++i)
            {
                var yOffset = staffLinesDistance * i;
                var line = new LineGeometry
                {
                    StartPoint = origin + new Vector(0.0, yOffset),
                    EndPoint = origin + new Vector(length, yOffset)
                };
                geometry.Children.Add(line);
            }
            return geometry;
        }
    }
}