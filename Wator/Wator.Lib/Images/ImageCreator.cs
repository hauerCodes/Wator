using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Wator.Lib.World;

namespace Wator.Lib.Images
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImageCreator<T> : IDisposable where T : IDrawable
    {
        /// <summary>
        /// The creator thread
        /// </summary>
        private Thread creatorThread;

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
        /// The image height
        /// </summary>
        private int height;

        /// <summary>
        /// The image save path
        /// </summary>
        private string imageSavePath;

        /// <summary>
        /// The image extension
        /// </summary>
        private string imageExtension = "bmp";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCreator{T}" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public ImageCreator(IWatorSettings settings)
        {
            this.settings = settings;
            this.width = settings.WorldWidth;
            this.height = settings.WorldHeight;
            this.imageSavePath = settings.SaveFolder;

            InitializeObjects();
            InitializeThread();
        }

        /// <summary>
        /// The is thread running
        /// </summary>
        public bool IsCreatorRunning { get; private set; }

        /// <summary>
        /// Occurs when a job is finished.
        /// </summary>
        public event EventHandler<ImageJob<T>> JobFinished;

        /// <summary>
        /// Adds the job.
        /// </summary>
        /// <param name="job">The job.</param>
        public void AddJob(ImageJob<T> job)
        {
            jobQueue.Enqueue(job);
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

            // cancel simulation - threadabortexcep
            // this.creatorThread.Abort();
            // wait for thread exit
            // this.creatorThread.Join();
        }

        /// <summary>
        /// Runs the creator.
        /// </summary>
        private void RunCreator()
        {
            while (IsCreatorRunning)
            {
                try
                {
                    while (jobQueue.Count == 0)
                    {
                        Thread.Sleep(100);
                    }

                    ImageJob<T> currentJob = null;
                    jobQueue.TryDequeue(out currentJob);

                    HandleImageJob(currentJob);
                }
                catch (ThreadAbortException ex)
                {
                    IsCreatorRunning = false;
                }
            }
        }

        /// <summary>
        /// Handles the image job.
        /// </summary>
        /// <param name="currentJob">The current job.</param>
        private void HandleImageJob(ImageJob<T> currentJob)
        {
            //load data from stream
            T imageData = currentJob.LoadJobData();

            //generate image from data
            Bitmap image = GenerateImage(imageData);

            image.Save(Path.Combine(imageSavePath,
                    string.Format("{0}.{1}", currentJob.Round, imageExtension)), ImageFormat.Bmp);
        }

        /// <summary>
        /// Generates a bitmap for the current state of the wator world
        /// </summary>
        /// <param name="imageData">The image data.</param>
        /// <returns></returns>
        private Bitmap GenerateImage(T imageData)
        {
            byte[] rgbValues = new byte[width * height * 4];
            int counter = 0;
            IColorProvider[,] fields = imageData.GetDrawingElements();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color col = fields[x, y].DrawColor;

                    rgbValues[counter++] = col.A;
                    rgbValues[counter++] = col.R;
                    rgbValues[counter++] = col.G;
                    rgbValues[counter++] = col.B;
                }
            }

            Rectangle rect = new Rectangle(0, 0, width, height);
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bmpData = null;

            try
            {
                // Lock the bitmap's bits.    
                bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

                // Get the address of the first line.                
                IntPtr ptr = bmpData.Scan0;

                // Copy the RGB values back to the bitmap             
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, rgbValues.Length);
            }
            finally
            {
                // Unlock the bits
                if (bmpData != null)
                {
                    bitmap.UnlockBits(bmpData);
                }
            }

            return bitmap;
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

            //initialize thread
            this.creatorThread = new Thread(RunCreator);

            //gets killed when foreground thread ends
            creatorThread.IsBackground = true;
        }

        /// <summary>
        /// Called when a job is finished.
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnJobFinished(ImageJob<T> e)
        {
            var handler = this.JobFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            creatorThread.Abort();
        }

    }
}
