// -----------------------------------------------------------------------
// <copyright file="Animal.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - Animal.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Animals
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Threading;

    using Wator.Lib.World;

    /// <summary>
    /// The animal.
    /// </summary>
    public abstract class Animal
    {
        /// <summary>
        /// The animal randomizer
        /// </summary>
        protected Random AnimalRandomizer;

        /// <summary>
        /// The found fields
        /// </summary>
        protected List<Direction> FoundDirections;

        /// <summary>
        /// The settings
        /// </summary>
        protected IWatorSettings Settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animal"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="field">
        /// The field.
        /// </param>
        protected Animal(IWatorSettings settings, WatorField field)
        {
            this.Settings = settings;
            this.IsMoved = false;
            this.Lifetime = 0;
            this.Field = field;
            this.FoundDirections = new List<Direction>();
            this.AnimalRandomizer = new Random(DateTime.Now.Millisecond * field.Position.X);
        }

        /// <summary>
        /// Gets the breed time.
        /// </summary>
        /// <value>
        /// The breed time.
        /// </value>
        [IgnoreDataMember]
        public abstract int BreedTime { get; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public WatorField Field { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is moved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is moved; otherwise, <c>false</c>.
        /// </value>
        public bool IsMoved { get; protected set; }

        /// <summary>
        /// Gets or sets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public int Lifetime { get; protected set; }

        /// <summary>
        /// This animal dies.
        /// Clear field in animal.
        /// Remove animal from field.
        /// </summary>
        public void Die()
        {
            this.Field.Animal = null;
            this.Field = null;
        }

        /// <summary>
        /// Finishes the step.
        /// </summary>
        public void FinishStep()
        {
            this.IsMoved = false;
        }

        ///// <summary>
        ///// Gets the color of the draw.
        ///// </summary>
        ///// <value>
        ///// The color of the draw.
        ///// </value>
        // public abstract Color DrawColor { get; }

        /// <summary>
        /// Steps this instance.
        /// </summary>
        public abstract void Step();

        /// <summary>
        /// Breed and move step.
        /// </summary>
        /// <returns>true if sibling otherwise false</returns>
        protected bool BreedMoveStep()
        {
            bool lockTaken = false;
            var freeDirection = this.GetFreeRandomDirection();

            if (this.Lifetime > this.BreedTime)
            {
                // create sibiling
                var siblingField = this.GetFieldFromDirection(freeDirection);

                // if free field found
                if (siblingField != null)
                {
                    if (this.CheckLockRequired(freeDirection, siblingField))
                    {
                        lock (siblingField)
                        {
                            siblingField.Animal = this.CreateSibling(siblingField);
                        }
                    }
                    else
                    {
                        siblingField.Animal = this.CreateSibling(siblingField);
                    }

                    return true;
                }
            }
            else
            {
                // move
                var moveField = this.GetFieldFromDirection(freeDirection);

                if (moveField != null)
                {
                    try
                    {
                        if (this.CheckLockRequired(freeDirection, moveField))
                        {
                            Monitor.Enter(moveField, ref lockTaken);
                        }

                        // clear old animal space
                        this.Field.Animal = null;

                        // set move field for this animal as new place
                        this.Field = moveField;

                        // change fields animal to this animal
                        moveField.Animal = this;
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(moveField);
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if lock required.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="targetField">
        /// The target field.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool CheckLockRequired(Direction direction, WatorField targetField)
        {
            if (direction == Direction.Up && this.Field.Position.Y == 0)
            {
                return targetField.Position.Y > this.Field.Position.Y;
            }

            if (direction == Direction.Down && this.Field.Position.Y == this.Settings.WorldHeight - 1)
            {
                return targetField.Position.Y < this.Field.Position.Y;
            }

            return false;
        }

        /// <summary>
        /// Creates the sibling depending on inherited type.
        /// </summary>
        /// <param name="siblingField">
        /// The sibling field.
        /// </param>
        /// <returns>
        /// The <see cref="Animal"/>.
        /// </returns>
        protected abstract Animal CreateSibling(WatorField siblingField);

        /// <summary>
        /// Gets the field from direction.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="WatorField"/>.
        /// </returns>
        protected WatorField GetFieldFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return this.Field.NeighbourFieldUp;
                case Direction.Down:
                    return this.Field.NeighbourFieldDown;
                case Direction.Left:
                    return this.Field.NeighbourFieldLeft;
                case Direction.Right:
                    return this.Field.NeighbourFieldRight;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets a free random field around the animal.
        /// </summary>
        /// <returns>
        /// The <see cref="Direction"/>.
        /// </returns>
        protected Direction GetFreeRandomDirection()
        {
            this.FoundDirections.Clear();

            if (this.Field.NeighbourFieldDown.Animal == null)
            {
                this.FoundDirections.Add(Direction.Down);
            }

            if (this.Field.NeighbourFieldUp.Animal == null)
            {
                this.FoundDirections.Add(Direction.Up);
            }

            if (this.Field.NeighbourFieldLeft.Animal == null)
            {
                this.FoundDirections.Add(Direction.Left);
            }

            if (this.Field.NeighbourFieldRight.Animal == null)
            {
                this.FoundDirections.Add(Direction.Right);
            }

            if (this.FoundDirections.Count == 0)
            {
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