using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class DRK : JobBase
    {
        public override string JobDisplayName => "Dark Knight";
        public override uint JobID => JobIDs.DRK;

        public const uint
            Souleater = 3632,
            HardSlash = 3617,
            SyphonStrike = 3623,
            StalwartSoul = 16468,
            Unleash = 3621;

        public DRK(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace Souleater with Souleater combo chain
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DarkSouleaterCombo))
            {
                if (actionID == Souleater)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == HardSlash && level >= 2)
                            return SyphonStrike;
                        if (lastMove == SyphonStrike && level >= 26)
                            return Souleater;
                    }

                    return HardSlash;
                }
            }

            // Replace Stalwart Soul with Stalwart Soul combo chain
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DarkStalwartSoulCombo))
            {
                if (actionID == StalwartSoul)
                {
                    if (comboTime > 0)
                        if (lastMove == Unleash && level >= 40)
                            return StalwartSoul;

                    return Unleash;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}