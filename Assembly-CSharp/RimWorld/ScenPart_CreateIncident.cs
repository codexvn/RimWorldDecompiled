using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

internal class ScenPart_CreateIncident : ScenPart_IncidentBase
{
	[CompilerGenerated]
	private sealed class _003CRandomizableIncidents_003Ed__19 : IEnumerable<IncidentDef>, IEnumerable, IEnumerator<IncidentDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IncidentDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		IncidentDef IEnumerator<IncidentDef>.Current
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
		public _003CRandomizableIncidents_003Ed__19(int _003C_003E1__state)
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
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.Eclipse;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.ToxicFallout;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.SolarFlare;
				_003C_003E1__state = 3;
				return true;
			case 3:
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
		IEnumerator<IncidentDef> IEnumerable<IncidentDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CRandomizableIncidents_003Ed__19(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IncidentDef>)this).GetEnumerator();
		}
	}

	private const float IntervalMidpoint = 30f;

	private const float IntervalDeviation = 15f;

	private float intervalDays;

	private float minDays;

	private float maxDays;

	private bool repeat;

	private string intervalDaysBuffer;

	private string minDaysBuffer;

	private string maxDaysBuffer;

	private float occurTick;

	private bool isFinished;

	protected override string IncidentTag => "CreateIncident";

	private float RandomInterval
	{
		get
		{
			float maxInclusive = ((maxDays > 0f && maxDays > minDays) ? maxDays : minDays);
			return Rand.Range(minDays, maxInclusive);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref intervalDays, "intervalDays", 0f);
		Scribe_Values.Look(ref minDays, "minDays", intervalDays);
		Scribe_Values.Look(ref maxDays, "maxDays", 0f);
		Scribe_Values.Look(ref repeat, "repeat", defaultValue: false);
		Scribe_Values.Look(ref occurTick, "occurTick", 0f);
		Scribe_Values.Look(ref isFinished, "isFinished", defaultValue: false);
	}

	public override int GetHashCode()
	{
		return (int)((uint)(base.GetHashCode() ^ intervalDays.GetHashCode()) ^ (repeat ? 1u : 0u) ^ (uint)occurTick.GetHashCode()) ^ (isFinished ? 1 : 0);
	}

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
		Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
		Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
		Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width / 2f, scenPartRect.height / 3f);
		Rect rect3 = new Rect(scenPartRect.x + scenPartRect.width / 2f, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width / 2f, scenPartRect.height / 3f);
		Rect rect4 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
		DoIncidentEditInterface(rect);
		Widgets.TextFieldNumericLabeled(rect2, "minDays".Translate(), ref minDays, ref minDaysBuffer);
		Widgets.TextFieldNumericLabeled(rect3, "maxDays".Translate(), ref maxDays, ref maxDaysBuffer);
		Widgets.CheckboxLabeled(rect4, "repeat".Translate(), ref repeat);
	}

	public override void Randomize()
	{
		base.Randomize();
		intervalDays = 15f * Rand.Gaussian() + 30f;
		if (intervalDays < 0f)
		{
			intervalDays = 0f;
		}
		minDays = intervalDays;
		repeat = Rand.Range(0, 100) < 50;
	}

	[IteratorStateMachine(typeof(_003CRandomizableIncidents_003Ed__19))]
	protected override IEnumerable<IncidentDef> RandomizableIncidents()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRandomizableIncidents_003Ed__19(-2);
	}

	public override void PostGameStart()
	{
		base.PostGameStart();
		occurTick = (float)Find.TickManager.TicksGame + 60000f * RandomInterval;
	}

	public override void Tick()
	{
		base.Tick();
		if (Find.AnyPlayerHomeMap == null || isFinished)
		{
			return;
		}
		if (incident == null)
		{
			Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null");
			isFinished = true;
		}
		else if ((float)Find.TickManager.TicksGame >= occurTick)
		{
			IncidentParms parms = StorytellerUtility.DefaultParmsNow(incident.category, Find.Maps.Where((Map x) => x.IsPlayerHome).RandomElement());
			if (!incident.Worker.TryExecute(parms))
			{
				isFinished = true;
			}
			else if (repeat && (minDays > 0f || maxDays > minDays))
			{
				occurTick += 60000f * RandomInterval;
			}
			else
			{
				isFinished = true;
			}
		}
	}
}
