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
        public ObservableCollection<string> Suggestions { get; private set; }

        public MainPage()
        {
            this.Suggestions = new ObservableCollection<string>() { "WC", "Auto", "Haus" };
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
            /////
            Suggestions.Clear();
            foreach (Room room in Devicelist.Roomlist.Roomlist)
                if (room.Name.ToLower().Contains(sender.Text.ToLower()))
                    Suggestions.Add(room.Name);
            foreach (Function function in Devicelist.Functionlist.Functionlist)
                if (function.Name.ToLower().Contains(sender.Text.ToLower()))
                    Suggestions.Add(function.Name);
            // doppelte aussortieren
            /*foreach (Device device in Devicelist.Devicelist)
                if (device.Device_type != null)
                    if (device.Device_type.ToLower().Contains(sender.Text.ToLower()))
                        Suggestions.Add(device.Device_type);
            foreach (Device device in Devicelist.Devicelist)
                if (device.Address != null)
                    if (device.Address.ToLower().Contains(sender.Text.ToLower()))
                        Suggestions.Add(device.Address);*/
            /////
            Debug.WriteLine("AutoSuggestBox_TextChanged");
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Suggestions.Clear();
                //Suggestions.Add(sender.Text + "1");
                //Suggestions.Add(sender.Text + "2");
            }
            asbSuche.ItemsSource = Suggestions;
            /*if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
             {
                 var suggestions = SearchControls(sender.Text);

                 if (suggestions.Count > 0)
                     sender.ItemsSource = suggestions;
                 else
                     sender.ItemsSource = new string[] { "No results found" };
             }*/
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Debug.WriteLine("AutoSuggestBox_QuerySubmitted");
            /*if (args.ChosenSuggestion != null && args.ChosenSuggestion is ControlInfoDataItem)
              {
                  //User selected an item, take an action
                  SelectControl(args.ChosenSuggestion as ControlInfoDataItem);
              }
              else if (!string.IsNullOrEmpty(args.QueryText))
              {
                  //Do a fuzzy search based on the text
                  var suggestions = SearchControls(sender.Text);
                  if (suggestions.Count > 0)
                  {
                      SelectControl(suggestions.FirstOrDefault());
                  }
              }*/
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Debug.WriteLine("AutoSuggestBox_SuggestionChosen");
            //Don't autocomplete the TextBox when we are showing "no results"
            /*if (args.SelectedItem is ControlInfoDataItem control)
            {
                sender.Text = control.Title;
            }*/
        }
    }
}
