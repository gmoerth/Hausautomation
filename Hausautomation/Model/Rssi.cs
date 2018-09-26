using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class RssiList
    {
        public List<Rssi> Rssilist { get; set; }

        public RssiList()
        {
            Rssilist = new List<Rssi>();
        }

        public Rssi GetRssi(string device)
        {
            foreach (Rssi rssi in Rssilist)
                if (rssi.Device == device)
                    return rssi;
            return null;
        }

        public void AddRssi(Rssi rssi)
        {
            Rssilist.Add(rssi);
        }
    }

    public class Rssi
    {
        #region Properties
        private string device;

        public string Device
        {
            get { return device; }
            set { device = value; }
        }

        private int rx;

        public int Rx
        {
            get { return rx; }
            set { rx = value; }
        }
        private int tx;

        public int Tx
        {
            get { return tx; }
            set { tx = value; }
        }
        #endregion

        #region Methoden
        public void Parse(XElement xElement)
        {
            // Room parsen
            foreach (XAttribute xattribute in xElement.Attributes())
            {
                //Debug.WriteLine(xattribute);
                switch (xattribute.Name.ToString())
                {
                    case "device":
                        Device = xattribute.Value;
                        break;
                    case "rx":
                        int.TryParse(xattribute.Value, out int rxvalue);
                        Rx = rxvalue;
                        break;
                    case "tx":
                        int.TryParse(xattribute.Value, out int txvalue);
                        Tx = txvalue;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}
