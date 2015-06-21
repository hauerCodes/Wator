using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Wator.Lib.Images;
using Wator.Lib.World;

namespace Wator.Lib.Simulation
{
    public class WatorSimulation : IDisposable
    {
        /// <summary>
        /// The simulation thread
        /// </summary>
        private Thread simulationThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatorSimulation"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public WatorSimulation(WatorSettings settings)
        {
            // stop obj creation if settings wrong
            CheckSettings(settings);

            // create wator world 
            InitializeWatorWorld(settings);

            // save settings in this instance
            this.Settings = settings;

            InitializeSimulationThread();

            // initialize image creator
            InitializeImageCreator();
        }

        #region Properties 

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the image creator.
        /// </summary>
        /// <value>
        /// The image creator.
        /// </value>
        public ImageCreator<WatorWorld> ImageCreator { get; private set; }

        /// <summary>
        /// Gets the is end reached.
        /// </summary>
        /// <value>
        /// The is end reached.
        /// </value>
        public bool IsEndReached { get; private set; }

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
        /// Gets the current simulation round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public int Round { get; private set; }

        #endregion

        public Phase BlackPhase
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Phase WhitePhase
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region Events

        /// <summary>
        /// Occurs when the end of wator simulation is reached.
        /// No sharks/fish left.
        /// </summary>
        public event EventHandler EndReached;

        #endregion

        /// <summary>
        /// Starts the simulation.
        /// </summary>
        public void StartSimulation()
        {
            this.IsRunning = true;

            this.ImageCreator.StartCreator();
            this.simulationThread.Start();
        }

        /// <summary>
        /// Stops the simulation.
        /// </summary>
        public void StopSimulation()
        {
            this.IsRunning = false;

            this.ImageCreator.StopCreator();

            // cancel simulation - threadabortexcep
            // this.simulationThread.Abort();
            // wait for thread exit
            // this.simulationThread.Join();
        }

        private void RunSimulation()
        {
            while (this.IsRunning)
            {
                //perform step
                SimulationStep();
                
                //create image of step
                ImageCreator.AddJob(new ImageJob<WatorWorld>(this.WatorWorld, this.Round));
            }
        }

        /// <summary>
        /// Run one Simulation step.
        /// </summary>
        private void SimulationStep()
        {

        }

        #region Initialize

        /// <summary>
        /// Checks the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Please enter a valid Wator Fields Height!
        /// or
        /// Please enter a valid Wator Fields Width!</exception>
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

            // condition for splitting world in "rows/phases" for paralellization
            if (settings.WorldHeight % 2 != 0)
            {
                throw new ArgumentOutOfRangeException("settings", "Please enter a valid even number as Wator Fields Height!");
            }
        }

        /// <summary>
        /// Initializes the wator world.
        /// </summary>
        /// <param name="settings">The settings.</param>
        private void InitializeWatorWorld(IWatorSettings settings)
        {
            this.IsEndReached = false;
            this.Round = 0;
            this.WatorWorld = new WatorWorld(settings);
        }

        /// <summary>
        /// Initializes the simulation.
        /// </summary>
        private void InitializeSimulationThread()
        {
            this.simulationThread = new Thread(RunSimulation);
            this.IsRunning = false;
        }

        /// <summary>
        /// Initializes the image creator.
        /// </summary>
        private void InitializeImageCreator()
        {
            this.ImageCreator = new ImageCreator<WatorWorld>(this.Settings);
        }

        #endregion

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
    }
}
