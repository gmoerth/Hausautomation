﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class DatapointList
    {
        public List<Datapoint> Datapointlist { get; set; }

        public DatapointList()
        {
            Datapointlist = new List<Datapoint>();
        }

        public Datapoint GetDatapoint(int ise_id)
        {
            foreach (Datapoint datapoint in Datapointlist)
                if (datapoint.Ise_id == ise_id)
                    return datapoint;
            return null;
        }

        public void AddDatapoint(Datapoint datapoint)
        {
            Datapointlist.Add(datapoint);
        }
    }

    public class Datapoint
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
        private int operations;

        public int Operations
        {
            get { return operations; }
            set { operations = value; }
        }
        private DateTime timestamp;

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        private string valueunit;

        public string Valueunit
        {
            get { return valueunit; }
            set { valueunit = value; }
        }
        private int valuetype;

        public int Valuetype
        {
            get { return valuetype; }
            set { valuetype = value; }
        }
        private double value1;

        public double Value
        {
            get { return value1; }
            set { value1 = value; }
        }
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion

        #region Methoden
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void Parse(XNode xnode)
        {
            NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            //CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US"); // Punkt als Komma
            CultureInfo culture = new CultureInfo("en-US"); // Punkt als Komma
            XElement xElement = Channel.ToXElement(xnode);
            if (xElement == null)
                throw new InvalidOperationException();
            // Datapoint parsen
            foreach (XAttribute xattribute in xElement.Attributes())
            {
                //Debug.WriteLine(xattribute);
                switch (xattribute.Name.ToString())
                {
                    case "ise_id":
                        int.TryParse(xattribute.Value, out int id);
                        Ise_id = id;
                        break;
                    case "name":
                        Name = xattribute.Value;
                        break;
                    case "operations":
                        int.TryParse(xattribute.Value, out int op);
                        Operations = op;
                        break;
                    case "timestamp":
                        double.TryParse(xattribute.Value, out double ts);
                        Timestamp = UnixTimeStampToDateTime(ts);
                        break;
                    case "valueunit":
                        Valueunit = xattribute.Value;
                        break;
                    case "valuetype":
                        int.TryParse(xattribute.Value, out int vt);
                        Valuetype = vt;
                        break;
                    case "value":
                        double.TryParse(xattribute.Value, style, culture, out double va);
                        Value = va;
                        if (xattribute.Value == "true")
                            Value = Double.PositiveInfinity;
                        if (xattribute.Value == "false")
                            Value = Double.NegativeInfinity;
                        break;
                    case "type":
                        Type = xattribute.Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}
