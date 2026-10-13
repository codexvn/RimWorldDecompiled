using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2C RID: 6956
	public class WeaponTraitWorker
	{
		// Token: 0x06009B59 RID: 39769 RVA: 0x002D14E4 File Offset: 0x002CF6E4
		public virtual void Notify_Bonded(Pawn pawn)
		{
			if (!this.def.bondedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.bondedHediffs.Count; i++)
				{
					pawn.health.AddHediff(this.def.bondedHediffs[i], pawn.health.hediffSet.GetBrain(), null, null);
				}
			}
		}

		// Token: 0x06009B5A RID: 39770 RVA: 0x002D1558 File Offset: 0x002CF758
		public virtual void Notify_Equipped(Pawn pawn)
		{
			if (!this.def.equippedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.equippedHediffs.Count; i++)
				{
					pawn.health.AddHediff(this.def.equippedHediffs[i], pawn.health.hediffSet.GetBrain(), null, null);
				}
			}
		}

		// Token: 0x06009B5B RID: 39771 RVA: 0x002D15CC File Offset: 0x002CF7CC
		public virtual void Notify_EquipmentLost(Pawn pawn)
		{
			if (!this.def.equippedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.equippedHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.def.equippedHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
			}
		}

		// Token: 0x06009B5C RID: 39772 RVA: 0x002D1638 File Offset: 0x002CF838
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
			if (this.def.killThought != null && pawn.needs.mood != null)
			{
				Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(this.def.killThought);
				thought_WeaponTrait.weapon = pawn.equipment.Primary;
				pawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
			}
		}

		// Token: 0x06009B5D RID: 39773 RVA: 0x002D16A4 File Offset: 0x002CF8A4
		public virtual void Notify_Unbonded(Pawn pawn)
		{
			if (!this.def.bondedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.bondedHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.def.bondedHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
			}
		}

		// Token: 0x06009B5E RID: 39774 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
		}

		// Token: 0x04006BBD RID: 27581
		public WeaponTraitDef def;
	}
}
