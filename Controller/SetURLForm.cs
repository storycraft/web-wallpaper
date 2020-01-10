using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using web_wallpaper.Wallpaper;

namespace web_wallpaper.Controller
{
    public partial class SetURLForm : Form
    {

        private WallpaperManager Manager { get; }
        public SetURLForm(WallpaperManager manager)
        {
            Manager = manager;
            InitializeComponent();

            urlBox.Text = manager.URL;

            selectBtn.Select();
        }

        protected void UpdateURL()
        {
            if (urlBox.Text.Length > 0)
            {
                Manager.URL = urlBox.Text;
            }

            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateURL();
        }
    }
}
