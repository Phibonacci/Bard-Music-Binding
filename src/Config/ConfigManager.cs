using Godot;

namespace BardMusicBinding.Config
{
    public partial class ConfigManager : Node
    {
        private Config config = new()
        {
            bardConfig = new()
        };

        public BardConfig Config
        {
            get
            {
                return config.bardConfig;
            }
            private set { }
        }

        public override void _EnterTree()
        {
            this.LoadConfig();
        }

        public override void _ExitTree()
        {
            this.SaveConfig();
        }

        public void SaveConfig()
        {
            this.config.Actions = new();
            foreach (var actionName in InputMap.GetActions())
            {
                if (InputMap.ActionGetEvents(actionName).Count <= 0 || InputMap.ActionGetEvents(actionName)[0] is not InputEventKey)
                {
                    continue;
                }
                this.config.Actions.Add(actionName, (InputEventKey)InputMap.ActionGetEvents(actionName)[0]);
            }
            ResourceSaver.Save(this.config, "user://bard-music-binding-config.tres");
        }

        public void LoadConfig()
        {
            Config config;
            try
            {
                config = ResourceLoader.Load<Config>("user://bard-music-binding-config.tres");
            }
            catch (System.InvalidCastException)
            {
                GD.Print("No config file found");
                return;
            }
            if (config == null || config.Actions.Count == 0)
            {
                GD.Print("Empty ressource loaded");
                return;
            }
            this.config = config;
            foreach (var action in InputMap.GetActions())
            {
                InputMap.EraseAction(action);
            }
            foreach (var item in this.config.Actions)
            {
                InputMap.AddAction(item.Key);
                InputMap.ActionAddEvent(item.Key, item.Value);
            }
            if (config.bardConfig == null)
            {
                config.bardConfig = new();
            }
        }
    }
}
