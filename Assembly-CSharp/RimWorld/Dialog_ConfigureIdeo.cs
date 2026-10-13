using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003140 RID: 12608
	public class Dialog_ConfigureIdeo : Window
	{
		// Token: 0x17002E1F RID: 11807
		// (get) Token: 0x06011BB6 RID: 72630 RVA: 0x0051D9A1 File Offset: 0x0051BBA1
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1100f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x17002E20 RID: 11808
		// (get) Token: 0x06011BB7 RID: 72631 RVA: 0x0051D9BD File Offset: 0x0051BBBD
		public Ideo CurrentPrimaryIdeo
		{
			get
			{
				return this.newPrimaryIdeo ?? this.initialPrimaryIdeo;
			}
		}

		// Token: 0x06011BB8 RID: 72632 RVA: 0x0051D9D0 File Offset: 0x0051BBD0
		public Dialog_ConfigureIdeo(IEnumerable<Pawn> pawns, Action nextAction, bool forArchonexusRestart = false) : base(null)
		{
			if (!ModLister.CheckIdeology("Configure ideo dialog"))
			{
				return;
			}
			this.forcePause = true;
			this.doCloseX = false;
			this.doCloseButton = false;
			this.closeOnClickedOutside = false;
			this.absorbInputAroundWindow = true;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
			this.openMenuOnCancel = true;
			this.preventSave = true;
			this.nextAction = nextAction;
			this.forArchonexusRestart = forArchonexusRestart;
			this.pawns.AddRange(pawns);
			this.initialPrimaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			foreach (Pawn key in pawns)
			{
				this.pawnConvertToIdeo[key] = null;
			}
		}

		// Token: 0x06011BB9 RID: 72633 RVA: 0x0051DAB8 File Offset: 0x0051BCB8
		public override void PreOpen()
		{
			base.PreOpen();
			IdeoUIUtility.SetSelected(Faction.OfPlayer.ideos.PrimaryIdeo);
		}

		// Token: 0x06011BBA RID: 72634 RVA: 0x0051DAD4 File Offset: 0x0051BCD4
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x06011BBB RID: 72635 RVA: 0x0051DAE4 File Offset: 0x0051BCE4
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 45f), "ConfigureIdeoligion".Translate());
			Text.Font = GameFont.Small;
			IdeoUIUtility.DoIdeoListAndDetails(new Rect(inRect.x, inRect.y + 45f, inRect.width, inRect.height - 55f - 45f), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, this.customOrLoadedIdeo != null, true, this.pawns, this.customOrLoadedIdeo, delegate
			{
				this.CreateNewIdeo(false);
			}, this.forArchonexusRestart, (Pawn p) => this.pawnConvertToIdeo[p] ?? p.Ideo, delegate(Ideo ideo)
			{
				this.CheckRemoveNewIdeoAndMakePrimary(ideo);
				this.customOrLoadedIdeo = ideo;
			}, false, true, delegate
			{
				this.CreateNewIdeo(true);
			});
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x - 18f, inRect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y), "Next".Translate(), true, true, true, null))
			{
				if (Faction.OfPlayer.ideos.PrimaryIdeo != this.CurrentPrimaryIdeo)
				{
					Faction.OfPlayer.ideos.SetPrimary(this.CurrentPrimaryIdeo);
				}
				foreach (Pawn pawn in this.pawns)
				{
					if (this.pawnConvertToIdeo[pawn] != null)
					{
						pawn.ideo.SetIdeo(this.pawnConvertToIdeo[pawn]);
					}
				}
				this.Close(true);
				this.nextAction();
			}
			IEnumerable<Pawn> source = from p in this.pawns
			where p.IsColonist && p.Ideo != Faction.OfPlayer.ideos.PrimaryIdeo
			select p;
			if (source.Any<Pawn>() && Widgets.ButtonText(new Rect(inRect.center.x - Dialog_ConfigureIdeo.AssignColonistsBtnSize.x / 2f, inRect.height - 55f, Dialog_ConfigureIdeo.AssignColonistsBtnSize.x, Dialog_ConfigureIdeo.AssignColonistsBtnSize.y), "AssignColonists".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ChooseColonistsForIdeo(this.CurrentPrimaryIdeo, source, (Pawn p) => p.Ideo != this.initialPrimaryIdeo && p.Ideo != this.CurrentPrimaryIdeo, (Pawn p) => p.Ideo, (Pawn p) => this.pawnConvertToIdeo[p] ?? p.Ideo, delegate(Pawn p, Ideo i)
				{
					if (p.Ideo == i)
					{
						this.pawnConvertToIdeo[p] = null;
						return;
					}
					this.pawnConvertToIdeo[p] = i;
				}));
			}
			Rect rect = new Rect(inRect.x + 18f, inRect.height - 55f, Dialog_ConfigureIdeo.ChangeOrDeleteIdeoBtnSize.x, Dialog_ConfigureIdeo.ChangeOrDeleteIdeoBtnSize.y);
			if (this.customOrLoadedIdeo != null && IdeoUIUtility.selected == this.customOrLoadedIdeo && this.CurrentPrimaryIdeo == this.customOrLoadedIdeo)
			{
				if (Widgets.ButtonText(rect, "RemoveNewIdeoligion".Translate(), true, true, true, null))
				{
					this.CheckRemoveNewIdeoAndMakePrimary(this.initialPrimaryIdeo);
					return;
				}
			}
			else if (IdeoUIUtility.selected != this.CurrentPrimaryIdeo && Widgets.ButtonText(rect, "MakeIdeoligionPrimary".Translate(), true, true, true, null))
			{
				this.CheckRemoveNewIdeoAndMakePrimary(IdeoUIUtility.selected);
			}
		}

		// Token: 0x06011BBC RID: 72636 RVA: 0x0051DE8C File Offset: 0x0051C08C
		private void CreateNewIdeo(bool fluid = false)
		{
			Ideo newIdeo = IdeoUtility.MakeEmptyIdeo();
			newIdeo.Fluid = fluid;
			Find.WindowStack.Add(new Dialog_ChooseMemes(newIdeo, MemeCategory.Structure, false, delegate()
			{
				this.CheckRemoveNewIdeoAndMakePrimary(newIdeo);
				this.customOrLoadedIdeo = newIdeo;
			}, null, false));
		}

		// Token: 0x06011BBD RID: 72637 RVA: 0x0051DEE2 File Offset: 0x0051C0E2
		private void RemoveNewIdeoIfCreated()
		{
			if (this.customOrLoadedIdeo != null)
			{
				Find.IdeoManager.Remove(this.customOrLoadedIdeo);
				this.customOrLoadedIdeo = null;
			}
		}

		// Token: 0x06011BBE RID: 72638 RVA: 0x0051DF04 File Offset: 0x0051C104
		private void CheckRemoveNewIdeoAndMakePrimary(Ideo primaryIdeo)
		{
			this.MakeIdeoPrimary(primaryIdeo);
			this.RemoveNewIdeoIfCreated();
			SoundDefOf.Click.PlayOneShotOnCamera(null);
		}

		// Token: 0x06011BBF RID: 72639 RVA: 0x0051DF20 File Offset: 0x0051C120
		private void MakeIdeoPrimary(Ideo primaryIdeo)
		{
			if (!Find.IdeoManager.IdeosListForReading.Contains(primaryIdeo))
			{
				Find.IdeoManager.Add(primaryIdeo);
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (this.pawnConvertToIdeo[pawn] == this.CurrentPrimaryIdeo)
				{
					this.pawnConvertToIdeo[pawn] = primaryIdeo;
				}
				else if (this.pawnConvertToIdeo[pawn] == null && pawn.Ideo == this.initialPrimaryIdeo)
				{
					this.pawnConvertToIdeo[pawn] = primaryIdeo;
				}
			}
			this.newPrimaryIdeo = primaryIdeo;
			IdeoUIUtility.SetSelected(primaryIdeo);
		}

		// Token: 0x0400B6F1 RID: 46833
		public const float TitleAreaHeight = 45f;

		// Token: 0x0400B6F2 RID: 46834
		private static readonly Vector2 ChangeOrDeleteIdeoBtnSize = new Vector2(250f, 40f);

		// Token: 0x0400B6F3 RID: 46835
		private static readonly Vector2 AssignColonistsBtnSize = new Vector2(250f, 40f);

		// Token: 0x0400B6F4 RID: 46836
		private Action nextAction;

		// Token: 0x0400B6F5 RID: 46837
		private bool forArchonexusRestart;

		// Token: 0x0400B6F6 RID: 46838
		private Vector2 scrollPosition_ideoList;

		// Token: 0x0400B6F7 RID: 46839
		private float scrollViewHeight_ideoList;

		// Token: 0x0400B6F8 RID: 46840
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x0400B6F9 RID: 46841
		private float scrollViewHeight_ideoDetails;

		// Token: 0x0400B6FA RID: 46842
		private Ideo initialPrimaryIdeo;

		// Token: 0x0400B6FB RID: 46843
		private Ideo newPrimaryIdeo;

		// Token: 0x0400B6FC RID: 46844
		private Ideo customOrLoadedIdeo;

		// Token: 0x0400B6FD RID: 46845
		private Dictionary<Pawn, Ideo> pawnConvertToIdeo = new Dictionary<Pawn, Ideo>();

		// Token: 0x0400B6FE RID: 46846
		private List<Pawn> pawns = new List<Pawn>();
	}
}
