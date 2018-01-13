using System.Collections.Generic;
using Stride.Music.Score;

namespace Stride.Music.Layout
{
    public interface Layout
    {
        IEnumerable<LayoutObject> CreateLayout(Page page);
    }
}