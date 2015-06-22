using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Wator.Lib.Animals;
using Wator.Lib.Images;

namespace Wator.Lib.World
{
    public class WatorWorld : IDrawable
    {
        /// <summary>
        /// The random generator
        /// </summary>
        private Random randomGenerator;

        /// <summary>
        /// Occurs when the moved stats have to be reseted.
        /// </summary>
        private event Action ResetMovedStats;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatorWorld" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="System.ArgumentNullException">settings</exception>
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
        /// Gets the drawing elements.
        /// </summary>
        /// <returns></returns>
        public int[,] GetDrawingElements()
        {
            int width = Settings.WorldWidth;
            int height = Settings.WorldHeight;
            int[,] data = new int[height, width];
            int col = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //col = Settings.WaterColor;

                    if (Fields[y, x].Animal != null)
                    {
                        if (Fields[y, x].Animal is Fish)
                        {
                            //col = Settings.FishColor;
                            col = 1;
                        }
                        else
                        {
                            //col = Settings.SharkColor;
                            col = -1;
                        }

                        data[y, x] = col;
                    }

                }
            }
            return data;
        }

        /// <summary>
        /// Finishes the steps of alle animals on all fields.
        /// </summary>
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
                int width = Settings.WorldWidth;
                int height = Settings.WorldHeight;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Fields[y, x].FinishStep();
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the objects.
        /// </summary>
        private void InitializeObjects()
        {
            randomGenerator = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
        }

        /// <summary>
        /// Initializes the world.
        /// </summary>
        private void InitializeWorldFields(bool createEvent = false)
        {
            int width = Settings.WorldWidth;
            int height = Settings.WorldHeight;

            //create world 2D Array
            this.Fields = new WatorField[height, width];

            //Set Fields Fields
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    this.Fields[y, x] = new WatorField(new Point(x, y), Settings);

                    if (createEvent)
                    {
                        this.ResetMovedStats += this.Fields[y, x].FinishStep;
                    }
                }
            }

            //set world field neighbors
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
            int fishPopulation = Settings.InitialFishPopulation;
            int sharkPopulation = Settings.InitialSharkPopulation;
            // ReSharper disable once RedundantAssignment
            Point randomPoint = Point.Empty;

            while (fishPopulation + sharkPopulation > 0)
            {
                if (fishPopulation > 0)
                {
                    randomPoint = GetRandomFreeField(Settings.WorldWidth, Settings.WorldHeight);
                    var field = this.Fields[randomPoint.Y, randomPoint.X];
                    field.Animal = new Fish(Settings, field);
                    fishPopulation--;
                }

                if (sharkPopulation > 0)
                {
                    randomPoint = GetRandomFreeField(Settings.WorldWidth, Settings.WorldHeight);
                    var field = this.Fields[randomPoint.Y, randomPoint.X];
                    field.Animal = new Shark(Settings, field);
                    sharkPopulation--;
                }
            }
        }

        /// <summary>
        /// Gets the random free field.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        private Point GetRandomFreeField(int width, int height)
        {
            int randomX = 0;
            int randomY = 0;

            do
            {
                randomX = randomGenerator.Next(0, width);
                randomY = randomGenerator.Next(0, height);
            }
            while (this.Fields[randomY, randomX].Animal != null);

            //if null free field found 
            return new Point(randomX, randomY);
        }

    }
}
