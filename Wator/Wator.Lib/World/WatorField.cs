using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Wator.Lib.Animals;
using Wator.Lib.Images;

namespace Wator.Lib.World
{
    public class WatorField
    {
        /// <summary>
        /// The settings
        /// </summary>
        private IWatorSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatorField"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="settings">The settings.</param>
        public WatorField(Point position, IWatorSettings settings)
        {
            this.Position = position;
            this.settings = settings;
        }

        /// <summary>
        /// Gets or sets the neighbour field up.
        /// </summary>
        /// <value>
        /// The neighbour field up.
        /// </value>

        public WatorField NeighbourFieldUp { get; set; }

        /// <summary>
        /// Gets or sets the neighbour field down.
        /// </summary>
        /// <value>
        /// The neighbour field down.
        /// </value>

        public WatorField NeighbourFieldDown { get; set; }

        /// <summary>
        /// Gets or sets the neighbour field left.
        /// </summary>
        /// <value>
        /// The neighbour field left.
        /// </value>

        public WatorField NeighbourFieldLeft { get; set; }

        /// <summary>
        /// Gets or sets the neighbour field right.
        /// </summary>
        /// <value>
        /// The neighbour field right.
        /// </value>

        public WatorField NeighbourFieldRight { get; set; }

        /// <summary>
        /// Gets or sets the animal.
        /// </summary>
        /// <value>
        /// The animal.
        /// </value>
        public Animal Animal { get; set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Point Position { get; private set; }

        ///// <summary>
        ///// Gets the color of the draw.
        ///// </summary>
        ///// <value>
        ///// The color of the draw.
        ///// </value>
        //public Color DrawColor
        //{
        //    get
        //    {
        //        if (Animal != null)
        //        {
        //            return Animal.DrawColor;
        //        }

        //        return settings.WaterColor;
        //    }
        //}

        /// <summary>
        /// Finishes the step of the animal - if there is one on this field.
        /// </summary>
        public void FinishStep()
        {
            if (this.Animal != null)
            {
                this.Animal.FinishStep();
            }
        }
    }
}
