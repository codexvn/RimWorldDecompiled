using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC3 RID: 7363
	public class QuestPart_TrackWhenExitMentalState : QuestPart
	{
		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x0600A415 RID: 42005 RVA: 0x002F0CE8 File Offset: 0x002EEEE8
		private List<Pawn> TrackedPawns
		{
			get
			{
				if (this.cachedPawns == null)
				{
					this.cachedPawns = (from p in this.mapParent.Map.mapPawns.AllPawnsSpawned
					where p.InMentalState && p.MentalStateDef == this.mentalStateDef && !p.questTags.NullOrEmpty<string>() && p.questTags.Contains(this.tag)
					select p).ToList<Pawn>();
				}
				return this.cachedPawns;
			}
		}

		// Token: 0x0600A416 RID: 42006 RVA: 0x002F0D34 File Offset: 0x002EEF34
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				this.mapParent = Find.AnyPlayerHomeMap.Parent;
			}
			if (!this.signalSent && this.inSignals.Contains(signal.tag))
			{
				Pawn pawn = this.TrackedPawns.Find((Pawn p) => p == signal.args.GetArg<Pawn>("SUBJECT"));
				if (pawn != null)
				{
					this.cachedPawns.Remove(pawn);
				}
				if (!this.cachedPawns.Any<Pawn>())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
					this.signalSent = true;
				}
			}
		}

		// Token: 0x0600A417 RID: 42007 RVA: 0x002F0DF4 File Offset: 0x002EEFF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Defs.Look<MentalStateDef>(ref this.mentalStateDef, "mentalStateDef");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.signalSent, "signalSent", false, false);
		}

		// Token: 0x04007155 RID: 29013
		public string tag;

		// Token: 0x04007156 RID: 29014
		public List<string> inSignals;

		// Token: 0x04007157 RID: 29015
		public string outSignal;

		// Token: 0x04007158 RID: 29016
		public MapParent mapParent;

		// Token: 0x04007159 RID: 29017
		public MentalStateDef mentalStateDef;

		// Token: 0x0400715A RID: 29018
		private bool signalSent;

		// Token: 0x0400715B RID: 29019
		[Unsaved(false)]
		private List<Pawn> cachedPawns;
	}
}
