using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EE5 RID: 12005
	public abstract class CompEntityHolder : ThingComp
	{
		// Token: 0x17002BB9 RID: 11193
		// (get) Token: 0x06010F26 RID: 69414 RVA: 0x004E07A3 File Offset: 0x004DE9A3
		public virtual float ContainmentStrength
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.ContainmentStrength, true, 15);
			}
		}

		// Token: 0x17002BBA RID: 11194
		// (get) Token: 0x06010F27 RID: 69415 RVA: 0x004E07B8 File Offset: 0x004DE9B8
		public CompProperties_EntityHolder Props
		{
			get
			{
				return (CompProperties_EntityHolder)this.props;
			}
		}

		// Token: 0x17002BBB RID: 11195
		// (get) Token: 0x06010F28 RID: 69416 RVA: 0x004E07C5 File Offset: 0x004DE9C5
		protected Building_HoldingPlatform HoldingPlatform
		{
			get
			{
				return (Building_HoldingPlatform)this.parent;
			}
		}

		// Token: 0x17002BBC RID: 11196
		// (get) Token: 0x06010F29 RID: 69417
		public abstract bool Available { get; }

		// Token: 0x17002BBD RID: 11197
		// (get) Token: 0x06010F2A RID: 69418
		public abstract Pawn HeldPawn { get; }

		// Token: 0x17002BBE RID: 11198
		// (get) Token: 0x06010F2B RID: 69419
		public abstract ThingOwner Container { get; }

		// Token: 0x06010F2C RID: 69420
		public abstract void EjectContents();

		// Token: 0x06010F2D RID: 69421 RVA: 0x004E07D4 File Offset: 0x004DE9D4
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			float statValue = this.parent.GetStatValue(StatDefOf.ContainmentStrength, true, -1);
			text += string.Format("{0}: {1:F0}", StatDefOf.ContainmentStrength.LabelCap, statValue);
			if (!this.parent.Spawned)
			{
				return text;
			}
			if (this.parent.IsOutside())
			{
				text += string.Format(" ({0})", "Outdoors".Translate());
			}
			else if (StatWorker_ContainmentStrength.AnyDoorForcedOpen(this.parent.GetRoom(RegionType.Set_All)))
			{
				text += string.Format(" ({0})", "Stat_ContainmentStrength_DoorForcedOpen".Translate());
			}
			return text;
		}
	}
}
