using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class FunctionList
    {
        public List<Function> Functionlist { get; set; }

        public FunctionList()
        {
            Functionlist = new List<Function>();
        }

        public Function GetFunction(int ise_id)
        {
            foreach (Function function in Functionlist)
                if (function.Ise_id == ise_id)
                    return function;
            return null;
        }

        public void AddFunction(Function function)
        {
            Functionlist.Add(function);
        }
    }

    public class Function
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

        public string Description { get; set; }
        #endregion

        #region Methoden
        public void Parse(XElement xElement)
        {
            // Function parsen
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
                    case "description":
                        Description = xattribute.Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}
