using Hausautomation.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class PageRoom : Page
    {
        private List<string> Roomlist = new List<string>();
        private IEnumerable<Device> Devicelist;

        public PageRoom()
        {
            this.InitializeComponent();

            foreach (Room function in MainPage.Devicelist.Roomlist.Roomlist)
                Roomlist.Add(function.Name);

            Devicelist = MainPage.Devicelist.Room;

            cbRoom.SelectedItem = MainPage.settingsPage.fa.RoomItemSel;
        }

        private void cbRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("cbRoom_SelectionChanged");
            ComboBox cb = (ComboBox)sender;
            string str = (string)cb.SelectedItem;
            if (str != null)
            {
                //Debug.WriteLine(str);
                foreach (Device device in MainPage.Devicelist.Devicelist)
                {
                    device.bRoom = false;
                    foreach (Channel channel in device.Channellist.Channellist)
                    {
                        foreach (Room function in channel.Roomlist.Roomlist)
                        {
                            if (function.Name == str)
                            {
                                device.bRoom = true;
                                //Debug.WriteLine($"device={device.Name}");
                            }
                        }
                    }
                }
                Favoriten fav = MainPage.settingsPage.fa;
                fav.RoomItemSel = str;
                MainPage.settingsPage.SaveSettingsXML();
                Devicelist = MainPage.Devicelist.Room;
                lvDevices.ItemsSource = Devicelist;
            }
        }
    }
}
