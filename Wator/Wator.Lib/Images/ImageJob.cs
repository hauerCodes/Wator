using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using Wator.Lib.World;

namespace Wator.Lib.Images
{
    public class ImageJob
    {
        public ImageJob(Stream dataStream, int round)
        {
            this.DataStream = dataStream;
            this.Round = round;

            this.IsFinished = false;
            this.File = null;
        }

        public Stream DataStream { get; private set; }

        public int Round { get; private set; }

        public Bitmap File { get; private set; }

        public bool IsFinished { get; private set; }

        public WatorWorld LoadFromStream()
        {
            throw new NotImplementedException();
            return null;
        }
    }
}
