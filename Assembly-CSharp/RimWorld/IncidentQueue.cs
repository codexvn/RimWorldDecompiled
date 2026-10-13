using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D5D RID: 7517
	public class IncidentQueue : IExposable
	{
		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x0600A87F RID: 43135 RVA: 0x0030393B File Offset: 0x00301B3B
		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x0600A880 RID: 43136 RVA: 0x00303948 File Offset: 0x00301B48
		public string DebugQueueReadout
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (QueuedIncident queuedIncident in this.queuedIncidents)
				{
					stringBuilder.AppendLine(queuedIncident.ToString() + " (in " + (queuedIncident.FireTick - Find.TickManager.TicksGame).ToString() + " ticks)");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600A881 RID: 43137 RVA: 0x003039D8 File Offset: 0x00301BD8
		public IEnumerator GetEnumerator()
		{
			foreach (QueuedIncident queuedIncident in this.queuedIncidents)
			{
				yield return queuedIncident;
			}
			List<QueuedIncident>.Enumerator enumerator = default(List<QueuedIncident>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600A882 RID: 43138 RVA: 0x003039E7 File Offset: 0x00301BE7
		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		// Token: 0x0600A883 RID: 43139 RVA: 0x003039F4 File Offset: 0x00301BF4
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600A884 RID: 43140 RVA: 0x00303A0C File Offset: 0x00301C0C
		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		// Token: 0x0600A885 RID: 43141 RVA: 0x00303A48 File Offset: 0x00301C48
		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null, int retryDurationTicks = 0)
		{
			QueuedIncident qi = new QueuedIncident(new FiringIncident(def, null, parms), fireTick, retryDurationTicks);
			this.Add(qi);
			return true;
		}

		// Token: 0x0600A886 RID: 43142 RVA: 0x00303A70 File Offset: 0x00301C70
		public void IncidentQueueTick()
		{
			for (int i = this.queuedIncidents.Count - 1; i >= 0; i--)
			{
				QueuedIncident queuedIncident = this.queuedIncidents[i];
				if (!queuedIncident.TriedToFire)
				{
					if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
					{
						bool flag = Find.Storyteller.TryFire(queuedIncident.FiringIncident, true);
						queuedIncident.Notify_TriedToFire();
						if (flag || queuedIncident.RetryDurationTicks == 0)
						{
							this.queuedIncidents.Remove(queuedIncident);
						}
					}
				}
				else if (queuedIncident.FireTick + queuedIncident.RetryDurationTicks <= Find.TickManager.TicksGame)
				{
					this.queuedIncidents.Remove(queuedIncident);
				}
				else if (Find.TickManager.TicksGame % 833 == Rand.RangeSeeded(0, 833, queuedIncident.FireTick))
				{
					bool flag2 = Find.Storyteller.TryFire(queuedIncident.FiringIncident, true);
					queuedIncident.Notify_TriedToFire();
					if (flag2)
					{
						this.queuedIncidents.Remove(queuedIncident);
					}
				}
			}
		}

		// Token: 0x0600A887 RID: 43143 RVA: 0x00303B6C File Offset: 0x00301D6C
		public void Notify_MapRemoved(Map map)
		{
			this.queuedIncidents.RemoveAll((QueuedIncident x) => x.FiringIncident.parms.target == map);
		}

		// Token: 0x040074B2 RID: 29874
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();
	}
}
