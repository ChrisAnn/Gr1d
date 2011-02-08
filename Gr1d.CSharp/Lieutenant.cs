using System.Linq;
using Gr1d.Api.Agent;

namespace Gr1d.CSharp
{
	public class Lieutenant : Sergeant, IEngineer5
	{
		public override void OnArrivedActions(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
		{
			if (arriver.Owner != agentUpdate.Owner)
				if ((arriver.EffectFlags & AgentEffect.Decompile) != AgentEffect.Decompile)
					Engineer.Decompile(arriver, this);

			base.OnArrivedActions(arriver, agentUpdate);
		}

		public override void OnAttackedActions(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
		{
			if ((attacker.EffectFlags & AgentEffect.Decompile) != AgentEffect.Decompile)
				Engineer.Decompile(attacker, this);

			base.OnAttackedActions(attacker, agentUpdate);
		}
	}
}
