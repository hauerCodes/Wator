using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Wator.Lib.World;

namespace Wator.Lib.Animals
{
    public abstract class Animal : Wator.Lib.IDrawable
    {

        public bool IsMoved
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Lifetime
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public abstract void Step();

        public abstract void Finish();

        public int DrawColor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public WatorField Field
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public abstract void Ageing();
    }
}
