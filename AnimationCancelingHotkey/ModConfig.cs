using StardewModdingAPI.Utilities;

namespace AnimationCancelingHotkey
{
    public sealed class ModConfig
    {
        public KeybindList ACKey { get; set; } = KeybindList.Parse("Space");
    }
}