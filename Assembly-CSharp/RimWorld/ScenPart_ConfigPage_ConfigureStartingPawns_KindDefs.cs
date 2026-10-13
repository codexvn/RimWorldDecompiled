using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200289B RID: 10395
	public class ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs : ScenPart_ConfigPage_ConfigureStartingPawnsBase
	{
		// Token: 0x1700229E RID: 8862
		// (get) Token: 0x0600E391 RID: 58257 RVA: 0x00428057 File Offset: 0x00426257
		private float ElementHeight
		{
			get
			{
				return ScenPart.RowHeight * 4f;
			}
		}

		// Token: 0x1700229F RID: 8863
		// (get) Token: 0x0600E392 RID: 58258 RVA: 0x00428064 File Offset: 0x00426264
		private IEnumerable<PawnKindDef> AvailableKinds
		{
			get
			{
				return from x in DefDatabase<PawnKindDef>.AllDefs
				where x.RaceProps.Humanlike && x.defaultFactionDef != null && x.defaultFactionDef.isPlayer
				select x;
			}
		}

		// Token: 0x170022A0 RID: 8864
		// (get) Token: 0x0600E393 RID: 58259 RVA: 0x0042808F File Offset: 0x0042628F
		protected override int TotalPawnCount
		{
			get
			{
				return this.kindCounts.Sum((PawnKindCount x) => x.count);
			}
		}

		// Token: 0x0600E394 RID: 58260 RVA: 0x004280BC File Offset: 0x004262BC
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartWithSpecificColonists".Translate((from x in this.kindCounts
			select x.Summary).ToCommaList(true, false), this.pawnChoiceCount);
		}

		// Token: 0x0600E395 RID: 58261 RVA: 0x0042811C File Offset: 0x0042631C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, this.ElementHeight * (float)this.kindCounts.Count + ScenPart.RowHeight);
			scenPartRect.height = ScenPart.RowHeight;
			for (int i = 0; i < this.kindCounts.Count; i++)
			{
				PawnKindCount kindCount = this.kindCounts[i];
				Widgets.TextFieldNumeric<int>(scenPartRect, ref kindCount.count, ref kindCount.countBuffer, 1f, 10f);
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect = scenPartRect;
				rect.xMax -= ScenPart.RowHeight;
				if (Widgets.ButtonText(rect, kindCount.kindDef.LabelCap, true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (PawnKindDef localKind2 in this.AvailableKinds)
					{
						PawnKindDef localKind = localKind2;
						list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
						{
							kindCount.kindDef = localKind;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					if (list.Any<FloatMenuOption>())
					{
						Find.WindowStack.Add(new FloatMenu(list));
					}
				}
				Rect butRect = scenPartRect;
				butRect.xMin = rect.xMax;
				if (Widgets.ButtonImage(butRect, TexButton.Delete, true, null))
				{
					this.kindCounts.RemoveAt(i);
					return;
				}
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect2 = scenPartRect;
				TaggedString taggedString = "Required".Translate();
				Vector2 vector = Text.CalcSize(taggedString);
				rect2.width = vector.x;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect2, taggedString);
				Text.Anchor = TextAnchor.UpperLeft;
				Widgets.Checkbox(scenPartRect.xMin + vector.x + 4f, scenPartRect.y, ref kindCount.requiredAtStart, 24f, false, false, null, null);
				scenPartRect.y += ScenPart.RowHeight * 2f;
			}
			if (Widgets.ButtonText(scenPartRect, "Add".Translate().CapitalizeFirst(), true, true, true, null))
			{
				this.kindCounts.Add(new PawnKindCount
				{
					kindDef = PawnKindDefOf.Colonist,
					count = 1
				});
				return;
			}
			scenPartRect.y += ScenPart.RowHeight;
		}

		// Token: 0x0600E396 RID: 58262 RVA: 0x004283E8 File Offset: 0x004265E8
		protected override ScenPart CopyForEditingInner()
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs scenPart_ConfigPage_ConfigureStartingPawns_KindDefs = new ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs
			{
				def = this.def,
				visible = this.visible,
				summarized = this.summarized,
				pawnChoiceCount = this.pawnChoiceCount,
				leftBehindPawnKind = this.leftBehindPawnKind
			};
			foreach (PawnKindCount pawnKindCount in this.kindCounts)
			{
				scenPart_ConfigPage_ConfigureStartingPawns_KindDefs.kindCounts.Add(new PawnKindCount
				{
					kindDef = pawnKindCount.kindDef,
					count = pawnKindCount.count,
					requiredAtStart = pawnKindCount.requiredAtStart
				});
			}
			return scenPart_ConfigPage_ConfigureStartingPawns_KindDefs;
		}

		// Token: 0x0600E397 RID: 58263 RVA: 0x004284AC File Offset: 0x004266AC
		protected override void GenerateStartingPawns()
		{
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				int num2 = 0;
				foreach (PawnKindCount pawnKindCount in this.kindCounts)
				{
					for (int i = 0; i < pawnKindCount.count; i++)
					{
						PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(num2);
						generationRequest.KindDef = pawnKindCount.kindDef;
						if (ModsConfig.BiotechActive)
						{
							if (pawnKindCount.kindDef.xenotypeSet != null)
							{
								generationRequest.ForcedXenotype = null;
							}
							else
							{
								generationRequest.ForcedXenotype = XenotypeDefOf.Baseliner;
							}
						}
						StartingPawnUtility.SetGenerationRequest(num2, generationRequest);
						StartingPawnUtility.AddNewPawn(num2);
						num2++;
					}
				}
				num++;
			}
			while (num <= 20 && !StartingPawnUtility.WorkTypeRequirementsSatisfied());
		}

		// Token: 0x0600E398 RID: 58264 RVA: 0x0042857C File Offset: 0x0042677C
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnKind = this.leftBehindPawnKind;
			Find.GameInitData.startingPawnsRequired = (from c in this.kindCounts
			where c.requiredAtStart
			select c).ToList<PawnKindCount>();
			Find.GameInitData.startingXenotypesRequired = null;
			base.PostIdeoChosen();
		}

		// Token: 0x0600E399 RID: 58265 RVA: 0x004285E4 File Offset: 0x004267E4
		public override bool HasNullDefs()
		{
			if (base.HasNullDefs())
			{
				return true;
			}
			using (List<PawnKindCount>.Enumerator enumerator = this.kindCounts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.kindDef == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600E39A RID: 58266 RVA: 0x00428648 File Offset: 0x00426848
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<PawnKindCount>(ref this.kindCounts, "kindCounts", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600E39B RID: 58267 RVA: 0x00428668 File Offset: 0x00426868
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			foreach (PawnKindCount pawnKindCount in this.kindCounts)
			{
				num ^= pawnKindCount.GetHashCode();
			}
			return num;
		}

		// Token: 0x04009865 RID: 39013
		public List<PawnKindCount> kindCounts = new List<PawnKindCount>();

		// Token: 0x04009866 RID: 39014
		private PawnKindDef leftBehindPawnKind;
	}
}
