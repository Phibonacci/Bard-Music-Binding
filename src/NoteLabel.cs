using Godot;

namespace BardMusicBinding
{
	public partial class NoteLabel : Label
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var note = this.GetParent<PianoKey>().Name.ToString();
			this.Text = note.Length == 3 ? "" + note[0] + note[2] : note;
			var showNotesButton = this.GetNode<CheckButton>("%ShowNotesCheckButton");
			showNotesButton.Connect("toggled", new Callable(this, MethodName.OnShowNotesButton));
			this.Visible = false;
		}

		public void OnShowNotesButton(bool toggled)
		{
			this.Visible = toggled;
		}
	}
}
