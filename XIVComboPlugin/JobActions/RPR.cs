using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class RPR : JobBase
    {
        public override string JobDisplayName => "Reaper";
        public override uint JobID => JobIDs.RPR;

        public const uint
            // Single Target
            Slice = 24373,
            WaxingSlice = 24374,
            InfernalSlice = 24375,
            // AoE
            SpinningScythe = 24376,
            NightmareScythe = 24377,
            // Shroud
            Enshroud = 24394,
            Communio = 24398,
            Perfectio = 36973,
            Egress = 24402,
            Ingress = 24401,
            Regress = 24403,
            ArcaneCircle = 24405,
            PlentifulHarvest = 24385;

        public static class Buffs
        {
            public const ushort
                Enshrouded = 2593,
                PerfectioParata = 3860,
                Threshold = 2595,
                ImSac1 = 2592,
                ImSac2 = 3204;
        }

        public static class Levels
        {
            public const byte
                Slice = 1,
                WaxingSlice = 5,
                SpinningScythe = 25,
                InfernalSlice = 30,
                NightmareScythe = 45,
                Enshroud = 80,
                Communio = 90;
        }

        public RPR(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ReaperSliceCombo))
            {
                if (actionID == Slice)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == Slice && level >= Levels.WaxingSlice)
                        {
                            return WaxingSlice;
                        }

                        if (lastMove == WaxingSlice && level >= Levels.InfernalSlice)
                        {
                            return InfernalSlice;
                        }
                    }

                    return Slice;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ReaperScytheCombo))
            {
                if (actionID == SpinningScythe)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == SpinningScythe && level >= Levels.NightmareScythe)
                        {
                            return NightmareScythe;
                        }
                    }

                    return SpinningScythe;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ReaperRegressFeature))
            {
                if (actionID is Egress or Ingress)
                {
                    if (HasBuff(Buffs.Threshold)) return Regress;
                    {
                        return actionID;
                    }
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ReaperEnshroudCombo))
            {
                if (actionID == Enshroud)
                {
                    if (HasBuff(Buffs.Enshrouded))
                    {
                        return Communio;
                    }
                    
                    if (HasBuff(Buffs.PerfectioParata))
                    {
                        return Perfectio;
                    }
                    
                    return actionID;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.ReaperArcaneFeature))
            {
                if (actionID == ArcaneCircle)
                {
                    if (HasBuff(Buffs.ImSac1) ||
                        HasBuff(Buffs.ImSac2))
                    {
                        return PlentifulHarvest;
                    }
                    
                    return actionID;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}