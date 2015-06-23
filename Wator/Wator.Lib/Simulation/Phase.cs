// -----------------------------------------------------------------------
// <copyright file="Phase.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - Phase.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Simulation
{
    using System;
    using System.Threading;

    using Wator.Lib.World;

    /// <summary>
    /// The phase.
    /// </summary>
    public class Phase
    {
        /// <summary>
        /// The event barrier
        /// after calculation ready of threads
        /// </summary>
        private ManualResetEventSlim eventBarrier;

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
        /// The initial run
        /// </summary>
        private bool initialRun;

        /// <summary>
        /// The world
        /// </summary>
        private WatorWorld world;

        /// <summary>
        /// Initializes a new instance of the <see cref="Phase"/> class.
        /// </summary>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <param name="blackPhase">
        /// if set to <c>true</c> then this represents 
        /// the black phase (even phase) otherwise whitephase (odd phase).
        /// </param>
        public Phase(WatorWorld world, bool blackPhase = false)
        {
            this.world = world;
            this.BlackPhase = blackPhase;
            this.initialRun = true;

            // set number for phase workers
            this.InitializeWorkerNumber();

            // createa events for control of workers
            this.InitializeConcurrencyEvents();

            // create the workers
            this.InitializeWorkers();
        }

        /// <summary>
        /// Gets a value indicating whether this phase is the black/white phase.
        /// black phase only even numbers 0/2/4/6/8/10/12/14
        /// white phase only odd numbers 1/3/5/7/9/11/13/15
        /// </summary>
        /// <value>
        ///   <c>true</c> if [black phase]; otherwise, <c>false</c>.
        /// </value>
        public bool BlackPhase { get; private set; }

        /// <summary>
        /// Gets the workers.
        /// </summary>
        /// <value>
        /// The workers.
        /// </value>
        public PhaseExecutionWorker[] Workers { get; private set; }

        /// <summary>
        /// Starts the signals the workers to start.
        /// </summary>
        public void Start()
        {
            if (initialRun)
            {
                this.eventReady.Wait();
                this.eventReady.Reset();
                this.initialRun = false;
            }

            // signal all workers to start work
            this.eventGo.Set();

            // barrier at calculation end
            this.eventBarrier.Reset();
        }

        /// <summary>
        /// Waits for end of all workers
        /// </summary>
        public void WaitForEnd()
        {
            // wait until all workers are ready 
            this.eventReady.Wait();

            // close "gate" for calculation start
            this.eventGo.Reset();

            // open barrier for run to next go
            this.eventBarrier.Set();

            // reset ready event
            this.eventReady.Reset();
        }

        /// <summary>
        /// Initializes the concurrency events.
        /// </summary>
        private void InitializeConcurrencyEvents()
        {
            this.eventGo = new ManualResetEventSlim(false);
            this.eventBarrier = new ManualResetEventSlim(false);
            this.eventReady = new CountdownEvent(this.phaseWorkerNumber);
        }

        /// <summary>
        /// Initializes the worker number.
        /// </summary>
        private void InitializeWorkerNumber()
        {
            // split the world in x rows - half of it for black/white phase
            // exp. 8 logical cores x 3 => 24 workers per phase * 2 => 48 workers overall
            this.overallWorkerNumber = (Environment.ProcessorCount * world.Settings.ThreadFaktor) * 2;

            // odd number of workers
            if (this.overallWorkerNumber % 2 == 1)
            {
                // even number of overall workers
                this.overallWorkerNumber--;
            }

            // world has less rows than workers
            while ((this.world.Settings.WorldHeight / (double)this.overallWorkerNumber) < 2.0)
            {
                // exp 48 => 24 => 12 => 6 ...
                this.overallWorkerNumber = this.overallWorkerNumber / 2;
            }

            // only half of workers for current phase (only odd / only even)
            this.phaseWorkerNumber = this.overallWorkerNumber / 2;
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

            this.Workers = new PhaseExecutionWorker[this.phaseWorkerNumber];

            // black/even phase go through even numbers - start with 0 increment 2
            // white/odd phase  go through odd numbers -  start with 1 increment 2
            for (int i = this.BlackPhase ? 0 : 1; i < this.overallWorkerNumber; i += 2)
            {
                // last row / exception - row incl. reminder
                if (i == this.overallWorkerNumber - 1)
                {
                    this.Workers[workerCounter++] = new PhaseExecutionWorker(
                        this.world,
                        i,
                        i * workerRowHeight,
                        this.world.Settings.WorldHeight - 1,
                        this.eventGo,
                        this.eventBarrier,
                        this.eventReady);
                }
                else
                {
                    this.Workers[workerCounter++] = new PhaseExecutionWorker(
                        this.world,
                        i,
                        i * workerRowHeight,

                        // inclusive last row of part
                        (i * workerRowHeight) + workerRowHeight - 1,
                        this.eventGo,
                        this.eventBarrier,
                        this.eventReady);
                }
            }
        }
    }
}