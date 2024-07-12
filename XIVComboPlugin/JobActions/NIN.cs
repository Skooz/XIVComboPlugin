using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class NIN : JobBase
    {
        public override string JobDisplayName => "Ninja";
        public override uint JobID => JobIDs.NIN;

        public const uint
            ArmorCrush = 3563,
            SpinningEdge = 2240,
            GustSlash = 2242,
            AeolianEdge = 2255,
            HakkeM = 16488,
            DeathBlossom = 2254,
            DWAD = 3566,
            Assassinate = 2246,
            Bunshin = 16493,
            PhantomK = 25774;

        public const ushort
            BuffPhantomKReady = 2723;

        public NIN(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace Armor Crush with Armor Crush combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.NinjaArmorCrushCombo))
            {
                if (actionID == ArmorCrush)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == SpinningEdge && level >= 4)
                        {
                            return GustSlash;
                        }
                        
                        if (lastMove == GustSlash && level >= 54)
                        {
                            return ArmorCrush;
                        }
                    }

                    return SpinningEdge;
                }
            }

            // Replace Aeolian Edge with Aeolian Edge combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.NinjaAeolianEdgeCombo))
            {
                if (actionID == AeolianEdge)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == SpinningEdge && level >= 4)
                        {
                            return GustSlash;
                        }
                        
                        if (lastMove == GustSlash && level >= 26)
                        {
                            return AeolianEdge;
                        }
                    }

                    return SpinningEdge;
                }
            }

            // Replace Hakke Mujinsatsu with Hakke Mujinsatsu combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.NinjaHakkeMujinsatsuCombo))
            {
                if (actionID == HakkeM)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == DeathBlossom && level >= 52)
                        {
                            return HakkeM;
                        }
                    }

                    return DeathBlossom;
                }
            }
            
            return iconHook.Original(self, actionID);
        }
    }
}