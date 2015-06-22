using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using GalaSoft.MvvmLight;

using Wator.Lib.Simulation;
using Wator.Lib.World;

namespace Wator.Application.ViewModel
{
    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The fish breed time.
        /// </summary>
        private int fishBreedTime;

        /// <summary>
        /// The fish population.
        /// </summary>
        private int fishPopulation;

        /// <summary>
        /// The picture save folder.
        /// </summary>
        private string pictureSaveFolder;

        /// <summary>
        /// The shark breed time.
        /// </summary>
        private int sharkBreedTime;

        /// <summary>
        /// The shark population.
        /// </summary>
        private int sharkPopulation;

        /// <summary>
        /// The shark starve time.
        /// </summary>
        private int sharkStarveTime;

        /// <summary>
        /// The world height
        /// </summary>
        private int worldHeight;

        /// <summary>
        /// The world width
        /// </summary>
        private int worldWidth;

        /// <summary>
        /// The current round
        /// </summary>
        private int currentRound;

        /// <summary>
        /// The current shark population
        /// </summary>
        private int currentSharkPopulation;

        /// <summary>
        /// The current fish population
        /// </summary>
        private int currentFishPopulation;

        /// <summary>
        /// The step time
        /// </summary>
        private TimeSpan stepTime;

        /// <summary>
        /// The wator simulation obj.
        /// </summary>
        private WatorSimulation watorSimulationObj;

        /// <summary>
        /// The command start simulation
        /// </summary>
        private RelayCommand cmdStartSimulation;

        /// <summary>
        /// The command stop simulation
        /// </summary>
        private RelayCommand cmdStopSimulation;

        /// <summary>
        /// The is simulation running
        /// </summary>
        private bool isSimulationRunning;

        /// <summary>
        /// The current image
        /// </summary>
        private ImageSource currentImage;

        public MainViewModel()
        {
            InitializeCommands();
            InitializeSimulationSettings();
        }

