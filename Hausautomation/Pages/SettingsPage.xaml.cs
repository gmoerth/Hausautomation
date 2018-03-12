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

        public SettingsPage()
        {
            this.InitializeComponent();
            xdoc = new ReadXDoc();
            LoadSettingsXML();
            MainPage.settingsPage = this;
        }

        public void LoadSettingsXML()
        {
            Debug.WriteLine("LoadSettingsXML");
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ReadXDoc));
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                // C:\Users\Gerhard\AppData\Local\Packages\7e54ccaa-a3c0-48e3-8ded-b0c43979c189_b0ckz6vx689s4\LocalState
                using (var reader = new StreamReader(File.Open(localFolder.Path + @"/settings.xml", FileMode.Open, FileAccess.Read)))
                {
                    xdoc = (ReadXDoc)serializer.Deserialize(reader);
                    reader.Close();
                }
                tbHMIP.Text = xdoc.HMIP;
                tbHMPO.Text = xdoc.HMPO.ToString();
                /*SMTPServer.Text = sm.SMTPHost;
                SMTPPort.Text = sm.SMTPPort.ToString();
                SMTPName.Text = sm.NCUsername;
                SMTPPass.Password = sm.NCPassword;
                AUT.IsChecked = sm.Authentification;
                SSL.IsChecked = sm.SSL;*/
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
                XmlSerializer serializer = new XmlSerializer(typeof(ReadXDoc));
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                // C:\Users\Gerhard\AppData\Local\Packages\7e54ccaa-a3c0-48e3-8ded-b0c43979c189_b0ckz6vx689s4\LocalState
                using (StreamWriter writer = new StreamWriter(File.Open(localFolder.Path + @"/settings.xml", FileMode.Create, FileAccess.Write)))
                {
                    tbHMIP_TextChanged(null, null);
                    tbHMPO_TextChanged(null, null);
                    tbSMTPServer_TextChanged(null, null);
                    tbSMTPPort_TextChanged(null, null);
                    cbSSL_Click(null, null);
                    tbSMTPName_TextChanged(null, null);
                    pbSMTPPass_TextChanged(null, null);
                    cbAUT_Click(null, null);
                    serializer.Serialize(writer, xdoc);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "Error Serialize settings.xml");
            }
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

        private void tbSMTPServer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbSMTPPort_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

         private void cbSSL_Click(object sender, RoutedEventArgs e)
        {

        }

       private void tbSMTPName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void pbSMTPPass_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btTestEmail_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbAUT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"LostFocus {sender.GetType().ToString()} {e.OriginalSource.GetType().Name}");
            SaveSettingsXML();
        }
    }
}
