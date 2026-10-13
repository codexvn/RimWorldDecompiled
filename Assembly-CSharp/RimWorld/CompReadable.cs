using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001927 RID: 6439
	public class CompReadable : ThingComp
	{
		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x0600926D RID: 37485 RVA: 0x002B240E File Offset: 0x002B060E
		public IEnumerable<ReadingOutcomeDoer> Doers
		{
			get
			{
				return this.doers;
			}
		}

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x0600926E RID: 37486 RVA: 0x002B2416 File Offset: 0x002B0616
		public CompProperties_Readable Props
		{
			get
			{
				return (CompProperties_Readable)this.props;
			}
		}

		// Token: 0x0600926F RID: 37487 RVA: 0x002B2423 File Offset: 0x002B0623
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.InitializeDoers();
		}

		// Token: 0x06009270 RID: 37488 RVA: 0x002B2434 File Offset: 0x002B0634
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.InitializeDoers();
			if (this.doers == null)
			{
				return;
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				this.doers[i].PostMake();
			}
		}

		// Token: 0x06009271 RID: 37489 RVA: 0x002B2480 File Offset: 0x002B0680
		public bool TryGetDoer<T>(out T doer) where T : ReadingOutcomeDoer
		{
			if (this.doers == null)
			{
				doer = default(T);
				return false;
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				T t = this.doers[i] as T;
				if (t != null)
				{
					doer = t;
					return true;
				}
			}
			doer = default(T);
			return false;
		}

		// Token: 0x06009272 RID: 37490 RVA: 0x002B24E4 File Offset: 0x002B06E4
		public T GetDoer<T>() where T : ReadingOutcomeDoer
		{
			if (this.doers == null)
			{
				return default(T);
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				T t = this.doers[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06009273 RID: 37491 RVA: 0x002B2543 File Offset: 0x002B0743
		public IEnumerable<T> GetDoers<T>() where T : ReadingOutcomeDoer
		{
			if (this.doers == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.doers.Count; i = num + 1)
			{
				T t = this.doers[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06009274 RID: 37492 RVA: 0x002B2554 File Offset: 0x002B0754
		private void InitializeDoers()
		{
			if (this.Props.doers.Empty<ReadingOutcomeProperties>())
			{
				return;
			}
			this.doers = new List<ReadingOutcomeDoer>();
			for (int i = 0; i < this.Props.doers.Count; i++)
			{
				ReadingOutcomeDoer readingOutcomeDoer = null;
				ReadingOutcomeProperties readingOutcomeProperties = this.Props.doers[i];
				try
				{
					readingOutcomeDoer = (ReadingOutcomeDoer)Activator.CreateInstance(readingOutcomeProperties.DoerClass);
					this.doers.Add(readingOutcomeDoer);
					readingOutcomeDoer.Initialize(this.parent, readingOutcomeProperties);
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Could not instantiate or initialize reading outcome doer ({0}): {1}", readingOutcomeProperties.DoerClass, arg));
					this.doers.Remove(readingOutcomeDoer);
				}
			}
		}

		// Token: 0x06009275 RID: 37493 RVA: 0x002B2610 File Offset: 0x002B0810
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeDoers();
			}
			if (this.doers == null)
			{
				return;
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				this.doers[i].PostExposeData();
			}
		}

		// Token: 0x04005D7C RID: 23932
		protected List<ReadingOutcomeDoer> doers;
	}
}
