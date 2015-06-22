// -----------------------------------------------------------------------
// <copyright file="PhaseExecutionWorker.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - PhaseExecutionWorker.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Simulation
{
    using System.Diagnostics;
    using System.Threading;

    using Wator.Lib.World;

    /// <summary>
    /// The phase execution worker.
    /// </summary>
    public class PhaseExecutionWorker
    {
        /// <summary>
        /// The cancel token
        /// </summary>
        private CancellationToken cancelToken;

        /// <summary>
        /// The cancel token source
        /// </summary>
        private CancellationTokenSource cancelTokenSource;

        /// <summary>
        /// The event go 
        /// Barrier after calcualtion ready
        /// </summary>
        private ManualResetEventSlim eventBarrier;

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
        /// The worker identifier
        /// </summary>
        private int workerId;

        /// <summary>
        /// The phase task
        /// </summary>
        // private Task workerTask;
        private Thread workerThread;

        /// <summary>
        /// The world width
        /// </summary>
        private int worldWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseExecutionWorker"/> class.
        /// </summary>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <param name="workerId">
        /// The worker identifier.
        /// </param>
        /// <param name="startRow">
        /// The worker start row.
        /// </param>
        /// <param name="endRow">
        /// The worker end row. (inclusive)
        /// </param>
        /// <param name="eventGo">
        /// The event go.
        /// </param>
        /// <param name="eventBarrier">
        /// The event barrier.
        /// </param>
        /// <param name="eventReady">
        /// The event ready.
        /// </param>
        public PhaseExecutionWorker(
            WatorWorld world, 
            int workerId, 
            int startRow, 
            int endRow, 
            ManualResetEventSlim eventGo, 
            ManualResetEventSlim eventBarrier, 
            CountdownEvent eventReady)
        {
            this.workerId = workerId;
            this.World = world;
            this.StartRow = startRow;
            this.EndRow = endRow;
            this.worldWidth = world.Settings.WorldWidth;

            this.eventGo = eventGo;
            this.eventReady = eventReady;
            this.eventBarrier = eventBarrier;

            // create and start task
            this.InitializeTask();
        }

        /// <summary>
        /// Gets the end row.
        /// </summary>
        /// <value>
        /// The end row.
        /// </value>
        public int EndRow { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the start row.
        /// </summary>
        /// <value>
        /// The start row.
        /// </value>
        public int StartRow { get; private set; }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <value>
        /// The world.
        /// </value>
        public WatorWorld World { get; private set; }

        /// <summary>
        /// Runs the worker "task".
        /// </summary>
        public void RunWorker()
        {
            Debug.WriteLine("Worker {0} - Starting", this.workerId);

            while (this.IsActive)
            {
                Debug.WriteLine("Worker {0} - Waiting for event go", this.workerId);

                // wait for go - start calculation (cancelation token supported)
                this.eventGo.Wait(this.cancelToken);

                Debug.WriteLine("Worker {0} - Running", this.workerId);

                if (this.cancelToken.IsCancellationRequested || !this.IsActive)
                {
                    return;
                }

                // calculate runnning 
                this.Calculate();

                Debug.WriteLine("Worker {0} - Singal Ready", this.workerId);

                // ready - countdownevent (running workers--) 
                // wait for all phase workers to end current step
                this.eventReady.Signal();

                // wait for next calculation sign - eventBarrier (start of loop)
                this.eventBarrier.Wait(this.cancelToken);
            }
        }

        /// <summary>
        /// Stops the worker.
        /// </summary>
        public void StopWorker()
        {
            this.IsActive = false;
            this.cancelTokenSource.Cancel();
        }

        /// <summary>
        /// Calculates this instance.
        /// </summary>
        private void Calculate()
        {
            for (int y = this.StartRow; y <= this.EndRow; y++)
            {
                for (int x = 0; x < this.worldWidth; x++)
                {
                    // if there is an animal on this cell that has not been moved in this simulation step          
                    if (this.World.Fields[y, x].Animal != null && !this.World.Fields[y, x].Animal.IsMoved)
                    {
                        // then we execute it                       
                        this.World.Fields[y, x].Animal.Step();
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the task.
        /// </summary>
        private void InitializeTask()
        {
            this.cancelTokenSource = new CancellationTokenSource();
            this.cancelToken = this.cancelTokenSource.Token;

            this.IsActive = true;

            // run task from thread pool
            // this.workerTask = Task.Factory.StartNew(this.RunWorker, this.cancelToken);
            this.workerThread = new Thread(this.RunWorker);
            this.workerThread.IsBackground = true;
            this.workerThread.Start();
        }
    }
}