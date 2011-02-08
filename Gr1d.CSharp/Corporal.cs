using System.Collections.Generic;
using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Skill;
using Gr1d.Api.Node;

namespace Gr1d.CSharp
{
	public class Corporal : Private, IEngineer3
	{
		public override void OnTick (IAgentUpdateInfo agentUpdate)
		{
			// We don't want to fill the arena with Struts, we want to be fully offensive in there.
			if (!Node.IsInArena(agentUpdate))
			{
				INodeInformation nodeWithEnemies = Node.AnyWithEnemies(agentUpdate);
	
			    if (null != nodeWithEnemies)
					this.Struts(nodeWithEnemies);
			}
				
			base.OnTick(agentUpdate);
		}
	}
}
