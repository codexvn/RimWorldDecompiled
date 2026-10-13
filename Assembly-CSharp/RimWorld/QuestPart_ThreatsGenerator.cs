using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D07 RID: 7431
	public class QuestPart_ThreatsGenerator : QuestPartActivable, IIncidentMakerQuestPart
	{
		// Token: 0x0600A60D RID: 42509 RVA: 0x002F8873 File Offset: 0x002F6A73
		public IEnumerable<FiringIncident> MakeIntervalIncidents()
		{
			if (this.mapParent != null && this.mapParent.HasMap)
			{
				return ThreatsGenerator.MakeIntervalIncidents(this.parms, this.mapParent.Map, base.EnableTick + this.threatStartTicks);
			}
			return Enumerable.Empty<FiringIncident>();
		}

		// Token: 0x0600A60E RID: 42510 RVA: 0x002F88B4 File Offset: 0x002F6AB4
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Log future incidents from " + base.GetType().Name, true, true, true, null))
			{
				StorytellerUtility.DebugLogTestFutureIncidents(false, null, this, 50);
			}
			curY += rect.height + 4f;
		}

		// Token: 0x0600A60F RID: 42511 RVA: 0x002F892A File Offset: 0x002F6B2A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThreatsGeneratorParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.threatStartTicks, "threatStartTicks", 0, false);
		}

		// Token: 0x040072BE RID: 29374
		public ThreatsGeneratorParams parms;

		// Token: 0x040072BF RID: 29375
		public MapParent mapParent;

		// Token: 0x040072C0 RID: 29376
		public int threatStartTicks;
	}
}
