using System.Collections.Generic;
using System.Linq;
using BardMusicBinding;
using BardMusicBinding.Controllers;
using Godot;

public partial class Timeline : Node2D
{
	private MidiController midiController;
	private Node2D keys;
	private PackedScene scene;

	class LastNote
	{
		public Control node;
		public double time;
	}

	private Dictionary<string, LastNote> lastNodeFromNote = new();
	private Dictionary<string, PianoKey> pianoKeys = new();
	private float speed = 120; // px/s

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.midiController = this.GetNode<MidiController>("/root/Main/MidiController");
		this.keys = this.GetNode<Node2D>("/root/Main/Desktop/MusicVisualizer/Piano/Keys");
		this.scene = GD.Load<PackedScene>("res://nodes/time_note.tscn");
		this.midiController.Connect("note_sent", new Callable(this, MethodName.OnNoteSent));
		foreach (PianoKey key in keys.GetChildren().Cast<PianoKey>())
		{
			pianoKeys.Add(key.Name, key);
		}
	}

	private void OnNoteSent(string note, bool noteStart, bool bindSentToWindow)
	{
		if (noteStart)
		{
			var node = (Control)this.scene.Instantiate();
			var key = pianoKeys[note];
			node.ZIndex = key.Name.ToString().Contains('#') ? 2 : 1;
			node.Size = new Vector2(key.Size.X, node.Size.Y);
			node.Position = new Vector2(key.Position.X, keys.GlobalPosition.Y);
			if (!bindSentToWindow)
			{
				var shader = (ShaderMaterial)node.GetChild<Control>(1).Material;
				shader.SetShaderParameter("color", new Color(0.2f, 0.2f, 0.2f));
			}
			this.AddChild(node);
			lastNodeFromNote[note] = new LastNote()
			{
				node = node,
				time = 0d
			};
		}
		else
		{
			if (lastNodeFromNote.ContainsKey(note))
			{
				lastNodeFromNote.Remove(note);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		List<Control> toRemove = new();
		foreach (Control node in GetChildren().Cast<Control>())
		{
			if (node.Position.Y + node.Size.Y < -10000)
			{
				toRemove.Add(node);
			}
			node.Position = new Vector2(node.Position.X, (float)(node.Position.Y - speed * delta));
		}
		foreach (var pair in lastNodeFromNote)
		{
			pair.Value.time += delta;
			var node = pair.Value.node;
			var time = pair.Value.time;
			node.Size = new Vector2(node.Size.X, (float)(time * speed));
		}
		foreach (var node in toRemove)
		{
			this.RemoveChild(node);
		}
	}
}
