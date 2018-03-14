using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Media.Imaging;

namespace Hausautomation.Model
{
    public class DeviceList
    {
        public ObservableCollection<Device> Devicelist { get; set; }

        public RoomList Roomlist { get; set; } // diese Liste ist redundant aber hilfreich

        public FunctionList Functionlist { get; set; } // diese Liste ist redundant aber hilfreich

        public DeviceList()
        {
            Devicelist = new ObservableCollection<Device>();
            Roomlist = new RoomList();
            Functionlist = new FunctionList();
        }

        #region Methoden
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
            // device parsen (devicelist + statelist)
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
            // room parsen (roomlist)
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
            // funtion parsen (functionlist)
            foreach (XElement element in xDocument.Descendants("function"))
            {
                //Debug.WriteLine(element);
                bool ok = int.TryParse(element.Attribute("ise_id").Value.ToString(), out int ise_id);
                Function function = Functionlist.GetFunction(ise_id);
                if (function != null)
                {
                    //Debug.WriteLine($"GetFunction - Parse\n{element}");
                    function.Parse(element);
                    InsertFunctionInChannel(element);
                }
                else
                {
                    //Debug.WriteLine($"AddFunction - Parse\n{element}");
                    function = new Function();
                    function.Parse(element);
                    InsertFunctionInChannel(element);
                    Functionlist.AddFunction(function);
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
                            bool ok2 = int.TryParse(xElement.Attribute("ise_id").Value.ToString(), out int ise_id2);
                            Room room = channel.Roomlist.GetRoom(ise_id2);
                            if (room != null)
                            {
                                //Debug.WriteLine($"{channel.Name} {room.Name}");
                            }
                            else
                            {
                                room = new Room();
                                room.Ise_id = ise_id2;
                                room.Name = xElement.Attribute("name").Value.ToString();
                                channel.Roomlist.AddRoom(room);
                                //Debug.WriteLine($"{channel.Name} {room.Name}");
                            }
                        }
            }

        }

        public void InsertFunctionInChannel(XElement xElement)
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
                            bool ok2 = int.TryParse(xElement.Attribute("ise_id").Value.ToString(), out int ise_id2);
                            Function function = channel.Functionlist.GetFunction(ise_id2);
                            if (function != null)
                            {
                                //Debug.WriteLine($"{channel.Name} {function.Name} {function.Description}");
                            }
                            else
                            {
                                function = new Function();
                                function.Ise_id = ise_id2;
                                function.Name = xElement.Attribute("name").Value.ToString();
                                function.Description = xElement.Attribute("description").Value.ToString();
                                channel.Functionlist.AddFunction(function);
                                //Debug.WriteLine($"{channel.Name} {function.Name} {function.Description}");
                            }
                        }
            }

        }
    }
    #endregion

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

        public string Textblock1 { get; set; }
        public string Textblock2 { get; set; }
        public string Textblock3 { get; set; }
        public string Textblock4 { get; set; }
        public string Textblock5 { get; set; }
        private static List<BitmapImage> sources;
        public BitmapImage Image { get; set; }
        #endregion

        #region Konstruktor
        public Device()
        {
            Channellist = new ChannelList();
            Image = new BitmapImage();
            sources = new List<BitmapImage>()
            {
                new BitmapImage(new Uri("ms-appx:///Assets/CCU2_thumb.png")),               // 0
                new BitmapImage(new Uri("ms-appx:///Assets/112_hmip-wrc2_thumb.png")),      // 1 
                new BitmapImage(new Uri("ms-appx:///Assets/113_hmip-psm_thumb.png")),       // 2 
                new BitmapImage(new Uri("ms-appx:///Assets/124_hm-sec-mdir_thumb.png")),    // 3
                new BitmapImage(new Uri("ms-appx:///Assets/4_hm-lc-sw1-fm_thumb.png")),     // 4
                new BitmapImage(new Uri("ms-appx:///Assets/5_hm-lc-sw2-fm_thumb.png")),     // 5        
                new BitmapImage(new Uri("ms-appx:///Assets/70_hm-pb-4dis-wm_thumb.png")),   // 6
                new BitmapImage(new Uri("ms-appx:///Assets/75_hm-pb-2-wm55_thumb.png")),    // 7
                new BitmapImage(new Uri("ms-appx:///Assets/93_hm-es-pmsw1-pl_thumb.png")),  // 8 
                new BitmapImage(new Uri("ms-appx:///Assets/98_hm-sec-sco_thumb.png")),      // 9
                new BitmapImage(new Uri("ms-appx:///Assets/PushButton-2ch-wm_thumb.png")),  // 10
                new BitmapImage(new Uri("ms-appx:///Assets/HmIP-Wandtaster-V.jpg")),
                new BitmapImage(new Uri("ms-appx:///Assets/HmIP-Schaltsteckdose-V-oS.jpg"))
            };
            Image = sources[0]; // Zentrale hat kein "device_type"
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
                        ParseImage();
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

        private void ParseImage()
        {
            switch (Device_type)
            {
                case "HM-ES-PMSw1-Pl-DN-R1":
                    Image = sources[8];
                    break;
                case "HM-LC-Sw1PBU-FM":
                    Image = sources[10];
                    break;
                case "HM-LC-Sw1-FM":
                    Image = sources[4];
                    break;
                case "HM-LC-Sw2-FM":
                    Image = sources[5];
                    break;
                case "HM-PB-2-WM55-2":
                    Image = sources[7];
                    break;
                case "HM-PB-4Dis-WM":
                    Image = sources[6];
                    break;
                case "HM-Sec-MDIR-3":
                    Image = sources[3];
                    break;
                case "HM-Sec-SCo":
                    Image = sources[9];
                    break;
                case "HmIP-BROLL":
                    Image = sources[10];
                    break;
                case "HmIP-BSM":
                    Image = sources[10];
                    break;
                case "HMIP-PSM":
                    Image = sources[12];//2
                    break;
                case "HMIP-WRC2":
                    Image = sources[11];//1
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        // Für alle Devices gleich
        public void PrepareTextblock1()
        {
            Textblock1 = "ID: " + Ise_id.ToString();
            Textblock1 += "\nName: " + Name;
            string strRoom = "";
            string strFunc = "";
            string strData = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                //Debug.Write($"    Channel {channel.Name}");
                foreach (Room room in channel.Roomlist.Roomlist)
                {
                    //Debug.Write($", Room {room.Name}");
                    if (strRoom.Contains(room.Name) == false)
                        strRoom += "\nRaum: " + room.Name;
                }
                foreach (Function function in channel.Functionlist.Functionlist)
                {
                    //Debug.Write($", Function {function.Name}");
                    if (strFunc.Contains(function.Name) == false)
                        strFunc += "\nFunktion: " + function.Name;
                }
                //Debug.Write("\n");
                foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                {
                    //Debug.WriteLine($"        Datapoint {datapoint.Name}");
                    if (channel.Index == 0 && datapoint.Type == "RSSI_DEVICE")
                        if (datapoint.Value > 100)
                        {
                            strData += "\nRSSI Gerät: " + datapoint.Value.ToString();
                        }
                    if (channel.Index == 0 && datapoint.Type == "RSSI_PEER")
                        if (datapoint.Value > 100)
                        {
                            strData += "\nRSSI Zentrale: " + datapoint.Value.ToString();
                            break;
                        }
                }
            }
            Textblock1 += (strRoom + strFunc + strData);
        }
        #endregion
    }
}
