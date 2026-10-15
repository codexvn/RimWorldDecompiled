using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class DynamicPawnRenderNodeSetup_Apparel : DynamicPawnRenderNodeSetup
{
	[CompilerGenerated]
	private sealed class _003CGetDynamicNodes_003Ed__3 : IEnumerable<(PawnRenderNode node, PawnRenderNode parent)>, IEnumerable, IEnumerator<(PawnRenderNode node, PawnRenderNode parent)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (PawnRenderNode node, PawnRenderNode parent) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private PawnRenderTree tree;

		public PawnRenderTree _003C_003E3__tree;

		private Dictionary<PawnRenderNode, int> _003ClayerOffsets_003E5__2;

		private PawnRenderNode _003CheadApparelNode_003E5__3;

		private PawnRenderNode _003CbodyApparelNode_003E5__4;

		private List<Apparel>.Enumerator _003C_003E7__wrap4;

		private IEnumerator<(PawnRenderNode node, PawnRenderNode parent)> _003C_003E7__wrap5;

		private (PawnRenderNode node, PawnRenderNode parent) _003Cresult_003E5__7;

		(PawnRenderNode node, PawnRenderNode parent) IEnumerator<(PawnRenderNode, PawnRenderNode)>.Current
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
		public _003CGetDynamicNodes_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
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
			_003ClayerOffsets_003E5__2 = null;
			_003CheadApparelNode_003E5__3 = null;
			_003CbodyApparelNode_003E5__4 = null;
			_003C_003E7__wrap4 = default(List<Apparel>.Enumerator);
			_003C_003E7__wrap5 = null;
			_003Cresult_003E5__7 = default((PawnRenderNode, PawnRenderNode));
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_014a;
				}
				_003C_003E1__state = -1;
				if (pawn.apparel == null || pawn.apparel.WornApparelCount == 0)
				{
					return false;
				}
				_003ClayerOffsets_003E5__2 = new Dictionary<PawnRenderNode, int>();
				_003CheadApparelNode_003E5__3 = (tree.TryGetNodeByTag(PawnRenderNodeTagDefOf.ApparelHead, out var node) ? node : null);
				_003CbodyApparelNode_003E5__4 = (tree.TryGetNodeByTag(PawnRenderNodeTagDefOf.ApparelBody, out var node2) ? node2 : null);
				_003C_003E7__wrap4 = pawn.apparel.WornApparel.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_01c1;
				IL_01c1:
				Apparel current;
				do
				{
					if (_003C_003E7__wrap4.MoveNext())
					{
						current = _003C_003E7__wrap4.Current;
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = default(List<Apparel>.Enumerator);
					return false;
				}
				while (!ShouldAddApparelNode(current));
				_003C_003E7__wrap5 = ProcessApparel(pawn, tree, current, _003CheadApparelNode_003E5__3, _003CbodyApparelNode_003E5__4, _003ClayerOffsets_003E5__2).GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_01a4;
				IL_014a:
				if (_003Cresult_003E5__7.parent != null && !_003ClayerOffsets_003E5__2.TryAdd(_003Cresult_003E5__7.parent, 1))
				{
					_003ClayerOffsets_003E5__2[_003Cresult_003E5__7.parent]++;
				}
				_003Cresult_003E5__7 = default((PawnRenderNode, PawnRenderNode));
				goto IL_01a4;
				IL_01a4:
				if (_003C_003E7__wrap5.MoveNext())
				{
					_003Cresult_003E5__7 = _003C_003E7__wrap5.Current;
					if (_003Cresult_003E5__7.node != null)
					{
						_003C_003E2__current = _003Cresult_003E5__7;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_014a;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap5 = null;
				goto IL_01c1;
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
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(PawnRenderNode node, PawnRenderNode parent)> IEnumerable<(PawnRenderNode, PawnRenderNode)>.GetEnumerator()
		{
			_003CGetDynamicNodes_003Ed__3 _003CGetDynamicNodes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetDynamicNodes_003Ed__ = this;
			}
			else
			{
				_003CGetDynamicNodes_003Ed__ = new _003CGetDynamicNodes_003Ed__3(0);
			}
			_003CGetDynamicNodes_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetDynamicNodes_003Ed__.tree = _003C_003E3__tree;
			return _003CGetDynamicNodes_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(PawnRenderNode, PawnRenderNode)>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CProcessApparel_003Ed__5 : IEnumerable<(PawnRenderNode node, PawnRenderNode parent)>, IEnumerable, IEnumerator<(PawnRenderNode node, PawnRenderNode parent)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (PawnRenderNode node, PawnRenderNode parent) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Apparel ap;

		public Apparel _003C_003E3__ap;

		private PawnRenderTree tree;

		public PawnRenderTree _003C_003E3__tree;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private PawnRenderNode headApparelNode;

		public PawnRenderNode _003C_003E3__headApparelNode;

		private PawnRenderNode bodyApparelNode;

		public PawnRenderNode _003C_003E3__bodyApparelNode;

		private Dictionary<PawnRenderNode, int> layerOffsets;

		public Dictionary<PawnRenderNode, int> _003C_003E3__layerOffsets;

		private List<PawnRenderNodeProperties>.Enumerator _003C_003E7__wrap1;

		(PawnRenderNode node, PawnRenderNode parent) IEnumerator<(PawnRenderNode, PawnRenderNode)>.Current
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
		public _003CProcessApparel_003Ed__5(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				ApparelGraphicRecord rec;
				PawnRenderNodeProperties pawnRenderNodeProperties;
				PawnRenderNode pawnRenderNode;
				DrawData drawData;
				ApparelLayerDef lastLayer;
				bool flag;
				PawnRenderNode node;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (ap.def.apparel.HasDefinedGraphicProperties)
					{
						_003C_003E7__wrap1 = ap.def.apparel.RenderNodeProperties.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00ec;
					}
					goto IL_010e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ec;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_010e:
					if (!ApparelGraphicRecordGetter.TryGetGraphicApparel(ap, pawn.story.bodyType, pawn.Drawer.renderer.StatueColor.HasValue, out rec))
					{
						return false;
					}
					pawnRenderNodeProperties = null;
					pawnRenderNode = null;
					drawData = ap.def.apparel.drawData;
					lastLayer = ap.def.apparel.LastLayer;
					flag = lastLayer == ApparelLayerDefOf.Overhead || lastLayer == ApparelLayerDefOf.EyeCover;
					if (ap.def.apparel.parentTagDef != null && tree.TryGetNodeByTag(ap.def.apparel.parentTagDef, out node))
					{
						pawnRenderNode = node;
						if (headApparelNode != null && pawnRenderNode == headApparelNode)
						{
							flag = true;
						}
						else if (bodyApparelNode != null && pawnRenderNode == bodyApparelNode)
						{
							flag = false;
						}
					}
					if (headApparelNode != null && flag)
					{
						if (pawnRenderNode == null)
						{
							pawnRenderNode = headApparelNode;
						}
						int valueOrDefault = layerOffsets.GetValueOrDefault(pawnRenderNode, 0);
						pawnRenderNodeProperties = new PawnRenderNodeProperties
						{
							debugLabel = ap.def.defName,
							workerClass = typeof(PawnRenderNodeWorker_Apparel_Head),
							baseLayer = pawnRenderNode.Props.baseLayer + (float)valueOrDefault,
							drawData = drawData,
							parentTagDef = ap.def.apparel.parentTagDef
						};
					}
					else if (bodyApparelNode != null)
					{
						if (pawnRenderNode == null)
						{
							pawnRenderNode = bodyApparelNode;
						}
						int valueOrDefault2 = layerOffsets.GetValueOrDefault(pawnRenderNode, 0);
						pawnRenderNodeProperties = new PawnRenderNodeProperties
						{
							debugLabel = ap.def.defName,
							workerClass = typeof(PawnRenderNodeWorker_Apparel_Body),
							baseLayer = pawnRenderNode.Props.baseLayer + (float)valueOrDefault2,
							drawData = drawData,
							parentTagDef = ap.def.apparel.parentTagDef
						};
						if (drawData == null && !ap.def.apparel.shellRenderedBehindHead)
						{
							if (lastLayer == ApparelLayerDefOf.Shell)
							{
								pawnRenderNodeProperties.drawData = DrawData.NewWithData(new DrawData.RotationalData(Rot4.North, 88f));
							}
							else if (ap.RenderAsPack())
							{
								pawnRenderNodeProperties.drawData = DrawData.NewWithData(new DrawData.RotationalData(Rot4.North, 93f), new DrawData.RotationalData(Rot4.South, -3f));
							}
						}
					}
					if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties))
					{
						_003C_003E2__current = (node: new PawnRenderNode_Apparel(pawn, pawnRenderNodeProperties, tree, ap), parent: pawnRenderNode);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E2__current = (node: null, parent: pawnRenderNode);
					_003C_003E1__state = 3;
					return true;
					IL_00ec:
					while (_003C_003E7__wrap1.MoveNext())
					{
						PawnRenderNodeProperties current = _003C_003E7__wrap1.Current;
						if (tree.ShouldAddNodeToTree(current))
						{
							PawnRenderNode pawnRenderNode2 = (PawnRenderNode)Activator.CreateInstance(current.nodeClass, pawn, current, tree);
							pawnRenderNode2.apparel = ap;
							_003C_003E2__current = (node: pawnRenderNode2, parent: null);
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
					goto IL_010e;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(PawnRenderNode node, PawnRenderNode parent)> IEnumerable<(PawnRenderNode, PawnRenderNode)>.GetEnumerator()
		{
			_003CProcessApparel_003Ed__5 _003CProcessApparel_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CProcessApparel_003Ed__ = this;
			}
			else
			{
				_003CProcessApparel_003Ed__ = new _003CProcessApparel_003Ed__5(0);
			}
			_003CProcessApparel_003Ed__.pawn = _003C_003E3__pawn;
			_003CProcessApparel_003Ed__.tree = _003C_003E3__tree;
			_003CProcessApparel_003Ed__.ap = _003C_003E3__ap;
			_003CProcessApparel_003Ed__.headApparelNode = _003C_003E3__headApparelNode;
			_003CProcessApparel_003Ed__.bodyApparelNode = _003C_003E3__bodyApparelNode;
			_003CProcessApparel_003Ed__.layerOffsets = _003C_003E3__layerOffsets;
			return _003CProcessApparel_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(PawnRenderNode, PawnRenderNode)>)this).GetEnumerator();
		}
	}

	private const int ApparelLayerShellNorth = 88;

	public override bool HumanlikeOnly => true;

	[IteratorStateMachine(typeof(_003CGetDynamicNodes_003Ed__3))]
	public override IEnumerable<(PawnRenderNode node, PawnRenderNode parent)> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDynamicNodes_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__tree = tree
		};
	}

	private static bool ShouldAddApparelNode(Apparel gear)
	{
		return !gear.def.IsWeapon;
	}

	[IteratorStateMachine(typeof(_003CProcessApparel_003Ed__5))]
	private static IEnumerable<(PawnRenderNode node, PawnRenderNode parent)> ProcessApparel(Pawn pawn, PawnRenderTree tree, Apparel ap, PawnRenderNode headApparelNode, PawnRenderNode bodyApparelNode, Dictionary<PawnRenderNode, int> layerOffsets)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CProcessApparel_003Ed__5(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__tree = tree,
			_003C_003E3__ap = ap,
			_003C_003E3__headApparelNode = headApparelNode,
			_003C_003E3__bodyApparelNode = bodyApparelNode,
			_003C_003E3__layerOffsets = layerOffsets
		};
	}
}
