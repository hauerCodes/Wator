using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Wator.Lib.World;

namespace Wator.Lib.Simulation
{
    public class PhaseExecutionWorker
    {
        /// <summary>
        /// The phase task
        /// </summary>
        private Task phaseTask;

        /// <summary>
        /// The event go 
        /// Wait for activate worker
        /// </summary>
        private ManualResetEventSlim eventGo;

        /// <summary>
        /// The event go 
        /// Wait for activate worker
        /// </summary>
        private CountdownEvent eventReady;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseExecutionWorker"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <param name="eventGo">The event go.</param>
        /// <param name="eventReady">The event ready.</param>
        public PhaseExecutionWorker(WatorWorld world,  int startRow, int endRow, 
            ManualResetEventSlim eventGo, CountdownEvent eventReady)
        {
            this.World = world;
            this.StartRow = startRow;
            this.EndRow = endRow;
            this.IsActive = false;

            this.eventGo = eventGo;
            this.eventReady = eventReady;

            this.phaseTask = new Task(RunPhase);
        }

        public bool IsActive { get; private set; }

        public WatorWorld World { get; private set; }

        public int StartRow { get; private set; }

        public int EndRow { get; private set; }

        public void RunPhase()
        {
            while (IsActive)
            {
                // wait for go - start calculation
                eventGo.Wait();

                // calculate runnning 

                // ready - countdownevent (count--) 
                // wait for alles phase workers to end current step
                eventReady.Signal();

                // wait for next calculation sign - eventGo (start of loop)
            }
        }
    }
}
