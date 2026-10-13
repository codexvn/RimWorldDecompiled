using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D82 RID: 11650
	public class CompObeliskTriggerInteractor : CompInteractable
	{
		// Token: 0x170029B6 RID: 10678
		// (get) Token: 0x06010600 RID: 67072 RVA: 0x004B97BC File Offset: 0x004B79BC
		private CompObelisk ObeliskComp
		{
			get
			{
				CompObelisk result;
				if ((result = this.obeliskComp) == null)
				{
					result = (this.obeliskComp = this.parent.GetComp<CompObelisk>());
				}
				return result;
			}
		}

		// Token: 0x170029B7 RID: 10679
		// (get) Token: 0x06010601 RID: 67073 RVA: 0x004B97E7 File Offset: 0x004B79E7
		public override string ExposeKey
		{
			get
			{
				return "Interactor";
			}
		}

		// Token: 0x06010602 RID: 67074 RVA: 0x004B97EE File Offset: 0x004B79EE
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (this.ObeliskComp.StudyLevel < 2 || this.ObeliskComp.ActivityComp.Deactivated || this.ObeliskComp.Activated)
			{
				return false;
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x06010603 RID: 67075 RVA: 0x004B982C File Offset: 0x004B7A2C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (2 > this.ObeliskComp.StudyLevel || this.ObeliskComp.ActivityComp.Deactivated || this.ObeliskComp.Activated)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06010604 RID: 67076 RVA: 0x004B983C File Offset: 0x004B7A3C
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (2 > this.ObeliskComp.StudyLevel || this.ObeliskComp.ActivityComp.Deactivated || this.ObeliskComp.Activated)
			{
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption in base.CompFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06010605 RID: 67077 RVA: 0x004B9853 File Offset: 0x004B7A53
		protected override void OnInteracted(Pawn caster)
		{
			if (this.obeliskComp.Activated)
			{
				return;
			}
			this.ObeliskComp.TriggerInteractionEffect(caster, true);
		}

		// Token: 0x0400AAD2 RID: 43730
		private CompObelisk obeliskComp;

		// Token: 0x0400AAD3 RID: 43731
		private const int StudyNeeded = 2;
	}
}
