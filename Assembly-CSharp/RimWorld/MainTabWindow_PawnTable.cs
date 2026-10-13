using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003404 RID: 13316
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		// Token: 0x17003117 RID: 12567
		// (get) Token: 0x06012BA5 RID: 76709 RVA: 0x0058BBE4 File Offset: 0x00589DE4
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		// Token: 0x17003118 RID: 12568
		// (get) Token: 0x06012BA6 RID: 76710 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17003119 RID: 12569
		// (get) Token: 0x06012BA7 RID: 76711
		protected abstract PawnTableDef PawnTableDef { get; }

		// Token: 0x1700311A RID: 12570
		// (get) Token: 0x06012BA8 RID: 76712 RVA: 0x0058BBEB File Offset: 0x00589DEB
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		// Token: 0x1700311B RID: 12571
		// (get) Token: 0x06012BA9 RID: 76713 RVA: 0x0058BBF4 File Offset: 0x00589DF4
		public override Vector2 RequestedTabSize
		{
			get
			{
				if (this.table == null)
				{
					return Vector2.zero;
				}
				return new Vector2(this.table.Size.x + this.Margin * 2f, this.table.Size.y + this.ExtraBottomSpace + this.ExtraTopSpace + this.Margin * 2f);
			}
		}

		// Token: 0x1700311C RID: 12572
		// (get) Token: 0x06012BAA RID: 76714 RVA: 0x0058BC5C File Offset: 0x00589E5C
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		// Token: 0x06012BAB RID: 76715 RVA: 0x0058BC6D File Offset: 0x00589E6D
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		// Token: 0x06012BAC RID: 76716 RVA: 0x0058BC8F File Offset: 0x00589E8F
		public override void DoWindowContents(Rect rect)
		{
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		// Token: 0x06012BAD RID: 76717 RVA: 0x0058BCB6 File Offset: 0x00589EB6
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		// Token: 0x06012BAE RID: 76718 RVA: 0x0058BCBE File Offset: 0x00589EBE
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		// Token: 0x06012BAF RID: 76719 RVA: 0x0058BCD4 File Offset: 0x00589ED4
		private PawnTable CreateTable()
		{
			return (PawnTable)Activator.CreateInstance(this.PawnTableDef.workerClass, new object[]
			{
				this.PawnTableDef,
				new Func<IEnumerable<Pawn>>(() => this.Pawns),
				UI.screenWidth - (int)(this.Margin * 2f),
				(int)((float)(UI.screenHeight - 35) - this.ExtraBottomSpace - this.ExtraTopSpace - this.Margin * 2f)
			});
		}

		// Token: 0x06012BB0 RID: 76720 RVA: 0x0058BD5C File Offset: 0x00589F5C
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x0400C27B RID: 49787
		private PawnTable table;
	}
}
