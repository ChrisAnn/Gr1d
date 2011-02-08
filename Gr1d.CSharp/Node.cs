using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Node;
using System.Collections.Generic;
using System.Text;
using Gr1d.Api.Deck;
using System;

namespace Gr1d.CSharp
{
	public class Node
	{
		public static INodeInformation ClosestNode(IAgentUpdateInfo agentUpdate)
		{
			var cleanNodes = CleanNodes(agentUpdate);
			return cleanNodes
				.Where(n => n.Distance(agentUpdate.Node) == cleanNodes.Min(e => e.Distance(agentUpdate.Node)))
				.FirstOrDefault();
		}

		public static INodeInformation AnyClaimable(IAgentUpdateInfo agentUpdate)
		{
			return CleanNodes(agentUpdate).Where(e => e.IsClaimable).FirstOrDefault();
		}

		public static INodeInformation AnyUnoccupied(IAgentUpdateInfo agentUpdate)
		{
			return agentUpdate.Node.Exits.Values
				.Where(e => e.OtherAgents.Count() <= 0 && e.Effects.Contains(NodeEffect.None))
				.FirstOrDefault();
		}

		public static INodeInformation AnyWithEnemies(IAgentUpdateInfo agentUpdate)
		{
			return CleanNodes(agentUpdate).Where(e => e.OpposingAgents.Count() > 0).FirstOrDefault();
		}

		public static INodeInformation GetNextNode(IAgentUpdateInfo agentUpdate, IDeck deck)
		{
			INodeInformation targetNode;

			// If we are in the Arena look for enemies
			if (IsInArena(agentUpdate) || agentUpdate.Action == AgentAction.Raiding)
			{
				LogExits(agentUpdate, deck);
				targetNode = Node.AnyWithEnemies(agentUpdate);
			}
			else // Look for nodes that can be claimed
				targetNode = Node.AnyClaimable(agentUpdate);

			// else look for an unoccupied Node
			if (null == targetNode)
				targetNode = Node.AnyUnoccupied(agentUpdate);

			// If all else fails, just Move to the closest Node!
			if (null == targetNode)
				targetNode = Node.ClosestNode(agentUpdate);

			return targetNode;
		}

		public static bool IsInArena(IAgentUpdateInfo agentUpdate)
		{
			return agentUpdate.Node.Sector.Name.Contains("Arena");
		}

		public static bool IsCleanNode(IAgentUpdateInfo agentUpdate)
		{
			return agentUpdate.Node.Effects.Count() <= 0 || agentUpdate.Node.Effects.Contains(NodeEffect.None);
		}

		private static IEnumerable<INodeInformation> CleanNodes(IAgentUpdateInfo agentUpdate)
		{
			var cleanNodes = agentUpdate.Node.Exits.Values.Where(e => e.Effects.Count() <= 0 || e.Effects.Contains(NodeEffect.None));
			if (null == cleanNodes)
				cleanNodes = new List<INodeInformation>();

			return cleanNodes;
		}

		private static void LogExits(IAgentUpdateInfo agentUpdate, IDeck deck)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Connected Nodes: ");
			foreach (INodeInformation node in agentUpdate.Node.Exits.Values)
			{
				sb.AppendLine(string.Format("Location: {0} (Distance: {4}), Number of Agents: {1} (of which {2} are Opposing), Effects: {3}"
					, node.Sector.Name + ", " + node.Layer + "," + node.Row + "," + node.Column
					, node.AllAgents.Count()
					, node.OpposingAgents.Count()
					, String.Join(", ",node.Effects.Select<NodeEffect, string>(e => e.ToString()).ToArray())
					, agentUpdate.Node.Distance(node)));
			}

			deck.Trace(sb.ToString(), TraceType.Information);
		}
	}
}
