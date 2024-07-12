using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class SAM : JobBase
    {
        public override string JobDisplayName => "Samurai";
        public override uint JobID => JobIDs.SAM;

        public const uint
            Yukikaze = 7480,
            Hakaze = 7477,
            Gekko = 7481,
            Jinpu = 7478,
            Kasha = 7482,
            Shifu = 7479,
            Mangetsu = 7484,
            Fuga = 7483,
            Oka = 7485,
            ThirdEye = 7498,
            Iaijutsu = 7867,
            Tsubame = 16483,
            OgiNamikiri = 25781,
            Ikishoten = 16482,
            KaeshiNamikiri = 25782,
            Fuko = 25780,
            Gyuofu = 36963,
            Zanshin = 36964;

        public const ushort
            BuffOgiNamikiriReady = 2959,
            BuffMeikyoShisui = 1233,
            BuffZanshinReady = 3855;

        private SAMGauge Gauge;

        public SAM(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state,
            config, gauges, log)
        {
            Gauge = gauges.Get<SAMGauge>();
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SamuraiTsubameCombo))
            {
                if (actionID == Iaijutsu)
                {
                    ulong x = iconHook.Original(self, Tsubame);
                    return x != Tsubame ? x : iconHook.Original(self, actionID);
                }
            }

            // Replace Yukikaze with Yukikaze combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SamuraiYukikazeCombo))
            {
                if (actionID == Yukikaze)
                {
                    if (HasBuff(BuffMeikyoShisui))
                    {
                        return Yukikaze;
                    }
                    
                    if (comboTime > 0)
                    {
                        if (lastMove == Hakaze && level >= 50 || lastMove == Gyuofu)
                        {
                            return Yukikaze;
                        }
                    }

                    if (level >= 92)
                    {
                        return Gyuofu;
                    }
                    
                    return Hakaze;
                }
            }

            // Replace Gekko with Gekko combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SamuraiGekkoCombo))
            {
                if (actionID == Gekko)
                {
                    if (HasBuff(BuffMeikyoShisui))
                    {
                        return Gekko;
                    }
                    
                    if (comboTime > 0)
                    {
                        if (lastMove == Hakaze && level >= 4 || lastMove == Gyuofu)
                        {
                            return Jinpu;
                        }
                        
                        if (lastMove == Jinpu && level >= 30)
                        {
                            return Gekko;
                        }
                    }

                    if (level >= 92)
                    {
                        return Gyuofu;
                    }
                    
                    return Hakaze;
                }
            }

            // Replace Kasha with Kasha combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SamuraiKashaCombo))
            {
                if (actionID == Kasha)
                {
                    if (HasBuff(BuffMeikyoShisui))
                    {
                        return Kasha;
                    }
                    
                    if (comboTime > 0)
                    {
                        if (lastMove == Hakaze && level >= 18 || lastMove == Gyuofu)
                        {
                            return Shifu;
                        }
                        
                        if (lastMove == Shifu && level >= 40)
                        {
                            return Kasha;
                        }
                    }

                    if (level >= 92)
                    {
                        return Gyuofu;
                    }
                    
                    return Hakaze;
                }
            }

            // Replace Mangetsu with Mangetsu combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SamuraiMangetsuCombo))
            {
                if (actionID == Mangetsu)
                {
                    if (HasBuff(BuffMeikyoShisui))
                    {
                        return Mangetsu;
                    }

                    if (comboTime > 0)
                    {
                        if ((lastMove == Fuga || lastMove == Fuko) && level >= 35)
                        {
                            return Mangetsu;
                        }
                    }

                    if (level >= 86)
                    {
                        return Fuko;
                    }

                    return Fuga;
                }
            }

            // Replace Oka with Oka combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SamuraiOkaCombo))
            {
                if (actionID == Oka)
                {
                    if (HasBuff(BuffMeikyoShisui))
                    {
                        return Oka;
                    }

                    if (comboTime > 0)
                    {
                        if ((lastMove == Fuga || lastMove == Fuko) && level >= 45)
                        {
                            return Oka;
                        }
                    }

                    if (level >= 86)
                    {
                        return Fuko;
                    }

                    return Fuga;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.SamuraiOgiCombo))
            {
                if (actionID == Ikishoten)
                {
                    if (HasBuff(BuffOgiNamikiriReady))
                    {
                        return OgiNamikiri;
                    }

                    if (Gauge.Kaeshi == Kaeshi.NAMIKIRI)
                    {
                        return KaeshiNamikiri;
                    }

                    return HasBuff(BuffZanshinReady) ? Zanshin : Ikishoten;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}