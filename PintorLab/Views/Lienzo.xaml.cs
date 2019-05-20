using PintorLab.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
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
    ///Clase de la página del Lienzo
    ///</summary>
    public sealed partial class Lienzo : Page
    {
        ///<summary>
        ///Almacena los cambios que se realizan sobre el InkCanvas
        ///</summary>
        public Stack<InkStroke> UndoStrokes { get; set; }

        ///<summary>
        ///Inicializa los componentes de la clase y tipos de dispositivos compatibles.
        ///</summary>
        public Lienzo()
        {
            UndoStrokes = new Stack<InkStroke>();
            this.InitializeComponent();
            canvasBorder.BorderThickness = new Thickness(2);
            miCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen |
                Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }

        //TODO
        ///<summary>
        ///Llama al padre y inicializa el Lienzo con el archivo pasado
        ///</summary>
        ///<param name="sf">
        ///Es la imagen que se carga en el InkCanvas
        /// </param>
        public Lienzo(StorageFile sf) : this()
        {

        }

        ///<summary>
        ///Evento de guardado
        ///</summary>
        ///<param name="e">
        ///El argumento del evento
        /// </param>
        /// <param name="sender">
        /// El objeto que lo envía
        /// </param>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await FileController.GuardarDibujo(miCanvas);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }

        ///<summary>
        ///Evento de apertura de archivo
        ///</summary>
        ///<param name="sender">
        ///El objeto que lo envía
        /// </param>
        /// <param name="e">
        /// El argumento del evento
        /// </param>
        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            StorageFile sf;
            
            if ((sf = await FileController.AbrirArchivo()) != null)
            {
                IRandomAccessStream stream = await sf.OpenAsync(FileAccessMode.Read);
                using (var inputStream = stream.GetInputStreamAt(0))
                {
                    try
                    {
                        await miCanvas.InkPresenter.StrokeContainer.LoadAsync(inputStream);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage(ex.Message);
                    }
                }
                stream.Dispose();
            }
        }

        ///<summary>
        ///Evento que elimina el contenido del InkCanvas
        ///</summary>
        ///<param name="sender">
        ///El objeto que lo envía
        /// </param>
        /// <param name="e">
        /// El argumento del evento
        /// </param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                miCanvas.InkPresenter.StrokeContainer.Clear();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
            
        }

        ///<summary>
        ///Vuelve al último cambio realizado en el InkCanvas
        ///</summary>
        ///<param name="sender">
        ///El objeto que lo envía
        /// </param>
        /// <param name="e">
        /// El argumento del evento
        /// </param>
        private void Undo(object sender, RoutedEventArgs e)
        {
            try
            {
                IReadOnlyList<InkStroke> strokes = miCanvas.InkPresenter.StrokeContainer.GetStrokes();
                if (strokes.Count > 0)
                {
                    strokes[strokes.Count - 1].Selected = true;
                    UndoStrokes.Push(strokes[strokes.Count - 1]);
                    miCanvas.InkPresenter.StrokeContainer.DeleteSelected();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }

        ///<summary>
        ///Rehace el Undo
        ///</summary>
        ///<param name="sender">
        ///El objeto que lo envía
        /// </param>
        /// <param name="e">
        /// El argumento del evento
        /// </param>
        private void Redo(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UndoStrokes.Count > 0)
                {
                    InkStroke stroke = UndoStrokes.Pop();
                    InkStrokeBuilder strokeBuilder = new InkStrokeBuilder();

                    strokeBuilder.SetDefaultDrawingAttributes(stroke.DrawingAttributes);
                    System.Numerics.Matrix3x2 matrix = stroke.PointTransform;
                    IReadOnlyList<InkPoint> inkPoints = stroke.GetInkPoints();
                    InkStroke stk = strokeBuilder.CreateStrokeFromInkPoints(inkPoints, matrix);
                    miCanvas.InkPresenter.StrokeContainer.AddStroke(stk);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }

        ///<summary>
        ///Evento para volver al menú
        ///</summary>
        ///<param name="sender">
        ///El objeto que lo envía
        /// </param>
        /// <param name="e">
        /// El argumento del evento
        /// </param>
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }

        ///<summary>
        ///Muestra un mensaje de error
        ///</summary>
        ///<param name="message">
        ///El mensaje que muestra
        /// </param>
        private async void ErrorMessage(string message)
        {
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "Ok"
            };
            await errorDialog.ShowAsync();
        }
    }
}
