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

        public PageProg()
        {
            this.InitializeComponent();

            Programs prog = new Programs();
            prog.Device = 1;
            prog.MAC = "00:00:00:00:00:00";
            prog.Ise_Id = 1234;
            prog.IfOnline = true;
            prog.Delay = 300;
            Programlist = new ObservableCollection<Programs>();
            Programlist.Add(prog);
        }
    }
}
