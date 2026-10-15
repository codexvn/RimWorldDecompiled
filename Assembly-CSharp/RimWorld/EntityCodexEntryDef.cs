using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class EntityCodexEntryDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__23 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public EntityCodexEntryDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__23(int _003C_003E1__state)
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
				EntityCodexEntryDef entityCodexEntryDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)entityCodexEntryDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0073;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0073;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00b2;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0073:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (entityCodexEntryDef.category == null)
					{
						_003C_003E2__current = "category is null.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b2;
					IL_00b2:
					if (entityCodexEntryDef.uiIconPath.NullOrEmpty())
					{
						_003C_003E2__current = "missing icon.";
						_003C_003E1__state = 3;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public EntityCategoryDef category;

	public bool startDiscovered;

	public List<ThingDef> linkedThings = new List<ThingDef>();

	private List<IncidentDef> linkedIncidents = new List<IncidentDef>();

	public List<ResearchProjectDef> discoveredResearchProjects = new List<ResearchProjectDef>();

	public List<IncidentDef> provocationIncidents = new List<IncidentDef>();

	public EntityDiscoveryType discoveryType;

	public bool allowDiscoveryWhileMapGenerating;

	public int orderInCategory = 9999999;

	public List<AnomalyPlaystyleDef> hideInPlaystyles = new List<AnomalyPlaystyleDef>();

	private ThingDef useDescriptionFrom;

	[NoTranslate]
	private string uiIconPath;

	public Texture2D icon;

	public Texture2D silhouette;

	private const string SilhouetteTexPathSuffix = "_Silhouette";

	public string Description
	{
		get
		{
			if (useDescriptionFrom == null)
			{
				return description;
			}
			return useDescriptionFrom.description;
		}
	}

	public bool Discovered => Find.EntityCodex.Discovered(this);

	public bool Visible
	{
		get
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return !hideInPlaystyles.Contains(Find.Storyteller.difficulty.AnomalyPlaystyleDef);
			}
			return true;
		}
	}

	public override void PostLoad()
	{
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			icon = (uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(uiIconPath));
			silhouette = (uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(uiIconPath + "_Silhouette"));
		});
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		foreach (ThingDef linkedThing in linkedThings)
		{
			if (linkedThing.entityCodexEntry == null)
			{
				linkedThing.entityCodexEntry = this;
				continue;
			}
			Log.Error($"EntityCodexEntryDef {defName} is linked to {linkedThing.defName} but {linkedThing.defName} is already linked to {linkedThing.entityCodexEntry.defName}.");
		}
		foreach (IncidentDef linkedIncident in linkedIncidents)
		{
			if (linkedIncident.codexEntry == null)
			{
				linkedIncident.codexEntry = this;
				continue;
			}
			Log.Error($"EntityCodexEntryDef {defName} is linked to {linkedIncident.defName} but {linkedIncident.defName} is already linked to {linkedIncident.codexEntry.defName}.");
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__23))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}
}
