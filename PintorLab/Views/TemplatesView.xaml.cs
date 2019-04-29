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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace PintorLab.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class TemplatesView : Page
    {
        private ObservableCollection<string> current;
        private Task Inicializacion { get; set; }

        public TemplatesView()
        {
            this.DataContext = current;
            Inicializacion = InicializaColeccionAsync();
        }

        private async Task InicializaColeccionAsync()
        {
            IEnumerable<string> temp = await GetFiles();
            current = new ObservableCollection<string>(temp);
            current.Add("ms-appx:///Assets/Templates/boat.png");
            this.InitializeComponent();
        }

        private async Task<IEnumerable<string>> GetFiles()
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
            return Directory.GetFiles(appFolder.Path, "*.*", SearchOption.TopDirectoryOnly)
            .Where(s => s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".bmp"));
        }

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

        private Image FileToImage(string file)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(file));
            return image;
        }
    }
}
