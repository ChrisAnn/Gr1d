using Gr1d.Api.Agent;
using Gr1d.Api.Skill;
using System;
using Gr1d.Api.Deck;

namespace Gr1d.CSharp
{
	public class Sergeant : Corporal, IEngineer4
	{
		public override void OnArrivedActions(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
		{
			if (arriver.Owner != agentUpdate.Owner)
				base.OnArrivedActions(arriver, agentUpdate);
			else
			{
				try
				{
					Engineer.Mentor(agentUpdate.Node.MyAgents, this);
				}
				catch (Exception ex)
				{
					Deck.Trace(string.Format("Exception in {0}. Message: {1}. StackTrace {2})", this.ToString(), ex.Message, ex.StackTrace), TraceType.Error);
				}
			}
		}
	}
}
