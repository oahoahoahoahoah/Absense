using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Photon.Realtime;
using TMPro;
using UnityEngine;

// Token: 0x02000022 RID: 34
public static class NameTags
{
	// Token: 0x060000F1 RID: 241 RVA: 0x00013208 File Offset: 0x00011408
	public static void Update()
	{
		bool flag = GorillaTagger.Instance == null;
		if (!flag)
		{
			bool flag2 = VRRigCache.ActiveRigs != null;
			if (flag2)
			{
				bool flag3 = NameTags.rigNameField == null;
				if (flag3)
				{
					Type typeFromHandle = typeof(VRRig);
					NameTags.rigNameField = typeFromHandle.GetField("fps", BindingFlags.Instance | BindingFlags.NonPublic);
					bool flag4 = !Settings.NameTagsModEnabled;
					if (flag4)
					{
						NameTags.EnsureReflection(NameTags.nameTags);
					}
					else
					{
						NameTags.ClearNameTags();
					}
					bool platformTagsEnabled = Settings.PlatformTagsEnabled;
					if (platformTagsEnabled)
					{
						NameTags.ClearPlatformTags();
					}
					else
					{
						NameTags.EnsureReflection(NameTags.platformTags);
					}
					bool fpstagsModEnabled = Settings.FPSTagsModEnabled;
					if (fpstagsModEnabled)
					{
						NameTags.ClearFpsTags();
					}
					else
					{
						NameTags.EnsureReflection(NameTags.fpsTags);
					}
				}
				else
				{
					bool flag5 = !Settings.NameTagsModEnabled;
					if (flag5)
					{
						NameTags.EnsureReflection(NameTags.nameTags);
					}
					else
					{
						NameTags.ClearNameTags();
					}
					bool platformTagsEnabled2 = Settings.PlatformTagsEnabled;
					if (platformTagsEnabled2)
					{
						NameTags.ClearPlatformTags();
					}
					else
					{
						NameTags.EnsureReflection(NameTags.platformTags);
					}
					bool fpstagsModEnabled2 = Settings.FPSTagsModEnabled;
					if (fpstagsModEnabled2)
					{
						NameTags.ClearFpsTags();
					}
					else
					{
						NameTags.EnsureReflection(NameTags.fpsTags);
					}
				}
			}
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00013340 File Offset: 0x00011540
	private static void ClearNameTags()
	{
		NameTags.GetRigColor(NameTags.nameTags);
		foreach (VRRig activeRig in VRRigCache.ActiveRigs)
		{
			bool flag = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
			if (flag)
			{
				bool flag2 = !NameTags.nameTags.ContainsKey(activeRig);
				if (flag2)
				{
					NameTags.CreateTag(activeRig, NameTags.nameTags, "NameTag");
				}
				NameTags.UpdateTag(activeRig, NameTags.nameTags, NameTags.GetPlayerName(activeRig), Settings.NameTagFontSize, Settings.NameTagOffset, activeRig.playerColor);
			}
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00013404 File Offset: 0x00011604
	private static void ClearPlatformTags()
	{
		NameTags.GetRigColor(NameTags.platformTags);
		foreach (VRRig activeRig in VRRigCache.ActiveRigs)
		{
			bool flag = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
			if (flag)
			{
				bool flag2 = !NameTags.platformTags.ContainsKey(activeRig);
				if (flag2)
				{
					NameTags.CreateTag(activeRig, NameTags.platformTags, "PlatTag");
				}
				string text = NameTags.GetFps(activeRig);
				NameTags.UpdateTag(activeRig, NameTags.platformTags, "[" + text + "]", Settings.PlatformTagFontSize, Settings.PlatformTagOffset, Color.yellow);
			}
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000134E0 File Offset: 0x000116E0
	private static void ClearFpsTags()
	{
		NameTags.GetRigColor(NameTags.fpsTags);
		foreach (VRRig activeRig in VRRigCache.ActiveRigs)
		{
			bool flag = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig;
			if (!flag)
			{
				bool flag2 = !NameTags.fpsTags.ContainsKey(activeRig);
				if (flag2)
				{
					NameTags.CreateTag(activeRig, NameTags.fpsTags, "FPSTag");
				}
				string text = "N/A";
				bool flag3 = NameTags.rigNameField != null;
				if (flag3)
				{
					object value = NameTags.rigNameField.GetValue(activeRig);
					bool flag4 = value == null;
					if (flag4)
					{
						goto IL_00C1;
					}
					object obj = value.ToString();
					bool flag5 = obj != null;
					if (!flag5)
					{
						goto IL_00C1;
					}
					IL_00F5:
					text = (string)obj;
					goto IL_00CB;
					IL_00C1:
					obj = "N/A";
					goto IL_00F5;
				}
				IL_00CB:
				NameTags.UpdateTag(activeRig, NameTags.fpsTags, "FPS: " + text, Settings.FPSTagFontSize, Settings.FPSTagOffset, NameTags.ParseColor(text));
			}
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00013614 File Offset: 0x00011814
	private static void CreateTag(VRRig rig, Dictionary<VRRig, GameObject> tagDict, string text)
	{
		GameObject val = new GameObject(text);
		val.hideFlags = 61;
		val.transform.localScale = Vector3.one * 0.25f;
		TextMeshPro val2 = val.AddComponent<TextMeshPro>();
		val2.enableAutoSizing = false;
		val2.fontSize = 3f;
		val2.alignment = 514;
		val2.fontStyle = 1;
		tagDict[rig] = val;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00013688 File Offset: 0x00011888
	private static void UpdateTag(VRRig rig, Dictionary<VRRig, GameObject> tagDict, string text, float fontSize, float offset, Color color)
	{
		bool flag = !tagDict.ContainsKey(rig);
		if (!flag)
		{
			GameObject val = tagDict[rig];
			bool flag2 = val == null;
			if (!flag2)
			{
				Transform val2 = ((rig.headMesh != null) ? rig.headMesh.transform : rig.transform);
				bool flag3 = Camera.main != null && Vector3.Distance(Camera.main.transform.position, val2.position) > Settings.NameTagRenderDistance;
				if (flag3)
				{
					bool activeSelf = val.activeSelf;
					if (activeSelf)
					{
						val.SetActive(false);
					}
				}
				else
				{
					bool flag4 = !val.activeSelf;
					if (flag4)
					{
						val.SetActive(true);
					}
					TextMeshPro component = val.GetComponent<TextMeshPro>();
					component.text = text;
					component.fontSize = fontSize;
					component.color = color;
					val.transform.position = val2.position + Vector3.up * offset;
					bool flag5 = Camera.main != null;
					if (flag5)
					{
						Vector3 forward = Camera.main.transform.forward;
						forward.y = 0f;
						bool flag6 = forward.sqrMagnitude > 0.001f;
						if (flag6)
						{
							val.transform.rotation = Quaternion.LookRotation(forward);
						}
					}
				}
			}
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000137F0 File Offset: 0x000119F0
	private static string GetPlayerName(VRRig rig)
	{
		string text;
		try
		{
			NetPlayer owningNetPlayer = rig.OwningNetPlayer;
			bool flag = owningNetPlayer == null;
			object obj;
			if (!flag)
			{
				obj = owningNetPlayer.NickName;
				bool flag2 = obj != null;
				if (flag2)
				{
					goto IL_0033;
				}
			}
			obj = "Unknown";
			IL_0033:
			text = (string)obj;
		}
		catch
		{
			text = "Unknown";
		}
		return text;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00013858 File Offset: 0x00011A58
	private static string GetPlatform(VRRig rig)
	{
		bool flag = NameTags.platformField == null;
		if (flag)
		{
			foreach (string name in NameTags.platformNames)
			{
				NameTags.platformField = typeof(VRRig).GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				bool flag2 = NameTags.platformField != null;
				if (flag2)
				{
					break;
				}
			}
		}
		string text;
		bool flag3;
		if (NameTags.platformField != null)
		{
			text = NameTags.platformField.GetValue(rig) as string;
			flag3 = text != null;
		}
		else
		{
			flag3 = false;
		}
		bool flag4 = flag3;
		string text3;
		if (flag4)
		{
			text3 = text.ToLower();
		}
		else
		{
			try
			{
				PropertyInfo property = typeof(VRRig).GetProperty("rawCosmeticString", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				string text2;
				bool flag5;
				if (property != null)
				{
					text2 = property.GetValue(rig) as string;
					flag5 = text2 != null;
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				if (flag6)
				{
					return text2.ToLower();
				}
			}
			catch
			{
			}
			text3 = "";
		}
		return text3;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0001396C File Offset: 0x00011B6C
	private static string GetFps(VRRig rig)
	{
		try
		{
			string text = NameTags.GetPlatform(rig);
			bool flag = !string.IsNullOrEmpty(text);
			if (flag)
			{
				bool flag2 = text.Contains("s. first login");
				if (flag2)
				{
					return "STEAM";
				}
				bool flag3 = text.Contains("first login") || text.Contains("game-purchase");
				if (flag3)
				{
					return "PC";
				}
			}
			try
			{
				bool flag4 = rig.Creator != null;
				if (flag4)
				{
					Player playerRef = rig.Creator.GetPlayerRef();
					bool flag5 = playerRef != null && playerRef.CustomProperties != null && ((Dictionary<object, object>)playerRef.CustomProperties).Count > 1;
					if (flag5)
					{
						return "PC";
					}
				}
			}
			catch
			{
			}
		}
		catch
		{
		}
		return "META";
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00013A60 File Offset: 0x00011C60
	private static Color ParseColor(string text)
	{
		int result = 0;
		bool flag = !int.TryParse(text, out result);
		Color color;
		if (flag)
		{
			color = Color.white;
		}
		else
		{
			bool flag2 = result >= 70;
			if (flag2)
			{
				color = Color.green;
			}
			else
			{
				int num3 = result;
				bool flag3 = num3 >= 58;
				if (flag3)
				{
					color = Color.yellow;
				}
				else
				{
					color = Color.red;
				}
			}
		}
		return color;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00013AC4 File Offset: 0x00011CC4
	public static string GetDisplayName(VRRig rig)
	{
		bool flag = !(rig == null);
		string text3;
		if (flag)
		{
			string text = NameTags.GetPlayerName(rig);
			string text2 = NameTags.GetFps(rig);
			float num3 = 0f;
			bool flag2 = NameTags.lastPositions.ContainsKey(rig);
			if (flag2)
			{
				num3 = Vector3.Distance(rig.transform.position, NameTags.lastPositions[rig]) / Time.deltaTime;
				NameTags.lastPositions[rig] = rig.transform.position;
				NameTags.lastUpdate[rig] = num3;
				float num4 = 0f;
				bool flag3 = !(GorillaTagger.Instance != null);
				if (!flag3)
				{
					num4 = Vector3.Distance(rig.transform.position, GorillaTagger.Instance.bodyCollider.transform.position);
				}
				text3 = string.Format("{0} | {1} | {2:F1}m | {3:F1} m/s", new object[] { text, text2, num4, num3 });
			}
			else
			{
				NameTags.lastPositions[rig] = rig.transform.position;
				NameTags.lastUpdate[rig] = num3;
				float num4 = 0f;
				bool flag4 = !(GorillaTagger.Instance != null);
				if (!flag4)
				{
					num4 = Vector3.Distance(rig.transform.position, GorillaTagger.Instance.bodyCollider.transform.position);
				}
				text3 = string.Format("{0} | {1} | {2:F1}m | {3:F1} m/s", new object[] { text, text2, num4, num3 });
			}
		}
		else
		{
			text3 = "";
		}
		return text3;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00013C80 File Offset: 0x00011E80
	private static void GetRigColor(Dictionary<VRRig, GameObject> tagDict)
	{
		List<VRRig> list = new List<VRRig>();
		foreach (KeyValuePair<VRRig, GameObject> item in tagDict)
		{
			bool flag = item.Key == null || VRRigCache.ActiveRigs == null || !VRRigCache.ActiveRigs.Contains(item.Key);
			if (flag)
			{
				bool flag2 = item.Value != null;
				if (flag2)
				{
					Object.Destroy(item.Value);
				}
				list.Add(item.Key);
			}
		}
		foreach (VRRig item2 in list)
		{
			tagDict.Remove(item2);
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00013D7C File Offset: 0x00011F7C
	private static void EnsureReflection(Dictionary<VRRig, GameObject> tagDict)
	{
		foreach (KeyValuePair<VRRig, GameObject> item in tagDict)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value);
			}
		}
		tagDict.Clear();
	}

	// Token: 0x04000228 RID: 552
	private static readonly Dictionary<VRRig, GameObject> nameTags = new Dictionary<VRRig, GameObject>();

	// Token: 0x04000229 RID: 553
	private static readonly Dictionary<VRRig, GameObject> platformTags = new Dictionary<VRRig, GameObject>();

	// Token: 0x0400022A RID: 554
	private static readonly Dictionary<VRRig, GameObject> fpsTags = new Dictionary<VRRig, GameObject>();

	// Token: 0x0400022B RID: 555
	private static readonly Dictionary<VRRig, Vector3> lastPositions = new Dictionary<VRRig, Vector3>();

	// Token: 0x0400022C RID: 556
	private static readonly Dictionary<VRRig, float> lastUpdate = new Dictionary<VRRig, float>();

	// Token: 0x0400022D RID: 557
	private static FieldInfo rigNameField;

	// Token: 0x0400022E RID: 558
	private static FieldInfo platformField;

	// Token: 0x0400022F RID: 559
	private static readonly string[] platformNames = new string[] { "rawCosmeticString", "concatStringOfCosmeticsAllowed", "cosmeticsAllowed", "cosmetics" };
}
