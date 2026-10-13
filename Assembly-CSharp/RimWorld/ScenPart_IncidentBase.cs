using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028BA RID: 10426
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x170022AE RID: 8878
		// (get) Token: 0x0600E415 RID: 58389 RVA: 0x0042A584 File Offset: 0x00428784
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x170022AF RID: 8879
		// (get) Token: 0x0600E416 RID: 58390
		protected abstract string IncidentTag { get; }

		// Token: 0x0600E417 RID: 58391 RVA: 0x0042A58C File Offset: 0x0042878C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.incident == null)
			{
				this.incident = this.RandomizableIncidents().FirstOrDefault<IncidentDef>();
				Log.Error("ScenPart has null incident after loading. Changing to " + this.incident.ToStringSafe<IncidentDef>());
			}
		}

		// Token: 0x0600E418 RID: 58392 RVA: 0x0042A5EA File Offset: 0x004287EA
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.incident != null) ? this.incident.GetHashCode() : 0);
		}

		// Token: 0x0600E419 RID: 58393 RVA: 0x0042A60C File Offset: 0x0042880C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x0600E41A RID: 58394 RVA: 0x0042A630 File Offset: 0x00428830
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x0600E41B RID: 58395 RVA: 0x0042A665 File Offset: 0x00428865
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		// Token: 0x0600E41C RID: 58396 RVA: 0x0042A67C File Offset: 0x0042887C
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x0600E41D RID: 58397 RVA: 0x0042A690 File Offset: 0x00428890
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x0600E41E RID: 58398 RVA: 0x0042A6B8 File Offset: 0x004288B8
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x0600E41F RID: 58399 RVA: 0x0042A6E2 File Offset: 0x004288E2
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x0600E420 RID: 58400 RVA: 0x0042A6EC File Offset: 0x004288EC
		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<IncidentDef>(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate()
				{
					this.incident = id;
				});
			}
		}

		// Token: 0x040098B0 RID: 39088
		protected IncidentDef incident;
	}
}
