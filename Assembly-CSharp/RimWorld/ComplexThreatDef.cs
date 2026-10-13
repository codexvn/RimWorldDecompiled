using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A65 RID: 6757
	public class ComplexThreatDef : Def
	{
		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x060097C7 RID: 38855 RVA: 0x002C4FDB File Offset: 0x002C31DB
		public ComplexThreatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (ComplexThreatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060097C8 RID: 38856 RVA: 0x002C500D File Offset: 0x002C320D
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.delayChance > 0f && this.delayTickOptions == null)
			{
				yield return "Chance to have a delayed threat is > 0 but no signal delay tick options are set.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400652A RID: 25898
		public Type workerClass = typeof(ComplexThreatWorker);

		// Token: 0x0400652B RID: 25899
		public FactionDef faction;

		// Token: 0x0400652C RID: 25900
		public float postSpawnPassiveThreatFactor = 1f;

		// Token: 0x0400652D RID: 25901
		public int minPoints;

		// Token: 0x0400652E RID: 25902
		public float spawnInOtherRoomChance;

		// Token: 0x0400652F RID: 25903
		public bool allowPassive = true;

		// Token: 0x04006530 RID: 25904
		public bool fallbackToRoomEnteredTrigger = true;

		// Token: 0x04006531 RID: 25905
		public float delayChance;

		// Token: 0x04006532 RID: 25906
		public List<int> delayTickOptions;

		// Token: 0x04006533 RID: 25907
		public SimpleCurve threatFactorOverDelayTicksCurve;

		// Token: 0x04006534 RID: 25908
		public SignalActionAmbushType signalActionAmbushType;

		// Token: 0x04006535 RID: 25909
		public bool spawnAroundComplex;

		// Token: 0x04006536 RID: 25910
		public bool useDropPods;

		// Token: 0x04006537 RID: 25911
		[Unsaved(false)]
		private ComplexThreatWorker workerInt;
	}
}
