using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace PintorLab.Views
{
    ///<summary>
    ///Clase que de las plantillas de dibujo
    ///</summary>
    public sealed partial class TemplatesView : Page
    {
        ///<summary>
        ///Coleccion de imagenes de previsualización
        ///</summary>
        private ObservableCollection<string> current;

        ///<summary>
        ///Tarea que inicializa elementos de forma asincrona
        ///</summary>
        private Task Inicializacion { get; set; }

        ///<summary>
        ///Inicializa las propiedades
        ///</summary>
        public TemplatesView()
        {
            this.DataContext = current;
            Inicializacion = InicializaColeccionAsync();
        }

        ///<summary>
        ///Inicializa la colección de imágenes para previsualización
        ///</summary>
        private async Task InicializaColeccionAsync()
        {
            current = await GetFiles();
            current.Add(@"D:\Users\Alex\Pictures\Templates\flowers.png");
            this.InitializeComponent();
        }

        ///<summary>
        ///Obtiene los archivos de la carpeta Templates en Pictures
        ///</summary>
        private async Task<ObservableCollection<string>> GetFiles()
        {
            StorageFolder appFolder = null;
            try
            {
                appFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync("Templates");
            }
            catch (FileNotFoundException)
            {
                appFolder = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFolderAsync("Templates");
                StorageFolder tempFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Templates");
                IReadOnlyList<StorageFile> files = await tempFolder.GetFilesAsync();
                foreach (StorageFile file in files)
                {
                    await file.CopyAsync(appFolder);
                }
            }
            IReadOnlyList<StorageFile> ff = await appFolder.GetFilesAsync();
            ObservableCollection<string> sfiles = new ObservableCollection<string>();
            foreach (StorageFile sf in ff)
            {
                string path = sf.Path;
                if (path.EndsWith(".jpeg") || path.EndsWith(".png") || path.EndsWith(".bmp"))
                {
                    sfiles.Add(path);
                }
            }
            return sfiles;
        }

        ///<summary>
        ///Crea la página de forma dinamica
        ///</summary>
        [Obsolete("Se reemplazó para crear un GridView")]
        private async void CreatePage()
        {
            IEnumerable<string> files = await GetFiles();
            int count = files.Count();

            StackPanel sp = new StackPanel();
            sp.Children.Add(new TextBlock()
            {
                Text = "Empty"
            });
            if (count == 0)
            {
            }
            else
            {
                Image[] images = new Image[count];
                for (int i = 0; i < images.Length; i++)
                {
                    images[i] = FileToImage(files.ElementAt(i));
                }

            }
        }

        ///<summary>
        ///Transforma un archivo a imagen
        ///</summary>
        [Obsolete]
        private Image FileToImage(string file)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(file));
            return image;
        }
    }
}
