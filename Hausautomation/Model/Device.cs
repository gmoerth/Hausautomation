using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Hausautomation.Model
{
    public class DeviceList
    {
        public List<Device> Devicelist { get; set; }

        public DeviceList()
        {
            Devicelist = new List<Device>();
        }

        public Device GetDevice(int ise_id)
        {
            foreach (Device device in Devicelist)
                if (device.Ise_id == ise_id)
                    return device;
            return null;
            //throw new IndexOutOfRangeException();
        }

        public void SetDevice(int ise_id, Device device)
        {
            throw new NotImplementedException();
        }
    }

    public class Device
    {
        #region Properties
        /*private List<Channel> channellist;

        public List<Channel> Channellist
        {
            get { return channellist; }
            set { channellist = value; }
        }*/
        public ChannelList Channellist;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private int ise_id;

        public int Ise_id
        {
            get { return ise_id; }
            set { ise_id = value; }
        }
        private string _interface;

        public string Interface
        {
            get { return _interface; }
            set { _interface = value; }
        }
        private string device_type;

        public string Device_type
        {
            get { return device_type; }
            set { device_type = value; }
        }
        private bool ready_config;

        public bool Ready_config
        {
            get { return ready_config; }
            set { ready_config = value; }
        }
        private bool config_pending;

        public bool Config_pending
        {
            get { return config_pending; }
            set { config_pending = value; }
        }
        private bool sticky_unreach;

        public bool Sticky_unreach
        {
            get { return sticky_unreach; }
            set { sticky_unreach = value; }
        }
        private bool unreach;

        public bool Unreach
        {
            get { return unreach; }
            set { unreach = value; }
        }
        #endregion

        public Device(string name, string address, int ise_id, string @interface, string device_type, bool ready_config, bool config_pending, bool sticky_unreach, bool unreach)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.address = address ?? throw new ArgumentNullException(nameof(address));
            this.ise_id = ise_id;
            _interface = @interface ?? throw new ArgumentNullException(nameof(@interface));
            this.device_type = device_type ?? throw new ArgumentNullException(nameof(device_type));
            this.ready_config = ready_config;
            this.config_pending = config_pending;
            this.sticky_unreach = sticky_unreach;
            this.unreach = unreach;
        }

        public Device()
        {
            //Channellist = new List<Channel>();
            Channellist = new ChannelList();
        }

        public void ParseDevicelist(XElement xElement)
        {
            // Device parsen
            foreach(XAttribute xattribute in xElement.Attributes())
            {
                //Debug.WriteLine(xattribute);
                switch (xattribute.Name.ToString())
                {
                    case "name":
                        Name = xattribute.Value;
                        break;
                    case "address":
                        Address = xattribute.Value;
                        break;
                    case "ise_id":
                        int.TryParse(xattribute.Value, out int id);
                        Ise_id = id;
                        break;
                    case "interface":
                        Interface = xattribute.Value;
                        break;
                    case "device_type":
                        Device_type = xattribute.Value;
                        break;
                    case "ready_config":
                        bool.TryParse(xattribute.Value, out bool rc);
                        Ready_config = rc;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            // Channel parsen
            foreach(XNode xnode in xElement.Nodes())
            {
                //Debug.WriteLine(xnode);
                Channel channel = new Channel();
                channel.ParseDevicelist(xnode);
                // hinzufügen zur Liste
                Channellist.Channellist.Add(channel);
            }
        }

        public void ParseStatelist(XElement xElement)
        {
            // Device parsen
            foreach (XAttribute xattribute in xElement.Attributes())
            {
                //Debug.WriteLine(xattribute);
                switch (xattribute.Name.ToString())
                {
                    case "name":
                        Name = xattribute.Value;
                        break;
                    case "ise_id":
                        int.TryParse(xattribute.Value, out int id);
                        Ise_id = id;
                        break;
                    case "config_pending":
                        bool.TryParse(xattribute.Value, out bool cp);
                        Config_pending = cp;
                        break;
                    case "sticky_unreach":
                        bool.TryParse(xattribute.Value, out bool su);
                        Sticky_unreach = su;
                        break;
                    case "unreach":
                        bool.TryParse(xattribute.Value, out bool un);
                        Unreach = un;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            // Channel parsen
            foreach (XNode xnode in xElement.Nodes())
            {
                //Debug.WriteLine(xnode);
                XElement element = Channel.ToXElement(xnode);
                bool ok = int.TryParse(element.Attribute("ise_id").Value.ToString(), out int ise_id);
                Channel channel = Channellist.GetChannel(ise_id);
                if (channel != null)
                {
                    channel.ParseStatelist(xnode);
                }
                else
                {
                    channel = new Channel();
                    channel.ParseStatelist(xnode);
                    // hinzufügen zur Liste
                    Channellist.Channellist.Add(channel);
                }
            }

        }

    }
}
