using PintorLab.Controllers;
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

namespace PintorLab.Views
{
    /**
     * Clase de la página principal
     */
    public sealed partial class MainPage : Page
    {
        /**
         * Inicializa la página
         */
        public MainPage()
        {
            this.InitializeComponent();
        }

        /**
         * Controla el evento Click del botón BtnNew
         */
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lienzo));
        }

        /**
         * Controla el evento Click del botón BtnOpen
         */
        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            await FileController.AbrirArchivo();
        }

        /**
         * Controla el evento Click del botón BtnTemplates
         */
        private void BtnTemplates_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TemplatesView));
        }
    }
}
