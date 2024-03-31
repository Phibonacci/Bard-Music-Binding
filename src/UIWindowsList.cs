using BardMusicBinding.Config;
using BardMusicBinding.Controllers;
using Godot;
using System.Collections.Generic;

namespace BardMusicBinding
{
	public partial class UIWindowsList : MenuButton
	{
		private WindowController windowController;
		private ConfigManager configManager;
		private CheckButton autolockButton;

		readonly List<Win32.Window.IWindowEntry> RegisteredWindows = new();
		private Win32.Window.IWindowEntry selected = null;
		public Win32.Window.IWindowEntry Selected
		{
			get { return selected; }
			private set { selected = value; }
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

			this.windowController = this.GetNode<WindowController>("/root/Main/WindowController");
			this.configManager = this.GetNode<ConfigManager>("/root/Main/ConfigManager");
			this.autolockButton = this.GetNode<CheckButton>("%AutolockCheckButton");

			this.UpdateText(null);
			this.windowController.Connect("selected_changed", new Callable(this, MethodName.OnSelectedWindowChanged));
			this.GetPopup().IndexPressed += (id) => this.OnSelectWindow(id);

			this.Connect("about_to_popup", new Callable(this, MethodName.OnAboutToPopup));
			var unsetWindowButton = this.GetNode<Button>("%UnsetWindowButton");
			unsetWindowButton.Connect("pressed", new Callable(this, MethodName.OnUnsetWindowButtonPressed));

			this.autolockButton.Connect("toggled", new Callable(this, MethodName.OnAutolockButtonToggled));
			this.autolockButton.ButtonPressed = this.configManager.Config.IsAutoLockingWindow;
		}

		private void OnUnsetWindowButtonPressed()
		{
			this.selected = null;
			this.autolockButton.ButtonPressed = false;
			this.windowController.UnselectWindow();
		}

		private void OnAutolockButtonToggled(bool toggled)
		{
			this.configManager.Config.IsAutoLockingWindow = toggled;
			if (!toggled)
			{
				this.configManager.Config.LockedWindow = null;
			}
			else
			{
				if (this.windowController.Selected != null)
				{
					this.configManager.Config.LockedWindow = this.windowController.Selected.Title;
				}
			}
		}

		private void OnSelectedWindowChanged(string windowName)
		{
			if (this.configManager.Config.IsAutoLockingWindow && windowName != null && windowName.Length > 0)
			{
				this.configManager.Config.LockedWindow = windowName;
			}
			UpdateText(windowName);
		}

		private void UpdateText(string windowName)
		{
			if (windowName == null || windowName.Length == 0)
			{
				this.Text = "<no window selected>";
			}
			else
			{
				this.Text = windowName;
			}
		}

		private void OnSelectWindow(long index)
		{
			this.windowController.SelectWindow(index);
		}

		double lastUpdate = 0;

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public Win32.Window.IWindowEntry GetSelectedWindow()
		{
			var focused = this.GetPopup().GetFocusedItem();
			if (focused == -1)
			{
				return null;
			}
			return this.RegisteredWindows[focused];
		}

		public void OnAboutToPopup()
		{
			var popup = this.GetPopup();
			popup.Clear();
			this.windowController.GetWindowsTitles().ForEach(t => popup.AddItem(t));
			GD.Print($"{this.Size.X} and {popup.Size.X}");
			//popup.Position = (Vector2I)new Vector2(this.Position.X + this.Size.X / 2 - popup.Size.X / 2, popup.Position.Y);
			popup.MaxSize = new Vector2I(GetViewport().GetWindow().Size.X, popup.MaxSize.Y);
		}
	}
}
