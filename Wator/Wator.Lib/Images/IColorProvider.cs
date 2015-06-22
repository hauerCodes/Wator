// -----------------------------------------------------------------------
// <copyright file="IColorProvider.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Lib - IColorProvider.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Lib.Images
{
    using System.Drawing;

    /// <summary>
    /// The ColorProvider interface.
    /// </summary>
    public interface IColorProvider
    {
        /// <summary>
        /// Gets the draw color.
        /// </summary>
        Color DrawColor { get; }
    }
}