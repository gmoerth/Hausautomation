using Hausautomation.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

        public SettingsPage()
        {
            this.InitializeComponent();
            xdoc = new ReadXDoc();
            sm = new SendMail();
            fa = new Favoriten();
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
                using (var reader2 = new StreamReader(File.Open(localFolder.Path + @"/sendmail.xml", FileMode.Open, FileAccess.Read)))
                {
                    sm = (SendMail)serializer2.Deserialize(reader2);
                }
                tbSMTPServer.Text = sm.SMTPServer;
                tbSMTPPort.Text = sm.SMTPPort.ToString();
                tbSMTPName.Text = sm.NCUsername;
                pbSMTPPass.Password = sm.NCPassword;
                cbAUT.IsChecked = sm.Authentification;
                cbSSL.IsChecked = sm.SSL;
                XmlSerializer serializer3 = new XmlSerializer(typeof(Favoriten)); // Ausnahme ausgelöst: "System.NotSupportedException" in System.Private.CoreLib.dll
                using (var reader3 = new StreamReader(File.Open(localFolder.Path + @"/favoriten.xml", FileMode.Open, FileAccess.Read)))
                {
                    if (fa.ise_id.Count > 0) // vorher löschen ... sonst wird die liste immer länger
                        fa.ise_id.Clear();
                    fa = (Favoriten)serializer3.Deserialize(reader3);
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

    }
}
