using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EF8 RID: 12024
	public class CompGrayStatueGas : CompGrayStatue
	{
		// Token: 0x17002BD6 RID: 11222
		// (get) Token: 0x06010F9C RID: 69532 RVA: 0x004E2B6A File Offset: 0x004E0D6A
		private CompProperties_GrayStatueGas Props
		{
			get
			{
				return (CompProperties_GrayStatueGas)this.props;
			}
		}

		// Token: 0x17002BD7 RID: 11223
		// (get) Token: 0x06010F9D RID: 69533 RVA: 0x004E2B77 File Offset: 0x004E0D77
		private int GasAmount
		{
			get
			{
				return Mathf.Min(Mathf.CeilToInt((float)this.parent.GetRoom(RegionType.Set_All).CellCount * 0.15f * 255f), 12750);
			}
		}

		// Token: 0x06010F9E RID: 69534 RVA: 0x004E2BA8 File Offset: 0x004E0DA8
		protected override void Trigger(Pawn target)
		{
			if (this.Props.gas == GasType.DeadlifeDust)
			{
				GasUtility.AddDeadifeGas(this.parent.Position, this.parent.Map, Faction.OfEntities, this.GasAmount);
				return;
			}
			GasUtility.AddGas(this.parent.Position, this.parent.Map, this.Props.gas, this.GasAmount);
		}

		// Token: 0x06010F9F RID: 69535 RVA: 0x004E2C17 File Offset: 0x004E0E17
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Test gas spread",
				action = delegate()
				{
					this.parent.Map.gasGrid.EstimateGasDiffusion(this.parent.Position, GasType.DeadlifeDust, this.GasAmount, delegate(IntVec3 c)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.PowerConduit, null), c, this.parent.Map, WipeMode.Vanish);
					});
				}
			};
			yield break;
		}

		// Token: 0x0400B090 RID: 45200
		private const float RoomCellGasFactor = 0.15f;

		// Token: 0x0400B091 RID: 45201
		private const int MaxGas = 12750;
	}
}
