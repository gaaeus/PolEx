using HTMLConverter;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PolEx.XAML.Controls
{
    /// <summary>
    /// Interaction logic for RSSItem.xaml
    /// </summary>
    public partial class RSSItem : UserControl
    {
        #region Properties

        private String _Title;
        public String Title
        {
            get { return this._Title; }
            set
            {
                this._Title = value;
                this.lblTitle.Content = value;
            }
        }

        private String _Contents;
        public String Contents
        {
            get { return this._Contents; }
            set
            {
                this._Contents = value;
                //lblContent.Text = value;

                var xaml = string.Empty;
                if (!string.IsNullOrEmpty(value))
                    xaml = HtmlToXamlConverter.ConvertHtmlToXaml(value, false);

                lblContent.Document = (FlowDocument)XamlReader.Parse(xaml);

                //lblContent.Document = (FlowDocument)XamlReader.Parse(value);
                //http://michaelsync.net/2009/06/09/bindable-wpf-richtext-editor-with-xamlhtml-convertor
                //https://code.msdn.microsoft.com/windowsdesktop/Converting-between-RTF-and-aaa02a6e
            }
        }

        // But probably should use this: http://stackoverflow.com/questions/10399400/best-way-to-read-rss-feed-in-net-using-c-sharp

        private String _Link;
        public String Link
        {
            get { return this._Link; }
            set
            {
                this._Link = value;
            }
        }

        #endregion

        #region Constructors

        public RSSItem()
        {
            InitializeComponent();
        }

        public RSSItem(String Title, String Contents)
        {
            InitializeComponent();

            this.Title = Title;
            this.Contents = Contents;
        }

        #endregion

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(!String.IsNullOrEmpty(this.Link))
            {
                System.Diagnostics.Process.Start(this.Link);
            }
        }
    }
}
