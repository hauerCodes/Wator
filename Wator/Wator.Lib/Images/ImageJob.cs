using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Wator.Lib.World;

namespace Wator.Lib.Images
{
    public class ImageJob<T> where T : IDrawable
    {
        /// <summary>
        /// The binary formatter
        /// </summary>
        private BinaryFormatter binaryFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageJob{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="round">The round.</param>
        public ImageJob(T data, int round)
        {
            this.Round = round;
            this.Initialize();

            SerializeDataToStream(data);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.binaryFormatter = new BinaryFormatter();
            this.DataStream = new MemoryStream();

            this.IsFinished = false;
            this.File = null;
        }

        /// <summary>
        /// Gets the data stream.
        /// </summary>
        /// <value>
        /// The data stream.
        /// </value>
        public Stream DataStream { get; private set; }

        /// <summary>
        /// Gets the round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public int Round { get; private set; }

        public Bitmap File { get; private set; }

        public bool IsFinished { get; private set; }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns></returns>
        public T LoadJobData()
        {
            try
            {
                return (T)binaryFormatter.Deserialize(DataStream);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return default(T);
        }

        /// <summary>
        /// Serializes the data to stream.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private void SerializeDataToStream(T data)
        {
            try
            {
                binaryFormatter.Serialize(DataStream, data);
                DataStream.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

      
    }
}
