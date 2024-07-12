using Dalamud.Game;
using Dalamud.Hooking;
using XIVComboPlugin.JobActions;
using FFXIVClientStructs.FFXIV.Client.Game;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin
{
    public class IconReplacer
    {
        public delegate ulong OnCheckIsIconReplaceableDelegate(uint actionID);

        public delegate ulong OnGetIconDelegate(byte param1, uint param2);

        private readonly IconReplacerAddressResolver Address;
        private readonly Hook<OnCheckIsIconReplaceableDelegate> checkerHook;
        private readonly IClientState clientState;

        private readonly Hook<OnGetIconDelegate> iconHook;

        private IGameInteropProvider HookProvider;
        private IPluginLog PluginLog;

        private unsafe delegate int* getArray(long* address);

        public IconReplacer(ISigScanner scanner, IClientState clientState, IGameInteropProvider hookProvider, IPluginLog pluginLog)
        {
            HookProvider = hookProvider;
            this.clientState = clientState;
            PluginLog = pluginLog;

            Address = new IconReplacerAddressResolver(scanner);

            iconHook = HookProvider.HookFromAddress<OnGetIconDelegate>(ActionManager.Addresses.GetAdjustedActionId.Value, GetIconDetour);
            checkerHook = HookProvider.HookFromAddress<OnCheckIsIconReplaceableDelegate>(Address.IsIconReplaceable, CheckIsIconReplaceableDetour);
            HookProvider = hookProvider;
        }

        public void Enable()
        {
            iconHook.Enable();
            checkerHook.Enable();
        }

        public void Dispose()
        {
            iconHook.Dispose();
            checkerHook.Dispose();
        }

        // I hate this function. This is the dumbest function to exist in the game. Just return 1.
        // Determines which abilities are allowed to have their icons updated.

        private ulong CheckIsIconReplaceableDetour(uint actionID)
        {
            return 1;
        }

        /// <summary>
        ///     Replace an ability with another ability
        ///     actionID is the original ability to be "used"
        ///     Return either actionID (itself) or a new Action table ID as the
        ///     ability to take its place.
        ///     I tend to make the "combo chain" button be the last move in the combo
        ///     For example, Souleater combo on DRK happens by dragging Souleater
        ///     onto your bar and mashing it.
        /// </summary>
        private ulong GetIconDetour(byte self, uint actionID)
        {
            if (clientState.LocalPlayer == null) return iconHook.Original(self, actionID);
            
            JobBase job = XIVComboPlugin.GetJob(clientState.LocalPlayer.ClassJob.Id);
            return job?.IconDetour(iconHook, self, actionID) ?? iconHook.Original(self, actionID);
        }
    }
}
