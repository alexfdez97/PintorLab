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
    ///<summary>
    ///Clase de la página principal
    ///</summary>
    public sealed partial class MainPage : Page
    {
        ///<summary>
        ///Inicializa la página
        ///</summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        ///<summary>
        ///Controla el evento Click del botón BtnNew
        ///</summary>
        ///<param name="e">
        ///El argumento del evento
        /// </param>
        /// <param name="sender">
        /// El objeto que lo envía
        /// </param>
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lienzo));
        }

        ///<summary>
        ///Controla el evento Click del botón BtnOpen
        ///</summary>
        ///<param name="e">
        ///El argumento del evento
        /// </param>
        /// <param name="sender">
        /// El objeto que lo envía
        /// </param>
        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            await FileController.AbrirArchivo();
        }

        ///<summary>
        ///Controla el evento Click del botón BtnTemplates
        ///</summary>
        ///<param name="e">
        ///El argumento del evento
        /// </param>
        /// <param name="sender">
        /// El objeto que lo envía
        /// </param>
        private void BtnTemplates_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TemplatesView));
        }
    }
}
