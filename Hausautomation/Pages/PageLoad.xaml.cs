using System;
using System.Collections.Generic;
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
using System.Reflection;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Hausautomation.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageLoad : Page
    {
        private string _Text1;

        public string Text1
        {
            get { return _Text1; }
            set
            {
                _Text1 = value;
                tbZeile1.Text = _Text1;
            }
        }
        private string _Text2;

        public string Text2
        {
            get { return _Text2; }
            set
            {
                _Text2 = value;
                tbZeile2.Text = _Text2;
            }
        }

        private string _Text3;

        public string Text3
        {
            get { return _Text3; }
            set
            {
                _Text3 = value;
                tbZeile3.Text = _Text3;
            }
        }
        private string _Text4;

        public string Text4
        {
            get { return _Text4; }
            set
            {
                _Text4 = value;
                tbZeile4.Text = _Text4;
            }
        }

        public PageLoad()
        {
            instance = this;
            this.InitializeComponent();
        }

        private static PageLoad instance;

        public static PageLoad Instance
        {
            get
            {
                return instance;
            }
        }

        public void Stop()
        {
            prStart.IsActive = false;
            tbZeile2.Text = "Fertig geladen";
        }

        public void ShowAbout()
        {
            prStart.IsActive = false;
            Text1 = GetProdukt();
            Text2 = GetCopyright();
            Text3 = GetTrademark();
            Text4 = "Version " + GetFileVersion();
        }

        private string GetTitle()
        {
            /*Assembly asm = Assembly.GetExecutingAssembly();
            object[] obj = asm.GetCustomAttributes(false);
            foreach (object o in obj)
            {
                if (o.GetType() == typeof(AssemblyTitleAttribute))
                {
                    AssemblyTitleAttribute ata = (AssemblyTitleAttribute)o;
                    return ata.Title;
                }
            }
            return string.Empty;*/
            return "Hausautomation";
        }

        private string GetProdukt()
        {
            /*Assembly asm = Assembly.GetExecutingAssembly();
            object[] obj = asm.GetCustomAttributes(false);
            foreach (object o in obj)
            {
                if (o.GetType() == typeof(AssemblyProductAttribute))
                {
                    AssemblyProductAttribute apa = (AssemblyProductAttribute)o;
                    return apa.Product;
                }
            }
            return string.Empty;*/
            return "Hausautomation";
        }

        private string GetCopyright()
        {
            /*Assembly asm = Assembly.GetExecutingAssembly();
            object[] obj = asm.GetCustomAttributes(false);
            foreach (object o in obj)
            {
                if (o.GetType() == typeof(AssemblyCopyrightAttribute))
                {
                    AssemblyCopyrightAttribute aca = (AssemblyCopyrightAttribute)o;
                    return aca.Copyright;
                }
            }
            return string.Empty;*/
            return "Copyright © G_&_CH_MOERTH 2018";
        }

        private string GetTrademark()
        {
            /*Assembly asm = Assembly.GetExecutingAssembly();
            object[] obj = asm.GetCustomAttributes(false);
            foreach (object o in obj)
            {
                if (o.GetType() == typeof(AssemblyTrademarkAttribute))
                {
                    AssemblyTrademarkAttribute ata = (AssemblyTrademarkAttribute)o;
                    return ata.Trademark;
                }
            }
            return string.Empty;*/
            return "GMC-COMPUTER";
        }

        private string GetFileVersion()
        {
            /*Assembly asm = Assembly.GetExecutingAssembly();
            object[] obj = asm.GetCustomAttributes(false);
            foreach (object o in obj)
            {
                if (o.GetType() == typeof(AssemblyFileVersionAttribute))
                {
                    AssemblyFileVersionAttribute afa = (AssemblyFileVersionAttribute)o;
                    return afa.Version;
                }
            }
            return string.Empty;*/
            return "1.1.1.0";
        }

    }
}
