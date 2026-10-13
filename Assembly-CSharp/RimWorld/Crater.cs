using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002AFE RID: 11006
	public class Crater : Building
	{
		// Token: 0x1700266D RID: 9837
		// (get) Token: 0x0600F5B7 RID: 62903 RVA: 0x00475E20 File Offset: 0x00474020
		protected virtual EffecterDef FilledInEffecter
		{
			get
			{
				return EffecterDefOf.ImpactSmallDustCloud;
			}
		}

		// Token: 0x1700266E RID: 9838
		// (get) Token: 0x0600F5B8 RID: 62904 RVA: 0x00475E27 File Offset: 0x00474027
		protected virtual SoundDef FilledInSound
		{
			get
			{
				return SoundDefOf.Crater_FilledIn;
			}
		}

		// Token: 0x0600F5B9 RID: 62905 RVA: 0x00475E30 File Offset: 0x00474030
		public virtual void FillIn()
		{
			EffecterDef filledInEffecter = this.FilledInEffecter;
			if (filledInEffecter != null)
			{
				filledInEffecter.Spawn(base.Position, base.Map, 1f).Cleanup();
			}
			SoundDef filledInSound = this.FilledInSound;
			if (filledInSound != null)
			{
				filledInSound.PlayOneShot(this);
			}
			this.Destroy(DestroyMode.Deconstruct);
		}

		// Token: 0x0600F5BA RID: 62906 RVA: 0x00475E82 File Offset: 0x00474082
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					action = new Action(this.FillIn),
					defaultLabel = "DEV: Fill in"
				};
			}
			yield break;
			yield break;
		}
	}
}
