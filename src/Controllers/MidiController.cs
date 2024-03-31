using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace BardMusicBinding.Controllers
{
    public partial class MidiController : Node
    {
        private bool connected = false;
        private WindowController windowController;
        private List<string> inputs = new();
        public List<string> Inputs
        {
            get
            {
                return inputs;
            }
        }

        private class KeyEvent
        {
            public Win32.Window.IWindowEntry window;
            public uint keyCode;
            public bool noteStart;
        }

        Dictionary<uint, Queue<KeyEvent>> keyToKeyEvents = new();
        Dictionary<uint, Task> keyToTask = new();
        HashSet<uint> tasksKeptAlive = new();

        [Signal]
        public delegate void input_changedEventHandler();

        [Signal]
        public delegate void note_sentEventHandler(string note, bool noteStart, bool bindSentToWindow);

        public override void _Ready()
        {
            windowController = this.GetNode<WindowController>("/root/Main/WindowController");
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMidi midiEvent)
            {
                SendKeyFromMidiEvent(midiEvent);
            }
        }

        public override void _ExitTree()
        {
        }

        private static readonly string[] notes =
            { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

        private static string PitchToNote(int pitch)
        {
            // For reference, the matching frequency with a conventional tuning
            // of A4 at 440Hz is:
            // Math.Pow(440 * 2, ((pitch - 69) / 12))
            // Useful link: https://newt.phys.unsw.edu.au/jw/notes.html
            var startingNotePitch = 21; // A0
            var lastNotePitch = 108;
            if (pitch < startingNotePitch || pitch > lastNotePitch)
            {
                return null;
            }
            var increment = pitch - startingNotePitch;
            var note = notes[increment % notes.Length];
            var octave =
                (increment + notes.Length - 3) // we start from A0, we want to start from C0
                / notes.Length;
            return $"{note}{octave}";
        }

        public void SendKeyFromNote(string note, bool noteStart)
        {
            var selectedWindow = this.windowController.Selected;
            var hasBind = InputMap.HasAction(note);
            this.EmitSignal(SignalName.note_sent, note, noteStart, selectedWindow != null && hasBind);
            if (!hasBind)
            {
                GD.Print($"No keybind for note: {note}");
                return;
            }
            var kc = (uint)((InputEventKey)InputMap.ActionGetEvents(note)[0]).PhysicalKeycode;
            //GD.Print($"sending godot key: {kc}");
            if (selectedWindow == null)
            {
                GD.Print($"No window selected ({note})");
                return;
            }
            if (!keyToKeyEvents.ContainsKey(kc))
            {
                keyToKeyEvents[kc] = new();
            }
            keyToKeyEvents[kc].Enqueue(new KeyEvent()
            {
                window = selectedWindow,
                keyCode = kc,
                noteStart = noteStart
            });
            if (!keyToTask.ContainsKey(kc))
            {
                tasksKeptAlive.Add(kc);
                keyToTask[kc] = Task.Run(() => ExecuteKeyEvents(kc));
            }
        }

        private async void ExecuteKeyEvents(uint keyCode)
        {
            while (tasksKeptAlive.Contains(keyCode) || (keyToKeyEvents.ContainsKey(keyCode) && keyToKeyEvents[keyCode].Count > 0))
            {
                ExecuteNextKeyEvent(keyCode);
                await Task.Delay(2);
            }
        }

        private void ExecuteNextKeyEvent(uint keyCode)
        {
            if (keyToKeyEvents.ContainsKey(keyCode) && keyToKeyEvents[keyCode].Count > 0)
            {
                var keyEvent = keyToKeyEvents[keyCode].Dequeue();
                Win32.KeyStroke.SendKeystroke(keyEvent.window, keyEvent.keyCode, keyEvent.noteStart);
            }
        }

        public void SendKeyFromMidiEvent(InputEventMidi midiEvent)
        {
            var note = PitchToNote(midiEvent.Pitch);
            GD.Print($"music note: {note}");
            var message = midiEvent.Message.ToString();
            if (message == "NoteOn")
            {
                this.SendKeyFromNote(note, true);
            }
            else if (message == "NoteOff")
            {
                this.SendKeyFromNote(note, false);
            }
            else
            {
                GD.Print($"Unknown MIDI event: {message}");
            }
        }

        private static void PrintMIDIInfo(InputEventMidi midiEvent)
        {
            GD.Print(midiEvent);
            GD.Print($"Channel {midiEvent.Channel}");
            GD.Print($"Message {midiEvent.Message}");
            GD.Print($"Pitch {midiEvent.Pitch}");
            GD.Print($"Velocity {midiEvent.Velocity}");
            GD.Print($"Instrument {midiEvent.Instrument}");
            GD.Print($"Pressure {midiEvent.Pressure}");
            GD.Print($"Controller number: {midiEvent.ControllerNumber}");
            GD.Print($"Controller value: {midiEvent.ControllerValue}");
        }

        private void Connect()
        {
            if (connected)
            {
                OS.CloseMidiInputs();
            }
            else
            {
                connected = true;
            }
            OS.OpenMidiInputs();
        }

        private void UpdateInputs()
        {
            var newInputs = OS.GetConnectedMidiInputs().ToList();
            if (this.inputs.SequenceEqual(newInputs))
            {
                return;
            }
            this.inputs = newInputs;
            this.EmitSignal(SignalName.input_changed);
        }

        public void RefreshDevices()
        {
            this.Connect();
            this.UpdateInputs();
        }
    }
}
