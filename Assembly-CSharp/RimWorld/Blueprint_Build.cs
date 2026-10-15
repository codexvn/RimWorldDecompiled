using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Blueprint_Build : Blueprint, IHaulEnroute, ILoadReferenceable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass17_0
	{
		public ThingDef thingDef;

		public Color stuffColor;

		public Blueprint_Build _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__0()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>
			{
				new FloatMenuOption("Basic".Translate().CapitalizeFirst(), delegate
				{
					_003C_003E4__this.ChangeStyleOfAllSelected(_003C_003E4__this.def.entityDefToBuild, null);
				}, Widgets.GetIconFor(thingDef, _003C_003E4__this.Stuff, null, null), stuffColor)
			};
			foreach (StyleCategoryDef relevantStyleCategory in thingDef.RelevantStyleCategories)
			{
				foreach (ThingDefStyle thingDefStyle in relevantStyleCategory.thingDefStyles)
				{
					if (thingDefStyle.ThingDef == thingDef)
					{
						_003C_003Ec__DisplayClass17_1 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass17_1
						{
							CS_0024_003C_003E8__locals1 = this,
							tdStyle = thingDefStyle
						};
						list.Add(new FloatMenuOption(relevantStyleCategory.LabelCap, delegate
						{
							CS_0024_003C_003E8__locals4.CS_0024_003C_003E8__locals1._003C_003E4__this.ChangeStyleOfAllSelected(CS_0024_003C_003E8__locals4.CS_0024_003C_003E8__locals1._003C_003E4__this.def.entityDefToBuild, CS_0024_003C_003E8__locals4.tdStyle.StyleDef);
						}, Widgets.GetIconFor(thingDef, _003C_003E4__this.Stuff, CS_0024_003C_003E8__locals4.tdStyle.StyleDef, null), stuffColor));
						break;
					}
				}
			}
			if (list.Any())
			{
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass17_1
	{
		public ThingDefStyle tdStyle;

		public _003C_003Ec__DisplayClass17_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetGizmos_003Eb__2()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.ChangeStyleOfAllSelected(CS_0024_003C_003E8__locals1._003C_003E4__this.def.entityDefToBuild, tdStyle.StyleDef);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__17 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Blueprint_Build _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<Command> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Blueprint_Build blueprint_Build = _003C_003E4__this;
				Command command;
				_003C_003Ec__DisplayClass17_0 CS_0024_003C_003E8__locals21;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Blueprint)blueprint_Build).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00f3;
				case 3:
					_003C_003E1__state = -1;
					goto IL_023a;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_0287;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					command = BuildCopyCommandUtility.BuildCopyCommand(blueprint_Build.def.entityDefToBuild, blueprint_Build.stuffToUse, blueprint_Build.StyleSourcePrecept as Precept_Building, blueprint_Build.selectedStyleDef ?? blueprint_Build.StyleDef, blueprint_Build.styleOverridden, blueprint_Build.glowerColorOverride);
					if (command != null)
					{
						_003C_003E2__current = command;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00f3;
					IL_00f3:
					if (blueprint_Build.Faction != Faction.OfPlayer)
					{
						break;
					}
					CS_0024_003C_003E8__locals21 = new _003C_003Ec__DisplayClass17_0
					{
						_003C_003E4__this = blueprint_Build
					};
					if (ModsConfig.IdeologyActive && Find.IdeoManager.classicMode)
					{
						BuildableDef entityDefToBuild = blueprint_Build.def.entityDefToBuild;
						CS_0024_003C_003E8__locals21.thingDef = entityDefToBuild as ThingDef;
						if (CS_0024_003C_003E8__locals21.thingDef != null && CS_0024_003C_003E8__locals21.thingDef.CanBeStyled())
						{
							CS_0024_003C_003E8__locals21.stuffColor = ((blueprint_Build.stuffToUse == null) ? Color.white : blueprint_Build.def.entityDefToBuild.GetColorForStuff(blueprint_Build.stuffToUse));
							Command_Action command_Action = new Command_Action
							{
								defaultLabel = "ChangeStyle".Translate().CapitalizeFirst(),
								defaultDesc = "ChangeStyleDesc".Translate(),
								icon = ChangeStyleTex.Texture,
								Order = 15f,
								action = delegate
								{
									List<FloatMenuOption> list = new List<FloatMenuOption>
									{
										new FloatMenuOption("Basic".Translate().CapitalizeFirst(), delegate
										{
											CS_0024_003C_003E8__locals21._003C_003E4__this.ChangeStyleOfAllSelected(CS_0024_003C_003E8__locals21._003C_003E4__this.def.entityDefToBuild, null);
										}, Widgets.GetIconFor(CS_0024_003C_003E8__locals21.thingDef, CS_0024_003C_003E8__locals21._003C_003E4__this.Stuff, null, null), CS_0024_003C_003E8__locals21.stuffColor)
									};
									foreach (StyleCategoryDef relevantStyleCategory in CS_0024_003C_003E8__locals21.thingDef.RelevantStyleCategories)
									{
										foreach (ThingDefStyle thingDefStyle in relevantStyleCategory.thingDefStyles)
										{
											if (thingDefStyle.ThingDef == CS_0024_003C_003E8__locals21.thingDef)
											{
												_003C_003Ec__DisplayClass17_1 CS_0024_003C_003E8__locals19 = new _003C_003Ec__DisplayClass17_1
												{
													CS_0024_003C_003E8__locals1 = CS_0024_003C_003E8__locals21,
													tdStyle = thingDefStyle
												};
												list.Add(new FloatMenuOption(relevantStyleCategory.LabelCap, delegate
												{
													CS_0024_003C_003E8__locals19.CS_0024_003C_003E8__locals1._003C_003E4__this.ChangeStyleOfAllSelected(CS_0024_003C_003E8__locals19.CS_0024_003C_003E8__locals1._003C_003E4__this.def.entityDefToBuild, CS_0024_003C_003E8__locals19.tdStyle.StyleDef);
												}, Widgets.GetIconFor(CS_0024_003C_003E8__locals21.thingDef, CS_0024_003C_003E8__locals21._003C_003E4__this.Stuff, CS_0024_003C_003E8__locals19.tdStyle.StyleDef, null), CS_0024_003C_003E8__locals21.stuffColor));
												break;
											}
										}
									}
									if (list.Any())
									{
										Find.WindowStack.Add(new FloatMenu(list));
									}
								}
							};
							if (!CS_0024_003C_003E8__locals21.thingDef.RelevantStyleCategories.Any())
							{
								command_Action.Disable("ChangeStyleDisabledNoCategories".Translate());
							}
							_003C_003E2__current = command_Action;
							_003C_003E1__state = 3;
							return true;
						}
					}
					goto IL_023a;
					IL_0287:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Command current4 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					break;
					IL_023a:
					_003C_003E7__wrap2 = BuildRelatedCommandUtility.RelatedBuildCommands(blueprint_Build.def.entityDefToBuild).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0287;
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

		private void _003C_003Em__Finally2()
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
			_003CGetGizmos_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__17(0)
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

	public ThingDef stuffToUse;

	public ThingStyleDef selectedStyleDef;

	public bool styleOverridden;

	public ColorInt? glowerColorOverride;

	private static readonly CachedTexture ChangeStyleTex = new CachedTexture("UI/Gizmos/ChangeStyle");

	public override string Label
	{
		get
		{
			string text = def.entityDefToBuild.label;
			if (base.StyleSourcePrecept != null)
			{
				text = base.StyleSourcePrecept.TransformThingLabel(text);
			}
			if (stuffToUse != null)
			{
				return "ThingMadeOfStuffLabel".Translate(stuffToUse.LabelAsStuff, text) + "BlueprintLabelExtra".Translate();
			}
			return text + "BlueprintLabelExtra".Translate();
		}
	}

	protected override float WorkTotal => def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, stuffToUse);

	public ThingDef BuildDef => def.entityDefToBuild as ThingDef;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref stuffToUse, "stuffToUse");
		Scribe_Defs.Look(ref selectedStyleDef, "selectedStyleDef");
		Scribe_Values.Look(ref styleOverridden, "styleOverridden", defaultValue: false);
		Scribe_Values.Look(ref glowerColorOverride, "glowerColorOverride", null);
	}

	public override ThingStyleDef EntityToBuildStyle()
	{
		return selectedStyleDef ?? StyleDef;
	}

	public override BuildableDef EntityToBuild()
	{
		return def.entityDefToBuild;
	}

	public override ThingDef EntityToBuildStuff()
	{
		return stuffToUse;
	}

	public override List<ThingDefCountClass> TotalMaterialCost()
	{
		return def.entityDefToBuild.CostListAdjusted(stuffToUse);
	}

	protected override Thing MakeSolidThing(out bool shouldSelect)
	{
		Frame frame = (Frame)ThingMaker.MakeThing(def.entityDefToBuild.frameDef, stuffToUse);
		frame.StyleSourcePrecept = base.StyleSourcePrecept;
		frame.StyleDef = StyleDef;
		frame.glowerColorOverride = glowerColorOverride;
		shouldSelect = false;
		base.Map.enrouteManager.SendReservations(this, frame);
		return frame;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__17))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void ChangeStyleOfAllSelected(BuildableDef buildable, ThingStyleDef styleDef)
	{
		foreach (object selectedObject in Find.Selector.SelectedObjects)
		{
			if (selectedObject is Blueprint_Build blueprint_Build && blueprint_Build.def.entityDefToBuild == buildable)
			{
				blueprint_Build.StyleDef = styleDef;
				blueprint_Build.styleOverridden = true;
				blueprint_Build.DirtyMapMesh(blueprint_Build.Map);
			}
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		Building edifice = base.Position.GetEdifice(base.Map);
		if ((edifice == null || edifice.def.building?.supportsWallAttachments != true) && (mode == DestroyMode.Cancel || (mode != 0 && !def.IsBlueprint)))
		{
			foreach (Thing attachedBuilding in GenConstruct.GetAttachedBuildings(this))
			{
				attachedBuilding.DeSpawn(mode);
			}
		}
		base.DeSpawn(mode);
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		if (stringBuilder.Length > 0)
		{
			stringBuilder.AppendLine();
		}
		stringBuilder.AppendLine("ContainedResources".Translate() + ":");
		bool flag = true;
		foreach (ThingDefCountClass item in TotalMaterialCost())
		{
			if (!flag)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(string.Concat(item.thingDef.LabelCap + ": 0 / ", item.count.ToString()));
			flag = false;
		}
		if (StyleDef?.Category != null && base.StyleSourcePrecept == null)
		{
			stringBuilder.AppendInNewLine("Style".Translate() + ": " + StyleDef.Category.LabelCap);
		}
		return stringBuilder.ToString().Trim();
	}
}
