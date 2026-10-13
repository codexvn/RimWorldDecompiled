using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BFF RID: 11263
	public class CompAttachPoints : ThingComp
	{
		// Token: 0x170027A5 RID: 10149
		// (get) Token: 0x0600FC5C RID: 64604 RVA: 0x0049310B File Offset: 0x0049130B
		public CompProperties_AttachPoints Props
		{
			get
			{
				return (CompProperties_AttachPoints)this.props;
			}
		}

		// Token: 0x0600FC5D RID: 64605 RVA: 0x00493118 File Offset: 0x00491318
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.points = new AttachPointTracker(this.Props.points, this.parent);
		}

		// Token: 0x0400A55C RID: 42332
		public AttachPointTracker points;
	}
}
