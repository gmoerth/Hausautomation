using Hausautomation.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

            /*Programs prog = new Programs();
            prog.Device = 1;
            prog.New_Value = "1234a";
            prog.Ise_Id = 1234;
            prog.Delay = 0;
            prog.SNr = "NEQ1542641";
            prog.Name = "HM-PB-2-WM55-2";
            prog.Status = 0;
            Programlist.Add(prog);

            Programs prog2 = new Programs();
            prog2.Device = 2;
            prog2.New_Value = "5678b";
            prog2.Ise_Id = 5678;
            prog2.Delay = 3;
            prog2.SNr = "NEQ1542641a";
            prog2.Name = "HM-PB-2-WM55-2a";
            prog2.Status = 1;
            Programlist.Add(prog2);*/

        }

    }

    public class Programs
    {
        private int _Device;

        public int Device
        {
            get { return _Device; }
            set
            {
                _Device = value;
                if(Devicelist.Count > 0) // nur speichern wenn Benutzer etwas auswählt
                {
                    SettingsPage settingsPage = new SettingsPage();
                    settingsPage.SaveSettingsXML();
                }
            }
        }
        private int _Status;

        public int Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                if (Statuslist.Count > 0) // nur speichern wenn Benutzer etwas auswählt
                {
                    SettingsPage settingsPage = new SettingsPage();
                    settingsPage.SaveSettingsXML();
                }
            }
        }

        private int _Delay;

        public int Delay
        {
            get { return _Delay; }
            set
            {
                _Delay = value;
                if (Delaylist.Count > 0) // nur speichern wenn Benutzer etwas auswählt
                {
                    SettingsPage settingsPage = new SettingsPage();
                    settingsPage.SaveSettingsXML();
                }
            }
        }
        private string _SNr;

        public string SNr
        {
            get { return _SNr; }
            set { _SNr = value; }
        }
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private int _Ise_id;

        public int Ise_Id
        {
            get { return _Ise_id; }
            set { _Ise_id = value; }
        }
        private string _New_Value;

        public string New_Value
        {
            get { return _New_Value; }
            set { _New_Value = value; }
        }
        private List<string> _Devicelist;

        public List<string> Devicelist
        {
            get { return _Devicelist; }
            set { _Devicelist = value; }
        }
        private List<string> _Statuslist;

        public List<string> Statuslist
        {
            get { return _Statuslist; }
            set { _Statuslist = value; }
        }
        private List<string> _Delaylist;

        public List<string> Delaylist
        {
            get { return _Delaylist; }
            set { _Delaylist = value; }
        }
        //public List<string> Devicelist = new List<string>() { "Gerät mit MAC 1", "Gerät mit MAC 2", "Gerät mit MAC 3", "Gerät mit MAC 4" };
        //public List<string> Statuslist = new List<string>() { "auf Online wechselt", "auf Offline wechselt" };
        //public List<string> Delaylist = new List<string>() { "sofort", "1 Minute", "2 Minuten", "5 Minuten", "10 Minuten", "30 Minuten", "1 Stunde", "2 Stunden", "5 Stunden", "10 Stunden" };

        public Programs()
        {
            Devicelist = new List<string>();
            Statuslist = new List<string>();
            Delaylist = new List<string>();
        }

        /*public void scDevice()
        {
            
        }*/
    }
}
