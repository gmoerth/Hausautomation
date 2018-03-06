using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Hausautomation.Model
{
    public class Item
    {
        #region Properties
        private BitmapImage imageSource;
        private static List<BitmapImage> sources;
        private string id;
        private string description;

        public BitmapImage ImageSource
        {
            get
            {
                return imageSource;
            }

            set
            {
                imageSource = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public Item()
        {
            imageSource = new BitmapImage();
            id = string.Empty;
            description = "Bla Bla Bla Hier kommt die Beschreibung, Bla Bla Bla Hier kommt die Beschreibung, Bla Bla Bla Hier kommt die Beschreibung, Bla Bla Bla Hier kommt die Beschreibung, Bla Bla Bla Hier kommt die Beschreibung, Bla Bla Bla Hier kommt die Beschreibung, ";

            sources = new List<BitmapImage>()
            {
                new BitmapImage(new Uri("ms-appx:///Assets/112_hmip-wrc2_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/113_hmip-psm_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/124_hm-sec-mdir_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/4_hm-lc-sw1-fm_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/5_hm-lc-sw2-fm_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/70_hm-pb-4dis-wm_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/70_hm-pb-4dis-wm_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/93_hm-es-pmsw1-pl_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/98_hm-sec-sco_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/PushButton-2ch-wm_thumb.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/Speedy.jpg")),
                //new BitmapImage(new Uri("ms-appx:///Assets/"))
            };
        }

        public static Item GetNewItem(int i, int j)
        {
            return new Item()
            {
                imageSource = GenerateImageSource(j),
                id = i.ToString()
            };
        }

        public static ObservableCollection<Item> GetItems(int numberOfContacts)
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>();

            for (int i = 0; i < numberOfContacts; i++)
            {
                items.Add(GetNewItem(i, i % 11));
            }
            return items;
        }

        private static BitmapImage GenerateImageSource(int i)
        {
            return sources[i];
        }
        #endregion
    }
}
