using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Xenogerm : GeneSetHolderBase
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public Xenogerm _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.SetTargetPawn(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public Pawn pawn;

		public Xenogerm _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__1()
		{
			_003C_003E4__this.SetTargetPawn(pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__14 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Xenogerm _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private _003C_003Ec__DisplayClass14_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Xenogerm xenogerm = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass14_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.selPawn = selPawn;
					_003C_003E7__wrap1 = ((ThingWithComps)xenogerm).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b2;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b2;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					_003C_003E1__state = -1;
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00b2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (ModsConfig.BiotechActive && _003C_003E8__1.selPawn.genes != null)
					{
						int num2 = GeneUtility.MetabolismAfterImplanting(_003C_003E8__1.selPawn, xenogerm.geneSet);
						if (num2 < GeneTuning.BiostatRange.TrueMin)
						{
							_003C_003E2__current = new FloatMenuOption("CannotGenericWorkCustom".Translate(string.Concat("OrderImplantationIntoPawn".Translate(_003C_003E8__1.selPawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "ResultingMetTooLow".Translate() + " (", num2.ToString(), ")")), null);
							_003C_003E1__state = 2;
							return true;
						}
						if (xenogerm.PawnIdeoDisallowsImplanting(_003C_003E8__1.selPawn))
						{
							_003C_003E2__current = new FloatMenuOption("CannotGenericWorkCustom".Translate("OrderImplantationIntoPawn".Translate(_003C_003E8__1.selPawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "IdeoligionForbids".Translate()), null);
							_003C_003E1__state = 3;
							return true;
						}
						_003C_003E2__current = new FloatMenuOption("OrderImplantationIntoPawn".Translate(_003C_003E8__1.selPawn.Named("PAWN")) + " (" + xenogerm.xenotypeName + ")", delegate
						{
							_003C_003E8__1._003C_003E4__this.SetTargetPawn(_003C_003E8__1.selPawn);
						});
						_003C_003E1__state = 4;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__14 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__16 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Xenogerm _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Xenogerm CS_0024_003C_003E8__locals31 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((GeneSetHolderBase)CS_0024_003C_003E8__locals31).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals31.geneSet == null)
					{
						break;
					}
					if (CS_0024_003C_003E8__locals31.targetPawn == null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "ImplantXenogerm".Translate() + "...",
							defaultDesc = "ImplantXenogermDesc".Translate(CS_0024_003C_003E8__locals31.RequiredMedicineForImplanting.Named("MEDICINEAMOUNT")),
							icon = ImplantTex.Texture,
							action = delegate
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								foreach (Pawn item in CS_0024_003C_003E8__locals31.Map.mapPawns.AllPawnsSpawned)
								{
									_003C_003Ec__DisplayClass16_0 CS_0024_003C_003E8__locals27 = new _003C_003Ec__DisplayClass16_0
									{
										_003C_003E4__this = CS_0024_003C_003E8__locals31,
										pawn = item
									};
									if (!CS_0024_003C_003E8__locals27.pawn.IsQuestLodger() && CS_0024_003C_003E8__locals27.pawn.genes != null && (CS_0024_003C_003E8__locals27.pawn.IsColonistPlayerControlled || CS_0024_003C_003E8__locals27.pawn.IsPrisonerOfColony || CS_0024_003C_003E8__locals27.pawn.IsSlaveOfColony || (CS_0024_003C_003E8__locals27.pawn.IsColonySubhuman && CS_0024_003C_003E8__locals27.pawn.IsGhoul)))
									{
										int num2 = GeneUtility.MetabolismAfterImplanting(CS_0024_003C_003E8__locals27.pawn, CS_0024_003C_003E8__locals31.geneSet);
										if (num2 < GeneTuning.BiostatRange.TrueMin)
										{
											list.Add(new FloatMenuOption(string.Concat(CS_0024_003C_003E8__locals27.pawn.LabelShortCap + ": " + "ResultingMetTooLow".Translate() + " (", num2.ToString(), ")"), null, CS_0024_003C_003E8__locals27.pawn, Color.white));
										}
										else if (CS_0024_003C_003E8__locals31.PawnIdeoDisallowsImplanting(CS_0024_003C_003E8__locals27.pawn))
										{
											list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals27.pawn.LabelShortCap + ": " + "IdeoligionForbids".Translate(), null, CS_0024_003C_003E8__locals27.pawn, Color.white));
										}
										else
										{
											list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals27.pawn.LabelShortCap + ", " + CS_0024_003C_003E8__locals27.pawn.genes.XenotypeLabelCap, delegate
											{
												CS_0024_003C_003E8__locals27._003C_003E4__this.SetTargetPawn(CS_0024_003C_003E8__locals27.pawn);
											}, CS_0024_003C_003E8__locals27.pawn, Color.white));
										}
									}
								}
								if (!list.Any())
								{
									list.Add(new FloatMenuOption("NoImplantablePawns".Translate(), null));
								}
								Find.WindowStack.Add(new FloatMenu(list));
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CancelImplanting".Translate(),
						defaultDesc = "CancelImplantingDesc".Translate(CS_0024_003C_003E8__locals31.targetPawn.Named("PAWN")),
						icon = CancelIcon,
						action = delegate
						{
							Bill bill = CS_0024_003C_003E8__locals31.targetPawn.BillStack?.Bills.FirstOrDefault((Bill x) => x.xenogerm == CS_0024_003C_003E8__locals31);
							if (bill != null)
							{
								CS_0024_003C_003E8__locals31.targetPawn.BillStack.Delete(bill);
							}
						}
					};
					_003C_003E1__state = 3;
					return true;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__16(0)
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

	private Pawn targetPawn;

	public string xenotypeName;

	public XenotypeIconDef iconDef;

	private static readonly CachedTexture ImplantTex = new CachedTexture("UI/Gizmos/ImplantGenes");

	private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	public override string LabelNoCount
	{
		get
		{
			if (xenotypeName.NullOrEmpty())
			{
				return base.LabelNoCount;
			}
			return "NamedXenogerm".Translate(xenotypeName.Named("NAME"));
		}
	}

	private int RequiredMedicineForImplanting
	{
		get
		{
			int num = 0;
			for (int i = 0; i < RecipeDefOf.ImplantXenogerm.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = RecipeDefOf.ImplantXenogerm.ingredients[i];
				if (ingredientCount.filter.Allows(ThingDefOf.MedicineIndustrial))
				{
					num += (int)ingredientCount.GetBaseCount();
				}
			}
			return num;
		}
	}

	public override void Notify_DebugSpawned()
	{
		geneSet = GeneUtility.GenerateGeneSet(null);
		xenotypeName = "Unique".Translate();
		iconDef = XenotypeIconDefOf.Basic;
	}

	public override void PostMake()
	{
		if (!ModLister.CheckBiotech("xenogerm"))
		{
			Destroy();
		}
		else
		{
			base.PostMake();
		}
	}

	public void SetTargetPawn(Pawn newTarget)
	{
		int trueMax = HediffDefOf.XenogerminationComa.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.TrueMax;
		TaggedString text = "ImplantXenogermWarningDesc".Translate(newTarget.Named("PAWN"), trueMax.ToStringTicksToPeriod().Named("COMADURATION"));
		if (newTarget.genes.Xenogenes.Any())
		{
			text += "\n\n" + "ImplantXenogermWarningOverwriteXenogenes".Translate(newTarget.Named("PAWN"), newTarget.genes.XenotypeLabelCap.Named("XENOTYPE"), newTarget.genes.Xenogenes.Select((Gene x) => x.LabelCap).ToLineList("  - ").Named("XENOGENES"));
		}
		int num = GeneUtility.MetabolismAfterImplanting(newTarget, geneSet);
		text += "\n\n" + "ImplantXenogermWarningNewMetabolism".Translate(newTarget.Named("PAWN"), num.Named("MET"), GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate(num).ToStringPercent().Named("CONSUMPTION"));
		text += "\n\n" + "WouldYouLikeToContinue".Translate();
		Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, delegate
		{
			Bill bill = targetPawn?.BillStack?.Bills?.FirstOrDefault((Bill x) => x.xenogerm == this);
			if (bill != null)
			{
				targetPawn.BillStack.Delete(bill);
			}
			HealthCardUtility.CreateSurgeryBill(newTarget, RecipeDefOf.ImplantXenogerm, null).xenogerm = this;
			targetPawn = newTarget;
			SendImplantationLetter(newTarget);
		}, destructive: true));
	}

	private void SendImplantationLetter(Pawn targetPawn)
	{
		string arg = string.Empty;
		if (!targetPawn.InBed() && !targetPawn.Map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(targetPawn, x.def) && ((Building_Bed)x).Medical))
		{
			arg = "XenogermOrderedImplantedBedNeeded".Translate(targetPawn.Named("PAWN"));
		}
		int requiredMedicineForImplanting = RequiredMedicineForImplanting;
		string arg2 = string.Empty;
		if (targetPawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine).Sum((Thing x) => x.stackCount) < requiredMedicineForImplanting)
		{
			arg2 = "XenogermOrderedImplantedMedicineNeeded".Translate(requiredMedicineForImplanting.Named("MEDICINENEEDED"));
		}
		Find.LetterStack.ReceiveLetter("LetterLabelXenogermOrderedImplanted".Translate(), "LetterXenogermOrderedImplanted".Translate(targetPawn.Named("PAWN"), requiredMedicineForImplanting.Named("MEDICINENEEDED"), arg.Named("BEDINFO"), arg2.Named("MEDICINEINFO")), LetterDefOf.NeutralEvent);
	}

	public void Initialize(List<Genepack> genepacks, string xenotypeName, XenotypeIconDef iconDef)
	{
		this.xenotypeName = xenotypeName;
		this.iconDef = iconDef;
		geneSet = new GeneSet();
		for (int i = 0; i < genepacks.Count; i++)
		{
			if (genepacks[i].GeneSet != null)
			{
				List<GeneDef> genesListForReading = genepacks[i].GeneSet.GenesListForReading;
				for (int j = 0; j < genesListForReading.Count; j++)
				{
					geneSet.AddGene(genesListForReading[j]);
				}
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__14))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__14(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public bool PawnIdeoDisallowsImplanting(Pawn selPawn)
	{
		if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
		{
			return false;
		}
		if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.PropagateBloodfeederGene, selPawn) && base.GeneSet.GenesListForReading.Any((GeneDef x) => x == GeneDefOf.Bloodfeeder))
		{
			return true;
		}
		if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.BecomeNonPreferredXenotype, selPawn))
		{
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__16))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void Notify_BillRemoved()
	{
		targetPawn = null;
	}

	public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
	{
		if (!target.IsValid || target.Pawn == null)
		{
			return false;
		}
		if (target.Pawn.IsQuestLodger())
		{
			if (showMessages)
			{
				Messages.Message("MessageCannotImplantInTempFactionMembers".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		if (!target.Pawn.IsColonist && !target.Pawn.IsPrisonerOfColony && !target.Pawn.IsSlaveOfColony)
		{
			if (showMessages)
			{
				Messages.Message("MessageCanOnlyTargetColonistsPrisonersAndSlaves".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return true;
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (targetPawn != null && targetPawn.Map == base.Map)
		{
			GenDraw.DrawLineBetween(this.TrueCenter(), targetPawn.TrueCenter());
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref targetPawn, "targetPawn");
		Scribe_Values.Look(ref xenotypeName, "xenotypeName");
		Scribe_Defs.Look(ref iconDef, "iconDef");
		if (Scribe.mode == LoadSaveMode.PostLoadInit && iconDef == null)
		{
			iconDef = XenotypeIconDefOf.Basic;
		}
	}
}
