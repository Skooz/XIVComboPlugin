using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class GNB : JobBase
    {
        public override string JobDisplayName => "Gunbreaker";
        public override uint JobID => JobIDs.GNB;

        public const uint
            SolidBarrel = 16145,
            KeenEdge = 16137,
            BrutalShell = 16139,
            WickedTalon = 16150,
            GnashingFang = 16146,
            SavageClaw = 16147,
            DemonSlaughter = 16149,
            DemonSlice = 16141,
            Continuation = 16155,
            JugularRip = 16156,
            AbdomenTear = 16157,
            EyeGouge = 16158,
            BurstStrike = 16162,
            Hypervelocity = 25759,
            FatedCircle = 16163,
            FatedBrand = 36936;

        public const ushort
            BuffReadyToRip = 1842,
            BuffReadyToTear = 1843,
            BuffReadyToGouge = 1844,
            BuffReadyToBlast = 2686,
            BuffReadyToRaze = 3839;

        public const byte
            LevelContinuation = 70,
            LevelEnhancedContinuation = 86,
            LevelEnhancedContinuation2 = 96;

        public GNB(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace Solid Barrel with Solid Barrel combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.GunbreakerSolidBarrelCombo))
            {
                if (actionID == SolidBarrel)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == KeenEdge && level >= 4)
                        {
                            return BrutalShell;
                        }
                        
                        if (lastMove == BrutalShell && level >= 26)
                        {
                            return SolidBarrel;
                        }
                    }

                    return KeenEdge;
                }
            }

            // Replace Wicked Talon with Gnashing Fang combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.GunbreakerGnashingFangCont))
            {
                if (actionID == GnashingFang)
                {
                    if (level >= LevelContinuation)
                    {
                        if (HasBuff(BuffReadyToRip))
                        {
                            return JugularRip;
                        }
                        
                        if (HasBuff(BuffReadyToTear))
                        {
                            return AbdomenTear;
                        }
                        
                        if (HasBuff(BuffReadyToGouge))
                        {
                            return EyeGouge;
                        }
                    }

                    return iconHook.Original(self, GnashingFang);
                }
            }

            // Replace Burst Strike with Continuation
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.GunbreakerBurstStrikeCont))
                if (actionID == BurstStrike)
                {
                    if (level >= LevelEnhancedContinuation)
                    {
                        if (HasBuff(BuffReadyToBlast))
                        {
                            return Hypervelocity;
                        }
                    }

                    return BurstStrike;
                }

            // Replace Demon Slaughter with Demon Slaughter combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.GunbreakerDemonSlaughterCombo))
            {
                if (actionID == DemonSlaughter)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == DemonSlice && level >= 40)
                        {
                            return DemonSlaughter;
                        }
                    }
                    
                    return DemonSlice;
                }
            }

            // Replace Fated Brand with Continuation
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.GunbreakerFatedCircleCont))
            {
                if (actionID == FatedCircle)
                {
                    if (level >= LevelEnhancedContinuation2)
                    {
                        if (HasBuff(BuffReadyToRaze))
                        {
                            return FatedBrand;
                        }
                    }

                    return FatedCircle;
                }
            }
            
            return iconHook.Original(self, actionID);
        }
    }
}