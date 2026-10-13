using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200230D RID: 8973
	public class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		// Token: 0x17001DCC RID: 7628
		// (get) Token: 0x0600C2DE RID: 49886 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool SpawnPartsWhenRemoved
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C2DF RID: 49887 RVA: 0x00388226 File Offset: 0x00386426
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			IEnumerable<BodyPartRecord> notMissingParts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			using (IEnumerator<BodyPartRecord> enumerator = notMissingParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BodyPartRecord part = enumerator.Current;
					if (pawn.health.hediffSet.HasDirectlyAddedPartFor(part))
					{
						yield return part;
					}
					else if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
					{
						yield return part;
					}
					else if (part != pawn.RaceProps.body.corePart && part.def.canSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Hediff d) => (!(d is Hediff_Injury) || d.IsPermanent()) && d.def.isBad && d.Visible && d.Part == part))
					{
						yield return part;
					}
					else if (part.def.forceAlwaysRemovable)
					{
						yield return part;
					}
				}
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C2E0 RID: 49888 RVA: 0x00388236 File Offset: 0x00386436
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return ((pawn.Faction != billDoerFaction && pawn.Faction != null) || pawn.IsQuestLodger()) && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x0600C2E1 RID: 49889 RVA: 0x00388260 File Offset: 0x00386460
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				Hediff directlyAddedPartFor = pawn.health.hediffSet.GetDirectlyAddedPartFor(part);
				if (directlyAddedPartFor != null)
				{
					directlyAddedPartFor.Notify_SurgicallyRemoved(billDoer);
				}
				if (this.SpawnPartsWhenRemoved)
				{
					MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
					MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
				}
			}
			this.DamagePart(pawn, part);
			pawn.Drawer.renderer.SetAllGraphicsDirty();
			if (flag)
			{
				this.ApplyThoughts(pawn, billDoer);
			}
			if (flag2)
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
			}
		}

		// Token: 0x0600C2E2 RID: 49890 RVA: 0x00388334 File Offset: 0x00386534
		public virtual void DamagePart(Pawn pawn, BodyPartRecord part)
		{
			pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, part, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
		}

		// Token: 0x0600C2E3 RID: 49891 RVA: 0x0038836B File Offset: 0x0038656B
		public virtual void ApplyThoughts(Pawn pawn, Pawn billDoer)
		{
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.OrganHarvesting);
				return;
			}
			ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn, billDoer);
		}

		// Token: 0x0600C2E4 RID: 49892 RVA: 0x00388388 File Offset: 0x00386588
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			if (!part.def.removeRecipeLabelOverride.NullOrEmpty())
			{
				return part.def.removeRecipeLabelOverride;
			}
			if (pawn.RaceProps.IsMechanoid || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				Hediff hediff;
				if (pawn.health.hediffSet.TryGetDirectlyAddedPartFor(part, out hediff))
				{
					return "RemovePart".Translate(hediff.Label);
				}
				return RecipeDefOf.RemoveBodyPart.label;
			}
			else
			{
				BodyPartRemovalIntent bodyPartRemovalIntent = HealthUtility.PartRemovalIntent(pawn, part);
				if (bodyPartRemovalIntent == BodyPartRemovalIntent.Harvest)
				{
					return "HarvestOrgan".Translate();
				}
				if (bodyPartRemovalIntent != BodyPartRemovalIntent.Amputate)
				{
					throw new InvalidOperationException();
				}
				if (part.depth == BodyPartDepth.Inside || part.def.socketed)
				{
					return "RemoveOrgan".Translate();
				}
				return "Amputate".Translate();
			}
		}
	}
}
