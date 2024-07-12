using System;

namespace XIVComboPlugin
{
    //TODO: Replace with bitarray before beast master comes out
    [Flags]
    public enum CustomComboPreset : long
    {
        None = 0,

        [CustomComboInfo("Coerthan Torment Combo", "Replace Coerthan Torment with its combo chain", JobIDs.DRG)]
        DragoonCoerthanTormentCombo = 1L << 0,

        [CustomComboInfo("Chaos Thrust Combo", "Replace Chaos Thrust with its combo chain", JobIDs.DRG)]
        DragoonChaosThrustCombo = 1L << 1,

        [CustomComboInfo("Full Thrust Combo", "Replace Full Thrust with its combo chain", JobIDs.DRG)]
        DragoonFullThrustCombo = 1L << 2,

        // DARK KNIGHT
        [CustomComboInfo("Souleater Combo", "Replace Souleater with its combo chain", JobIDs.DRK)]
        DarkSouleaterCombo = 1L << 3,

        [CustomComboInfo("Stalwart Soul Combo", "Replace Stalwart Soul with its combo chain", JobIDs.DRK)]
        DarkStalwartSoulCombo = 1L << 4,

        // PALADIN
        [CustomComboInfo("Royal Authority Combo", "Replace Royal Authority/Rage of Halone with its combo chain",
            JobIDs.PLD)]
        PaladinRoyalAuthorityCombo = 1L << 5,

        [CustomComboInfo("Prominence Combo", "Replace Prominence with its combo chain", JobIDs.PLD)]
        PaladinProminenceCombo = 1L << 6,

        [CustomComboInfo("Requiescat Confiteor",
            "Replace Requiescat with Confiteor while under the effect of Requiescat", JobIDs.PLD)]
        PaladinRequiescatCombo = 1L << 7,

        // WARRIOR
        [CustomComboInfo("Storms Path Combo", "Replace Storms Path with its combo chain", JobIDs.WAR)]
        WarriorStormsPathCombo = 1L << 8,

        [CustomComboInfo("Storms Eye Combo", "Replace Storms Eye with its combo chain", JobIDs.WAR)]
        WarriorStormsEyeCombo = 1L << 9,

        [CustomComboInfo("Mythril Tempest Combo", "Replace Mythril Tempest with its combo chain", JobIDs.WAR)]
        WarriorMythrilTempestCombo = 1L << 10,

        [CustomComboInfo("IR Combo",
            "Replace Inner Release with Primal Rend or Primal Ruination if they are ready and Wrathful status is not active",
            JobIDs.WAR)]
        WarriorIRCombo = 1L << 11,

        // SAMURAI
        [CustomComboInfo("Yukikaze Combo", "Replace Yukikaze with its combo chain", JobIDs.SAM)]
        SamuraiYukikazeCombo = 1L << 12,

        [CustomComboInfo("Gekko Combo", "Replace Gekko with its combo chain", JobIDs.SAM)]
        SamuraiGekkoCombo = 1L << 13,

        [CustomComboInfo("Kasha Combo", "Replace Kasha with its combo chain", JobIDs.SAM)]
        SamuraiKashaCombo = 1L << 14,

        [CustomComboInfo("Mangetsu Combo", "Replace Mangetsu with its combo chain", JobIDs.SAM)]
        SamuraiMangetsuCombo = 1L << 15,

        [CustomComboInfo("Oka Combo", "Replace Oka with its combo chain", JobIDs.SAM)]
        SamuraiOkaCombo = 1L << 16,

        [CustomComboInfo("Iaijutsu into Tsubame", "Replace Iaijutsu with Tsubame after using an Iaijutsu", JobIDs.SAM)]
        SamuraiTsubameCombo = 1L << 17,

        [CustomComboInfo("Ogi Namikiri Combo", "Replace Ikishoten with Ogi Namiki and Kaeshi Namikiri when appropriate",
            JobIDs.SAM)]
        SamuraiOgiCombo = 1L << 18,

        // NINJA
        [CustomComboInfo("Armor Crush Combo", "Replace Armor Crush with its combo chain", JobIDs.NIN)]
        NinjaArmorCrushCombo = 1L << 19,

