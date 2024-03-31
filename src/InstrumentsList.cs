using BardMusicBinding.Controllers;
using Godot;
using System.Collections.Generic;

namespace BardMusicBinding
{

	public partial class InstrumentsList : ItemList
	{
		private List<string> midiInputs = new();
		private MidiController midiController;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.midiController = this.GetNode<MidiController>("/root/Main/MidiController");
			this.midiController.Connect("input_changed", new Callable(this, MethodName.OnMidiInputChanged));
			midiController.RefreshDevices();
			var button = this.GetParent().GetNode<Button>("%RefreshInstrumentsButton");
			button.Connect("pressed", new Callable(this, MethodName.OnRefreshInstrumentsButtonPressed));
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{

		}

		private void OnMidiInputChanged()
		{
			this.midiInputs = midiController.Inputs;
			this.Clear();
			foreach (var midiInput in midiInputs)
			{
				this.AddItem(midiInput);
			}
		}

		private void OnRefreshInstrumentsButtonPressed()
		{
			midiController.RefreshDevices();
		}
	}
}
