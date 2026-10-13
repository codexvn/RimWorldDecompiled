using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EE7 RID: 12007
	public class CompEntityHolderPlatform : CompEntityHolder
	{
		// Token: 0x17002BBF RID: 11199
		// (get) Token: 0x06010F30 RID: 69424 RVA: 0x004E08C0 File Offset: 0x004DEAC0
		public override bool Available
		{
			get
			{
				return !base.HoldingPlatform.Occupied;
			}
		}

		// Token: 0x17002BC0 RID: 11200
		// (get) Token: 0x06010F31 RID: 69425 RVA: 0x004E08D0 File Offset: 0x004DEAD0
		public override Pawn HeldPawn
		{
			get
			{
				return base.HoldingPlatform.HeldPawn;
			}
		}

		// Token: 0x17002BC1 RID: 11201
		// (get) Token: 0x06010F32 RID: 69426 RVA: 0x004E08DD File Offset: 0x004DEADD
		public override ThingOwner Container
		{
			get
			{
				return base.HoldingPlatform.innerContainer;
			}
		}

		// Token: 0x17002BC2 RID: 11202
		// (get) Token: 0x06010F33 RID: 69427 RVA: 0x004E08EA File Offset: 0x004DEAEA
		public new CompProperties_EntityHolderPlatform Props
		{
			get
			{
				return (CompProperties_EntityHolderPlatform)this.props;
			}
		}

		// Token: 0x06010F34 RID: 69428 RVA: 0x004E08F7 File Offset: 0x004DEAF7
		public override void EjectContents()
		{
			base.HoldingPlatform.EjectContents();
		}
	}
}
