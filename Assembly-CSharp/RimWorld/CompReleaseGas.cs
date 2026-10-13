using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003864 RID: 14436
	public class CompReleaseGas : ThingComp
	{
		// Token: 0x170032F7 RID: 13047
		// (get) Token: 0x06013F4D RID: 81741 RVA: 0x005EBD9F File Offset: 0x005E9F9F
		private CompProperties_ReleaseGas Props
		{
			get
			{
				return (CompProperties_ReleaseGas)this.props;
			}
		}

		// Token: 0x170032F8 RID: 13048
		// (get) Token: 0x06013F4E RID: 81742 RVA: 0x005EBDAC File Offset: 0x005E9FAC
		private int TotalGas
		{
			get
			{
				return Mathf.CeilToInt(this.Props.cellsToFill * 255f);
			}
		}

		// Token: 0x170032F9 RID: 13049
		// (get) Token: 0x06013F4F RID: 81743 RVA: 0x005EBDC4 File Offset: 0x005E9FC4
		private float GasReleasedPerTick
		{
			get
			{
				return (float)this.TotalGas / this.Props.durationSeconds / 60f;
			}
		}

		// Token: 0x170032FA RID: 13050
		// (get) Token: 0x06013F50 RID: 81744 RVA: 0x005EBDE0 File Offset: 0x005E9FE0
		private Thing EffecterSourceThing
		{
			get
			{
				ThingWithComps result = this.parent;
				if (!this.parent.Spawned)
				{
					IThingHolder parentHolder = this.parent.ParentHolder;
					if (parentHolder != null)
					{
						Pawn_ApparelTracker pawn_ApparelTracker = parentHolder as Pawn_ApparelTracker;
						if (pawn_ApparelTracker != null)
						{
							result = pawn_ApparelTracker.pawn;
						}
						else
						{
							Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
							if (pawn_CarryTracker != null)
							{
								result = pawn_CarryTracker.pawn;
							}
							else
							{
								Pawn_EquipmentTracker pawn_EquipmentTracker = parentHolder as Pawn_EquipmentTracker;
								if (pawn_EquipmentTracker != null)
								{
									result = pawn_EquipmentTracker.pawn;
								}
							}
						}
					}
				}
				return result;
			}
		}

		// Token: 0x06013F51 RID: 81745 RVA: 0x005EBE4B File Offset: 0x005EA04B
		public override void PostPostMake()
		{
			this.remainingGas = this.TotalGas;
		}

		// Token: 0x06013F52 RID: 81746 RVA: 0x005EBE59 File Offset: 0x005EA059
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x06013F53 RID: 81747 RVA: 0x005EBE73 File Offset: 0x005EA073
		public void StartRelease()
		{
			this.started = true;
		}

		// Token: 0x06013F54 RID: 81748 RVA: 0x005EBE7C File Offset: 0x005EA07C
		public override void Notify_WearerDied()
		{
			this.started = false;
			this.remainingGas = this.TotalGas;
		}

		// Token: 0x06013F55 RID: 81749 RVA: 0x005EBE94 File Offset: 0x005EA094
		public override void CompTick()
		{
			if (!this.started || this.parent.MapHeld == null)
			{
				return;
			}
			if (this.Props.effecterReleasing != null)
			{
				if (this.effecter == null)
				{
					this.effecter = this.Props.effecterReleasing.Spawn(this.EffecterSourceThing, TargetInfo.Invalid, 1f);
				}
				this.effecter.EffectTick(this.EffecterSourceThing, TargetInfo.Invalid);
			}
			if (this.remainingGas > 0 && this.parent.IsHashIntervalTick(30))
			{
				int num = Mathf.Min(this.remainingGas, Mathf.RoundToInt(this.GasReleasedPerTick * 30f));
				GasUtility.AddGas(this.parent.PositionHeld, this.parent.MapHeld, this.Props.gasType, num);
				this.remainingGas -= num;
				if (this.remainingGas <= 0)
				{
					this.started = false;
					this.remainingGas = this.TotalGas;
				}
			}
		}

		// Token: 0x06013F56 RID: 81750 RVA: 0x005EBF98 File Offset: 0x005EA198
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.remainingGas, "remainingGas", 0, false);
			Scribe_Values.Look<bool>(ref this.started, "started", false, false);
		}

		// Token: 0x0400DC5F RID: 56415
		private int remainingGas;

		// Token: 0x0400DC60 RID: 56416
		private bool started;

		// Token: 0x0400DC61 RID: 56417
		[Unsaved(false)]
		private Effecter effecter;

		// Token: 0x0400DC62 RID: 56418
		private const int ReleaseGasInterval = 30;
	}
}
