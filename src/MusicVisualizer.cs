using Godot;
using System;

public partial class MusicVisualizer : CanvasLayer
{
	private Window window;
	private Vector2I defaultWindowSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.window = this.GetTree().Root;
		this.defaultWindowSize = this.window.Size;
		this.window.SizeChanged += this.OnWindowResized;
	}

	private void OnWindowResized()
	{
		var ratio = this.window.Size.X / (float)defaultWindowSize.X;
		this.Scale = new Vector2(ratio, ratio);
		this.Offset = new Vector2(0, this.window.Size.Y - defaultWindowSize.Y + defaultWindowSize.Y - defaultWindowSize.Y * ratio);
	}
}
