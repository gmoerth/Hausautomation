using Hausautomation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class SamplePage1 : Page
    {

        private ObservableCollection<Item> _items;
        //private SolidColorBrush InvalidFormat = new SolidColorBrush(Colors.Red);
        //private SolidColorBrush ValidFormat = new SolidColorBrush(Colors.Green);


        public SamplePage1()
        {
            this.InitializeComponent();
            _items = Item.GetItems(100);
            //SeiteEinlesen("http://192.168.178.15", 80, "addons/xmlapi/devicelist.cgi"); 
            //SeiteEinlesen("http://192.168.178.15", 80, "addons/xmlapi/statelist.cgi");
        }

        /*public async Task<string> SeiteEinlesen(string ip, int port, string page)
        {
            string str = "";
            try
            {
                Uri uri = new Uri(ip + ":" + port.ToString() + "/" + page);
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                XDocument xdoc = XDocument.Load(reader);
                //IEnumerable<XElement> deviceList = xdoc.Elements();

                foreach (XElement element in xdoc.Descendants("device").Descendants("channel"))
                {
                    Debug.WriteLine(element);
                    
                    //Debug.WriteLine(element.Element("name").Value);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SeiteEinlesen " + ex.Message.ToString());
            }
            return str;
        }*/


    }
}
