using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000540 RID: 1344
	public sealed class TooltipGiverList
	{
		// Token: 0x060022DD RID: 8925 RVA: 0x000B563C File Offset: 0x000B383C
		public void Notify_ThingSpawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Add(t);
			}
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000B5660 File Offset: 0x000B3860
		public void Notify_ThingDespawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Remove(t);
			}
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000B5688 File Offset: 0x000B3888
		public void DispenseAllThingTooltips()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (Find.WindowStack.FloatMenu != null)
			{
				return;
			}
			if (Find.Targeter.IsTargeting && Find.Targeter.targetingSource != null && Find.Targeter.targetingSource.HidePawnTooltips)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			float cellSizePixels = Find.CameraDriver.CellSizePixels;
			Vector2 vector = new Vector2(cellSizePixels, cellSizePixels);
			Rect rect = new Rect(0f, 0f, vector.x, vector.y);
			for (int i = 0; i < this.givers.Count; i++)
			{
				Thing thing = this.givers[i];
				if (currentViewRect.Contains(thing.Position) && !thing.Position.Fogged(thing.Map))
				{
					Vector2 vector2 = thing.DrawPos.MapToUIPosition();
					rect.x = vector2.x - vector.x / 2f;
					rect.y = vector2.y - vector.y / 2f;
					if (rect.Contains(Event.current.mousePosition))
					{
						string text = this.ShouldShowShotReport(thing) ? TooltipUtility.ShotCalculationTipString(thing) : null;
						if (thing.def.hasTooltip || !text.NullOrEmpty())
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null && pawn.IsHiddenFromPlayer())
							{
								return;
							}
							TipSignal tooltip = thing.GetTooltip();
							if (!text.NullOrEmpty())
							{
								tooltip.text = tooltip.text + "\n\n" + text;
							}
							TooltipHandler.TipRegion(rect, tooltip);
						}
					}
				}
			}
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000B583B File Offset: 0x000B3A3B
		private bool ShouldShowShotReport(Thing t)
		{
			return t.def.hasTooltip || t is Hive || t is IAttackTarget;
		}

		// Token: 0x04001D2C RID: 7468
		private List<Thing> givers = new List<Thing>();
	}
}
