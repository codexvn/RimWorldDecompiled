using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCA RID: 7370
	public abstract class QuestPart : IExposable, ILoadReferenceable
	{
		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x0600A42C RID: 42028 RVA: 0x002F115A File Offset: 0x002EF35A
		public virtual string DescriptionPart { get; }

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x0600A42D RID: 42029 RVA: 0x002F1162 File Offset: 0x002EF362
		public int Index
		{
			get
			{
				return this.quest.PartsListForReading.IndexOf(this);
			}
		}

		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x0600A42E RID: 42030 RVA: 0x002EC854 File Offset: 0x002EAA54
		public virtual IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return Enumerable.Empty<GlobalTargetInfo>();
			}
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x0600A42F RID: 42031 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string QuestSelectTargetsLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x0600A430 RID: 42032 RVA: 0x002EC854 File Offset: 0x002EAA54
		public virtual IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				return Enumerable.Empty<GlobalTargetInfo>();
			}
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x0600A431 RID: 42033 RVA: 0x002F1175 File Offset: 0x002EF375
		public virtual IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				return Enumerable.Empty<Faction>();
			}
		}

		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x0600A432 RID: 42034 RVA: 0x002F117C File Offset: 0x002EF37C
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return Enumerable.Empty<Dialog_InfoCard.Hyperlink>();
			}
		}

		// Token: 0x17001949 RID: 6473
		// (get) Token: 0x0600A433 RID: 42035 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IncreasesPopulation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x0600A434 RID: 42036 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool RequiresAccepter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x0600A435 RID: 42037 RVA: 0x002F1183 File Offset: 0x002EF383
		public virtual bool PreventsAutoAccept
		{
			get
			{
				return this.RequiresAccepter;
			}
		}

		// Token: 0x0600A436 RID: 42038 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool QuestPartReserves(Pawn p)
		{
			return false;
		}

		// Token: 0x0600A437 RID: 42039 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool QuestPartReserves(Faction f)
		{
			return false;
		}

		// Token: 0x0600A438 RID: 42040 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool QuestPartReserves(TransportShip ship)
		{
			return false;
		}

		// Token: 0x0600A439 RID: 42041 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600A43A RID: 42042 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void AssignDebugData()
		{
		}

		// Token: 0x0600A43B RID: 42043 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreQuestAccept()
		{
		}

		// Token: 0x0600A43C RID: 42044 RVA: 0x002F118B File Offset: 0x002EF38B
		public virtual void ExposeData()
		{
			Scribe_Values.Look<QuestPart.SignalListenMode>(ref this.signalListenMode, "signalListenMode", QuestPart.SignalListenMode.OngoingOnly, false);
			Scribe_Values.Look<string>(ref this.debugLabel, "debugLabel", null, false);
		}

		// Token: 0x0600A43D RID: 42045 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_QuestSignalReceived(Signal signal)
		{
		}

		// Token: 0x0600A43E RID: 42046 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
		}

		// Token: 0x0600A43F RID: 42047 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
		}

		// Token: 0x0600A440 RID: 42048 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
		}

		// Token: 0x0600A441 RID: 42049 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
		}

		// Token: 0x0600A442 RID: 42050 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x0600A443 RID: 42051 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnDiscarded(Pawn pawn)
		{
		}

		// Token: 0x0600A444 RID: 42052 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PreCleanup()
		{
		}

		// Token: 0x0600A445 RID: 42053 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostQuestAdded()
		{
		}

		// Token: 0x0600A446 RID: 42054 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ReplacePawnReferences(Pawn replace, Pawn with)
		{
		}

		// Token: 0x0600A447 RID: 42055 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
		}

		// Token: 0x0600A448 RID: 42056 RVA: 0x002F11B4 File Offset: 0x002EF3B4
		public override string ToString()
		{
			string format = "{0} (quest.id={1}, index={2}";
			object name = base.GetType().Name;
			Quest quest = this.quest;
			string str = string.Format(format, name, (quest != null) ? quest.id : -1, this.Index);
			if (!this.debugLabel.NullOrEmpty())
			{
				str = str + ", debugLabel=" + this.debugLabel;
			}
			return str + ")";
		}

		// Token: 0x0600A449 RID: 42057 RVA: 0x002F1228 File Offset: 0x002EF428
		public string GetUniqueLoadID()
		{
			return "QuestPart_" + this.quest.id.ToString() + "_" + this.Index.ToString();
		}

		// Token: 0x04007164 RID: 29028
		public Quest quest;

		// Token: 0x04007165 RID: 29029
		public QuestPart.SignalListenMode signalListenMode;

		// Token: 0x04007166 RID: 29030
		public string debugLabel;

		// Token: 0x02001CCB RID: 7371
		public enum SignalListenMode
		{
			// Token: 0x04007169 RID: 29033
			OngoingOnly,
			// Token: 0x0400716A RID: 29034
			NotYetAcceptedOnly,
			// Token: 0x0400716B RID: 29035
			OngoingOrNotYetAccepted,
			// Token: 0x0400716C RID: 29036
			HistoricalOnly,
			// Token: 0x0400716D RID: 29037
			Always
		}
	}
}
