using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027F8 RID: 10232
	public class CompRitualEffect_StaticAreaRandomMote : CompRitualEffect_Constant
	{
		// Token: 0x1700222C RID: 8748
		// (get) Token: 0x0600DFE8 RID: 57320 RVA: 0x0041BF56 File Offset: 0x0041A156
		protected CompProperties_RitualEffectStaticAreaRandomMote Props
		{
			get
			{
				return (CompProperties_RitualEffectStaticAreaRandomMote)this.props;
			}
		}

		// Token: 0x0600DFE9 RID: 57321 RVA: 0x001E5B28 File Offset: 0x001E3D28
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x1700222D RID: 8749
		// (get) Token: 0x0600DFEA RID: 57322 RVA: 0x0041BF63 File Offset: 0x0041A163
		protected override ThingDef MoteDef
		{
			get
			{
				return this.Props.moteDefs.RandomElement<ThingDef>();
			}
		}

		// Token: 0x0600DFEB RID: 57323 RVA: 0x0041BF78 File Offset: 0x0041A178
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			CellRect cellRect = CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map);
			List<IntVec3> list = new List<IntVec3>();
			for (int i = 0; i < this.Props.spawnCount; i++)
			{
				IntVec3 pos = IntVec3.Invalid;
				Predicate<IntVec3> <>9__0;
				for (int j = 0; j < 15; j++)
				{
					pos = cellRect.RandomCell;
					List<IntVec3> list2 = list;
					Predicate<IntVec3> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((IntVec3 c) => c.InHorDistOf(pos, this.Props.minDist)));
					}
					if (!list2.Any(predicate))
					{
						break;
					}
				}
				if (pos.IsValid)
				{
					Mote mote = this.SpawnMote(ritual, new Vector3?(pos.ToVector3Shifted() + this.Props.offset));
					if (mote != null)
					{
						parent.AddMoteToMaintain(mote);
					}
					list.Add(pos);
				}
			}
		}
	}
}
