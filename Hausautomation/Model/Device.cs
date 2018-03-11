using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class DeviceList
    {
        public List<Device> Devicelist { get; set; }

        public RoomList Roomlist { get; set; } // diese Liste ist eigentlich redundant aber hilfreich

        public DeviceList()
        {
            Devicelist = new List<Device>();
            Roomlist = new RoomList();
        }

        public Device GetDevice(int ise_id)
        {
            foreach (Device device in Devicelist)
                if (device.Ise_id == ise_id)
                    return device;
            return null;
        }

        public void AddDevice(Device device)
        {
            Devicelist.Add(device);
        }

        public void LoadXDocument(XDocument xDocument)
        {
            // device parsen
            foreach (XElement element in xDocument.Descendants("device")/*.Descendants("channel")*/)
            {
                //Debug.WriteLine(element);
                bool ok = int.TryParse(element.Attribute("ise_id").Value.ToString(), out int ise_id);
                Device device = GetDevice(ise_id);
                if (device != null)
                {
                    //Debug.WriteLine($"GetDevice - Parse\n{element}");
                    device.Parse(element);
                }
                else
                {
                    //Debug.WriteLine($"AddDevice - Parse\n{element}");
                    device = new Device();
                    device.Parse(element);
                    AddDevice(device);
                }
            }
            // room parsen
            foreach (XElement element in xDocument.Descendants("room"))
            {
                //Debug.WriteLine(element);
                bool ok = int.TryParse(element.Attribute("ise_id").Value.ToString(), out int ise_id);
                Room room = Roomlist.GetRoom(ise_id);
                if (room != null)
                {
                    //Debug.WriteLine($"GetRoom - Parse\n{element}");
                    room.Parse(element);
                    InsertRoomInChannel(element);
                }
                else
                {
                    //Debug.WriteLine($"AddRoom - Parse\n{element}");
                    room = new Room();
                    room.Parse(element);
                    InsertRoomInChannel(element);
                    Roomlist.AddRoom(room);

                }
            }
        }

        public void InsertRoomInChannel(XElement xElement)
        {
            // Room Name zu Channel hinzufügen
            foreach (XNode xnode in xElement.Nodes())
            {
                //Debug.WriteLine(xnode);
                XElement xNodeElement = Channel.ToXElement(xnode);
                if (xNodeElement == null)
                    throw new InvalidOperationException();
                bool ok = int.TryParse(xNodeElement.Attribute("ise_id").Value.ToString(), out int ise_id);
                foreach (Device device in Devicelist)
                    foreach (Channel channel in device.Channellist.Channellist)
                        if (channel.Ise_id == ise_id)
                        {
                            channel.Room = xElement.Attribute("name").Value.ToString();
                            Debug.WriteLine($"{channel.Name} {channel.Room}");
                        }
            }

        }
    }

    public class Device
    {
        #region Properties
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

        #region Konstruktoren
        public Device()
        {
            Channellist = new ChannelList();
        }

        public Device(ChannelList channellist, string name, string address, int ise_id, string @interface, string device_type, bool ready_config, bool config_pending, bool sticky_unreach, bool unreach)
        {
            Channellist = channellist ?? throw new ArgumentNullException(nameof(channellist));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Ise_id = ise_id;
            Interface = @interface ?? throw new ArgumentNullException(nameof(@interface));
            Device_type = device_type ?? throw new ArgumentNullException(nameof(device_type));
            Ready_config = ready_config;
            Config_pending = config_pending;
            Sticky_unreach = sticky_unreach;
            Unreach = unreach;
        }
        #endregion

        #region Methoden
        public void Parse(XElement xElement)
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
                XElement xNodeElement = Channel.ToXElement(xnode);
                if (xNodeElement == null)
                    throw new InvalidOperationException();
                bool ok = int.TryParse(xNodeElement.Attribute("ise_id").Value.ToString(), out int ise_id);
                Channel channel = Channellist.GetChannel(ise_id);
                if (channel != null)
                {
                    channel.Parse(xnode);
                }
                else
                {
                    channel = new Channel();
                    channel.Parse(xnode);
                    // hinzufügen zur Liste
                    Channellist.AddChannel(channel);
                }
            }
        }
        #endregion
    }
}
