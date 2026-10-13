using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC2 RID: 7106
	public class QuestPart_BossgroupArrives : QuestPartActivable
	{
		// Token: 0x06009E2F RID: 40495 RVA: 0x002DC424 File Offset: 0x002DA624
		public override void QuestPartTick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.enableTick + this.minDelay <= ticksGame)
			{
				if (this.enableTick + this.maxDelay <= ticksGame)
				{
					base.Complete();
					return;
				}
				if (this.mapParent.IsHashIntervalTick(2500) && this.bossgroupDef.Worker.ShouldSummonNow(this.mapParent.Map))
				{
					base.Complete();
				}
			}
		}

		// Token: 0x06009E30 RID: 40496 RVA: 0x002DC4A0 File Offset: 0x002DA6A0
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			int num = this.enableTick + this.minDelay - Find.TickManager.TicksGame;
			int numTicks = this.enableTick + this.maxDelay - Find.TickManager.TicksGame;
			string text = "";
			if (num >= 0)
			{
				text = string.Concat(new string[]
				{
					text,
					"\nTicks until available: ",
					num.ToString(),
					" (",
					num.ToStringTicksToPeriodVerbose(true, true),
					")"
				});
			}
			text = string.Concat(new string[]
			{
				text,
				"\nTicks until forced arrival: ",
				numTicks.ToString(),
				" (",
				numTicks.ToStringTicksToPeriodVerbose(true, true),
				")"
			});
			text = text + "\nShould summon now (blocked by visitors etc.): " + this.bossgroupDef.Worker.ShouldSummonNow(this.mapParent.Map).ToString();
			Vector2 vector = Text.CalcSize(text);
			Rect rect = new Rect(innerRect.x, curY, innerRect.width, vector.y);
			Widgets.Label(rect, text);
			curY += rect.height + 4f;
			Rect rect2 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect2, "End " + ((this != null) ? this.ToString() : null), true, true, true, null))
			{
				base.Complete();
			}
			curY += rect2.height;
		}

		// Token: 0x06009E31 RID: 40497 RVA: 0x002DC63C File Offset: 0x002DA83C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.minDelay, "minDelay", 0, false);
			Scribe_Values.Look<int>(ref this.maxDelay, "maxDelay", 0, false);
			Scribe_Defs.Look<BossgroupDef>(ref this.bossgroupDef, "bossgroupDef");
		}

		// Token: 0x04006D80 RID: 28032
		private const int CheckInterval = 2500;

		// Token: 0x04006D81 RID: 28033
		public int minDelay;

		// Token: 0x04006D82 RID: 28034
		public int maxDelay;

		// Token: 0x04006D83 RID: 28035
		public MapParent mapParent;

		// Token: 0x04006D84 RID: 28036
		public BossgroupDef bossgroupDef;
	}
}
