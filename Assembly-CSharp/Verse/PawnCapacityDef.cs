using System;

namespace Verse
{
	// Token: 0x020002B4 RID: 692
	public class PawnCapacityDef : Def
	{
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0004FAAD File Offset: 0x0004DCAD
		public PawnCapacityWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnCapacityWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0004FAD4 File Offset: 0x0004DCD4
		public string GetLabelFor(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				return this.label;
			}
			if (pawn.RaceProps.IsFlesh && !this.labelAnimals.NullOrEmpty())
			{
				return this.labelAnimals;
			}
			if (pawn.RaceProps.IsAnomalyEntity && !this.labelAnomalyEntity.NullOrEmpty())
			{
				return this.labelAnomalyEntity;
			}
			if (pawn.RaceProps.IsMechanoid && !this.labelMechanoids.NullOrEmpty())
			{
				return this.labelMechanoids;
			}
			if (pawn.RaceProps.IsDrone && !this.labelDrones.NullOrEmpty())
			{
				return this.labelDrones;
			}
			return this.label;
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004FB7F File Offset: 0x0004DD7F
		public string GetLabelFor()
		{
			return this.label;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0004FB88 File Offset: 0x0004DD88
		public bool CanShowOnPawn(Pawn p)
		{
			if (p.def.race.Humanlike)
			{
				return this.showOnHumanlikes;
			}
			if (p.def.race.Animal)
			{
				return this.showOnAnimals;
			}
			if (p.def.race.IsAnomalyEntity)
			{
				return this.showOnAnomalyEntities;
			}
			if (p.def.race.IsDrone)
			{
				return this.showOnDrones;
			}
			return this.showOnMechanoids;
		}

		// Token: 0x04000E6A RID: 3690
		public int listOrder;

		// Token: 0x04000E6B RID: 3691
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x04000E6C RID: 3692
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x04000E6D RID: 3693
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x04000E6E RID: 3694
		[MustTranslate]
		public string labelAnomalyEntity = "";

		// Token: 0x04000E6F RID: 3695
		[MustTranslate]
		public string labelDrones = "";

		// Token: 0x04000E70 RID: 3696
		public bool showOnHumanlikes = true;

		// Token: 0x04000E71 RID: 3697
		public bool showOnAnimals = true;

		// Token: 0x04000E72 RID: 3698
		public bool showOnMechanoids = true;

		// Token: 0x04000E73 RID: 3699
		public bool showOnAnomalyEntities = true;

		// Token: 0x04000E74 RID: 3700
		public bool showOnDrones = true;

		// Token: 0x04000E75 RID: 3701
		public bool lethalFlesh;

		// Token: 0x04000E76 RID: 3702
		public bool lethalMechanoids;

		// Token: 0x04000E77 RID: 3703
		public float minForCapable;

		// Token: 0x04000E78 RID: 3704
		public float minValue;

		// Token: 0x04000E79 RID: 3705
		public bool zeroIfCannotBeAwake;

		// Token: 0x04000E7A RID: 3706
		public bool showOnCaravanHealthTab;

		// Token: 0x04000E7B RID: 3707
		[Unsaved(false)]
		private PawnCapacityWorker workerInt;
	}
}
