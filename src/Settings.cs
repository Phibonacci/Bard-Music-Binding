using Godot;
using System;

public partial class Settings : CanvasLayer
{
	CanvasLayer desktop;
	PanelContainer settingsPanel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.desktop = this.GetNode<CanvasLayer>("/root/Main/Desktop");
		this.settingsPanel = this.GetNode<PanelContainer>("SettingsPanel");

		this.Connect("visibility_changed", new Callable(this, MethodName.OnVisible));
	}

	public override void _Input(InputEvent @event)
	{
		if (this.Visible && @event is InputEventMouseButton && @event.IsPressed())
		{
			var localEvent = (InputEventMouseButton)this.settingsPanel.MakeInputLocal(@event);
			var panelZone = new Rect2(new Vector2I(0, 0), this.settingsPanel.Size);
			if (!panelZone.HasPoint(localEvent.Position))
			{
				this.Visible = false;
				this.settingsPanel.AcceptEvent();
			}
		}
	}

	private void OnVisible() => this.desktop.ProcessMode = this.Visible ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
