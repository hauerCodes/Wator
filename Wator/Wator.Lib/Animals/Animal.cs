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
        protected IWatorSettings settings;

        protected Animal(IWatorSettings settings)
        {
            this.settings = settings;
            this.IsMoved = false;
            this.Lifetime = 0;
        }

        public WatorField Field { get; protected set; }

        public bool IsMoved { get; private set; }

        public int Lifetime { get; private set; }

        public abstract void Step();

        public abstract void Finish();

        public abstract Color DrawColor { get; }

        public abstract void Ageing();


    }
}
