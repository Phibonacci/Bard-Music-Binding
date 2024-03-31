using Godot;

namespace BardMusicBinding
{
	public partial class Main : Node
	{
		public override void _EnterTree()
		{
			DisplayServer.WindowSetMinSize(new Vector2I(420, 422));
		}

		public override void _ExitTree()
		{
			// Before leaving we want to make sure the keys have time to end the notes.
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
