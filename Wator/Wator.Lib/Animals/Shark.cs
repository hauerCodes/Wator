// -----------------------------------------------------------------------
// <copyright file="Shark.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - Shark.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Animals
{
    using System.Threading;

    using Wator.Lib.Simulation;
    using Wator.Lib.World;

    /// <summary>
    /// The shark.
    /// </summary>
    public class Shark : Animal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Shark"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="field">
        /// The field.
        /// </param>
        public Shark(IWatorSettings settings, WatorField field)
            : base(settings, field)
        {
            this.Starve = 0; // "Hunger" 
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
                return this.Settings.SharkBreedTime;
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
        // return this.Settings.SharkColor;
        // }
        // }

        /// <summary>
        /// Gets the starve.
        /// </summary>
        /// <value>
        /// The starve.
        /// </value>
        public int Starve { get; private set; }

        /// <summary>
        /// Steps of shark.
        /// </summary>
        public override void Step()
        {
            bool lockTaken = false;

            // increase lifetime
            this.Lifetime++;

            // find fish around
            var preyFieldDirection = this.GetRandomFishDirectionAround();
            var preyField = this.GetFieldFromDirection(preyFieldDirection);

            // shark eats a fish if found
            if (preyField != null)
            {
                try
                {
                    if (this.CheckLockRequired(preyFieldDirection, preyField))
                    {
                        Monitor.Enter(preyField, ref lockTaken);
                    }

                    // check again fish is on field - could be changed in meantime
                    if (preyField.Animal != null)
                    {
                        // clear own old animal space
                        this.Field.Animal = null;

                        // fish dies (clear animal.field and field.animal)
                        preyField.Animal.Die();
                        WatorSimulation.ChangeFishPopulation(false);

                        // fish is dead place shark on field
                        preyField.Animal = this;

                        // set fíeld as new place for shark
                        this.Field = preyField;
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(preyField);
                    }
                }
            }
            else
            {
                // if no fish found - increase starve 
                this.Starve++;

                if (this.Starve > this.Settings.SharkStarveTime)
                {
                    // shark dies (animal.field null and field.animal.null)
                    this.Die();
                    WatorSimulation.ChangeSharkPopulation(false);

                    return;
                }
            }

            // execute breed and move
            if (this.BreedMoveStep())
            {
                WatorSimulation.ChangeSharkPopulation(true);
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
            return new Shark(this.Settings, siblingField);
        }

        /// <summary>
        /// Find a random fish around.
        /// </summary>
        /// <returns>
        /// The <see cref="Direction"/>.
        /// </returns>
        protected Direction GetRandomFishDirectionAround()
        {
            this.FoundDirections.Clear();

            if (this.Field.NeighbourFieldDown.Animal is Fish)
            {
                this.FoundDirections.Add(Direction.Down);
            }

            if (this.Field.NeighbourFieldUp.Animal is Fish)
            {
                this.FoundDirections.Add(Direction.Up);
            }

            if (this.Field.NeighbourFieldLeft.Animal is Fish)
            {
                this.FoundDirections.Add(Direction.Left);
            }

            if (this.Field.NeighbourFieldRight.Animal is Fish)
            {
                this.FoundDirections.Add(Direction.Right);
            }

            if (this.FoundDirections.Count == 0)
            {
                // no field with fish found
                return Direction.None;
            }

            if (this.FoundDirections.Count == 1)
            {
                // only one field found
                return this.FoundDirections[0];
            }

            return this.FoundDirections[this.AnimalRandomizer.Next(0, this.FoundDirections.Count)];
        }
    }
}