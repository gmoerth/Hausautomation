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
        DeviceList devicelist;

        public ReadXDoc(DeviceList devicelist)
        {
            this.devicelist = devicelist;
            HMIP = "http://192.168.178.15";
            HMPO = 80;
#pragma warning disable 4014
            SeiteEinlesen("addons/xmlapi/devicelist.cgi");
            //SeiteEinlesen2("addons/xmlapi/statelist.cgi");
#pragma warning restore 4014
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

                // XML File parsen
                foreach (XElement element in xdoc.Descendants("device")/*.Descendants("channel")*/)
                {
                    //Debug.WriteLine(element);
                    // Device parsen
                    Device device = new Device();
                    device.ParseDevicelist(element);
                    // hinzufügen zur Liste
                    devicelist.Devicelist.Add(device);
                }

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

                // XML File parsen
                foreach (XElement element in xdoc.Descendants("device")/*.Descendants("channel")*/)
                {
                    //Debug.WriteLine(element);
                    // Device parsen
                    bool ok = int.TryParse(element.Attribute("ise_id").Value.ToString(), out int ise_id);
                    Device device = devicelist.GetDevice(ise_id);
                    if(device != null)
                        device.ParseStatelist(element);
                    else
                    {
                        device = new Device();
                        device.ParseStatelist(element);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SeiteEinlesen2 " + ex.Message.ToString());
            }
        }


    }
}
