using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class ChannelList
    {
        public List<Channel> Channellist { get; set; }

        public ChannelList()
        {
            Channellist = new List<Channel>();
        }

        public Channel GetChannel(int ise_id)
        {
            foreach (Channel channel in Channellist)
                if (channel.Ise_id == ise_id)
                    return channel;
            return null;
        }

        public void AddChannel(Channel channel)
        {
            Channellist.Add(channel);
        }
    }

    public class Channel
    {
        #region Properties
        public DatapointList Datapointlist;

        public RoomList Roomlist;

        public FunctionList Functionlist;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
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
        private string direction;

        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        private int parent_device;

        public int Parent_Device
        {
            get { return parent_device; }
            set { parent_device = value; }
        }
        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        private string group_partner;

        public string Group_partner
        {
            get { return group_partner; }
            set { group_partner = value; }
        }
        private bool aes_available;

        public bool Aes_available
        {
            get { return aes_available; }
            set { aes_available = value; }
        }
        private string transmission_mode;

        public string Transmission_mode
        {
            get { return transmission_mode; }
            set { transmission_mode = value; }
        }
        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        private bool ready_config;

        public bool Ready_config
        {
            get { return ready_config; }
            set { ready_config = value; }
        }
        private bool operate;

        public bool Operate
        {
            get { return operate; }
            set { operate = value; }
        }
        #endregion

        #region Konstruktor
        public Channel()
        {
            Datapointlist = new DatapointList();
            Roomlist = new RoomList();
            Functionlist = new FunctionList();
        }
        #endregion

        #region Methoden
        public static XElement ToXElement(XNode xnode)
        {
            return xnode as XElement; // returns null if node is not an XElement
        }

        public void Parse(XNode xnode)
        {
            XElement xElement = ToXElement(xnode);
            if (xElement == null)
                throw new InvalidOperationException();
            // Channel parsen
            foreach (XAttribute xattribute in xElement.Attributes())
            {
                //Debug.WriteLine(xattribute);
                switch (xattribute.Name.ToString())
                {
                    case "name":
                        Name = xattribute.Value;
                        break;
                    case "type":
                        int.TryParse(xattribute.Value, out int type);
                        Type = type;
                        break;
                    case "address":
                        Address = xattribute.Value;
                        break;
                    case "ise_id":
                        int.TryParse(xattribute.Value, out int id);
                        Ise_id = id;
                        break;
                    case "direction":
                        Direction = xattribute.Value;
                        break;
                    case "parent_device":
                        int.TryParse(xattribute.Value, out int pd);
                        Parent_Device = pd;
                        break;
                    case "index":
                        int.TryParse(xattribute.Value, out int ind);
                        Index = ind;
                        break;
                    case "group_partner":
                        Group_partner = xattribute.Value;
                        break;
                    case "aes_available":
                        bool.TryParse(xattribute.Value, out bool aes);
                        Aes_available = aes;
                        break;
                    case "transmission_mode":
                        Transmission_mode = xattribute.Value;
                        break;
                    case "visible":
                        bool.TryParse(xattribute.Value, out bool vis);
                        Visible = vis;
                        break;
                    case "ready_config":
                        bool.TryParse(xattribute.Value, out bool ready);
                        Ready_config = ready;
                        break;
                    case "operate":
                        bool.TryParse(xattribute.Value, out bool op);
                        Operate = op;
                        break;
                    case "room":
                        Transmission_mode = xattribute.Value;
                        break;
                    case "function":
                        Transmission_mode = xattribute.Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            // Datapoint parsen
            foreach (XNode xnode2 in xElement.Nodes())
            {
                //Debug.WriteLine(xnode2);
                XElement xNodeElement = Channel.ToXElement(xnode2);
                if (xNodeElement == null)
                    throw new InvalidOperationException();
                bool ok = int.TryParse(xNodeElement.Attribute("ise_id").Value.ToString(), out int ise_id);
                Datapoint datapoint = Datapointlist.GetDatapoint(ise_id);
                if (datapoint != null)
                {
                    datapoint.Parse(xnode2);
                }
                else
                {
                    datapoint = new Datapoint();
                    datapoint.Parse(xnode2);
                    // hinzufügen zur Liste
                    Datapointlist.AddDatapoint(datapoint);
                }
            }
        }
        #endregion
    }
}
