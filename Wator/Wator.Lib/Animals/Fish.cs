using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

        public override void Step()
        {
            IsMoved = true;

        }

        public override void FinishStep()
        {
            throw new NotImplementedException();
        }

        public override void Ageing()
        {
            throw new NotImplementedException();
        }

    }
}
