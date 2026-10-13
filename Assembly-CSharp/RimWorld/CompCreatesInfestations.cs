using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C49 RID: 11337
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x17002806 RID: 10246
		// (get) Token: 0x0600FE3E RID: 65086 RVA: 0x0049B9F8 File Offset: 0x00499BF8
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedLastTick()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x17002807 RID: 10247
		// (get) Token: 0x0600FE3F RID: 65087 RVA: 0x0049BA33 File Offset: 0x00499C33
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 420000;
			}
		}

		// Token: 0x17002808 RID: 10248
		// (get) Token: 0x0600FE40 RID: 65088 RVA: 0x0049BA50 File Offset: 0x00499C50
		public bool CantFireBecauseSomethingElseCreatedInfestationRecently
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				List<Thing> list = this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != this.parent && list[i].Position.InHorDistOf(this.parent.Position, 10f) && list[i].TryGetComp<CompCreatesInfestations>().CantFireBecauseCreatedInfestationRecently)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600FE41 RID: 65089 RVA: 0x0049BAE0 File Offset: 0x00499CE0
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x0600FE42 RID: 65090 RVA: 0x0049BAF8 File Offset: 0x00499CF8
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0400A6A0 RID: 42656
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x0400A6A1 RID: 42657
		private const float MinRefireDays = 7f;

		// Token: 0x0400A6A2 RID: 42658
		private const float PreventInfestationsDist = 10f;
	}
}
