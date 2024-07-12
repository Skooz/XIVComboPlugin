using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class AST : JobBase
    {
        public override string JobDisplayName => "Astrologian";
        public override uint JobID => JobIDs.AST;

        public const uint
            Benefic = 3594,
            Benefic2 = 3610,
            Play = 17055,
            Draw = 3590,
            Balance = 4401,
            Bole = 4404,
            Arrow = 4402,
            Spear = 4403,
            Ewer = 4405,
            Spire = 4406,
            MinorArcana = 7443,
            CrownPlay = 25869;

        public const ushort
            BuffLordOfCrownsDrawn = 2054,
            BuffLadyOfCrownsDrawn = 2055;

        public AST(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.AstrologianBeneficSync))
            {
                if (actionID == Benefic2)
                {
                    if (level < 26)
                    {
                        return Benefic;
                    }
                    
                    return Benefic2;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}