using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E2B RID: 11819
	public class CompSpawnerFilthOnTakeDamage : ThingComp
	{
		// Token: 0x17002AA3 RID: 10915
		// (get) Token: 0x06010A46 RID: 68166 RVA: 0x004CC99E File Offset: 0x004CAB9E
		public CompProperties_SpawnFilthOnTakeDamage Props
		{
			get
			{
				return (CompProperties_SpawnFilthOnTakeDamage)this.props;
			}
		}

		// Token: 0x06010A47 RID: 68167 RVA: 0x004CC9AC File Offset: 0x004CABAC
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.parent.Spawned && Rand.Chance(this.Props.chance))
			{
				int randomInRange = this.Props.filthCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Filth filth;
					FilthMaker.TryMakeFilth(this.parent.OccupiedRect().ExpandedBy(1).EdgeCells.RandomElement<IntVec3>(), this.parent.Map, this.Props.filthDef, out filth, this.parent.LabelNoParenthesis, FilthSourceFlags.None, false);
				}
			}
		}
	}
}
