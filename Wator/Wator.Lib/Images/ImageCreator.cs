// -----------------------------------------------------------------------
// <copyright file="ImageCreator.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - ImageCreator.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Images
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Wator.Lib.World;

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ImageCreator<T> : IDisposable
        where T : IDrawable
    {
        /// <summary>
        /// The creator thread
        /// </summary>
        private Thread creatorThread;

        /// <summary>
        /// The image height
        /// </summary>
        private int height;

        /// <summary>
        /// The image extension
        /// </summary>
        private string imageExtension = "bmp";

        /// <summary>
        /// The image save path
        /// </summary>
        private string imageSavePath;

        /// <summary>
        /// The job queue
        /// </summary>
        private ConcurrentQueue<ImageJob<T>> jobQueue;

        /// <summary>
        /// The settings
        /// </summary>
        private IWatorSettings settings;

        /// <summary>
        /// The image width
        /// </summary>
        private int width;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCreator{T}"/> class. 
        /// Initializes a new instance of the <see cref="ImageCreator"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public ImageCreator(IWatorSettings settings)
        {
            this.settings = settings;
            this.width = settings.WorldWidth;
            this.height = settings.WorldHeight;
            this.imageSavePath = settings.SaveFolder;

            this.InitializeObjects();
            this.InitializeThread();
        }

        /// <summary>
        /// Occurs when a job is finished.
        /// </summary>
        public event EventHandler<ImageJob<T>> JobFinished;

        /// <summary>
        /// The is thread running
        /// </summary>
        public bool IsCreatorRunning { get; private set; }

        /// <summary>
        /// Adds the job.
        /// </summary>
        /// <param name="job">
        /// The job.
        /// </param>
        public void AddJob(ImageJob<T> job)
        {
            this.jobQueue.Enqueue(job);
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            this.creatorThread.Abort();
        }

        /// <summary>
        /// Starts the creator.
        /// </summary>
        public void StartCreator()
        {
            this.IsCreatorRunning = true;
            this.creatorThread.Start();
        }

        /// <summary>
        /// Stops the creator.
        /// </summary>
        public void StopCreator()
        {
            this.IsCreatorRunning = false;

            try
            {
                // cancel simulation - threadabortexcep
                this.creatorThread.Abort();

                // wait for thread exit
                this.creatorThread.Join();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Called when a job is finished.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnJobFinished(ImageJob<T> e)
        {
            var handler = this.JobFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Generates a bitmap for the current state of the wator world
        /// </summary>
        /// <param name="imageData">
        /// The image data.
        /// </param>
        /// <returns>
        /// The <see cref="Bitmap"/>.
        /// </returns>
        private Bitmap GenerateImage(int[,] imageData)
        {
            Rectangle rect = new Rectangle(0, 0, this.width, this.height);
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);
            Color color = this.settings.WaterColor;
            BitmapData bmpData = null;

            try
            {
                // Lock the bitmap's bits.    
                bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

                // byte[] rgbValues = new byte[width * height * 3];
                byte[] rgbValues = new byte[bmpData.Stride * this.height];

                int counter = 0;

                for (int y = 0; y < this.height; y++)
                {
                    for (int x = 0; x < this.width; x++)
                    {
                        int fieldCol = imageData[x, y];

                        if (fieldCol == 0)
                        {
                            color = this.settings.WaterColor;
                        }
                        else if (fieldCol == 1)
                        {
                            color = this.settings.FishColor;
                        }
                        else if (fieldCol == -1)
                        {
                            color = this.settings.SharkColor;
                        }

                        rgbValues[counter++] = color.B;
                        rgbValues[counter++] = color.G;
                        rgbValues[counter++] = color.R;
                        rgbValues[counter++] = color.A;
                    }
                }

                // Get the address of the first line.                
                IntPtr ptr = bmpData.Scan0;

                // Copy the RGB values back to the bitmap             
                Marshal.Copy(rgbValues, 0, ptr, rgbValues.Length);
            }
            finally
            {
                // Unlock the bits
                if (bmpData != null)
                {
                    bitmap.UnlockBits(bmpData);
                }
            }

            return bitmap.Clone(rect, PixelFormat.Format32bppArgb);
        }

        /// <summary>
        /// Handles the image job.
        /// </summary>
        /// <param name="currentJob">
        /// The current job.
        /// </param>
        private void HandleImageJob(ImageJob<T> currentJob)
        {
            // load data from stream
            int[,] imageData = currentJob.Data;

            try
            {
                // generate image from data
                Bitmap image = this.GenerateImage(imageData);

                var path = Path.Combine(
                    this.imageSavePath, 
                    string.Format("{0}.{1}", currentJob.Round, this.imageExtension));

                image.Save(path, ImageFormat.Bmp);

                currentJob.File = path;
                currentJob.IsFinished = true;
                this.OnJobFinished(currentJob);
            }
            catch (Exception ex)
            {
                currentJob.IsFinished = false;
                Debug.WriteLine("Error - image Creation for Round {0} failed - {1}", currentJob.Round, ex.Message);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void InitializeObjects()
        {
            this.jobQueue = new ConcurrentQueue<ImageJob<T>>();
        }

        /// <summary>
        /// Initializes the thread.
        /// </summary>
        private void InitializeThread()
        {
            this.IsCreatorRunning = false;

            // initialize thread
            this.creatorThread = new Thread(this.RunCreator);

            // gets killed when foreground thread ends
            this.creatorThread.IsBackground = true;
            this.creatorThread.Priority = ThreadPriority.Highest;
        }

        /// <summary>
        /// Runs the creator.
        /// </summary>
        private void RunCreator()
        {
            while (this.IsCreatorRunning)
            {
                try
                {
                    while (this.jobQueue.Count == 0)
                    {
                        Thread.Sleep(100);
                    }

                    ImageJob<T> currentJob = null;
                    this.jobQueue.TryDequeue(out currentJob);

                    this.HandleImageJob(currentJob);
                }
                catch (ThreadAbortException ex)
                {
                    this.IsCreatorRunning = false;
                }
            }
        }
    }
}