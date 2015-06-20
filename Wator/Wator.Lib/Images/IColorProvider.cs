using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wator.Lib.Images
{
    public interface IColorProvider
    {
        Color DrawColor
        {
            get;
        }
    }
}
