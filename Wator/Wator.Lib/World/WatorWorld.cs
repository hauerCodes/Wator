using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Wator.Lib.Animals;

namespace Wator.Lib.World
{
    public class WatorWorld
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="WatorWorld"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public WatorWorld(WatorSettings settings)
        {
            this.Settings = settings;
        }

        public WatorField[,] World { get; private set; }

        public WatorSettings Settings { get; private set; }
    }
}
