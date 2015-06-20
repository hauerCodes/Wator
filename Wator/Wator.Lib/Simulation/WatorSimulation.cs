using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Wator.Lib.World;

namespace Wator.Lib
{
    public class WatorSimulation
    {
        public WatorSimulation(WatorSettings settings)
        {
            this.Settings = settings;
        }

        public event EventHandler EndReached;

        public int IsEndReached
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Wator.Lib.World.IWatorSettings Settings { get; set; }

        public WatorWorld WatorWorld { get; private set; }

        public int Round
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void StartSimulation()
        {
            throw new System.NotImplementedException();
        }

        public void StopSimulation()
        {
            throw new System.NotImplementedException();
        }

        private void InitializeWatorWorld()
        {
            throw new System.NotImplementedException();
        }
    }
}
