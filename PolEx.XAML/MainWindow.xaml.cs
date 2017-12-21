using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PolEx.XAML.Controls;

using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;

namespace PolEx.XAML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadRSS();
        }

        private void LoadRSS()
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Ignore;

                //string url = "http://www.jogossantacasa.pt/web/SCRss/rssFeedCartRes";
                string url = "http://www.police.public.lu/fr/support/rss/actualites/index.html";

                XmlDocument doc = new XmlDocument();
                doc.Load("testrss.xml");

                string xmlString = doc.InnerXml;

                TextReader tr = new StringReader(xmlString);
                XmlReader xmlReader = XmlReader.Create(tr);
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                xmlReader.Close();


                // Load items on a listbox
                List<RSSItem> lstRSS = new List<RSSItem>();
                foreach (SyndicationItem item in feed.Items)
                {
                    //lstRSS.Add(new RSSItem(item.Title.Text, ReformatHTML(item.Summary.Text)));
                    lstRSS.Add(new RSSItem(item.Title.Text, item.Summary.Text));
                }
                //ListRSS.ItemsSource = lstRSS;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private String ReformatHTML(String htmlText)
        {
            return htmlText.Replace("b>", "bold>");
        }


    }
}
