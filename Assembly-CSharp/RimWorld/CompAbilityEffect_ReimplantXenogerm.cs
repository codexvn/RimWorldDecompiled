using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompAbilityEffect_ReimplantXenogerm : CompAbilityEffect
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public CompAbilityEffect_ReimplantXenogerm _003C_003E4__this;

		public Pawn myPawn;

		internal void _003CCompGetGizmosExtra_003Eb__0()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<Pawn> list2 = myPawn.MapHeld.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
			for (int i = 0; i < list2.Count; i++)
			{
				_003C_003Ec__DisplayClass7_1 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass7_1
				{
					CS_0024_003C_003E8__locals1 = this,
					absorber = list2[i]
				};
				if (CS_0024_003C_003E8__locals15.absorber.genes == null || !CS_0024_003C_003E8__locals15.absorber.IsColonistPlayerControlled || GeneUtility.SameXenotype(CS_0024_003C_003E8__locals15.absorber, myPawn) || !CS_0024_003C_003E8__locals15.absorber.CanReach(myPawn, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					continue;
				}
				if (!PawnIdeoCanAcceptReimplant(_003C_003E4__this.parent.pawn, CS_0024_003C_003E8__locals15.absorber))
				{
					list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals15.absorber.LabelCap + ": " + "IdeoligionForbids".Translate(), null, CS_0024_003C_003E8__locals15.absorber, Color.white));
					continue;
				}
				list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals15.absorber.LabelShort, delegate
				{
					if (GeneUtility.PawnWouldDieFromReimplanting(CS_0024_003C_003E8__locals15.CS_0024_003C_003E8__locals1.myPawn))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromReimplanting".Translate(CS_0024_003C_003E8__locals15.CS_0024_003C_003E8__locals1.myPawn.Named("PAWN")), delegate
						{
							GeneUtility.GiveReimplantJob(CS_0024_003C_003E8__locals15.absorber, CS_0024_003C_003E8__locals15.CS_0024_003C_003E8__locals1.myPawn);
						}, destructive: true));
					}
					else
					{
						GeneUtility.GiveReimplantJob(CS_0024_003C_003E8__locals15.absorber, CS_0024_003C_003E8__locals15.CS_0024_003C_003E8__locals1.myPawn);
					}
				}, CS_0024_003C_003E8__locals15.absorber, Color.white));
			}
			if (list.Any())
			{
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_1
	{
		public Pawn absorber;

		public _003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals1;

		public Action _003C_003E9__2;

		internal void _003CCompGetGizmosExtra_003Eb__1()
		{
			if (GeneUtility.PawnWouldDieFromReimplanting(CS_0024_003C_003E8__locals1.myPawn))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromReimplanting".Translate(CS_0024_003C_003E8__locals1.myPawn.Named("PAWN")), delegate
				{
					GeneUtility.GiveReimplantJob(absorber, CS_0024_003C_003E8__locals1.myPawn);
				}, destructive: true));
			}
			else
			{
				GeneUtility.GiveReimplantJob(absorber, CS_0024_003C_003E8__locals1.myPawn);
			}
		}

		internal void _003CCompGetGizmosExtra_003Eb__2()
		{
			GeneUtility.GiveReimplantJob(absorber, CS_0024_003C_003E8__locals1.myPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_ReimplantXenogerm _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCompGetGizmosExtra_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompAbilityEffect_ReimplantXenogerm compAbilityEffect_ReimplantXenogerm = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals29 = new _003C_003Ec__DisplayClass7_0
				{
					_003C_003E4__this = _003C_003E4__this
				};
				if (!GeneUtility.CanAbsorbXenogerm(compAbilityEffect_ReimplantXenogerm.parent.pawn))
				{
					break;
				}
				CS_0024_003C_003E8__locals29.myPawn = compAbilityEffect_ReimplantXenogerm.parent.pawn;
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "ForceXenogermImplantation".Translate(),
					defaultDesc = "ForceXenogermImplantationDesc".Translate(),
					icon = ReimplantIcon.Texture,
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						List<Pawn> list2 = CS_0024_003C_003E8__locals29.myPawn.MapHeld.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
						for (int i = 0; i < list2.Count; i++)
						{
							_003C_003Ec__DisplayClass7_1 CS_0024_003C_003E8__locals22 = new _003C_003Ec__DisplayClass7_1
							{
								CS_0024_003C_003E8__locals1 = CS_0024_003C_003E8__locals29,
								absorber = list2[i]
							};
							if (CS_0024_003C_003E8__locals22.absorber.genes != null && CS_0024_003C_003E8__locals22.absorber.IsColonistPlayerControlled && !GeneUtility.SameXenotype(CS_0024_003C_003E8__locals22.absorber, CS_0024_003C_003E8__locals29.myPawn) && CS_0024_003C_003E8__locals22.absorber.CanReach(CS_0024_003C_003E8__locals29.myPawn, PathEndMode.ClosestTouch, Danger.Deadly))
							{
								if (!PawnIdeoCanAcceptReimplant(CS_0024_003C_003E8__locals29._003C_003E4__this.parent.pawn, CS_0024_003C_003E8__locals22.absorber))
								{
									list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals22.absorber.LabelCap + ": " + "IdeoligionForbids".Translate(), null, CS_0024_003C_003E8__locals22.absorber, Color.white));
								}
								else
								{
									list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals22.absorber.LabelShort, delegate
									{
										if (GeneUtility.PawnWouldDieFromReimplanting(CS_0024_003C_003E8__locals22.CS_0024_003C_003E8__locals1.myPawn))
										{
											Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromReimplanting".Translate(CS_0024_003C_003E8__locals22.CS_0024_003C_003E8__locals1.myPawn.Named("PAWN")), delegate
											{
												GeneUtility.GiveReimplantJob(CS_0024_003C_003E8__locals22.absorber, CS_0024_003C_003E8__locals22.CS_0024_003C_003E8__locals1.myPawn);
											}, destructive: true));
										}
										else
										{
											GeneUtility.GiveReimplantJob(CS_0024_003C_003E8__locals22.absorber, CS_0024_003C_003E8__locals22.CS_0024_003C_003E8__locals1.myPawn);
										}
									}, CS_0024_003C_003E8__locals22.absorber, Color.white));
								}
							}
						}
						if (list.Any())
						{
							Find.WindowStack.Add(new FloatMenu(list));
						}
					}
				};
				if (CS_0024_003C_003E8__locals29.myPawn.IsQuestLodger())
				{
					command_Action.Disable("TemporaryFactionMember".Translate(CS_0024_003C_003E8__locals29.myPawn.Named("PAWN")));
				}
				else if (CS_0024_003C_003E8__locals29.myPawn.health.hediffSet.HasHediff(HediffDefOf.XenogermLossShock))
				{
					command_Action.Disable("XenogermLossShockPresent".Translate(CS_0024_003C_003E8__locals29.myPawn.Named("PAWN")));
				}
				else if (CS_0024_003C_003E8__locals29.myPawn.IsPrisonerOfColony && !CS_0024_003C_003E8__locals29.myPawn.Downed)
				{
					command_Action.Disable("MessageTargetMustBeDownedToForceReimplant".Translate(CS_0024_003C_003E8__locals29.myPawn.Named("PAWN")));
				}
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCustomWarmupMotes_003Ed__5 : IEnumerable<Mote>, IEnumerable, IEnumerator<Mote>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Mote _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LocalTargetInfo target;

		public LocalTargetInfo _003C_003E3__target;

		Mote IEnumerator<Mote>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCustomWarmupMotes_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Pawn pawn = target.Pawn;
				_003C_003E2__current = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_XenogermImplantation, new Vector3(0f, 0f, 0.3f));
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Mote> IEnumerable<Mote>.GetEnumerator()
		{
			_003CCustomWarmupMotes_003Ed__5 _003CCustomWarmupMotes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCustomWarmupMotes_003Ed__ = this;
			}
			else
			{
				_003CCustomWarmupMotes_003Ed__ = new _003CCustomWarmupMotes_003Ed__5(0);
			}
			_003CCustomWarmupMotes_003Ed__.target = _003C_003E3__target;
			return _003CCustomWarmupMotes_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Mote>)this).GetEnumerator();
		}
	}

	private static readonly CachedTexture ReimplantIcon = new CachedTexture("UI/Icons/Genes/Gene_XenogermReimplanter");

	private new CompProperties_AbilityReimplantXenogerm Props => (CompProperties_AbilityReimplantXenogerm)props;

	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
	{
		if (!ModLister.CheckBiotech("xenogerm reimplantation"))
		{
			return;
		}
		base.Apply(target, dest);
		Pawn pawn = target.Pawn;
		if (pawn != null)
		{
			GeneUtility.ReimplantXenogerm(parent.pawn, pawn);
			FleckMaker.AttachedOverlay(pawn, FleckDefOf.FlashHollow, new Vector3(0f, 0f, 0.26f));
			if (PawnUtility.ShouldSendNotificationAbout(parent.pawn) || PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				int max = HediffDefOf.XenogerminationComa.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.max;
				int max2 = HediffDefOf.XenogermLossShock.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.max;
				Find.LetterStack.ReceiveLetter("LetterLabelGenesImplanted".Translate(), "LetterTextGenesImplanted".Translate(parent.pawn.Named("CASTER"), pawn.Named("TARGET"), max.ToStringTicksToPeriod().Named("COMADURATION"), max2.ToStringTicksToPeriod().Named("SHOCKDURATION")), LetterDefOf.NeutralEvent, new LookTargets(parent.pawn, pawn));
			}
		}
	}

	public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
	{
		Pawn pawn = target.Pawn;
		if (pawn == null)
		{
			return base.Valid(target, throwMessages);
		}
		if (pawn.IsQuestLodger())
		{
			if (throwMessages)
			{
				Messages.Message("MessageCannotImplantInTempFactionMembers".Translate(), pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		if (pawn.HostileTo(parent.pawn) && !pawn.Downed)
		{
			if (throwMessages)
			{
				Messages.Message("MessageCantUseOnResistingPerson".Translate(parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		if (!parent.pawn.genes.Xenogenes.Any())
		{
			if (throwMessages)
			{
				Messages.Message("MessagePawnHasNoXenogenes".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		if (GeneUtility.SameXenotype(pawn, parent.pawn))
		{
			if (throwMessages)
			{
				Messages.Message("MessageCannotUseOnSameXenotype".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		if (!PawnIdeoCanAcceptReimplant(parent.pawn, pawn))
		{
			if (throwMessages)
			{
				Messages.Message("MessageCannotBecomeNonPreferredXenotype".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return base.Valid(target, throwMessages);
	}

	public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
	{
		if (GeneUtility.PawnWouldDieFromReimplanting(parent.pawn))
		{
			return Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromReimplanting".Translate(parent.pawn.Named("PAWN")), confirmAction, destructive: true);
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CCustomWarmupMotes_003Ed__5))]
	public override IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCustomWarmupMotes_003Ed__5(-2)
		{
			_003C_003E3__target = target
		};
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__7))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static bool PawnIdeoCanAcceptReimplant(Pawn implanter, Pawn implantee)
	{
		if (!ModsConfig.IdeologyActive)
		{
			return true;
		}
		if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.PropagateBloodfeederGene, implantee) && implanter.genes.Xenogenes.Any((Gene x) => x.def == GeneDefOf.Bloodfeeder))
		{
			return false;
		}
		if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.BecomeNonPreferredXenotype, implantee) && !implantee.Ideo.IsPreferredXenotype(implanter))
		{
			return false;
		}
		return true;
	}
}
