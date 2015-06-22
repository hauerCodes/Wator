// -----------------------------------------------------------------------
// <copyright file="Fish.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - Fish.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Animals
{
    using Wator.Lib.Simulation;
    using Wator.Lib.World;

    /// <summary>
    /// The fish.
    /// </summary>
    public class Fish : Animal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fish"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="field">
        /// The field.
        /// </param>
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
                return this.Settings.FishBreedTime;
            }
        }

        ///// <summary>
        ///// Gets the color of the draw.
        ///// </summary>
        ///// <value>
        ///// The color of the draw.
        ///// </value>
        // public override Color DrawColor
        // {
        // get
        // {
        // return this.Settings.FishColor;
        // }
        // }

        /// <summary>
        /// Steps of fish.
        /// </summary>
        public override void Step()
        {
            // increase lifetime
            this.Lifetime++;

            if (this.BreedMoveStep())
            {
                WatorSimulation.ChangeFishPopulation(true);
            }

            // set step down - animal moved
            this.IsMoved = true;
        }

        /// <summary>
        /// Creates the sibling depending on inherited type.
        /// </summary>
        /// <param name="siblingField">
        /// The sibling Field.
        /// </param>
        /// <returns>
        /// The <see cref="Animal"/>.
        /// </returns>
        protected override Animal CreateSibling(WatorField siblingField)
        {
            return new Fish(this.Settings, siblingField);
        }
    }
}