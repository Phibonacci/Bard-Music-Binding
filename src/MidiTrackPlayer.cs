using System.Collections.Generic;
using BardMusicBinding.Controllers;
using Godot;

namespace BardMusicBinding
{
    public partial class MidiTrackPlayer
    {
        private int nextEventId = 0;
        private double nextEventTime = 0;
        private double currentTime = 0;
        private readonly int ticksPerQuarterNote;
        private double secondsPerTick = 0; // should be overriden by a midievent
        private readonly MidiController midiController;
        private readonly Callable onUpdateTempo;
        private readonly MidiParser.MidiTrack track;
        private readonly HashSet<int> playingNotes = new();

        public MidiTrackPlayer(MidiParser.MidiTrack track,
            int ticksPerQuarterNote,
            MidiController midiController,
            Callable onUpdateTempo)
        {
            this.track = track;
            this.ticksPerQuarterNote = ticksPerQuarterNote;
            this.midiController = midiController;
            this.onUpdateTempo = onUpdateTempo;
            if (this.nextEventId < this.track.MidiEvents.Count)
            {
                nextEventTime = this.track.MidiEvents[nextEventId].Time / 1000d; // should be 0
            }
        }

        public void SetTempo(double tempo)
        {
            if (this.secondsPerTick != 0)
            {
                this.currentTime = this.currentTime / this.secondsPerTick * tempo;
                this.nextEventTime = this.nextEventTime / this.secondsPerTick * tempo;
            }
            this.secondsPerTick = tempo;
        }

        public void Update(double delta)
        {
            if (this.track.MidiEvents.Count <= this.nextEventId)
            {
                return;
            }
            this.currentTime += delta;
            while (PlayEvent()) { }
        }

        public void Pause()
        {
            foreach (var note in this.playingNotes)
            {
                var godotMidiEvent = new InputEventMidi
                {
                    Pitch = note,
                    Message = MidiMessage.NoteOff,
                };
                this.midiController.SendKeyFromMidiEvent(godotMidiEvent);
            }
        }

        public void Stop()
        {
            foreach (var note in this.playingNotes)
            {
                var godotMidiEvent = new InputEventMidi
                {
                    Pitch = note,
                    Message = MidiMessage.NoteOff,
                };
                this.midiController.SendKeyFromMidiEvent(godotMidiEvent);
            }
            this.nextEventTime = 0;
            this.nextEventId = 0;
            this.currentTime = 0;
        }

        private bool PlayEvent()
        {
            if (currentTime <= nextEventTime || this.nextEventId >= this.track.MidiEvents.Count)
            {
                return false;
            }
            var midiEvent = this.track.MidiEvents[nextEventId];
            if (midiEvent.MidiEventType == MidiParser.MidiEventType.NoteOn)
            {
                var godotMidiEvent = new InputEventMidi
                {
                    Pitch = midiEvent.Note,
                    Message = MidiMessage.NoteOn,
                };
                this.midiController.SendKeyFromMidiEvent(godotMidiEvent);
                this.playingNotes.Add(midiEvent.Note);
            }
            else if (midiEvent.MidiEventType == MidiParser.MidiEventType.NoteOff)
            {
                var godotMidiEvent = new InputEventMidi
                {
                    Pitch = midiEvent.Note,
                    Message = MidiMessage.NoteOff,
                };
                this.midiController.SendKeyFromMidiEvent(godotMidiEvent);
                this.playingNotes.Remove(midiEvent.Note);
            }
            else if (midiEvent.MidiEventType == MidiParser.MidiEventType.MetaEvent
                && midiEvent.Arg1 == (byte)MidiParser.MetaEventType.Tempo)
            {
                // https://stackoverflow.com/a/54754549
                var microsecondPerQuarter = (double)midiEvent.Arg2;
                var secondsPerTick = microsecondPerQuarter / ticksPerQuarterNote / 1000d;
                this.onUpdateTempo.Call(secondsPerTick);
            }
            this.nextEventId += 1;
            if (this.nextEventId < this.track.MidiEvents.Count)
            {
                nextEventTime = this.track.MidiEvents[nextEventId].Time / 1000d * this.secondsPerTick;
            }
            return true;
        }
    }
}
