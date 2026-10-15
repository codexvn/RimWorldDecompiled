using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class PawnRenderNode_Apparel : PawnRenderNode
{
	[CompilerGenerated]
	private sealed class _003CGraphicsFor_003Ed__5 : IEnumerable<Graphic>, IEnumerable, IEnumerator<Graphic>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Graphic _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PawnRenderNode_Apparel _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		Graphic IEnumerator<Graphic>.Current
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
		public _003CGraphicsFor_003Ed__5(int _003C_003E1__state)
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
			PawnRenderNode_Apparel pawnRenderNode_Apparel = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (ApparelGraphicRecordGetter.TryGetGraphicApparel(pawnRenderNode_Apparel.apparel, pawnRenderNode_Apparel.tree.pawn.story.bodyType, pawn.Drawer.renderer.StatueColor.HasValue, out var rec))
				{
					_003C_003E2__current = rec.graphic;
					_003C_003E1__state = 1;
					return true;
				}
				break;
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
		IEnumerator<Graphic> IEnumerable<Graphic>.GetEnumerator()
		{
			_003CGraphicsFor_003Ed__5 _003CGraphicsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGraphicsFor_003Ed__ = this;
			}
			else
			{
				_003CGraphicsFor_003Ed__ = new _003CGraphicsFor_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGraphicsFor_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGraphicsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Graphic>)this).GetEnumerator();
		}
	}

	public bool useHeadMesh;

	public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
		: base(pawn, props, tree)
	{
		useHeadMesh = props.parentTagDef == PawnRenderNodeTagDefOf.ApparelHead;
		meshSet = MeshSetFor(pawn);
	}

	public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel)
		: base(pawn, props, tree)
	{
		base.apparel = apparel;
		useHeadMesh = props.parentTagDef == PawnRenderNodeTagDefOf.ApparelHead;
		meshSet = MeshSetFor(pawn);
	}

	public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel, bool useHeadMesh)
		: base(pawn, props, tree)
	{
		base.apparel = apparel;
		this.useHeadMesh = useHeadMesh;
		meshSet = MeshSetFor(pawn);
	}

	public override GraphicMeshSet MeshSetFor(Pawn pawn)
	{
		if (apparel == null)
		{
			return base.MeshSetFor(pawn);
		}
		if (base.Props.overrideMeshSize.HasValue)
		{
			return MeshPool.GetMeshSetForSize(base.Props.overrideMeshSize.Value.x, base.Props.overrideMeshSize.Value.y);
		}
		if (useHeadMesh)
		{
			return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn);
		}
		return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn);
	}

	[IteratorStateMachine(typeof(_003CGraphicsFor_003Ed__5))]
	protected override IEnumerable<Graphic> GraphicsFor(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGraphicsFor_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}

	protected override int TexSeedFor(Pawn pawn)
	{
		return base.TexSeedFor(pawn) + apparel.thingIDNumber;
	}
}
