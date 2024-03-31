using Godot;
using System;

public partial class Message : CanvasLayer
{
	private Window window;
	private Label message;
	private Vector2I defaultWindowSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.window = this.GetTree().Root;
		this.message = this.GetNode<Label>("%MessageLabel");

		this.defaultWindowSize = this.window.Size;
		this.window.SizeChanged += this.OnWindowResized;
	}

	private void OnWindowResized()
	{
		var ratio = this.window.Size.X / (float)defaultWindowSize.X;
		this.Scale = new Vector2(ratio, ratio);
		this.message.Position = new Vector2(
			this.window.Size.X / 2 - this.message.Size.X / 2 * ratio,
			(this.window.Size.Y / 2 * 1 / ratio) - this.message.Size.Y / 2 * ratio);
	}
}
