using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Wator.Lib.World;

namespace Wator.Lib
{
    public class Phase
    {
        /// <summary>
        /// The phase task
        /// </summary>
        private Task phaseTask;

        public Phase(WatorWorld world, int startRow, int endRow)
        {
            this.World = world;
            this.StartRow = startRow;
            this.EndRow = endRow;
            this.IsActive = false;
            this.IsStopped = false; 

            this.phaseTask = new Task(RunPhase);
        }


        public bool IsActive { get; set; }

        public bool IsStopped { get; set; }

        public WatorWorld World { get; private set; }

        public int StartRow { get; private set; }

        public int EndRow { get; private set; }

        public void RunPhase()
        {
            while (!IsStopped)
            {
                //calculate phase 
                if (IsActive)
                {
                }

                //set current step done
                IsActive = false;

                //barrier - wait for alles phases

                //wait for next calculation sign
                //Signaling? -> !no while isactive pooling
            }
        }
    }
}
