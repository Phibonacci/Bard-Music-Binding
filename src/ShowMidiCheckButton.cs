using BardMusicBinding.Config;
using Godot;

namespace BardMusicBinding
{
	public partial class ShowMidiCheckButton : CheckButton
	{
		private ConfigManager configManager;
		private CanvasItem midiPlayerControls;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.configManager = this.GetNode<ConfigManager>("/root/Main/ConfigManager");
			this.midiPlayerControls = this.GetNode<CanvasItem>("%MidiPlayerControls");

			this.Connect("toggled", new Callable(this, MethodName.OnButtonToggled));
			this.ButtonPressed = this.configManager.Config.IsShowingMidi;
			this.midiPlayerControls.Visible = this.configManager.Config.IsShowingMidi;
		}

		private void OnButtonToggled(bool toggled)
		{
			this.configManager.Config.IsShowingMidi = toggled;
			this.midiPlayerControls.Visible = toggled;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
