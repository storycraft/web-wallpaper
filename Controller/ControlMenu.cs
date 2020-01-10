using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace web_wallpaper.Controller
{
    public class ControlMenu
    {
        private ContextMenu Menu { get; }

        private MenuItem BarItem { get; }

        public MenuItem WaterMarkItem { get; }
        public MenuItem SetURLItem { get; }
        public MenuItem ExitItem { get; }
        public ControlMenu()
        {
            Menu = new ContextMenu();

            BarItem = new MenuItem
            {
                Text = "-"
            };

            WaterMarkItem = new MenuItem
            {
                Text = "WebWallpaper by storycraft",
                Enabled = false
            };

            SetURLItem = new MenuItem
            {
                Text = "Set URL"
            };

            ExitItem = new MenuItem
            {
                Text = "Exit"
            };

            Menu.MenuItems.AddRange(new MenuItem[]{
                WaterMarkItem,
                BarItem,
                SetURLItem,
                ExitItem
            });
        }

        public void Hook(NotifyIcon icon)
        {
            icon.ContextMenu = Menu;
        }

    }
}
