using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EB3 RID: 11955
	public class CompProperties_TurretGun : CompProperties
	{
		// Token: 0x06010DCB RID: 69067 RVA: 0x004DB568 File Offset: 0x004D9768
		public CompProperties_TurretGun()
		{
			this.compClass = typeof(CompTurretGun);
		}

		// Token: 0x06010DCC RID: 69068 RVA: 0x004DB587 File Offset: 0x004D9787
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>())
			{
				foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.renderNodeProperties)
				{
					if (!typeof(PawnRenderNode_TurretGun).IsAssignableFrom(pawnRenderNodeProperties.nodeClass))
					{
						yield return "contains nodeClass which is not PawnRenderNode_TurretGun or subclass thereof.";
					}
				}
				List<PawnRenderNodeProperties>.Enumerator enumerator = default(List<PawnRenderNodeProperties>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400AF8B RID: 44939
		public ThingDef turretDef;

		// Token: 0x0400AF8C RID: 44940
		public float angleOffset;

		// Token: 0x0400AF8D RID: 44941
		public bool autoAttack = true;

		// Token: 0x0400AF8E RID: 44942
		public List<PawnRenderNodeProperties> renderNodeProperties;
	}
}
