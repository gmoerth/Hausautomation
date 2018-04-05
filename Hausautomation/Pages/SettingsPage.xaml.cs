using Hausautomation.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Hausautomation.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public ReadXDoc xdoc;
        public SendMail sm;
        public Favoriten fa;
        public Fritzbox fb;
        public ProgramList pgl;

        public SettingsPage()
        {
            this.InitializeComponent();
            xdoc = new ReadXDoc();
            sm = new SendMail();
            fa = new Favoriten();
            fb = MainPage.fritzbox;
            pgl = MainPage.Devicelist.Programlist;
            LoadSettingsXML();
            MainPage.settingsPage = this;
        }

        public void LoadSettingsXML()
        {
            Debug.WriteLine("LoadSettingsXML");
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                // C:\Users\Gerhard\AppData\Local\Packages\7e54ccaa-a3c0-48e3-8ded-b0c43979c189_b0ckz6vx689s4\LocalState
                XmlSerializer serializer = new XmlSerializer(typeof(ReadXDoc)); // Ausnahme ausgelöst: "System.NotSupportedException" in System.Private.CoreLib.dll
                using (var reader = new StreamReader(File.Open(localFolder.Path + @"/readxdoc.xml", FileMode.Open, FileAccess.Read)))
                {
                    xdoc = (ReadXDoc)serializer.Deserialize(reader);
                }
                tbHMIP.Text = xdoc.HMIP;
                tbHMPO.Text = xdoc.HMPO.ToString();
                cbOnline.IsChecked = xdoc.online;
                tbRefresh.Text = xdoc.Refresh.ToString();
                XmlSerializer serializer2 = new XmlSerializer(typeof(SendMail)); // Ausnahme ausgelöst: "System.NotSupportedException" in System.Private.CoreLib.dll
                using (var reader = new StreamReader(File.Open(localFolder.Path + @"/sendmail.xml", FileMode.Open, FileAccess.Read)))
                {
                    sm = (SendMail)serializer2.Deserialize(reader);
                }
                tbSMTPServer.Text = sm.SMTPServer;
                tbSMTPPort.Text = sm.SMTPPort.ToString();
                tbSMTPName.Text = sm.NCUsername;
                pbSMTPPass.Password = sm.NCPassword;
                cbAUT.IsChecked = sm.Authentification;
                cbSSL.IsChecked = sm.SSL;
                XmlSerializer serializer3 = new XmlSerializer(typeof(Favoriten)); // Ausnahme ausgelöst: "System.NotSupportedException" in System.Private.CoreLib.dll
                using (var reader = new StreamReader(File.Open(localFolder.Path + @"/favoriten.xml", FileMode.Open, FileAccess.Read)))
                {
                    if (fa.ise_id.Count > 0) // vorher löschen ... sonst wird die liste immer länger
                        fa.ise_id.Clear();
                    fa = (Favoriten)serializer3.Deserialize(reader);
                }
                if (fb == null) // nur das erste mal laden
                {
                    XmlSerializer serializer4 = new XmlSerializer(typeof(Fritzbox)); // Ausnahme ausgelöst: "System.NotSupportedException" in System.Private.CoreLib.dll
                    using (var reader = new StreamReader(File.Open(localFolder.Path + @"/fritzbox.xml", FileMode.Open, FileAccess.Read)))
                    {
                        fb = (Fritzbox)serializer4.Deserialize(reader);
                    }
                }
                if (pgl.Programlist.Count == 0) // nur laden wenn liste leer (das erste mal)
                {
                    XmlSerializer serializer5 = new XmlSerializer(typeof(ProgramList)); // Ausnahme ausgelöst: "System.NotSupportedException" in System.Private.CoreLib.dll
                    using (var reader = new StreamReader(File.Open(localFolder.Path + @"/programs.xml", FileMode.Open, FileAccess.Read)))
                    {
                        pgl = (ProgramList)serializer5.Deserialize(reader);
                        MainPage.Devicelist.Programlist = pgl;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // First Start Save File with Default Values
                SaveSettingsXML();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "Error Deserialize settings.xml");
            }
        }

        public void SaveSettingsXML()
        {
            Debug.WriteLine("SaveSettingsXML");
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                // C:\Users\Gerhard\AppData\Local\Packages\7e54ccaa-a3c0-48e3-8ded-b0c43979c189_b0ckz6vx689s4\LocalState
                XmlSerializer serializer = new XmlSerializer(typeof(ReadXDoc));
                using (StreamWriter writer = new StreamWriter(File.Open(localFolder.Path + @"/readxdoc.xml", FileMode.Create, FileAccess.Write)))
                {
                    tbHMIP_TextChanged(null, null);
                    tbHMPO_TextChanged(null, null);
                    cbOnline_Click(null, null);
                    tbRefresh_TextChanged(null, null);
                    serializer.Serialize(writer, xdoc);
                }
                XmlSerializer serializer2 = new XmlSerializer(typeof(SendMail));
                using (StreamWriter writer = new StreamWriter(File.Open(localFolder.Path + @"/sendmail.xml", FileMode.Create, FileAccess.Write)))
                {
                    tbSMTPServer_TextChanged(null, null);
                    tbSMTPPort_TextChanged(null, null);
                    cbSSL_Click(null, null);
                    tbSMTPName_TextChanged(null, null);
                    pbSMTPPass_TextChanged(null, null);
                    cbAUT_Click(null, null);
                    serializer2.Serialize(writer, sm);
                }
                XmlSerializer serializer3 = new XmlSerializer(typeof(Favoriten));
                using (StreamWriter writer = new StreamWriter(File.Open(localFolder.Path + @"/favoriten.xml", FileMode.Create, FileAccess.Write)))
                {
                    serializer3.Serialize(writer, fa);
                }
                XmlSerializer serializer4 = new XmlSerializer(typeof(Fritzbox));
                using (StreamWriter writer = new StreamWriter(File.Open(localFolder.Path + @"/fritzbox.xml", FileMode.Create, FileAccess.Write)))
                {
                    serializer4.Serialize(writer, fb);
                }
                XmlSerializer serializer5 = new XmlSerializer(typeof(ProgramList));
                using (StreamWriter writer = new StreamWriter(File.Open(localFolder.Path + @"/programs.xml", FileMode.Create, FileAccess.Write)))
                {
                    serializer5.Serialize(writer, pgl);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "Error Serialize settings.xml");
            }
        }

        private void Page_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"LostFocus {sender.GetType().ToString()} {e.OriginalSource.GetType().Name}");
            SaveSettingsXML();
        }

        private void tbHMIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            xdoc.HMIP = tbHMIP.Text;
        }

        private void tbHMPO_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(tbHMPO.Text, out int port);
            xdoc.HMPO = port;
        }

        private void cbOnline_Click(object sender, RoutedEventArgs e)
        {
            if (xdoc.online == false && cbOnline.IsChecked == true)
            {
                xdoc.online = (bool)cbOnline.IsChecked;
                xdoc.ReadAllXDocumentsAsync();
            }
            xdoc.online = (bool)cbOnline.IsChecked;
        }

        private void tbRefresh_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(tbRefresh.Text, out int refresh);
            xdoc.Refresh = refresh;
        }

        private void tbSMTPServer_TextChanged(object sender, TextChangedEventArgs e)
        {
            sm.SMTPServer = tbSMTPServer.Text;
        }

        private void tbSMTPPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(tbSMTPPort.Text, out int port);
            sm.SMTPPort = port;
        }

        private void cbSSL_Click(object sender, RoutedEventArgs e)
        {
            sm.SSL = (bool)cbSSL.IsChecked;
        }

        private void tbSMTPName_TextChanged(object sender, TextChangedEventArgs e)
        {
            sm.NCUsername = tbSMTPName.Text;
        }

        private void pbSMTPPass_TextChanged(object sender, RoutedEventArgs e)
        {
            sm.NCPassword = pbSMTPPass.Password;
        }

        private void btTestEmail_Button_Click(object sender, RoutedEventArgs e)
        {
            sm.SendEmail("gmc@chello.at", "Test Betreff", "Test Mail");
        }

        private void cbAUT_Click(object sender, RoutedEventArgs e)
        {
            sm.Authentification = (bool)cbAUT.IsChecked;
        }

        public async Task<bool> UpdateIP_AND_PW(string ip, string sid)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    // Your UI update code goes here!
                    Brush wh = new SolidColorBrush(Colors.White);
                    Brush gy = new SolidColorBrush(Colors.GreenYellow);
                    Brush to = new SolidColorBrush(Colors.Tomato);
                    if (FB1.IsChecked == true)
                    {
                        if (ip == IP1.Text)
                        {
                            if (sid == "")
                            {
                                IP1.Background = to;
                                PW1.Background = wh;
                                return;
                            }
                            if (sid == "0000000000000000")
                            {
                                IP1.Background = gy;
                                PW1.Background = to;
                                return;
                            }
                            if (sid != "SW")
                            {
                                IP1.Background = gy;
                                PW1.Background = gy;
                            }
                        }
                    }
                    else
                    {
                        IP1.Background = wh;
                        PW1.Background = wh;
                    }
                    if (FB2.IsChecked == true)
                    {
                        if (ip == IP2.Text)
                        {
                            if (sid == "")
                            {
                                IP2.Background = to;
                                PW2.Background = wh;
                                return;
                            }
                            if (sid == "0000000000000000")
                            {
                                IP2.Background = gy;
                                PW2.Background = to;
                                return;
                            }
                            if (sid != "SW")
                            {
                                IP2.Background = gy;
                                PW2.Background = gy;
                            }
                        }
                    }
                    else
                    {
                        IP2.Background = wh;
                        PW2.Background = wh;
                    }
                    if (FB3.IsChecked == true)
                    {
                        if (ip == IP3.Text)
                        {
                            if (sid == "")
                            {
                                IP3.Background = to;
                                PW3.Background = wh;
                                return;
                            }
                            if (sid == "0000000000000000")
                            {
                                IP3.Background = gy;
                                PW3.Background = to;
                                return;
                            }
                            if (sid != "SW")
                            {
                                IP3.Background = gy;
                                PW3.Background = gy;
                            }
                        }
                    }
                    else
                    {
                        IP3.Background = wh;
                        PW3.Background = wh;
                    }
                    if (FB4.IsChecked == true)
                    {
                        if (ip == IP4.Text)
                        {
                            if (sid == "")
                            {
                                IP4.Background = to;
                                PW4.Background = wh;
                                return;
                            }
                            if (sid == "0000000000000000")
                            {
                                IP4.Background = gy;
                                PW4.Background = to;
                                return;
                            }
                            if (sid != "SW")
                            {
                                IP4.Background = gy;
                                PW4.Background = gy;
                            }
                        }
                    }
                    else
                    {
                        IP4.Background = wh;
                        PW4.Background = wh;
                    }
                });
            return true;
        }

        public async void UpdateMAC_AND_EM()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    // Your UI update code goes here!
                    if (fb == null)
                        return;
                    Brush wh = new SolidColorBrush(Colors.White);
                    Brush gy = new SolidColorBrush(Colors.GreenYellow);
                    Brush to = new SolidColorBrush(Colors.Tomato);
                    if (fb.MAC1anz == 0)
                    {
                        MAC1.Background = wh;
                        EM1.Background = wh;
                    }
                    if (fb.MAC1anz > 0)
                        MAC1.Background = gy;
                    if (fb.MAC1anz < 0)
                        MAC1.Background = to;
                    if (fb.MAC2anz == 0)
                    {
                        MAC2.Background = wh;
                        EM2.Background = wh;
                    }
                    if (fb.MAC2anz > 0)
                        MAC2.Background = gy;
                    if (fb.MAC2anz < 0)
                        MAC2.Background = to;
                    if (fb.MAC3anz == 0)
                    {
                        MAC3.Background = wh;
                        EM3.Background = wh;
                    }
                    if (fb.MAC3anz > 0)
                        MAC3.Background = gy;
                    if (fb.MAC3anz < 0)
                        MAC3.Background = to;
                    if (fb.MAC4anz == 0)
                    {
                        MAC4.Background = wh;
                        EM4.Background = wh;
                    }
                    if (fb.MAC4anz > 0)
                        MAC4.Background = gy;
                    if (fb.MAC4anz < 0)
                        MAC4.Background = to;
                });
        }

        public async Task<bool> UpdateTitle(string title)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    // Your UI update code goes here!
                    Brush ye = new SolidColorBrush(Colors.Yellow);
                    if (title.Contains(MAC1.Text) == true)
                        EM1.Background = ye;
                    if (title.Contains(MAC2.Text) == true)
                        EM2.Background = ye;
                    if (title.Contains(MAC3.Text) == true)
                        EM3.Background = ye;
                    if (title.Contains(MAC4.Text) == true)
                        EM4.Background = ye;
                    var appView = ApplicationView.GetForCurrentView();
                    appView.Title = title;
                });
            return true;
        }

    }
}
