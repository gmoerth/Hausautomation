using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Hausautomation.Model;
using Hausautomation.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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
        public static Fritzbox fritzbox;

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.VisibilityChanged += Window_VisibilityChanged;

            Devicelist = new DeviceList();
            settingsPage = new SettingsPage();

            ReadXDoc readXDoc = new ReadXDoc();
            readXDoc.ReadAllXDocuments(); // Lese alle Dokumente von der HomeMatic ein und speicher sie in der Devicelist
            readXDoc.StartStateListTask(); // Alle 30-60 Sekunden die StateList aktualisieren

            fritzbox = new Fritzbox();
            fritzbox.StarteMonitoringTask(); // Alle 60 Sekunden die Fritzboxen abfragen

            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.ForegroundColor = Colors.Black;
            ApplicationView.PreferredLaunchViewSize = new Size(1024, 768);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
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
            contentFrame.Navigate(typeof(PageLoad));
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
                if (!(args.SelectedItem is NavigationViewItem)) // wegen der AutoSuggestBox
                    return;
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
                    case "PageProg":
                        nvHausautomation.Header = "Programme";
                        break;
                    case "PageHelp":
                        nvHausautomation.Header = "Hilfe";
                        break;
                    default:
                        throw new NotImplementedException();
                }
                //nvHausautomation.Header = ((string)selectedItem.Tag);
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            Debug.WriteLine("AutoSuggestBox_TextChanged");
            ObservableCollection<string> Suggestions = new ObservableCollection<string>() { };
            // Device_Type
            foreach (Device device in Devicelist.Devicelist)
                if (device.Device_type != null)                                      // doppelte aussortieren
                    if (device.Device_type.ToLower().Contains(sender.Text.ToLower()) && !Suggestions.Contains(device.Device_type))
                        Suggestions.Add(device.Device_type);
            // Address
            foreach (Device device in Devicelist.Devicelist)
                if (device.Address != null)
                    if (device.Address.ToLower().Contains(sender.Text.ToLower()))
                        Suggestions.Add(device.Address);
            // Room
            foreach (Room room in Devicelist.Roomlist.Roomlist)
                if (room.Name.ToLower().Contains(sender.Text.ToLower()))
                    Suggestions.Add(room.Name);
            // Function
            foreach (Function function in Devicelist.Functionlist.Functionlist)
                if (function.Name.ToLower().Contains(sender.Text.ToLower()))
                    Suggestions.Add(function.Name);
            // sort einbauen
            List<string> suchvorschlag = Suggestions.OrderBy(x => x.FirstOrDefault()).ToList();
            asbSuche.ItemsSource = suchvorschlag;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Debug.WriteLine("AutoSuggestBox_QuerySubmitted");
            string itemtag = "";
            // Room
            foreach (Room room in Devicelist.Roomlist.Roomlist)
                if (room.Name.ToLower().Contains(args.QueryText.ToLower()))
                {
                    MainPage.settingsPage.fa.RoomItemSel = room.Name;
                    Type pageType = Type.GetType("Hausautomation.Pages.PageRoom");
                    contentFrame.Navigate(pageType);
                    itemtag = "PageRoom";
                    break;
                }
            // Function
            foreach (Function function in Devicelist.Functionlist.Functionlist)
                if (function.Name.ToLower().Contains(args.QueryText.ToLower()))
                {
                    MainPage.settingsPage.fa.FuncItemSel = function.Name;
                    Type pageType = Type.GetType("Hausautomation.Pages.PageFunc");
                    contentFrame.Navigate(pageType);
                    itemtag = "PageFunc";
                    break;
                }
            // Device_Type
            foreach (Device device in Devicelist.Devicelist)
                if (device.Device_type != null)
                    if (device.Device_type.ToLower().Contains(args.QueryText.ToLower()))
                    {
                        MainPage.settingsPage.fa.AllItemScroll = device.Device_type;
                        Type pageType = Type.GetType("Hausautomation.Pages.PageAll");
                        contentFrame.Navigate(pageType);
                        itemtag = "PageAll";
                        break;
                    }
            // Address
            foreach (Device device in Devicelist.Devicelist)
                if (device.Address != null)
                    if (device.Address.ToLower().Contains(args.QueryText.ToLower()))
                    {
                        MainPage.settingsPage.fa.AllItemScroll = device.Address;
                        Type pageType = Type.GetType("Hausautomation.Pages.PageAll");
                        contentFrame.Navigate(pageType);
                        itemtag = "PageAll";
                        break;
                    }
            // damit der blauen Marker in der NavigationView stimmt
            foreach (var navViewMenuItem in nvHausautomation.MenuItems)
                if (navViewMenuItem is NavigationViewItem item)
                    if (item.Tag.Equals(itemtag))
                        item.IsSelected = true;
        }

    }
}
