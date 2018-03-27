using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace Hausautomation.Model
{
    public class DeviceList
    {
        public ObservableCollection<Device> Devicelist { get; set; }

        public RoomList Roomlist { get; set; } // diese Liste ist redundant aber hilfreich

        public FunctionList Functionlist { get; set; } // diese Liste ist redundant aber hilfreich

        public IEnumerable<Device> Favoriten
        {
            get { return Devicelist.Where(item => item.bFavoriten); }
        }

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

        public void PrepareAllDevicesIntheList()
        {
            foreach (Device device in Devicelist)
            {
                device.PrepareAllDevices();
                device.NotifyPropertyChanged("bSwitch1State");
                device.NotifyPropertyChanged("bSwitch2State");
                device.NotifyPropertyChanged("iSlider1");
            }
        }

        public static double DateTimeToUnixTimeStamp(DateTime date)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return (date - dtDateTime).TotalSeconds;
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
            // result
            foreach (XElement element in xDocument.Descendants("result"))
            {
                //Debug.WriteLine(element);
                // Einpflegen
                //MessageDialog showDialog = new MessageDialog(element.ToString(), "Schaltbefehl ausgeführt!");
                //var result = showDialog.ShowAsync();
                //bool ok = int.TryParse(element.Attribute("changed id").Value.ToString(), out int changed_id);
                // funktioniert nicht weil keine Leerzeichen erlaubt sind => XML-API hält sich nicht an Standard
                // daher selber parsen
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US"); // Punkt als Komma
                string str = element.Document.ToString(); // "<result>\r\n  <changed id=\"19073\" new_value=\"0.79\" />\r\n</result>"
                int index1 = str.IndexOf("changed id=\"") + 12;
                int index2 = str.IndexOf("\"", index1);
                int changed_id = int.Parse(str.Substring(index1, index2 - index1));
                index1 = str.IndexOf("new_value=\"") + 11;
                index2 = str.IndexOf("\"", index1);
                double new_value;
                if (str.Substring(index1, index2 - index1).Contains("True") == true)
                    new_value = Double.PositiveInfinity;
                else if (str.Substring(index1, index2 - index1).Contains("False") == true)
                    new_value = Double.NegativeInfinity;
                else
                    new_value = double.Parse(str.Substring(index1, index2 - index1), culture);
                foreach (Device device in Devicelist)
                {
                    foreach (Channel channel in device.Channellist.Channellist)
                    {
                        foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                        {
                            if (datapoint.Ise_id == changed_id)
                            {
                                //Debug.WriteLine(datapoint.Value.ToString());
                                datapoint.Value = new_value;
                                datapoint.Timestamp = DateTime.Now;
                                device.PrepareAllDevices(); // Update Device => Es sollte der Timer Thread in ca. 2 Sekunden gestartet werden damit die Updates von Strom Spannung etc. aktualisiert werden
                                //Thread.Sleep(2000);
                                device.bSliderThreadActive = false;
                                device.StateChange(null);
                                return;
                            }
                        }
                    }
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

    public class Device : INotifyPropertyChanged
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

        // Ab hier die Properties welche nur für die Anzeige in der View (Databinding) benötigt werden
        public string Textblock1 { get { return _textblock1; } set { _textblock1 = value; NotifyPropertyChanged(); } } // Inhalt des Textblock1
        public string Textblock2 { get { return _textblock2; } set { _textblock2 = value; NotifyPropertyChanged(); } }
        public string Textblock3 { get { return _textblock3; } set { _textblock3 = value; NotifyPropertyChanged(); } }
        public string Textblock4 { get { return _textblock4; } set { _textblock4 = value; NotifyPropertyChanged(); } }
        public string Textblock5 { get { return _textblock5; } set { _textblock5 = value; NotifyPropertyChanged(); } }
        public string Textblock6 { get { return _textblock6; } set { _textblock6 = value; NotifyPropertyChanged(); } }
        public bool bSlider1 { get; set; } // sichtbarkeit Slider
        public bool bButton1 { get; set; } // sichtbarkeit Ein Aus
        public bool bButton2 { get; set; } // sichtbarkeit Ob Un Li Re
        public bool bSwitch1 { get; set; } // sichtbarkeit Schalter 1
        public bool bSwitch2 { get; set; } // sichtbarkeit Schalter 2
        public bool bSwitch1State
        {
            get { return _bSwitch1State; }
            set
            {
                if (value != _bSwitch1State)
                {
                    _bSwitch1State = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Switch1State");
                }
            }
        } // Status des Switch1
        public bool Switch1State
        {
            get { return _bSwitch1State; }
            set
            {
                if (value != _bSwitch1State)
                {
                    _bSwitch1State = value;
                    StateChange(value);
                }
            }
        } // Status des Switch1
        public bool bSwitch2State
        {
            get { return _bSwitch2State; }
            set
            {
                _bSwitch2State = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Switch2State");
            }
        } // Status des Switch2
        public bool Switch2State
        {
            get { return _bSwitch2State; }
            set
            {
                _bSwitch2State = value;
                StateChange2(value);
            }
        } // Status des Switch2
        public bool bTextblock2 { get; set; } // sichtbarkeit
        public bool bTextblock3 { get; set; } // sichtbarkeit
        public bool bTextblock4 { get; set; } // sichtbarkeit
        public bool bTextblock5 { get; set; } // sichtbarkeit
        public bool bTextblock6 { get; set; } // sichtbarkeit
        public bool bFavoriten { get { return _bfavoriten; } set { _bfavoriten = value; NotifyPropertyChanged(); } } // Favoriten
        public int iSlider1 { get { return _iSlider1; } set { _iSlider1 = value; /*NotifyPropertyChanged();*/ } } // Wert des Slider
        public int iChannel { get { return _iChannel; } set { _iChannel = value; NotifyPropertyChanged(); } } // Welcher Kanal wird bei HM-PB-4Dis-WM angezeigt
        public int iStateChangeID { get; set; } // ise_id wird für StateChange benötigt
        public int iStateChangeID2 { get; set; } // ise_id wird für StateChange benötigt
        private static List<BitmapImage> sources; // Images der Devices
        private string _textblock1;
        private string _textblock2;
        private string _textblock3;
        private string _textblock4;
        private string _textblock5;
        private string _textblock6;
        private bool _bSwitch1State;
        private bool _bSwitch2State;
        private bool _bfavoriten;
        private int _iSlider1;
        private int _iChannel;
        private double? dLastSliderStatus; // Letzen Slider Wert merken
        public bool bSliderThreadActive = false; // Thread not active
        private long lLastSliderMilliseconds; // Ticks seit der letzten Slideränderung

        public BitmapImage Image { get; set; }
        #endregion

        #region Konstruktor
        public Device()
        {
            Channellist = new ChannelList();
            Image = new BitmapImage();
            sources = new List<BitmapImage>()
            {
                new BitmapImage(new Uri("ms-appx:///Assets/HM-Cen-O-TW-x-x-2.png")),    // 0
                new BitmapImage(new Uri("ms-appx:///Assets/HM-Sec-MDIR-3.png")),        // 1
                new BitmapImage(new Uri("ms-appx:///Assets/HM-Sec-SCo.png")),           // 2
                new BitmapImage(new Uri("ms-appx:///Assets/HM-LC-Sw1PBU-FM.jpg")),      // 3 
                new BitmapImage(new Uri("ms-appx:///Assets/HM-LC-Sw1-FM.png")),         // 4
                new BitmapImage(new Uri("ms-appx:///Assets/HM-LC-Sw2-FM.png")),         // 5        
                new BitmapImage(new Uri("ms-appx:///Assets/HM-PB-4Dis-WM.jpg")),        // 6
                new BitmapImage(new Uri("ms-appx:///Assets/HM-PB-2-WM55-2.jpg")),       // 7
                new BitmapImage(new Uri("ms-appx:///Assets/HM-ES-PMSw1-Pl-DN-R1.png")), // 8 
                new BitmapImage(new Uri("ms-appx:///Assets/HMIP-PSM.png")),             // 9 
                new BitmapImage(new Uri("ms-appx:///Assets/HmIP-BROLL.jpg")),           // 10
                new BitmapImage(new Uri("ms-appx:///Assets/HMIP-WRC2.jpg")),            // 11
                new BitmapImage(new Uri("ms-appx:///Assets/HM-OU-LED16.png")),          // 12
                new BitmapImage(new Uri("ms-appx:///Assets/HM-RC-Key4-2.jpg")),          // 12
            };
            Image = sources[0]; // Zentrale hat kein "device_type"
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
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
                    Image = sources[3];
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
                    Image = sources[1];
                    break;
                case "HM-Sec-SCo":
                    Image = sources[2];
                    break;
                case "HmIP-BROLL":
                    Image = sources[10];
                    break;
                case "HmIP-BSM":
                    Image = sources[3];
                    break;
                case "HMIP-PSM":
                    Image = sources[9];
                    break;
                case "HMIP-WRC2":
                    Image = sources[11];
                    break;
                case "HM-OU-LED16":
                    Image = sources[12];
                    break;
                case "HM-RC-Key4-2":
                    Image = sources[13];
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        // Für alle Devices gleich
        public void PrepareAllDevices()
        {
            PrepareTextblock1();
            switch (Device_type)
            {
                case "HM-ES-PMSw1-Pl-DN-R1":
                    PrepareHMESPMSw1PlDNR1();
                    break;
                case "HM-LC-Sw1PBU-FM":
                    PrepareHMLCSw1PBUFM();
                    break;
                case "HM-LC-Sw1-FM":
                    PrepareHMLCSw1FM();
                    break;
                case "HM-LC-Sw2-FM":
                    PrepareHMLCSw2FM();
                    break;
                case "HM-PB-2-WM55-2":
                    PrepareHMPB2WM552();
                    break;
                case "HM-PB-4Dis-WM":
                    PrepareHMPB4DisWM();
                    break;
                case "HM-Sec-MDIR-3":
                    PrepareHMSecMDIR3();
                    break;
                case "HM-Sec-SCo":
                    PrepareHMSecSCo();
                    break;
                case "HmIP-BROLL":
                    PrepareHmIPBROLL();
                    break;
                case "HmIP-BSM":
                    PrepareHmIPBSM();
                    break;
                case "HMIP-PSM":
                    PrepareHMIPPSM();
                    break;
                case "HMIP-WRC2":
                    PrepareHMIPWRC2();
                    break;
                case "HM-OU-LED16":
                    break;
                case "HM-RC-Key4-2":
                    break;
                case null: // HM-RCV-50 Zentrale
                    break;
                default:
                    break;
                    //throw new NotImplementedException();
            }
        }

        public void PrepareTextblock1()
        {
            Textblock1 = "ID: " + Ise_id.ToString();
            if (Address != null)
                Textblock1 += "\nSNr.: " + Address;
            if (Device_type != null)
                Textblock1 += "\nName: " + Device_type;
            else
                Textblock1 += "\nName: HM-RCV-50"; // Zentrale
            string strRoom = "";
            string strFunc = "";
            string strData = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                foreach (Room room in channel.Roomlist.Roomlist)
                {
                    if (strRoom.Contains(room.Name) == false)
                        strRoom += "\nRaum: " + room.Name;
                }
                foreach (Function function in channel.Functionlist.Functionlist)
                {
                    if (strFunc.Contains(function.Name) == false)
                        strFunc += "\nFunktion: " + function.Name;
                }
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "RSSI_DEVICE")
                        {
                            if (datapoint.Value > 100)
                            {
                                strData += "\nRSSI Gerät: " + datapoint.Value.ToString();
                                //strData += "\n" + datapoint.Timestamp.ToString();
                            }
                        }
                        if (datapoint.Type == "RSSI_PEER")
                        {
                            if (datapoint.Value > 100)
                            {
                                strData += "\nRSSI Zentrale: " + datapoint.Value.ToString();
                                //strData += "\n" + datapoint.Timestamp.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            Textblock1 += strRoom + strFunc + strData;
        }

        public void PrepareHMESPMSw1PlDNR1()
        {
            bSwitch1 = true;
            bTextblock4 = true;
            string strvo = "";
            string strcu = "";
            string strfr = "";
            string strpo = "";
            string stren = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            if (datapoint.Value == double.NegativeInfinity)
                                bSwitch1State = false;
                            else if (datapoint.Value == double.PositiveInfinity)
                                bSwitch1State = true;
                            Textblock4 = "Letzer Schaltbefehl:\n";
                            Textblock4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 2)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "VOLTAGE")
                        {
                            strvo += "\nLetze Messwerte:\n";
                            strvo += datapoint.Timestamp.ToString();
                            strvo += "\nSpannung: ";
                            strvo += datapoint.Value.ToString();
                            strvo += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "CURRENT")
                        {
                            strcu += "\nStrom: ";
                            strcu += datapoint.Value.ToString();
                            strcu += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "FREQUENCY")
                        {
                            strfr += "\nFrequenz: ";
                            strfr += datapoint.Value.ToString();
                            strfr += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "POWER")
                        {
                            strpo += "\nLeistung: ";
                            strpo += datapoint.Value.ToString();
                            strpo += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "ENERGY_COUNTER")
                        {
                            stren += "\nEnergieverb.: ";
                            stren += datapoint.Value.ToString("F");
                            stren += " " + datapoint.Valueunit.ToString();
                        }
                    }
                }
            }
            Textblock4 += strvo + strcu + strfr + strpo + stren;
        }

        public void PrepareHMLCSw1PBUFM()
        {
            bSwitch1 = true;
            bTextblock4 = true;
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "RSSI_PEER")
                        {
                            Textblock4 = "Letze Aktualisierung:\n";
                            Textblock4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            if (datapoint.Value == double.NegativeInfinity)
                                bSwitch1State = false;
                            if (datapoint.Value == double.PositiveInfinity)
                                bSwitch1State = true;
                            Textblock4 += "\nLetzer Schaltbefehl:\n";
                            Textblock4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
            }
        }

        public void PrepareHMLCSw1FM()
        {
            bSwitch1 = true;
            bTextblock4 = true;
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "RSSI_PEER")
                        {
                            Textblock4 = "Letze Aktualisierung:\n";
                            Textblock4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            if (datapoint.Value == double.NegativeInfinity)
                                bSwitch1State = false;
                            if (datapoint.Value == double.PositiveInfinity)
                                bSwitch1State = true;
                            Textblock4 += "\nLetzer Schaltbefehl:\n";
                            Textblock4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
            }
        }

        public void PrepareHMLCSw2FM()
        {
            bSwitch1 = true;
            bSwitch2 = true;
            bTextblock5 = true;
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "RSSI_PEER")
                        {
                            Textblock5 = "Letze Aktualisierung:\n";
                            Textblock5 += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            if (datapoint.Value == double.NegativeInfinity)
                                bSwitch1State = false;
                            if (datapoint.Value == double.PositiveInfinity)
                                bSwitch1State = true;
                            Textblock5 += "\nLetzer Schaltbefehl 1:\n";
                            Textblock5 += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 2)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            iStateChangeID2 = datapoint.Ise_id;
                            if (datapoint.Value == double.NegativeInfinity)
                                bSwitch2State = false;
                            if (datapoint.Value == double.PositiveInfinity)
                                bSwitch2State = true;
                            Textblock5 += "\nLetzer Schaltbefehl 2:\n";
                            Textblock5 += datapoint.Timestamp.ToString();
                        }
                    }
                }
            }
        }

        public void PrepareHMPB2WM552()
        {
            bButton1 = true;
            bTextblock3 = true;
            string strein = "";
            string straus = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "LOWBAT")
                        {
                            Textblock3 = "Letze Aktualisierung:\n";
                            Textblock3 += datapoint.Timestamp.ToString();
                            Textblock3 += "\nBatterie: ";
                            if (datapoint.Value == double.NegativeInfinity)
                                Textblock3 += "OK";
                            else if (datapoint.Value == double.PositiveInfinity)
                                Textblock3 += "tauschen!";
                        }
                    }
                }
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "PRESS_LONG")
                        {
                        }
                        if (datapoint.Type == "PRESS_SHORT")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            straus = "\nAus: ";
                            straus += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 2)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "PRESS_LONG")
                        {
                        }
                        if (datapoint.Type == "PRESS_SHORT")
                        {
                            iStateChangeID2 = datapoint.Ise_id;
                            strein = "\nLetzter Tastendruck:\nEin: ";
                            strein += datapoint.Timestamp.ToString();
                        }
                    }
                }
            }
            Textblock3 += strein + straus;
        }

        public void PrepareHMPB4DisWM(bool? bnext = null)
        {
            bButton2 = true;
            bTextblock6 = true;
            string strein = "";
            string straus = "";
            if (bnext != null)
            {
                if (bnext == true && iChannel < 9)
                    iChannel++;
                else if (bnext == false && iChannel > 0)
                    iChannel--;
            }
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "LOWBAT")
                        {
                            Textblock6 = "Letze Aktualisierung:\n";
                            Textblock6 += datapoint.Timestamp.ToString();
                            Textblock6 += "\nBatterie: ";
                            if (datapoint.Value == double.NegativeInfinity)
                                Textblock6 += "OK";
                            else if (datapoint.Value == double.PositiveInfinity)
                                Textblock6 += "tauschen!";
                        }
                    }
                }
                //if (channel.Index == 1)
                if (channel.Index == iChannel * 2 + 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "PRESS_LONG")
                        {
                        }
                        if (datapoint.Type == "PRESS_SHORT")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            straus = "\nAus: ";
                            straus += datapoint.Timestamp.ToString();
                        }
                    }
                }
                //if (channel.Index == 2)
                if (channel.Index == iChannel * 2 + 2)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "PRESS_LONG")
                        {
                        }
                        if (datapoint.Type == "PRESS_SHORT")
                        {
                            iStateChangeID2 = datapoint.Ise_id;
                            strein = "\nAnzeige für Kanal: " + (iChannel + 1).ToString();
                            strein += "\nLetzter Tastendruck:\nEin: ";
                            strein += datapoint.Timestamp.ToString();
                        }
                    }
                }
            }
            Textblock6 += strein + straus;
        }

        public void PrepareHMSecSCo()
        {
            bTextblock2 = true;
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            Textblock2 = "Letze Aktualisierung:\n";
                            Textblock2 += datapoint.Timestamp.ToString();
                            Textblock2 += "\nTüre: ";
                            if (datapoint.Value == double.NegativeInfinity)
                                Textblock2 += "geschlossen";
                            else if (datapoint.Value == double.PositiveInfinity)
                                Textblock2 += "offen";
                        }
                        if (datapoint.Type == "ERROR")
                        {
                            Textblock2 += "\nSabotage: ";
                            if (datapoint.Value == 0)
                                Textblock2 += "nein";
                            else if (datapoint.Value != 0)
                                Textblock2 += "ja";
                        }
                        if (datapoint.Type == "LOWBAT")
                        {
                            Textblock2 += "\nBatterie: ";
                            if (datapoint.Value == double.NegativeInfinity)
                                Textblock2 += "OK";
                            else if (datapoint.Value == double.PositiveInfinity)
                                Textblock2 += "tauschen!";
                        }
                    }
                }
            }
        }

        public void PrepareHMSecMDIR3()
        {
            bTextblock2 = true;
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "BRIGHTNESS")
                        {
                            Textblock2 = "Letze Aktualisierung:\n";
                            Textblock2 += datapoint.Timestamp.ToString();
                            Textblock2 += "\nHelligkeit: ";
                            Textblock2 += datapoint.Value.ToString();
                        }
                        if (datapoint.Type == "MOTION")
                        {
                            Textblock2 += "\nBewegung: ";
                            if (datapoint.Value == double.NegativeInfinity)
                                Textblock2 += "keine";
                            else if (datapoint.Value == double.PositiveInfinity)
                                Textblock2 += "erkannt";
                        }
                        if (datapoint.Type == "ERROR")
                        {
                            Textblock2 += "\nSabotage: ";
                            if (datapoint.Value == 0)
                                Textblock2 += "nein";
                            else if (datapoint.Value != 0)
                                Textblock2 += "ja";
                        }
                    }
                }
            }
        }

        public void PrepareHmIPBROLL()
        {
            bSlider1 = true;
            bTextblock2 = true;
            string str0 = "";
            string str4 = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "ACTUAL_TEMPERATURE")
                        {
                            str0 = "\nTemperatur: ";
                            str0 += (datapoint.Value - 4).ToString();
                            str0 += "°C\n" + datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 4)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "LEVEL")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            iSlider1 = (int)(datapoint.Value * 100 + 0.5);
                            str4 = "Letze Aktualisierung:\n";
                            str4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
            }
            Textblock2 = str4 + str0;
        }

        public void PrepareHmIPBSM()
        {
            bSwitch1 = true;
            bTextblock4 = true;
            string strvo = "";
            string strcu = "";
            string strfr = "";
            string strpo = "";
            string stren = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 4)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            if (datapoint.Value == double.NegativeInfinity)
                                bSwitch1State = false;
                            if (datapoint.Value == double.PositiveInfinity)
                                bSwitch1State = true;
                            Textblock4 = "Letze Aktualisierung:\n";
                            Textblock4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 7)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "VOLTAGE")
                        {
                            strvo += "\nSpannung: ";
                            strvo += datapoint.Value.ToString();
                            strvo += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "CURRENT")
                        {
                            strcu += "\nStrom: ";
                            strcu += datapoint.Value.ToString();
                            strcu += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "FREQUENCY")
                        {
                            strfr += "\nFrequenz: ";
                            strfr += datapoint.Value.ToString();
                            strfr += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "POWER")
                        {
                            strpo += "\nLeistung: ";
                            strpo += datapoint.Value.ToString();
                            strpo += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "ENERGY_COUNTER")
                        {
                            stren += "\nEnergieverb.: ";
                            stren += datapoint.Value.ToString("F");
                            stren += " " + datapoint.Valueunit.ToString();
                        }
                    }
                }
            }
            Textblock4 += strvo + strcu + strfr + strpo + stren;
        }

        public void PrepareHMIPPSM()
        {
            bSwitch1 = true;
            bTextblock4 = true;
            string strvo = "";
            string strcu = "";
            string strfr = "";
            string strpo = "";
            string stren = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 3)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "STATE")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            if (datapoint.Value == double.NegativeInfinity)
                                bSwitch1State = false;
                            if (datapoint.Value == double.PositiveInfinity)
                                bSwitch1State = true;
                            Textblock4 = "Letze Aktualisierung:\n";
                            Textblock4 += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 6)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "VOLTAGE")
                        {
                            strvo += "\nSpannung: ";
                            strvo += datapoint.Value.ToString();
                            strvo += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "CURRENT")
                        {
                            strcu += "\nStrom: ";
                            strcu += datapoint.Value.ToString();
                            strcu += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "FREQUENCY")
                        {
                            strfr += "\nFrequenz: ";
                            strfr += datapoint.Value.ToString();
                            strfr += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "POWER")
                        {
                            strpo += "\nLeistung: ";
                            strpo += datapoint.Value.ToString();
                            strpo += " " + datapoint.Valueunit.ToString();
                        }
                        if (datapoint.Type == "ENERGY_COUNTER")
                        {
                            stren += "\nEnergieverb.: ";
                            stren += datapoint.Value.ToString("F");
                            stren += " " + datapoint.Valueunit.ToString();
                        }
                    }
                }
            }
            Textblock4 += strvo + strcu + strfr + strpo + stren;
        }

        public void PrepareHMIPWRC2()
        {
            bButton1 = true;
            bTextblock3 = true;
            string strein = "";
            string straus = "";
            foreach (Channel channel in Channellist.Channellist)
            {
                if (channel.Index == 0)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "LOW_BAT")
                        {
                            Textblock3 = "Letze Aktualisierung:\n";
                            Textblock3 += datapoint.Timestamp.ToString();
                            Textblock3 += "\nBatterie: ";
                            if (datapoint.Value == double.NegativeInfinity)
                                Textblock3 += "OK";
                            else if (datapoint.Value == double.PositiveInfinity)
                                Textblock3 += "tauschen!";
                        }
                        if (datapoint.Type == "OPERATING_VOLTAGE")
                        {
                            Textblock3 += "\nSpannung: ";
                            Textblock3 += datapoint.Value.ToString();
                            Textblock3 += "V";
                        }
                    }
                }
                if (channel.Index == 1)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "PRESS_LONG")
                        {
                        }
                        if (datapoint.Type == "PRESS_SHORT")
                        {
                            iStateChangeID = datapoint.Ise_id;
                            straus = "\nAus: ";
                            straus += datapoint.Timestamp.ToString();
                        }
                    }
                }
                if (channel.Index == 2)
                {
                    foreach (Datapoint datapoint in channel.Datapointlist.Datapointlist)
                    {
                        if (datapoint.Type == "PRESS_LONG")
                        {
                        }
                        if (datapoint.Type == "PRESS_SHORT")
                        {
                            iStateChangeID2 = datapoint.Ise_id;
                            strein = "\nLetzter Tastendruck:\nEin: ";
                            strein += datapoint.Timestamp.ToString();
                        }
                    }
                }
            }
            Textblock3 += strein + straus;
        }

        public void StateChange(bool bStatus)
        {
            ReadXDoc readXDoc = new ReadXDoc();
            readXDoc.NewId = iStateChangeID;
            readXDoc.NewValue = bStatus ? Double.PositiveInfinity : Double.NegativeInfinity;
            readXDoc.ReadStateChangeXDoc();
        }

        public void StateChange2(bool bStatus)
        {
            ReadXDoc readXDoc = new ReadXDoc();
            readXDoc.NewId = iStateChangeID2;
            readXDoc.NewValue = bStatus ? Double.PositiveInfinity : Double.NegativeInfinity;
            readXDoc.ReadStateChangeXDoc();
        }

        public void StateChange(double? dStatus)
        {
            if (dStatus != null)
                dLastSliderStatus = dStatus;
            //if (bSliderThreadActive == false && dLastSliderStatus != null)
            if (DateTime.Now.Ticks - lLastSliderMilliseconds > 20000000 && dLastSliderStatus != null)
            {
                //bSliderThreadActive = true;
                lLastSliderMilliseconds = DateTime.Now.Ticks;
                ReadXDoc readXDoc = new ReadXDoc();
                readXDoc.NewId = iStateChangeID;
                readXDoc.NewValue = (double)dStatus / 100;
                dLastSliderStatus = null;
                //Thread thread = new Thread(readXDoc.ReadStateChangeXDoc); // Absturz bei PropertyChanged() weil anderer Thread
                //thread.Start();
                readXDoc.ReadStateChangeXDoc();
            }

        }
        #endregion

    }
}
