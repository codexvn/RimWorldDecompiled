using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003258 RID: 12888
	public abstract class Transferable : IExposable
	{
		// Token: 0x17002ED3 RID: 11987
		// (get) Token: 0x060121F1 RID: 74225
		public abstract Thing AnyThing { get; }

		// Token: 0x17002ED4 RID: 11988
		// (get) Token: 0x060121F2 RID: 74226
		public abstract ThingDef ThingDef { get; }

		// Token: 0x17002ED5 RID: 11989
		// (get) Token: 0x060121F3 RID: 74227
		public abstract bool Interactive { get; }

		// Token: 0x17002ED6 RID: 11990
		// (get) Token: 0x060121F4 RID: 74228
		public abstract bool HasAnyThing { get; }

		// Token: 0x17002ED7 RID: 11991
		// (get) Token: 0x060121F5 RID: 74229 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsThing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002ED8 RID: 11992
		// (get) Token: 0x060121F6 RID: 74230
		public abstract string Label { get; }

		// Token: 0x17002ED9 RID: 11993
		// (get) Token: 0x060121F7 RID: 74231 RVA: 0x0054E90C File Offset: 0x0054CB0C
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x17002EDA RID: 11994
		// (get) Token: 0x060121F8 RID: 74232
		public abstract string TipDescription { get; }

		// Token: 0x17002EDB RID: 11995
		// (get) Token: 0x060121F9 RID: 74233
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x17002EDC RID: 11996
		// (get) Token: 0x060121FA RID: 74234
		// (set) Token: 0x060121FB RID: 74235
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x17002EDD RID: 11997
		// (get) Token: 0x060121FC RID: 74236 RVA: 0x0054E91F File Offset: 0x0054CB1F
		public int CountToTransferToSource
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return -this.CountToTransfer;
				}
				return this.CountToTransfer;
			}
		}

		// Token: 0x17002EDE RID: 11998
		// (get) Token: 0x060121FD RID: 74237 RVA: 0x0054E937 File Offset: 0x0054CB37
		public int CountToTransferToDestination
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return this.CountToTransfer;
				}
				return -this.CountToTransfer;
			}
		}

		// Token: 0x17002EDF RID: 11999
		// (get) Token: 0x060121FE RID: 74238 RVA: 0x0054E94F File Offset: 0x0054CB4F
		// (set) Token: 0x060121FF RID: 74239 RVA: 0x0054E957 File Offset: 0x0054CB57
		public string EditBuffer
		{
			get
			{
				return this.editBuffer;
			}
			set
			{
				this.editBuffer = value;
			}
		}

		// Token: 0x06012200 RID: 74240
		public abstract int GetMinimumToTransfer();

		// Token: 0x06012201 RID: 74241
		public abstract int GetMaximumToTransfer();

		// Token: 0x06012202 RID: 74242 RVA: 0x0054E960 File Offset: 0x0054CB60
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x06012203 RID: 74243 RVA: 0x0054E96F File Offset: 0x0054CB6F
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x06012204 RID: 74244 RVA: 0x0054E983 File Offset: 0x0054CB83
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06012205 RID: 74245 RVA: 0x0054E993 File Offset: 0x0054CB93
		public AcceptanceReport CanAdjustTo(int destination)
		{
			if (destination == this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (this.ClampAmount(destination) != this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (destination < this.CountToTransfer)
			{
				return this.UnderflowReport();
			}
			return this.OverflowReport();
		}

		// Token: 0x06012206 RID: 74246 RVA: 0x0054E9CF File Offset: 0x0054CBCF
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06012207 RID: 74247 RVA: 0x0054E9E0 File Offset: 0x0054CBE0
		public void AdjustTo(int destination)
		{
			if (!this.CanAdjustTo(destination).Accepted)
			{
				Log.Error("Failed to adjust transferable counts");
				return;
			}
			this.CountToTransfer = this.ClampAmount(destination);
		}

		// Token: 0x06012208 RID: 74248 RVA: 0x0054EA16 File Offset: 0x0054CC16
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x06012209 RID: 74249 RVA: 0x0054EA1F File Offset: 0x0054CC1F
		public void ForceToSource(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(value);
				return;
			}
			this.ForceTo(-value);
		}

		// Token: 0x0601220A RID: 74250 RVA: 0x0054EA39 File Offset: 0x0054CC39
		public void ForceToDestination(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(-value);
				return;
			}
			this.ForceTo(value);
		}

		// Token: 0x0601220B RID: 74251 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawIcon(Rect iconRect)
		{
		}

		// Token: 0x0601220C RID: 74252 RVA: 0x000F9927 File Offset: 0x000F7B27
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x0601220D RID: 74253 RVA: 0x000F9927 File Offset: 0x000F7B27
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x0601220E RID: 74254 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x0400BC3F RID: 48191
		private string editBuffer = "0";
	}
}
