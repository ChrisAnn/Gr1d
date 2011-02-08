using System.Collections.Generic;
using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Node;
using Gr1d.Api.Skill;

namespace Gr1d.CSharp
{
	public class Engineer
	{
		public static void UnitTest(IAgentUpdateInfo agentUpdate, IEngineer1 engineer)
		{
			if ((agentUpdate.EffectFlags & AgentEffect.UnitTest) != AgentEffect.UnitTest)
				engineer.UnitTest();
		}

		public static void Pin(IAgentInfo attacker, IEngineer2 engineer)
		{
			if ((attacker.EffectFlags & AgentEffect.Pin) != AgentEffect.Pin)
				engineer.Pin(attacker);
		}

		public static void Struts(INodeInformation node, IEngineer3 engineer)
		{
			if (!node.Effects.Contains(NodeEffect.Struts))
				engineer.Struts(node);
		}

		public static void Mentor(IEnumerable<IAgentInfo> targets, IEngineer4 engineer)
		{
			// Mentor seems broken!
			//var targetsToMentor = targets.Where(t => ((t.EffectFlags & AgentEffect.Mentor) != AgentEffect.Mentor));

			//if (null != targetsToMentor && targetsToMentor.Count() > 0)
			//    engineer.Mentor(targetsToMentor);
		}

		public static void Decompile(IAgentInfo target, IEngineer5 engineer)
		{
			if ((target.EffectFlags & AgentEffect.Decompile) != AgentEffect.Decompile)
				engineer.Decompile(target);
		}

		public static void Scaffold(INodeInformation node, IEngineer6 engineer)
		{
			if (!node.Effects.Contains(NodeEffect.Scaffold))
				engineer.Scaffold(node);
		}
	}
}
