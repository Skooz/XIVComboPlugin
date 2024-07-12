using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class DRG : JobBase
    {
        public override string JobDisplayName => "Dragoon";
        public override uint JobID => JobIDs.DRG;

        public const uint CTorment = 16477,
            DoomSpike = 86,
            SonicThrust = 7397,
            ChaosThrust = 88,
            RaidenThrust = 16479,
            HeavensThrust = 25771,
            ChaoticSpring = 25772,
            DraconianFury = 25770,
            TrueThrust = 75,
            Disembowel = 87,
            FangAndClaw = 3554,
            WheelingThrust = 3556,
            FullThrust = 84,
            VorpalThrust = 78,
            LanceBarrage = 36954,
            SpiralBlow = 36955,
            Drakesbane = 36952;

        public const ushort BuffDraconianFire = 1863;

        public DRG(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }

        public override ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID)
        {
            // Replace Coerthan Torment with Coerthan Torment combo chain
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DragoonCoerthanTormentCombo))
            {
                if (actionID == CTorment)
                {
                    if (comboTime > 0)
                    {
                        if ((lastMove == DoomSpike || lastMove == DraconianFury) && level >= 62)
                        {
                            return SonicThrust;
                        }

                        if (lastMove == SonicThrust && level >= 72)
                        {
                            return CTorment;
                        }
                    }

                    return iconHook.Original(self, DoomSpike);
                }
            }

            // Replace Chaos Thrust with the Chaos Thrust combo chain
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DragoonChaosThrustCombo))
            {
                if (actionID is ChaosThrust or ChaoticSpring)
                {
                    if (comboTime > 0)
                    {
                        if ((lastMove == TrueThrust || lastMove == RaidenThrust) && level >= 18)
                        {
                            if (level >= 96)
                            {
                                return SpiralBlow;
                            }

                            return Disembowel;
                        }

                        if (lastMove == Disembowel || lastMove == SpiralBlow)
                        {
                            if (level >= 86)
                            {
                                return ChaoticSpring;
                            }

                            if (level >= 50)
                            {
                                return ChaosThrust;
                            }
                        }

                        if ((lastMove == ChaosThrust || lastMove == ChaoticSpring) && level >= 58)
                        {
                            return WheelingThrust;
                        }

                        if ((lastMove == WheelingThrust) && level >= 64)
                        {
                            return Drakesbane;
                        }
                    }

                    if (HasBuff(BuffDraconianFire) && level >= 76)
                        return RaidenThrust;

                    return TrueThrust;
                }
            }

            // Replace Full Thrust with the Full Thrust combo chain
            if (Configuration.ComboPresets.HasFlag(CustomComboPreset.DragoonFullThrustCombo))
            {
                if (actionID is FullThrust or HeavensThrust)
                {
                    if (comboTime > 0)
                    {
                        if ((lastMove == TrueThrust || lastMove == RaidenThrust) && level >= 4)
                        {
                            if (level >= 96)
                            {
                                return LanceBarrage;
                            }

                            return VorpalThrust;
                        }

                        if (lastMove == VorpalThrust || lastMove == LanceBarrage)
                        {
                            if (level >= 86)
                            {
                                return HeavensThrust;
                            }

                            if (level >= 26)
                            {
                                return FullThrust;
                            }
                        }

                        if ((lastMove == FullThrust || lastMove == HeavensThrust) && level >= 56)
                        {
                            return FangAndClaw;
                        }

                        if ((lastMove == FangAndClaw) && level >= 64)
                        {
                            return Drakesbane;
                        }
                    }

                    if (HasBuff(BuffDraconianFire) && level >= 76)
                    {
                        return RaidenThrust;
                    }

                    return TrueThrust;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}
