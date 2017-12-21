using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PolEX;
using PolEX.Model;

using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;
using PolEX.Database;

namespace PolEX.Win
{
    // Video streaming http://www.codeproject.com/Articles/995238/Live-Video-Streaming-from-Windows-Phone
    // https://www.microsoftvirtualacademy.com/en-us/training-courses/developing-universal-windows-apps-with-c-and-xaml-8363
    // Ideas: https://www.tipsubmit.com/WebTips.aspx?AgencyID=1294&HR=https://www.crimereports.com/

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            LoadRSS();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBManager db = new DBManager();
            //db.CreateSchema("pol_ex_settings");
            db.CreateSchema(string.Empty);

            UserSettings userSettings = new UserSettings(null, "Hélio", "gaaeus", "helio.a.silva@gmail.com", "234", "There", "There too", "nothing", "xyz", true, false, true);
            db.CreateUpdateUserSettings(userSettings);
        }

        // Load RSS
        private void LoadRSS()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;

            //string url = "http://www.jogossantacasa.pt/web/SCRss/rssFeedCartRes";
            string url = "http://www.police.public.lu/fr/support/rss/actualites/index.html";

            XmlDocument doc = new XmlDocument();
            doc.Load(url);

            string xmlString = doc.InnerXml;

            TextReader tr = new StringReader(xmlString);
            XmlReader xmlReader = XmlReader.Create(tr);
            SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
            xmlReader.Close();


            // Load items on a listbox

            foreach(SyndicationItem item in feed.Items)
            {
                listView1.Items.Add(new ListViewItem(item.Summary.Text));
            }
        }

        private async void CreateDBAsync_Click(object sender, EventArgs e)
        {
            DBController dbController = new DBController();
            await dbController.GetConnection();

            if(dbController.DatabaseConnection != null)
            {
            
            }
        }
    }
}
