using BardMusicBinding.Controllers;
using Godot;

namespace BardMusicBinding
{
	public partial class PianoKey : TextureButton
	{
		private bool isSharp;
		private Texture2D normalTexturePressed;
		private Texture2D errorTexturePressed;
		private string noteName;

		private MidiController midiController;
		private Keybind keybindButton;
		private CanvasLayer desktop;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			noteName = this.Name.ToString();

			this.isSharp = noteName.Contains('#');
			var texturePath = isSharp ? "res://assets/images/piano/Flats + Sharps/RedPressed.png" : "res://assets/images/piano/Keys/Red1Pressed.png";
			this.normalTexturePressed = this.TexturePressed;
			this.errorTexturePressed = GD.Load<Texture2D>(texturePath);

			this.midiController = this.GetNode<MidiController>("/root/Main/MidiController");
			this.keybindButton = this.GetNode<Keybind>("Keybind");
			this.desktop = this.GetNode<CanvasLayer>("/root/Main/Desktop");

			this.Connect("gui_input", new Callable(this, MethodName.OnGuiInput));
			this.Connect("mouse_entered", new Callable(this, MethodName.OnMouseEntered));
			this.Connect("mouse_exited", new Callable(this, MethodName.OnMouseExited));
			this.midiController.Connect("note_sent", new Callable(this, MethodName.OnNoteSent));
		}

		public override void _ExitTree()
		{
			if (this.ButtonPressed)
			{
				this.midiController.SendKeyFromNote(this.noteName, false);
			}
		}

		private bool isMousePressed = false;
		private bool isEventPressed = false;

		private void OnGuiInput(InputEvent @event)
		{
			if (@event is InputEventMouseButton mouseEvent)
			{
				if (mouseEvent.IsPressed())
				{
					this.MousePressKey();
				}
				else
				{
					this.MouseReleaseKey();
				}
			}
		}

		private void OnMouseEntered()
		{
			if (this.desktop.ProcessMode != ProcessModeEnum.Disabled && Input.IsMouseButtonPressed(MouseButton.Left))
			{
				this.MousePressKey();
			}
		}

		private void OnMouseExited()
		{
			this.MouseReleaseKey();
		}

		private void OnNoteSent(string note, bool noteStart, bool bindSentToWindow)
		{
			if (note != this.noteName)
			{
				return;
			}
			if (noteStart)
			{
				EventPressKey(bindSentToWindow);
			}
			else
			{
				EventReleaseKey();
			}
		}

		private void MousePressKey()
		{
			if (!this.isMousePressed)
			{
				this.isMousePressed = true;
				this.midiController.SendKeyFromNote(this.noteName, true);
			}
		}

		private void MouseReleaseKey()
		{
			if (this.isMousePressed)
			{
				isMousePressed = false;
				this.midiController.SendKeyFromNote(this.noteName, false);
			}
		}

		public void EventPressKey(bool bindSentToWindow)
		{
			isEventPressed = true;
			this.ButtonPressed = true;
			if (this.keybindButton != null)
			{
				this.keybindButton.ButtonPressed = true;
				var offset = this.isSharp ? 2 : 2;
				this.keybindButton.Position = new Vector2(this.keybindButton.Position.X, this.keybindButton.Position.Y + offset);
			}
			if (bindSentToWindow)
			{
				this.TexturePressed = this.normalTexturePressed;
			}
			else
			{
				this.TexturePressed = this.errorTexturePressed;
			}
		}

		public void EventReleaseKey()
		{
			isEventPressed = false;
			if (!isMousePressed)
			{
				if (this.keybindButton != null)
				{
					this.keybindButton.ButtonPressed = false;
					var offset = this.isSharp ? 2 : 2;
					this.keybindButton.Position = new Vector2(this.keybindButton.Position.X, this.keybindButton.Position.Y - offset);
				}
				this.ButtonPressed = false;
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (!Input.IsMouseButtonPressed(MouseButton.Left))
			{
				this.MouseReleaseKey();
			}
		}
	}
}
