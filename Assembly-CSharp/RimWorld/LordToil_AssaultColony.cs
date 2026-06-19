using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class LordToil_AssaultColony : LordToil
{
	private bool attackDownedIfStarving;

	private bool canPickUpOpportunisticWeapons;

	public override bool ForceHighStoryDanger => true;

	public override bool AllowSatisfyLongNeeds => false;

	public LordToil_AssaultColony(bool attackDownedIfStarving = false, bool canPickUpOpportunisticWeapons = false)
	{
		this.attackDownedIfStarving = attackDownedIfStarving;
		this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
	}

	public override void Init()
	{
		base.Init();
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
	}

	public override void UpdateAllDuties()
	{
		for (int i = 0; i < lord.ownedPawns.Count; i++)
		{
			Pawn pawn = lord.ownedPawns[i];
			if (pawn.mindState != null && pawn.mindState.duty?.def != DutyDefOf.AssaultColony)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				pawn.mindState.duty.attackDownedIfStarving = attackDownedIfStarving;
				pawn.mindState.duty.pickupOpportunisticWeapon = canPickUpOpportunisticWeapons;
				pawn.TryGetComp<CompCanBeDormant>()?.WakeUp();
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
		}
	}
}
