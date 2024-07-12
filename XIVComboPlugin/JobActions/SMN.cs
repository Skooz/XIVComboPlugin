using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class SMN : JobBase
    {
        public override string JobDisplayName => "Summoner";
        public override uint JobID => JobIDs.SMN;

        public const uint
            Deathflare = 3582,
            EnkindlePhoenix = 16516,
            EnkindleBahamut = 7429,
            DWT = 3581,
            SummonBahamut = 7427,
            FBTLow = 16513,
            FBTHigh = 16549,
            Ruin1 = 163,
            Ruin3 = 3579,
            BrandOfPurgatory = 16515,
            FountainOfFire = 16514,
            Fester = 181,
            Necrotize = 36990,
            EnergyDrain = 16508,
            Painflare = 3578,
            EnergySyphon = 16510;

        private SMNGauge Gauge;
        
        public SMN(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
            Gauge = gauges.Get<SMNGauge>();
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            if (clientState.LocalPlayer == null) return iconHook.Original(self, actionID);
            
            // Change Fester into Energy Drain
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SummonerEDFesterCombo))
            {
                if (actionID == Fester || (actionID == Necrotize && level >= 92))
                {
                    if (!Gauge.HasAetherflowStacks)
                    {
                        return EnergyDrain;
                    }
                    
                    return actionID;
                }
            }

            //Change Painflare into Energy Syphon
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SummonerESPainflareCombo))
            {
                if (actionID == Painflare)
                {
                    if (!Gauge.HasAetherflowStacks)
                    {
                        return EnergySyphon;
                    }
                    
                    return Painflare;
                }
            }
            
            return iconHook.Original(self, actionID);
        }
    }
}
