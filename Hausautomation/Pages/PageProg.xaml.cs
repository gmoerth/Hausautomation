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
    public sealed partial class PageProg : Page
    {
        private ObservableCollection<Programs> Programlist;
        private bool _Lernen;

        public bool Lernen
        {
            get
            {
                ReadXDoc readXDoc = new ReadXDoc();
                _Lernen = readXDoc.Learn;
                return _Lernen;
            }
            set
            {
                _Lernen = value;
                ReadXDoc readXDoc = new ReadXDoc();
                readXDoc.Learn = _Lernen;
                MainPage.settingsPage.SaveSettingsXML();
            }
        }

        public PageProg()
        {
            this.InitializeComponent();

            Programlist = MainPage.Devicelist.Programlist.Programlist;
        }
    }
}
