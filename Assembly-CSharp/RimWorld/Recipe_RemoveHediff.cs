using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002315 RID: 8981
	public class Recipe_RemoveHediff : Recipe_Surgery
	{
		// Token: 0x0600C307 RID: 49927 RVA: 0x003889C4 File Offset: 0x00386BC4
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if ((!this.recipe.targetsBodyPart || hediffs[i].Part != null) && hediffs[i].def == this.recipe.removesHediff && hediffs[i].Visible)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C308 RID: 49928 RVA: 0x00388A50 File Offset: 0x00386C50
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
			int num;
			for (int i = 0; i < allHediffs.Count; i = num + 1)
			{
				if (allHediffs[i].Part != null && allHediffs[i].def == recipe.removesHediff && allHediffs[i].Visible)
				{
					yield return allHediffs[i].Part;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600C309 RID: 49929 RVA: 0x00388A68 File Offset: 0x00386C68
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
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
				if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
				{
					string text;
					if (!this.recipe.successfullyRemovedHediffMessage.NullOrEmpty())
					{
						text = this.recipe.successfullyRemovedHediffMessage.Formatted(billDoer.LabelShort, pawn.LabelShort);
					}
					else
					{
						text = "MessageSuccessfullyRemovedHediff".Translate(billDoer.LabelShort, pawn.LabelShort, this.recipe.removesHediff.label.Named("HEDIFF"), billDoer.Named("SURGEON"), pawn.Named("PATIENT"));
					}
					Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.recipe.targetsBodyPart)
			{
				Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == this.recipe.removesHediff && x.Part == part && x.Visible);
				if (hediff != null)
				{
					pawn.health.RemoveHediff(hediff);
					return;
				}
			}
			else
			{
				for (int i = pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
				{
					Hediff hediff2 = pawn.health.hediffSet.hediffs[i];
					if (hediff2.def == this.recipe.removesHediff && hediff2.Visible)
					{
						pawn.health.RemoveHediff(hediff2);
					}
				}
			}
		}
	}
}
