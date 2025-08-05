using StardewModdingAPI.Utilities;

namespace AnimationCancelingHotkey
{
    public sealed class ModConfig
    {
        public bool Enable { get; set; } = true;
        public KeybindList ACKey { get; set; } = KeybindList.Parse("Space");
    }
}