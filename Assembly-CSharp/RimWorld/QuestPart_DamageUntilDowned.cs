using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1E RID: 7198
	public class QuestPart_DamageUntilDowned : QuestPart
	{
		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x0600A04E RID: 41038 RVA: 0x002E3404 File Offset: 0x002E1604
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A04F RID: 41039 RVA: 0x002E3414 File Offset: 0x002E1614
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						HealthUtility.DamageUntilDowned(this.pawns[i], this.allowBleedingWounds, null, null, null);
					}
				}
			}
		}

		// Token: 0x0600A050 RID: 41040 RVA: 0x002E3480 File Offset: 0x002E1680
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.allowBleedingWounds, "allowBleedingWounds", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A051 RID: 41041 RVA: 0x002E3500 File Offset: 0x002E1700
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x0600A052 RID: 41042 RVA: 0x002E3545 File Offset: 0x002E1745
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04006ED7 RID: 28375
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006ED8 RID: 28376
		public string inSignal;

		// Token: 0x04006ED9 RID: 28377
		public bool allowBleedingWounds = true;
	}
}
