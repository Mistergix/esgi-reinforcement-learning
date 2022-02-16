using UnityEngine;
using PGSauce.Core.PGFiniteStateMachine;

namespace PGSauce.Games.Drop Da Bomb
{
	[CreateAssetMenu(menuName = "PG/Finite State Machine/Decisions/Player/Frozen")]
	public class FrozenDecisionPlayer : Decision<StateControllerPlayer>
	{
		public override bool Decide(StateControllerPlayer controller)
		{
			return true || false;
		}
	}
}
