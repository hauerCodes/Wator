using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Wator.Lib.World;

namespace Wator.Lib.Simulation
{
    public class Phase
    {
        public Phase(WatorWorld world)
        {

        }

        public List<PhaseExecutionWorker> Workers { get; private set; }

        public int Name { get; private set; }

        public void AddWorker()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void WaitForEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}