        [CustomComboInfo("Aeolian Edge Combo", "Replace Aeolian Edge with its combo chain", JobIDs.NIN)]
        NinjaAeolianEdgeCombo = 1L << 20,

        [CustomComboInfo("Hakke Mujinsatsu Combo", "Replace Hakke Mujinsatsu with its combo chain", JobIDs.NIN)]
        NinjaHakkeMujinsatsuCombo = 1L << 21,

        // GUNBREAKER
        [CustomComboInfo("Solid Barrel Combo", "Replace Solid Barrel with its combo chain", JobIDs.GNB)]
        GunbreakerSolidBarrelCombo = 1L << 22,

        [CustomComboInfo("Gnashing Fang Continuation", "Put Continuation moves on Gnashing Fang when appropriate",
            JobIDs.GNB)]
        GunbreakerGnashingFangCont = 1L << 23,

        [CustomComboInfo("Burst Strike Continuation", "Put Continuation moves on Burst Strike when appropriate",
            JobIDs.GNB)]
        GunbreakerBurstStrikeCont = 1L << 24,

        [CustomComboInfo("Demon Slaughter Combo", "Replace Demon Slaughter with its combo chain", JobIDs.GNB)]
        GunbreakerDemonSlaughterCombo = 1L << 25,

        [CustomComboInfo("Fated Circle Continuation", "Put Continuation moves on Fated Circle when appropriate",
            JobIDs.GNB)]
        GunbreakerFatedCircleCont = 1L << 26,

        // MACHINIST
        [CustomComboInfo("(Heated) Shot Combo", "Replace either form of Clean Shot with its combo chain", JobIDs.MCH)]
        MachinistMainCombo = 1L << 27,

        [CustomComboInfo("Spread Shot Heat", "Replace Spread Shot or Scattergun with Auto Crossbow when overheated",
            JobIDs.MCH)]
        MachinistSpreadShotFeature = 1L << 28,

        [CustomComboInfo("Heat Blast when overheated",
            "Replace Hypercharge with Heat Blast/Blazing Shot when overheated", JobIDs.MCH)]
        MachinistOverheatFeature = 1L << 29,

        // BLACK MAGE
        [CustomComboInfo("Enochian Stance Switcher",
            "Change Fire 4 and Blizzard 4 to the appropriate element depending on stance, as well as Flare and Freeze",
            JobIDs.BLM)]
        BlackEnochianFeature = 1L << 30,

        // SUMMONER

        [CustomComboInfo("ED Fester", "Change Fester and Necrotize into Energy Drain when out of Aetherflow stacks",
            JobIDs.SMN)]
        SummonerEDFesterCombo = 1L << 31,

        [CustomComboInfo("ES Painflare", "Change Painflare into Energy Syphon when out of Aetherflow stacks",
            JobIDs.SMN)]
        SummonerESPainflareCombo = 1L << 32,

        // DANCER
        [CustomComboInfo("AoE GCD procs", "DNC AoE procs turn into their normal abilities when not procced",
            JobIDs.DNC)]
        DancerAoeGcdFeature = 1L << 33,

        [CustomComboInfo("Fan Dance Combos", "Change Fan Dance and Fan Dance 2 into Fan Dance 3 while flourishing",
            JobIDs.DNC)]
        DancerFanDanceCombo = 1L << 34,

        [CustomComboInfo("Fan Dance IV", "Change Flourish into Fan Dance IV while flourishing", JobIDs.DNC)]
        DancerFanDance4Combo = 1L << 35,

        [CustomComboInfo("Devilment into Starfall",
            "Change Devilment into Starfall Dance while under the effect of Flourishing Starfall", JobIDs.DNC)]
        DancerDevilmentCombo = 1L << 36,

        // WHITE MAGE
        [CustomComboInfo("Solace into Misery",
            "Replaces Afflatus Solace with Afflatus Misery when Misery is ready to be used", JobIDs.WHM)]
        WhiteMageSolaceMiseryFeature = 1L << 37,

        [CustomComboInfo("Rapture into Misery",
            "Replaces Afflatus Rapture with Afflatus Misery when Misery is ready to be used", JobIDs.WHM)]
        WhiteMageRaptureMiseryFeature = 1L << 38,

