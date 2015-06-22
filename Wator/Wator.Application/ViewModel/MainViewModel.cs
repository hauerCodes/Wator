using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// The wator simulation obj.
        /// </summary>
        private WatorSimulation watorSimulationObj;

        /// <summary>
        /// The settings
        /// </summary>
        private WatorSettings settings;

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

            FishBreedTime = 2;
            SharkBreedTime = 5;
            FishPopulation = 10000;
            SharkPopulation = 10000;
            SharkStarveTime = 5;
            PictureSaveFolder = @"C:\Temp\Wator";
        }

        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitializeCommands()
        {
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
    }
}