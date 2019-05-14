using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PintorLab.Controllers
{
    ///<summary>
    ///Controlador que gestiona el comportamiento con los archivos
    ///</summary>
    public static class FileController
    {
        ///<summary>
        ///Guarda el dibujo del InkCanvas en un archivo con el formato especificado
        ///</summary>
        ///<remarks>
        ///Se utiliza el paquete Win2D.UWP
        ///</remarks>
        ///<param name="inkCanvas">
        ///El InkCanvas del que se recoge el dibujo
        /// </param>
        public static async Task GuardarDibujo(InkCanvas inkCanvas)
        {
            FileSavePicker savePicker = new FileSavePicker();

            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("PNG", new List<string>() { ".png"});
            savePicker.FileTypeChoices.Add("JPG", new List<string>() { ".bmp"});
            savePicker.FileTypeChoices.Add("JPEG", new List<string>() { ".jpeg"});
            savePicker.SuggestedFileName = "newdraw";

            StorageFile sf = await savePicker.PickSaveFileAsync();
            if (sf != null)
            {
                var stream = await sf.OpenAsync(FileAccessMode.ReadWrite);
                string fName = sf.Name.ToLower();
                if (fName.EndsWith("bmp"))
                {
                    await DibujoAImagen(inkCanvas).SaveAsync(stream, CanvasBitmapFileFormat.Bmp, 1f);
                }
                else if (fName.EndsWith("png"))
                {
                    await DibujoAImagen(inkCanvas).SaveAsync(stream, CanvasBitmapFileFormat.Png, 1f);
                }
                else if (fName.EndsWith("jpeg"))
                {
                    await DibujoAImagen(inkCanvas).SaveAsync(stream, CanvasBitmapFileFormat.Jpeg, 1f);
                }
            }
        }

        ///<summary>
        ///Abre un FileOpenPicker y retorna el archivo seleccionado
        ///</summary>
        public static async Task<StorageFile> AbrirArchivo()
        {
            FileOpenPicker fop = new FileOpenPicker();

            fop.ViewMode = PickerViewMode.Thumbnail;
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.FileTypeFilter.Add(".png");

            return await fop.PickSingleFileAsync();
        }

        ///<summary>
        ///Transforma el dibujo del InkCanvas a una imagen
        ///</summary>
        ///<param name="inkCanvas">
        ///El InkCanvas de donde se recoge el dibujo
        /// </param>
        private static CanvasRenderTarget DibujoAImagen(InkCanvas inkCanvas)
        {
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, (int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, 96);

            using (CanvasDrawingSession session = renderTarget.CreateDrawingSession())
            {
                session.Clear(Colors.White);
                session.DrawInk(inkCanvas.InkPresenter.StrokeContainer.GetStrokes());
            }

            return renderTarget;
        }
    }
}
