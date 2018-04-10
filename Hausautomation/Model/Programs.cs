using Hausautomation.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Hausautomation.Model
{
    public class Statements
    {
        public int NewId;
        public string NewValue;
        public DateTime dateTime;
    }

    public class ProgramList
    {
        public ObservableCollection<Programs> Programlist { get; set; }
        private int MAC1anz;
        private int MAC2anz;
        private int MAC3anz;
        private int MAC4anz;
        private List<Statements> Statementslist;

        public ProgramList()
        {
            Programlist = new ObservableCollection<Programs>();
            Statementslist = new List<Statements>();
        }

        public DateTime ComboIndexToDateTime(int Value)
        {
            switch (Value)
            {
                case 0:
                    return DateTime.Now;
                case 1:
                    return DateTime.Now + new TimeSpan(0, 0, 30); // 30 sek
                case 2:
                    return DateTime.Now + new TimeSpan(0, 1, 30);
                case 3:
                    return DateTime.Now + new TimeSpan(0, 4, 30);
                case 4:
                    return DateTime.Now + new TimeSpan(0, 9, 30);
                case 5:
                    return DateTime.Now + new TimeSpan(0, 29, 30);
                case 6:
                    return DateTime.Now + new TimeSpan(0, 59, 30);
                case 7:
                    return DateTime.Now + new TimeSpan(1, 59, 30);
                case 8:
                    return DateTime.Now + new TimeSpan(5, 59, 30);
                case 9:
                    return DateTime.Now + new TimeSpan(9, 59, 30);
                default:
                    return DateTime.Now;
            }
        }

        public void CheckProgramsForExecution(int mac1anz, int mac2anz, int mac3anz, int mac4anz)
        {
            if (MAC1anz < 0 && mac1anz > 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 0 && prog.Status == 0 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC1anz = mac1anz;
                Debug.WriteLine($"Programme mit MAC1 auf Online Anz={Statementslist.Count}");
            }
            else if (MAC1anz > 0 && mac1anz < 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 0 && prog.Status == 1 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC1anz = mac1anz;
                Debug.WriteLine($"Programme mit MAC1 auf Offline Anz={Statementslist.Count}");
            }
            else
            {
                MAC1anz = mac1anz;
            }

            if (MAC2anz < 0 && mac2anz > 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 1 && prog.Status == 0 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC2anz = mac2anz;
                Debug.WriteLine($"Programme mit MAC2 auf Online Anz={Statementslist.Count}");
            }
            else if (MAC2anz > 0 && mac2anz < 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 1 && prog.Status == 1 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC2anz = mac2anz;
                Debug.WriteLine($"Programme mit MAC2 auf Offline Anz={Statementslist.Count}");
            }
            else
            {
                MAC2anz = mac2anz;
            }

            if (MAC3anz < 0 && mac3anz > 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 2 && prog.Status == 0 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC3anz = mac3anz;
                Debug.WriteLine($"Programme mit MAC3 auf Online Anz={Statementslist.Count}");
            }
            else if (MAC3anz > 0 && mac3anz < 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 2 && prog.Status == 1 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC3anz = mac3anz;
                Debug.WriteLine($"Programme mit MAC3 auf Offline Anz={Statementslist.Count}");
            }
            else
            {
                MAC3anz = mac3anz;
            }

            if (MAC4anz < 0 && mac4anz > 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 3 && prog.Status == 0 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC4anz = mac4anz;
                Debug.WriteLine($"Programme mit MAC4 auf Online Anz={Statementslist.Count}");
            }
            else if (MAC4anz > 0 && mac4anz < 0)
            {
                foreach (Programs prog in Programlist)
                {
                    if (prog.Device == 3 && prog.Status == 1 && prog.Active == true)
                    {
                        Statements statements = new Statements();
                        statements.NewId = prog.Ise_Id;
                        statements.NewValue = prog.New_Value;
                        statements.dateTime = ComboIndexToDateTime(prog.Delay);
                        Statementslist.Add(statements);
                    }
                }
                MAC4anz = mac4anz;
                Debug.WriteLine($"Programme mit MAC4 auf Offline Anz={Statementslist.Count}");
            }
            else
            {
                MAC4anz = mac4anz;
            }
            GetDelayedProgramsForExecution();
        }

        public void GetDelayedProgramsForExecution()
        {
            Debug.WriteLine($"GetDelayedProgramsForExecution Anz={Statementslist.Count}");
            int i = Statementslist.Count;
            while (i-- > 0)
            {
                Statements state = Statementslist.ElementAt(i);
                if (DateTime.Now > state.dateTime)
                {
                    NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                    //CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US"); // Punkt als Komma
                    CultureInfo culture = new CultureInfo("en-US"); // Punkt als Komma
                    ReadXDoc readXDoc = new ReadXDoc();
                    readXDoc.NewId = state.NewId;
                    double.TryParse(state.NewValue, style, culture, out double val);
                    if (state.NewValue == "True")
                        val = Double.PositiveInfinity;
                    if (state.NewValue == "False")
                        val = Double.NegativeInfinity;
                    readXDoc.NewValue = val;
                    readXDoc.Anzahl = 1;
                    readXDoc.ReadStateChangeXDoc();
                    Statementslist.RemoveAt(i);
                }
            }
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

        public void Delete()
        {
#pragma warning disable 4014
            DeleteAsync();
#pragma warning restore 4014
        }

        public async Task DeleteAsync()
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
