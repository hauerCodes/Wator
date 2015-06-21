using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Wator.Lib.World;

namespace Wator.Lib.Simulation
{
    public class Phase
    {
        /// <summary>
        /// The world
        /// </summary>
        private WatorWorld world;

        /// <summary>
        /// The event go
        /// start/stop execution workers 
        /// </summary>
        private ManualResetEventSlim eventGo;

        /// <summary>
        /// The event ready
        /// wait for workers to end current step
        /// </summary>
        private CountdownEvent eventReady;

        /// <summary>
        /// The max number of execution workers
        /// </summary>
        private int overallWorkerNumber;

        /// <summary>
        /// The max number of execution workers
        /// </summary>
        private int phaseWorkerNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Phase"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="blackPhase">if set to <c>true</c> then this represents 
        /// the black phase otherwise whitephase.</param>
        public Phase(WatorWorld world, bool blackPhase = false)
        {
            this.world = world;
            this.BlackPhase = blackPhase;

            // set number for phase workers
            this.InitializeWorkerNumber();

            // createa events for control of workers
            this.InitializeConcurrencyEvents();

            // create the workers
            this.InitializeWorkers();
        }

        /// <summary>
        /// Gets the workers.
        /// </summary>
        /// <value>
        /// The workers.
        /// </value>
        public PhaseExecutionWorker[] Workers { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this phase is the black/white phase.
        /// black phase only even numbers 0/2/4/6/8/10/12/14
        /// white phase only odd numbers 1/3/5/7/9/11/13/15
        /// </summary>
        /// <value>
        ///   <c>true</c> if [black phase]; otherwise, <c>false</c>.
        /// </value>
        public bool BlackPhase { get; private set; }

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

        #region Initialize

        /// <summary>
        /// Initializes the worker number.
        /// </summary>
        private void InitializeWorkerNumber()
        {
            //split the world in x rows - half of it (even part or not) for current phase
            this.overallWorkerNumber = (Environment.ProcessorCount * 3);

            // world has less rows than workers
            if ((world.Settings.WorldHeight / (double)overallWorkerNumber) < 2.0)
            {
                this.overallWorkerNumber = Environment.ProcessorCount;
            }

            if (this.overallWorkerNumber % 2 == 1) //odd number
            {
                this.overallWorkerNumber--; //even number of overall workers
            }

            //only half of workers for current phase (only odd / only even)
            this.phaseWorkerNumber = this.overallWorkerNumber / 2;
        }

        /// <summary>
        /// Initializes the concurrency events.
        /// </summary>
        private void InitializeConcurrencyEvents()
        {
            this.eventGo = new ManualResetEvent(false);
            this.eventReady = new CountdownEvent(phaseWorkerNumber);
        }

        /// <summary>
        /// Initializes the workers.
        /// Splits the world in rows for the workers
        /// (info: last workers gehts one more row than all others (remainder/rest))
        /// </summary>
        private void InitializeWorkers()
        {
            int workerRowHeight = this.world.Settings.WorldHeight / this.overallWorkerNumber;
            int workerCounter = 0;

            this.Workers = new PhaseExecutionWorker[phaseWorkerNumber];

            // if we are in the black/even phase go through even numbers (start with 0 + 2)
            // white phase odd numbers start with 1 + 2
            for (int i = BlackPhase ? 0 : 1; i < this.overallWorkerNumber; i += 2)
            {
                // last row / exception - row incl. reminder
                if ((i * workerRowHeight) > (world.Settings.WorldHeight - workerRowHeight))
                {
                    this.Workers[workerCounter++] = new PhaseExecutionWorker(
                        world,
                        i * workerRowHeight,
                        this.world.Settings.WorldHeight,
                        eventGo,
                        eventReady);
                }
                else
                {
                    this.Workers[workerCounter++] = new PhaseExecutionWorker(
                        world,
                        i * workerRowHeight,
                        (i * workerRowHeight) + workerRowHeight + 1,
                        eventGo,
                        eventReady);
                }
            }
        }


        #endregion
    }
}
