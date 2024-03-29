﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class RoomList
    {
        public List<Room> Roomlist { get; set; }

        public RoomList()
        {
            Roomlist = new List<Room>();
        }

        public Room GetRoom(int ise_id)
        {
            foreach (Room room in Roomlist)
                if (room.Ise_id == ise_id)
                    return room;
            return null;
        }

        public void AddRoom(Room room)
        {
            Roomlist.Add(room);
        }
    }

    public class Room
    {
        #region Properties
        private int ise_id;

        public int Ise_id
        {
            get { return ise_id; }
            set { ise_id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
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
                    case "name":
                        Name = xattribute.Value;
                        break;
                    case "ise_id":
                        int.TryParse(xattribute.Value, out int id);
                        Ise_id = id;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}
