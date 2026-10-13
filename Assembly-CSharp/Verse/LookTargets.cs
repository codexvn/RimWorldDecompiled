using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C7D RID: 3197
	public class LookTargets : IExposable
	{
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x00002C42 File Offset: 0x00000E42
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x001A842C File Offset: 0x001A662C
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x001A8447 File Offset: 0x001A6647
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x001A8458 File Offset: 0x001A6658
		public GlobalTargetInfo PrimaryTarget
		{
			get
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					if (this.targets[i].IsValid)
					{
						return this.targets[i];
					}
				}
				if (this.targets.Count != 0)
				{
					return this.targets[0];
				}
				return GlobalTargetInfo.Invalid;
			}
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x001A84BD File Offset: 0x001A66BD
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x001A84D5 File Offset: 0x001A66D5
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x001A84E8 File Offset: 0x001A66E8
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x001A850C File Offset: 0x001A670C
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x001A8530 File Offset: 0x001A6730
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x001A8556 File Offset: 0x001A6756
		public LookTargets(PlanetTile tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001A857A File Offset: 0x001A677A
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x001A859C File Offset: 0x001A679C
		public LookTargets(params GlobalTargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x001A85E0 File Offset: 0x001A67E0
		public LookTargets(IEnumerable<TargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				IList<TargetInfo> list = targets as IList<TargetInfo>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
					return;
				}
				foreach (TargetInfo target in targets)
				{
					this.targets.Add(target);
				}
			}
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x001A867C File Offset: 0x001A687C
		public LookTargets(params TargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x001A86C2 File Offset: 0x001A68C2
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x001A86DC File Offset: 0x001A68DC
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x001A86F6 File Offset: 0x001A68F6
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x001A8710 File Offset: 0x001A6910
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x001A872A File Offset: 0x001A692A
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x001A8744 File Offset: 0x001A6944
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x001A875E File Offset: 0x001A695E
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001A8778 File Offset: 0x001A6978
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x001A8780 File Offset: 0x001A6980
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001A8788 File Offset: 0x001A6988
		public static implicit operator LookTargets(TargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x001A87AB File Offset: 0x001A69AB
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001A87B3 File Offset: 0x001A69B3
		public static implicit operator LookTargets(GlobalTargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x001A87D1 File Offset: 0x001A69D1
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x001A87D9 File Offset: 0x001A69D9
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001A87E1 File Offset: 0x001A69E1
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x001A87E9 File Offset: 0x001A69E9
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x001A87F1 File Offset: 0x001A69F1
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x001A87F9 File Offset: 0x001A69F9
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x001A8801 File Offset: 0x001A6A01
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x001A8809 File Offset: 0x001A6A09
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x001A8814 File Offset: 0x001A6A14
		public static bool SameTargets(LookTargets a, LookTargets b)
		{
			if (a == null)
			{
				return b == null || !b.Any;
			}
			if (b == null)
			{
				return a == null || !a.Any;
			}
			if (a.targets.Count != b.targets.Count)
			{
				return false;
			}
			for (int i = 0; i < a.targets.Count; i++)
			{
				if (a.targets[i] != b.targets[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x001A8898 File Offset: 0x001A6A98
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x001A88D0 File Offset: 0x001A6AD0
		private void AppendThingTargets<T>(IEnumerable<T> things) where T : Thing
		{
			if (things == null)
			{
				return;
			}
			IList<T> list = things as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in things)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x001A8964 File Offset: 0x001A6B64
		private void AppendWorldObjectTargets<T>(IEnumerable<T> worldObjects) where T : WorldObject
		{
			if (worldObjects == null)
			{
				return;
			}
			IList<T> list = worldObjects as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in worldObjects)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x001A89F8 File Offset: 0x001A6BF8
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x04003AD1 RID: 15057
		public List<GlobalTargetInfo> targets;
	}
}
