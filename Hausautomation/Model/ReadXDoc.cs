using Hausautomation.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Popups;

namespace Hausautomation.Model
{
    public class ReadXDoc
    {
        public string HMIP { get; set; }
        public int HMPO { get; set; }
        public bool online { get; set; }

        public ReadXDoc()
        {
            HMIP = "http://192.168.178.15";
            HMPO = 80;
#pragma warning disable 4014
            SeiteEinlesen("addons/xmlapi/devicelist.cgi");
            //SeiteEinlesen2("addons/xmlapi/statelist.cgi");
        }

        public async Task Frage_ob_Online()
        {
            MessageDialog showDialog = new MessageDialog("Online = JA\nOffline = Nein", "Online Verbindung mit HomeMatic herstellen?");
            showDialog.Commands.Add(new UICommand("Ja") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Nein") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
                online = true;
            else
                online = false;
        }

        public async Task SeiteEinlesen(string page)
        {
            await Frage_ob_Online();
            try
            {
                XDocument xdoc;
                if (online == true)
                {
                    // Daten von der HomeMatic laden
                    Uri uri = new Uri(HMIP + ":" + HMPO.ToString() + "/" + page);
                    HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    xdoc = XDocument.Load(reader);
                }
                else
                {
                    // Lokales File laden
                    xdoc = XDocument.Load("z_devicelist.xml");
                }
                MainPage.devicelist.LoadXDocument(xdoc);
                Debug.WriteLine("Seite 1 fertig");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SeiteEinlesen " + ex.Message.ToString());
            }
            SeiteEinlesen2("addons/xmlapi/statelist.cgi");
        }

        public async Task SeiteEinlesen2(string page)
        {
            await Frage_ob_Online();
            try
            {
                XDocument xdoc;
                if (online == true)
                {
                    // Daten von der HomeMatic laden
                    Uri uri = new Uri(HMIP + ":" + HMPO.ToString() + "/" + page);
                    HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    xdoc = XDocument.Load(reader);
                }
                else
                {
                    // Lokales File laden
                    xdoc = XDocument.Load("z_statelist.xml");
                }
                MainPage.devicelist.LoadXDocument(xdoc);
                Debug.WriteLine("Seite 2 fertig");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SeiteEinlesen2 " + ex.Message.ToString());
            }
            SeiteEinlesen3("addons/xmlapi/roomlist.cgi");
        }

        public async Task SeiteEinlesen3(string page)
        {
            await Frage_ob_Online();
            try
            {
                XDocument xdoc;
                if (online == true)
                {
                    // Daten von der HomeMatic laden
                    Uri uri = new Uri(HMIP + ":" + HMPO.ToString() + "/" + page);
                    HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    xdoc = XDocument.Load(reader);
                }
                else
                {
                    // Lokales File laden
                    xdoc = XDocument.Load("z_roomlist.xml");
                }
                MainPage.devicelist.LoadXDocument(xdoc);
                Debug.WriteLine("Seite 3 fertig");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SeiteEinlesen3 " + ex.Message.ToString());
            }
            SeiteEinlesen4("addons/xmlapi/functionlist.cgi");
        }

        public async Task SeiteEinlesen4(string page)
        {
            await Frage_ob_Online();
            try
            {
                XDocument xdoc;
                if (online == true)
                {
                    // Daten von der HomeMatic laden
                    Uri uri = new Uri(HMIP + ":" + HMPO.ToString() + "/" + page);
                    HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    xdoc = XDocument.Load(reader);
                }
                else
                {
                    // Lokales File laden
                    xdoc = XDocument.Load("z_functionlist.xml");
                }
                MainPage.devicelist.LoadXDocument(xdoc);
                Debug.WriteLine("Seite 4 fertig");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SeiteEinlesen4 " + ex.Message.ToString());
            }
            ///////////
            foreach (Device device in MainPage.devicelist.Devicelist)
            {
                Debug.WriteLine($"Device {device.Name}");
                foreach (Channel channel in device.Channellist.Channellist)
                {
                    foreach (Room room in channel.Roomlist.Roomlist)
                    {
                        Debug.WriteLine($"   Channel {channel.Name} {room.Name} ");
                    }
                    foreach (Function function in channel.Functionlist.Functionlist)
                    {
                        Debug.WriteLine($"   Channel {channel.Name} {function.Name} {function.Description}");
                    }
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        Debug.WriteLine($"      Datapoint {datapoint.Name}");
                    }
                }
            }
            ///////////
        }
#pragma warning restore 4014

    }
}
