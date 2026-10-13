using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003181 RID: 12673
	public abstract class Dialog_GiveName : Window
	{
		// Token: 0x17002E4A RID: 11850
		// (get) Token: 0x06011D37 RID: 73015 RVA: 0x0052B3BE File Offset: 0x005295BE
		private float Height
		{
			get
			{
				if (!this.useSecondName)
				{
					return 200f;
				}
				return 300f;
			}
		}

		// Token: 0x17002E4B RID: 11851
		// (get) Token: 0x06011D38 RID: 73016 RVA: 0x0052B3D3 File Offset: 0x005295D3
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, this.Height);
			}
		}

		// Token: 0x17002E4C RID: 11852
		// (get) Token: 0x06011D39 RID: 73017 RVA: 0x0052B3E5 File Offset: 0x005295E5
		protected virtual int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x17002E4D RID: 11853
		// (get) Token: 0x06011D3A RID: 73018 RVA: 0x0052B3E5 File Offset: 0x005295E5
		protected virtual int SecondCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x06011D3B RID: 73019 RVA: 0x0052B3EC File Offset: 0x005295EC
		public Dialog_GiveName() : base(null)
		{
			if (Find.AnyPlayerHomeMap != null && Find.AnyPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
			{
				if (Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
				}
				else
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement<Pawn>();
				}
			}
			else
			{
				this.suggestingPawn = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.RandomElement<Pawn>();
			}
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011D3C RID: 73020 RVA: 0x0052B490 File Offset: 0x00529690
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect2;
			if (!this.useSecondName)
			{
				Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst());
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, 80f, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true, null))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, 80f, rect.width / 2f + 70f, 35f), this.curName, this.FirstCharLimit, null);
				rect2 = new Rect(rect.width / 2f + 90f, rect.height - 35f, rect.width / 2f - 90f, 35f);
			}
			else
			{
				float num = 0f;
				string text = this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst();
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true, null))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curName, this.FirstCharLimit, null);
				num += 60f;
				text = this.secondNameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn);
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.secondNameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true, null))
				{
					this.curSecondName = this.secondNameGenerator();
				}
				this.curSecondName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curSecondName, this.SecondCharLimit, null);
				num += 45f;
				float num2 = rect.width / 2f - 90f;
				rect2 = new Rect(rect.width / 2f - num2 / 2f, rect.height - 35f, num2, 35f);
			}
			if (Widgets.ButtonText(rect2, "OK".Translate(), true, true, true, null) || flag)
			{
				string text2 = this.curName;
				string text3 = (text2 != null) ? text2.Trim() : null;
				string text4 = this.curSecondName;
				string text5 = (text4 != null) ? text4.Trim() : null;
				if (this.IsValidName(text3) && (!this.useSecondName || this.IsValidSecondName(text5)))
				{
					if (this.useSecondName)
					{
						this.Named(text3);
						this.NamedSecond(text5);
						Messages.Message(this.gainedNameMessageKey.Translate(text3, text5), MessageTypeDefOf.TaskCompletion, false);
					}
					else
					{
						this.Named(text3);
						Messages.Message(this.gainedNameMessageKey.Translate(text3), MessageTypeDefOf.TaskCompletion, false);
					}
					Find.WindowStack.TryRemove(this, true);
				}
				else
				{
					Messages.Message(this.invalidNameMessageKey.Translate(), MessageTypeDefOf.RejectInput, false);
				}
				Event.current.Use();
			}
		}

		// Token: 0x06011D3D RID: 73021
		protected abstract bool IsValidName(string s);

		// Token: 0x06011D3E RID: 73022
		protected abstract void Named(string s);

		// Token: 0x06011D3F RID: 73023 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		// Token: 0x06011D40 RID: 73024 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void NamedSecond(string s)
		{
		}

		// Token: 0x0400B854 RID: 47188
		protected Pawn suggestingPawn;

		// Token: 0x0400B855 RID: 47189
		protected string curName;

		// Token: 0x0400B856 RID: 47190
		protected Func<string> nameGenerator;

		// Token: 0x0400B857 RID: 47191
		protected string nameMessageKey;

		// Token: 0x0400B858 RID: 47192
		protected string gainedNameMessageKey;

		// Token: 0x0400B859 RID: 47193
		protected string invalidNameMessageKey;

		// Token: 0x0400B85A RID: 47194
		protected bool useSecondName;

		// Token: 0x0400B85B RID: 47195
		protected string curSecondName;

		// Token: 0x0400B85C RID: 47196
		protected Func<string> secondNameGenerator;

		// Token: 0x0400B85D RID: 47197
		protected string secondNameMessageKey;

		// Token: 0x0400B85E RID: 47198
		protected string invalidSecondNameMessageKey;
	}
}
