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

        public MenuItem MouseMovementItem { get; }
        public MenuItem MouseInteractionItem { get; }

        public MenuItem KeyboardItem { get; }

        public MenuItem PopupItem { get; }

        public MenuItem ToggleRendering { get; }

        public MenuItem DevToolsItem { get; }
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

            MouseMovementItem = new MenuItem
            {
                Text = "Enable Mouse Movement"
            };

            MouseInteractionItem = new MenuItem
            {
                Text = "Enable Mouse Interaction"
            };

            KeyboardItem = new MenuItem
            {
                Text = "Enable Keyboard"
            };

            PopupItem = new MenuItem
            {
                Text = "Redirect new window to wallpaper"
            };

            ToggleRendering = new MenuItem
            {
                Text = "Toggle Rendering"
            };

            DevToolsItem = new MenuItem
            {
                Text = "Show DevTools"
            };

            ExitItem = new MenuItem
            {
                Text = "Exit"
            };

            Menu.MenuItems.AddRange(new MenuItem[]{
                WaterMarkItem,
                BarItem,
                SetURLItem,
                MouseMovementItem,
                MouseInteractionItem,
                KeyboardItem,
                PopupItem,
                ToggleRendering,
                DevToolsItem,
                ExitItem
            });
        }

        public void Hook(NotifyIcon icon)
        {
            icon.ContextMenu = Menu;
        }

    }
}
