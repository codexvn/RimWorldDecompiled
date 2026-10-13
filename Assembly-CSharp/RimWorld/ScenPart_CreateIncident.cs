using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028AA RID: 10410
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x170022A7 RID: 8871
		// (get) Token: 0x0600E3D6 RID: 58326 RVA: 0x00429739 File Offset: 0x00427939
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x170022A8 RID: 8872
		// (get) Token: 0x0600E3D7 RID: 58327 RVA: 0x00429740 File Offset: 0x00427940
		private float RandomInterval
		{
			get
			{
				float maxInclusive = (this.maxDays > 0f && this.maxDays > this.minDays) ? this.maxDays : this.minDays;
				return Rand.Range(this.minDays, maxInclusive);
			}
		}

		// Token: 0x0600E3D8 RID: 58328 RVA: 0x00429784 File Offset: 0x00427984
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<float>(ref this.minDays, "minDays", this.intervalDays, false);
			Scribe_Values.Look<float>(ref this.maxDays, "maxDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x0600E3D9 RID: 58329 RVA: 0x00429814 File Offset: 0x00427A14
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.intervalDays.GetHashCode() ^ (this.repeat ? 1 : 0) ^ this.occurTick.GetHashCode() ^ (this.isFinished ? 1 : 0);
		}

		// Token: 0x0600E3DA RID: 58330 RVA: 0x00429850 File Offset: 0x00427A50
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width / 2f, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x + scenPartRect.width / 2f, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width / 2f, scenPartRect.height / 3f);
			Rect rect4 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			base.DoIncidentEditInterface(rect);
			Widgets.TextFieldNumericLabeled<float>(rect2, "minDays".Translate(), ref this.minDays, ref this.minDaysBuffer, 0f, 1E+09f);
			Widgets.TextFieldNumericLabeled<float>(rect3, "maxDays".Translate(), ref this.maxDays, ref this.maxDaysBuffer, 0f, 1E+09f);
			Widgets.CheckboxLabeled(rect4, "repeat".Translate(), ref this.repeat, false, null, null, false, false);
		}

		// Token: 0x0600E3DB RID: 58331 RVA: 0x004299D8 File Offset: 0x00427BD8
		public override void Randomize()
		{
			base.Randomize();
			this.intervalDays = 15f * Rand.Gaussian(0f, 1f) + 30f;
			if (this.intervalDays < 0f)
			{
				this.intervalDays = 0f;
			}
			this.minDays = this.intervalDays;
			this.repeat = (Rand.Range(0, 100) < 50);
		}

		// Token: 0x0600E3DC RID: 58332 RVA: 0x00429A42 File Offset: 0x00427C42
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		// Token: 0x0600E3DD RID: 58333 RVA: 0x00429A4B File Offset: 0x00427C4B
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + 60000f * this.RandomInterval;
		}

		// Token: 0x0600E3DE RID: 58334 RVA: 0x00429A74 File Offset: 0x00427C74
		public override void Tick()
		{
			base.Tick();
			if (Find.AnyPlayerHomeMap == null)
			{
				return;
			}
			if (this.isFinished)
			{
				return;
			}
			if (this.incident == null)
			{
				Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null");
				this.isFinished = true;
				return;
			}
			if ((float)Find.TickManager.TicksGame >= this.occurTick)
			{
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.incident.category, (from x in Find.Maps
				where x.IsPlayerHome
				select x).RandomElement<Map>());
				if (!this.incident.Worker.TryExecute(parms))
				{
					this.isFinished = true;
					return;
				}
				if (this.repeat && (this.minDays > 0f || this.maxDays > this.minDays))
				{
					this.occurTick += 60000f * this.RandomInterval;
					return;
				}
				this.isFinished = true;
			}
		}

		// Token: 0x0400988A RID: 39050
		private const float IntervalMidpoint = 30f;

		// Token: 0x0400988B RID: 39051
		private const float IntervalDeviation = 15f;

		// Token: 0x0400988C RID: 39052
		private float intervalDays;

		// Token: 0x0400988D RID: 39053
		private float minDays;

		// Token: 0x0400988E RID: 39054
		private float maxDays;

		// Token: 0x0400988F RID: 39055
		private bool repeat;

		// Token: 0x04009890 RID: 39056
		private string intervalDaysBuffer;

		// Token: 0x04009891 RID: 39057
		private string minDaysBuffer;

		// Token: 0x04009892 RID: 39058
		private string maxDaysBuffer;

		// Token: 0x04009893 RID: 39059
		private float occurTick;

		// Token: 0x04009894 RID: 39060
		private bool isFinished;
	}
}
