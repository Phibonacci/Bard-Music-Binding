using Godot;

namespace BardMusicBinding
{
	public partial class StopMidiFileButton : Button
	{
		MidiPlayer midiPlayer;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.midiPlayer = this.GetNode<MidiPlayer>("/root/Main/MidiPlayer");

			this.Connect("pressed", new Callable(this, MethodName.OnPressed));
		}

		private void OnPressed()
		{
			this.midiPlayer.Stop();
		}
	}
}
