using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wator.Lib.Simulation;
using Wator.Lib.World;

namespace Wator.App.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private int round;
        private int currentFishPopulation;
        private int currentSharkPopulation;
        private WatorSimulation simulation;
        private IWatorSettings watorSettings;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the fish breed time.
        /// </summary>
        public int FishBreedTime { get; set; }

        /// <summary>
        /// Gets or sets the fish population.
        /// </summary>
        public int FishPopulation { get; set; }

        /// <summary>
        /// Gets or sets the shark population.
        /// </summary>
        public int SharkPopulation { get; set; }

        /// <summary>
        /// Gets or sets the shark breed time.
        /// </summary>
        public int SharkBreedTime { get; set; }

        /// <summary>
        /// Gets or sets the shark starve time.
        /// </summary>
        public int SharkStarveTime { get; set; }

        /// <summary>
        /// Gets or sets the world height.
        /// </summary>
        public int WorldHeight { get; set; }

        /// <summary>
        /// Gets or sets the world width.
        /// </summary>
        public int WorldWidth { get; set; }

        /// <summary>
        /// Gets or sets the current shark population.
        /// </summary>
        public int CurrentSharkPopulation
        {
            get { return currentSharkPopulation; }
            set
            {
                currentSharkPopulation = value;
                OnPropertyChanged("CurrentSharkPopulation");
                
            }
        }

        /// <summary>
        /// Gets or sets the current fish population.
        /// </summary>
        public int CurrentFishPopulation
        {
            get { return currentFishPopulation; }
            set
            {
                currentFishPopulation = value;
                OnPropertyChanged("CurrentFishPopulation");
            }
        }


        /// <summary>
        /// Gets or sets the number of rounds.
        /// </summary>
        public int Round
        {
            get { return round; }
            set
            {
                round = value;
                OnPropertyChanged("Round");
            }
        }

        public MainViewModel()
        {
            this.watorSettings.FishBreedTime = 20;
            this.watorSettings.SharkBreedTime = 30;
            this.watorSettings.InitialFishPopulation = 100;
            this.watorSettings.InitialSharkPopulation = 80;
            this.watorSettings.SharkStarveTime = 50;
            this.watorSettings.WorldWidth = this.watorSettings.WorldWidth = 500;
            this.simulation = new WatorSimulation(this.watorSettings);
        }

        protected void OnPropertyChanged(string name)
        {
            var safe = this.PropertyChanged;
            if (safe != null)
            {
                safe(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
