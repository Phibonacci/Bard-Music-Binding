using Godot;

namespace BardMusicBinding
{
	public partial class TopRightControls : HBoxContainer
	{
		private Window window;
		private Vector2I defaultWindowSize;
		private Vector2 defaultPosition;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.window = this.GetTree().Root;
			this.defaultWindowSize = this.window.Size;
			this.defaultPosition = this.Position;
			this.window.SizeChanged += this.OnWindowResized;
		}

		private void OnWindowResized()
		{
			this.Position = new Vector2(defaultPosition.X + this.window.Size.X - defaultWindowSize.X, this.Position.Y);
		}
	}
}
