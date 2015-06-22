// -----------------------------------------------------------------------
// <copyright file="WatorWorld.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - WatorWorld.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.World
{
    using System;
    using System.Drawing;

    using Wator.Lib.Animals;
    using Wator.Lib.Images;

    /// <summary>
    /// The wator world.
    /// </summary>
    public class WatorWorld : IDrawable
    {
        /// <summary>
        /// The random generator
        /// </summary>
        private Random randomGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatorWorld"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// settings
        /// </exception>
        public WatorWorld(IWatorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.Settings = settings;

            this.InitializeObjects();
            this.InitializeWorldFields();
            this.PopulateWorld();
        }

        /// <summary>
        /// Occurs when the moved stats have to be reseted.
        /// </summary>
        private event Action ResetMovedStats;

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public WatorField[,] Fields { get; private set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public IWatorSettings Settings { get; private set; }

        /// <summary>
        /// Finishes the steps of alle animals on all fields.
        /// </summary>
        /// <param name="useEvent">
        /// The use Event.
        /// </param>
        public void FinishSteps(bool useEvent = true)
        {
            if (useEvent)
            {
                if (this.ResetMovedStats != null)
                {
                    this.ResetMovedStats();
                }
            }
            else
            {
                int width = this.Settings.WorldWidth;
                int height = this.Settings.WorldHeight;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        this.Fields[y, x].FinishStep();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the drawing elements.
        /// </summary>
        /// <returns>
        /// The <see cref="sbyte[,]"/>.
        /// </returns>
        public sbyte[,] GetDrawingElements()
        {
            int width = this.Settings.WorldWidth;
            int height = this.Settings.WorldHeight;
            sbyte[,] data = new sbyte[height, width];
            sbyte col = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // col = Settings.WaterColor;
                    if (this.Fields[y, x].Animal != null)
                    {
                        if (this.Fields[y, x].Animal is Fish)
                        {
                            // col = Settings.FishColor;
                            col = 1;
                        }
                        else
                        {
                            // col = Settings.SharkColor;
                            col = -1;
                        }

                        data[y, x] = col;
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Initializes the objects.
        /// </summary>
        private void InitializeObjects()
        {
            this.randomGenerator = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
        }

        /// <summary>
        /// Initializes the world.
        /// </summary>
        /// <param name="createEvent">
        /// The create Event.
        /// </param>
        private void InitializeWorldFields(bool createEvent = false)
        {
            int width = this.Settings.WorldWidth;
            int height = this.Settings.WorldHeight;

            // create world 2D Array
            this.Fields = new WatorField[height, width];

            // Set Fields Fields
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    this.Fields[y, x] = new WatorField(new Point(x, y), this.Settings);

                    if (createEvent)
                    {
                        this.ResetMovedStats += this.Fields[y, x].FinishStep;
                    }
                }
            }

            // set world field neighbors
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // operations on Torus world
                    int x1 = (x - 1 + width) % width; // x-1 left
                    int x2 = (x + 1) % width; // x+1 right
                    int y1 = (y - 1 + height) % height; // y-1 up
                    int y2 = (y + 1) % height; // y+1 down

                    this.Fields[y, x].NeighbourFieldLeft = this.Fields[y, x1];
                    this.Fields[y, x].NeighbourFieldRight = this.Fields[y, x2];
                    this.Fields[y, x].NeighbourFieldUp = this.Fields[y1, x];
                    this.Fields[y, x].NeighbourFieldDown = this.Fields[y2, x];
                }
            }
        }

        /// <summary>
        /// Populates the world.
        /// </summary>
        private void PopulateWorld()
        {
            int fishPopulation = this.Settings.InitialFishPopulation;
            int sharkPopulation = this.Settings.InitialSharkPopulation;

            // ReSharper disable once RedundantAssignment
            Point randomPoint = Point.Empty;

            while (fishPopulation + sharkPopulation > 0)
            {
                if (fishPopulation > 0)
                {
                    randomPoint = this.GetRandomFreeField(this.Settings.WorldWidth, this.Settings.WorldHeight);
                    var field = this.Fields[randomPoint.Y, randomPoint.X];
                    field.Animal = new Fish(this.Settings, field);
                    fishPopulation--;
                }

                if (sharkPopulation > 0)
                {
                    randomPoint = this.GetRandomFreeField(this.Settings.WorldWidth, this.Settings.WorldHeight);
                    var field = this.Fields[randomPoint.Y, randomPoint.X];
                    field.Animal = new Shark(this.Settings, field);
                    sharkPopulation--;
                }
            }
        }

        /// <summary>
        /// Gets the random free field.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        private Point GetRandomFreeField(int width, int height)
        {
            int randomX = 0;
            int randomY = 0;

            do
            {
                randomX = this.randomGenerator.Next(0, width);
                randomY = this.randomGenerator.Next(0, height);
            }
            while (this.Fields[randomY, randomX].Animal != null);

            // if null free field found 
            return new Point(randomX, randomY);
        }
    }
}