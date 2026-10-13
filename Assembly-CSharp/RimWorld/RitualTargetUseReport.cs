using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026E0 RID: 9952
	public struct RitualTargetUseReport
	{
		// Token: 0x17002167 RID: 8551
		// (get) Token: 0x0600DA21 RID: 55841 RVA: 0x004087F9 File Offset: 0x004069F9
		public bool ShouldShowGizmo
		{
			get
			{
				return this.canUse || !this.failReason.NullOrEmpty();
			}
		}

		// Token: 0x0600DA22 RID: 55842 RVA: 0x00408814 File Offset: 0x00406A14
		public static implicit operator RitualTargetUseReport(bool canUse)
		{
			return new RitualTargetUseReport
			{
				canUse = canUse,
				failReason = null
			};
		}

		// Token: 0x0600DA23 RID: 55843 RVA: 0x0040883C File Offset: 0x00406A3C
		public static implicit operator RitualTargetUseReport(string failReason)
		{
			return new RitualTargetUseReport
			{
				canUse = false,
				failReason = failReason
			};
		}

		// Token: 0x0600DA24 RID: 55844 RVA: 0x00408864 File Offset: 0x00406A64
		public static implicit operator RitualTargetUseReport(TaggedString failReason)
		{
			return new RitualTargetUseReport
			{
				canUse = false,
				failReason = failReason
			};
		}

		// Token: 0x040093B0 RID: 37808
		public bool canUse;

		// Token: 0x040093B1 RID: 37809
		public string failReason;
	}
}
