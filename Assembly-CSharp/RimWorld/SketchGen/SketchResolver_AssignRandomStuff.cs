using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038C5 RID: 14533
	public class SketchResolver_AssignRandomStuff : SketchResolver
	{
		// Token: 0x06014102 RID: 82178 RVA: 0x005F6400 File Offset: 0x005F4600
		protected override void ResolveInt(SketchResolveParams parms)
		{
			ThingDef assignRandomStuffTo = parms.assignRandomStuffTo;
			bool valueOrDefault = parms.connectedGroupsSameStuff.GetValueOrDefault();
			bool allowWood = parms.allowWood ?? true;
			bool allowFlammableWalls = parms.allowFlammableWalls ?? true;
			this.thingsAt.Clear();
			foreach (SketchThing sketchThing in parms.sketch.Things)
			{
				if (assignRandomStuffTo == null || sketchThing.def == assignRandomStuffTo)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						List<SketchThing> list;
						if (!this.thingsAt.TryGetValue(key, out list))
						{
							list = new List<SketchThing>();
							this.thingsAt.Add(key, list);
						}
						list.Add(sketchThing);
					}
				}
			}
			this.visited.Clear();
			using (List<SketchThing>.Enumerator enumerator = parms.sketch.Things.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SketchThing thing = enumerator.Current;
					if ((assignRandomStuffTo == null || thing.def == assignRandomStuffTo) && !this.visited.Contains(thing))
					{
						ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(thing.def, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, allowFlammableWalls, thing.def));
						thing.stuff = stuff;
						this.visited.Add(thing);
						if (valueOrDefault)
						{
							this.stack.Clear();
							this.stack.Push(thing);
							while (this.stack.Count != 0)
							{
								SketchThing sketchThing2 = this.stack.Pop();
								sketchThing2.stuff = stuff;
								foreach (IntVec3 key2 in sketchThing2.OccupiedRect.ExpandedBy(1))
								{
									List<SketchThing> list2;
									if (this.thingsAt.TryGetValue(key2, out list2))
									{
										for (int i = 0; i < list2.Count; i++)
										{
											if (list2[i].def == thing.def && !this.visited.Contains(list2[i]))
											{
												this.visited.Add(list2[i]);
												this.stack.Push(list2[i]);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06014103 RID: 82179 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x0400DD96 RID: 56726
		private Dictionary<IntVec3, List<SketchThing>> thingsAt = new Dictionary<IntVec3, List<SketchThing>>();

		// Token: 0x0400DD97 RID: 56727
		private HashSet<SketchThing> visited = new HashSet<SketchThing>();

		// Token: 0x0400DD98 RID: 56728
		private Stack<SketchThing> stack = new Stack<SketchThing>();
	}
}
