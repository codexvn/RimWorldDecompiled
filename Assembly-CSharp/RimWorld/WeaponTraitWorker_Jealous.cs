using System;

namespace RimWorld
{
	// Token: 0x02001B2E RID: 6958
	public class WeaponTraitWorker_Jealous : WeaponTraitWorker
	{
		// Token: 0x06009B62 RID: 39778 RVA: 0x002D1738 File Offset: 0x002CF938
		public override void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
			if (weapon.CodedPawn == null || weapon.CodedPawn.Dead || weapon.CodedPawn.needs.mood == null)
			{
				return;
			}
			Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(ThoughtDefOf.JealousRage);
			thought_WeaponTrait.weapon = weapon.parent;
			weapon.CodedPawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
		}
	}
}
