using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Wator.Lib.Animals;
using Wator.Lib.Images;

namespace Wator.Lib.World
{
    public class WatorField : IColorProvider
    {

        private IWatorSettings settings;

        public WatorField(Point position, IWatorSettings settings)
        {
            this.Position = position;
            this.settings = settings;
        }

        public WatorField NeighbourFieldUp { get; set; }

        public WatorField NeighbourFieldDown { get; set; }

        public WatorField NeighbourFieldLeft { get; set; }

        public WatorField NeighbourFieldRight { get; set; }

        public Animal Animal { get; set; }

        public Point Position { get; private set; }

        public Color DrawColor
        {
            get
            {
                return settings.WaterColor;
            }
        }
    }
}
