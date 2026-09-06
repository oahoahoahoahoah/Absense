using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003D RID: 61
public static class Trails
{
	// Token: 0x060001C7 RID: 455 RVA: 0x0001F768 File Offset: 0x0001D968
	public static void Update()
	{
		bool flag = !Settings.TrailsEnabled;
		if (flag)
		{
			Trails.Clear();
		}
		else
		{
			bool flag2 = GorillaParent.instance == null || VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null;
			if (!flag2)
			{
				float deltaTime = Time.deltaTime;
				bool flag3 = deltaTime <= 0f;
				if (!flag3)
				{
					HashSet<int> hashSet = new HashSet<int>();
					foreach (VRRig activeRig in VRRigCache.ActiveRigs)
					{
						bool flag4 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
						if (flag4)
						{
							int instanceID = activeRig.GetInstanceID();
							hashSet.Add(instanceID);
							Vector3 position = activeRig.transform.position;
							TrailRenderer value;
							bool flag5 = Trails.trails.TryGetValue(instanceID, out value) && !(value == null);
							if (flag5)
							{
								Vector3 val = (Trails.lastPositions.ContainsKey(instanceID) ? Trails.lastPositions[instanceID] : position);
								float num = (position - val).magnitude / deltaTime;
								Trails.lastPositions[instanceID] = position;
								bool emitting = num > Settings.TrailMinSpeed;
								value.emitting = emitting;
								float num2 = Mathf.Clamp01((num - Settings.TrailMinSpeed) / Mathf.Max(0.01f, Settings.TrailSpeedScale - Settings.TrailMinSpeed));
								value.time = Mathf.Lerp(Settings.TrailMinTime, Settings.TrailMaxTime, num2);
								value.startWidth = Settings.TrailWidth;
								value.endWidth = 0f;
								Color val2 = (Settings.TrailUsePlayerColor ? activeRig.playerColor : Color.HSVToRGB(Settings.TrailColorHue, Settings.TrailColorSat, Settings.TrailColorVal));
								Gradient val3 = new Gradient();
								val3.colorKeys = (GradientColorKey[])new GradientColorKey[]
								{
									new GradientColorKey(val2, 0f),
									new GradientColorKey(val2, 1f)
								};
								val3.alphaKeys = (GradientAlphaKey[])new GradientAlphaKey[]
								{
									new GradientAlphaKey(1f, 0f),
									new GradientAlphaKey(0f, 1f)
								};
								value.colorGradient = val3;
							}
							else
							{
								value = Trails.GetOrCreateTrail(activeRig);
								Trails.trails[instanceID] = value;
								Trails.lastPositions[instanceID] = position;
							}
						}
					}
					Trails.activeIds.Clear();
					foreach (KeyValuePair<int, TrailRenderer> item in Trails.trails)
					{
						bool flag6 = !hashSet.Contains(item.Key);
						if (flag6)
						{
							bool flag7 = item.Value != null;
							if (flag7)
							{
								Object.Destroy(item.Value.gameObject);
							}
							Trails.activeIds.Add(item.Key);
						}
					}
					foreach (int item2 in Trails.activeIds)
					{
						Trails.trails.Remove(item2);
						Trails.lastPositions.Remove(item2);
					}
				}
			}
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0001FB48 File Offset: 0x0001DD48
	private static TrailRenderer GetOrCreateTrail(VRRig rig)
	{
		GameObject val = new GameObject("Absense_Trail_" + rig.GetInstanceID().ToString());
		val.hideFlags = 61;
		Transform transform = val.transform;
		Transform transform2 = rig.transform;
		transform.SetParent(transform2, false);
		val.transform.localPosition = Vector3.zero;
		TrailRenderer val2 = val.AddComponent<TrailRenderer>();
		val2.material = Trails.CreateMaterial();
		val2.alignment = 0;
		val2.numCapVertices = 4;
		val2.numCornerVertices = 4;
		val2.minVertexDistance = 0.05f;
		val2.shadowCastingMode = 0;
		val2.receiveShadows = false;
		val2.time = Settings.TrailMinTime;
		val2.startWidth = Settings.TrailWidth;
		val2.endWidth = 0f;
		val2.emitting = false;
		val2.Clear();
		return val2;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0001FC28 File Offset: 0x0001DE28
	private static Material CreateMaterial()
	{
		bool flag = !(Trails.trailMaterial == null);
		if (!flag)
		{
			Shader val = Shader.Find("Sprites/Default");
			bool flag2 = !(val == null);
			if (!flag2)
			{
				val = Shader.Find("Hidden/Internal-Colored");
			}
			Trails.trailMaterial = new Material(val);
		}
		return Trails.trailMaterial;
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0001FC90 File Offset: 0x0001DE90
	public static void Clear()
	{
		foreach (KeyValuePair<int, TrailRenderer> item in Trails.trails)
		{
			bool flag = item.Value != null;
			if (flag)
			{
				Object.Destroy(item.Value.gameObject);
			}
		}
		Trails.trails.Clear();
		Trails.lastPositions.Clear();
	}

	// Token: 0x0400048D RID: 1165
	private static Dictionary<int, TrailRenderer> trails = new Dictionary<int, TrailRenderer>();

	// Token: 0x0400048E RID: 1166
	private static Dictionary<int, Vector3> lastPositions = new Dictionary<int, Vector3>();

	// Token: 0x0400048F RID: 1167
	private static List<int> activeIds = new List<int>();

	// Token: 0x04000490 RID: 1168
	private static Material trailMaterial;
}
