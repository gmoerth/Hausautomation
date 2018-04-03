using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hausautomation.Model
{
    public class ProgramList
    {
        public ObservableCollection<Programs> Programlist { get; set; }

        public ProgramList()
        {
            Programlist = new ObservableCollection<Programs>();
        }

    }

    public class Programs
    {
        private int _device;

        public int Device
        {
            get { return _device; }
            set { _device = value; }
        }
        private string _mac;

        public string MAC
        {
            get { return _mac; }
            set { _mac = value; }
        }
        private int _ise_id;

        public int Ise_Id
        {
            get { return _ise_id; }
            set { _ise_id = value; }
        }
        private double _new_value;

        public double New_Value
        {
            get { return _new_value; }
            set { _new_value = value; }
        }
        private bool _ifonline;

        public bool IfOnline
        {
            get { return _ifonline; }
            set { _ifonline = value; }
        }
        private int _delay;

        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public Programs()
        {

        }
    }
}
