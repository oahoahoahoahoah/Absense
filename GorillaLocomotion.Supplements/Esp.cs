using System;
using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000012 RID: 18
public static class Esp
{
	// Token: 0x0600004B RID: 75 RVA: 0x0000654C File Offset: 0x0000474C
	public static void Update()
	{
		bool flag = !Settings.ESPEnabled;
		if (flag)
		{
			Esp.ClearAll();
		}
		else
		{
			bool flag2 = !(GorillaParent.instance == null) && VRRigCache.ActiveRigs != null && !(GorillaTagger.Instance == null);
			if (flag2)
			{
				bool tracersEnabled = Settings.TracersEnabled;
				if (tracersEnabled)
				{
					Esp.DrawTracers();
				}
				else
				{
					Esp.ClearTracers();
				}
				bool hitboxesEnabled = Settings.HitboxesEnabled;
				if (hitboxesEnabled)
				{
					Esp.DrawHitboxes();
				}
				else
				{
					Esp.ClearHitboxes();
				}
				bool cornerESPEnabled = Settings.CornerESPEnabled;
				if (cornerESPEnabled)
				{
					Esp.DrawBoxes();
				}
				else
				{
					Esp.ClearBoxes();
				}
				bool flag3 = Settings.CornerESPEnabled && Settings.FillESPEnabled;
				if (flag3)
				{
					Esp.DrawBoxFill();
				}
				else
				{
					Esp.ClearBoxFill();
				}
				bool nameTagsEnabled = Settings.NameTagsEnabled;
				if (nameTagsEnabled)
				{
					Esp.DrawNameTags();
				}
				else
				{
					Esp.ClearNameTags();
				}
				bool boneESPEnabled = Settings.BoneESPEnabled;
				if (boneESPEnabled)
				{
					Esp.DrawBones();
				}
				else
				{
					Esp.ClearBones();
				}
				bool chamsEnabled = Settings.ChamsEnabled;
				if (chamsEnabled)
				{
					Esp.ApplyChams();
				}
				else
				{
					Esp.ClearChams();
				}
				bool recolorTaggedEnabled = Settings.RecolorTaggedEnabled;
				if (recolorTaggedEnabled)
				{
					Esp.ApplyRecolorTagged();
				}
				else
				{
					Esp.ClearRecolorTagged();
				}
				bool distanceESPEnabled = Settings.DistanceESPEnabled;
				if (distanceESPEnabled)
				{
					Esp.DrawDistanceHud();
				}
				else
				{
					Esp.ClearDistanceHud();
				}
				bool beaconsEnabled = Settings.BeaconsEnabled;
				if (beaconsEnabled)
				{
					Esp.DrawBeacons();
				}
				else
				{
					Esp.ClearBeacons();
				}
				bool chinaHatESPEnabled = Settings.ChinaHatESPEnabled;
				if (chinaHatESPEnabled)
				{
					Esp.DrawChinaHats();
				}
				else
				{
					Esp.ClearChinaHats();
				}
				bool ringESPEnabled = Settings.RingESPEnabled;
				if (ringESPEnabled)
				{
					Esp.DrawRings();
				}
				else
				{
					Esp.ClearRings();
				}
			}
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00006708 File Offset: 0x00004908
	public static Color GetEspColor()
	{
		return Color.HSVToRGB(Settings.ESPColorHue, Settings.ESPColorSat, Settings.ESPColorVal);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00006730 File Offset: 0x00004930
	private static Material GetLineMaterial()
	{
		bool flag = !(Esp.lineMaterial == null);
		if (!flag)
		{
			Esp.lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			bool flag2 = Esp.lineMaterial != null;
			if (flag2)
			{
				Material val2 = Esp.lineMaterial;
				val2.SetInt("_SrcBlend", 5);
				Esp.lineMaterial.SetInt("_DstBlend", 10);
				Esp.lineMaterial.SetInt("_Cull", 0);
				Material val3 = Esp.lineMaterial;
				val3.SetInt("_ZWrite", 0);
				Material val4 = Esp.lineMaterial;
				val4.SetInt("_ZTest", 8);
			}
		}
		return Esp.lineMaterial;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000067E8 File Offset: 0x000049E8
	private static void DrawTracers()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			Color val = Esp.GetEspColor();
			Vector3 position = GorillaTagger.Instance.rightHandTransform.position;
			HashSet<int> hashSet = new HashSet<int>();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
				if (flag2)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					LineRenderer value;
					bool flag3 = !Esp.boxObjects.TryGetValue(instanceID, out value) || value == null;
					if (flag3)
					{
						value = new GameObject("ESP_Tracer_" + instanceID.ToString())
						{
							hideFlags = 61
						}.AddComponent<LineRenderer>();
						value.material = Esp.GetLineMaterial();
						value.startWidth = 0.01f;
						value.endWidth = 0.01f;
						value.positionCount = 2;
						Esp.boxObjects[instanceID] = value;
					}
					value.startColor = val;
					value.endColor = val;
					value.SetPosition(0, position);
					value.SetPosition(1, activeRig.transform.position);
					value.enabled = true;
				}
			}
			Esp.boxStaleKeys.Clear();
			foreach (KeyValuePair<int, LineRenderer> item in Esp.boxObjects)
			{
				bool flag4 = !hashSet.Contains(item.Key);
				if (flag4)
				{
					bool flag5 = item.Value != null;
					if (flag5)
					{
						Object.Destroy(item.Value.gameObject);
					}
					Esp.boxStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.boxStaleKeys)
			{
				Esp.boxObjects.Remove(item2);
			}
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00006A98 File Offset: 0x00004C98
	private static void ClearTracers()
	{
		foreach (KeyValuePair<int, LineRenderer> item in Esp.boxObjects)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value.gameObject);
			}
		}
		Esp.boxObjects.Clear();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00006B18 File Offset: 0x00004D18
	private static void DrawBeacons()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			HashSet<int> hashSet = new HashSet<int>();
			RaycastHit val = default(RaycastHit);
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
				if (flag2)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					LineRenderer value;
					bool flag3 = !Esp.fillObjects.TryGetValue(instanceID, out value) || value == null;
					if (flag3)
					{
						value = new GameObject("ESP_Beacon_" + instanceID.ToString())
						{
							hideFlags = 61
						}.AddComponent<LineRenderer>();
						value.material = Esp.GetLineMaterial();
						value.positionCount = 2;
						value.numCapVertices = 4;
						Esp.fillObjects[instanceID] = value;
					}
					Color val2 = (value.startColor = activeRig.playerColor);
					Color endColor = val2;
					value.endColor = endColor;
					value.startWidth = Settings.BeaconWidth;
					value.endWidth = Settings.BeaconWidth;
					Vector3 val3 = activeRig.transform.position;
					bool flag4 = Physics.Raycast(val3 + Vector3.up * 0.5f, Vector3.down, ref val, 3f);
					if (flag4)
					{
						val3 = val.point;
					}
					value.SetPosition(0, val3);
					value.SetPosition(1, val3 + Vector3.up * 5000f);
					value.enabled = true;
				}
			}
			Esp.fillStaleKeys.Clear();
			foreach (KeyValuePair<int, LineRenderer> item in Esp.fillObjects)
			{
				bool flag5 = !hashSet.Contains(item.Key);
				if (flag5)
				{
					bool flag6 = item.Value != null;
					if (flag6)
					{
						Object.Destroy(item.Value.gameObject);
					}
					Esp.fillStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.fillStaleKeys)
			{
				Esp.fillObjects.Remove(item2);
			}
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00006E20 File Offset: 0x00005020
	private static void ClearBeacons()
	{
		foreach (KeyValuePair<int, LineRenderer> item in Esp.fillObjects)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value.gameObject);
			}
		}
		Esp.fillObjects.Clear();
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00006EA0 File Offset: 0x000050A0
	private static void DrawChinaHats()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			Color white = Color.white;
			HashSet<int> hashSet = new HashSet<int>();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig) && !(activeRig.headMesh == null);
				if (flag2)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					LineRenderer value;
					bool flag3 = !Esp.nameTagObjects.TryGetValue(instanceID, out value) || value == null;
					if (flag3)
					{
						value = new GameObject("ESP_ChinaHat_" + instanceID.ToString())
						{
							hideFlags = 61
						}.AddComponent<LineRenderer>();
						value.material = Esp.GetLineMaterial();
						value.numCapVertices = 2;
						Esp.nameTagObjects[instanceID] = value;
					}
					Color val = (value.startColor = white);
					Color endColor = val;
					value.endColor = endColor;
					value.startWidth = 0.012f;
					value.endWidth = 0.012f;
					Vector3 val2 = activeRig.headMesh.transform.position + Vector3.up * 0.2f;
					Vector3 val3 = val2 + Vector3.up * 0.3f;
					value.positionCount = 25;
					Vector3[] array = (Vector3[])new Vector3[12];
					for (int i = 0; i < 12; i++)
					{
						float num = (float)i / 12f * 3.14159274f * 2f;
						array[i] = val2 + new Vector3(Mathf.Cos(num) * 0.3f, 0f, Mathf.Sin(num) * 0.3f);
					}
					int num2 = 0;
					for (int j = 0; j < 12; j++)
					{
						value.SetPosition(num2++, val3);
						value.SetPosition(num2++, array[j]);
					}
					value.positionCount = 37;
					num2 = 0;
					for (int k = 0; k < 12; k++)
					{
						int num3 = (k + 1) % 12;
						value.SetPosition(num2++, val3);
						value.SetPosition(num2++, array[k]);
						value.SetPosition(num2++, array[num3]);
					}
					value.SetPosition(num2++, val3);
					value.enabled = true;
				}
			}
			Esp.nameTagStaleKeys.Clear();
			foreach (KeyValuePair<int, LineRenderer> item in Esp.nameTagObjects)
			{
				bool flag4 = !hashSet.Contains(item.Key);
				if (flag4)
				{
					bool flag5 = item.Value != null;
					if (flag5)
					{
						Object.Destroy(item.Value.gameObject);
					}
					Esp.nameTagStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.nameTagStaleKeys)
			{
				Esp.nameTagObjects.Remove(item2);
			}
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000072B8 File Offset: 0x000054B8
	private static void ClearChinaHats()
	{
		foreach (KeyValuePair<int, LineRenderer> item in Esp.nameTagObjects)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value.gameObject);
			}
		}
		Esp.nameTagObjects.Clear();
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00007338 File Offset: 0x00005538
	private static void DrawRings()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			Color val = Esp.GetEspColor();
			float num = (Mathf.Sin(Time.time * 2f) + 1f) * 0.5f;
			HashSet<int> hashSet = new HashSet<int>();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
				if (flag2)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					LineRenderer value;
					bool flag3 = !Esp.boneObjects.TryGetValue(instanceID, out value) || value == null;
					if (flag3)
					{
						value = new GameObject("ESP_Ring_" + instanceID.ToString())
						{
							hideFlags = 61
						}.AddComponent<LineRenderer>();
						value.material = Esp.GetLineMaterial();
						value.numCapVertices = 2;
						Esp.boneObjects[instanceID] = value;
					}
					Vector3 val2 = activeRig.transform.position + Vector3.down;
					float num2 = num;
					Color val3 = (Esp.IsValidTarget(activeRig) ? Color.red : val);
					Color val4 = (value.startColor = val3);
					Color endColor = val4;
					value.endColor = endColor;
					value.startWidth = 0.03f;
					value.endWidth = 0.03f;
					value.positionCount = 25;
					Vector3 val5 = val2 + Vector3.up * num2;
					for (int i = 0; i <= 24; i++)
					{
						float num3 = (float)i / 24f * 3.14159274f * 2f;
						float num4 = Mathf.Cos(num3) * 0.5f;
						float num5 = Mathf.Sin(num3) * 0.5f;
						value.SetPosition(i, val5 + new Vector3(num4, 0f, num5));
					}
					value.enabled = true;
				}
			}
			Esp.boneStaleKeys.Clear();
			foreach (KeyValuePair<int, LineRenderer> item in Esp.boneObjects)
			{
				bool flag4 = !hashSet.Contains(item.Key);
				if (flag4)
				{
					bool flag5 = item.Value != null;
					if (flag5)
					{
						Object.Destroy(item.Value.gameObject);
					}
					Esp.boneStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.boneStaleKeys)
			{
				Esp.boneObjects.Remove(item2);
			}
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000076A0 File Offset: 0x000058A0
	private static void ClearRings()
	{
		foreach (KeyValuePair<int, LineRenderer> item in Esp.boneObjects)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value.gameObject);
			}
		}
		Esp.boneObjects.Clear();
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00007720 File Offset: 0x00005920
	private static void DrawHitboxes()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			Color color = Esp.GetEspColor();
			color.a = 0.4f;
			HashSet<int> hashSet = new HashSet<int>();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig;
				if (!flag2)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					GameObject value;
					bool flag3 = !Esp.chamsOriginals.TryGetValue(instanceID, out value) || value == null;
					if (flag3)
					{
						value = GameObject.CreatePrimitive(3);
						value.name = "ESP_Hitbox_" + instanceID.ToString();
						value.hideFlags = 61;
						Object.Destroy(value.GetComponent<Collider>());
						Renderer component = value.GetComponent<Renderer>();
						bool flag4 = component != null;
						if (flag4)
						{
							component.material = Esp.GetLineMaterial();
						}
						value.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
						Esp.chamsOriginals[instanceID] = value;
					}
					bool flag5 = activeRig.headMesh != null;
					if (flag5)
					{
						value.transform.position = activeRig.headMesh.transform.position;
					}
					else
					{
						value.transform.position = activeRig.transform.position + Vector3.up * 0.3f;
					}
					Renderer component2 = value.GetComponent<Renderer>();
					bool flag6 = component2 != null && component2.material != null;
					if (flag6)
					{
						component2.material.color = color;
					}
					value.SetActive(true);
				}
			}
			Esp.chamsStaleKeys.Clear();
			foreach (KeyValuePair<int, GameObject> item in Esp.chamsOriginals)
			{
				bool flag7 = !hashSet.Contains(item.Key);
				if (flag7)
				{
					bool flag8 = item.Value != null;
					if (flag8)
					{
						Object.Destroy(item.Value);
					}
					Esp.chamsStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.chamsStaleKeys)
			{
				Esp.chamsOriginals.Remove(item2);
			}
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00007A58 File Offset: 0x00005C58
	private static void ClearHitboxes()
	{
		foreach (KeyValuePair<int, GameObject> item in Esp.chamsOriginals)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value);
			}
		}
		Esp.chamsOriginals.Clear();
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00007AD4 File Offset: 0x00005CD4
	private static void DrawBoxes()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			bool flag2 = Esp.scratchA != Settings.BoxESPMode;
			if (flag2)
			{
				Esp.ClearBoxes();
				Esp.scratchA = Settings.BoxESPMode;
			}
			Color color = Esp.GetEspColor();
			HashSet<int> hashSet = new HashSet<int>();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag3 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig;
				if (!flag3)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					GameObject value;
					bool flag4 = !Esp.recolorOriginals.TryGetValue(instanceID, out value) || value == null;
					if (flag4)
					{
						value = ((Settings.BoxESPMode != 1) ? Esp.CreateCornerBox(instanceID) : Esp.Create3DBox(instanceID));
						Esp.recolorOriginals[instanceID] = value;
					}
					Vector3 position = activeRig.transform.position;
					Vector3 val = position - GorillaTagger.Instance.headCollider.transform.position;
					bool flag5 = val != Vector3.zero;
					if (flag5)
					{
						value.transform.rotation = Quaternion.LookRotation(val);
					}
					value.transform.position = position;
					Renderer[] componentsInChildren = value.GetComponentsInChildren<Renderer>();
					Renderer[] array = componentsInChildren;
					foreach (Renderer val2 in array)
					{
						bool flag6 = val2.material != null;
						if (flag6)
						{
							val2.material.color = color;
						}
					}
					value.SetActive(true);
				}
			}
			Esp.recolorStaleKeys.Clear();
			foreach (KeyValuePair<int, GameObject> item in Esp.recolorOriginals)
			{
				bool flag7 = !hashSet.Contains(item.Key);
				if (flag7)
				{
					bool flag8 = item.Value != null;
					if (flag8)
					{
						Object.Destroy(item.Value);
					}
					Esp.recolorStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.recolorStaleKeys)
			{
				Esp.recolorOriginals.Remove(item2);
			}
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00007DD8 File Offset: 0x00005FD8
	private static GameObject CreateCornerBox(int mode)
	{
		GameObject val3 = new GameObject("ESP_Box_" + mode.ToString());
		Material val4 = Esp.GetLineMaterial();
		Vector3[] array = (Vector3[])new Vector3[]
		{
			new Vector3(0f, 0.45f, 0f),
			new Vector3(0f, -0.45f, 0f),
			new Vector3(0.25f, 0f, 0f),
			new Vector3(-0.25f, 0f, 0f)
		};
		Vector3[] array2 = (Vector3[])new Vector3[]
		{
			new Vector3(0.5f, 0.02f, 0.02f),
			new Vector3(0.5f, 0.02f, 0.02f),
			new Vector3(0.02f, 0.9f, 0.02f),
			new Vector3(0.02f, 0.9f, 0.02f)
		};
		int num3 = 0;
		for (;;)
		{
			bool flag = num3 < 4;
			if (!flag)
			{
				break;
			}
			GameObject val5 = GameObject.CreatePrimitive(3);
			val5.transform.SetParent(val3.transform, false);
			Object.Destroy(val5.GetComponent<Collider>());
			Renderer val6 = val5.GetComponent<Renderer>();
			bool flag2 = val6 != null;
			if (flag2)
			{
				bool flag3 = val4 != null;
				if (flag3)
				{
				}
			}
			num3++;
		}
		return val3;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00007F78 File Offset: 0x00006178
	private static GameObject Create2DBox(int mode)
	{
		GameObject val4 = new GameObject("ESP_Fill_" + mode.ToString());
		Material val5 = Esp.GetLineMaterial();
		GameObject val6 = GameObject.CreatePrimitive(3);
		Transform transform = val6.transform;
		Transform transform2 = val4.transform;
		transform.SetParent(transform2, false);
		bool flag = Settings.BoxESPMode != 1;
		if (flag)
		{
		}
		Object.Destroy(val6.GetComponent<Collider>());
		Renderer val7 = val6.GetComponent<Renderer>();
		bool flag2 = !(val7 != null);
		if (!flag2)
		{
			bool flag3 = val5 != null;
			if (flag3)
			{
			}
		}
		return val4;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00008020 File Offset: 0x00006220
	private static GameObject Create3DBox(int mode)
	{
		GameObject val3 = new GameObject("ESP_Corners_" + mode.ToString());
		Vector3[] array = (Vector3[])new Vector3[]
		{
			new Vector3(0.24f, 0.35f, 0f),
			new Vector3(0.33f, 0.26f, 0f),
			new Vector3(-0.24f, 0.35f, 0f),
			new Vector3(-0.33f, 0.26f, 0f),
			new Vector3(-0.24f, -0.55f, 0f),
			new Vector3(-0.33f, -0.46f, 0f),
			new Vector3(0.24f, -0.55f, 0f),
			new Vector3(0.33f, -0.46f, 0f)
		};
		Vector3[] array2 = (Vector3[])new Vector3[]
		{
			new Vector3(0.18f, 0.02f, 0.01f),
			new Vector3(0.02f, 0.18f, 0.01f),
			new Vector3(0.18f, 0.02f, 0.01f),
			new Vector3(0.02f, 0.18f, 0.01f),
			new Vector3(0.18f, 0.02f, 0.01f),
			new Vector3(0.02f, 0.18f, 0.01f),
			new Vector3(0.18f, 0.02f, 0.01f),
			new Vector3(0.02f, 0.18f, 0.01f)
		};
		Material val4 = Esp.GetLineMaterial();
		int num3 = 0;
		for (;;)
		{
			bool flag = num3 < 8;
			if (!flag)
			{
				break;
			}
			GameObject val5 = GameObject.CreatePrimitive(3);
			val5.transform.SetParent(val3.transform, false);
			Object.Destroy(val5.GetComponent<Collider>());
			Renderer val6 = val5.GetComponent<Renderer>();
			bool flag2 = !(val6 != null);
			if (!flag2)
			{
				bool flag3 = !(val4 != null);
				if (flag3)
				{
				}
			}
			num3++;
		}
		return val3;
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000082A4 File Offset: 0x000064A4
	private static void ClearBoxes()
	{
		foreach (KeyValuePair<int, GameObject> item in Esp.recolorOriginals)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value);
			}
		}
		Esp.recolorOriginals.Clear();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00008320 File Offset: 0x00006520
	private static void DrawBoxFill()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			bool flag2 = Esp.scratchB != Settings.BoxESPMode;
			if (flag2)
			{
				Esp.ClearBoxFill();
				Esp.scratchB = Settings.BoxESPMode;
			}
			Color color = Color.HSVToRGB(Settings.FillESPColorHue, Settings.FillESPColorSat, Settings.FillESPColorVal);
			color.a = Settings.FillESPOpacity;
			HashSet<int> hashSet = new HashSet<int>();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag3 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig;
				if (!flag3)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					GameObject value;
					bool flag4 = !Esp.beaconObjects.TryGetValue(instanceID, out value) || value == null;
					if (flag4)
					{
						value = Esp.Create2DBox(instanceID);
						Esp.beaconObjects[instanceID] = value;
					}
					Vector3 position = activeRig.transform.position;
					Vector3 val = position - GorillaTagger.Instance.headCollider.transform.position;
					bool flag5 = val != Vector3.zero;
					if (flag5)
					{
						value.transform.rotation = Quaternion.LookRotation(val);
					}
					value.transform.position = position;
					Renderer[] componentsInChildren = value.GetComponentsInChildren<Renderer>();
					Renderer[] array = componentsInChildren;
					foreach (Renderer val2 in array)
					{
						bool flag6 = val2.material != null;
						if (flag6)
						{
							val2.material.color = color;
						}
					}
					value.SetActive(true);
				}
			}
			Esp.beaconStaleKeys.Clear();
			foreach (KeyValuePair<int, GameObject> item in Esp.beaconObjects)
			{
				bool flag7 = !hashSet.Contains(item.Key);
				if (flag7)
				{
					bool flag8 = item.Value != null;
					if (flag8)
					{
						Object.Destroy(item.Value);
					}
					Esp.beaconStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.beaconStaleKeys)
			{
				Esp.beaconObjects.Remove(item2);
			}
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000862C File Offset: 0x0000682C
	private static void ClearBoxFill()
	{
		foreach (KeyValuePair<int, GameObject> item in Esp.beaconObjects)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value);
			}
		}
		Esp.beaconObjects.Clear();
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000086A8 File Offset: 0x000068A8
	private static void DrawNameTags()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag)
		{
			Color color = Esp.GetEspColor();
			HashSet<int> hashSet = new HashSet<int>();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
				if (flag2)
				{
					int instanceID = activeRig.GetInstanceID();
					hashSet.Add(instanceID);
					GameObject value;
					bool flag3 = !Esp.chinaHatObjects.TryGetValue(instanceID, out value) || value == null;
					if (flag3)
					{
						value = new GameObject("ESP_NameTag_" + instanceID.ToString());
						value.hideFlags = 61;
						TextMesh val = value.AddComponent<TextMesh>();
						val.alignment = 1;
						val.anchor = 4;
						val.fontSize = 24;
						val.characterSize = 0.02f;
						val.fontStyle = 1;
						Esp.chinaHatObjects[instanceID] = value;
					}
					string text = "Player";
					bool flag4 = activeRig.Creator != null;
					if (flag4)
					{
						text = activeRig.Creator.NickName;
					}
					Vector3 val2 = ((activeRig.headMesh != null) ? activeRig.headMesh.transform.position : (activeRig.transform.position + Vector3.up * 0.3f));
					value.transform.position = val2 + Vector3.up * 0.4f;
					bool flag5 = Camera.main != null;
					if (flag5)
					{
						value.transform.LookAt(Camera.main.transform);
						value.transform.Rotate(0f, 180f, 0f);
					}
					TextMesh component = value.GetComponent<TextMesh>();
					bool flag6 = component != null;
					if (flag6)
					{
						component.text = text;
						component.color = color;
					}
					value.SetActive(true);
				}
			}
			Esp.chinaHatStaleKeys.Clear();
			foreach (KeyValuePair<int, GameObject> item in Esp.chinaHatObjects)
			{
				bool flag7 = !hashSet.Contains(item.Key);
				if (flag7)
				{
					bool flag8 = item.Value != null;
					if (flag8)
					{
						Object.Destroy(item.Value);
					}
					Esp.chinaHatStaleKeys.Add(item.Key);
				}
			}
			foreach (int item2 in Esp.chinaHatStaleKeys)
			{
				Esp.chinaHatObjects.Remove(item2);
			}
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00008A14 File Offset: 0x00006C14
	private static void ClearNameTags()
	{
		foreach (KeyValuePair<int, GameObject> item in Esp.chinaHatObjects)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value);
			}
		}
		Esp.chinaHatObjects.Clear();
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00008A90 File Offset: 0x00006C90
	private static void DrawBones()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null;
		if (!flag)
		{
			List<VRRig> list = new List<VRRig>();
			foreach (KeyValuePair<VRRig, List<LineRenderer>> item in Esp.ringObjects)
			{
				bool flag2 = VRRigCache.ActiveRigs.Contains(item.Key);
				if (!flag2)
				{
					list.Add(item.Key);
					foreach (LineRenderer item2 in item.Value)
					{
						bool flag3 = item2 != null;
						if (flag3)
						{
							Object.Destroy(item2.gameObject);
						}
					}
				}
			}
			foreach (VRRig item3 in list)
			{
				Esp.ringObjects.Remove(item3);
			}
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag4;
				if (!(activeRig == null))
				{
					Object @object = activeRig;
					GorillaTagger instance = GorillaTagger.Instance;
					flag4 = @object == ((instance != null) ? instance.offlineVRRig : null);
				}
				else
				{
					flag4 = true;
				}
				bool flag5 = flag4;
				if (!flag5)
				{
					try
					{
						List<LineRenderer> value;
						bool flag6 = !Esp.ringObjects.TryGetValue(activeRig, out value);
						if (flag6)
						{
							value = new List<LineRenderer>();
							bool flag7 = activeRig.head != null && activeRig.head.rigTarget != null;
							if (flag7)
							{
								LineRenderer val = activeRig.head.rigTarget.gameObject.GetComponent<LineRenderer>();
								bool flag8 = val == null;
								if (flag8)
								{
									val = activeRig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
								}
								bool flag9 = Esp.initialized;
								if (flag9)
								{
									val.numCapVertices = 10;
									val.numCornerVertices = 5;
								}
								val.material = Esp.GetLineMaterial();
								val.positionCount = 2;
								value.Add(val);
							}
							bool flag10 = activeRig.mainSkin != null && activeRig.mainSkin.bones != null;
							if (flag10)
							{
								int num = Mathf.Min(19, activeRig.mainSkin.bones.Length / 2);
								for (int i = 0; i < num; i++)
								{
									int num2 = i * 2;
									bool flag11 = num2 + 1 >= Esp.boneParents.Length || Esp.boneParents[num2] >= activeRig.mainSkin.bones.Length || Esp.boneParents[num2 + 1] >= activeRig.mainSkin.bones.Length;
									if (!flag11)
									{
										Transform val2 = activeRig.mainSkin.bones[Esp.boneParents[num2]];
										bool flag12 = val2 != null;
										if (flag12)
										{
											LineRenderer val3 = val2.gameObject.GetComponent<LineRenderer>();
											bool flag13 = val3 == null;
											if (flag13)
											{
												val3 = val2.gameObject.AddComponent<LineRenderer>();
											}
											bool flag14 = Esp.initialized;
											if (flag14)
											{
												val3.numCapVertices = 10;
												val3.numCornerVertices = 5;
											}
											val3.material = Esp.GetLineMaterial();
											val3.positionCount = 2;
											value.Add(val3);
										}
									}
								}
							}
							Esp.ringObjects.Add(activeRig, value);
						}
						Color playerColor = activeRig.playerColor;
						bool flag15 = value.Count > 0 && value[0] != null && activeRig.head != null && activeRig.head.rigTarget != null;
						if (flag15)
						{
							LineRenderer val4 = value[0];
							val4.startWidth = 0.025f;
							val4.endWidth = 0.025f;
							val4.startColor = playerColor;
							val4.endColor = playerColor;
							val4.SetPosition(0, activeRig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
							val4.SetPosition(1, activeRig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));
						}
						bool flag16 = !(activeRig.mainSkin != null) || activeRig.mainSkin.bones == null;
						if (!flag16)
						{
							int j = 0;
							while (j < value.Count - 1 && j < 19)
							{
								bool flag17 = value[j + 1] == null;
								if (!flag17)
								{
									int num3 = j * 2;
									bool flag18 = num3 + 1 < Esp.boneParents.Length && Esp.boneParents[num3] < activeRig.mainSkin.bones.Length && Esp.boneParents[num3 + 1] < activeRig.mainSkin.bones.Length;
									if (flag18)
									{
										LineRenderer val5 = value[j + 1];
										val5.startWidth = 0.025f;
										val5.endWidth = 0.025f;
										val5.startColor = playerColor;
										val5.endColor = playerColor;
										Transform val6 = activeRig.mainSkin.bones[Esp.boneParents[num3]];
										Transform val7 = activeRig.mainSkin.bones[Esp.boneParents[num3 + 1]];
										bool flag19 = val6 != null && val7 != null;
										if (flag19)
										{
											val5.SetPosition(0, val6.position);
											val5.SetPosition(1, val7.position);
										}
									}
								}
								j++;
							}
						}
					}
					catch
					{
					}
				}
			}
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00009138 File Offset: 0x00007338
	private static void ClearBones()
	{
		foreach (KeyValuePair<VRRig, List<LineRenderer>> item in Esp.ringObjects)
		{
			foreach (LineRenderer item2 in item.Value)
			{
				bool flag = item2 != null;
				if (flag)
				{
					Object.Destroy(item2);
				}
			}
		}
		Esp.ringObjects.Clear();
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000091EC File Offset: 0x000073EC
	private static void UpdateNameTag()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null;
		if (!flag)
		{
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2;
				if (!(activeRig == null))
				{
					Object @object = activeRig;
					GorillaTagger instance = GorillaTagger.Instance;
					flag2 = @object == ((instance != null) ? instance.offlineVRRig : null);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (!flag3)
				{
					try
					{
						bool flag4 = activeRig.skeleton != null && activeRig.skeleton.renderer != null;
						if (flag4)
						{
							activeRig.skeleton.renderer.enabled = true;
							activeRig.skeleton.renderer.material.shader = Shader.Find("GUI/Text Shader");
							activeRig.skeleton.renderer.material.color = activeRig.playerColor;
						}
					}
					catch
					{
					}
				}
			}
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x0000931C File Offset: 0x0000751C
	private static void RemoveStaleNameTags()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null;
		if (!flag)
		{
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2;
				if (!(activeRig == null))
				{
					Object @object = activeRig;
					GorillaTagger instance = GorillaTagger.Instance;
					flag2 = @object == ((instance != null) ? instance.offlineVRRig : null);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (!flag3)
				{
					try
					{
						bool flag4 = activeRig.skeleton != null && activeRig.skeleton.renderer != null;
						if (flag4)
						{
							activeRig.skeleton.renderer.enabled = false;
							activeRig.skeleton.renderer.material.shader = Shader.Find("GorillaTag/UberShader");
							bool flag5 = activeRig.skeleton.renderer.material.name.Contains("gorilla_body");
							if (flag5)
							{
								activeRig.skeleton.renderer.material.color = activeRig.playerColor;
							}
						}
					}
					catch
					{
					}
				}
			}
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00009474 File Offset: 0x00007674
	private static bool IsValidTarget(VRRig rig)
	{
		bool flag;
		try
		{
			flag = rig.mainSkin.material.name.ToLower().Contains("fected");
		}
		catch
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x000094BC File Offset: 0x000076BC
	private static Material GetChamsMaterial()
	{
		bool flag = Esp.chamsMaterial == null;
		Material material;
		if (flag)
		{
			Esp.chamsMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			bool flag2 = !(Esp.chamsMaterial != null);
			if (!flag2)
			{
				Material val2 = Esp.chamsMaterial;
				val2.SetInt("_SrcBlend", 5);
				Material val3 = Esp.chamsMaterial;
				val3.SetInt("_DstBlend", 10);
				Material val4 = Esp.chamsMaterial;
				val4.SetInt("_Cull", 0);
				Material val5 = Esp.chamsMaterial;
				val5.SetInt("_ZWrite", 0);
				Material val6 = Esp.chamsMaterial;
				val6.SetInt("_ZTest", 8);
			}
			material = Esp.chamsMaterial;
		}
		else
		{
			material = Esp.chamsMaterial;
		}
		return material;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00009584 File Offset: 0x00007784
	private static void ApplyChamsColor(Renderer renderer, Color color)
	{
		bool flag = renderer == null;
		if (!flag)
		{
			Material val = Esp.GetChamsMaterial();
			bool flag2 = val == null;
			if (!flag2)
			{
				val.SetColor(Esp.scratchC, color);
				Material[] array = renderer.materials;
				int num3 = 0;
				for (;;)
				{
					bool flag3 = num3 >= array.Length;
					if (flag3)
					{
						break;
					}
					bool flag4 = array[num3] == null;
					if (!flag4)
					{
						array[num3].SetColor("_Color", color);
						Material val2 = array[num3];
						val2.SetInt("_SrcBlend", 5);
						Material val3 = array[num3];
						val3.SetInt("_DstBlend", 10);
						Material val4 = array[num3];
						val4.SetInt("_Cull", 0);
						Material val5 = array[num3];
						val5.SetInt("_ZWrite", 0);
						Material val6 = array[num3];
						val6.SetInt("_ZTest", 8);
					}
					num3++;
				}
			}
		}
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0000968C File Offset: 0x0000788C
	private static void ApplyChams()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || Esp.GetChamsMaterial() == null;
		if (!flag)
		{
			float num = (Mathf.Sin(Time.time * 3f) + 1f) * 0.5f;
			Color val = Color.HSVToRGB(Settings.ChamsTaggedColorAHue, Settings.ChamsTaggedColorASat, Settings.ChamsTaggedColorAVal);
			Color val2 = Color.Lerp(Color.HSVToRGB(Settings.ChamsTaggedColorBHue, Settings.ChamsTaggedColorBSat, Settings.ChamsTaggedColorBVal), val, num);
			Color.HSVToRGB(Settings.ChamsColorHue, Settings.ChamsColorSat, Settings.ChamsColorVal);
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2;
				if (!(activeRig == null))
				{
					Object @object = activeRig;
					GorillaTagger instance = GorillaTagger.Instance;
					flag2 = !(@object == ((instance != null) ? instance.offlineVRRig : null));
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					try
					{
						Color color_ = ((!Esp.IsValidTarget(activeRig)) ? activeRig.playerColor : val2);
						color_.a = 1f;
						Esp.ApplyChamsColor(activeRig.mainSkin, color_);
						Esp.ApplyChamsColor(activeRig.faceSkin, color_);
					}
					catch
					{
					}
				}
			}
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x000097F8 File Offset: 0x000079F8
	private static void ClearChams()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null;
		if (!flag)
		{
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag2;
				if (!(activeRig == null))
				{
					Object @object = activeRig;
					GorillaTagger instance = GorillaTagger.Instance;
					flag2 = !(@object == ((instance != null) ? instance.offlineVRRig : null));
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					try
					{
						Esp.ApplyRecolorColor(activeRig.mainSkin, activeRig.playerColor);
						Esp.ApplyRecolorColor(activeRig.faceSkin, activeRig.playerColor);
					}
					catch
					{
					}
				}
			}
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000098CC File Offset: 0x00007ACC
	private static void ApplyRecolorColor(Renderer renderer, Color color)
	{
		bool flag = !(renderer == null);
		if (flag)
		{
			Shader val = Shader.Find("GorillaTag/UberShader");
			Material[] array = renderer.materials;
			int num3 = 0;
			for (;;)
			{
				bool flag2 = num3 >= array.Length;
				if (flag2)
				{
					break;
				}
				bool flag3 = array[num3] == null;
				if (!flag3)
				{
					bool flag4 = !(val != null);
					if (flag4)
					{
					}
					array[num3].SetColor("_Color", color);
					Material val2 = array[num3];
					val2.SetInt("_SrcBlend", 1);
					Material val3 = array[num3];
					val3.SetInt("_DstBlend", 0);
					Material val4 = array[num3];
					val4.SetInt("_Cull", 2);
					Material val5 = array[num3];
					val5.SetInt("_ZWrite", 1);
					Material val6 = array[num3];
					val6.SetInt("_ZTest", 4);
				}
				num3++;
			}
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000099D0 File Offset: 0x00007BD0
	private static void ApplyRecolorTagged()
	{
		bool flag = GorillaParent.instance == null || VRRigCache.ActiveRigs == null;
		if (!flag)
		{
			Color color_ = Color.HSVToRGB(Settings.RecolorTaggedColorHue, Settings.RecolorTaggedColorSat, Settings.RecolorTaggedColorVal);
			color_.a = 1f;
			int num = Esp.ColorToKey(color_);
			bool flag2 = Esp.frameCounter != num;
			if (flag2)
			{
				Esp.ClearRecolorState();
				Esp.frameCounter = num;
			}
			Esp.activeIds.Clear();
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag3;
				if (!(activeRig == null))
				{
					Object @object = activeRig;
					GorillaTagger instance = GorillaTagger.Instance;
					flag3 = @object == ((instance != null) ? instance.offlineVRRig : null);
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				if (!flag4)
				{
					try
					{
						bool flag5 = Esp.IsValidTarget(activeRig);
						if (flag5)
						{
							Esp.ApplyChamsAdvanced(activeRig.mainSkin, color_, true);
							Esp.ApplyChamsAdvanced(activeRig.faceSkin, color_, false);
						}
						else
						{
							Esp.RestoreRenderer(activeRig.mainSkin);
							Esp.RestoreRenderer(activeRig.faceSkin);
						}
					}
					catch
					{
					}
				}
			}
			Esp.RestoreChams();
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00009B30 File Offset: 0x00007D30
	private static void ApplyChamsAdvanced(Renderer renderer, Color color, bool tagged)
	{
		bool flag2 = renderer == null;
		if (!flag2)
		{
			Material[] array = renderer.sharedMaterials;
			bool flag3 = array == null;
			if (!flag3)
			{
				bool flag = false;
				int num3 = 0;
				for (;;)
				{
					bool flag4 = num3 < array.Length;
					if (!flag4)
					{
						break;
					}
					Material val = array[num3];
					bool flag5 = !(val == null);
					if (flag5)
					{
						bool flag6 = !tagged;
						if (flag6)
						{
							bool flag7 = !Esp.IsTransparentMaterial(val);
							if (!flag7)
							{
								Texture val2 = Esp.GetMainTexture(val);
								Texture2D val3 = Esp.CopyTexture(val2);
								bool flag8 = !(val3 == null);
								if (flag8)
								{
									Esp.propertyBlock.Clear();
									renderer.GetPropertyBlock(Esp.propertyBlock, num3);
									bool flag9 = !(val3 != null);
									if (flag9)
									{
										Esp.propertyBlock.SetColor(Esp.BaseColorId, color);
										Esp.propertyBlock.SetColor(Esp.ColorId, color);
									}
									else
									{
										Esp.propertyBlock.SetTexture(Esp.BaseMapId, val3);
										Esp.propertyBlock.SetTexture(Esp.MainTexId, val3);
										Esp.propertyBlock.SetColor(Esp.BaseColorId, Color.white);
										Esp.propertyBlock.SetColor(Esp.ColorId, Color.white);
									}
									renderer.SetPropertyBlock(Esp.propertyBlock, num3);
									flag = true;
								}
								else
								{
									bool flag10 = val2 == null;
									if (flag10)
									{
										bool flag11 = !Esp.SupportsColorProperty(val);
										if (!flag11)
										{
											Esp.propertyBlock.Clear();
											renderer.GetPropertyBlock(Esp.propertyBlock, num3);
											bool flag12 = !(val3 != null);
											if (flag12)
											{
												Esp.propertyBlock.SetColor(Esp.BaseColorId, color);
												Esp.propertyBlock.SetColor(Esp.ColorId, color);
											}
											else
											{
												Esp.propertyBlock.SetTexture(Esp.BaseMapId, val3);
												Esp.propertyBlock.SetTexture(Esp.MainTexId, val3);
												Esp.propertyBlock.SetColor(Esp.BaseColorId, Color.white);
												Esp.propertyBlock.SetColor(Esp.ColorId, Color.white);
											}
											renderer.SetPropertyBlock(Esp.propertyBlock, num3);
											flag = true;
										}
									}
									else
									{
										Esp.propertyBlock.Clear();
										renderer.GetPropertyBlock(Esp.propertyBlock, num3);
										bool flag13 = !(val3 != null);
										if (flag13)
										{
											Esp.propertyBlock.SetColor(Esp.BaseColorId, color);
											Esp.propertyBlock.SetColor(Esp.ColorId, color);
										}
										else
										{
											Esp.propertyBlock.SetTexture(Esp.BaseMapId, val3);
											Esp.propertyBlock.SetTexture(Esp.MainTexId, val3);
											Esp.propertyBlock.SetColor(Esp.BaseColorId, Color.white);
											Esp.propertyBlock.SetColor(Esp.ColorId, Color.white);
										}
										renderer.SetPropertyBlock(Esp.propertyBlock, num3);
										flag = true;
									}
								}
							}
						}
						else
						{
							Texture val2 = Esp.GetMainTexture(val);
							Texture2D val3 = Esp.CopyTexture(val2);
							bool flag14 = !(val3 == null);
							if (flag14)
							{
								Esp.propertyBlock.Clear();
								renderer.GetPropertyBlock(Esp.propertyBlock, num3);
								bool flag15 = !(val3 != null);
								if (flag15)
								{
									Esp.propertyBlock.SetColor(Esp.BaseColorId, color);
									Esp.propertyBlock.SetColor(Esp.ColorId, color);
								}
								else
								{
									Esp.propertyBlock.SetTexture(Esp.BaseMapId, val3);
									Esp.propertyBlock.SetTexture(Esp.MainTexId, val3);
									Esp.propertyBlock.SetColor(Esp.BaseColorId, Color.white);
									Esp.propertyBlock.SetColor(Esp.ColorId, Color.white);
								}
								renderer.SetPropertyBlock(Esp.propertyBlock, num3);
								flag = true;
							}
							else
							{
								bool flag16 = val2 == null;
								if (flag16)
								{
									bool flag17 = !Esp.SupportsColorProperty(val);
									if (!flag17)
									{
										Esp.propertyBlock.Clear();
										renderer.GetPropertyBlock(Esp.propertyBlock, num3);
										bool flag18 = !(val3 != null);
										if (flag18)
										{
											Esp.propertyBlock.SetColor(Esp.BaseColorId, color);
											Esp.propertyBlock.SetColor(Esp.ColorId, color);
										}
										else
										{
											Esp.propertyBlock.SetTexture(Esp.BaseMapId, val3);
											Esp.propertyBlock.SetTexture(Esp.MainTexId, val3);
											Esp.propertyBlock.SetColor(Esp.BaseColorId, Color.white);
											Esp.propertyBlock.SetColor(Esp.ColorId, Color.white);
										}
										renderer.SetPropertyBlock(Esp.propertyBlock, num3);
										flag = true;
									}
								}
								else
								{
									Esp.propertyBlock.Clear();
									renderer.GetPropertyBlock(Esp.propertyBlock, num3);
									bool flag19 = !(val3 != null);
									if (flag19)
									{
										Esp.propertyBlock.SetColor(Esp.BaseColorId, color);
										Esp.propertyBlock.SetColor(Esp.ColorId, color);
									}
									else
									{
										Esp.propertyBlock.SetTexture(Esp.BaseMapId, val3);
										Esp.propertyBlock.SetTexture(Esp.MainTexId, val3);
										Esp.propertyBlock.SetColor(Esp.BaseColorId, Color.white);
										Esp.propertyBlock.SetColor(Esp.ColorId, Color.white);
									}
									renderer.SetPropertyBlock(Esp.propertyBlock, num3);
									flag = true;
								}
							}
						}
					}
					num3++;
				}
				bool flag20 = flag;
				if (flag20)
				{
					int instanceID = renderer.GetInstanceID();
					Esp.activeIds.Add(instanceID);
					Esp.hitboxObjects[instanceID] = renderer;
				}
				else
				{
					Esp.RestoreRenderer(renderer);
				}
			}
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0000A0F4 File Offset: 0x000082F4
	private static Texture GetMainTexture(Material material)
	{
		bool flag = material == null;
		Texture texture3;
		if (flag)
		{
			texture3 = null;
		}
		else
		{
			try
			{
				bool flag2 = material.HasProperty("_BaseMap");
				if (flag2)
				{
					Texture texture = material.GetTexture("_BaseMap");
					bool flag3 = texture != null;
					if (flag3)
					{
						return texture;
					}
				}
				bool flag4 = material.HasProperty("_MainTex");
				if (flag4)
				{
					Texture texture2 = material.GetTexture("_MainTex");
					bool flag5 = texture2 != null;
					if (flag5)
					{
						return texture2;
					}
				}
				texture3 = material.mainTexture;
			}
			catch
			{
				texture3 = null;
			}
		}
		return texture3;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000A198 File Offset: 0x00008398
	private static Texture2D CopyTexture(Texture source)
	{
		bool flag = source == null;
		Texture2D texture2D;
		if (flag)
		{
			texture2D = null;
		}
		else
		{
			int instanceID = source.GetInstanceID();
			Texture2D value;
			bool flag2 = Esp.tracerLines.TryGetValue(instanceID, out value) && value != null;
			if (flag2)
			{
				texture2D = value;
			}
			else
			{
				Texture2D val = Esp.CopyTextureReadable(source);
				bool flag3 = val == null;
				if (flag3)
				{
					texture2D = null;
				}
				else
				{
					try
					{
						Color[] pixels = val.GetPixels();
						for (int i = 0; i < pixels.Length; i++)
						{
							pixels[i] = Esp.GetTaggedColor(pixels[i]);
						}
						val.SetPixels(pixels);
						val.Apply(false, false);
						Esp.tracerLines[instanceID] = val;
						texture2D = val;
					}
					catch
					{
						Object.Destroy(val);
						texture2D = null;
					}
				}
			}
		}
		return texture2D;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0000A284 File Offset: 0x00008484
	private static Texture2D CopyTextureReadable(Texture source)
	{
		bool flag = source == null;
		Texture2D texture2D;
		if (flag)
		{
			texture2D = null;
		}
		else
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture val = null;
			Texture2D val2 = null;
			try
			{
				int num = Mathf.Max(1, source.width);
				int num2 = Mathf.Max(1, source.height);
				val = RenderTexture.GetTemporary(num, num2, 0, 0);
				Graphics.Blit(source, val);
				RenderTexture.active = val;
				val2 = new Texture2D(num, num2, 4, false);
				val2.hideFlags = 61;
				val2.wrapMode = source.wrapMode;
				val2.filterMode = source.filterMode;
				val2.anisoLevel = source.anisoLevel;
				val2.ReadPixels(new Rect(0f, 0f, (float)num, (float)num2), 0, 0);
				val2.Apply(false, false);
				texture2D = val2;
			}
			catch
			{
				bool flag2 = val2 != null;
				if (flag2)
				{
					Object.Destroy(val2);
				}
				texture2D = null;
			}
			finally
			{
				RenderTexture.active = active;
				bool flag3 = val != null;
				if (flag3)
				{
					RenderTexture.ReleaseTemporary(val);
				}
			}
		}
		return texture2D;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0000A3AC File Offset: 0x000085AC
	private static Color GetTaggedColor(Color color)
	{
		float num3 = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
		bool flag = num3 <= 0.03f;
		Color color2;
		if (flag)
		{
			color2 = color;
		}
		else
		{
			bool flag2 = color.a > 0f;
			if (flag2)
			{
				Color result = Color.HSVToRGB(Settings.RecolorTaggedColorHue, Settings.RecolorTaggedColorSat, Mathf.Clamp01(num3 * Settings.RecolorTaggedColorVal));
				color2 = result;
			}
			else
			{
				color2 = color;
			}
		}
		return color2;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000A42C File Offset: 0x0000862C
	private static bool IsTransparentMaterial(Material material)
	{
		bool flag = material == null;
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			try
			{
				string text = material.name.ToLower();
				bool flag3 = !text.Contains("fected") && !text.Contains("lava");
				if (flag3)
				{
					Texture val = Esp.GetMainTexture(material);
					bool flag4 = val == null;
					if (flag4)
					{
						flag2 = false;
					}
					else
					{
						string text2 = val.name.ToLower();
						flag2 = text2.Contains("fected") || text2.Contains("lava");
					}
				}
				else
				{
					flag2 = true;
				}
			}
			catch
			{
				flag2 = false;
			}
		}
		return flag2;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000A4E4 File Offset: 0x000086E4
	private static bool SupportsColorProperty(Material material)
	{
		bool flag = !(material != null);
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			bool flag3 = material.HasProperty("_BaseColor");
			flag2 = flag3 || material.HasProperty("_Color");
		}
		return flag2;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0000A52C File Offset: 0x0000872C
	private static int ColorToKey(Color color)
	{
		Color32 val = color;
		byte r = val.r;
		byte g = val.g;
		int num = (int)r | ((int)g << 8);
		byte b = val.b;
		return num | ((int)b << 16);
	}

	// Token: 0x06000074 RID: 116 RVA: 0x0000A56C File Offset: 0x0000876C
	private static void RestoreChams()
	{
		Esp.tracerStaleKeys.Clear();
		foreach (int key in Esp.hitboxObjects.Keys)
		{
			bool flag = !Esp.activeIds.Contains(key);
			if (flag)
			{
				Esp.tracerStaleKeys.Add(key);
			}
		}
		for (int i = 0; i < Esp.tracerStaleKeys.Count; i++)
		{
			Renderer value;
			bool flag2 = Esp.hitboxObjects.TryGetValue(Esp.tracerStaleKeys[i], out value) && value != null;
			if (flag2)
			{
				Esp.RestoreRenderer(value);
			}
			else
			{
				Esp.hitboxObjects.Remove(Esp.tracerStaleKeys[i]);
			}
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000A658 File Offset: 0x00008858
	private static void RestoreRenderer(Renderer renderer)
	{
		bool flag = renderer == null;
		if (!flag)
		{
			Material[] array = renderer.sharedMaterials;
			bool flag2 = array == null;
			if (!flag2)
			{
				int num3 = 0;
				for (;;)
				{
					bool flag3 = num3 >= array.Length;
					if (flag3)
					{
						break;
					}
					Esp.propertyBlock.Clear();
					renderer.SetPropertyBlock(Esp.propertyBlock, num3);
					num3++;
				}
			}
			int instanceID = renderer.GetInstanceID();
			Esp.hitboxObjects.Remove(instanceID);
			Esp.activeIds.Remove(instanceID);
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x0000A6EC File Offset: 0x000088EC
	private static void ClearRecolorTagged()
	{
		foreach (Renderer item in Esp.hitboxObjects.Values.ToList<Renderer>())
		{
			Esp.RestoreRenderer(item);
		}
		Esp.hitboxObjects.Clear();
		Esp.activeIds.Clear();
		Esp.ClearRecolorState();
		Esp.frameCounter = int.MinValue;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0000A774 File Offset: 0x00008974
	private static void ClearRecolorState()
	{
		foreach (Texture2D value in Esp.tracerLines.Values)
		{
			bool flag = value != null;
			if (flag)
			{
				Object.Destroy(value);
			}
		}
		Esp.tracerLines.Clear();
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000A7E8 File Offset: 0x000089E8
	private static void DrawDistanceHud()
	{
		bool flag4 = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
		if (!flag4)
		{
			Vector3 position = GorillaTagger.Instance.headCollider.transform.position;
			bool flag = Esp.IsValidTarget(GorillaTagger.Instance.offlineVRRig);
			float num = float.MaxValue;
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag5 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig;
				if (!flag5)
				{
					bool flag2 = Esp.IsValidTarget(activeRig);
					bool flag6 = (!flag || !flag2) && (flag || flag2);
					if (flag6)
					{
						float num2 = Vector3.Distance(position, activeRig.transform.position);
						bool flag7 = num2 < num;
						if (flag7)
						{
							num = num2;
						}
					}
				}
			}
			bool flag3 = !flag && num > 5f;
			string text = (flag ? "Hunting: " : (flag3 ? "Safe: " : "Not Safe: "));
			string text2 = ((num < float.MaxValue) ? (num.ToString("F1") + "m") : "---");
			Color color = (flag ? new Color(1f, 0.5f, 0f) : (flag3 ? Color.green : Color.red));
			GTPlayer instance = GTPlayer.Instance;
			bool flag8 = instance != null && instance.RightHand.controllerTransform != null && num < float.MaxValue;
			if (flag8)
			{
				bool flag9 = Esp.hudObject0 == null;
				if (flag9)
				{
					Esp.hudObject0 = new GameObject("GameSense_Hand");
					GameObject val = Esp.hudObject0;
					val.hideFlags = 61;
					TextMesh val2 = Esp.hudObject0.AddComponent<TextMesh>();
					val2.alignment = 1;
					val2.anchor = 7;
					val2.fontSize = 28;
					val2.characterSize = 0.014f;
					val2.fontStyle = 1;
				}
				Transform controllerTransform = instance.RightHand.controllerTransform;
				Esp.hudObject0.transform.position = controllerTransform.position + controllerTransform.up * 0.08f;
				bool flag10 = Camera.main != null;
				if (flag10)
				{
					Esp.hudObject0.transform.rotation = Quaternion.LookRotation(Esp.hudObject0.transform.position - Camera.main.transform.position);
				}
				TextMesh component = Esp.hudObject0.GetComponent<TextMesh>();
				bool flag11 = component != null;
				if (flag11)
				{
					component.text = text + text2;
					component.color = color;
				}
				Esp.hudObject0.SetActive(true);
			}
			else
			{
				bool flag12 = Esp.hudObject0 != null;
				if (flag12)
				{
					Esp.hudObject0.SetActive(false);
				}
			}
			Camera main = Camera.main;
			bool flag13 = Settings.DistanceESPHUD && main != null && num < float.MaxValue;
			if (flag13)
			{
				bool flag14 = Esp.hudObject1 == null;
				if (flag14)
				{
					Esp.hudObject1 = new GameObject("GameSense_HUD");
					Esp.hudObject1.hideFlags = 61;
					TextMesh val3 = Esp.hudObject1.AddComponent<TextMesh>();
					val3.alignment = 1;
					val3.anchor = 4;
					val3.fontSize = 48;
					val3.characterSize = 0.012f;
					val3.fontStyle = 1;
					MeshRenderer component2 = Esp.hudObject1.GetComponent<MeshRenderer>();
					bool flag15 = component2 != null;
					if (flag15)
					{
						Material material = component2.material;
						material.renderQueue = 5000;
						component2.shadowCastingMode = 0;
						component2.receiveShadows = false;
					}
				}
				Transform transform = main.transform;
				Esp.hudObject1.transform.position = transform.position + transform.forward * 2f + transform.up + transform.right;
				Esp.hudObject1.transform.rotation = Quaternion.LookRotation(Esp.hudObject1.transform.position - transform.position);
				TextMesh component3 = Esp.hudObject1.GetComponent<TextMesh>();
				bool flag16 = component3 != null;
				if (flag16)
				{
					component3.text = text + text2;
					component3.color = color;
				}
				GameObject val4 = Esp.hudObject1;
				val4.SetActive(true);
			}
			else
			{
				bool flag17 = Esp.hudObject1 != null;
				if (flag17)
				{
					GameObject val5 = Esp.hudObject1;
					val5.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x0000ACDC File Offset: 0x00008EDC
	private static void ClearDistanceHud()
	{
		bool flag = Esp.hudObject0 != null;
		if (flag)
		{
			Object.Destroy(Esp.hudObject0);
			Esp.hudObject0 = null;
			bool flag2 = !(Esp.hudObject1 != null);
			if (!flag2)
			{
				Object.Destroy(Esp.hudObject1);
				Esp.hudObject1 = null;
			}
			bool flag3 = !(Esp.hudObject2 != null);
			if (!flag3)
			{
				Object.Destroy(Esp.hudObject2);
				Esp.hudObject2 = null;
			}
			bool flag4 = Esp.hudObject3 != null;
			if (flag4)
			{
				Object.Destroy(Esp.hudObject3);
				Esp.hudObject3 = null;
			}
		}
		else
		{
			bool flag5 = !(Esp.hudObject1 != null);
			if (!flag5)
			{
				Object.Destroy(Esp.hudObject1);
				Esp.hudObject1 = null;
			}
			bool flag6 = !(Esp.hudObject2 != null);
			if (!flag6)
			{
				Object.Destroy(Esp.hudObject2);
				Esp.hudObject2 = null;
			}
			bool flag7 = Esp.hudObject3 != null;
			if (flag7)
			{
				Object.Destroy(Esp.hudObject3);
				Esp.hudObject3 = null;
			}
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x0000AE00 File Offset: 0x00009000
	private static GameObject CreateTextObject(string text, float size)
	{
		GameObject val = new GameObject(text);
		val.hideFlags = 61;
		int num = ((size > 0.06f) ? 48 : 40);
		GameObject val2 = new GameObject("Circle")
		{
			hideFlags = 61
		};
		Transform transform = val2.transform;
		Transform transform2 = val.transform;
		transform.SetParent(transform2, false);
		LineRenderer val3 = val2.AddComponent<LineRenderer>();
		val3.material = Esp.GetLineMaterial();
		val3.startWidth = 0.003f;
		val3.endWidth = 0.003f;
		val3.startColor = Color.green;
		val3.endColor = Color.green;
		val3.useWorldSpace = false;
		val3.positionCount = num + 1;
		for (int i = 0; i <= num; i++)
		{
			float num2 = (float)i / (float)num * 3.14159274f * 2f;
			val3.SetPosition(i, new Vector3(Mathf.Cos(num2) * size, Mathf.Sin(num2) * size, 0f));
		}
		GameObject val4 = new GameObject("Fill")
		{
			hideFlags = 61
		};
		val4.transform.SetParent(val.transform, false);
		MeshFilter val5 = val4.AddComponent<MeshFilter>();
		MeshRenderer val6 = val4.AddComponent<MeshRenderer>();
		val6.material = Esp.GetLineMaterial();
		val6.material.color = new Color(0f, 0f, 0f, 0.85f);
		Mesh val7 = new Mesh();
		Vector3[] array = (Vector3[])new Vector3[num + 1];
		array[0] = Vector3.zero;
		for (int j = 0; j < num; j++)
		{
			float num3 = (float)j / (float)num * 3.14159274f * 2f;
			array[j + 1] = new Vector3(Mathf.Cos(num3) * size, Mathf.Sin(num3) * size, 0.001f);
		}
		val7.vertices = array;
		int[] array2 = new int[num * 3];
		for (int k = 0; k < num; k++)
		{
			array2[k * 3] = 0;
			array2[k * 3 + 1] = k + 1;
			array2[k * 3 + 2] = (k + 1) % num + 1;
		}
		val7.triangles = array2;
		val7.RecalculateNormals();
		val5.mesh = val7;
		GameObject val8 = GameObject.CreatePrimitive(0);
		val8.name = "SelfDot";
		val8.hideFlags = 61;
		Transform transform3 = val8.transform;
		Transform transform4 = val.transform;
		transform3.SetParent(transform4, false);
		val8.transform.localPosition = Vector3.zero;
		val8.transform.localScale = Vector3.one * (size * 0.12f);
		Object.Destroy(val8.GetComponent<Collider>());
		Renderer component = val8.GetComponent<Renderer>();
		component.material = Esp.GetLineMaterial();
		component.material.color = Color.blue;
		GameObject val9 = new GameObject("Arrow")
		{
			hideFlags = 61
		};
		val9.transform.SetParent(val.transform, false);
		LineRenderer val10 = val9.AddComponent<LineRenderer>();
		val10.material = Esp.GetLineMaterial();
		val10.startWidth = 0.004f;
		val10.endWidth = 0.001f;
		val10.startColor = Color.red;
		val10.endColor = Color.red;
		val10.useWorldSpace = false;
		val10.positionCount = 2;
		val10.SetPosition(0, Vector3.zero);
		val10.SetPosition(1, Vector3.zero);
		return val;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000B1A8 File Offset: 0x000093A8
	private static void UpdateTextObject(GameObject textObject, Vector3 position, bool tagged, float size, float offset)
	{
		bool flag2 = textObject == null;
		if (!flag2)
		{
			Transform val = textObject.transform.Find("Arrow");
			LineRenderer val2 = ((val != null) ? val.GetComponent<LineRenderer>() : null);
			int num = 0;
			float num2 = float.MaxValue;
			Vector3 val3 = Vector3.zero;
			Vector3 forward = GorillaTagger.Instance.headCollider.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			float num3 = Mathf.Atan2(forward.x, forward.z);
			Vector2 val4 = default(Vector2);
			Vector3 val5 = default(Vector3);
			foreach (VRRig activeRig in VRRigCache.ActiveRigs)
			{
				bool flag3 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig;
				if (!flag3)
				{
					bool flag = Esp.IsValidTarget(activeRig);
					bool flag4 = (!tagged && !flag) || (tagged && flag);
					if (!flag4)
					{
						Vector3 val6 = activeRig.transform.position - position;
						float magnitude = val6.magnitude;
						bool flag5 = magnitude <= offset;
						if (flag5)
						{
							float num4 = magnitude / offset;
							val4 = new Vector2(val6.x, val6.z);
							float num5 = Mathf.Atan2(val4.x, val4.y) - num3;
							float num6 = Mathf.Sin(num5) * num4 * size;
							float num7 = Mathf.Cos(num5) * num4 * size;
							string text = "Dot_" + num.ToString();
							Transform val7 = textObject.transform.Find(text);
							bool flag6 = val7 == null;
							GameObject val8;
							if (flag6)
							{
								val8 = GameObject.CreatePrimitive(0);
								val8.name = text;
								val8.hideFlags = 61;
								val8.transform.SetParent(textObject.transform, false);
								val8.transform.localScale = Vector3.one * (size * 0.1f);
								Object.Destroy(val8.GetComponent<Collider>());
								Renderer component = val8.GetComponent<Renderer>();
								component.material = Esp.GetLineMaterial();
								component.material.color = Color.red;
							}
							else
							{
								val8 = val7.gameObject;
							}
							val8.transform.localPosition = new Vector3(num6, num7, -0.001f);
							val8.SetActive(true);
							num++;
							bool flag7 = magnitude < num2;
							if (flag7)
							{
								num2 = magnitude;
								val3 = new Vector3(num6, num7, 0f).normalized;
							}
						}
					}
				}
			}
			int num8 = num;
			for (;;)
			{
				int num9 = num8;
				int num10 = num;
				bool flag8 = num9 >= num10 + 20;
				if (flag8)
				{
					break;
				}
				Transform val9 = textObject.transform.Find("Dot_" + num8.ToString());
				bool flag9 = !(val9 != null);
				if (flag9)
				{
					break;
				}
				GameObject gameObject = val9.gameObject;
				gameObject.SetActive(false);
				num8++;
			}
			bool flag10 = val2 != null;
			if (flag10)
			{
				bool flag11 = num2 < float.MaxValue;
				if (flag11)
				{
					val2.SetPosition(0, Vector3.zero);
					val2.SetPosition(1, val3 * size);
					val2.enabled = true;
				}
				else
				{
					val2.enabled = false;
				}
			}
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x0000B548 File Offset: 0x00009748
	private static void ClearAll()
	{
		Esp.ClearTracers();
		Esp.ClearBeacons();
		Esp.ClearChinaHats();
		Esp.ClearRings();
		Esp.ClearHitboxes();
		Esp.ClearBoxes();
		Esp.ClearBoxFill();
		Esp.ClearNameTags();
		Esp.ClearBones();
		Esp.RemoveStaleNameTags();
		Esp.ClearChams();
		Esp.ClearRecolorTagged();
		Esp.ClearDistanceHud();
	}

	// Token: 0x0600007D RID: 125 RVA: 0x0000B5A4 File Offset: 0x000097A4
	static Esp()
	{
		int[] array = new int[]
		{
			4, 3, 3, 5, 5, 4, 5, 6, 6, 7,
			7, 8, 4, 9, 9, 10, 10, 11, 3, 12,
			12, 13, 13, 14, 14, 15, 13, 16, 16, 17,
			17, 18, 18, 19, 13, 20, 20, 21, 21, 22,
			22, 23
		};
		Esp.boneParents = array;
		Esp.scratchA = -1;
		Esp.scratchB = -1;
		Esp.scratchC = -1;
		Esp.lastDistanceText = null;
		Esp.lastDistanceSafe = true;
	}

	// Token: 0x0400017D RID: 381
	private static Material lineMaterial;

	// Token: 0x0400017E RID: 382
	private const string BaseMapProp = "_BaseMap";

	// Token: 0x0400017F RID: 383
	private const string MainTexProp = "_MainTex";

	// Token: 0x04000180 RID: 384
	private const string BaseColorProp = "_BaseColor";

	// Token: 0x04000181 RID: 385
	private const string ColorProp = "_Color";

	// Token: 0x04000182 RID: 386
	private static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");

	// Token: 0x04000183 RID: 387
	private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

	// Token: 0x04000184 RID: 388
	private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

	// Token: 0x04000185 RID: 389
	private static readonly int ColorId = Shader.PropertyToID("_Color");

	// Token: 0x04000186 RID: 390
	private static readonly MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

	// Token: 0x04000187 RID: 391
	private static readonly Dictionary<int, Texture2D> tracerLines = new Dictionary<int, Texture2D>();

	// Token: 0x04000188 RID: 392
	private static readonly Dictionary<int, Renderer> hitboxObjects = new Dictionary<int, Renderer>();

	// Token: 0x04000189 RID: 393
	private static readonly HashSet<int> activeIds = new HashSet<int>();

	// Token: 0x0400018A RID: 394
	private static readonly List<int> tracerStaleKeys = new List<int>();

	// Token: 0x0400018B RID: 395
	private static int frameCounter = int.MinValue;

	// Token: 0x0400018C RID: 396
	private static Dictionary<int, LineRenderer> boxObjects = new Dictionary<int, LineRenderer>();

	// Token: 0x0400018D RID: 397
	private static List<int> boxStaleKeys = new List<int>();

	// Token: 0x0400018E RID: 398
	private static Dictionary<int, LineRenderer> fillObjects = new Dictionary<int, LineRenderer>();

	// Token: 0x0400018F RID: 399
	private static List<int> fillStaleKeys = new List<int>();

	// Token: 0x04000190 RID: 400
	private static Dictionary<int, LineRenderer> nameTagObjects = new Dictionary<int, LineRenderer>();

	// Token: 0x04000191 RID: 401
	private static List<int> nameTagStaleKeys = new List<int>();

	// Token: 0x04000192 RID: 402
	private static Dictionary<int, LineRenderer> boneObjects = new Dictionary<int, LineRenderer>();

	// Token: 0x04000193 RID: 403
	private static List<int> boneStaleKeys = new List<int>();

	// Token: 0x04000194 RID: 404
	private static Dictionary<int, GameObject> chamsOriginals = new Dictionary<int, GameObject>();

	// Token: 0x04000195 RID: 405
	private static List<int> chamsStaleKeys = new List<int>();

	// Token: 0x04000196 RID: 406
	private static Dictionary<int, GameObject> recolorOriginals = new Dictionary<int, GameObject>();

	// Token: 0x04000197 RID: 407
	private static List<int> recolorStaleKeys = new List<int>();

	// Token: 0x04000198 RID: 408
	private static Dictionary<int, GameObject> beaconObjects = new Dictionary<int, GameObject>();

	// Token: 0x04000199 RID: 409
	private static List<int> beaconStaleKeys = new List<int>();

	// Token: 0x0400019A RID: 410
	private static Dictionary<int, GameObject> chinaHatObjects = new Dictionary<int, GameObject>();

	// Token: 0x0400019B RID: 411
	private static List<int> chinaHatStaleKeys = new List<int>();

	// Token: 0x0400019C RID: 412
	private static Dictionary<VRRig, List<LineRenderer>> ringObjects = new Dictionary<VRRig, List<LineRenderer>>();

	// Token: 0x0400019D RID: 413
	private static bool initialized = true;

	// Token: 0x0400019E RID: 414
	private static readonly int[] boneParents;

	// Token: 0x0400019F RID: 415
	private const float MaxDistance = 5000f;

	// Token: 0x040001A0 RID: 416
	private static int scratchA;

	// Token: 0x040001A1 RID: 417
	private static int scratchB;

	// Token: 0x040001A2 RID: 418
	private static Material chamsMaterial;

	// Token: 0x040001A3 RID: 419
	private static int scratchC;

	// Token: 0x040001A4 RID: 420
	private static GameObject hudObject0;

	// Token: 0x040001A5 RID: 421
	private static GameObject hudObject1;

	// Token: 0x040001A6 RID: 422
	private static GameObject hudObject2;

	// Token: 0x040001A7 RID: 423
	private static GameObject hudObject3;

	// Token: 0x040001A8 RID: 424
	private const float DefaultWidth = 0.04f;

	// Token: 0x040001A9 RID: 425
	private const int RingSegments = 40;

	// Token: 0x040001AA RID: 426
	private const float DefaultRingRadius = 30f;

	// Token: 0x040001AB RID: 427
	public static string lastDistanceText;

	// Token: 0x040001AC RID: 428
	public static bool lastDistanceSafe;

	// Token: 0x040001AD RID: 429
	private const int CircleSegments = 48;
}
