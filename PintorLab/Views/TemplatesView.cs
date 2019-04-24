using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PintorLab.Views
{
    public class TemplatesView : Page
    {
        public TemplatesView()
        {
            CreatePage();
        }

        private IEnumerable<string> GetFiles()
        {
            return Directory.GetFiles(@"..\Templates", "*.*", SearchOption.TopDirectoryOnly)
            .Where(s => s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".bmp"));
        }

        private void CreatePage()
        {
            IEnumerable<string> files = GetFiles();
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
