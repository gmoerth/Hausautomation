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
                Debug.WriteLine(device.Ise_id + " " + device.Name);
                //device.bFavoriten = !device.bFavoriten
                Favoriten fav = new Favoriten();
                if (device.bFavoriten == true)
                    fav.AddFavoriten(device.Ise_id);
                else
                    fav.RemoveFavoriten(device.Ise_id);
            }
        }

        private void slSlider1_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider sli = (Slider)sender;
            Device device = (Device)sli.DataContext;
            if (device != null && !(e.NewValue == 0 && e.OldValue == 100) && !(e.NewValue == 100 && e.OldValue == 0) && device.Device_type == "HmIP-BROLL")
            {
                Debug.WriteLine("slSlider1_ValueChanged");
                Debug.WriteLine(device.Ise_id + " " + device.Name + " " + sli.Value.ToString());
                device.StateChange(sli.Value);
            }
        }

        private void btButton1_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btButton1_Click");
            Button btn = (Button)sender;
            Device device = (Device)btn.DataContext;
            if (device != null)
            {
                Debug.WriteLine(device.Ise_id + " " + device.Name);
                device.StateChange(false);
            }
        }

        private void btButton2_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btButton2_Click");
            Button btn = (Button)sender;
            Device device = (Device)btn.DataContext;
            if (device != null)
            {
                Debug.WriteLine(device.Ise_id + " " + device.Name);
                device.StateChange2(false);
            }
        }

        private void btButtonOb_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btButtonOb_Click");
            Button btn = (Button)sender;
            Device device = (Device)btn.DataContext;
            if (device != null)
            {
                Debug.WriteLine(device.Ise_id + " " + device.Name);
                device.StateChange2(false);
            }
        }

        private void btButtonUn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btButtonUn_Click");
            Button btn = (Button)sender;
            Device device = (Device)btn.DataContext;
            if (device != null)
            {
                Debug.WriteLine(device.Ise_id + " " + device.Name);
                device.StateChange(false);
            }
        }

        private void btButtonLi_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btButtonLi_Click");
            Button btn = (Button)sender;
            Device device = (Device)btn.DataContext;
            if (device != null)
            {
                device.PrepareHMPB4DisWM(false);
            }
        }

        private void btButtonRe_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("btButtonRe_Click");
            Button btn = (Button)sender;
            Device device = (Device)btn.DataContext;
            if (device != null)
            {
                device.PrepareHMPB4DisWM(true);
            }
        }

        private void tgSwitch1_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch tgs = (ToggleSwitch)sender;
            Device device = (Device)tgs.DataContext;
            if (device != null)
            {
                Debug.WriteLine("tgSwitch1_Toggled");
                Debug.WriteLine(device.Ise_id + " " + device.Name + " " + tgs.IsOn.ToString());
                device.StateChange(tgs.IsOn);
            }
        }

        private void tgSwitch2_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch tgs = (ToggleSwitch)sender;
            Device device = (Device)tgs.DataContext;
            if (device != null)
            {
                Debug.WriteLine("tgSwitch2_Toggled");
                Debug.WriteLine(device.Ise_id + " " + device.Name + " " + tgs.IsOn.ToString());
                device.StateChange2(tgs.IsOn);
            }
        }
    }
}
