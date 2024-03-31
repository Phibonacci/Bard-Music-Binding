using BardMusicBinding.Controllers;
using Godot;

namespace BardMusicBinding
{
	public partial class LoadMidiFileButton : Button
	{
		private MidiPlayer midiPlayer;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.midiPlayer = this.GetNode<MidiPlayer>("/root/Main/MidiPlayer");
			this.Connect("pressed", new Callable(this, MethodName.OnButtonPressed));
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		private void OnButtonPressed()
		{
			DisplayServer.FileDialogShow("Pick a MIDI file", null, null, false, DisplayServer.FileDialogMode.OpenFile, new string[] { "*.midi, *.mid" }, new Callable(this, MethodName.OnFilePicked));
		}

		private void OnFilePicked(bool status, string[] selected_paths, int selected_filter_index)
		{
			if (selected_paths != null && selected_paths.Length > 0)
			{
				this.midiPlayer.Load(selected_paths[0]);
			}
		}
	}
}
