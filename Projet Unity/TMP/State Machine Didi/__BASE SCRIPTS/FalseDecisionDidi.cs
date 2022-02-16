using UnityEngine;
using PGSauce.Core.PGFiniteStateMachine;

namespace PGSauce.Games.DropDaBomb
{
	[CreateAssetMenu(menuName = "PG/Finite State Machine/Decisions/Didi/False")]
	public class FalseDecisionDidi : Decision<StateControllerDidi>
	{
		public override bool Decide(StateControllerDidi controller)
		{
			return false;
		}
	}
}
