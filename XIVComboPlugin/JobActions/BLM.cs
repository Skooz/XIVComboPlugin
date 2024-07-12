using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class BLM : JobBase
    {
        public override string JobDisplayName => "Black Mage";
        public override uint JobID => JobIDs.BLM;

        public const uint
            Blizzard4 = 3576,
            Fire4 = 3577,
            Transpose = 149,
            UmbralSoul = 16506,
            LeyLines = 3573,
            BTL = 7419,
            Flare = 162,
            Freeze = 159;

        public const ushort BuffLeyLines = 737;

        private BLMGauge Gauge;

        public BLM(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
            Gauge = gauges.Get<BLMGauge>();
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // B4 and F4 change to each other depending on stance, as do Flare and Freeze.
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.BlackEnochianFeature))
            {
                if (actionID is Fire4 or Blizzard4)
                {
                    if (Gauge.InUmbralIce && level >= 58)
                    {
                        return Blizzard4;
                    }
                    
                    if (level >= 60)
                    {
                        return Fire4;
                    }
                }

                if (actionID is Flare or Freeze)
                {
                    if (Gauge.InAstralFire && level >= 50)
                    {
                        return Flare;
                    }
                    
                    return Freeze;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}