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
    public sealed partial class Lienzo : Page
    {
        public Stack<InkStroke> UndoStrokes { get; set; }

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

        public Lienzo(StorageFile sf) : this()
        {

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await FileController.GuardarDibujo(miCanvas);
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            StorageFile sf;
            if ((sf = await FileController.AbrirArchivo()) != null)
            {
                IRandomAccessStream stream = await sf.OpenAsync(FileAccessMode.Read);
                using (var inputStream = stream.GetInputStreamAt(0))
                {
                    await miCanvas.InkPresenter.StrokeContainer.LoadAsync(inputStream);
                }
                stream.Dispose();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            miCanvas.InkPresenter.StrokeContainer.Clear();
        }

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

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
