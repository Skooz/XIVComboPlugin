using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class RDM : JobBase
    {
        public override string JobDisplayName => "Red Mage";
        public override uint JobID => JobIDs.RDM;

        public const uint
            Veraero2 = 16525,
            Verthunder2 = 16524,
            Impact = 16526,
            GrandImpact = 37006,
            Redoublement = 7516,
            ERedoublement = 7529,
            Zwerchhau = 7512,
            EZwerchhau = 7528,
            Riposte = 7504,
            ERiposte = 7527,
            Scatter = 7509,
            Verstone = 7511,
            Verfire = 7510,
            Jolt = 7503,
            Jolt2 = 7524,
            Jolt3 = 37004,
            Verholy = 7526,
            Verflare = 7525,
            Scorch = 16530,
            Resolution = 25858;

        public const ushort
            BuffSwiftcast = 167,
            BuffDualcast = 1249,
            BuffAcceleration = 1238,
            BuffChainspell = 2560,
            BuffVerstoneReady = 1235,
            BuffVerfireReady = 1234,
            BuffGrandImpactReady = 3877,
            BuffMagickedSwordplay = 3875;

        private RDMGauge Gauge;

        public RDM(IClientState state, XIVComboConfiguration config, IJobGauges Gauges, IPluginLog log) : base(state,
            config, Gauges, log)
        {
            Gauge = Gauges.Get<RDMGauge>();
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace Veraero/thunder 2 with Impact when Dualcast is active, or Grand Impact when Grand Impact Ready
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.RedMageAoECombo))
            {
                if (actionID == Veraero2)
                {
                    if (HasBuff(BuffGrandImpactReady))
                    {
                        return GrandImpact;
                    }

                    if (HasBuff(BuffSwiftcast) || HasBuff(BuffDualcast) ||
                        HasBuff(BuffAcceleration) || HasBuff(BuffChainspell))
                    {
                        if (level >= 66)
                        {
                            return Impact;
                        }
                        
                        return Scatter;
                    }
                }

                if (actionID == Verthunder2)
                {
                    if (HasBuff(BuffGrandImpactReady))
                    {
                        return GrandImpact;
                    }

                    if (HasBuff(BuffSwiftcast) || HasBuff(BuffDualcast) ||
                        HasBuff(BuffAcceleration) || HasBuff(BuffChainspell))
                    {
                        if (level >= 66)
                        {
                            return Impact;
                        }
                        
                        return Scatter;
                    }
                }
            }

            // Replace Redoublement with Redoublement combo, Enchanted if possible.
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.RedMageMeleeCombo))
                if (actionID == Redoublement)
                {
                    if ((lastMove == Riposte || lastMove == ERiposte) && level >= 35)
                    {
                        if (Gauge.BlackMana >= 15 && Gauge.WhiteMana >= 15 || HasBuff(BuffMagickedSwordplay))
                        {
                            return EZwerchhau;
                        }
                        
                        return Zwerchhau;
                    }

                    if (lastMove == Zwerchhau && level >= 50)
                    {
                        if (Gauge.BlackMana >= 15 && Gauge.WhiteMana >= 15 || HasBuff(BuffMagickedSwordplay))
                        {
                            return ERedoublement;
                        }
                        
                        return Redoublement;
                    }

                    if (Gauge.BlackMana >= 20 && Gauge.WhiteMana >= 20 || HasBuff(BuffMagickedSwordplay))
                    {
                        return ERiposte;
                    }
                    
                    return Riposte;
                }

            // Replace Verstone or Verfire with Jolt if they're NOT ready, or Grand Impact when Grand Impact Ready
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.RedMageVerprocCombo))
            {
                if (actionID == Verstone)
                {
                    if (level >= 80 && (lastMove == Verflare || lastMove == Verholy))
                    {
                        return Scorch;
                    }
                    
                    if (level >= 90 && lastMove == Scorch)
                    {
                        return Resolution;
                    }

                    if (HasBuff(BuffGrandImpactReady))
                    {
                        return GrandImpact;
                    }

                    if (HasBuff(BuffVerstoneReady))
                    {
                        return Verstone;
                    }

                    return level switch
                    {
                        < 62 => Jolt,
                        < 84 => Jolt2,
                        _ => Jolt3
                    };
                }

                if (actionID == Verfire)
                {
                    if (level >= 80 && (lastMove == Verflare || lastMove == Verholy))
                    {
                        return Scorch;
                    }

                    if (level >= 90 && lastMove == Scorch)
                    {
                        return Resolution;
                    }

                    if (HasBuff(BuffGrandImpactReady))
                    {
                        return GrandImpact;
                    }

                    if (HasBuff(BuffVerfireReady))
                    {
                        return Verfire;
                    }

                    return level switch
                    {
                        < 62 => Jolt,
                        < 84 => Jolt2,
                        _ => Jolt3
                    };
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}