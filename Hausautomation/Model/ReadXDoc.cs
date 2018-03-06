using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class ReadXDoc
    {


        public async Task<string> SeiteEinlesen(string ip, int port, string page)
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

                foreach (XElement element in xdoc.Descendants("device")/*.Descendants("channel")*/)
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
        }



    }
}