        // BARD
        [CustomComboInfo("Heavy Shot into Straight Shot",
            "Replaces Heavy Shot/Burst Shot with Straight Shot/Refulgent Arrow when procced", JobIDs.BRD)]
        BardStraightShotUpgradeFeature = 1L << 39,

        [CustomComboInfo("Quick Nock into Wide Volley", "Replaces Quick Nock/Ladonsbite with Wide Volley/Shadowbite when procced", JobIDs.BRD)]
        BardAoEUpgradeFeature = 1L << 40,

        // RED MAGE
        [CustomComboInfo("Red Mage AoE Combo",
            "Replaces Veraero/thunder 2 with Impact when Dualcast or Swiftcast are active", JobIDs.RDM)]
        RedMageAoECombo = 1L << 41,

        [CustomComboInfo("Redoublement combo",
            "Replaces Redoublement with its combo chain, following enchantment rules", JobIDs.RDM)]
        RedMageMeleeCombo = 1L << 42,

        [CustomComboInfo("Verproc into Jolt", "Replaces Verstone/Verfire with Jolt/Scorch when no proc is available.",
            JobIDs.RDM)]
        RedMageVerprocCombo = 1L << 43,

        // REAPER
        [CustomComboInfo("Slice Combo", "Replace Slice with its combo chain.", JobIDs.RPR)]
        ReaperSliceCombo = 1L << 44,

        [CustomComboInfo("Scythe Combo", "Replace Spinning Scythe with its combo chain.", JobIDs.RPR)]
        ReaperScytheCombo = 1L << 45,

        [CustomComboInfo("Double Regress", "Regress always replaces both Hell's Egress and Hell's Ingress.",
            JobIDs.RPR)]
        ReaperRegressFeature = 1L << 46,

        [CustomComboInfo("Enshroud Combo",
            "Replace Enshroud with Communio while Enshrouded, and Perfectio while Perfectio Parata", JobIDs.RPR)]
        ReaperEnshroudCombo = 1L << 47,

        [CustomComboInfo("Arcane Circle Combo",
            "Replace Arcane Circle with Plentiful Harvest while you have Immortal Sacrifice.", JobIDs.RPR)]
        ReaperArcaneFeature = 1L << 48,

        [CustomComboInfo("Dreadwinder Combo",
            "Swaps Hunter's Coil/Swiftskin's Coil with the relevant twin ending chain", JobIDs.VPR)]
        ViperDreadwinderCombo = 1L << 49,

        [CustomComboInfo("Pit of Dread Combo", "Swaps Hunter's Den/Swiftskin's Den with the relevant twin ending chain",
            JobIDs.VPR)]
        ViperPitOfDreadCombo = 1L << 50,

        [CustomComboInfo("Uncoiled Fury Combo",
            "Swap Hunter's Coil / Swiftskin's Coil with their relevant twin during Uncoiled Fury.", JobIDs.VPR)]
        ViperUncoiledFuryCombo = 1L << 51,

        [CustomComboInfo("Reawaken Cleanup",
            "Cleans up some of the over button usage of Reawaken and rebinds Ouroboros to Serpent's Tail", JobIDs.VPR)]
        ViperReawakenCleanup = 1L << 52,

        // ASTROLOGIAN
        [CustomComboInfo("Benefic II downgrade", "Replaces Benefic II with Benefic when under level 30", 33)]
        AstrologianBeneficSync = 1L << 53,

        // SCHOLAR
        [CustomComboInfo("Adloquium downgrade", "Replaces Adloquium with Physick when under level 30", 28)]
        ScholarAdloquiumSync = 1L << 54,
        
        // WHITE MAGE
        [CustomComboInfo("Cure II downgrade", "Replaces Cure II with Cure when under level 30", 24)]
        WhiteMageCureSync = 1L << 55,
    }

    public class CustomComboInfoAttribute : Attribute
    {
        internal CustomComboInfoAttribute(string fancyName, string description, uint classJob)
        {
            FancyName = fancyName;
            Description = description;
            ClassJob = classJob;
        }

        public string FancyName { get; }
        public string Description { get; }
        public uint ClassJob { get; }
    }
}