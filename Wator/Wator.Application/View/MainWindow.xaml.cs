// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>Wator.Application - MainWindow.xaml.cs</summary>
// -----------------------------------------------------------------------
namespace Wator.Application.View
{
    using System.Windows;

    using GalaSoft.MvvmLight.CommandWpf;

    using Wator.Application.ViewModel;

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            var mainViewModel = this.DataContext as MainViewModel;
            if (mainViewModel != null)
            {
                mainViewModel.StopSimulation.Execute(this);
            }
        }
    }
}