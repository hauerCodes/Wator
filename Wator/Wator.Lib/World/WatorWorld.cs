using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Wator.Lib.Animals;
using Wator.Lib.Images;

namespace Wator.Lib.World
{
    public class WatorWorld : Wator.Lib.Image.IDrawable
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

        public Wator.Lib.World.IWatorSettings Settings { get; private set; }

        public ImageJob ToImageJob()
        {
            throw new System.NotImplementedException();
        }

        public Image.IColorProvider[,] GetDrawingElements()
        {
            throw new NotImplementedException();
        }

        private void InitializeFields()
        {
            throw new System.NotImplementedException();
        }
    }
}
