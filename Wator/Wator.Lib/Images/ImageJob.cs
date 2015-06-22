// -----------------------------------------------------------------------
// <copyright file="ImageJob.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - ImageJob.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Images
{
    /// <summary>
    /// The image job.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ImageJob<T>
        where T : IDrawable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageJob{T}"/> class.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="round">
        /// The round.
        /// </param>
        public ImageJob(T data, int round)
        {
            this.Data = data.GetDrawingElements();
            this.Round = round;

            this.Initialize();
        }

        /// <summary>
        /// Gets the round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public sbyte[,] Data { get; private set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is finished.
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// Gets the round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public int Round { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.IsFinished = false;
            this.File = null;
        }
    }
}