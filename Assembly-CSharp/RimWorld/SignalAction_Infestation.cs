using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ABE RID: 10942
	public class SignalAction_Infestation : SignalAction_Delay
	{
		// Token: 0x1700261F RID: 9759
		// (get) Token: 0x0600F3CF RID: 62415 RVA: 0x0046D49E File Offset: 0x0046B69E
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null)
				{
					this.cachedAlert = new Alert_InfestationDelay(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x0600F3D0 RID: 62416 RVA: 0x0046D4BC File Offset: 0x0046B6BC
		protected override void Complete()
		{
			base.Complete();
			Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, base.Map, this.spawnAnywhereIfNoGoodCell, this.ignoreRoofedRequirement, null, this.overrideLoc, this.insectsPoints);
			if (thing != null && this.sendStandardLetter)
			{
				IntVec3 cell = this.overrideLoc ?? thing.Position;
				Find.LetterStack.ReceiveLetter(IncidentDefOf.Infestation.letterLabel, IncidentDefOf.Infestation.letterText, IncidentDefOf.Infestation.letterDef, new TargetInfo(cell, base.Map, false), null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600F3D1 RID: 62417 RVA: 0x0046D570 File Offset: 0x0046B770
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hivesCount, "hivesCount", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnAnywhereIfNoGoodCell, "spawnAnywhereIfNoGoodCell", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreRoofedRequirement, "ignoreRoofedRequirement", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.overrideLoc, "overrideLoc", null, false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", false, false);
			Scribe_Values.Look<float?>(ref this.insectsPoints, "insectsPoints", null, false);
		}

		// Token: 0x0400A09A RID: 41114
		public int hivesCount = 1;

		// Token: 0x0400A09B RID: 41115
		public float? insectsPoints;

		// Token: 0x0400A09C RID: 41116
		public bool spawnAnywhereIfNoGoodCell;

		// Token: 0x0400A09D RID: 41117
		public bool ignoreRoofedRequirement;

		// Token: 0x0400A09E RID: 41118
		public IntVec3? overrideLoc;

		// Token: 0x0400A09F RID: 41119
		public bool sendStandardLetter;

		// Token: 0x0400A0A0 RID: 41120
		private Alert_ActionDelay cachedAlert;
	}
}
