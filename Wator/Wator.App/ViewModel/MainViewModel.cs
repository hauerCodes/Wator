using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wator.Lib.Simulation;
using Wator.Lib.World;

namespace Wator.App.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private int currentFishPopulation;
        private int currentSharkPopulation;
        private WatorSimulation simulation;
        private WatorSettings watorSettings;
        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand PlayCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ForwardCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WatorSettings WatorSettings
        {
            get { return watorSettings; }
            set
            {
                watorSettings = value;
                OnPropertyChanged("WatorSettings");

            }
        }

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
            get { return this.simulation.Round; }
            set
            {
                OnPropertyChanged("Round");
            }
        }

        public MainViewModel()
        {
            StartCommand = new RelayCommand(StartSimulation);
            StopCommand = new RelayCommand(StopSimulation);
            ForwardCommand = new RelayCommand(Forward);
            BackCommand = new RelayCommand(Back);
            ResetCommand = new RelayCommand(Reset);
            PlayCommand = new RelayCommand(Play);

            this.watorSettings = new WatorSettings();
            this.watorSettings.FishBreedTime = 20;
            this.watorSettings.SharkBreedTime = 30;
            this.watorSettings.InitialFishPopulation = 100;
            this.watorSettings.InitialSharkPopulation = 80;
            this.watorSettings.SharkStarveTime = 50;
            this.watorSettings.WorldWidth = this.watorSettings.WorldHeight = 500;

            this.simulation = new WatorSimulation(this.WatorSettings);
            
        }

        private void Forward()
        {
        }

        private void Back()
        {
        }

        private void Reset()
        {

        }

        private void Play()
        {
            
        }

        private void StartSimulation()
        {
            this.simulation.StartSimulation();
        }

        private void StopSimulation()
        {
            this.simulation.StopSimulation();
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
