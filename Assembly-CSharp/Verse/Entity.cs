using System;

namespace Verse
{
	// Token: 0x02000351 RID: 849
	public abstract class Entity
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060015C3 RID: 5571
		public abstract string LabelCap { get; }

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060015C4 RID: 5572
		public abstract string Label { get; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x00066AFC File Offset: 0x00064CFC
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x00066AFC File Offset: 0x00064CFC
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060015C7 RID: 5575 RVA: 0x00066B04 File Offset: 0x00064D04
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x060015C8 RID: 5576
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x060015C9 RID: 5577
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x060015CA RID: 5578 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void Tick()
		{
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void TickInterval(int delta)
		{
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00066AFC File Offset: 0x00064CFC
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
