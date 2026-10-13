using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CFE RID: 7422
	public class QuestPart_Incident : QuestPart
	{
		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x0600A5C3 RID: 42435 RVA: 0x002F751B File Offset: 0x002F571B
		// (set) Token: 0x0600A5C4 RID: 42436 RVA: 0x002F7523 File Offset: 0x002F5723
		public MapParent MapParent
		{
			get
			{
				return this.mapParent;
			}
			set
			{
				this.mapParent = value;
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x0600A5C5 RID: 42437 RVA: 0x002F752C File Offset: 0x002F572C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A5C6 RID: 42438 RVA: 0x002F753C File Offset: 0x002F573C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.incidentParms != null)
			{
				if (!this.incidentParms.forced)
				{
					string str = "QuestPart incident should always be forced but it's not. incident=";
					IncidentDef incidentDef = this.incident;
					Log.Error(str + ((incidentDef != null) ? incidentDef.ToString() : null));
					this.incidentParms.forced = true;
				}
				this.incidentParms.quest = this.quest;
				if (this.incidentParms.target == null && (this.mapParent == null || !this.mapParent.HasMap))
				{
					Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
					this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
				}
				if (this.mapParent != null)
				{
					if (!this.mapParent.HasMap)
					{
						return;
					}
					this.incidentParms.target = this.mapParent.Map;
					if (this.incident.Worker.CanFireNow(this.incidentParms))
					{
						this.incident.Worker.TryExecute(this.incidentParms);
					}
					this.incidentParms.target = null;
					return;
				}
				else if (this.incidentParms.target != null && this.incident.Worker.CanFireNow(this.incidentParms))
				{
					this.incident.Worker.TryExecute(this.incidentParms);
				}
			}
		}

		// Token: 0x0600A5C7 RID: 42439 RVA: 0x002F769C File Offset: 0x002F589C
		public void SetIncidentParmsAndRemoveTarget(IncidentParms value)
		{
			this.incidentParms = value;
			Map map = this.incidentParms.target as Map;
			if (map != null)
			{
				this.mapParent = map.Parent;
				this.incidentParms.target = null;
				return;
			}
			this.mapParent = null;
		}

		// Token: 0x0600A5C8 RID: 42440 RVA: 0x002F76E4 File Offset: 0x002F58E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x0600A5C9 RID: 42441 RVA: 0x002F7740 File Offset: 0x002F5940
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.incident = IncidentDefOf.RaidEnemy;
				this.SetIncidentParmsAndRemoveTarget(new IncidentParms
				{
					target = Find.RandomPlayerHomeMap,
					points = 500f
				});
			}
		}

		// Token: 0x04007285 RID: 29317
		public string inSignal;

		// Token: 0x04007286 RID: 29318
		public IncidentDef incident;

		// Token: 0x04007287 RID: 29319
		private IncidentParms incidentParms;

		// Token: 0x04007288 RID: 29320
		private MapParent mapParent;
	}
}
