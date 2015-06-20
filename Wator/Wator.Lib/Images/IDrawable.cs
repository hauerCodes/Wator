using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wator.Lib.Images
{
    public interface IDrawable
    {
        IColorProvider[,] GetDrawingElements();
    }
}
