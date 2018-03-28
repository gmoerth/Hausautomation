using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hausautomation.Model
{
    public class Favoriten
    {
        public List<int> ise_id { get; set; }
        public string RoomItemSel { get; set; }
        public string FuncItemSel { get; set; }

        public Favoriten()
        {
            ise_id = new List<int>();
            if (MainPage.settingsPage != null)
            {
                ise_id = MainPage.settingsPage.fa.ise_id;
            }
        }

        public void LoadFavoriten()
        {
            foreach (int i in ise_id)
            {
                foreach (Device device in MainPage.Devicelist.Devicelist)
                {
                    if (device.Ise_id == i)
                        device.bFavoriten = true;
                }
            }
        }

        public void AddFavoriten(int id)
        {
            ise_id.Add(id);
            MainPage.settingsPage.SaveSettingsXML();
        }

        public void RemoveFavoriten(int id)
        {
            ise_id.Remove(id);
            MainPage.settingsPage.SaveSettingsXML();
        }
    }
}
