using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;

namespace Verse;

public class HediffWithComps : Hediff
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HediffWithComps _003C_003E4__this;

		private int _003Ci_003E5__2;

		private IEnumerator<Gizmo> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__15(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				HediffWithComps hediffWithComps = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_0087;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00b3;
				IL_00b3:
				if (_003Ci_003E5__2 < hediffWithComps.comps.Count)
				{
					IEnumerable<Gizmo> enumerable = hediffWithComps.comps[_003Ci_003E5__2].CompGetGizmos();
					if (enumerable != null)
					{
						_003C_003E7__wrap2 = enumerable.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0087;
					}
					goto IL_00a1;
				}
				return false;
				IL_0087:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				goto IL_00a1;
				IL_00a1:
				_003Ci_003E5__2++;
				goto IL_00b3;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CGetGizmos_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__15(0)
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

	public List<HediffComp> comps = new List<HediffComp>();

	public override string LabelBase
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < comps?.Count; i++)
			{
				string compLabelPrefix = comps[i].CompLabelPrefix;
				if (!compLabelPrefix.NullOrEmpty())
				{
					stringBuilder.Append(compLabelPrefix);
					stringBuilder.Append(" ");
				}
			}
			stringBuilder.Append(base.LabelBase);
			return stringBuilder.ToString();
		}
	}

	public override string LabelInBrackets
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.LabelInBrackets);
			for (int i = 0; i < comps?.Count; i++)
			{
				string compLabelInBracketsExtra = comps[i].CompLabelInBracketsExtra;
				if (!compLabelInBracketsExtra.NullOrEmpty())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(compLabelInBracketsExtra);
				}
			}
			return stringBuilder.ToString();
		}
	}

	public override bool ShouldRemove
	{
		get
		{
			if (comps != null)
			{
				for (int i = 0; i < comps.Count; i++)
				{
					if (comps[i].CompShouldRemove)
					{
						return true;
					}
				}
			}
			return base.ShouldRemove;
		}
	}

	public override bool Visible
	{
		get
		{
			if (comps != null)
			{
				for (int i = 0; i < comps.Count; i++)
				{
					if (comps[i].CompDisallowVisible())
					{
						return false;
					}
				}
			}
			return base.Visible;
		}
	}

	public override string TipStringExtra
	{
		get
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.TipStringExtra);
			if (comps != null)
			{
				for (int i = 0; i < comps.Count; i++)
				{
					string compTipStringExtra = comps[i].CompTipStringExtra;
					if (!compTipStringExtra.NullOrEmpty())
					{
						if (stringBuilder.Length > 0 && !flag)
						{
							stringBuilder.AppendLine();
							flag = true;
						}
						stringBuilder.AppendLine(compTipStringExtra);
					}
				}
			}
			return stringBuilder.ToString();
		}
	}

	public override string Description
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder(base.Description);
			for (int i = 0; i < comps?.Count; i++)
			{
				string compDescriptionExtra = comps[i].CompDescriptionExtra;
				if (!compDescriptionExtra.NullOrEmpty())
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(compDescriptionExtra);
				}
			}
			return stringBuilder.ToString();
		}
	}

	public override TextureAndColor StateIcon
	{
		get
		{
			for (int i = 0; i < comps.Count; i++)
			{
				TextureAndColor compStateIcon = comps[i].CompStateIcon;
				if (compStateIcon.HasValue)
				{
					return compStateIcon;
				}
			}
			return TextureAndColor.None;
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__15))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void CopyFrom(Hediff other)
	{
		base.CopyFrom(other);
		if (!(other is HediffWithComps hediffWithComps))
		{
			return;
		}
		foreach (HediffComp comp in comps)
		{
			foreach (HediffComp comp2 in hediffWithComps.comps)
			{
				if (!(comp.GetType() != comp2.GetType()))
				{
					comp.CopyFrom(comp2);
				}
			}
		}
	}

	public override void PostAdd(DamageInfo? dinfo)
	{
		base.PostAdd(dinfo);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].CompPostPostAdd(dinfo);
			}
		}
	}

	public override void PostRemoved()
	{
		base.PostRemoved();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].CompPostPostRemoved();
			}
		}
	}

	public override void PostTick()
	{
		if (comps != null)
		{
			float severityAdjustment = 0f;
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].CompPostTick(ref severityAdjustment);
			}
			if (severityAdjustment != 0f)
			{
				Severity += severityAdjustment;
			}
		}
	}

	public override void PostTickInterval(int delta)
	{
		if (comps != null)
		{
			float severityAdjustment = 0f;
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].CompPostTickInterval(ref severityAdjustment, delta);
			}
			if (severityAdjustment != 0f)
			{
				Severity += severityAdjustment;
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			InitializeComps();
		}
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].CompExposeData();
			}
		}
	}

	public override void Tended(float quality, float maxQuality, int batchPosition = 0)
	{
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].CompTended(quality, maxQuality, batchPosition);
		}
	}

	public override bool TryMergeWith(Hediff other)
	{
		if (base.TryMergeWith(other))
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].CompPostMerged(other);
			}
			return true;
		}
		return false;
	}

	public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
	{
		base.Notify_PawnDied(dinfo, culprit);
		for (int num = comps.Count - 1; num >= 0; num--)
		{
			comps[num].Notify_PawnDied(dinfo, culprit);
		}
	}

	public override void Notify_PawnKilled()
	{
		base.Notify_PawnKilled();
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_PawnKilled();
		}
	}

	public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
	{
		base.Notify_KilledPawn(victim, dinfo);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_KilledPawn(victim, dinfo);
		}
	}

	public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
		}
	}

	public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
	{
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].CompModifyChemicalEffect(chem, ref effect);
		}
	}

	public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
	{
		base.Notify_PawnUsedVerb(verb, target);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_PawnUsedVerb(verb, target);
		}
	}

	public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing src = null)
	{
		base.Notify_EntropyGained(baseAmount, finalAmount, src);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_EntropyGained(baseAmount, finalAmount, src);
		}
	}

	public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
	{
		base.Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
		}
	}

	public override void Notify_Spawned()
	{
		base.Notify_Spawned();
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_Spawned();
		}
	}

	public override void Notify_SurgicallyRemoved(Pawn surgeon)
	{
		base.Notify_SurgicallyRemoved(surgeon);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_SurgicallyRemoved(surgeon);
		}
	}

	public override void Notify_SurgicallyReplaced(Pawn surgeon)
	{
		base.Notify_SurgicallyReplaced(surgeon);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].Notify_SurgicallyReplaced(surgeon);
		}
	}

	public override void PostMake()
	{
		base.PostMake();
		InitializeComps();
		for (int num = comps.Count - 1; num >= 0; num--)
		{
			try
			{
				comps[num].CompPostMake();
			}
			catch (Exception ex)
			{
				Log.Error("Error in HediffComp.CompPostMake(): " + ex);
				comps.RemoveAt(num);
			}
		}
	}

	private void InitializeComps()
	{
		if (def.comps == null)
		{
			return;
		}
		comps = new List<HediffComp>();
		for (int i = 0; i < def.comps.Count; i++)
		{
			HediffComp hediffComp = null;
			try
			{
				hediffComp = (HediffComp)Activator.CreateInstance(def.comps[i].compClass);
				hediffComp.props = def.comps[i];
				hediffComp.parent = this;
				comps.Add(hediffComp);
			}
			catch (Exception ex)
			{
				Log.Error("Could not instantiate or initialize a HediffComp: " + ex);
				comps.Remove(hediffComp);
			}
		}
	}

	public T GetComp<T>() where T : HediffComp
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (comps[i] is T)
			{
				return comps[i] as T;
			}
		}
		return null;
	}

	public override string DebugString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(base.DebugString());
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				string text = ((!comps[i].ToString().Contains('_')) ? comps[i].ToString() : comps[i].ToString().Split('_')[1]);
				stringBuilder.AppendLine("--" + text);
				string text2 = comps[i].CompDebugString();
				if (!text2.NullOrEmpty())
				{
					stringBuilder.AppendLine(text2.TrimEnd().Indented());
				}
			}
		}
		return stringBuilder.ToString();
	}
}
