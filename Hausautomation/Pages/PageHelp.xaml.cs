using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Hausautomation.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageHelp : Page
    {
        private bool once = true;
        public PageHelp()
        {
            this.InitializeComponent();
            try
            {
                //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Grobkonzept_Gerhard_Moerth.html"));
                //StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("HTML", CreationCollisionOption.OpenIfExists);
                //await file.CopyAsync(folder, "Grobkonzept_Gerhard_Moerth.html", NameCollisionOption.ReplaceExisting);

                wvHelp.NavigationFailed += WvHelp_NavigationFailed;
                Uri targetUri = new Uri("ms-appdata:///local/HTML/Uebersicht/Uebersicht.html"); // zuerst lokale version probieren
                wvHelp.Navigate(targetUri);
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }

        private void WvHelp_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            if (once == true)
            {
                Uri targetUri = new Uri("http://members.chello.at/gmoerth/Uebersicht/Uebersicht.html");
                wvHelp.Navigate(targetUri);
                once = false;
            }
        }
    }

}
