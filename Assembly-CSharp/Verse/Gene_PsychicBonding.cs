using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200065E RID: 1630
	public class Gene_PsychicBonding : Gene
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002914 RID: 10516 RVA: 0x000D8408 File Offset: 0x000D6608
		public bool CanBondToNewPawn
		{
			get
			{
				if (this.bondedPawn != null)
				{
					return false;
				}
				if (this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicBondTorn, false))
				{
					return false;
				}
				Pawn_NeedsTracker needs = this.pawn.needs;
				bool flag;
				if (needs == null)
				{
					flag = (null != null);
				}
				else
				{
					Need_Mood mood = needs.mood;
					if (mood == null)
					{
						flag = (null != null);
					}
					else
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts == null)
						{
							flag = (null != null);
						}
						else
						{
							MemoryThoughtHandler memories = thoughts.memories;
							flag = (((memories != null) ? memories.GetFirstMemoryOfDef(ThoughtDefOf.PsychicBondTorn) : null) != null);
						}
					}
				}
				return !flag;
			}
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x000D8484 File Offset: 0x000D6684
		public override void PostAdd()
		{
			base.PostAdd();
			Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond != null)
			{
				this.bondedPawn = (Pawn)hediff_PsychicBond.target;
			}
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000D84CC File Offset: 0x000D66CC
		public override void PostRemove()
		{
			base.PostRemove();
			this.Notify_MyOrPartnersGeneRemoved();
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000D84DC File Offset: 0x000D66DC
		public void BondTo(Pawn newBond)
		{
			if (!ModLister.CheckBiotech("Psychic bonding"))
			{
				return;
			}
			if (newBond == null)
			{
				return;
			}
			if (this.bondedPawn == newBond)
			{
				return;
			}
			if (this.bondedPawn != null)
			{
				Log.Error("Tried to bond to more than one pawn.");
				return;
			}
			this.bondedPawn = newBond;
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == this.bondedPawn);
						}
					}
				}
			}
			Pawn_NeedsTracker needs2 = this.bondedPawn.needs;
			if (needs2 != null)
			{
				Need_Mood mood2 = needs2.mood;
				if (mood2 != null)
				{
					ThoughtHandler thoughts2 = mood2.thoughts;
					if (thoughts2 != null)
					{
						MemoryThoughtHandler memories2 = thoughts2.memories;
						if (memories2 != null)
						{
							memories2.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == this.pawn);
						}
					}
				}
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.bondedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false);
			if (firstHediffOfDef2 != null)
			{
				this.bondedPawn.health.RemoveHediff(firstHediffOfDef2);
			}
			Hediff_PsychicBond hediff_PsychicBond = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, this.pawn, null);
			hediff_PsychicBond.target = this.bondedPawn;
			this.pawn.health.AddHediff(hediff_PsychicBond, null, null, null);
			Pawn_GeneTracker genes = this.bondedPawn.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			if (gene_PsychicBonding != null)
			{
				gene_PsychicBonding.BondTo(this.pawn);
				return;
			}
			Hediff_PsychicBond hediff_PsychicBond2 = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, this.bondedPawn, null);
			hediff_PsychicBond2.target = this.pawn;
			this.bondedPawn.health.AddHediff(hediff_PsychicBond2, null, null, null);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000D86BC File Offset: 0x000D68BC
		public void RemoveBond()
		{
			if (this.bondedPawn == null)
			{
				return;
			}
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.PsychicBondTorn, this.bondedPawn, null);
						}
					}
				}
			}
			Pawn_NeedsTracker needs2 = this.bondedPawn.needs;
			if (needs2 != null)
			{
				Need_Mood mood2 = needs2.mood;
				if (mood2 != null)
				{
					ThoughtHandler thoughts2 = mood2.thoughts;
					if (thoughts2 != null)
					{
						MemoryThoughtHandler memories2 = thoughts2.memories;
						if (memories2 != null)
						{
							memories2.TryGainMemory(ThoughtDefOf.PsychicBondTorn, this.pawn, null);
						}
					}
				}
			}
			Pawn pawn = this.bondedPawn;
			this.bondedPawn = null;
			Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond != null && hediff_PsychicBond.target == pawn)
			{
				this.pawn.health.RemoveHediff(hediff_PsychicBond);
			}
			Hediff_PsychicBond hediff_PsychicBond2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond2 != null)
			{
				pawn.health.RemoveHediff(hediff_PsychicBond2);
			}
			Pawn_GeneTracker genes = pawn.genes;
			if (genes != null)
			{
				Gene_PsychicBonding firstGeneOfType = genes.GetFirstGeneOfType<Gene_PsychicBonding>();
				if (firstGeneOfType != null)
				{
					firstGeneOfType.RemoveBond();
				}
			}
			if (!this.pawn.Dead)
			{
				if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false) == null)
				{
					Hediff_PsychicBondTorn hediff_PsychicBondTorn = (Hediff_PsychicBondTorn)HediffMaker.MakeHediff(HediffDefOf.PsychicBondTorn, pawn, null);
					hediff_PsychicBondTorn.target = this.pawn;
					pawn.health.AddHediff(hediff_PsychicBondTorn, null, null, null);
				}
				MentalBreakDef mentalBreakDef;
				if ((from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
				where d.intensity == MentalBreakIntensity.Extreme && d.Worker.BreakCanOccur(this.pawn)
				select d).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out mentalBreakDef))
				{
					mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_BondedHumanDeath".Translate(pawn), false);
				}
			}
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000D88A0 File Offset: 0x000D6AA0
		public void Notify_MyOrPartnersGeneRemoved()
		{
			if (this.bondedPawn != null)
			{
				Pawn_GeneTracker genes = this.bondedPawn.genes;
				if (((genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null) == null)
				{
					Pawn pawn = this.bondedPawn;
					this.bondedPawn = null;
					Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond != null && hediff_PsychicBond.target == pawn)
					{
						this.pawn.health.RemoveHediff(hediff_PsychicBond);
					}
					Hediff_PsychicBond hediff_PsychicBond2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond2 != null)
					{
						pawn.health.RemoveHediff(hediff_PsychicBond2);
					}
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 == null)
					{
						return;
					}
					Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
					if (firstGeneOfType == null)
					{
						return;
					}
					firstGeneOfType.Notify_MyOrPartnersGeneRemoved();
					return;
				}
			}
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000D8960 File Offset: 0x000D6B60
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos && this.CanBondToNewPawn && this.pawn.Spawned)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Bond to random pawn",
					action = delegate()
					{
						Pawn newBond;
						if ((from x in this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction)
						where x.RaceProps.Humanlike && x != this.pawn
						select x).TryRandomElement(out newBond))
						{
							this.BondTo(newBond);
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x000D8970 File Offset: 0x000D6B70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.bondedPawn, "bondedPawn", false);
		}

		// Token: 0x040021A3 RID: 8611
		private Pawn bondedPawn;
	}
}
