using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class MCH : JobBase
    {
        public override string JobDisplayName => "Machinist";
        public override uint JobID => JobIDs.MCH;

        public const uint
            CleanShot = 2873,
            HeatedCleanShot = 7413,
            SplitShot = 2866,
            HeatedSplitShot = 7411,
            SlugShot = 2868,
            HeatedSlugshot = 7412,
            Hypercharge = 17209,
            HeatBlast = 7410,
            BlazingShot = 36978,
            SpreadShot = 2870,
            AutoCrossbow = 16497,
            Scattergun = 25786;

        private MCHGauge Gauge;

        public MCH(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
            Gauge = gauges.Get<MCHGauge>();
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace Clean Shot with Heated Clean Shot combo
            // Or with Heat Blast when overheated.
            // For some reason the shots use their unheated IDs as combo moves
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.MachinistMainCombo))
            {
                if (actionID is CleanShot or HeatedCleanShot)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == SplitShot)
                        {
                            if (level >= 60)
                            {
                                return HeatedSlugshot;
                            }
                            
                            if (level >= 2)
                            {
                                return SlugShot;
                            }
                        }

                        if (lastMove == SlugShot)
                        {
                            if (level >= 64)
                            {
                                return HeatedCleanShot;
                            }
                            
                            if (level >= 26)
                            {
                                return CleanShot;
                            }
                        }
                    }

                    if (level >= 54)
                    {
                        return HeatedSplitShot;
                    }
                    
                    return SplitShot;
                }
            }
            
            // Replace Hypercharge with Heat Blast when overheated
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.MachinistOverheatFeature))
            {
                if (actionID == Hypercharge)
                {
                    if (Gauge.IsOverheated && level >= 35)
                    {
                        if (level >= 68)
                        {
                            return BlazingShot;
                        }
                        
                        return HeatBlast;
                    }

                    return Hypercharge;
                }
            }

            // Replace Spread Shot with Auto Crossbow when overheated.
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.MachinistSpreadShotFeature))
            {
                if (actionID is SpreadShot or Scattergun)
                {
                    if (Gauge.IsOverheated && level >= 52)
                    {
                        return AutoCrossbow;
                    }
                    
                    if (level >= 82)
                    {
                        return Scattergun;
                    }
                    
                    return SpreadShot;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}