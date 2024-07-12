using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class WAR : JobBase
    {
        public override string JobDisplayName => "Warrior";
        public override uint JobID => JobIDs.WAR;

        public const uint
            StormsPath = 42,
            HeavySwing = 31,
            Maim = 37,
            StormsEye = 45,
            MythrilTempest = 16462,
            Overpower = 41,
            InnerRelease = 7389,
            PrimalRend = 25753,
            PrimalRuination = 36925,
            Berserk = 38;

        public const ushort
            BuffPrimalRendReady = 2624,
            BuffWrathful = 3901,
            BuffPrimalRuinationReady = 3834;

        public WAR(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            if (clientState.LocalPlayer == null) return iconHook.Original(self, actionID);

            // Replace Storm's Path with Storm's Path combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.WarriorStormsPathCombo))
            {
                if (actionID == StormsPath)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == HeavySwing && level >= 4)
                        {
                            return Maim;
                        }
                        
                        if (lastMove == Maim && level >= 26)
                        {
                            return StormsPath;
                        }
                    }

                    return HeavySwing;
                }
            }

            // Replace Storm's Eye with Storm's Eye combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.WarriorStormsEyeCombo))
            {
                if (actionID == StormsEye)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == HeavySwing && level >= 4)
                        {
                            return Maim;
                        }
                        
                        if (lastMove == Maim && level >= 50)
                        {
                            return StormsEye;
                        }
                    }

                    return HeavySwing;
                }
            }

            // Replace Mythril Tempest with Mythril Tempest combo
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.WarriorMythrilTempestCombo))
            {
                if (actionID == MythrilTempest)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == Overpower && level >= 40)
                        {
                            return MythrilTempest;
                        }
                    }
                    
                    return Overpower;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.WarriorIRCombo))
            {
                if (actionID is InnerRelease or Berserk)
                {
                    if (!HasBuff(BuffWrathful) && HasBuff(BuffPrimalRendReady))
                    {
                        return PrimalRend;
                    }
                    
                    if (!HasBuff(BuffWrathful) && HasBuff(BuffPrimalRuinationReady))
                    {
                        return PrimalRuination;
                    }
                    
                    return iconHook.Original(self, actionID);
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}