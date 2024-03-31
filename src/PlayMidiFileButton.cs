using Godot;

namespace BardMusicBinding
{
	public partial class PlayMidiFileButton : Button
	{
		MidiPlayer midiPlayer;

		Texture2D playTexture;
		Texture2D pauseTexture;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.midiPlayer = this.GetNode<MidiPlayer>("/root/Main/MidiPlayer");

			this.playTexture = GD.Load<Texture2D>("res://assets/images/icons/play.png");
			this.pauseTexture = GD.Load<Texture2D>("res://assets/images/icons/pause.png");

			this.midiPlayer.Connect("stop", new Callable(this, MethodName.OnStop));
			this.Connect("pressed", new Callable(this, MethodName.OnPressed));
		}

		private void OnStop()
		{
			this.Icon = this.midiPlayer.Playing ? this.pauseTexture : this.playTexture;
		}

		private void OnPressed()
		{

			if (this.midiPlayer.Playing)
			{
				this.midiPlayer.Pause();
			}
			else
			{
				this.midiPlayer.Play();
			}
			this.Icon = this.midiPlayer.Playing ? this.pauseTexture : this.playTexture;
		}
	}
}
