using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001986 RID: 6534
	public class CompProperties_Stunnable : CompProperties
	{
		// Token: 0x0600943B RID: 37947 RVA: 0x002B8C37 File Offset: 0x002B6E37
		public CompProperties_Stunnable()
		{
			this.compClass = typeof(CompStunnable);
		}

		// Token: 0x0600943C RID: 37948 RVA: 0x002B8C65 File Offset: 0x002B6E65
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.affectedDamageDefs.Count == 0)
			{
				yield return "CompProperties_Stunnable requires at least one affectedDamageDef";
			}
			foreach (DamageDef damageDef in this.affectedDamageDefs)
			{
				if (!damageDef.causeStun)
				{
					yield return "CompProperties_Stunnable: " + damageDef.defName + " does not cause stun";
				}
			}
			List<DamageDef>.Enumerator enumerator2 = default(List<DamageDef>.Enumerator);
			foreach (DamageDef damageDef2 in this.adaptableDamageDefs)
			{
				if (damageDef2.stunAdaptationTicks <= 0)
				{
					yield return "CompProperties_Stunnable: " + damageDef2.defName + " does not have stunAdaptationTicks set";
				}
			}
			enumerator2 = default(List<DamageDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04005FC0 RID: 24512
		public List<DamageDef> affectedDamageDefs = new List<DamageDef>();

		// Token: 0x04005FC1 RID: 24513
		public List<DamageDef> adaptableDamageDefs = new List<DamageDef>();

		// Token: 0x04005FC2 RID: 24514
		public bool useLargeEMPEffecter;

		// Token: 0x04005FC3 RID: 24515
		public Vector3? empEffecterDimensions;

		// Token: 0x04005FC4 RID: 24516
		public Vector3? empEffecterOffset;

		// Token: 0x04005FC5 RID: 24517
		public float? empChancePerTick;
	}
}
