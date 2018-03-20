using Hausautomation.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Popups;

namespace Hausautomation.Model
{
    public class ReadXDoc
    {
        public string HMIP { get; set; }
        public int HMPO { get; set; }
        public bool online { get; set; } // Modus zum entwickeln und testen ... geht schneller

        public ReadXDoc()
        {
            if (MainPage.settingsPage != null)
            {
                HMIP = MainPage.settingsPage.xdoc.HMIP;
                HMPO = MainPage.settingsPage.xdoc.HMPO;
                online = MainPage.settingsPage.xdoc.online;
            }
        }

        public void ReadAllXDocuments()
        {
#pragma warning disable 4014
            ReadAllXDocumentsAsync();
#pragma warning restore 4014
        }

        public async Task ReadAllXDocumentsAsync()
        {
            await ReadXDocument("addons/xmlapi/statelist.cgi", "statelist.xml");
            await ReadXDocument("addons/xmlapi/devicelist.cgi", "devicelist.xml");
            await ReadXDocument("addons/xmlapi/roomlist.cgi", "roomlist.xml");
            await ReadXDocument("addons/xmlapi/functionlist.cgi", "functionlist.xml");
            MainPageHeader mph = MainPageHeader.Instance;
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
                        xdoc.Save(localFolder.Path + "/" + xml);
                    }
                    else
                    {
                        // Lokales File laden
                        xdoc = XDocument.Load(localFolder.Path + "/" + xml);
                    }
                }
                catch (WebException)
                {
                    // Fehler bei der Onlinevervindung ... lade Chache
                    await Hinweis_auf_Offline();
                    xdoc = XDocument.Load(localFolder.Path + "/" + xml);
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

        public async Task Hinweis_auf_Offline()
        {
            MessageDialog showDialog = new MessageDialog("Lade den Chache von der letzten Verbindung [OK]", "Konnte keine Online Verbindung mit HomeMatic herstellen!");
            showDialog.Commands.Add(new UICommand("OK") { Id = 0 });
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
                online = false;
        }

    }
}
