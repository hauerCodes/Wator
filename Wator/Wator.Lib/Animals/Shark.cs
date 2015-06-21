using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Wator.Lib.World;

namespace Wator.Lib.Animals
{
    public class Shark : Animal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Shark" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="field">The field.</param>
        public Shark(IWatorSettings settings, WatorField field)
            : base(settings, field)
        {
            this.Starve = 0; // "Hunger" 
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
                return this.Settings.SharkColor;
            }
        }

        /// <summary>
        /// Gets the starve.
        /// </summary>
        /// <value>
        /// The starve.
        /// </value>
        public int Starve { get; private set; }

        public override void Step()
        {
            throw new NotImplementedException();
        }

        public override void Finish()
        {
            throw new NotImplementedException();
        }

        public override void Ageing()
        {
            throw new NotImplementedException();
        }


    }
}
