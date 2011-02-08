using System;
using System.Collections.Generic;
using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Skill;

namespace Gr1d.CSharp
{
	public class Scout : IEngineer1
	{
		protected IDeck Deck;

		#region IAgent Members

		public void Initialise(IDeck deck)
		{
			Deck = deck;
		}

		public void OnArrived(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
		{
			Deck.Trace(":: OnArrived", TraceType.Verbose);
			Deck.Trace(String.Format("Owner:{0} Level:{1}", arriver.Owner, arriver.Level), TraceType.Warning);
			
			try 
			{				
				OnArrivedActions(arriver, agentUpdate);
			}
			catch (Exception e)
			{
				Deck.Trace(string.Format("Exception in OnArrived: {0}", e.Message), TraceType.Error);
			}
		}

		public void OnAttacked(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
		{
			Deck.Trace(":: OnAttacked", TraceType.Verbose);
			Deck.Trace(String.Format("Owner:{0} Level:{1}", attacker.Owner, attacker.Level), TraceType.Warning);
			
			try
			{
				OnAttackedActions(attacker, agentUpdate);
			}
			catch (Exception e)
			{
				Deck.Trace(string.Format("Exception in OnAttacked: {0}", e.Message), TraceType.Error);
			}
		}

		public void Tick(IAgentUpdateInfo agentUpdate)
		{
			Deck.Trace(":: Tick", TraceType.Verbose);
			
			try
			{
				OnTick(agentUpdate);
			}
			catch (Exception e)
			{
				Deck.Trace(string.Format("Exception in Tick: {0}", e.Message), TraceType.Error);
			}
		}

		#endregion

		public virtual void OnArrivedActions(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
		{
			if (arriver.Owner != agentUpdate.Owner)
			{
				this.Attack(arriver);
			}
		}

		public virtual void OnAttackedActions(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
		{
			this.Attack(attacker);
		}
		
		public virtual void OnTick(IAgentUpdateInfo agentUpdate)
		{
			Engineer.UnitTest(agentUpdate, this);

			Deck.Trace(string.Format("Number of Opposing Agents on current Node: {0}", agentUpdate.Node.OpposingAgents.Count()), TraceType.Verbose);

			IAgentInfo attacker = GetWeakestEnemy(agentUpdate);

			if (null != attacker)
			{
				Deck.Trace(string.Format("Attacker Owner: {0}, Level: {1}, Stack: {2}", attacker.Owner, attacker.Level, attacker.Stack), TraceType.Verbose);
				OnAttackedActions(attacker, agentUpdate);
			}

			if (agentUpdate.Node.IsClaimable && this.Claim(agentUpdate.Node).Result == NodeResultType.Success)
				return;
					  
			var nextNode = Node.GetNextNode(agentUpdate, Deck);
			if (null != nextNode)
				this.Move(nextNode);
		}

		private static IAgentInfo GetWeakestEnemy(IAgentUpdateInfo agentUpdate)
		{
			if (agentUpdate.Node.OpposingAgents.Count() <= 0)
				return null;

			int minStack = agentUpdate.Node.OpposingAgents.Min(o => o.Stack);
			return agentUpdate.Node.OpposingAgents.
				Where(a => a.Stack == minStack).FirstOrDefault();
		}
	}
}
