﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class Channel
    {
        #region Properties
        private List<Datapoint> datapointlist;

        public List<Datapoint> Datapointlist
        {
            get { return datapointlist; }
            set { datapointlist = value; }
        }

        private List<Room> roomlist;

        public List<Room> Roomlist
        {
            get { return roomlist; }
            set { roomlist = value; }
        }

        private List<Function> functionlist;

        public List<Function> Functionslist
        {
            get { return functionlist; }
            set { functionlist = value; }
        }

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

        public Channel(List<Datapoint> datapointlist, string name, int type, string address, int ise_id, string direction, int parent_device, int index, string group_partner, bool aes_available, string transmission_mode, bool visible, bool ready_config, bool operate)
        {
            this.datapointlist = datapointlist ?? throw new ArgumentNullException(nameof(datapointlist));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.type = type;
            this.address = address ?? throw new ArgumentNullException(nameof(address));
            this.ise_id = ise_id;
            this.direction = direction ?? throw new ArgumentNullException(nameof(direction));
            this.parent_device = parent_device;
            this.index = index;
            this.group_partner = group_partner ?? throw new ArgumentNullException(nameof(group_partner));
            this.aes_available = aes_available;
            this.transmission_mode = transmission_mode ?? throw new ArgumentNullException(nameof(transmission_mode));
            this.visible = visible;
            this.ready_config = ready_config;
            this.operate = operate;
        }

        public Channel()
        {

        }

        static XElement ToXElement(XNode xnode)
        {
            return xnode as XElement; // returns null if node is not an XElement
        }

        public void ParseDevicelist(XNode xnode)
        {
            XElement xElement = ToXElement(xnode);
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
                    default:
                        break;
                }
            }
        }

    }
}