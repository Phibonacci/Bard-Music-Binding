using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BardMusicBinding.Config;
using Godot;

namespace BardMusicBinding.Controllers
{
    public partial class WindowController : Node
    {
        private ConfigManager configManager;

        private List<Win32.Window.IWindowEntry> registeredWindows = new();
        private Win32.Window.IWindowEntry selected = null;

        public Win32.Window.IWindowEntry Selected
        {
            get { return selected; }
            private set { selected = value; }
        }

        [Signal]
        public delegate void selected_changedEventHandler(string name);

        public override void _Ready()
        {
            this.configManager = this.GetNode<ConfigManager>("/root/Main/ConfigManager");
        }

        public void SelectWindow(long index)
        {
            if (index >= 0 && index < registeredWindows.Count)
            {
                this.Selected = registeredWindows[(int)index];
            }
            else
            {
                this.Selected = null;
            }
            this.EmitSignal(SignalName.selected_changed, Selected?.Title);

        }

        public List<string> GetWindowsTitles() => registeredWindows.Select(w => w.Title).ToList();

        public void UnselectWindow()
        {
            this.SelectWindow(-1);
        }

        private double lastUpdate = 1;
        public override async void _Process(double delta)
        {
            lastUpdate += delta;
            if (lastUpdate < 1)
            {
                return;
            }
            lastUpdate = 0;
            var task = Task.Run(() => Win32.Window.WindowsListFactory.Load().Windows.ToList());
            var windowsList = await task;
            if (this.Selected != null)
            {
                var selectedWindowIndex = windowsList.FindIndex(w => w.HWnd == this.Selected.HWnd);
                if (selectedWindowIndex == -1)
                {
                    this.Selected = null;
                    this.EmitSignal(SignalName.selected_changed, string.Empty);
                }
                else if (this.selected.Title != windowsList[selectedWindowIndex].Title)
                {
                    this.Selected = windowsList[selectedWindowIndex];
                    this.EmitSignal(SignalName.selected_changed, this.Selected?.Title);
                }
            }
            else if (this.configManager.Config.IsAutoLockingWindow && this.configManager.Config.LockedWindow != null)
            {
                var lockedWindowIndex = windowsList.FindIndex(w => w.Title == this.configManager.Config.LockedWindow);
                if (lockedWindowIndex != -1)
                {
                    this.Selected = windowsList[lockedWindowIndex];
                    this.EmitSignal(SignalName.selected_changed, this.Selected?.Title);
                }
            }
            this.registeredWindows = windowsList;
        }
    }
}
