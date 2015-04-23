using EDwI_lab1.Model;
using EDwI_lab1.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDwI_lab1.View
{
    public partial class InternetBotForm : Form
    {
        private const int COUNT = 5;
        private const string INDEX_PATH = @"C:\webmining\indexed_pages";
        private const string FILE_WEBPAGES_PATH = @"webpages.txt";

        //private const string FILE_WEBPAGES_PATH = @"webPages2.txt";

        private LuceneIndexing _luceneTool;

        public InternetBotForm(bool getIndexedPages)
        {
            InitializeComponent();
            int indexedPages;
            _luceneTool = new LuceneIndexing(INDEX_PATH, getIndexedPages, true);
            if (getIndexedPages == false)
                _luceneTool.BuildWebPagesIndex(getListWebPages(FILE_WEBPAGES_PATH));
            else
            {
                indexedPages = _luceneTool.IndexReaderLucene.NumDocs();
                Console.WriteLine("IndexedPages: " + indexedPages);
            }

        }

        

        private List<WebPage> getListWebPages(string fileName)
        {
            using (StreamReader streamReader = File.OpenText(fileName))
            {
                List<WebPage> result = new List<WebPage>();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    result.Add(new WebPage("http://" + line));
                }
                return result.Where(w => w.IsValid).ToList();
            }
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                this.Enabled = false;
                Search(SearchTextBox.Text);
                this.Enabled = true;
            }
        }


        private void Search(string search)
        {
            BindSearchListView(_luceneTool.searchWebPage(search, COUNT));
        }

        private void BindSearchListView(List<WebPage> list)
        {
            SearchListView.Items.Clear();
            foreach (WebPage item in list)
            {
                ListViewItem lvi = new ListViewItem(item.Address);
                lvi.SubItems.Add(item.Hits.ToString());
                SearchListView.Items.Add(lvi);
            }
        }

    }
}
