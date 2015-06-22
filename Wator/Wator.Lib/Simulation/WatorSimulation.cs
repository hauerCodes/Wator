// -----------------------------------------------------------------------
// <copyright file="WatorSimulation.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - WatorSimulation.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Simulation
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Wator.Lib.Images;
    using Wator.Lib.World;

    /// <summary>
    /// The wator simulation.
    /// </summary>
    public class WatorSimulation : IDisposable
    {
        /// <summary>
        /// The current fish popluation
        /// </summary>
        private static int currentFishPopluation;

        /// <summary>
        /// The current shark popluation
        /// </summary>
        private static int currentSharkPopluation;

        /// <summary>
        /// The cancel token
        /// </summary>
        private CancellationToken cancelToken;

        /// <summary>
        /// The cancel token source
        /// </summary>
        private CancellationTokenSource cancelTokenSource;

        /// <summary>
        /// The create world task
        /// </summary>
        private Task createWorldTask;

        /// <summary>
        /// The image creator
        /// </summary>
        private ImageCreator<WatorWorld> imageCreator;

        /// <summary>
        /// The phase randomizer
        /// </summary>
        private Random phaseRandomizer;

        /// <summary>
        /// The black/white phase as array
        /// </summary>
        private Phase[] simulationPhases;

        /// <summary>
        /// The simulation thread
        /// </summary>
        private Thread simulationThread;

        /// <summary>
        /// The step watch
        /// </summary>
        private Stopwatch stepWatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatorSimulation"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public WatorSimulation(WatorSettings settings)
        {
            // stop obj creation if settings wrong
            this.CheckSettings(settings);

            this.InitializeSimulationThread();

            // save settings in this instance
            this.Settings = settings;

            // intialize stop watches
            this.InitializeTimeTracking();

            this.cancelTokenSource = new CancellationTokenSource();
            this.cancelToken = this.cancelTokenSource.Token;

            this.createWorldTask = Task.Factory.StartNew(
                () =>
                    {
                        // create wator world 
                        this.InitializeWatorWorld(settings);

                        // initialize image creator
                        this.InitializeImageCreator();

                        // intialize concurrency of simulation (phases)
                        this.InitializeConcurrency();
                    }, 
                this.cancelToken);
        }

        /// <summary>
        /// Occurs when the end of wator simulation is reached.
        /// No sharks/fish left.
        /// </summary>
        public event EventHandler EndReached;

        /// <summary>
        /// Occurs when a image is finished.
        /// </summary>
        public event EventHandler<ImageJob<WatorWorld>> ImageFinished;

        /// <summary>
        /// Occurs when a simulation step is done.
        /// </summary>
        public event EventHandler<SimulationState> StepDone;

        /// <summary>
        /// Gets the is end reached.
        /// </summary>
        /// <value>
        /// The is end reached.
        /// </value>
        public bool IsEndReached { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the current simulation round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public int Round { get; private set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public IWatorSettings Settings { get; private set; }

        /// <summary>
        /// Gets the wator world.
        /// </summary>
        /// <value>
        /// The wator world.
        /// </value>
        public WatorWorld WatorWorld { get; private set; }

        /// <summary>
        /// Changes the fish population.
        /// </summary>
        /// <param name="increase">
        /// if set to <c>true</c> increase otherwise decrease.
        /// </param>
        public static void ChangeFishPopulation(bool increase = true)
        {
            if (increase)
            {
                Interlocked.Increment(ref currentFishPopluation);
            }
            else
            {
                Interlocked.Decrement(ref currentFishPopluation);
            }
        }

        /// <summary>
        /// Changes the shark population.
        /// </summary>
        /// <param name="increase">
        /// if set to <c>true</c> increase otherwise decrease.
        /// </param>
        public static void ChangeSharkPopulation(bool increase = true)
        {
            if (increase)
            {
                Interlocked.Increment(ref currentSharkPopluation);
            }
            else
            {
                Interlocked.Decrement(ref currentSharkPopluation);
            }
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben aus, 
        /// die mit dem Freigeben, Zurückgeben oder 
        /// Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            if (this.simulationThread.IsAlive)
            {
                this.IsRunning = false;
                this.simulationThread.Abort();
            }

            this.simulationThread = null;
        }

        /// <summary>
        /// Starts the simulation.
        /// </summary>
        public void StartSimulation()
        {
            this.IsRunning = true;

            this.simulationThread.Start();
        }

        /// <summary>
        /// Stops the simulation.
        /// </summary>
        public void StopSimulation()
        {
            this.cancelTokenSource.Cancel();
            this.IsRunning = false;

            this.imageCreator.StopCreator();

            // cancel simulation - fire threadabortexcep
            this.simulationThread.Abort();

            // wait for thread exit
            this.simulationThread.Join();
        }

        /// <summary>
        /// Called when end reached.
        /// </summary>
        protected virtual void OnEndReached()
        {
            var handler = this.EndReached;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when a image is finished.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnImageFinished(ImageJob<WatorWorld> e)
        {
            var handler = this.ImageFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called when a simulation step is done.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnStepDone(SimulationState e)
        {
            var handler = this.StepDone;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Checks the settings.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Please enter a valid Wator Fields Height!
        /// or
        /// Please enter a valid Wator Fields Width!
        /// </exception>
        private void CheckSettings(IWatorSettings settings)
        {
            if (settings.WorldHeight <= 0)
            {
                throw new ArgumentOutOfRangeException("settings", "Please enter a valid Wator Fields Height!");
            }

            if (settings.WorldWidth <= 0)
            {
                throw new ArgumentOutOfRangeException("settings", "Please enter a valid Wator Fields Width!");
            }

            //// condition for splitting world in "rows/phases" for paralellization
            // if (settings.WorldHeight % 2 != 0)
            // {
            // throw new ArgumentOutOfRangeException("settings", "Please enter a valid even number as Wator Fields Height!");
            // }

            // if ((settings.WorldHeight / (Environment.ProcessorCount * 3)) > 2)
            // {
            // throw new ArgumentOutOfRangeException("settings", "Please enter a valid even number as Wator Fields Height!");
            // }
        }

        /// <summary>
        /// Cleans up memory.
        /// </summary>
        private void CleanUp()
        {
            try
            {
                // only in .NET 4.5.1
                // GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// Initializes the concurrency.
        /// Creates phases, splits up world in "rows" -> phase execution workers
        /// sets up tasks for execution
        /// waits for simulation to start
        /// </summary>
        private void InitializeConcurrency()
        {
            this.simulationPhases = new[]
                                        {
                                            new Phase(this.WatorWorld, true), // black phase
                                            new Phase(this.WatorWorld, false) // white phase
                                        };

            this.phaseRandomizer = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Initializes the image creator.
        /// </summary>
        private void InitializeImageCreator()
        {
            this.imageCreator = new ImageCreator<WatorWorld>(this.Settings);
            this.imageCreator.JobFinished += (sender, e) => this.OnImageFinished(e);
        }

        /// <summary>
        /// Initializes the simulation.
        /// </summary>
        private void InitializeSimulationThread()
        {
            this.simulationThread = new Thread(this.RunSimulation);
            this.IsRunning = false;
        }

        /// <summary>
        /// Initializes the time tracking.
        /// </summary>
        private void InitializeTimeTracking()
        {
            this.stepWatch = new Stopwatch();
        }

        /// <summary>
        /// Initializes the wator world.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        private void InitializeWatorWorld(IWatorSettings settings)
        {
            currentSharkPopluation = settings.InitialSharkPopulation;
            currentFishPopluation = settings.InitialFishPopulation;
            this.IsEndReached = false;
            this.Round = 0;

            this.WatorWorld = new WatorWorld(settings);
        }

        /// <summary>
        /// Runs the simulation.
        /// </summary>
        private void RunSimulation()
        {
            try
            {
                this.createWorldTask.Wait(this.cancelToken);
            }
            catch
            {
                if (this.cancelToken.IsCancellationRequested)
                {
                    return;
                }
            }

            // start image processor
            this.imageCreator.StartCreator();

            // begin main simulation loop
            while (this.IsRunning)
            {
                try
                {
                    this.stepWatch.Start();

                    // perform step
                    this.SimulationStep();

                    this.stepWatch.Stop();

                    // create image of step
                    this.imageCreator.AddJob(new ImageJob<WatorWorld>(this.WatorWorld, this.Round));

                    // Increase round
                    this.Round++;

                    // simulation step done
                    this.OnStepDone(
                        new SimulationState()
                            {
                                FishPopulation = currentFishPopluation, 
                                SharkPopulation = currentSharkPopluation, 
                                Round = this.Round, 
                                StepTime = this.stepWatch.Elapsed
                            });

                    this.stepWatch.Reset();

                    this.CleanUp();
                }
                catch (ThreadAbortException ex)
                {
                    Debug.WriteLine("Simluation Thread aborted.");
                    return;
                }
            }
        }

        /// <summary>
        /// Run one Simulation step.
        /// </summary>
        private void SimulationStep()
        {
            int firstPhase = this.phaseRandomizer.Next(0, 1);
            int secondPhase = firstPhase == 0 ? 1 : 0; // opposite

            // Start phases
            this.simulationPhases[firstPhase].Start();
            this.simulationPhases[firstPhase].WaitForEnd();

            this.simulationPhases[secondPhase].Start();
            this.simulationPhases[secondPhase].WaitForEnd();

            // reset moved stats
            this.WatorWorld.FinishSteps(false);
        }
    }
}