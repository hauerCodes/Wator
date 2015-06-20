using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wator.Lib.Images
{
    public class ImageCreator
    {
        private Thread creatorThread;
        private Queryable<ImageJob> jobQueue;

        public event System.EventHandler<ImageJob> JobFinished;

        public void AddJob(ImageJob job)
        {
            throw new System.NotImplementedException();
        }
    }
}
