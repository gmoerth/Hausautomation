using Hausautomation.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Hausautomation.Model
{
    public class ReadXDoc
    {
        public string HMIP { get; set; }
        public int HMPO { get; set; }
        public bool online { get; set; } // Modus zum entwickeln und testen ... geht schneller
        private int _NewId;
        public int NewId { set { _NewId = value; } }
        private double _NewValue;
        public double NewValue { set { _NewValue = value; } }
        static private bool _bShortTaskRunning = false;
        static private DateTime dateTime = DateTime.Now;

        public ReadXDoc()
        {
            if (MainPage.settingsPage != null)
            {
                HMIP = MainPage.settingsPage.xdoc.HMIP;
                HMPO = MainPage.settingsPage.xdoc.HMPO;
                online = MainPage.settingsPage.xdoc.online;
            }
        }

        public async void StartStateListTask(object sender, RoutedEventArgs e)
        {
            // dauerhaft
            while (true)
            {
                await Task.Run(() => StateListTask());
                // Update the UI with results
                MainPage.Devicelist.PrepareAllDevicesIntheList();
            }
        }

        public async void StartStateListTaskShort(object sender, RoutedEventArgs e)
        {
            dateTime = DateTime.Now;
            _bShortTaskRunning = true;
            await Task.Delay(3005);
            if (dateTime.Ticks + 30000000 > DateTime.Now.Ticks)
            {
                Debug.WriteLine("Abort Statelist");
                return;
            }
            await Task.Run(() => StateListTaskShort());
            // Update the UI with results
            MainPage.Devicelist.PrepareAllDevicesIntheList();
            _bShortTaskRunning = false;
        }

        private async Task StateListTask()
        {
            await Task.Delay(60000);
            // nur wenn nicht gerade geklickt wurde
            if (_bShortTaskRunning == false)
                await ReadXDocument("addons/xmlapi/statelist.cgi", "statelist.xml");
        }

        private async Task StateListTaskShort()
        {
            await ReadXDocument("addons/xmlapi/statelist.cgi", "statelist.xml");
        }

        public void ReadAllXDocuments()
        {
#pragma warning disable 4014
            ReadAllXDocumentsAsync();
#pragma warning restore 4014
        }

        public void ReadStateChangeXDoc(/*string NewIdAndValue*/)
        {
            //CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US"); // Punkt als Komma
            CultureInfo culture = new CultureInfo("en-US"); // Punkt als Komma
            string _NewIdAndValue = "?ise_id=" + _NewId.ToString() + "&new_value=";
            if (_NewValue == Double.PositiveInfinity)
                _NewIdAndValue += "True";
            else if (_NewValue == Double.NegativeInfinity)
                _NewIdAndValue += "False";
            else
                _NewIdAndValue += _NewValue.ToString("F", culture);
#pragma warning disable 4014
            ReadXDocument("addons/xmlapi/statechange.cgi" + _NewIdAndValue, "statechange.xml");
#pragma warning restore 4014
            StartStateListTaskShort(this, null);
        }

        public async Task ReadAllXDocumentsAsync()
        {
            await ReadXDocument("addons/xmlapi/statelist.cgi", "statelist.xml");
            await ReadXDocument("addons/xmlapi/devicelist.cgi", "devicelist.xml");
            await ReadXDocument("addons/xmlapi/roomlist.cgi", "roomlist.xml");
            await ReadXDocument("addons/xmlapi/functionlist.cgi", "functionlist.xml");
            MainPage.Devicelist.PrepareAllDevicesIntheList();
            Favoriten favoriten = new Favoriten();
            favoriten.LoadFavoriten(); // Favoriten einlesen
            MainPageHeader mph = MainPageHeader.Instance;
            while (mph == null) // falls null kurz warten und nochmal probieren
            {                // cache laden geht schneller und die View ist noch gar nicht geladen   
                await Task.Delay(500);
                mph = MainPageHeader.Instance;
            }
            mph.Stop();
            // Demo debug Ausgabe der kompletten Liste
            /*foreach (Device device in MainPage.Devicelist.Devicelist)
            {
                Debug.WriteLine($"Device {device.Name}");
                foreach (Channel channel in device.Channellist.Channellist)
                {
                    Debug.Write($"    Channel {channel.Name}");
                    foreach (Room room in channel.Roomlist.Roomlist)
                    {
                        Debug.Write($", Room {room.Name}");
                    }
                    foreach (Function function in channel.Functionlist.Functionlist)
                    {
                        Debug.Write($", Function {function.Name}");
                    }
                    Debug.Write("\n");
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        Debug.WriteLine($"        Datapoint {datapoint.Name}");
                    }
                }
            }*/
        }

        private async Task Frage_ob_Online()
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

        public async Task ReadXDocument(string page, string xml)
        {
            //await Frage_ob_Online();
            XDocument xdoc = null;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            // C:\Users\Gerhard\AppData\Local\Packages\7e54ccaa-a3c0-48e3-8ded-b0c43979c189_b0ckz6vx689s4\LocalState
            try
            {
                try
                {
                    if (online == true)
                    {
                        // Daten von der HomeMatic laden
                        Uri uri = new Uri("http://" + HMIP + ":" + HMPO.ToString() + "/" + page);
                        HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync() as HttpWebResponse;
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                        xdoc = XDocument.Load(reader);
                        xdoc.Save(localFolder.Path + "/" + xml); // TODO
                    }
                    else
                    {
                        // Lokales File laden
                        xdoc = XDocument.Load(localFolder.Path + "/" + xml);
                        // Für Offline Modus FakeFile in xdoc schreiben
                        if (xml == "statechange.xml")
                            xdoc = GenerateOffLineDocument();
                    }
                }
                catch (WebException)
                {
                    // Fehler bei der Onlinevervindung ... lade Chache
                    await Hinweis_auf_Offline();
                    xdoc = XDocument.Load(localFolder.Path + "/" + xml);
                    // Für Offline Modus FakeFile in xdoc schreiben
                    if (xml == "statechange.xml")
                        xdoc = GenerateOffLineDocument();
                }
                catch (Exception ex)
                {
                    // Anderer Fehler
                    Debug.WriteLine("ReadXDocument " + ex.Message.ToString());
                }
                MainPage.Devicelist.LoadXDocument(xdoc);
                Debug.WriteLine($"ReadXDocument {xml} fertig");
            }
            catch (Exception ex)
            {
                // Fehler im inneren Catch Zweig
                Debug.WriteLine("ReadXDocument " + ex.Message.ToString());
            }
        }

        private XDocument GenerateOffLineDocument()
        {
            //string xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><result><changed id=\"XXXXX\" new_value=\"z.B.False\"/></result>";
            //CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US"); // Punkt als Komma
            CultureInfo culture = new CultureInfo("en-US"); // Punkt als Komma
            string xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><result><changed id=\"";
            xml += _NewId.ToString();
            xml += "\" new_value=\"";
            if (_NewValue == Double.PositiveInfinity)
                xml += "True";
            else if (_NewValue == Double.NegativeInfinity)
                xml += "False";
            else
                xml += _NewValue.ToString("F", culture);
            xml += "\"/></result>";
            XDocument xdoc = XDocument.Parse(xml);
            return xdoc;
        }

        private async Task Hinweis_auf_Offline()
        {
            MessageDialog showDialog = new MessageDialog("Lade den Chache von der letzten Verbindung [OK]", "Konnte keine Online Verbindung mit HomeMatic herstellen!");
            showDialog.Commands.Add(new UICommand("OK") { Id = 0 });
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
                online = false;
        }

    }
}
