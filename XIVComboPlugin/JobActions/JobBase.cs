using System;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace XIVComboPlugin.JobActions;

public abstract class JobBase 
{
    public abstract string JobDisplayName { get; }
    public abstract uint JobID { get; }

    private static bool globalTypesInitialized;
    protected static IClientState clientState;
    protected static IPluginLog PluginLog;
    protected static XIVComboConfiguration Configuration;
    protected static IntPtr lastComboMove;
    protected static IntPtr comboTimer;

    protected int lastMove => Marshal.ReadInt32(lastComboMove);
    protected float comboTime => Marshal.PtrToStructure<float>(comboTimer);
    protected byte level => clientState.LocalPlayer.Level;

    public JobBase(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log)
    {
        if (!globalTypesInitialized)
        {
            globalTypesInitialized = true;

            lastComboMove = IntPtr.Zero;
            comboTimer = IntPtr.Zero;

            clientState = state;
            Configuration = config;
            PluginLog = log;

            if (!clientState.IsLoggedIn)
            {
                clientState.Login += SetupComboData;
            }
            else
            {
                SetupComboData();
            }
        }
    }

    public virtual ulong IconDetour(Hook<IconReplacer.OnGetIconDelegate> iconHook, byte self, uint actionID) => iconHook.Original(self, actionID);

    protected static bool HasBuff(ushort needle)
    {
        if (needle == 0) return false;
        StatusList buffs = clientState.LocalPlayer.StatusList;
        return buffs.Any(buff => buff.StatusId == needle);
    }

    private static unsafe void SetupComboData()
    {
        byte* actionManager = (byte*)ActionManager.Instance();
        comboTimer = (IntPtr)(actionManager + 0x60);
        lastComboMove = comboTimer + 0x4;
    }
}