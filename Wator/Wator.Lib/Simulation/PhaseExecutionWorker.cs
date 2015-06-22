using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// The worker identifier
        /// </summary>
        private int workerId;

        /// <summary>
        /// The phase task
        /// </summary>
        //private Task workerTask;
        private Thread workerThread;

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
        /// Wait for activate worker
        /// </summary>
        private ManualResetEventSlim eventGo;

        /// <summary>
        /// The event go 
        /// Barrier after calcualtion ready
        /// </summary>
        private ManualResetEventSlim eventBarrier;

        /// <summary>
        /// The event go 
        /// Wait for activate worker
        /// </summary>
        private CountdownEvent eventReady;

        /// <summary>
        /// The world width
        /// </summary>
        private int worldWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseExecutionWorker" /> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="workerId">The worker identifier.</param>
        /// <param name="startRow">The worker start row.</param>
        /// <param name="endRow">The worker end row. (inclusive)</param>
        /// <param name="eventGo">The event go.</param>
        /// <param name="eventBarrier">The event barrier.</param>
        /// <param name="eventReady">The event ready.</param>
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

            //create and start task
            this.InitializeTask();
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <value>
        /// The world.
        /// </value>
        public WatorWorld World { get; private set; }

        /// <summary>
        /// Gets the start row.
        /// </summary>
        /// <value>
        /// The start row.
        /// </value>
        public int StartRow { get; private set; }

        /// <summary>
        /// Gets the end row.
        /// </summary>
        /// <value>
        /// The end row.
        /// </value>
        public int EndRow { get; private set; }

        #endregion

        /// <summary>
        /// Runs the worker "task".
        /// </summary>
        public void RunWorker()
        {
            Debug.WriteLine("Worker {0} - Starting", workerId);

            while (IsActive)
            {
                Debug.WriteLine("Worker {0} - Waiting for event go", workerId);

                // wait for go - start calculation (cancelation token supported)
                eventGo.Wait(this.cancelToken);

                Debug.WriteLine("Worker {0} - Running", workerId);

                if (this.cancelToken.IsCancellationRequested || !IsActive)
                {
                    return;
                }

                // calculate runnning 
                Calculate();

                Debug.WriteLine("Worker {0} - Singal Ready", workerId);
                // ready - countdownevent (running workers--) 
                // wait for all phase workers to end current step
                eventReady.Signal();

                // wait for next calculation sign - eventBarrier (start of loop)
                eventBarrier.Wait(this.cancelToken);
            }
        }

        /// <summary>
        /// Calculates this instance.
        /// </summary>
        private void Calculate()
        {
            for (int y = StartRow; y <= EndRow; y++)
            {
                for (int x = 0; x < worldWidth; x++)
                {
                    // if there is an animal on this cell that has not been moved in this simulation step          
                    if (World.Fields[y, x].Animal != null && !World.Fields[y, x].Animal.IsMoved)
                    {
                        // then we execute it                       
                        World.Fields[y, x].Animal.Step();
                    }
                }
            }
        }

        /// <summary>
        /// Stops the worker.
        /// </summary>
        public void StopWorker()
        {
            this.IsActive = false;
            cancelTokenSource.Cancel();
        }

        /// <summary>
        /// Initializes the task.
        /// </summary>
        private void InitializeTask()
        {
            this.cancelTokenSource = new CancellationTokenSource();
            this.cancelToken = cancelTokenSource.Token;

            this.IsActive = true;

            // run task from thread pool
            //this.workerTask = Task.Factory.StartNew(this.RunWorker, this.cancelToken);

            this.workerThread = new Thread(RunWorker);
            this.workerThread.IsBackground = true;
            this.workerThread.Start();
        }
    }
}
