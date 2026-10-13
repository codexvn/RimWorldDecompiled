using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200068D RID: 1677
	public class HediffComp_AttachPoints : HediffComp
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002A51 RID: 10833 RVA: 0x000DEDB1 File Offset: 0x000DCFB1
		public HediffCompProperties_AttachPoints Props
		{
			get
			{
				return (HediffCompProperties_AttachPoints)this.props;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x000DEDC0 File Offset: 0x000DCFC0
		public AttachPointTracker Points
		{
			get
			{
				if (this.parent.pawn.story == null)
				{
					return null;
				}
				if (this.points == null)
				{
					if (this.parent.pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
					{
						Pawn_StoryTracker story = this.parent.pawn.story;
						bool flag;
						if (story == null)
						{
							flag = (null != null);
						}
						else
						{
							BodyTypeDef bodyType = story.bodyType;
							flag = (((bodyType != null) ? bodyType.attachPointsDessicated : null) != null);
						}
						if (!flag)
						{
							string format = "Pawn {0} is dessicated but their {1} bodyType def doesn't declare attachPointsDessicated, falling back to non-dessicated attachpoints instead";
							object pawn = this.parent.pawn;
							Pawn_StoryTracker story2 = this.parent.pawn.story;
							Log.Warning(string.Format(format, pawn, (story2 != null) ? story2.bodyType : null));
						}
					}
					if (this.parent.pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
					{
						Pawn_StoryTracker story3 = this.parent.pawn.story;
						bool flag2;
						if (story3 == null)
						{
							flag2 = (null != null);
						}
						else
						{
							BodyTypeDef bodyType2 = story3.bodyType;
							flag2 = (((bodyType2 != null) ? bodyType2.attachPointsDessicated : null) != null);
						}
						if (flag2)
						{
							this.points = new AttachPointTracker(this.parent.pawn.story.bodyType.attachPointsDessicated, this.parent.pawn);
							goto IL_16F;
						}
					}
					Pawn_StoryTracker story4 = this.parent.pawn.story;
					bool flag3;
					if (story4 == null)
					{
						flag3 = (null != null);
					}
					else
					{
						BodyTypeDef bodyType3 = story4.bodyType;
						flag3 = (((bodyType3 != null) ? bodyType3.attachPoints : null) != null);
					}
					if (flag3)
					{
						this.points = new AttachPointTracker(this.parent.pawn.story.bodyType.attachPoints, this.parent.pawn);
					}
				}
				IL_16F:
				return this.points;
			}
		}

		// Token: 0x0400224C RID: 8780
		private AttachPointTracker points;
	}
}
