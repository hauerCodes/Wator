using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Wator.Lib.World;

namespace Wator.Lib
{
    public class Phase
    {
        public Phase(WatorWorld world, int startRow, int endRow)
        {

        }

        public int StartRow { get; private set; }

        public int EndRow { get; private set; }

        public void RunPhase()
        {
            throw new System.NotImplementedException();
        }
    }
}
