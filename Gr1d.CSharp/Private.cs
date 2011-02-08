using System.Linq;
using Gr1d.Api.Agent;

namespace Gr1d.CSharp
{
	public class Private : Scout, IEngineer2
	{
		public override void OnArrivedActions(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
		{
			if (arriver.Owner != agentUpdate.Owner)
				if ((arriver.EffectFlags & AgentEffect.Pin) != AgentEffect.Pin)
					Engineer.Pin(arriver, this);

			base.OnArrivedActions(arriver, agentUpdate);
		}

		public override void OnAttackedActions(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
		{
			if ((attacker.EffectFlags & AgentEffect.Pin) != AgentEffect.Pin)
				Engineer.Pin(attacker, this);

			base.OnAttackedActions(attacker, agentUpdate);
		}
	}
}
