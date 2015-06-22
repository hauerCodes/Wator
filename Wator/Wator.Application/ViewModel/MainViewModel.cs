// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Application - MainViewModel.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Application.ViewModel
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Wator.Lib.Images;
    using Wator.Lib.Simulation;
    using Wator.Lib.World;

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The command start simulation
        /// </summary>
        private RelayCommand cmdStartSimulation;

        /// <summary>
        /// The command stop simulation
        /// </summary>
        private RelayCommand cmdStopSimulation;

        /// <summary>
        /// The current fish population
        /// </summary>
        private int currentFishPopulation;

        /// <summary>
        /// The current image
        /// </summary>
        private ImageSource currentImage;

        /// <summary>
        /// The current round
        /// </summary>
        private int currentRound;

        /// <summary>
        /// The current shark population
        /// </summary>
        private int currentSharkPopulation;

        /// <summary>
        /// The fish breed time.
        /// </summary>
        private int fishBreedTime;

        /// <summary>
        /// The fish population.
        /// </summary>
        private int fishPopulation;

        /// <summary>
        /// The is simulation running
        /// </summary>
        private bool isSimulationRunning;

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
        /// The step time
        /// </summary>
        private TimeSpan stepTime;

        /// <summary>
        /// The wator simulation obj.
        /// </summary>
        private WatorSimulation watorSimulationObj;

        /// <summary>
        /// The world height
        /// </summary>
        private int worldHeight;

        /// <summary>
        /// The world width
        /// </summary>
        private int worldWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.InitializeCommands();
            this.InitializeSimulationSettings();
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
                this.RaisePropertyChanged(() => this.CurrentFishPopulation);
            }
        }

        /// <summary>
        /// Gets or sets the current image.
        /// </summary>
        public ImageSource CurrentImage
        {
            get
            {
                return this.currentImage;
            }

            set
            {
                this.currentImage = value;
                this.RaisePropertyChanged(() => this.CurrentImage);
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
                this.RaisePropertyChanged(() => this.CurrentRound);
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
                this.RaisePropertyChanged(() => this.CurrentSharkPopulation);
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
                this.RaisePropertyChanged(() => this.FishBreedTime);
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
                this.RaisePropertyChanged(() => this.FishPopulation);
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
                this.RaisePropertyChanged(() => this.PictureSaveFolder);
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
                this.RaisePropertyChanged(() => this.SharkBreedTime);
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
                this.RaisePropertyChanged(() => this.SharkPopulation);
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
                this.RaisePropertyChanged(() => this.SharkStarveTime);
            }
        }

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
                return this.cmdStartSimulation;
            }

            set
            {
                this.cmdStartSimulation = value;
                this.RaisePropertyChanged(() => this.StartSimulation);
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
                this.RaisePropertyChanged(() => this.StepTime);
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
                return this.cmdStopSimulation;
            }

            set
            {
                this.cmdStopSimulation = value;
                this.RaisePropertyChanged(() => this.StopSimulation);
            }
        }

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
                this.RaisePropertyChanged(() => this.WorldHeight);
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
                this.RaisePropertyChanged(() => this.WorldWidth);
            }
        }

        /// <summary>
        /// The excute stop simulation.
        /// </summary>
        private void ExcuteStopSimulation()
        {
            this.watorSimulationObj.StopSimulation();
        }

        /// <summary>
        /// The execute start simulation.
        /// </summary>
        private void ExecuteStartSimulation()
        {
            this.isSimulationRunning = true;

            try
            {
                if (!Directory.Exists(this.PictureSaveFolder))
                {
                    Directory.CreateDirectory(this.PictureSaveFolder);
                }
            }
            catch
            {
                this.PictureSaveFolder = @".\";
            }

            this.watorSimulationObj =
                new WatorSimulation(
                    new WatorSettings()
                        {
                            FishBreedTime = this.FishBreedTime, 
                            InitialFishPopulation = this.FishPopulation, 
                            InitialSharkPopulation = this.SharkPopulation, 
                            SaveFolder = this.PictureSaveFolder, 
                            SharkBreedTime = this.SharkBreedTime, 
                            SharkStarveTime = this.SharkStarveTime, 
                            WorldHeight = this.WorldHeight, 
                            WorldWidth = this.WorldWidth, 
                        });

            this.watorSimulationObj.ImageFinished += this.ImageCreatorJobFinished;
            this.watorSimulationObj.StepDone += this.WatorSimulationObjStepDone;
            this.watorSimulationObj.StartSimulation();
        }

        /// <summary>
        /// The image creator job finished.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ImageCreatorJobFinished(object sender, ImageJob<WatorWorld> e)
        {
            Application.Current.Dispatcher.Invoke(() => { this.CurrentImage = new BitmapImage(new Uri(e.File)); });
        }

        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitializeCommands()
        {
            this.StartSimulation = new RelayCommand(this.ExecuteStartSimulation);
            this.StopSimulation = new RelayCommand(this.ExcuteStopSimulation);
        }

        /// <summary>
        /// The initialize simulation settings.
        /// </summary>
        private void InitializeSimulationSettings()
        {
            this.isSimulationRunning = false;

            this.WorldHeight = 1000;
            this.WorldWidth = 1000;

            this.FishBreedTime = 20;
            this.SharkBreedTime = 10;
            this.FishPopulation = 50000;
            this.SharkPopulation = 30000;
            this.SharkStarveTime = 15;
            this.PictureSaveFolder = @"C:\Wator\" + DateTime.Now.ToShortDateString();
        }

        /// <summary>
        /// Wators the simulation obj_ step done.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WatorSimulationObjStepDone(object sender, SimulationState e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(
                    () =>
                        {
                            this.CurrentFishPopulation = e.FishPopulation;
                            this.CurrentSharkPopulation = e.SharkPopulation;
                            this.CurrentRound = e.Round;
                            this.StepTime = e.StepTime;
                        });
            }
            catch
            {
                ;
            }
        }
    }
}