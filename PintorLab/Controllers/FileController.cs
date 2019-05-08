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
    /**
     * Controlador que gestiona el comportamiento con los archivos
     */
    public static class FileController
    {
        /**
         * Guarda el dibujo del InkCanvas en un archivo con el formato especificado
         * Se utiliza paquete
         */
        public static async Task GuardarDibujo(InkCanvas inkCanvas)
        {
            FileSavePicker savePicker = new FileSavePicker();

            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("Image", new List<string>() { ".png", ".jpeg", ".jpg" });
            savePicker.SuggestedFileName = "newdraw";

            StorageFile sf = await savePicker.PickSaveFileAsync();

            if (sf != null)
            {
                var stream = await sf.OpenAsync(FileAccessMode.ReadWrite);
                await DibujoAImagen(inkCanvas).SaveAsync(stream, CanvasBitmapFileFormat.Png, 1f);
            }
        }

        /**
         * Abre un FileOpenPicker y retorna el archivo seleccionado
         */
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

        /**
         * Transforma el dibujo del InkCanvas a una imagen
         */
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
