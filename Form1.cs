using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
namespace nhentai_dl_gui
{
    public partial class Form1 : Form
    {
        HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
        public Form1()
        {
            InitializeComponent();
        }

        public string getPages(string hentaiId)
        {
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://nhentai.net/g/" + hentaiId + "/1");
            var tagToLookFor = doc.DocumentNode.SelectSingleNode("//span[@class='num-pages']");
            return tagToLookFor.InnerText;
        }
        public void downloadHentai(string hentaiID)
        {
            string mediaID = getMediaId(hentaiID);
            int pages = Int32.Parse(getPages(hentaiID));
            string urlToDownloadFrom = "https://i.nhentai.net" + "/galleries/" + mediaID + "/";
            string pathToSave = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + hentaiID + "\\";
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }
            MessageBox.Show("The program might stop responding while it is downloading, don't worry!");
            for (int i = 1; i < pages + 1; i++)
            {
                progressBar1.Value = i * progressBar1.Maximum / pages;
                Application.DoEvents();
                label2.Text = "Progress: " + i + " of " + pages + " downloaded";
                using (var client = new WebClient())
                {
                    client.DownloadFile(urlToDownloadFrom + i.ToString() + ".jpg", @"" + pathToSave + i.ToString() + ".jpg");
                }
            }
            MessageBox.Show("Your hentai has been saved to: " + pathToSave);

            

            
        }

        public string getMediaId(string hentaiId)
        {
            string mediaID = "";
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://nhentai.net/g/" + hentaiId + "/1");
            HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img[@src]");
            foreach (HtmlNode img in imgs)
            {
                
                HtmlAttribute src = img.Attributes["src"];
                var urlToTest = src.Value;
                if (urlToTest.StartsWith("https://i.nhentai.net"))
                {
                    mediaID = urlToTest;
                    break;
                }
            }
            string[] parts = mediaID.Split('/');
            string realMediaID = parts[4];
            return realMediaID;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var test = textBox1.Text;
            if (test == "" || test == null) {
                MessageBox.Show("The text box can't be empty.");
            }
            else
            {
                var res = getPages(test);
                label1.Text = "Pages available: " + res;
                var lol = getMediaId(test);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var test = textBox1.Text;
            if (test == "" || test == null)
            {
                MessageBox.Show("The text box can't be empty.");
            }
            else
            {
                downloadHentai(test);
            }
        }

        private void ProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int id = rnd.Next(1, 100000);
            textBox1.Text = id.ToString();
        }
    }
}
