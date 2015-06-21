using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Wator.Lib.Images;
using Wator.Lib.World;

namespace Wator.Lib.Animals
{
    public abstract class Animal : IColorProvider
    {
        /// <summary>
        /// The settings
        /// </summary>
        protected IWatorSettings Settings;

        protected Animal(IWatorSettings settings, WatorField field)
        {
            this.Settings = settings;
            this.IsMoved = false;
            this.Lifetime = 0;
            this.Field = field;
        }

        public WatorField Field { get; protected set; }

        public bool IsMoved { get; protected set; }

        public int Lifetime { get; protected set; }

        public abstract void Step();

        public abstract void FinishStep();

        public abstract Color DrawColor { get; }

        public abstract void Ageing();


    }
}
