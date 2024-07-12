using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class SCH : JobBase
    {
        public override string JobDisplayName => "Scholar";
        public override uint JobID => JobIDs.SCH;

        public const uint
            Physick = 190,
            Adloquium = 185,
            FeyBless = 16543,
            Consolation = 16546,
            EnergyDrain = 167,
            Aetherflow = 166;

        public SCH(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ScholarAdloquiumSync))
            {
                if (actionID == Adloquium)
                {
                    if (level < 30)
                    {
                        return Physick;
                    }
                    
                    return Adloquium;
                }
            }
            
            return iconHook.Original(self, actionID);
        }
    }
}