        private void InitializeSimulationSettings()
        {
            isSimulationRunning = false;

            WorldHeight = 1000;
            WorldWidth = 1000;

            FishBreedTime = 20;
            SharkBreedTime = 10;
            FishPopulation = 50000;
            SharkPopulation = 30000;
            SharkStarveTime = 15;
            PictureSaveFolder = @"C:\Wator\" + DateTime.Now.ToShortDateString();
        }

        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitializeCommands()
        {
            StartSimulation = new RelayCommand(ExecuteStartSimulation);
            StopSimulation = new RelayCommand(ExcuteStopSimulation);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the height of the world.
        /// </summary>
        /// <value>
        /// The height of the world.
        /// </value>
        public int WorldHeight
        {
            get
            {
                return this.worldHeight;
            }
            set
            {
                this.worldHeight = value;
                RaisePropertyChanged(() => WorldHeight);
            }
        }

        /// <summary>
        /// Gets or sets the width of the world.
        /// </summary>
        /// <value>
        /// The width of the world.
        /// </value>
        public int WorldWidth
        {
            get
            {
                return this.worldWidth;
            }
            set
            {
                this.worldWidth = value;
                RaisePropertyChanged(() => WorldWidth);
            }
        }

        /// <summary>
        /// Gets or sets the fish breed time.
        /// </summary>
        public int FishBreedTime
        {
            get
            {
                return this.fishBreedTime;
            }
            set
            {
                this.fishBreedTime = value;
                RaisePropertyChanged(() => FishBreedTime);
            }
        }

        /// <summary>
        /// Gets or sets the fish population.
        /// </summary>
        public int FishPopulation
        {
            get
            {
                return this.fishPopulation;
            }
            set
            {
                this.fishPopulation = value;
                RaisePropertyChanged(() => FishPopulation);
            }
        }

        /// <summary>
        /// Gets or sets the fish population.
        /// </summary>
        /// <value>
        /// The current fish population.
        /// </value>
        public int CurrentFishPopulation
        {
            get
            {
                return this.currentFishPopulation;
            }
            set
            {
                this.currentFishPopulation = value;
                RaisePropertyChanged(() => CurrentFishPopulation);
            }
        }

        /// <summary>
        /// Gets or sets the fish population.
        /// </summary>
        /// <value>
        /// The current shark population.
        /// </value>
        public int CurrentSharkPopulation
        {
            get
            {
                return this.currentSharkPopulation;
            }
            set
            {
                this.currentSharkPopulation = value;
                RaisePropertyChanged(() => CurrentSharkPopulation);
            }
        }

        /// <summary>
        /// Gets or sets the current round.
        /// </summary>
        /// <value>
        /// The current round.
        /// </value>
        public int CurrentRound
        {
            get
            {
                return this.currentRound;
            }
            set
            {
                this.currentRound = value;
                RaisePropertyChanged(() => CurrentRound);
            }
        }

        /// <summary>
        /// Gets or sets the step time.
        /// </summary>
        /// <value>
        /// The step time.
        /// </value>
        public TimeSpan StepTime
        {
            get
            {
                return this.stepTime;
            }
            set
            {
                this.stepTime = value;
                RaisePropertyChanged(() => StepTime);
            }
        }

        /// <summary>
        /// Gets or sets the picture save folder.
        /// </summary>
        public string PictureSaveFolder
        {
            get
            {
                return this.pictureSaveFolder;
            }
            set
            {
                this.pictureSaveFolder = value;
                RaisePropertyChanged(() => PictureSaveFolder);
            }
        }

        /// <summary>
        /// Gets or sets the shark breed time.
        /// </summary>
        public int SharkBreedTime
        {
            get
            {
                return this.sharkBreedTime;
            }
            set
            {
                this.sharkBreedTime = value;
                RaisePropertyChanged(() => SharkBreedTime);
            }
        }

        /// <summary>
        /// Gets or sets the shark population.
        /// </summary>
        public int SharkPopulation
        {
            get
            {
                return this.sharkPopulation;
            }
            set
            {
                this.sharkPopulation = value;
                RaisePropertyChanged(() => SharkPopulation);
            }
        }

        /// <summary>
        /// Gets or sets the shark starve time.
        /// </summary>
        public int SharkStarveTime
        {
            get
            {
                return this.sharkStarveTime;
            }
            set
            {
                this.sharkStarveTime = value;
                RaisePropertyChanged(() => SharkStarveTime);
            }
        }

        public ImageSource CurrentImage
        {
            get
            {
                return currentImage;
            }
            set
            {
                this.currentImage = value;
                RaisePropertyChanged(() => CurrentImage);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the start simulation.
        /// </summary>
        /// <value>
        /// The start simulation.
        /// </value>
        public RelayCommand StartSimulation
        {
            get
            {
                return cmdStartSimulation;
            }
            set
            {
                cmdStartSimulation = value;
                RaisePropertyChanged(() => StartSimulation);
            }
        }

        /// <summary>
        /// Gets or sets the stop simulation.
        /// </summary>
        /// <value>
        /// The stop simulation.
        /// </value>
        public RelayCommand StopSimulation
        {
            get
            {
                return cmdStopSimulation;
            }
            set
            {
                cmdStopSimulation = value;
                RaisePropertyChanged(() => StopSimulation);
            }
        }

        #endregion

        private void ExecuteStartSimulation()
        {
            this.isSimulationRunning = true;

            try
            {
                if (!Directory.Exists(PictureSaveFolder))
                {
                    Directory.CreateDirectory(PictureSaveFolder);
                }
            }
            catch
            {
                PictureSaveFolder = @".\";
            }

            this.watorSimulationObj = new WatorSimulation(new WatorSettings()
            {
                FishBreedTime = FishBreedTime,
                InitialFishPopulation = FishPopulation,
                InitialSharkPopulation = SharkPopulation,
                SaveFolder = PictureSaveFolder,
                SharkBreedTime = SharkBreedTime,
                SharkStarveTime = SharkStarveTime,
                WorldHeight = WorldHeight,
                WorldWidth = WorldWidth,
            });

            this.watorSimulationObj.ImageCreator.JobFinished += ImageCreatorJobFinished;
            this.watorSimulationObj.StepDone += WatorSimulationObjStepDone;
            this.watorSimulationObj.StartSimulation();
        }

        private void ImageCreatorJobFinished(object sender, Lib.Images.ImageJob<WatorWorld> e)
        {
            App.Current.Dispatcher.Invoke(() =>
                {
                    CurrentImage = new BitmapImage(new Uri(e.File));
                });
        }

        /// <summary>
        /// Wators the simulation obj_ step done.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void WatorSimulationObjStepDone(object sender, SimulationState e)
        {
            try
            {
                App.Current.Dispatcher.Invoke(
                    () =>
                    {
                        this.CurrentFishPopulation = e.FishPopulation;
                        this.CurrentSharkPopulation = e.SharkPopulation;
                        this.CurrentRound = e.Round;
                        this.StepTime = e.StepTime;
                    });
            }
            catch { ;}
        }

        private void ExcuteStopSimulation()
        {
            this.watorSimulationObj.StopSimulation();
        }
    }
}