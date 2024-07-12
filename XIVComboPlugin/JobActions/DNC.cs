using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class DNC : JobBase
    {
        public override string JobDisplayName => "Dancer";
        public override uint JobID => JobIDs.DNC;

        public const uint
            Bladeshower = 15994,
            Bloodshower = 15996,
            Windmill = 15993,
            RisingWindmill = 15995,
            FanDance1 = 16007,
            FanDance2 = 16008,
            FanDance3 = 16009,
            FanDance4 = 25791,
            Flourish = 16013,
            Devilment = 16011,
            StarfallDance = 25792;

        public const ushort
            BuffFlourishingSymmetry = 3017,
            BuffFlourishingFlow = 3018,
            BuffThreefoldFanDance = 1820,
            BuffFourfoldFanDance = 2699,
            BuffStarfallDanceReady = 2700,
            BuffSilkenSymmetry = 2693,
            BuffSilkenFlow = 2694;

        public DNC(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // AoE GCDs are split into two buttons, because priority matters
            // differently in different single-target moments. Thanks yoship.
            // Replaces each GCD with its procced version.
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DancerAoeGcdFeature))
            {
                if (actionID == Bloodshower)
                {
                    if (HasBuff(BuffFlourishingFlow) || HasBuff(BuffSilkenFlow))
                    {
                        return Bloodshower;
                    }

                    return Bladeshower;
                }

                if (actionID == RisingWindmill)
                {
                    if (HasBuff(BuffFlourishingSymmetry) || HasBuff(BuffSilkenSymmetry))
                    {
                        return RisingWindmill;
                    }

                    return Windmill;
                }
            }

            // Fan Dance changes into Fan Dance 3 while flourishing.
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DancerFanDanceCombo))
            {
                if (actionID == FanDance1)
                {
                    if (HasBuff(BuffThreefoldFanDance))
                    {
                        return FanDance3;
                    }

                    return FanDance1;
                }

                // Fan Dance 2 changes into Fan Dance 3 while flourishing.
                if (actionID == FanDance2)
                {
                    if (HasBuff(BuffThreefoldFanDance))
                    {
                        return FanDance3;
                    }

                    return FanDance2;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DancerFanDance4Combo))
            {
                if (actionID == Flourish)
                {
                    if (HasBuff(BuffFourfoldFanDance))
                    {
                        return FanDance4;
                    }

                    return Flourish;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DancerDevilmentCombo))
            {
                if (actionID == Devilment)
                {
                    if (HasBuff(BuffStarfallDanceReady))
                    {
                        return StarfallDance;
                    }

                    return Devilment;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}