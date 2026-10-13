using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028E6 RID: 10470
	public class ScenPart_StartingMech : ScenPart
	{
		// Token: 0x170022C2 RID: 8898
		// (get) Token: 0x0600E4F7 RID: 58615 RVA: 0x0042DC73 File Offset: 0x0042BE73
		private IEnumerable<PawnKindDef> PossibleMechs
		{
			get
			{
				return from td in DefDatabase<PawnKindDef>.AllDefs
				where td.RaceProps.IsMechanoid && td.race.GetCompProperties<CompProperties_OverseerSubject>() != null
				select td;
			}
		}

		// Token: 0x0600E4F8 RID: 58616 RVA: 0x0042DC9E File Offset: 0x0042BE9E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.mechKind, "mechKind");
			Scribe_Values.Look<float>(ref this.overseenByPlayerPawnChance, "overseenByPlayerPawnChance", 0f, false);
		}

		// Token: 0x0600E4F9 RID: 58617 RVA: 0x0042DCCC File Offset: 0x0042BECC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, 2f * ScenPart.RowHeight + 4f);
			if (Widgets.ButtonText(new Rect(scenPartRect.xMin, scenPartRect.yMin, scenPartRect.width, ScenPart.RowHeight), (this.mechKind != null) ? this.mechKind.LabelCap : "RandomMech".Translate(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("RandomMech".Translate().CapitalizeFirst(), delegate()
				{
					this.mechKind = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				foreach (PawnKindDef localKind2 in this.PossibleMechs)
				{
					PawnKindDef localKind = localKind2;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
					{
						this.mechKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Rect rect = new Rect(scenPartRect.xMin, scenPartRect.yMin + ScenPart.RowHeight + 4f, scenPartRect.width, ScenPart.RowHeight);
			TaggedString taggedString = "StartOverseenChance".Translate();
			float x = Text.CalcSize(taggedString).x;
			Widgets.Label(new Rect(rect.xMin, rect.yMin, x, ScenPart.RowHeight), taggedString);
			Rect rect2 = new Rect(rect.xMin + x + 4f, rect.yMin, rect.width - x - 4f, ScenPart.RowHeight);
			this.overseenByPlayerPawnChance = Widgets.HorizontalSlider(rect2, this.overseenByPlayerPawnChance, 0f, 1f, false, this.overseenByPlayerPawnChance.ToStringPercent(), null, null, 0.01f);
		}

		// Token: 0x0600E4FA RID: 58618 RVA: 0x0042DEF8 File Offset: 0x0042C0F8
		public override void Randomize()
		{
			this.mechKind = this.PossibleMechs.RandomElement<PawnKindDef>();
			this.overseenByPlayerPawnChance = Rand.Range(0f, 1f);
		}

		// Token: 0x0600E4FB RID: 58619 RVA: 0x0042D59E File Offset: 0x0042B79E
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600E4FC RID: 58620 RVA: 0x0042DF20 File Offset: 0x0042C120
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return "Mechanoid".Translate().CapitalizeFirst() + ": " + this.mechKind.LabelCap;
			}
			yield break;
		}

		// Token: 0x0600E4FD RID: 58621 RVA: 0x0042DF37 File Offset: 0x0042C137
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			if (this.mechKind == null)
			{
				this.mechKind = this.PossibleMechs.RandomElement<PawnKindDef>();
			}
			PawnGenerationRequest request = new PawnGenerationRequest(this.mechKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
			Pawn mech = PawnGenerator.GeneratePawn(request);
			if (Rand.Chance(this.overseenByPlayerPawnChance) && mech.OverseerSubject != null)
			{
				Pawn pawn = (from p in Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount)
				where MechanitorUtility.IsMechanitor(p) && p.mechanitor.CanOverseeSubject(mech)
				orderby p.mechanitor.OverseenPawns.Count
				select p).RandomElementWithFallback(null);
				if (pawn != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, mech);
				}
			}
			yield return mech;
			yield break;
		}

		// Token: 0x0600E4FE RID: 58622 RVA: 0x0042DF47 File Offset: 0x0042C147
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.mechKind != null) ? this.mechKind.GetHashCode() : 0);
		}

		// Token: 0x04009940 RID: 39232
		private PawnKindDef mechKind;

		// Token: 0x04009941 RID: 39233
		private float overseenByPlayerPawnChance = 0.5f;
	}
}
