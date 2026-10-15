using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

[StaticConstructorOnStartup]
public class PawnRenderNodeProperties
{
	public enum Side
	{
		Center,
		Left,
		Right
	}

	public enum AttachmentColorType
	{
		Custom,
		Hair,
		Skin
	}

	public enum RenderNodePawnType
	{
		Any,
		HumanlikeOnly,
		NonHumanlikeOnly
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__49 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PawnRenderNodeProperties _003C_003E4__this;

		private List<PawnRenderNodeProperties>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<string> _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__49(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 5)
			{
				try
				{
					if (num == -4 || num == 5)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PawnRenderNodeProperties pawnRenderNodeProperties = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (pawnRenderNodeProperties.nodeClass == null)
					{
						_003C_003E2__current = "Node " + pawnRenderNodeProperties.debugLabel + ": nodeClass is null.";
						_003C_003E1__state = 1;
						return true;
					}
					if (!typeof(PawnRenderNode).IsAssignableFrom(pawnRenderNodeProperties.nodeClass))
					{
						_003C_003E2__current = "Node " + pawnRenderNodeProperties.debugLabel + ": nodeClass is not a PawnRenderNode or subclass thereof.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c1;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00c1;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c1;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0148;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0148;
				case 5:
					{
						_003C_003E1__state = -4;
						goto IL_01bd;
					}
					IL_01d7:
					if (_003C_003E7__wrap1.MoveNext())
					{
						PawnRenderNodeProperties current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01bd;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
					break;
					IL_01bd:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_01d7;
					IL_0148:
					if (pawnRenderNodeProperties.children == null)
					{
						break;
					}
					_003C_003E7__wrap1 = pawnRenderNodeProperties.children.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01d7;
					IL_00c1:
					if (pawnRenderNodeProperties.workerClass == null)
					{
						_003C_003E2__current = "Node " + pawnRenderNodeProperties.debugLabel + ": workerClass is null.";
						_003C_003E1__state = 3;
						return true;
					}
					if (!typeof(PawnRenderNodeWorker).IsAssignableFrom(pawnRenderNodeProperties.workerClass))
					{
						_003C_003E2__current = "Node " + pawnRenderNodeProperties.debugLabel + ": workerClass is not a PawnRenderNodeWorker or subclass thereof.";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0148;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__49 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__49(0)
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

	public Type workerClass = typeof(PawnRenderNodeWorker);

	public List<Type> subworkerClasses;

	public Type nodeClass = typeof(PawnRenderNode);

	public PawnRenderNodeTagDef tagDef;

	public PawnRenderNodeTagDef parentTagDef;

	public List<PawnRenderNodeProperties> children;

	[NoTranslate]
	public string texPath;

	[NoTranslate]
	public List<string> texPaths;

	[NoTranslate]
	public string texPathFemale;

	[NoTranslate]
	public List<string> texPathsFemale;

	public List<BodyTypeGraphicData> bodyTypeGraphicPaths;

	public int texSeed;

	public bool useGraphic = true;

	public RenderSkipFlagDef skipFlag;

	public RotDrawMode rotDrawMode = RotDrawMode.Fresh | RotDrawMode.Rotting | RotDrawMode.Dessicated;

	public RenderNodePawnType pawnType = RenderNodePawnType.HumanlikeOnly;

	public BodyPartGroupDef linkedBodyPartsGroup;

	public Color? color;

	public float colorRGBPostFactor = 1f;

	public bool useRottenColor;

	public bool useSkinShader;

	public ShaderTypeDef shaderTypeDef;

	public AttachmentColorType colorType;

	public float baseLayer;

	public DrawData drawData;

	public bool oppositeFacingLayerWhenFlipped;

	public Vector2? overrideMeshSize;

	public Vector2 drawSize = Vector2.one;

	[NoTranslate]
	public string anchorTag;

	public Side side;

	public float narrowCrownHorizontalOffset;

	public List<Rot4> visibleFacing;

	public bool flipGraphic;

	public bool rotateIndependently;

	public string debugLabel;

	public PawnOverlayDrawer.OverlayLayer overlayLayer;

	public bool? overlayOverApparel;

	private PawnRenderNodeWorker workerInt;

	private List<PawnRenderSubWorker> subWorkersInt;

	public PawnRenderNodeWorker Worker => GenWorker<PawnRenderNodeWorker>.Get(workerClass);

	public List<PawnRenderSubWorker> SubWorkers => subWorkersInt ?? (subWorkersInt = PawnRenderNodeWorker.GetSubWorkerList(subworkerClasses));

	public void EnsureInitialized()
	{
		if (workerInt == null)
		{
			workerInt = GenWorker<PawnRenderNodeWorker>.Get(workerClass);
		}
		if (subWorkersInt == null)
		{
			subWorkersInt = PawnRenderNodeWorker.GetSubWorkerList(subworkerClasses);
		}
	}

	public virtual void ResolveReferences()
	{
	}

	public virtual void ResolveReferencesRecursive()
	{
		ResolveReferences();
		if (children == null)
		{
			return;
		}
		foreach (PawnRenderNodeProperties child in children)
		{
			child.ResolveReferencesRecursive();
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__49))]
	public virtual IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__49(-2)
		{
			_003C_003E4__this = this
		};
	}
}
