using Hausautomation.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

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
        private bool _Active;

        public bool Active
        {
            get { return _Active; }
            set
            {
                _Active = value;
            }
        }

        private int _Device;

        public int Device
        {
            get { return _Device; }
            set
            {
                _Device = value;
                if (Devicelist.Count > 0) // nur speichern wenn Benutzer etwas auswählt
                {
                    MainPage.settingsPage.SaveSettingsXML();
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
                    MainPage.settingsPage.SaveSettingsXML();
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
                    MainPage.settingsPage.SaveSettingsXML();
                }
            }
        }
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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

        public Programs()
        {
        }

        public Programs(bool WithNewList)
        {
            Devicelist = new List<string>() { "Gerät mit MAC 1", "Gerät mit MAC 2", "Gerät mit MAC 3", "Gerät mit MAC 4" };
            Statuslist = new List<string>() { "auf Online wechselt", "auf Offline wechselt" };
            Delaylist = new List<string>() { "sofort", "1 Minute", "2 Minuten", "5 Minuten", "10 Minuten", "30 Minuten", "1 Stunde", "2 Stunden", "5 Stunden", "10 Stunden" };
            Programs programs = MainPage.Devicelist.Programlist.Programlist.LastOrDefault();
            if (programs != null)
                ID = programs.ID + 1;
            else
                ID = 1;
        }

        public async Task Delete()
        {
            MessageDialog showDialog = new MessageDialog("Soll das Programm endgültig gelöscht werden?", "Programm Löschen?");
            showDialog.Commands.Add(new UICommand("Ja") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Nein") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                MainPage.Devicelist.Programlist.Programlist.Remove(this);
                MainPage.settingsPage.SaveSettingsXML();
            }
        }

        public void Checkbox_Click()
        {
            MainPage.settingsPage.SaveSettingsXML();
        }

    }
}
