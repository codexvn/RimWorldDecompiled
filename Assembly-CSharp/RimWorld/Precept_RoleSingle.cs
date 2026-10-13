using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002817 RID: 10263
	public class Precept_RoleSingle : Precept_Role
	{
		// Token: 0x17002243 RID: 8771
		// (get) Token: 0x0600E082 RID: 57474 RVA: 0x0041EBC7 File Offset: 0x0041CDC7
		public Pawn ChosenPawnValue
		{
			get
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				if (ideoRoleInstance == null)
				{
					return null;
				}
				return ideoRoleInstance.pawn;
			}
		}

		// Token: 0x17002244 RID: 8772
		// (get) Token: 0x0600E083 RID: 57475 RVA: 0x0041EBDA File Offset: 0x0041CDDA
		public List<Ability> AbilitiesForReading
		{
			get
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				if (ideoRoleInstance == null)
				{
					return null;
				}
				return ideoRoleInstance.abilities;
			}
		}

		// Token: 0x0600E084 RID: 57476 RVA: 0x0041EBED File Offset: 0x0041CDED
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			this.chosenPawn = new IdeoRoleInstance(this);
			base.Init(ideo, generatingFor);
		}

		// Token: 0x0600E085 RID: 57477 RVA: 0x0041EC03 File Offset: 0x0041CE03
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			base.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			if (p == this.ChosenPawnValue && oldFaction.IsPlayer)
			{
				this.Assign(null, false);
			}
		}

		// Token: 0x0600E086 RID: 57478 RVA: 0x0041EC27 File Offset: 0x0041CE27
		public override IEnumerable<Pawn> ChosenPawns()
		{
			if (this.ChosenPawnValue != null)
			{
				yield return this.chosenPawn.pawn;
			}
			yield break;
		}

		// Token: 0x0600E087 RID: 57479 RVA: 0x0041EC38 File Offset: 0x0041CE38
		public override void RecacheActivity()
		{
			int colonistBelieverCountCached = this.ideo.ColonistBelieverCountCached;
			bool flag = Faction.OfPlayer.ideos.Has(this.ideo);
			if (this.active && this.def.deactivationBelieverCount >= 0 && colonistBelieverCountCached <= this.def.deactivationBelieverCount && !this.def.leaderRole)
			{
				this.active = false;
				if (flag)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRoleInactive".Translate(this.name).CapitalizeFirst(), "LetterLabelRoleInactiveDesc".Translate(this.ideo.memberName, this.def.deactivationBelieverCount, this.def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent, null, 0, true);
				}
				if (this.ChosenPawnValue != null)
				{
					if (flag)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonLowBelieversDesc".Translate(this.ideo.memberName).CapitalizeFirst(), LetterDefOf.NeutralEvent, this.ChosenPawnValue, null, null, null, null, 0, true);
					}
					base.Notify_PawnUnassigned(this.ChosenPawnValue);
					this.chosenPawn.pawn = null;
				}
			}
			if (!this.active && this.def.activationBelieverCount >= 0 && (colonistBelieverCountCached >= this.def.activationBelieverCount || this.def.leaderRole))
			{
				this.active = true;
				if (flag && !this.def.leaderRole && this.def.activationBelieverCount > 0 && Find.TickManager.TicksGame > 1)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRoleActive".Translate(this.name).CapitalizeFirst(), "LetterLabelRoleActiveDesc".Translate(this.ideo.memberName, this.def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent, null, 0, true);
				}
			}
			if (this.ChosenPawnValue != null && !base.ValidatePawn(this.ChosenPawnValue))
			{
				base.Notify_PawnUnassigned(this.ChosenPawnValue);
				this.chosenPawn.pawn = null;
			}
		}

		// Token: 0x0600E088 RID: 57480 RVA: 0x0041EEFC File Offset: 0x0041D0FC
		public override void Assign(Pawn p, bool addThoughts)
		{
			if (p != this.ChosenPawnValue)
			{
				if (p != null && !base.ValidatePawn(p))
				{
					Log.Error("Invalid pawn assigned to " + base.LabelCap + " role. pawn=" + p.GetUniqueLoadID());
				}
				if (this.ChosenPawnValue != null && addThoughts)
				{
					if (p != null)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonUnassignedDesc".Translate(this.ChosenPawnValue.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, this.ChosenPawnValue, null, null, null, null, 0, true);
					}
					this.ChosenPawnValue.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this), null);
				}
				Pawn chosenPawnValue = this.ChosenPawnValue;
				this.chosenPawn.pawn = p;
				base.Notify_PawnUnassigned(chosenPawnValue);
				base.Notify_PawnAssigned(p);
				if (this.def.leaderRole && Current.ProgramState == ProgramState.Playing)
				{
					Faction ofPlayer = Faction.OfPlayer;
					if (ofPlayer != null)
					{
						ofPlayer.leader = p;
						foreach (Ideo ideo in ofPlayer.ideos.AllIdeos)
						{
							foreach (Precept precept in ideo.PreceptsListForReading)
							{
								if (precept != this)
								{
									Precept_Role precept_Role = precept as Precept_Role;
									if (precept_Role != null && precept_Role.def.leaderRole)
									{
										precept_Role.Assign(null, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600E089 RID: 57481 RVA: 0x0041F110 File Offset: 0x0041D310
		public override void FillOrUpdateAbilities()
		{
			if (!this.def.grantedAbilities.NullOrEmpty<AbilityDef>())
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				Pawn chosenPawnValue = this.ChosenPawnValue;
				IdeoRoleInstance ideoRoleInstance2 = this.chosenPawn;
				ideoRoleInstance.abilities = base.FillOrUpdateAbilityList(chosenPawnValue, (ideoRoleInstance2 != null) ? ideoRoleInstance2.abilities : null);
			}
		}

		// Token: 0x0600E08A RID: 57482 RVA: 0x0041F14D File Offset: 0x0041D34D
		public override List<Ability> AbilitiesFor(Pawn p)
		{
			return this.chosenPawn.abilities;
		}

		// Token: 0x0600E08B RID: 57483 RVA: 0x0041EBC7 File Offset: 0x0041CDC7
		public override Pawn ChosenPawnSingle()
		{
			IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
			if (ideoRoleInstance == null)
			{
				return null;
			}
			return ideoRoleInstance.pawn;
		}

		// Token: 0x0600E08C RID: 57484 RVA: 0x0041F15A File Offset: 0x0041D35A
		public override bool IsAssigned(Pawn p)
		{
			IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
			return ((ideoRoleInstance != null) ? ideoRoleInstance.pawn : null) == p;
		}

		// Token: 0x0600E08D RID: 57485 RVA: 0x0041F171 File Offset: 0x0041D371
		public override void Unassign(Pawn p, bool generateThoughts)
		{
			this.Assign(null, generateThoughts);
		}

		// Token: 0x0600E08E RID: 57486 RVA: 0x0041F17C File Offset: 0x0041D37C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<IdeoRoleInstance>(ref this.chosenPawn, "chosenPawn", new object[]
			{
				this
			});
			if (GameDataSaveLoader.IsSavingOrLoadingExternalIdeo && Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.chosenPawn = new IdeoRoleInstance(this);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ChosenPawnValue != null && !base.ValidatePawn(this.ChosenPawnValue))
				{
					Pawn pawn = this.chosenPawn.pawn;
					this.chosenPawn.pawn = null;
					base.Notify_PawnUnassigned(pawn);
				}
				this.chosenPawn.sourceRole = this;
				this.FillOrUpdateAbilities();
			}
		}

		// Token: 0x0600E08F RID: 57487 RVA: 0x0041F213 File Offset: 0x0041D413
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			((Precept_RoleSingle)precept).chosenPawn = this.chosenPawn;
		}

		// Token: 0x040096F6 RID: 38646
		public IdeoRoleInstance chosenPawn;
	}
}
