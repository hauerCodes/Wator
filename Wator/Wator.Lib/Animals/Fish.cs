using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

using Wator.Lib.Simulation;
using Wator.Lib.World;

namespace Wator.Lib.Animals
{
    public class Fish : Animal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fish" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="field">The field.</param>
        public Fish(IWatorSettings settings, WatorField field)
            : base(settings, field)
        {

        }

        /// <summary>
        /// Gets the breed time.
        /// </summary>
        /// <value>
        /// The breed time.
        /// </value>
        public override int BreedTime
        {
            get
            {
                return Settings.FishBreedTime;
            }
        }

        /// <summary>
        /// Gets the color of the draw.
        /// </summary>
        /// <value>
        /// The color of the draw.
        /// </value>
        public override Color DrawColor
        {
            get
            {
                return this.Settings.FishColor;
            }
        }

        /// <summary>
        /// Steps of fish.
        /// </summary>
        public override void Step()
        {
            // increase lifetime
            this.Lifetime++;

            if (BreedMoveStep())
            {
                WatorSimulation.ChangeFishPopulation(true);
            }

            // set step down - animal moved
            this.IsMoved = true;
        }

        /// <summary>
        /// Creates the sibling depending on inherited type.
        /// </summary>
        /// <returns></returns>
        protected override Animal CreateSibling(WatorField siblingField)
        {
            return new Fish(this.Settings, siblingField);
        }
    }
}
