using Dalamud.Game.Command;
using Dalamud.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ImGuiNET;
using Dalamud.Game;
using Dalamud.Utility;
using System.Diagnostics;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;
using Dalamud.Interface.Utility;
using XIVComboPlugin.JobActions;

namespace XIVComboPlugin
{
    class XIVComboPlugin : IDalamudPlugin
    {
        public string Name => "XIV Combo Plugin";

        public XIVComboConfiguration Configuration;

        private IconReplacer iconReplacer;
        private CustomComboPreset[] orderedByClassJob;

        private ICommandManager CommandManager { get; init; }
        private IDalamudPluginInterface PluginInterface { get; init; }
        private ISigScanner TargetModuleScanner { get; init; }
        private IClientState ClientState { get; init; }
        private IChatGui ChatGui { get; init; }
        private IJobGauges JobGauges { get; init; }
        private IGameInteropProvider HookProvider { get; init; }
        private IPluginLog PluginLog { get; init; }

        private static Dictionary<uint, JobBase> jobCollection;

        public XIVComboPlugin(IClientState clientState, ICommandManager commandManager, IDataManager manager, IDalamudPluginInterface pluginInterface, ISigScanner sigScanner, IJobGauges jobGauges, IChatGui chatGui, IGameInteropProvider gameInteropProvider, IPluginLog pluginLog)
        {
            ClientState = clientState;
            CommandManager = commandManager;
            PluginInterface =  pluginInterface;
            TargetModuleScanner = sigScanner;
            JobGauges = jobGauges;
            HookProvider = gameInteropProvider;
            ChatGui = chatGui;
            PluginLog = pluginLog;

            CommandManager.AddHandler("/pcombo", new CommandInfo(OnCommandDebugCombo)
            {
                HelpMessage = "Open a window to edit custom combo settings.",
                ShowInHelp = true
            });

            Configuration = PluginInterface.GetPluginConfig() as XIVComboConfiguration ?? new XIVComboConfiguration();
            if (Configuration.Version < 4)
            {
                Configuration.Version = 4;
            }

            SetupJobs();
            
            iconReplacer = new IconReplacer(TargetModuleScanner, ClientState, HookProvider, PluginLog);
            iconReplacer.Enable();

            PluginInterface.UiBuilder.OpenConfigUi += () => isImguiComboSetupOpen = true;
            PluginInterface.UiBuilder.Draw += UiBuilder_OnBuildUi;

            IEnumerable<CustomComboPreset> values = Enum.GetValues(typeof(CustomComboPreset)).Cast<CustomComboPreset>();
            orderedByClassJob = values.Where(x => x != CustomComboPreset.None && x.GetAttribute<CustomComboInfoAttribute>() != null).OrderBy(x => x.GetAttribute<CustomComboInfoAttribute>().ClassJob).ToArray();
            UpdateConfig();
        }

        private bool isImguiComboSetupOpen = false;

        private void UpdateConfig()
        {

        }

        private void UiBuilder_OnBuildUi()
        {
            if (!isImguiComboSetupOpen)
                return;
            
            bool[] flagsSelected = new bool[orderedByClassJob.Length];
            for (int i = 0; i < orderedByClassJob.Length; i++)
            {
                flagsSelected[i] = Configuration.ComboPresets.HasFlag(orderedByClassJob[i]);
            }

            ImGui.SetNextWindowSize(new Vector2(750, (30 + ImGui.GetStyle().ItemSpacing.Y) * 17));

            ImGui.Begin("Custom Combo Setup", ref isImguiComboSetupOpen, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar);

            ImGui.Text("This window allows you to enable and disable custom combos to your liking.");
            ImGui.Separator();

            ImGui.BeginChild("scrolling", new Vector2(0, -(25 + ImGui.GetStyle().ItemSpacing.Y)) * ImGuiHelpers.GlobalScale, true, ImGuiWindowFlags.HorizontalScrollbar);

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(0, 5));

            uint lastClassJob = 0;

            for (int i = 0; i < orderedByClassJob.Length; i++)
            {
                CustomComboPreset flag = orderedByClassJob[i];
                CustomComboInfoAttribute flagInfo = flag.GetAttribute<CustomComboInfoAttribute>();
                if (lastClassJob != flagInfo.ClassJob)
                {
                    lastClassJob = flagInfo.ClassJob;
                    JobBase job = GetJob(lastClassJob);
                    if (job == null) continue;
                    
                    if (ImGui.CollapsingHeader(job.JobDisplayName))
                    {
                        for (int j = i; j < orderedByClassJob.Length; j++)
                        {
                            flag = orderedByClassJob[j];
                            flagInfo = flag.GetAttribute<CustomComboInfoAttribute>();
                            if (lastClassJob != flagInfo.ClassJob)
                            {
                                break;
                            }
                            
                            ImGui.PushItemWidth(200);
                            ImGui.Checkbox(flagInfo.FancyName, ref flagsSelected[j]);
                            ImGui.PopItemWidth();
                            ImGui.TextColored(new Vector4(0.68f, 0.68f, 0.68f, 1.0f), $"#{j+1}: " + flagInfo.Description);
                            ImGui.Spacing();
                        }
                    }
                }
            }

