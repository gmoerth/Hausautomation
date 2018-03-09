using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //throw new IndexOutOfRangeException();
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

        public Datapoint(int ise_id, string name, int operations, DateTime timestamp, string valueunit, int valuetype, double value, string type)
        {
            this.ise_id = ise_id;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.operations = operations;
            this.timestamp = timestamp;
            this.valueunit = valueunit ?? throw new ArgumentNullException(nameof(valueunit));
            this.valuetype = valuetype;
            this.value1 = value;
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }






    }
}
