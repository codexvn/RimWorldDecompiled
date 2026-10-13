using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028A1 RID: 10401
	public class ScenPart_ConfigPage_ConfigureStartingPawns_Xenotypes : ScenPart_ConfigPage_ConfigureStartingPawnsBase
	{
		// Token: 0x170022A2 RID: 8866
		// (get) Token: 0x0600E3AB RID: 58283 RVA: 0x00428057 File Offset: 0x00426257
		private float ElementHeight
		{
			get
			{
				return ScenPart.RowHeight * 4f;
			}
		}

		// Token: 0x170022A3 RID: 8867
		// (get) Token: 0x0600E3AC RID: 58284 RVA: 0x00428831 File Offset: 0x00426A31
		protected override int TotalPawnCount
		{
			get
			{
				return this.xenotypeCounts.Sum((XenotypeCount x) => x.count);
			}
		}

		// Token: 0x0600E3AD RID: 58285 RVA: 0x00428860 File Offset: 0x00426A60
		public override string Summary(Scenario scen)
		{
			string result;
			if ((result = this.customSummary) == null)
			{
				result = "ScenPart_StartWithSpecificColonists".Translate((from x in this.xenotypeCounts
				select x.Summary).ToCommaList(true, false), this.pawnChoiceCount);
			}
			return result;
		}

		// Token: 0x0600E3AE RID: 58286 RVA: 0x004288C8 File Offset: 0x00426AC8
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, this.ElementHeight * (float)this.xenotypeCounts.Count + ScenPart.RowHeight);
			scenPartRect.height = ScenPart.RowHeight;
			for (int i = 0; i < this.xenotypeCounts.Count; i++)
			{
				XenotypeCount xenotypeCount = this.xenotypeCounts[i];
				Widgets.TextFieldNumeric<int>(scenPartRect, ref xenotypeCount.count, ref xenotypeCount.countBuffer, 1f, 10f);
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect = scenPartRect;
				rect.xMax -= ScenPart.RowHeight;
				if (Widgets.ButtonText(rect, xenotypeCount.xenotype.LabelCap, true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (XenotypeDef localXen2 in DefDatabase<XenotypeDef>.AllDefs)
					{
						XenotypeDef localXen = localXen2;
						list.Add(new FloatMenuOption(localXen.LabelCap, delegate()
						{
							xenotypeCount.xenotype = localXen;
						}, localXen.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
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
					this.xenotypeCounts.RemoveAt(i);
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
				Widgets.Checkbox(scenPartRect.xMin + vector.x + 4f, scenPartRect.y, ref xenotypeCount.requiredAtStart, 24f, false, false, null, null);
				scenPartRect.y += ScenPart.RowHeight * 2f;
			}
			if (Widgets.ButtonText(scenPartRect, "Add".Translate().CapitalizeFirst(), true, true, true, null))
			{
				this.xenotypeCounts.Add(new XenotypeCount
				{
					xenotype = XenotypeDefOf.Baseliner,
					count = 1
				});
				return;
			}
			scenPartRect.y += ScenPart.RowHeight;
		}

		// Token: 0x0600E3AF RID: 58287 RVA: 0x00428BA4 File Offset: 0x00426DA4
		protected override ScenPart CopyForEditingInner()
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_Xenotypes scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes = new ScenPart_ConfigPage_ConfigureStartingPawns_Xenotypes
			{
				def = this.def,
				visible = this.visible,
				summarized = this.summarized,
				pawnChoiceCount = this.pawnChoiceCount,
				customSummary = this.customSummary
			};
			foreach (XenotypeCount xenotypeCount in this.xenotypeCounts)
			{
				scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes.xenotypeCounts.Add(new XenotypeCount
				{
					xenotype = xenotypeCount.xenotype,
					count = xenotypeCount.count,
					allowedDevelopmentalStages = xenotypeCount.allowedDevelopmentalStages,
					description = xenotypeCount.description,
					requiredAtStart = xenotypeCount.requiredAtStart
				});
			}
			foreach (XenotypePawnKind xenotypePawnKind in this.overrideKinds)
			{
				scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes.overrideKinds.Add(new XenotypePawnKind
				{
					xenotype = xenotypePawnKind.xenotype,
					pawnKind = xenotypePawnKind.pawnKind
				});
			}
			return scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes;
		}

		// Token: 0x0600E3B0 RID: 58288 RVA: 0x00428CE8 File Offset: 0x00426EE8
		protected override void GenerateStartingPawns()
		{
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				int num2 = 0;
				foreach (XenotypeCount xenotypeCount in this.xenotypeCounts)
				{
					for (int i = 0; i < xenotypeCount.count; i++)
					{
						PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(num2);
						generationRequest.ForcedXenotype = xenotypeCount.xenotype;
						if (xenotypeCount.xenotype != null)
						{
							generationRequest.PawnKindDefGetter = delegate(XenotypeDef t)
							{
								foreach (XenotypePawnKind xenotypePawnKind in this.overrideKinds)
								{
									if (t == xenotypePawnKind.xenotype)
									{
										return xenotypePawnKind.pawnKind;
									}
								}
								return Find.GameInitData.startingPawnKind ?? Faction.OfPlayer.def.basicMemberKind;
							};
						}
						else
						{
							generationRequest.PawnKindDefGetter = null;
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

		// Token: 0x0600E3B1 RID: 58289 RVA: 0x00428DB4 File Offset: 0x00426FB4
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnsRequired = null;
			Find.GameInitData.startingPawnKind = null;
			Find.GameInitData.startingXenotypesRequired = (from c in this.xenotypeCounts
			where c.requiredAtStart
			select c).ToList<XenotypeCount>();
			base.PostIdeoChosen();
		}

		// Token: 0x0600E3B2 RID: 58290 RVA: 0x00428E18 File Offset: 0x00427018
		public override bool HasNullDefs()
		{
			if (base.HasNullDefs())
			{
				return true;
			}
			using (List<XenotypeCount>.Enumerator enumerator = this.xenotypeCounts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.xenotype == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600E3B3 RID: 58291 RVA: 0x00428E7C File Offset: 0x0042707C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<XenotypeCount>(ref this.xenotypeCounts, "xenotypeCounts", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600E3B4 RID: 58292 RVA: 0x00428E9C File Offset: 0x0042709C
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			foreach (XenotypeCount xenotypeCount in this.xenotypeCounts)
			{
				num ^= xenotypeCount.GetHashCode();
			}
			return num;
		}

		// Token: 0x04009874 RID: 39028
		public List<XenotypeCount> xenotypeCounts = new List<XenotypeCount>();

		// Token: 0x04009875 RID: 39029
		public List<XenotypePawnKind> overrideKinds = new List<XenotypePawnKind>();

		// Token: 0x04009876 RID: 39030
		[MustTranslate]
		public string customSummary;
	}
}
