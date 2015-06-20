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
        public Shark(IWatorSettings settings)
            : base(settings)
        {
        }

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

        public override Color DrawColor
        {
            get
            {
                return settings.SharkColor;
            }
        }


    }
}
