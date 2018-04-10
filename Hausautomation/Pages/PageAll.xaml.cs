using Hausautomation.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Hausautomation.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageAll : Page
    {
        private ObservableCollection<Device> Devicelist;

        public PageAll()
        {
            this.InitializeComponent();

            Devicelist = MainPage.Devicelist.Devicelist;
        }

        private void cbFavoriten_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("cbFavoriten_Click");
            CheckBox cb = (CheckBox)sender;
            Device device = (Device)cb.DataContext;
            if (device != null)
            {
                //Debug.WriteLine(device.Ise_id + " " + device.Name);
                Favoriten fav = new Favoriten();
                if (device.bFavoriten == true)
                    fav.AddFavoriten(device.Ise_id);
                else
                    fav.RemoveFavoriten(device.Ise_id);
            }
        }

        private void lvDevices_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("lvDevices_Loaded");
            // nur wenn die suche aktiv ist
            if (MainPage.settingsPage.fa.AllItemScroll != null)
            {
                foreach (Device device in Devicelist)
                    if (device.Address == MainPage.settingsPage.fa.AllItemScroll)
                    {
                        lvDevices.SelectedItem = device;
                        lvDevices.ScrollIntoView(device);
                        break;
                    }
                foreach (Device device in Devicelist)
                    if (device.Device_type == MainPage.settingsPage.fa.AllItemScroll)
                    {
                        lvDevices.SelectedItem = device;
                        lvDevices.ScrollIntoView(device);
                        break;
                    }
                MainPage.settingsPage.fa.AllItemScroll = null;
            }
        }
    }
}
