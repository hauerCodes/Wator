// -----------------------------------------------------------------------
// <copyright file="WatorSettings.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - WatorSettings.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.World
{
    using System.Drawing;

    /// <summary>
    /// The wator settings.
    /// </summary>
    public class WatorSettings : IWatorSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatorSettings"/> class.
        /// </summary>
        public WatorSettings()
        {
            this.FishColor = Color.YellowGreen;
            this.SharkColor = Color.DarkCyan;
            this.WaterColor = Color.Black;
            this.ImageExtension = "bmp";
            this.ThreadFaktor = 3;
        }

        /// <summary>
        /// Gets or sets the fish breed time.
        /// </summary>
        public string ImageExtension { get; set; }

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
        /// Gets or sets the thread faktor.
        /// </summary>
        /// <value>
        /// The thread faktor.
        /// </value>
        public int ThreadFaktor { get; set; }

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