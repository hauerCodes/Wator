// -----------------------------------------------------------------------
// <copyright file="IWatorSettings.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - IWatorSettings.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.World
{
    using System.Drawing;

    /// <summary>
    /// The WatorSettings interface.
    /// </summary>
    public interface IWatorSettings
    {
        /// <summary>
        /// Gets or sets the fish breed time.
        /// </summary>
        int FishBreedTime { get; set; }

        /// <summary>
        /// Gets or sets the fish breed time.
        /// </summary>
        string  ImageExtension{ get; set; }

        /// <summary>
        /// Gets or sets the fish color.
        /// </summary>
        Color FishColor { get; set; }

        /// <summary>
        /// Gets or sets the initial fish population.
        /// </summary>
        int InitialFishPopulation { get; set; }

        /// <summary>
        /// Gets or sets the initial shark population.
        /// </summary>
        int InitialSharkPopulation { get; set; }

        /// <summary>
        /// Gets or sets the save folder.
        /// </summary>
        string SaveFolder { get; set; }

        /// <summary>
        /// Gets or sets the shark breed time.
        /// </summary>
        int SharkBreedTime { get; set; }

        /// <summary>
        /// Gets or sets the shark color.
        /// </summary>
        Color SharkColor { get; set; }

        /// <summary>
        /// Gets or sets the shark starve time.
        /// </summary>
        int SharkStarveTime { get; set; }

        /// <summary>
        /// Gets or sets the water color.
        /// </summary>
        Color WaterColor { get; set; }

        /// <summary>
        /// Gets or sets the world height.
        /// </summary>
        int WorldHeight { get; set; }

        /// <summary>
        /// Gets or sets the world width.
        /// </summary>
        int WorldWidth { get; set; }
    }
}