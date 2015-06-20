using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wator.Lib.World
{
    public interface IWatorSettings
    {
        int WorldHeight { get; set; }
        int WorldWidth { get; set; }
        int InitialFishPopulation { get; set; }
        int InitialSharkPopulation { get; set; }
        int SharkBreedTime { get; set; }
        int FishBreedTime { get; set; }
        int SharkStarveTime { get; set; }

        Color WaterColor
        {
            get;
            set;
        }

        Color FishColor
        {
            get;
            set;
        }

        Color SharkColor
        {
            get;
            set;
        }
    }
}
