using BardMusicBinding.Controllers;
using Godot;
using System.Collections.Generic;

namespace BardMusicBinding
{
    public partial class MidiPlayer : Node
    {
        private MidiController midiController;
        private MidiParser.MidiFile parser = null;
        private List<MidiTrackPlayer> tracks;
        private double nextEventTime = 0;
        private double currentTime = 1;
        private bool isPlaying = false;

        [Signal]
        public delegate void stopEventHandler();

        public override void _Ready()
        {
            this.midiController = this.GetNode<MidiController>("/root/Main/MidiController");
        }

        public bool Playing
        {
            get
            {
                return isPlaying;
            }
            set { }
        }

        public void Load(string filePath)
        {
            this.Stop();
            this.parser = new MidiParser.MidiFile(filePath);
            this.tracks = new();
            foreach (var track in parser.Tracks)
            {
                this.tracks.Add(new MidiTrackPlayer(track, parser.TicksPerQuarterNote, this.midiController, new Callable(this, MethodName.UpdateTempo)));
            }
            GD.Print($"Loaded {System.IO.Path.GetFileName(filePath)}");
        }

        public override void _Process(double delta)
        {
            if (!isPlaying || parser == null || parser.Tracks == null || parser.Tracks.Length <= 0)
            {
                return;
            }
            foreach (var track in tracks)
            {
                track.Update(delta);
            }
        }

        public void Pause()
        {
            if (this.isPlaying == true && tracks?.Count > 0)
            {
                this.isPlaying = false;
            }
            foreach (var track in this.tracks)
            {
                track.Pause();
            }
        }

        public void Play()
        {
            if (this.isPlaying == false && tracks?.Count > 0)
            {
                this.isPlaying = true;
            }
        }

        public void Stop()
        {
            this.isPlaying = false;
            if (this.tracks == null)
            {
                return;
            }
            foreach (var track in this.tracks)
            {
                track.Stop();
            }
            this.EmitSignal("stop");
        }

        private void UpdateTempo(double tempo)
        {
            foreach (var track in tracks)
            {
                track.SetTempo(tempo);
            }
        }
    }
}
