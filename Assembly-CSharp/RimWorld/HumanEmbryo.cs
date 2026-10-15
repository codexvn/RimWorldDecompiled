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
public class HumanEmbryo : GeneSetHolderBase
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0
	{
		public List<FloatMenuOption> surrogateOptions;

		internal void _003CGetGizmos_003Eb__0()
		{
			Find.WindowStack.Add(new FloatMenu(surrogateOptions));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_1
	{
		public Building_GrowthVat bestVat;

		public HumanEmbryo _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__1()
		{
			bestVat.SelectEmbryo(_003C_003E4__this);
			_003C_003E4__this.implantTarget = bestVat;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HumanEmbryo _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__11(int _003C_003E1__state)
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
				HumanEmbryo CS_0024_003C_003E8__locals22 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((GeneSetHolderBase)CS_0024_003C_003E8__locals22).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass11_1 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass11_1
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals22,
						bestVat = CS_0024_003C_003E8__locals22.BestAvailableGrowthVat()
					};
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "InsertGrowthVatLabel".Translate() + "...",
						defaultDesc = "InsertEmbryoGrowthVatDesc".Translate(540000.ToStringTicksToPeriod()).Resolve(),
						icon = Building_GrowthVat.InsertEmbryoIcon.Texture,
						activateSound = SoundDefOf.Tick_Tiny,
						action = delegate
						{
							CS_0024_003C_003E8__locals8.bestVat.SelectEmbryo(CS_0024_003C_003E8__locals8._003C_003E4__this);
							CS_0024_003C_003E8__locals8._003C_003E4__this.implantTarget = CS_0024_003C_003E8__locals8.bestVat;
						}
					};
					if (CS_0024_003C_003E8__locals8.bestVat == null)
					{
						command_Action.Disable("ImplantDisabledNoVats".Translate());
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 3;
					return true;
				}
				case 3:
					_003C_003E1__state = -1;
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!Find.Storyteller.difficulty.ChildrenAllowed)
					{
						return false;
					}
					if (CS_0024_003C_003E8__locals22.MapHeld == null)
					{
						break;
					}
					CS_0024_003C_003E8__locals22.EnsureImplantTargetValid();
					if (CS_0024_003C_003E8__locals22.implantTarget == null)
					{
						_003C_003Ec__DisplayClass11_0 CS_0024_003C_003E8__locals17 = new _003C_003Ec__DisplayClass11_0
						{
							surrogateOptions = new List<FloatMenuOption>()
						};
						foreach (Pawn item in CS_0024_003C_003E8__locals22.MapHeld.mapPawns.FreeColonistsAndPrisonersSpawned)
						{
							FloatMenuOption floatMenuOption = CS_0024_003C_003E8__locals22.CanImplantFloatOption(item, pawnAsLabel: true);
							if (floatMenuOption != null)
							{
								CS_0024_003C_003E8__locals17.surrogateOptions.Add(floatMenuOption);
							}
						}
						Command_Action command_Action2 = new Command_Action
						{
							defaultLabel = "ImplantLabel".Translate() + "...",
							defaultDesc = "ImplantDescription".Translate(),
							icon = ImplantIcon.Texture,
							action = delegate
							{
								Find.WindowStack.Add(new FloatMenu(CS_0024_003C_003E8__locals17.surrogateOptions));
							}
						};
						if (CS_0024_003C_003E8__locals17.surrogateOptions.Count == 0)
						{
							command_Action2.Disable("ImplantDisabledNoWomen".Translate());
						}
						_003C_003E2__current = command_Action2;
						_003C_003E1__state = 2;
						return true;
					}
					if (CS_0024_003C_003E8__locals22.Spawned)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "ImplantEmbryoCancel".Translate(),
							defaultDesc = "ImplantEmbryoCancel".Translate(),
							icon = CancelImplantIcon,
							action = delegate
							{
								CS_0024_003C_003E8__locals22.ImplantPawnValid(cancel: true);
								CS_0024_003C_003E8__locals22.ImplantVatValid(cancel: true);
								CS_0024_003C_003E8__locals22.implantTarget = null;
							},
							activateSound = SoundDefOf.Designate_Cancel
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__11(0)
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

	private const int MinImplantationAge = 16;

	public static readonly CachedTexture ImplantIcon = new CachedTexture("UI/Gizmos/ImplantEmbryo");

	private static readonly Texture2D CancelImplantIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	public Thing implantTarget;

	private List<Building_GrowthVat> tmpEligibleVats = new List<Building_GrowthVat>();

	public Pawn Mother => this.TryGetComp<CompHasPawnSources>().pawnSources?.FirstOrDefault((Pawn p) => p.gender == Gender.Female);

	public Pawn Father => this.TryGetComp<CompHasPawnSources>().pawnSources?.FirstOrDefault((Pawn p) => p.gender == Gender.Male);

	protected override string InspectGeneDescription => "InspectGenesEmbryoDesc".Translate();

	public override void PostMake()
	{
		if (!ModLister.CheckBiotech("human embryo"))
		{
			Destroy();
		}
		else
		{
			base.PostMake();
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__11))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void EnsureImplantTargetValid()
	{
		if (implantTarget == null)
		{
			return;
		}
		if (implantTarget is Pawn pawn)
		{
			if (pawn.Destroyed || !ImplantPawnValid(cancel: false))
			{
				implantTarget = null;
			}
		}
		else if (implantTarget is Building_GrowthVat building_GrowthVat && (building_GrowthVat.Destroyed || !ImplantVatValid(cancel: false)))
		{
			implantTarget = null;
		}
	}

	private bool ImplantPawnValid(bool cancel)
	{
		if (implantTarget is Pawn pawn)
		{
			foreach (Bill item in pawn.BillStack)
			{
				if (!(item is Bill_Medical bill_Medical) || bill_Medical.uniqueRequiredIngredients.NullOrEmpty())
				{
					continue;
				}
				foreach (Thing uniqueRequiredIngredient in bill_Medical.uniqueRequiredIngredients)
				{
					if (this == uniqueRequiredIngredient)
					{
						if (cancel)
						{
							bill_Medical.billStack.Delete(bill_Medical);
							implantTarget = null;
							return false;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool ImplantVatValid(bool cancel)
	{
		if (implantTarget is Building_GrowthVat building_GrowthVat)
		{
			if (cancel)
			{
				building_GrowthVat.selectedEmbryo = null;
				implantTarget = null;
				return false;
			}
			return building_GrowthVat.selectedEmbryo == this;
		}
		return false;
	}

	private Building_GrowthVat BestAvailableGrowthVat()
	{
		if (base.MapHeld == null)
		{
			return null;
		}
		List<Building> list = base.MapHeld.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GrowthVat);
		tmpEligibleVats.Clear();
		foreach (Building item in list)
		{
			if (item is Building_GrowthVat { Working: false, selectedEmbryo: null, SelectedPawn: null } building_GrowthVat)
			{
				tmpEligibleVats.Add(building_GrowthVat);
			}
		}
		if (tmpEligibleVats.NullOrEmpty())
		{
			return null;
		}
		tmpEligibleVats.SortBy((Building_GrowthVat v) => (!v.PowerOn) ? 1 : 0, (Building_GrowthVat v) => (base.PositionHeld - v.Position).LengthHorizontal);
		return tmpEligibleVats[0];
	}

	private FloatMenuOption CanImplantFloatOption(Pawn pawn, bool pawnAsLabel)
	{
		AcceptanceReport acceptanceReport = CanImplantReport(pawn);
		if (acceptanceReport.Accepted)
		{
			return new FloatMenuOption(pawnAsLabel ? pawn.LabelShort : "ImplantLabel".Translate().ToString(), delegate
			{
				float num = PregnancyUtility.PregnancyChanceImplantEmbryo(pawn);
				if (num < 1f)
				{
					Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ImplantSurgeryConfirm".Translate(Label, pawn.LabelShort, num.ToStringPercent()), delegate
					{
						HealthCardUtility.CreateSurgeryBill(pawn, RecipeDefOf.ImplantEmbryo, null, new List<Thing> { this });
						implantTarget = pawn;
					}, destructive: true);
					Find.WindowStack.Add(window);
				}
				else
				{
					HealthCardUtility.CreateSurgeryBill(pawn, RecipeDefOf.ImplantEmbryo, null, new List<Thing> { this });
					implantTarget = pawn;
				}
			}, pawn, Color.white);
		}
		if (!acceptanceReport.Reason.NullOrEmpty())
		{
			return new FloatMenuOption(pawnAsLabel ? "DisabledOption".Translate(pawn.LabelShort, acceptanceReport.Reason).ToString() : ("ImplantCannotFloatOption".Translate().ToString() + ": " + acceptanceReport.Reason), null, pawn, Color.white);
		}
		return null;
	}

	private AcceptanceReport CanImplantReport(Pawn pawn)
	{
		if (pawn.gender != Gender.Female)
		{
			return false;
		}
		if (pawn.IsQuestLodger())
		{
			return false;
		}
		HashSet<Pawn> hashSet = new HashSet<Pawn>();
		base.Map.reservationManager.ReserversOf(pawn, hashSet);
		if (hashSet.Any())
		{
			Pawn pawn2 = hashSet.First();
			return "ReservedBy".Translate(pawn2.LabelShort, pawn2);
		}
		if (pawn.BillStack.Bills.Any((Bill b) => b.recipe == RecipeDefOf.ImplantEmbryo))
		{
			return "CannotImplantingOtherEmbryo".Translate();
		}
		if (pawn.ageTracker.AgeBiologicalYears < 16)
		{
			return "CannotMustBeAge".Translate(16).CapitalizeFirst();
		}
		if (pawn.health.hediffSet.HasHediff(HediffDefOf.PregnantHuman))
		{
			return "CannotPregnant".Translate();
		}
		if (pawn.Sterile())
		{
			return "CannotSterile".Translate();
		}
		return true;
	}

	public bool TryPopulateGenes()
	{
		bool success = true;
		if (geneSet == null)
		{
			geneSet = PregnancyUtility.GetInheritedGeneSet(Father, Mother, out success);
		}
		return success;
	}

	public override void Notify_DebugSpawned()
	{
		CompHasPawnSources compHasPawnSources = this.TryGetComp<CompHasPawnSources>();
		if (base.Map.mapPawns.AllPawns.Where((Pawn x) => x.RaceProps.Humanlike && x.gender == Gender.Male).TryRandomElement(out var result))
		{
			compHasPawnSources.AddSource(result);
		}
		if (base.Map.mapPawns.AllPawns.Where((Pawn x) => x.RaceProps.Humanlike && x.gender == Gender.Female).TryRandomElement(out var result2))
		{
			compHasPawnSources.AddSource(result2);
		}
		geneSet = PregnancyUtility.GetInheritedGeneSet(Father, Mother);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref implantTarget, "implantTarget");
		if (Scribe.mode == LoadSaveMode.PostLoadInit && geneSet == null)
		{
			TryPopulateGenes();
		}
	}
}
