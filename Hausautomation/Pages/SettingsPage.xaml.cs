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
        public bool bLoaded = false;
        public ReadXDoc xdoc;


        public SettingsPage()
        {
            this.InitializeComponent();
            if (MainPage.settingsPage == null)
            {
                LoadEmailXML();
                bLoaded = true;
            }
        }

        public void LoadEmailXML()
        {
            Debug.WriteLine("LoadXML");
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ReadXDoc));
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                using (var reader = new StreamReader(File.Open(localFolder.Path + @"/ReadXDoc.xml", FileMode.Open, FileAccess.Read)))
                {
                    xdoc = (ReadXDoc)serializer.Deserialize(reader);
                }
                tbHMIP.Text = xdoc.HMIP;
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
                SaveEmailXML();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Error Deserialize SendMail");
                Debug.WriteLine(ex.ToString(), "Error Deserialize SendMail");
            }
        }

        public void SaveEmailXML()
        {
            Debug.WriteLine("SaveEmailXML");
            try
            {
                XmlSerializer serializerSM = new XmlSerializer(typeof(ReadXDoc));
                //using (StreamWriter writer = new StreamWriter(@"../../Email.xml"))
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                using (StreamWriter writer = new StreamWriter(File.Open(localFolder.Path + @"/ReadXDoc.xml", FileMode.Create, FileAccess.Write)))
                {
                    /*TextBox_TextChanged_SMTPServer(null, null);
                    TextBox_TextChanged_SMTPPort(null, null);
                    TextBox_TextChanged_SMTPName(null, null);
                    TextBox_TextChanged_SMTPPass(null, null);
                    AUT_Click(null, null);
                    SSL_Click(null, null);*/
                    xdoc = new ReadXDoc();
                    serializerSM.Serialize(writer, xdoc);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Error Serialize SendMail");
                Debug.WriteLine(ex.ToString(), "Error Serialize SendMail");
            }
        }


        //public string tbHMIP { get; set; }

        //public string MyProperty { get; set; }

        public void ReadXDoc()
        {
            //tbHMIP 
        }


        private void tbHMIP_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbHMPO_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
