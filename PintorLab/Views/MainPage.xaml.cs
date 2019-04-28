﻿using PintorLab.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace PintorLab.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lienzo));
        }

        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            await FileController.AbrirArchivo();
        }

        private void BtnTemplates_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TemplatesView));
        }
    }
}