using BardMusicBinding.Config;
using Godot;

namespace BardMusicBinding
{
	public partial class Keybind : TextureButton
	{
		private PianoKey pianoKey;
		private Label label;
		private CheckButton showKeybindsButton;
		private Node desktop;
		private Label messageLabel;
		private ConfigManager configManager;

		private bool isWaitingForKey = false;
		private string note;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.pianoKey = this.GetParent<PianoKey>();
			this.label = this.GetNode<Label>("KeybindLabel");
			this.showKeybindsButton = this.GetNode<CheckButton>("%ShowKeybindsCheckButton");
			this.desktop = this.GetNode<Node>("/root/Main/Desktop");
			this.messageLabel = this.GetNode<Label>("%MessageLabel");
			this.configManager = this.GetNode<ConfigManager>("/root/Main/ConfigManager");

			note = pianoKey.Name;

			showKeybindsButton.Connect("toggled", new Callable(this, MethodName.OnShowKeybindsButton));
			this.Connect("pressed", new Callable(this, MethodName.OnKeybindButton));

			label.Text = "";
			if (InputMap.HasAction(note))
			{
				var inputEvent = (InputEventKey)InputMap.ActionGetEvents(note)[0];
				var keycode = DisplayServer.KeyboardGetKeycodeFromPhysical(inputEvent.PhysicalKeycode);
				this.label.Text = OS.GetKeycodeString(keycode);
			}
			this.Visible = false;
		}

		public void OnShowKeybindsButton(bool toggled)
		{
			this.Visible = toggled;
		}

		public void OnKeybindButton()
		{
			if (this.ButtonPressed)
			{
				this.StartKeybindPick();
			}
			else
			{
				this.EndKeybindPick();
			}
		}

		private void StartKeybindPick()
		{
			this.isWaitingForKey = true;
			this.desktop.ProcessMode = ProcessModeEnum.Disabled;
			this.ProcessMode = ProcessModeEnum.Always;
			this.messageLabel.Text = $"Press a key to assign to {note}\nESC to unbind or click again to cancel";
			this.messageLabel.Visible = true;
		}

		private void EndKeybindPick()
		{
			this.isWaitingForKey = false;
			this.desktop.ProcessMode = ProcessModeEnum.Inherit;
			this.ProcessMode = ProcessModeEnum.Inherit;
			this.messageLabel.Text = "";
			this.messageLabel.Visible = false;
		}

		public override void _Input(InputEvent @event)
		{
			if (this.ButtonPressed && @event is InputEventKey keyEvent && keyEvent.Pressed && this.isWaitingForKey)
			{
				if (InputMap.HasAction(note))
				{
					InputMap.EraseAction(note);
				}
				if (keyEvent.PhysicalKeycode.ToString() == "Escape")
				{
					this.EndKeybindPick();
					this.ButtonPressed = false;
					this.label.Text = "";
				}
				else
				{
					var keycode = DisplayServer.KeyboardGetKeycodeFromPhysical(keyEvent.PhysicalKeycode);
					this.label.Text = OS.GetKeycodeString(keycode);
					InputMap.AddAction(note);
					InputMap.ActionAddEvent(note, keyEvent);

					this.EndKeybindPick();
					this.ButtonPressed = false;
				}
				this.configManager.SaveConfig();
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
