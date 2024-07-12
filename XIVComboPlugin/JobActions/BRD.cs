using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class BRD : JobBase
    {
        public override string JobDisplayName => "Bard";
        public override uint JobID => JobIDs.BRD;

        public const uint
            WanderersMinuet = 3559,
            PitchPerfect = 7404,
            HeavyShot = 97,
            BurstShot = 16495,
            StraightShot = 98,
            RefulgentArrow = 7409,
            QuickNock = 106,
            Ladonsbite = 25783,
            WideVolley = 36974,
            Shadowbite = 16494;

        public const ushort
            BuffHawksEye = 3861,
            BuffBarrage = 128;

        public BRD(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace HS/BS with SS/RA when procced.
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.BardStraightShotUpgradeFeature))
            {
                if (actionID is HeavyShot or BurstShot)
                {
                    if (HasBuff(BuffHawksEye) || HasBuff(BuffBarrage))
                    {
                        if (level >= 70)
                        {
                            return RefulgentArrow;
                        }
                        
                        return StraightShot;
                    }

                    if (level >= 76)
                    {
                        return BurstShot;
                    }
                    
                    return HeavyShot;
                }
            }

            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.BardAoEUpgradeFeature))
            {
                if (actionID is QuickNock or Ladonsbite)
                {
                    if (HasBuff(BuffHawksEye) || HasBuff(BuffBarrage))
                    {
                        if (level >= 72)
                        {
                            return Shadowbite;
                        }
                        
                        return WideVolley;
                    }

                    if (level >= 82)
                    {
                        return Ladonsbite;
                    }
                    
                    return QuickNock;
                }
            }

            return actionID;
        }
    }
}