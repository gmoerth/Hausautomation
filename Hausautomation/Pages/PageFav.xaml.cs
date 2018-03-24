using Hausautomation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class PageFav : Page
    {
        private IEnumerable<Device> Devicelist;

        public PageFav()
        {
            this.InitializeComponent();

            Devicelist = MainPage.Devicelist.Favoriten;
        }

        private void slSlider1_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

        }

        private void btButton1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btButton2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btButtonOb_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btButtonUn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btButtonLi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btButtonRe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tgSwitch1_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void tgSwitch2_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void lvDevices_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
