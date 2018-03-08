using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Hausautomation.Model;
using Hausautomation.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        static DeviceList devicelist;


        public MainPage()
        {
            this.InitializeComponent();

            devicelist = new DeviceList();
            ReadXDoc readXDoc = new ReadXDoc(devicelist);


            Debug.WriteLine("Main fertig\r\n");
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            //contentFrame.Navigate(typeof(SamplePage1));
            contentFrame.Navigate(typeof(MainPageHeader));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                string pageName = "Hausautomation.Pages." + ((string)selectedItem.Tag);
                Type pageType = Type.GetType(pageName);
                contentFrame.Navigate(pageType);
                nvSample.Header = ((string)selectedItem.Tag);
            }
        }

    }
}
