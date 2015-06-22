// -----------------------------------------------------------------------
// <copyright file="IDrawable.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - IDrawable.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Images
{
    /// <summary>
    /// The Drawable interface.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// The get drawing elements.
        /// </summary>
        /// <returns>
        /// The <see cref="int[,]"/>.
        /// </returns>
        int[,] GetDrawingElements();
    }
}