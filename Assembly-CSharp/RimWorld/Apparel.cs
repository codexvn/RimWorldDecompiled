using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Apparel : ThingWithComps
{
	[CompilerGenerated]
	private sealed class _003CGetWornGizmos_003Ed__30 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Apparel _003C_003E4__this;

		private List<ThingComp> _003Ccomps_003E5__2;

		private int _003Ci_003E5__3;

		private IEnumerator<Gizmo> _003C_003E7__wrap3;

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
		public _003CGetWornGizmos_003Ed__30(int _003C_003E1__state)
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
			_003Ccomps_003E5__2 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Apparel apparel = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_0090;
				}
				_003C_003E1__state = -1;
				_003Ccomps_003E5__2 = apparel.AllComps;
				_003Ci_003E5__3 = 0;
				goto IL_00bc;
				IL_0090:
				if (_003C_003E7__wrap3.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap3.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap3 = null;
				_003Ci_003E5__3++;
				goto IL_00bc;
				IL_00bc:
				if (_003Ci_003E5__3 < _003Ccomps_003E5__2.Count)
				{
					ThingComp thingComp = _003Ccomps_003E5__2[_003Ci_003E5__3];
					_003C_003E7__wrap3 = thingComp.CompGetWornGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0090;
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
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
			_003CGetWornGizmos_003Ed__30 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetWornGizmos_003Ed__30(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__31 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Apparel _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__31(int _003C_003E1__state)
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
				Apparel CS_0024_003C_003E8__locals2 = _003C_003E4__this;
				RoyalTitleDef royalTitleDef;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)CS_0024_003C_003E8__locals2).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0074;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0074;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0074:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					royalTitleDef = (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
						where t.requiredApparel != null && t.requiredApparel.Any((ApparelRequirement req) => req.ApparelMeetsRequirement(CS_0024_003C_003E8__locals2.def, allowUnmatched: false))
						orderby t.seniority descending
						select t).FirstOrDefault();
					if (royalTitleDef != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Apparel_MaxSatisfiedTitle_Desc".Translate(), 2752, null, new Dialog_InfoCard.Hyperlink[1]
						{
							new Dialog_InfoCard.Hyperlink(royalTitleDef)
						});
						_003C_003E1__state = 2;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllAbilitiesForReading_003Ed__17 : IEnumerable<Ability>, IEnumerable, IEnumerator<Ability>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Ability _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Apparel _003C_003E4__this;

		private List<Ability>.Enumerator _003C_003E7__wrap1;

		Ability IEnumerator<Ability>.Current
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
		public _003Cget_AllAbilitiesForReading_003Ed__17(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Ability>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Apparel apparel = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (apparel.abilities == null && apparel.def.apparel.abilities != null)
					{
						apparel.FillOrUpdateAbilities();
					}
					if (apparel.abilities == null)
					{
						return false;
					}
					_003C_003E7__wrap1 = apparel.abilities.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Ability current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Ability>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Ability> IEnumerable<Ability>.GetEnumerator()
		{
			_003Cget_AllAbilitiesForReading_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllAbilitiesForReading_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Ability>)this).GetEnumerator();
		}
	}

	private bool wornByCorpseInt;

	private List<Ability> abilities;

	public Pawn Wearer
	{
		get
		{
			if (!(base.ParentHolder is Pawn_ApparelTracker pawn_ApparelTracker))
			{
				return null;
			}
			return pawn_ApparelTracker.pawn;
		}
	}

	public bool WornByCorpse
	{
		get
		{
			return wornByCorpseInt;
		}
		set
		{
			wornByCorpseInt = value;
		}
	}

	public string WornGraphicPath
	{
		get
		{
			if (StyleDef != null && !StyleDef.wornGraphicPath.NullOrEmpty())
			{
				return StyleDef.wornGraphicPath;
			}
			if (!def.apparel.wornGraphicPaths.NullOrEmpty())
			{
				return def.apparel.wornGraphicPaths[thingIDNumber % def.apparel.wornGraphicPaths.Count];
			}
			return def.apparel.wornGraphicPath;
		}
	}

	public override string DescriptionDetailed
	{
		get
		{
			string text = base.DescriptionDetailed;
			if (WornByCorpse)
			{
				text += "\n" + "WasWornByCorpse".Translate();
			}
			return text;
		}
	}

	public override Color DrawColor
	{
		get
		{
			Color color = ((StyleDef == null || !(StyleDef.color != default(Color))) ? base.DrawColor : StyleDef.color);
			if (WornByCorpse)
			{
				color = PawnRenderUtility.GetRottenColor(color);
			}
			Pawn wearer = Wearer;
			if (wearer != null)
			{
				Pawn_DrawTracker drawer = wearer.Drawer;
				if (drawer != null)
				{
					PawnRenderer renderer = drawer.renderer;
					if (renderer != null)
					{
						Color? statueColor = renderer.StatueColor;
						if (statueColor.HasValue)
						{
							Color valueOrDefault = statueColor.GetValueOrDefault();
							color = valueOrDefault;
						}
					}
				}
			}
			return color;
		}
	}

	public Color? DesiredColor
	{
		get
		{
			return GetComp<CompColorable>()?.DesiredColor;
		}
		set
		{
			CompColorable comp = GetComp<CompColorable>();
			if (comp != null)
			{
				comp.DesiredColor = value;
			}
			else
			{
				Log.Error("Tried setting Apparel.DesiredColor without having CompColorable comp!");
			}
		}
	}

	public IEnumerable<Ability> AllAbilitiesForReading
	{
		[IteratorStateMachine(typeof(_003Cget_AllAbilitiesForReading_003Ed__17))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllAbilitiesForReading_003Ed__17(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override string GetInspectStringLowPriority()
	{
		string text = base.GetInspectStringLowPriority();
		if (StyleDef != null)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "VariantOf".Translate().CapitalizeFirst() + ": " + def.LabelCap;
		}
		if (ModsConfig.BiotechActive)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "WearableBy".Translate() + ": " + def.apparel.developmentalStageFilter.ToCommaList().CapitalizeFirst();
		}
		return text;
	}

	public bool PawnCanWear(Pawn pawn, bool ignoreGender = false)
	{
		if (!def.IsApparel)
		{
			return false;
		}
		if (!def.apparel.PawnCanWear(pawn, ignoreGender))
		{
			return false;
		}
		return true;
	}

	public Color GetColorIgnoringTainted()
	{
		if (StyleDef != null && StyleDef.color != default(Color))
		{
			return StyleDef.color;
		}
		return base.DrawColor;
	}

	public void Notify_PawnKilled()
	{
		if (def.apparel.careIfWornByCorpse)
		{
			wornByCorpseInt = true;
		}
		foreach (ThingComp allComp in base.AllComps)
		{
			allComp.Notify_WearerDied();
		}
	}

	public void Notify_PawnResurrected(Pawn pawn)
	{
		if (!pawn.IsMutant || !pawn.mutant.Def.isConsideredCorpse)
		{
			wornByCorpseInt = false;
		}
	}

	public override void Notify_ColorChanged()
	{
		if (Wearer != null)
		{
			Wearer.apparel.Notify_ApparelChanged();
		}
		base.Notify_ColorChanged();
	}

	public override void Notify_Equipped(Pawn pawn)
	{
		base.Notify_Equipped(pawn);
		foreach (Ability item in AllAbilitiesForReading)
		{
			item.pawn = pawn;
			item.verb.caster = pawn;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref wornByCorpseInt, "wornByCorpse", defaultValue: false);
		Scribe_Collections.Look(ref abilities, "abilities", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			FillOrUpdateAbilities();
		}
	}

	public void FillOrUpdateAbilities()
	{
		if (def.apparel.abilities == null)
		{
			return;
		}
		if (abilities == null)
		{
			abilities = new List<Ability>();
			foreach (AbilityDef ability in def.apparel.abilities)
			{
				abilities.Add(AbilityUtility.MakeAbility(ability, Wearer));
			}
		}
		foreach (Ability ability2 in abilities)
		{
			ability2.pawn = Wearer;
			ability2.verb.caster = Wearer;
		}
	}

	public virtual void DrawWornExtras()
	{
		List<ThingComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			allComps[i].CompDrawWornExtras();
		}
	}

	public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
	{
		List<ThingComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			allComps[i].PostPreApplyDamage(ref dinfo, out var absorbed);
			if (absorbed)
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool AllowVerbCast(Verb verb)
	{
		List<ThingComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			if (!allComps[i].CompAllowVerbCast(verb))
			{
				return false;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetWornGizmos_003Ed__30))]
	public virtual IEnumerable<Gizmo> GetWornGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetWornGizmos_003Ed__30(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__31))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (WornByCorpse)
		{
			if (text.Length > 0)
			{
				text += "\n";
			}
			text += "WasWornByCorpse".Translate();
		}
		return text;
	}

	public virtual float GetSpecialApparelScoreOffset()
	{
		float num = 0f;
		List<ThingComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			num += allComps[i].CompGetSpecialApparelScoreOffset();
		}
		return num;
	}

	[DebugOutput]
	private static void ApparelValidLifeStages()
	{
		List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
		list.Add(new TableDataGetter<ThingDef>("name", (ThingDef t) => t.LabelCap));
		list.Add(new TableDataGetter<ThingDef>("valid life stage", (ThingDef t) => t.apparel.developmentalStageFilter.ToCommaList()));
		DebugTables.MakeTablesDialog(DefDatabase<ThingDef>.AllDefs.Where((ThingDef t) => t.IsApparel), list.ToArray());
	}
}
