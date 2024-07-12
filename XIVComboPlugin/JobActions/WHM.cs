using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class WHM : JobBase
    {
        public override string JobDisplayName => "White Mage";
        public override uint JobID => JobIDs.WHM;

        public const uint
            Cure = 120,
            Cure2 = 135,
            Solace = 16531,
            Rapture = 16534,
            Misery = 16535;

        private WHMGauge Gauge;

        public WHM(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
            Gauge = gauges.Get<WHMGauge>();
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            if (clientState.LocalPlayer == null) return iconHook.Original(self, actionID);

            // Replace Solace with Misery when full blood lily
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.WhiteMageSolaceMiseryFeature))
            {
                if (actionID == Solace)
                {
                    if (Gauge.BloodLily == 3)
                    {
                        return Misery;
                    }

                    return Solace;
                }
            }

            // Replace Solace with Misery when full blood lily
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.WhiteMageRaptureMiseryFeature))
            {
                if (actionID == Rapture)
                {
                    if (Gauge.BloodLily == 3)
                    {
                        return Misery;
                    }

                    return Rapture;
                }
            }
            
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.WhiteMageCureSync))
            {
                if (actionID == Cure2)
                {
                    if (level < 30)
                    {
                        return Cure;
                    }
                    
                    return Cure2;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}