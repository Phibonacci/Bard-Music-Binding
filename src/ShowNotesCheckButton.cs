using BardMusicBinding.Config;
using Godot;

namespace BardMusicBinding
{
	public partial class ShowNotesCheckButton : CheckButton
	{
		private ConfigManager configManager;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.configManager = this.GetNode<ConfigManager>("/root/Main/ConfigManager");

			this.Connect("toggled", new Callable(this, MethodName.OnButtonToggled));
			this.ButtonPressed = this.configManager.Config.IsShowingNotes;
		}

		private void OnButtonToggled(bool toggled)
		{
			this.configManager.Config.IsShowingNotes = toggled;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
