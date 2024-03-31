using Godot;
using System;

public partial class SettingsPanelButton : Button
{
	CanvasLayer settings;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.settings = this.GetNode<CanvasLayer>("/root/Main/Settings");
		this.Connect("pressed", new Callable(this, MethodName.OnPressed));
	}

	private void OnPressed()
	{
		this.settings.Visible = !this.settings.Visible;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
