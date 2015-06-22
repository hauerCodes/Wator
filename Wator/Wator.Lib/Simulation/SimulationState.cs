using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wator.Lib.Simulation
{
    public class SimulationState
    {
        public int Round { get; set; }

        public TimeSpan StepTime { get; set; }

        public int FishPopulation { get; set; }

        public int SharkPopulation { get; set; }
    }
}
