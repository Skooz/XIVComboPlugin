using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class PLD : JobBase
    {
        public override string JobDisplayName => "Paladin";
        public override uint JobID => JobIDs.PLD;

        public const uint
            GoringBlade = 3538,
            FastBlade = 9,
            RiotBlade = 15,
            RoyalAuthority = 3539,
            RageOfHalone = 21,
            Prominence = 16457,
            TotalEclipse = 7381,
            Requiescat = 7383,
            Confiteor = 16459,
            BladeOfFaith = 25748,
            BladeOfTruth = 25749,
            BladeOfValor = 25750,
            Imperator = 36921,
            BladeOfHonor = 36922;

        public const ushort
            BuffRequiescat = 1368,
            BuffBladeOfFaithReady = 3019,
            BuffBladeOfHonor = 3831;

        public PLD(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace Royal Authority with Royal Authority combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.PaladinRoyalAuthorityCombo))
            {
                if (actionID is RoyalAuthority or RageOfHalone)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == FastBlade && level >= 4)
                        {
                            return RiotBlade;
                        }
                        
                        if (lastMove == RiotBlade)
                        {
                            if (level >= 60)
                            {
                                return RoyalAuthority;
                            }
                            
                            if (level >= 26)
                            {
                                return RageOfHalone;
                            }
                        }
                    }

                    return FastBlade;
                }
            }

            // Replace Prominence with Prominence combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.PaladinProminenceCombo))
            {
                if (actionID == Prominence)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == TotalEclipse && level >= 40)
                        {
                            return Prominence;
                        }
                    }

                    return TotalEclipse;
                }
            }

            // Replace Requiescat with Confiteor when under the effect of Requiescat
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.PaladinRequiescatCombo))
            {
                if (actionID is Requiescat or Imperator)
                {
                    if (HasBuff(BuffRequiescat) && level >= 80)
                    {
                        return iconHook.Original(self, Confiteor);
                    }

                    if (HasBuff(BuffBladeOfHonor))
                    {
                        return BladeOfHonor;
                    }

                    return iconHook.Original(self, actionID);
                }
            }
            
            return iconHook.Original(self, actionID);
        }
    }
}