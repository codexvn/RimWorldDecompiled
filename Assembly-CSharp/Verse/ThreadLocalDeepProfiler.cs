using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x02000CA5 RID: 3237
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x06005301 RID: 21249 RVA: 0x001AC1C4 File Offset: 0x001AA3C4
		static ThreadLocalDeepProfiler()
		{
			for (int i = 0; i < 50; i++)
			{
				ThreadLocalDeepProfiler.Prefixes[i] = "";
				for (int j = 0; j < i; j++)
				{
					string[] prefixes = ThreadLocalDeepProfiler.Prefixes;
					int num = i;
					prefixes[num] += " -";
				}
			}
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x001AC21A File Offset: 0x001AA41A
		public void Start(string label = null)
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x001AC238 File Offset: 0x001AA438
		public void End()
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			if (this.watchers.Count == 0)
			{
				Log.Error("Ended deep profiling while not profiling.");
				return;
			}
			ThreadLocalDeepProfiler.Watcher watcher = this.watchers.Pop();
			watcher.Watch.Stop();
			if (this.watchers.Count > 0)
			{
				this.watchers.Peek().AddChildResult(watcher);
				return;
			}
			this.Output(watcher);
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x001AC2A4 File Offset: 0x001AA4A4
		private void Output(ThreadLocalDeepProfiler.Watcher root)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (UnityData.IsInMainThread)
			{
				stringBuilder.AppendLine("--- Main thread ---");
			}
			else
			{
				stringBuilder.AppendLine("--- Thread " + Thread.CurrentThread.ManagedThreadId.ToString() + " ---");
			}
			List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
			list.Add(root);
			this.AppendStringRecursive(stringBuilder, root.Label, root.Children, root.ElapsedMilliseconds, 0, list, -1.0);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			this.HotspotAnalysis(stringBuilder, list);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x001AC348 File Offset: 0x001AA548
		private void HotspotAnalysis(StringBuilder sb, List<ThreadLocalDeepProfiler.Watcher> allWatchers)
		{
			List<ThreadLocalDeepProfiler.LabelTimeTuple> list = new List<ThreadLocalDeepProfiler.LabelTimeTuple>();
			foreach (IGrouping<string, ThreadLocalDeepProfiler.Watcher> grouping in from w in allWatchers
			group w by w.Label)
			{
				double num = 0.0;
				double num2 = 0.0;
				int num3 = 0;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in grouping)
				{
					num3++;
					num += watcher.ElapsedMilliseconds;
					if (watcher.Children != null)
					{
						foreach (ThreadLocalDeepProfiler.Watcher watcher2 in watcher.Children)
						{
							num2 += watcher2.ElapsedMilliseconds;
						}
					}
				}
				list.Add(new ThreadLocalDeepProfiler.LabelTimeTuple
				{
					label = num3.ToString() + "x " + grouping.Key,
					totalTime = num,
					selfTime = num - num2
				});
			}
			sb.AppendLine("Hotspot analysis");
			sb.AppendLine("----------------------------------------");
			foreach (ThreadLocalDeepProfiler.LabelTimeTuple labelTimeTuple in from e in list
			orderby e.selfTime descending
			select e)
			{
				string[] array = new string[6];
				array[0] = labelTimeTuple.label;
				array[1] = " -> ";
				int num4 = 2;
				double num5 = labelTimeTuple.selfTime;
				array[num4] = num5.ToString("0.000");
				array[3] = " ms (total (w/children): ";
				int num6 = 4;
				num5 = labelTimeTuple.totalTime;
				array[num6] = num5.ToString("0.000");
				array[5] = " ms)";
				sb.AppendLine(string.Concat(array));
			}
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x001AC584 File Offset: 0x001AA784
		private void AppendStringRecursive(StringBuilder sb, string label, List<ThreadLocalDeepProfiler.Watcher> children, double elapsedTime, int depth, List<ThreadLocalDeepProfiler.Watcher> allWatchers, double parentElapsedTime)
		{
			string text = ThreadLocalDeepProfiler.Prefixes[depth] + " " + elapsedTime.ToString("0.000") + "ms";
			if (depth > 0)
			{
				text = text + " (" + ((float)(elapsedTime / parentElapsedTime)).ToStringPercent() + ")";
			}
			if (children != null)
			{
				double num = elapsedTime;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in children)
				{
					num -= watcher.ElapsedMilliseconds;
				}
				sb.AppendLine(string.Concat(new string[]
				{
					text,
					" (self: ",
					num.ToString("0.000"),
					" ms) ",
					label
				}));
			}
			else
			{
				sb.AppendLine(text + " " + label);
			}
			if (children != null)
			{
				allWatchers.AddRange(children);
				foreach (ValueTuple<List<ThreadLocalDeepProfiler.Watcher>, double> valueTuple in from x in (from c in children
				group c by c.Label).Select(delegate(IGrouping<string, ThreadLocalDeepProfiler.Watcher> x)
				{
					double num4 = 0.0;
					foreach (ThreadLocalDeepProfiler.Watcher watcher3 in x)
					{
						num4 += watcher3.ElapsedMilliseconds;
					}
					return new ValueTuple<List<ThreadLocalDeepProfiler.Watcher>, double>(x.ToList<ThreadLocalDeepProfiler.Watcher>(), num4);
				})
				orderby x.Item2 descending
				select x)
				{
					List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
					double num2 = 0.0;
					int num3 = 0;
					foreach (ThreadLocalDeepProfiler.Watcher watcher2 in valueTuple.Item1)
					{
						if (watcher2.Children != null)
						{
							foreach (ThreadLocalDeepProfiler.Watcher item in watcher2.Children)
							{
								list.Add(item);
							}
						}
						num2 += watcher2.ElapsedMilliseconds;
						num3++;
					}
					string label2 = valueTuple.Item1[0].Label;
					if (num3 <= 1)
					{
						this.AppendStringRecursive(sb, label2, list, num2, depth + 1, allWatchers, elapsedTime);
					}
					else
					{
						this.AppendStringRecursive(sb, num3.ToString() + "x " + label2, list, num2, depth + 1, allWatchers, elapsedTime);
					}
				}
			}
		}

		// Token: 0x04003BF7 RID: 15351
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x04003BF8 RID: 15352
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x04003BF9 RID: 15353
		private const int MaxDepth = 50;

		// Token: 0x02000CA6 RID: 3238
		private class Watcher
		{
			// Token: 0x17000CC2 RID: 3266
			// (get) Token: 0x06005308 RID: 21256 RVA: 0x001AC874 File Offset: 0x001AAA74
			public double ElapsedMilliseconds
			{
				get
				{
					return this.watch.Elapsed.TotalMilliseconds;
				}
			}

			// Token: 0x17000CC3 RID: 3267
			// (get) Token: 0x06005309 RID: 21257 RVA: 0x001AC894 File Offset: 0x001AAA94
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17000CC4 RID: 3268
			// (get) Token: 0x0600530A RID: 21258 RVA: 0x001AC89C File Offset: 0x001AAA9C
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17000CC5 RID: 3269
			// (get) Token: 0x0600530B RID: 21259 RVA: 0x001AC8A4 File Offset: 0x001AAAA4
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x0600530C RID: 21260 RVA: 0x001AC8AC File Offset: 0x001AAAAC
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x0600530D RID: 21261 RVA: 0x001AC8CD File Offset: 0x001AAACD
			public Watcher(string label, Stopwatch stopwatch)
			{
				this.label = label;
				this.watch = stopwatch;
				this.children = null;
			}

			// Token: 0x0600530E RID: 21262 RVA: 0x001AC8EA File Offset: 0x001AAAEA
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x04003BFA RID: 15354
			private string label;

			// Token: 0x04003BFB RID: 15355
			private Stopwatch watch;

			// Token: 0x04003BFC RID: 15356
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}

		// Token: 0x02000CA7 RID: 3239
		private struct LabelTimeTuple
		{
			// Token: 0x04003BFD RID: 15357
			public string label;

			// Token: 0x04003BFE RID: 15358
			public double totalTime;

			// Token: 0x04003BFF RID: 15359
			public double selfTime;
		}
	}
}
