using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Wator.Lib.World;

namespace Wator.Lib.Images
{
    public class ImageCreator<T> : IDisposable where T : IDrawable
    {
        private Thread creatorThread;
        private Queue<ImageJob<T>> jobQueue;
        private bool isThreadRunning;

        public ImageCreator(IWatorSettings settings)
        {

        }

        public event EventHandler<ImageJob<T>> JobFinished;

        public void AddJob(ImageJob<T> job)
        {
            throw new System.NotImplementedException();
        }

        private void RunCreator()
        {
            throw new System.NotImplementedException();
        }

        private void HandleImageJob()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            creatorThread.Abort();
        }
    }
}
