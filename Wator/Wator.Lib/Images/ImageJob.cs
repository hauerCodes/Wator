using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using Wator.Lib.World;

namespace Wator.Lib.Images
{
    public class ImageJob<T> where T : IDrawable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageJob{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="round">The round.</param>
        public ImageJob(T data, int round)
        {
            this.Data = data.GetDrawingElements();
            this.Round = round;

            this.Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.IsFinished = false;
            this.File = null;
        }

        /// <summary>
        /// Gets the round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public int Round { get; private set; }

        /// <summary>
        /// Gets the round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public Color[,] Data { get; private set; }

        public string File { get; set; }

        public bool IsFinished { get; set; }

    }
}