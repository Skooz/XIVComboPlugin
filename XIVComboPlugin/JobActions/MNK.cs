using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.JobActions
{
    public class MNK : JobBase
    {
        public override string JobDisplayName => "Monk";
        public override uint JobID => JobIDs.MNK;
        
        public MNK(IClientState state, XIVComboConfiguration config, IJobGauges gauges, IPluginLog log) : base(state, config, gauges, log)
        {
        }
    }
}
