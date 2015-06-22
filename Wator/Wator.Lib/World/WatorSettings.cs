using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wator.Lib.World
{
    /// <summary>
    /// The wator settings.
    /// </summary>
    public class WatorSettings : IWatorSettings
    {
        public WatorSettings()
        {
            FishColor = Color.YellowGreen;
            SharkColor = Color.DarkCyan;
            WaterColor = Color.Black;
        }

        /// <summary>
        /// Gets or sets the fish breed time.
        /// </summary>
        public int FishBreedTime { get; set; }

        /// <summary>
        /// Gets or sets the fish color.
        /// </summary>
        public Color FishColor { get; set; }

        /// <summary>
        /// Gets or sets the initial fish population.
        /// </summary>
        public int InitialFishPopulation { get; set; }

        /// <summary>
        /// Gets or sets the initial shark population.
        /// </summary>
        public int InitialSharkPopulation { get; set; }

        /// <summary>
        /// Gets or sets the save folder.
        /// </summary>
        public string SaveFolder { get; set; }

        /// <summary>
        /// Gets or sets the shark breed time.
        /// </summary>
        public int SharkBreedTime { get; set; }

        /// <summary>
        /// Gets or sets the shark color.
        /// </summary>
        public Color SharkColor { get; set; }

        /// <summary>
        /// Gets or sets the shark starve time.
        /// </summary>
        public int SharkStarveTime { get; set; }

        /// <summary>
        /// Gets or sets the water color.
        /// </summary>
        public Color WaterColor { get; set; }

        /// <summary>
        /// Gets or sets the world height.
        /// </summary>
        public int WorldHeight { get; set; }

        /// <summary>
        /// Gets or sets the world width.
        /// </summary>
        public int WorldWidth { get; set; }
    }
}