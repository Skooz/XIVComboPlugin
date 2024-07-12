using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions;

public class VPR : JobBase
{
    public override string JobDisplayName => "Viper";
    public override uint JobID => JobIDs.VPR;

    public const ushort
        SteelMaw = 34614,
        DreadMaw = 34615,
        HuntersCoil = 34621,
        SwiftskinsCoil = 34622,
        HuntersDen = 34624,
        SwiftskinsDen = 34625,
        Reawaken = 34626,
        Ouroboros = 34631,
        TwinfangBite = 34636,
        TwinbloodBite = 34637,
        TwinfangThresh = 34638,
        TwinbloodThresh = 34639,
        UncoiledTwinfang = 34644,
        UncoiledTwinblood = 34645,
        SerpentsTail = 35920;

    public const ushort
        BuffHuntersVenom = 3657,
        BuffSwiftskinsVenom = 3658,
        BuffFellhuntersVenom = 3659,
        BuffFellskinsVenom = 3660,
        BuffPoisedForTwinfang = 3665,
        BuffPoisedForTwinblood = 3666;

    private VPRGauge Gauge;

    public VPR(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state,
        config, gauges, log)
    {
        Gauge = gauges.Get<VPRGauge>();
    }

    public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
    {
        if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ViperDreadwinderCombo))
        {
            if (actionID is HuntersCoil)
            {
                if (HasBuff(BuffHuntersVenom))
                {
                    return TwinfangBite;
                }
            }

            if (actionID is SwiftskinsCoil)
            {
                if (HasBuff(BuffSwiftskinsVenom))
                {
                    return TwinbloodBite;
                }
            }
        }

        if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ViperPitOfDreadCombo))
        {
            if (actionID is HuntersDen && level >= 80)
            {
                if (HasBuff(BuffFellhuntersVenom))
                {
                    return TwinfangThresh;
                }
            }

            if (actionID is SwiftskinsDen && level >= 80)
            {
                if (HasBuff(BuffFellskinsVenom))
                {
                    return TwinbloodThresh;
                }
            }
        }
        
        if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ViperUncoiledFuryCombo))
        {
            if (level >= 92)
            {
                if (actionID is HuntersCoil && HasBuff(BuffPoisedForTwinfang))
                {
                    return UncoiledTwinfang;
                }

                if (actionID is SwiftskinsCoil && HasBuff(BuffPoisedForTwinblood))
                {
                    return UncoiledTwinblood;
                }
            }
        }

        if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ViperReawakenCleanup))
        {
            if (actionID is Reawaken)
            {
                return Reawaken;
            }

            if (actionID is SteelMaw or DreadMaw or HuntersDen or SwiftskinsDen)
            {
                return actionID;
            }

            if (Gauge.AnguineTribute == 1 && actionID is SerpentsTail)
            {
                ulong current = iconHook.Original(self, actionID);
                return current == SerpentsTail ? Ouroboros : current;
            }
        }

        return iconHook.Original(self, actionID);
    }
}