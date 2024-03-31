using Godot;
namespace BardMusicBinding.Config
{
    public partial class Config : Resource
    {
        [Export]
        public Godot.Collections.Dictionary<string, InputEventKey> Actions { get; set; }

        [Export]
        public BardConfig bardConfig;
    }
}