            if (ImGui.CollapsingHeader("Monk"))
            {
                ImGui.Text("Not happening.");
                if (ImGui.Button("External link for more detailed explanation (for real this time)"))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://github.com/attickdoor/XIVComboPlugin/blob/master/why-no-monk.md",
                        UseShellExecute = true
                    });
                }
            }

            for (int i = 0; i < orderedByClassJob.Length; i++)
            {
                if (flagsSelected[i])
                {
                    Configuration.ComboPresets |= orderedByClassJob[i];
                }
                else
                {
                    Configuration.ComboPresets &= ~orderedByClassJob[i];
                }
            }

            ImGui.PopStyleVar();

            ImGui.EndChild();

            ImGui.Separator();
            if (ImGui.Button("Save"))
            {
                PluginInterface.SavePluginConfig(Configuration);
                UpdateConfig();
            }
            ImGui.SameLine();
            if (ImGui.Button("Save and Close"))
            {
                PluginInterface.SavePluginConfig(Configuration);
                isImguiComboSetupOpen = false;
                UpdateConfig();
            }

            ImGui.End();
        }

        public void Dispose()
        {
            iconReplacer.Dispose();

            CommandManager.RemoveHandler("/pcombo");

            //PluginInterface.Dispose();
        }

        private void OnCommandDebugCombo(string command, string arguments)
        {
            string[] argumentsParts = arguments.Split();

            switch (argumentsParts[0])
            {
                case "setall":
                    {
                        foreach (CustomComboPreset value in Enum.GetValues(typeof(CustomComboPreset)).Cast<CustomComboPreset>())
                        {
                            if (value == CustomComboPreset.None)
                                continue;

                            Configuration.ComboPresets |= value;
                        }

                        ChatGui.Print("all SET");
                    }
                    break;
                case "unsetall":
                    {
                        foreach (CustomComboPreset value in Enum.GetValues(typeof(CustomComboPreset)).Cast<CustomComboPreset>())
                        {
                            Configuration.ComboPresets &= value;
                        }

                        ChatGui.Print("all UNSET");
                    }
                    break;
                case "set":
                    {
                        foreach (CustomComboPreset value in Enum.GetValues(typeof(CustomComboPreset)).Cast<CustomComboPreset>())
                        {
                            if (value.ToString().ToLower() != argumentsParts[1].ToLower())
                                continue;

                            Configuration.ComboPresets |= value;
                        }
                    }
                    break;
                case "toggle":
                    {
                        foreach (CustomComboPreset value in Enum.GetValues(typeof(CustomComboPreset)).Cast<CustomComboPreset>())
                        {
                            if (value.ToString().ToLower() != argumentsParts[1].ToLower())
                                continue;

                            Configuration.ComboPresets ^= value;
                        }
                    }
                    break;

                case "unset":
                    {
                        foreach (CustomComboPreset value in Enum.GetValues(typeof(CustomComboPreset)).Cast<CustomComboPreset>())
                        {
                            if (value.ToString().ToLower() != argumentsParts[1].ToLower())
                                continue;

                            Configuration.ComboPresets &= ~value;
                        }
                    }
                    break;

                case "list":
                    {
                        foreach (CustomComboPreset value in Enum.GetValues(typeof(CustomComboPreset)).Cast<CustomComboPreset>().Where(x => x != CustomComboPreset.None))
                        {
                            if (argumentsParts[1].ToLower() == "set")
                            {
                                if (Configuration.ComboPresets.HasFlag(value))
                                    ChatGui.Print(value.ToString());
                            }
                            else if (argumentsParts[1].ToLower() == "all")
                                ChatGui.Print(value.ToString());
                        }
                    }
                    break;

                default:
                    isImguiComboSetupOpen = true;
                    break;
            }

            PluginInterface.SavePluginConfig(Configuration);
        }

        private void SetupJobs()
        {
            jobCollection = new Dictionary<uint, JobBase>();
            
            IEnumerable<Type> types = typeof(JobBase).Assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(JobBase)));
            foreach (Type type in types)
            {
                if (Activator.CreateInstance(type, [ClientState, Configuration, JobGauges, PluginLog]) is not JobBase instance) continue;
                
                jobCollection.TryAdd(instance.JobID, instance);
            }
        }

        public static JobBase GetJob(uint jobID) => jobCollection.GetValueOrDefault(jobID);
    }
}
