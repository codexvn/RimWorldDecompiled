using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Precept_Apparel : Precept
{
	[CompilerGenerated]
	private sealed class _003CEditFloatMenuOptions_003Ed__19 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Apparel _003C_003E4__this;

		private FloatMenuOption _003CsetFemale_003E5__2;

		private FloatMenuOption _003CsetNone_003E5__3;

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
		public _003CEditFloatMenuOptions_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CsetFemale_003E5__2 = null;
			_003CsetNone_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Precept_Apparel CS_0024_003C_003E8__locals11 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				string text = "SetTargetGender".Translate() + ": ";
				FloatMenuOption floatMenuOption = new FloatMenuOption(text + Gender.Male.GetLabel().CapitalizeFirst(), delegate
				{
					CS_0024_003C_003E8__locals11.overrideGender = Gender.Male;
					CS_0024_003C_003E8__locals11.InitDescription();
				});
				_003CsetFemale_003E5__2 = new FloatMenuOption(text + Gender.Female.GetLabel().CapitalizeFirst(), delegate
				{
					CS_0024_003C_003E8__locals11.overrideGender = Gender.Female;
					CS_0024_003C_003E8__locals11.InitDescription();
				});
				_003CsetNone_003E5__3 = new FloatMenuOption(text + "Everyone".Translate().CapitalizeFirst(), delegate
				{
					CS_0024_003C_003E8__locals11.overrideGender = Gender.None;
					CS_0024_003C_003E8__locals11.InitDescription();
				});
				switch (CS_0024_003C_003E8__locals11.TargetGender)
				{
				case Gender.None:
					_003C_003E2__current = floatMenuOption;
					_003C_003E1__state = 1;
					return true;
				case Gender.Male:
					_003C_003E2__current = _003CsetFemale_003E5__2;
					_003C_003E1__state = 3;
					return true;
				case Gender.Female:
					_003C_003E2__current = floatMenuOption;
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_01bf;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CsetFemale_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				goto IL_01bf;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CsetNone_003E5__3;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				goto IL_01bf;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CsetNone_003E5__3;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				goto IL_01bf;
			case 7:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_01bf:
				if (CS_0024_003C_003E8__locals11.def.apparelPreceptSwapDef != null)
				{
					_003C_003E2__current = new FloatMenuOption("SetApparelType".Translate() + ": " + CS_0024_003C_003E8__locals11.def.apparelPreceptSwapDef.LabelCap, delegate
					{
						CS_0024_003C_003E8__locals11.def = CS_0024_003C_003E8__locals11.def.apparelPreceptSwapDef;
					});
					_003C_003E1__state = 7;
					return true;
				}
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CEditFloatMenuOptions_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEditFloatMenuOptions_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public ThingDef apparelDef;

	private Gender targetGender;

	private Gender? overrideGender;

	private static List<ThingDef> usedThingDefsTmp = new List<ThingDef>();

	public override bool SortByImpact => false;

	public override bool CanRegenerate => def.Worker.ThingDefs.Count() > 1;

	public override string TipLabel => UIInfoFirstLine + ": " + def.LabelCap;

	public override string UIInfoFirstLine
	{
		get
		{
			if (apparelDef != null)
			{
				return apparelDef.LabelCap;
			}
			return base.UIInfoFirstLine;
		}
	}

	public override string UIInfoSecondLine
	{
		get
		{
			if (TargetGender == Gender.None)
			{
				return "Everyone".Translate() + ": " + base.UIInfoSecondLine;
			}
			return TargetGender.GetLabel().CapitalizeFirst() + ": " + base.UIInfoSecondLine;
		}
	}

	public Gender TargetGender => overrideGender ?? targetGender;

	public override void Init(Ideo ideo, FactionDef generatingFor = null)
	{
		base.Init(ideo, generatingFor);
		for (int i = 0; i < def.comps.Count; i++)
		{
			if (def.comps[i] is PreceptComp_Apparel preceptComp_Apparel)
			{
				targetGender = preceptComp_Apparel.AffectedGender(ideo);
				break;
			}
		}
		InitDescription();
	}

	public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
	{
		IEnumerable<PreceptThingChance> enumerable = null;
		if (!def.canUseAlreadyUsedThingDef)
		{
			usedThingDefsTmp.Clear();
			foreach (Precept item in ideo.PreceptsListForReading)
			{
				if (item is Precept_Apparel precept_Apparel)
				{
					usedThingDefsTmp.Add(precept_Apparel.apparelDef);
				}
			}
			enumerable = from bd in def.Worker.ThingDefsForIdeo(ideo, generatingFor)
				where !usedThingDefsTmp.Contains(bd.def)
				select bd;
		}
		else
		{
			enumerable = def.Worker.ThingDefsForIdeo(ideo, generatingFor);
		}
		if (enumerable.Any() && enumerable.TryRandomElementByWeight((PreceptThingChance d) => d.chance, out var result))
		{
			apparelDef = result.def;
		}
		else
		{
			apparelDef = def.Worker.ThingDefsForIdeo(ideo, generatingFor).RandomElementByWeight((PreceptThingChance d) => d.chance).def;
			Log.Warning("Failed to generate a unique apparel for " + ideo.name);
		}
		if (ideo.SupremeGender == Gender.None && Rand.Value < 0.5f)
		{
			overrideGender = ((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
		}
		else
		{
			overrideGender = null;
		}
		base.Regenerate(ideo);
	}

	public override void DrawIcon(Rect rect)
	{
		Widgets.ThingIcon(rect, apparelDef, null, ideo.GetStyleFor(apparelDef), 1f, ideo.ApparelColor, null);
	}

	[IteratorStateMachine(typeof(_003CEditFloatMenuOptions_003Ed__19))]
	public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEditFloatMenuOptions_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void InitDescription()
	{
		string arg = ((TargetGender != 0) ? TargetGender.GetLabel() : ((string)"All".Translate()));
		descOverride = def.description.Formatted(arg.Named("GENDER"), NamedArgumentUtility.Named(apparelDef, "APPAREL")).CapitalizeFirst();
	}

	public override bool CompatibleWith(Precept other)
	{
		if (other.def.prefersNudity && (TargetGender == Gender.None || TargetGender == other.def.genderPrefersNudity))
		{
			return false;
		}
		if (other is Precept_Apparel precept_Apparel && def == other.def && (TargetGender == precept_Apparel.TargetGender || TargetGender == Gender.None || precept_Apparel.TargetGender == Gender.None) && !ApparelUtility.CanWearTogether(apparelDef, precept_Apparel.apparelDef, BodyDefOf.Human))
		{
			return false;
		}
		return base.CompatibleWith(other);
	}

	public override bool GetPlayerWarning(out string shortText, out string description)
	{
		foreach (Precept_Role item in ideo.RolesListForReading)
		{
			if ((TargetGender != 0 && item.restrictToSupremeGender && ideo.SupremeGender != 0 && ideo.SupremeGender != TargetGender) || item.ApparelRequirements == null)
			{
				continue;
			}
			foreach (PreceptApparelRequirement apparelRequirement in item.ApparelRequirements)
			{
				IEnumerable<ThingDef> source = apparelRequirement.requirement.AllRequiredApparel(ideo.SupremeGender);
				if (source.Any((ThingDef ap) => ap != apparelDef && !ApparelUtility.CanWearTogether(apparelDef, ap, BodyDefOf.Human)))
				{
					shortText = "MessageIdeoWarnRoleApparelOverlapsDesiredApparel".Translate(apparelDef.label.Named("DESIREDAPPAREL"), item.def.label.Named("ROLE"));
					description = "DescriptionIdeoWarnRoleApparelOverlapsDesiredApparel".Translate(apparelDef.label.Named("DESIREDAPPAREL"), item.def.label.Named("ROLE"), string.Join(", ", source.Select((ThingDef ap) => ap.label)).CapitalizeFirst().Named("ROLEAPPAREL")).CapitalizeFirst();
					return true;
				}
			}
		}
		shortText = null;
		description = null;
		return false;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref apparelDef, "apparelDef");
		Scribe_Values.Look(ref targetGender, "targetGender", Gender.None);
		Scribe_Values.Look(ref overrideGender, "overrideGender", null);
	}

	public override void CopyTo(Precept other)
	{
		base.CopyTo(other);
		Precept_Apparel obj = (Precept_Apparel)other;
		obj.apparelDef = apparelDef;
		obj.targetGender = targetGender;
		obj.overrideGender = overrideGender;
	}
}
