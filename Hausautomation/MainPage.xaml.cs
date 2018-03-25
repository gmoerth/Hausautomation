using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Hausautomation.Model;
using Hausautomation.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace Hausautomation
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static DeviceList Devicelist; // Die Liste die alle Geräte enthält
        public static SettingsPage settingsPage; // Enthält alle gespeicherten Einstellungen

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.VisibilityChanged += Window_VisibilityChanged;

            Devicelist = new DeviceList();
            settingsPage = new SettingsPage();

            ReadXDoc readXDoc = new ReadXDoc(); 
            readXDoc.ReadAllXDocuments(); // Lese alle Dokumente von der HomeMatic ein und speicher sie in der Devicelist

            Debug.WriteLine("Main fertig");
        }

        private void Window_VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            if (!e.Visible)
            {
                Debug.WriteLine("Window Hidden");
                //settingsPage.SaveSettingsXML();
            }
            else
                Debug.WriteLine("Window Visible");
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            //contentFrame.Navigate(typeof(PageAll));
            contentFrame.Navigate(typeof(MainPageHeader));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(SettingsPage));
                nvHausautomation.Header = "Einstellungen";
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                string pageName = "Hausautomation.Pages." + ((string)selectedItem.Tag);
                Type pageType = Type.GetType(pageName);
                contentFrame.Navigate(pageType);
                switch ((string)selectedItem.Tag)
                {
                    case "PageAll":
                        nvHausautomation.Header = "Alle Geräte";
                        break;
                    case "PageFav":
                        nvHausautomation.Header = "Favoriten";
                        break;
                    case "PageRoom":
                        nvHausautomation.Header = "Räume";
                        break;
                    case "PageFunc":
                        nvHausautomation.Header = "Gewerke";
                        break;
                    case "MainPageHeader":
                        nvHausautomation.Header = "Hilfe";
                        break;
                    default:
                        throw new NotImplementedException();
                }
                //nvHausautomation.Header = ((string)selectedItem.Tag);
            }
        }

    }
}
