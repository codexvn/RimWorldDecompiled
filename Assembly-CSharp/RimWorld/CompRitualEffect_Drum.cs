using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002802 RID: 10242
	public class CompRitualEffect_Drum : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x0600E000 RID: 57344 RVA: 0x001E5B28 File Offset: 0x001E3D28
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x17002231 RID: 8753
		// (get) Token: 0x0600E001 RID: 57345 RVA: 0x0041C52C File Offset: 0x0041A72C
		protected new CompProperties_RitualEffectDrum Props
		{
			get
			{
				return (CompProperties_RitualEffectDrum)this.props;
			}
		}

		// Token: 0x0600E002 RID: 57346 RVA: 0x0041C53C File Offset: 0x0041A73C
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			foreach (Thing thing in ritual.Map.listerBuldingOfDefInProximity.GetForCell(ritual.selectedTarget.Cell, (float)this.Props.maxDistance, ThingDefOf.Drum, null))
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && thing.GetRoom(RegionType.Set_All) == ritual.selectedTarget.Cell.GetRoom(ritual.Map) && building_MusicalInstrument.IsBeingPlayed)
				{
					for (int i = 0; i < this.Props.spawnCount; i++)
					{
						float num = (float)Rand.Sign;
						float num2 = (float)Rand.Sign;
						Vector3 b = new Vector3(num * Rand.Value * this.Props.maxOffset, 0f, num2 * Rand.Value * this.Props.maxOffset);
						base.SpawnFleck(ritual, new Vector3?(thing.Position.ToVector3Shifted() + b), null);
					}
				}
			}
			this.burstsDone++;
			this.lastSpawnTick = GenTicks.TicksGame;
		}
	}
}
