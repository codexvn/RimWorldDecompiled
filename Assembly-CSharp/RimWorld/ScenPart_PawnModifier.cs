using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002887 RID: 10375
	public class ScenPart_PawnModifier : ScenPart
	{
		// Token: 0x0600E302 RID: 58114 RVA: 0x00426DB8 File Offset: 0x00424FB8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Values.Look<PawnGenerationContext>(ref this.context, "context", PawnGenerationContext.All, false);
			Scribe_Values.Look<bool>(ref this.hideOffMap, "hideOffMap", false, false);
		}

		// Token: 0x0600E303 RID: 58115 RVA: 0x00426E08 File Offset: 0x00425008
		protected void DoPawnModifierEditInterface(Rect rect)
		{
			Rect rect2 = rect.TopHalf();
			Rect rect3 = rect2.LeftPart(0.333f).Rounded();
			Rect rect4 = rect2.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect3, "chance".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect4, ref this.chance, ref this.chanceBuf, 0f, 1f);
			Rect rect5 = rect.BottomHalf();
			Rect rect6 = rect5.LeftPart(0.333f).Rounded();
			Rect rect7 = rect5.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect6, "context".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonText(rect7, this.context.ToStringHuman(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(PawnGenerationContext)))
				{
					PawnGenerationContext localCont2 = (PawnGenerationContext)obj;
					PawnGenerationContext localCont = localCont2;
					list.Add(new FloatMenuOption(localCont.ToStringHuman(), delegate()
					{
						this.context = localCont;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600E304 RID: 58116 RVA: 0x00426F8C File Offset: 0x0042518C
		public override void Randomize()
		{
			this.chance = GenMath.RoundedHundredth(Rand.Range(0.05f, 1f));
			this.context = PawnGenerationContextUtility.GetRandom();
			this.hideOffMap = false;
		}

		// Token: 0x0600E305 RID: 58117 RVA: 0x00426FBA File Offset: 0x004251BA
		public override void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyNewPawn(pawn);
			}
		}

		// Token: 0x0600E306 RID: 58118 RVA: 0x00426FF9 File Offset: 0x004251F9
		public override void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyPawnPostGenerate(pawn, redressed);
			}
		}

		// Token: 0x0600E307 RID: 58119 RVA: 0x0042703C File Offset: 0x0042523C
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.hideOffMap && this.context.Includes(PawnGenerationContext.PlayerStarter))
			{
				foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
				{
					if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
					{
						this.ModifyHideOffMapStartingPawnPostMapGenerate(pawn);
					}
				}
			}
		}

		// Token: 0x0600E308 RID: 58120 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ModifyNewPawn(Pawn p)
		{
		}

		// Token: 0x0600E309 RID: 58121 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
		}

		// Token: 0x0600E30A RID: 58122 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
		}

		// Token: 0x0600E30B RID: 58123 RVA: 0x004270CC File Offset: 0x004252CC
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.context.GetHashCode() ^ this.chance.GetHashCode() ^ (this.hideOffMap ? 1 : 0);
		}

		// Token: 0x04009832 RID: 38962
		protected float chance = 1f;

		// Token: 0x04009833 RID: 38963
		protected PawnGenerationContext context;

		// Token: 0x04009834 RID: 38964
		protected bool hideOffMap;

		// Token: 0x04009835 RID: 38965
		private string chanceBuf;
	}
}
