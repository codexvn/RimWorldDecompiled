using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA6 RID: 6822
	public class PawnColumnDef : Def
	{
		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x060098BF RID: 39103 RVA: 0x002C8A73 File Offset: 0x002C6C73
		public PawnColumnWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnColumnWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x060098C0 RID: 39104 RVA: 0x002C8AA5 File Offset: 0x002C6CA5
		public Texture2D HeaderIcon
		{
			get
			{
				if (this.headerIconTex == null && !this.headerIcon.NullOrEmpty())
				{
					this.headerIconTex = ContentFinder<Texture2D>.Get(this.headerIcon, true);
				}
				return this.headerIconTex;
			}
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x060098C1 RID: 39105 RVA: 0x002C8ADC File Offset: 0x002C6CDC
		public Vector2 HeaderIconSize
		{
			get
			{
				if (this.headerIconSize != default(Vector2))
				{
					return this.headerIconSize;
				}
				if (this.HeaderIcon != null)
				{
					return PawnColumnDef.IconSize;
				}
				return Vector2.zero;
			}
		}

		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x060098C2 RID: 39106 RVA: 0x002C8B1F File Offset: 0x002C6D1F
		public bool HeaderInteractable
		{
			get
			{
				return this.sortable || !this.headerTip.NullOrEmpty() || this.headerAlwaysInteractable;
			}
		}

		// Token: 0x04006798 RID: 26520
		public Type workerClass = typeof(PawnColumnWorker);

		// Token: 0x04006799 RID: 26521
		public bool sortable;

		// Token: 0x0400679A RID: 26522
		public bool ignoreWhenCalculatingOptimalTableSize;

		// Token: 0x0400679B RID: 26523
		[NoTranslate]
		public string headerIcon;

		// Token: 0x0400679C RID: 26524
		public Vector2 headerIconSize;

		// Token: 0x0400679D RID: 26525
		[MustTranslate]
		public string headerTip;

		// Token: 0x0400679E RID: 26526
		public bool headerAlwaysInteractable;

		// Token: 0x0400679F RID: 26527
		public bool paintable;

		// Token: 0x040067A0 RID: 26528
		public bool groupable;

		// Token: 0x040067A1 RID: 26529
		public TrainableDef trainable;

		// Token: 0x040067A2 RID: 26530
		public int gap;

		// Token: 0x040067A3 RID: 26531
		public WorkTypeDef workType;

		// Token: 0x040067A4 RID: 26532
		public bool moveWorkTypeLabelDown;

		// Token: 0x040067A5 RID: 26533
		public bool showIcon;

		// Token: 0x040067A6 RID: 26534
		public bool useLabelShort;

		// Token: 0x040067A7 RID: 26535
		public int widthPriority;

		// Token: 0x040067A8 RID: 26536
		public int width = -1;

		// Token: 0x040067A9 RID: 26537
		[Unsaved(false)]
		private PawnColumnWorker workerInt;

		// Token: 0x040067AA RID: 26538
		[Unsaved(false)]
		private Texture2D headerIconTex;

		// Token: 0x040067AB RID: 26539
		private const int IconWidth = 26;

		// Token: 0x040067AC RID: 26540
		private static readonly Vector2 IconSize = new Vector2(26f, 26f);
	}
}
