using Godot;

namespace BardMusicBinding.Config
{
    public partial class BardConfig : Resource
    {
        [Export]
        public string LockedWindow { get; set; }

        [Export]
        public bool IsAutoLockingWindow { get; set; }

        [Export]
        public bool IsShowingKeyBinds { get; set; }

        [Export]
        public bool IsShowingNotes { get; set; }

        [Export]
        public bool IsShowingMidi { get; set; }
    }
}
