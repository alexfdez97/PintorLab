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
    /**
     * Clase de la página del Lienzo
     */
    public sealed partial class Lienzo : Page
    {
        /**
         * Almacena los cambios que se realizan sobre el InkCanvas
         */
        public Stack<InkStroke> UndoStrokes { get; set; }

        /**
         * Inicializa los componentes de la clase y tipos de dispositivos compatibles.
         */
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
        /**
         * Llama al padre y inicializa el Lienzo con el archivo pasado
         */
        public Lienzo(StorageFile sf) : this()
        {

        }

        /**
         * Evento de guardado
         */
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await FileController.GuardarDibujo(miCanvas);
        }

        /**
         * Evento de apertura de archivo
         */
        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            StorageFile sf;
            
            if ((sf = await FileController.AbrirArchivo()) != null)
            {
                IRandomAccessStream stream = await sf.OpenAsync(FileAccessMode.Read);
                //Image image = new Image();
                //image.Source = new BitmapImage(new Uri("ms - appx:///Assets/donut-icon.png"));
                using (var inputStream = stream.GetInputStreamAt(0))
                {
                    await miCanvas.InkPresenter.StrokeContainer.LoadAsync(inputStream);
                }
                stream.Dispose();
            }
        }

        /**
         * Evento que elimina el contenido del InkCanvas
         */
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            miCanvas.InkPresenter.StrokeContainer.Clear();
        }

        /**
         * Vuelve al último cambio realizado en el InkCanvas
         */
        private void Undo(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<InkStroke> strokes = miCanvas.InkPresenter.StrokeContainer.GetStrokes();
            if (strokes.Count > 0)
            {
                strokes[strokes.Count - 1].Selected = true;
                UndoStrokes.Push(strokes[strokes.Count - 1]);
                miCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            }
        }

        /**
         * Rehace el Undo
         */
        private void Redo(object sender, RoutedEventArgs e)
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

        /**
         * Evento para volver al menú
         */
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
