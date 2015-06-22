// -----------------------------------------------------------------------
// <copyright file="SimulationState.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - SimulationState.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Simulation
{
    using System;

    /// <summary>
    /// The simulation state.
    /// </summary>
    public class SimulationState
    {
        /// <summary>
        /// Gets or sets the fish population.
        /// </summary>
        public int FishPopulation { get; set; }

        /// <summary>
        /// Gets or sets the round.
        /// </summary>
        public int Round { get; set; }

        /// <summary>
        /// Gets or sets the shark population.
        /// </summary>
        public int SharkPopulation { get; set; }

        /// <summary>
        /// Gets or sets the step time.
        /// </summary>
        public TimeSpan StepTime { get; set; }
    }
}