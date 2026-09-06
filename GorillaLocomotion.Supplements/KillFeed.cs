using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000019 RID: 25
public static class KillFeed
{
	// Token: 0x06000088 RID: 136 RVA: 0x0000C190 File Offset: 0x0000A390
	public static void Update()
	{
		bool flag3 = !Settings.KillFeedEnabled || GorillaTagger.Instance == null || VRRigCache.ActiveRigs == null;
		if (!flag3)
		{
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag4 = activeRig == null;
				if (!flag4)
				{
					bool flag = KillFeed.IsTagged(activeRig);
					bool flag2 = KillFeed.taggedState.ContainsKey(activeRig) && KillFeed.taggedState[activeRig];
					bool flag5 = flag && !flag2;
					if (flag5)
					{
						NetPlayer owningNetPlayer = activeRig.OwningNetPlayer;
						bool flag6 = owningNetPlayer == null;
						if (flag6)
						{
							goto IL_00BF;
						}
						object obj = owningNetPlayer.NickName;
						bool flag7 = obj != null;
						if (!flag7)
						{
							goto IL_00BF;
						}
						IL_00CA:
						KillFeed.AddEntry((string)obj + " was tagged!");
						goto IL_00E4;
						IL_00BF:
						obj = "Unknown";
						goto IL_00CA;
					}
					IL_00E4:
					KillFeed.taggedState[activeRig] = flag;
				}
			}
			KillFeed.entries.RemoveAll((KillFeed.KillEntry struct4_0) => Time.time - struct4_0.time > Settings.KillFeedDuration);
			bool killFeedWorldSpace = Settings.KillFeedWorldSpace;
			if (killFeedWorldSpace)
			{
				KillFeed.EnsureWorldText();
			}
			else
			{
				bool flag8 = KillFeed.worldObject != null;
				if (flag8)
				{
					GameObject val = KillFeed.worldObject;
					val.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x0000C31C File Offset: 0x0000A51C
	private static bool IsTagged(VRRig rig)
	{
		bool flag = !(rig.mainSkin != null);
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			bool flag3 = !(rig.mainSkin.material != null);
			if (flag3)
			{
				flag2 = false;
			}
			else
			{
				string text = rig.mainSkin.material.name.ToLower();
				flag2 = text.Contains("fected");
			}
		}
		return flag2;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0000C388 File Offset: 0x0000A588
	private static void AddEntry(string text)
	{
		KillFeed.entries.Add(new KillFeed.KillEntry
		{
			text = text,
			time = Time.time
		});
		bool flag = KillFeed.entries.Count <= 10;
		if (!flag)
		{
			KillFeed.entries.RemoveAt(0);
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x0000C3E8 File Offset: 0x0000A5E8
	public static void OnGUI()
	{
		bool flag = !Settings.KillFeedEnabled || Settings.KillFeedWorldSpace || KillFeed.entries.Count == 0;
		if (!flag)
		{
			bool flag2 = KillFeed.style == null;
			if (flag2)
			{
				KillFeed.style = new GUIStyle();
				GUIStyle val = KillFeed.style;
				val.fontSize = 14;
				KillFeed.style.normal.textColor = Color.white;
				GUIStyle val2 = KillFeed.style;
				val2.alignment = 0;
			}
			float num = 10f;
			foreach (KillFeed.KillEntry item in KillFeed.entries)
			{
				float num2 = Mathf.Clamp01((Settings.KillFeedDuration - (Time.time - item.time)) / 1f);
				GUI.color = new Color(1f, 1f, 1f, num2);
				GUI.Label(new Rect(10f, num, 400f, 20f), item.text, KillFeed.style);
				num += 18f;
			}
			GUI.color = Color.white;
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x0000C52C File Offset: 0x0000A72C
	private static void EnsureWorldText()
	{
		bool flag = Camera.main == null;
		if (!flag)
		{
			bool flag2 = KillFeed.worldObject == null;
			if (flag2)
			{
				KillFeed.worldObject = new GameObject("KillFeedVR");
				GameObject val = KillFeed.worldObject;
				val.hideFlags = 61;
				KillFeed.worldText = KillFeed.worldObject.AddComponent<TextMesh>();
				TextMesh val2 = KillFeed.worldText;
				val2.fontSize = 24;
				KillFeed.worldText.characterSize = 0.01f;
				TextMesh val3 = KillFeed.worldText;
				val3.anchor = 0;
				KillFeed.worldText.color = Color.white;
			}
			GameObject val4 = KillFeed.worldObject;
			val4.SetActive(true);
			Transform transform = Camera.main.transform;
			KillFeed.worldObject.transform.position = transform.position + transform.forward * 1.5f + transform.up * 0.4f - transform.right * 0.3f;
			KillFeed.worldObject.transform.rotation = transform.rotation;
			KillFeed.worldObject.transform.localScale = Vector3.one * 0.5f;
			string text = "";
			foreach (KillFeed.KillEntry item in KillFeed.entries)
			{
				bool flag3 = Mathf.Clamp01((Settings.KillFeedDuration - (Time.time - item.time)) / 1f) > 0.1f;
				if (flag3)
				{
					text = text + item.text + "\n";
				}
			}
			KillFeed.worldText.text = text;
		}
	}

	// Token: 0x040001CB RID: 459
	private static readonly List<KillFeed.KillEntry> entries = new List<KillFeed.KillEntry>();

	// Token: 0x040001CC RID: 460
	private static readonly Dictionary<VRRig, bool> taggedState = new Dictionary<VRRig, bool>();

	// Token: 0x040001CD RID: 461
	private static GUIStyle style;

	// Token: 0x040001CE RID: 462
	private static GameObject worldObject;

	// Token: 0x040001CF RID: 463
	private static TextMesh worldText;

	// Token: 0x0200005B RID: 91
	private struct KillEntry
	{
		// Token: 0x040004E3 RID: 1251
		public string text;

		// Token: 0x040004E4 RID: 1252
		public float time;
	}

	// Token: 0x0200005C RID: 92
	[CompilerGenerated]
	[Serializable]
	private sealed class Class12
	{
		// Token: 0x0600022E RID: 558 RVA: 0x00023008 File Offset: 0x00021208
		internal bool method_0(KillFeed.KillEntry struct4_0)
		{
			return Time.time - struct4_0.time > Settings.KillFeedDuration;
		}

		// Token: 0x040004E5 RID: 1253
		public static readonly KillFeed.Class12 _003C_003E9 = new KillFeed.Class12();

		// Token: 0x040004E6 RID: 1254
		public static Predicate<KillFeed.KillEntry> _003C_003E9__6_0;
	}
}
