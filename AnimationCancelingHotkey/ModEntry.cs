using System.Reflection;
using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AnimationCancelingHotkey
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config;
        private MethodInfo overrideButton;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonsChanged += OnButtonsChanged;
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Config = Helper.ReadConfig<ModConfig>();
            overrideButton = Game1.input.GetType().GetMethod("OverrideButton") ?? throw new InvalidOperationException("Can't find 'OverrideButton' method on SMAPI's input class.");
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            IGenericModConfigMenuApi configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is not null)
            {
                configMenu.Register(
                    mod: ModManifest,
                    reset: () => Config = new ModConfig(),
                    save: () => Helper.WriteConfig(Config)
                );
                configMenu.AddKeybindList(
                    mod: ModManifest,
                    name: () => "AC Keybind",
                    tooltip: () => "The hotkey for the AC script.",
                    getValue: () => Config.ACKey,
                    setValue: value => Config.ACKey = value
                );
            }
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.activeClickableMenu is not null)
                return;

            if (Config is not null && Config.ACKey.JustPressed())
            {
                this.AC();
            }
        }

        // animation cancel
        private void AC()
        {
            this.overrideButton.Invoke(Game1.input, [SButton.RightShift, true]);
            this.overrideButton.Invoke(Game1.input, [SButton.R, true]);
            this.overrideButton.Invoke(Game1.input, [SButton.Delete, true]);
        }

        // private void Log(string msg) // ease of use
        // {
        //     this.Monitor.Log(msg, LogLevel.Debug);
        // }
    }
}