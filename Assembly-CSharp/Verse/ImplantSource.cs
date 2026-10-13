using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200075A RID: 1882
	public class ImplantSource : IExposable
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x000ED093 File Offset: 0x000EB293
		public bool WasPawn
		{
			get
			{
				return this.sourcePawn != null;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002E12 RID: 11794 RVA: 0x000ED09E File Offset: 0x000EB29E
		public Pawn Pawn
		{
			get
			{
				return this.sourcePawn;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002E13 RID: 11795 RVA: 0x000ED0A6 File Offset: 0x000EB2A6
		public int Biosignature
		{
			get
			{
				return this.biosignature;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002E14 RID: 11796 RVA: 0x000ED0B0 File Offset: 0x000EB2B0
		public string BiosignatureName
		{
			get
			{
				string result;
				if ((result = this.cachedName) == null)
				{
					result = (this.cachedName = AnomalyUtility.GetBiosignatureName(this.biosignature));
				}
				return result;
			}
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000ED0DB File Offset: 0x000EB2DB
		public ImplantSource()
		{
			this.biosignature = Rand.Int;
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000ED0F0 File Offset: 0x000EB2F0
		public ImplantSource(Pawn pawn, string descKey = null, string descResolved = null)
		{
			this.sourcePawn = pawn;
			this.descKey = descKey;
			this.descResolved = descResolved;
			Hediff_MetalhorrorImplant hediff_MetalhorrorImplant;
			if (pawn != null && pawn.health.hediffSet.TryGetHediff<Hediff_MetalhorrorImplant>(out hediff_MetalhorrorImplant))
			{
				this.biosignature = hediff_MetalhorrorImplant.Biosignature;
				return;
			}
			this.biosignature = Rand.Int;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000ED148 File Offset: 0x000EB348
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.descKey, "descKeyDesc", null, false);
			Scribe_Values.Look<string>(ref this.descResolved, "descResolved", null, false);
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
			Scribe_References.Look<Pawn>(ref this.sourcePawn, "sourcePawn", true);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000ED19C File Offset: 0x000EB39C
		public void DebugSetBiosignature(int newBiosignature)
		{
			this.biosignature = newBiosignature;
			this.cachedName = null;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000ED1AC File Offset: 0x000EB3AC
		public string GetSourceDesc()
		{
			if (!string.IsNullOrEmpty(this.descResolved))
			{
				return this.descResolved;
			}
			if (string.IsNullOrEmpty(this.descKey))
			{
				return "ImplantSourceUnknown".Translate();
			}
			if (this.sourcePawn != null)
			{
				return this.descKey.Translate(this.sourcePawn.Named("SOURCE"));
			}
			return this.descKey.Translate();
		}

		// Token: 0x0400244A RID: 9290
		private Pawn sourcePawn;

		// Token: 0x0400244B RID: 9291
		private string descKey;

		// Token: 0x0400244C RID: 9292
		private string descResolved;

		// Token: 0x0400244D RID: 9293
		private int biosignature;

		// Token: 0x0400244E RID: 9294
		private string cachedName;
	}
}
