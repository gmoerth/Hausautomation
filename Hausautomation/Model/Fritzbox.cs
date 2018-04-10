using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hausautomation.Model
{
    public class Fritzbox
    {
        public string IP1 { get; set; }
        public string IP2 { get; set; }
        public string IP3 { get; set; }
        public string IP4 { get; set; }
        public string PW1 { get; set; }
        public string PW2 { get; set; }
        public string PW3 { get; set; }
        public string PW4 { get; set; }
        private string _MAC1;
        public string MAC1
        {
            get { return _MAC1; }
            set
            {
                _MAC1 = value;
                MAC1anz = 0;
                UpdateMAC_AND_EM();
            }
        }
        private string _MAC2;
        public string MAC2
        {
            get { return _MAC2; }
            set
            {
                _MAC2 = value;
                MAC2anz = 0;
                UpdateMAC_AND_EM();
            }
        }
        private string _MAC3;
        public string MAC3
        {
            get { return _MAC3; }
            set
            {
                _MAC3 = value;
                MAC3anz = 0;
                UpdateMAC_AND_EM();
            }
        }
        private string _MAC4;
        public string MAC4
        {
            get { return _MAC4; }
            set
            {
                _MAC4 = value;
                MAC4anz = 0;
                UpdateMAC_AND_EM();
            }
        }
        public string EM1 { get; set; }
        public string EM2 { get; set; }
        public string EM3 { get; set; }
        public string EM4 { get; set; }
        private bool _FB1;
        public bool FB1
        {
            get { return _FB1; }
            set
            {
                _FB1 = value;
                SID1Stat = 0;
                UpdateIP_AND_PW();
            }
        }
        private bool _FB2;
        public bool FB2
        {
            get { return _FB2; }
            set
            {
                _FB2 = value;
                SID2Stat = 0;
                UpdateIP_AND_PW();
            }
        }
        private bool _FB3;
        public bool FB3
        {
            get { return _FB3; }
            set
            {
                _FB3 = value;
                SID3Stat = 0;
                UpdateIP_AND_PW();
            }
        }
        private bool _FB4;
        public bool FB4
        {
            get { return _FB4; }
            set
            {
                _FB4 = value;
                SID4Stat = 0;
                UpdateIP_AND_PW();
            }
        }
        private bool _DV1;
        public bool DV1
        {
            get { return _DV1; }
            set
            {
                _DV1 = value;
                MAC1anz = 0;
                EM1Anz = 0;
                UpdateMAC_AND_EM();
            }
        }
        private bool _DV2;
        public bool DV2
        {
            get { return _DV2; }
            set
            {
                _DV2 = value;
                MAC2anz = 0;
                EM2Anz = 0;
                UpdateMAC_AND_EM();
            }
        }
        private bool _DV3;
        public bool DV3
        {
            get { return _DV3; }
            set
            {
                _DV3 = value;
                MAC3anz = 0;
                EM3Anz = 0;
                UpdateMAC_AND_EM();
            }
        }
        private bool _DV4;
        public bool DV4
        {
            get { return _DV4; }
            set
            {
                _DV4 = value;
                MAC4anz = 0;
                EM4Anz = 0;
                UpdateMAC_AND_EM();
            }
        }

        public int MAC1anz { get; set; }
        public int MAC2anz { get; set; }
        public int MAC3anz { get; set; }
        public int MAC4anz { get; set; }

        public int SID1Stat { get; set; }
        public int SID2Stat { get; set; }
        public int SID3Stat { get; set; }
        public int SID4Stat { get; set; }

        public int EM1Anz { get; set; }
        public int EM2Anz { get; set; }
        public int EM3Anz { get; set; }
        public int EM4Anz { get; set; }

        bool MAC1found;
        bool MAC2found;
        bool MAC3found;
        bool MAC4found;

        string MACGAST = "", MACOLD = "";

        static Timer timer = null;
        private SendMail sm;

        public Fritzbox()
        {
            sm = new SendMail();
            if (MainPage.settingsPage != null && MainPage.settingsPage.fb != null)
            {
                IP1 = MainPage.settingsPage.fb.IP1;
                IP2 = MainPage.settingsPage.fb.IP2;
                IP3 = MainPage.settingsPage.fb.IP3;
                IP4 = MainPage.settingsPage.fb.IP4;
                PW1 = MainPage.settingsPage.fb.PW1;
                PW2 = MainPage.settingsPage.fb.PW2;
                PW3 = MainPage.settingsPage.fb.PW3;
                PW4 = MainPage.settingsPage.fb.PW4;
                MAC1 = MainPage.settingsPage.fb.MAC1;
                MAC2 = MainPage.settingsPage.fb.MAC2;
                MAC3 = MainPage.settingsPage.fb.MAC3;
                MAC4 = MainPage.settingsPage.fb.MAC4;
                EM1 = MainPage.settingsPage.fb.EM1;
                EM2 = MainPage.settingsPage.fb.EM2;
                EM3 = MainPage.settingsPage.fb.EM3;
                EM4 = MainPage.settingsPage.fb.EM4;
                FB1 = MainPage.settingsPage.fb.FB1;
                FB2 = MainPage.settingsPage.fb.FB2;
                FB3 = MainPage.settingsPage.fb.FB3;
                FB4 = MainPage.settingsPage.fb.FB4;
                DV1 = MainPage.settingsPage.fb.DV1;
                DV2 = MainPage.settingsPage.fb.DV2;
                DV3 = MainPage.settingsPage.fb.DV3;
                DV4 = MainPage.settingsPage.fb.DV4;
                MAC1anz = MainPage.settingsPage.fb.MAC1anz;
                MAC2anz = MainPage.settingsPage.fb.MAC2anz;
                MAC3anz = MainPage.settingsPage.fb.MAC3anz;
                MAC4anz = MainPage.settingsPage.fb.MAC4anz;
                SID1Stat = MainPage.settingsPage.fb.SID1Stat;
                SID2Stat = MainPage.settingsPage.fb.SID2Stat;
                SID3Stat = MainPage.settingsPage.fb.SID3Stat;
                SID4Stat = MainPage.settingsPage.fb.SID4Stat;
                EM1Anz = MainPage.settingsPage.fb.EM1Anz;
                EM2Anz = MainPage.settingsPage.fb.EM2Anz;
                EM3Anz = MainPage.settingsPage.fb.EM3Anz;
                EM4Anz = MainPage.settingsPage.fb.EM4Anz;
            }
        }

        public void StarteMonitoringTask()
        {
            if (timer == null)
                timer = new Timer(new TimerCallback(TimerProc), this, 5000, 60000); // Callback Methode   
        }

        public async void TimerProc(object state)
        {
            MAC1found = false;
            MAC2found = false;
            MAC3found = false;
            MAC4found = false;
            MACOLD = MACGAST;
            MACGAST = "";
            if (FB1 == true)
                Test(IP1, PW1);
            if (FB2 == true)
                Test(IP2, PW2);
            if (FB3 == true)
                Test(IP3, PW3);
            if (FB4 == true)
                Test(IP4, PW4);
            // 30 sek warten warten bis Test() fertig ist
            await Task.Delay(30000);
            if (DV1 == true)
                MAC1E();
            if (DV2 == true)
                MAC2E();
            if (DV3 == true)
                MAC3E();
            if (DV4 == true)
                MAC4E();
            UpdateMAC_AND_EM();
        }

        public void UpdateMAC_AND_EM()
        {
            if (MainPage.settingsPage != null)
                MainPage.settingsPage.UpdateMAC_AND_EM();
            MainPage.Devicelist.Programlist.CheckProgramsForExecution(MAC1anz, MAC2anz, MAC3anz, MAC4anz);
        }

        public void UpdateIP_AND_PW()
        {
            if (MainPage.settingsPage != null)
                MainPage.settingsPage.UpdateIP_AND_PW();
        }

        public void SendEmail(string mailto, string subject, string body)
        {
            if (mailto != "")
                sm.SendEmail(mailto, subject, body);
            if (MainPage.settingsPage != null)
                MainPage.settingsPage.UpdateTitle(subject + " " + body);
        }

        public void MAC1E()
        {
            if (MAC1found == true)
            {
                if (MAC1anz < 0)
                {
                    SendEmail(EM1, "MAC: " + MAC1, $"ist online -> war {-MAC1anz} Minuten offline");
                    MAC1anz = 0;
                    EM1Anz++;
                }
                MAC1anz++;
                Debug.WriteLine($"MAC1anz = {MAC1anz}");
            }
            else
            {
                if (MAC1anz > 0)
                {
                    SendEmail(EM1, "MAC: " + MAC1, $"ist offline -> war {MAC1anz} Minuten online");
                    MAC1anz = 0;
                    EM1Anz++;
                }
                MAC1anz--;
                Debug.WriteLine($"MAC1anz = {MAC1anz}");
            }
        }

        public void MAC2E()
        {
            if (MAC2found == true)
            {
                if (MAC2anz < 0)
                {
                    SendEmail(EM2, "MAC: " + MAC2, $"ist online -> war {-MAC2anz} Minuten offline");
                    MAC2anz = 0;
                    EM2Anz++;
                }
                MAC2anz++;
                Debug.WriteLine($"MAC2anz = {MAC2anz}");
            }
            else
            {
                if (MAC2anz > 0)
                {
                    SendEmail(EM2, "MAC: " + MAC2, $"ist offline -> war {MAC2anz} Minuten online");
                    MAC2anz = 0;
                    EM2Anz++;
                }
                MAC2anz--;
                Debug.WriteLine($"MAC2anz = {MAC2anz}");
            }
        }

        public void MAC3E()
        {
            if (MAC3found == true)
            {
                if (MAC3anz < 0)
                {
                    SendEmail(EM3, "MAC: " + MAC3, $"ist online -> war {-MAC3anz} Minuten offline");
                    MAC3anz = 0;
                    EM3Anz++;
                }
                MAC3anz++;
                Debug.WriteLine($"MAC3anz = {MAC3anz}");
            }
            else
            {
                if (MAC3anz > 0)
                {
                    SendEmail(EM3, "MAC: " + MAC3, $"ist offline -> war {MAC3anz} Minuten online");
                    MAC3anz = 0;
                    EM3Anz++;
                }
                MAC3anz--;
                Debug.WriteLine($"MAC3anz = {MAC3anz}");
            }
        }

        public void MAC4E()
        {
            if (MAC4found == true)
            {
                if (MAC4anz < 0)
                {
                    SendEmail(EM4, "MAC: " + MAC4, $"ist online -> war {-MAC4anz} Minuten offline {MACGAST}");
                    MAC4anz = 0;
                    EM4Anz++;
                }
                MAC4anz++;
                Debug.WriteLine($"MAC4anz = {MAC4anz}");
            }
            else
            {
                if (MAC4anz > 0)
                {
                    SendEmail(EM4, "MAC: " + MAC4, $"ist offline -> war {MAC4anz} Minuten online {MACOLD}");
                    MAC4anz = 0;
                    EM4Anz++;
                }
                MAC4anz--;
                Debug.WriteLine($"MAC4anz = {MAC4anz}");
            }
        }

        public async void Test(string ipfritzbox, string pwfritzbox)
        {
            string benutzername = "xxxxxxxx";
            string kennwort = pwfritzbox;
            // SessionID ermitteln
            string sid = await GetSessionId(benutzername, kennwort, ipfritzbox);
            string sHtml_of_website = await SeiteEinlesen(@"http://" + ipfritzbox + @"/wlan/wlan_settings.lua", sid);
            if (MAC1found == false && DV1 == true)
            {
                int index = sHtml_of_website.IndexOf(MAC1);
                if (index != -1)
                {
                    for (int i = index; i > 0; i--)
                    {
                        if (sHtml_of_website[i] == '%')
                        {
                            index = sHtml_of_website.IndexOf("wlan_rssi", i);
                            break;
                        }
                    }
                    if (sHtml_of_website[index + 9] == '_') // _guest
                    {
                        if (sHtml_of_website[index + 15] != '0') // 0 bis 5
                        {
                            Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC1}  {sHtml_of_website.Substring(index, 16)}");
                            MAC1found = true;
                        }
                    }
                    else
                    {
                        if (sHtml_of_website[index + 9] != '0') // 0 bis 5
                        {
                            Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC1}  {sHtml_of_website.Substring(index, 10)}");
                            MAC1found = true;
                        }
                    }
                }
            }
            if (MAC2found == false && DV2 == true)
            {
                int index = sHtml_of_website.IndexOf(MAC2);
                if (index != -1)
                {
                    for (int i = index; i > 0; i--)
                    {
                        if (sHtml_of_website[i] == '%')
                        {
                            index = sHtml_of_website.IndexOf("wlan_rssi", i);
                            break;
                        }
                    }
                    if (sHtml_of_website[index + 9] == '_') // _guest
                    {
                        if (sHtml_of_website[index + 15] != '0') // 0 bis 5
                        {
                            Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC2}  {sHtml_of_website.Substring(index, 16)}");
                            MAC2found = true;
                        }
                    }
                    else
                    {
                        if (sHtml_of_website[index + 9] != '0') // 0 bis 5
                        {
                            Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC2}  {sHtml_of_website.Substring(index, 10)}");
                            MAC2found = true;
                        }
                    }
                }
            }
            if (MAC3found == false && DV3 == true)
            {
                int index = sHtml_of_website.IndexOf(MAC3);
                if (index != -1)
                {
                    for (int i = index; i > 0; i--)
                    {
                        if (sHtml_of_website[i] == '%')
                        {
                            index = sHtml_of_website.IndexOf("wlan_rssi", i);
                            break;
                        }
                    }
                    if (sHtml_of_website[index + 9] == '_') // _guest
                    {
                        if (sHtml_of_website[index + 15] != '0') // 0 bis 5
                        {
                            Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC3}  {sHtml_of_website.Substring(index, 16)}");
                            MAC3found = true;
                        }
                    }
                    else
                    {
                        if (sHtml_of_website[index + 9] != '0') // 0 bis 5
                        {
                            Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC3}  {sHtml_of_website.Substring(index, 10)}");
                            MAC3found = true;
                        }
                    }
                }
            }
            if (MAC4found == false && DV4 == true)
            {
                int index = sHtml_of_website.IndexOf(MAC4);
                if (index != -1)
                {
                    if (MAC4.Contains("Gastzugang"))
                    {
                        while ((index = sHtml_of_website.IndexOf(@"""MAC-Adresse", index)) != -1)
                            MACGAST += "\n" + sHtml_of_website.Substring(index++ + 14, 17) + " @ " + ipfritzbox;
                        Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC4}  found");
                        MAC4found = true;
                    }
                    else
                    {
                        for (int i = index; i > 0; i--)
                        {
                            if (sHtml_of_website[i] == '%')
                            {
                                index = sHtml_of_website.IndexOf("wlan_rssi", i);
                                break;
                            }
                        }
                        if (sHtml_of_website[index + 9] == '_') // _guest
                        {
                            if (sHtml_of_website[index + 15] != '0') // 0 bis 5
                            {
                                Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC4}  {sHtml_of_website.Substring(index, 16)}");
                                MAC4found = true;
                            }
                        }
                        else
                        {
                            if (sHtml_of_website[index + 9] != '0') // 0 bis 5
                            {
                                Debug.WriteLine($"{ipfritzbox}  {sid}  {MAC4}  {sHtml_of_website.Substring(index, 10)}");
                                MAC4found = true;
                            }
                        }
                    }
                }
            }
        }

        public async Task<string> SeiteEinlesen(string url, string sid)
        {
            string str = "";
            try
            {
                Uri uri = new Uri(url + "?sid=" + sid);
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                str = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SeiteEinlesen " + ex.Message.ToString());
            }
            return str;
        }

        public async Task<string> GetSessionId(string benutzername, string kennwort, string ipfritzbox)
        {
            string sid = "";
            try
            {
                HttpClient client = new HttpClient();
                Stream stream = await client.GetStreamAsync(@"http://" + ipfritzbox + "/login_sid.lua");
                XDocument doc = XDocument.Load(stream);
                sid = fl_Get_Value_of_Node_in_XDocument_by_NodeName(doc, "SID");
                if (sid == "0000000000000000")
                {
                    string challenge = fl_Get_Value_of_Node_in_XDocument_by_NodeName(doc, "Challenge");
                    string sResponse = fl_GetResponse_by_TempUser_Passwort(challenge, kennwort);
                    string uri = @"http://" + ipfritzbox + @"/login_sid.lua?username=" + benutzername + @"&response=" + sResponse;
                    client = new HttpClient();
                    stream = await client.GetStreamAsync(uri);
                    doc = XDocument.Load(stream);
                    sid = fl_Get_Value_of_Node_in_XDocument_by_NodeName(doc, "SID");
                    //Debug.WriteLine($"Challenge: {challenge} Response: {sResponse} SID: {sid}" );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetSessionId " + ex.Message.ToString());
            }
            if (ipfritzbox == IP1)
            {
                if (sid == "")
                    SID1Stat = 1;
                else if (sid == "0000000000000000")
                    SID1Stat = 2;
                else
                    SID1Stat = 3;
            }
            else if (ipfritzbox == IP2)
            {
                if (sid == "")
                    SID2Stat = 1;
                else if (sid == "0000000000000000")
                    SID2Stat = 2;
                else
                    SID2Stat = 3;
            }
            else if (ipfritzbox == IP3)
            {
                if (sid == "")
                    SID3Stat = 1;
                else if (sid == "0000000000000000")
                    SID3Stat = 2;
                else
                    SID3Stat = 3;
            }
            else if (ipfritzbox == IP4)
            {
                if (sid == "")
                    SID4Stat = 1;
                else if (sid == "0000000000000000")
                    SID4Stat = 2;
                else
                    SID4Stat = 3;
            }
            UpdateIP_AND_PW();
            return sid;
        }

        public string fl_GetResponse_by_TempUser_Passwort(string challenge, string kennwort)
        {
            return challenge + "-" + fl_Get_MD5Hash_of_String(challenge + "-" + kennwort);
        }

        public string fl_Get_MD5Hash_of_String(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public string fl_Get_Value_of_Node_in_XDocument_by_NodeName(XDocument doc, string name)
        {
            XElement info = doc.FirstNode as XElement;
            return info.Element(name).Value;
        }
    }
}
