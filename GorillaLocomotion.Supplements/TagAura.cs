using System;
using GorillaGameModes;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000038 RID: 56
internal class TagAura
{
	// Token: 0x060001A2 RID: 418 RVA: 0x0001C770 File Offset: 0x0001A970
	private static void TagRig(VRRig rig)
	{
		try
		{
			bool flag = rig != null && rig.Creator != null;
			if (flag)
			{
				bool desyncEnabled = Settings.DesyncEnabled;
				if (desyncEnabled)
				{
					Desync.TriggerLagSpike(0.3f);
				}
				GameMode.ReportTag(rig.Creator);
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0001C7D4 File Offset: 0x0001A9D4
	public static void Update()
	{
		bool flag = !Settings.DCFlickEnabled;
		if (flag)
		{
			bool flag2 = !TagAura.flicking;
			if (!flag2)
			{
				bool flag3 = GorillaTagger.Instance != null;
				if (flag3)
				{
					TagAura.flicking = false;
				}
				TagAura.flicking = false;
			}
		}
		else
		{
			bool flag4 = GorillaTagger.Instance != null;
			if (flag4)
			{
				TagAura.flicking = true;
			}
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0001C83C File Offset: 0x0001AA3C
	private static bool IsValidTarget(VRRig rig)
	{
		string text = rig.mainSkin.material.name.ToLower();
		return text.Contains("fected");
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0001C870 File Offset: 0x0001AA70
	public static void ApplyTagAura()
	{
		bool flag = !Settings.TagAuraEnabled || !Settings.IsPressed(Settings.TagAuraKeybind);
		if (!flag)
		{
			try
			{
				bool flag2 = GorillaTagger.Instance == null || GorillaTagger.Instance.offlineVRRig == null || GTPlayer.Instance == null || GTPlayer.Instance.disableMovement || GorillaParent.instance == null || VRRigCache.ActiveRigs == null;
				if (!flag2)
				{
					Vector3 position = GorillaTagger.Instance.bodyCollider.transform.position;
					Vector3 position2 = GorillaTagger.Instance.offlineVRRig.headMesh.transform.position;
					Vector3 forward = GorillaTagger.Instance.offlineVRRig.headMesh.transform.forward;
					float num = (Settings.TagAuraFovEnabled ? Mathf.Cos(Settings.TagAuraFov * 0.5f * 0.0174532924f) : (-1f));
					foreach (VRRig activeRig in VRRigCache.ActiveRigs)
					{
						bool flag3 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig || !TagAura.IsValidTarget(GorillaTagger.Instance.offlineVRRig) || TagAura.IsValidTarget(activeRig) || activeRig.headMesh == null;
						if (!flag3)
						{
							Vector3 position3 = activeRig.headMesh.transform.position;
							bool flag4 = Vector3.Distance(position3, position) >= Settings.TagAuraDistance;
							if (!flag4)
							{
								bool tagAuraFovEnabled = Settings.TagAuraFovEnabled;
								if (tagAuraFovEnabled)
								{
									Vector3 normalized = (position3 - position2).normalized;
									bool flag5 = Vector3.Dot(forward, normalized) < num;
									if (flag5)
									{
										continue;
									}
								}
								TagAura.TagRig(activeRig);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
	public static void AutoDcFlick()
	{
		bool flag = !Settings.DcFlickAutoEnabled;
		if (flag)
		{
			TagAura.handSaved = false;
		}
		else
		{
			bool flag2 = TagAura.handSaved;
			if (flag2)
			{
				TagAura.autoHoldTimer -= Time.deltaTime;
				bool flag3 = TagAura.autoHoldTimer > 0f;
				if (flag3)
				{
					try
					{
						GTPlayer instance = GTPlayer.Instance;
						bool flag4 = instance != null && GorillaTagger.Instance != null;
						if (flag4)
						{
							Transform transform = GorillaTagger.Instance.bodyCollider.transform;
							Vector3 val = transform.rotation * Quaternion.Inverse(TagAura.savedHandRot) * TagAura.savedHandPos;
							Vector3 position = transform.position + val * TagAura.flickTimer;
							instance.RightHand.controllerTransform.position = position;
						}
						return;
					}
					catch
					{
						return;
					}
				}
				TagAura.handSaved = false;
			}
			else
			{
				bool flag5 = !Settings.IsPressed(Settings.DcFlickAutoKeybind);
				if (!flag5)
				{
					try
					{
						bool flag6 = GorillaTagger.Instance == null || GorillaTagger.Instance.offlineVRRig == null || GTPlayer.Instance == null || GTPlayer.Instance.disableMovement || GorillaParent.instance == null || VRRigCache.ActiveRigs == null || !TagAura.IsValidTarget(GorillaTagger.Instance.offlineVRRig);
						if (!flag6)
						{
							VRRig val2 = null;
							float num = float.MaxValue;
							Vector3 position2 = GorillaTagger.Instance.bodyCollider.transform.position;
							foreach (VRRig activeRig in VRRigCache.ActiveRigs)
							{
								bool flag7 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig || TagAura.IsValidTarget(activeRig) || activeRig.headMesh == null;
								if (!flag7)
								{
									Vector3 position3 = activeRig.headMesh.transform.position;
									float num2 = Vector3.Distance(position3, position2);
									bool flag8 = num2 > Settings.DcFlickAutoMaxDistance;
									if (!flag8)
									{
										bool dcFlickAutoLOS = Settings.DcFlickAutoLOS;
										if (dcFlickAutoLOS)
										{
											Vector3 position4 = GorillaTagger.Instance.offlineVRRig.headMesh.transform.position;
											Vector3 normalized = (position3 - position4).normalized;
											Vector3 forward = GorillaTagger.Instance.offlineVRRig.headMesh.transform.forward;
											float num3 = Mathf.Cos(Settings.DcFlickAutoFov * 0.5f * 0.0174532924f);
											bool flag9 = Vector3.Dot(forward, normalized) < num3;
											if (flag9)
											{
												continue;
											}
										}
										bool flag10 = num2 < num;
										if (flag10)
										{
											num = num2;
											val2 = activeRig;
										}
									}
								}
							}
							bool flag11 = val2 != null;
							if (flag11)
							{
								Vector3 position5 = val2.headMesh.transform.position;
								Transform transform2 = GorillaTagger.Instance.bodyCollider.transform;
								Vector3 val3 = position5 - transform2.position;
								TagAura.flickTimer = val3.magnitude;
								TagAura.savedHandPos = val3.normalized;
								TagAura.savedHandRot = transform2.rotation;
								TagAura.TagRig(val2);
								TagAura.handSaved = true;
								TagAura.autoHoldTimer = Settings.DcFlickAutoHoldTime;
								GTPlayer instance2 = GTPlayer.Instance;
								bool flag12 = instance2 != null;
								if (flag12)
								{
									instance2.RightHand.controllerTransform.position = position5;
								}
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0001CE8C File Offset: 0x0001B08C
	private static GameObject GetClosestTarget()
	{
		GameObject val = GameObject.CreatePrimitive(0);
		val.name = "HitboxExpander";
		SphereCollider component = val.GetComponent<SphereCollider>();
		bool flag = component != null;
		if (flag)
		{
			Object.DestroyImmediate(component);
		}
		Renderer component2 = val.GetComponent<Renderer>();
		Material val2 = new Material(Shader.Find("Hidden/Internal-Colored"));
		bool flag2 = val2.shader == null || val2.shader.name == "Hidden/InternalErrorShader";
		if (flag2)
		{
			val2 = new Material(Shader.Find("Sprites/Default"));
		}
		Material val3 = val2;
		val3.hideFlags = 61;
		Material val4 = val2;
		val4.SetInt("_SrcBlend", 5);
		Material val5 = val2;
		val5.SetInt("_DstBlend", 10);
		Material val6 = val2;
		val6.SetInt("_Cull", 0);
		Material val7 = val2;
		val7.SetInt("_ZWrite", 0);
		Material val8 = val2;
		val8.SetInt("_ZTest", 8);
		val2.color = new Color(1f, 0f, 0f, 0.3f);
		component2.material = val2;
		component2.enabled = true;
		return val;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0001CFBC File Offset: 0x0001B1BC
	public static void PerformDcFlick()
	{
		bool flag = !(TagAura.hitboxExpanderR != null);
		if (!flag)
		{
			Object.DestroyImmediate(TagAura.hitboxExpanderR);
			TagAura.hitboxExpanderR = null;
		}
		bool flag2 = !(TagAura.hitboxExpanderL != null);
		if (!flag2)
		{
			Object.DestroyImmediate(TagAura.hitboxExpanderL);
			TagAura.hitboxExpanderL = null;
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0001D020 File Offset: 0x0001B220
	public static void ApplyHitboxExpander()
	{
		bool flag2 = !Settings.HitboxExpanderEnabled;
		if (flag2)
		{
			TagAura.PerformDcFlick();
		}
		else
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag3 = instance == null;
			if (flag3)
			{
				TagAura.PerformDcFlick();
			}
			else
			{
				bool flag4 = !(GorillaTagger.Instance == null) && !(GorillaTagger.Instance.offlineVRRig == null);
				if (flag4)
				{
					bool flag5 = !(GorillaParent.instance == null) && VRRigCache.ActiveRigs != null;
					if (flag5)
					{
						float hitboxExpanderSize = Settings.HitboxExpanderSize;
						Color color = default(Color);
						color..ctor(1f, 0f, 0f, Settings.HitboxExpanderOpacity);
						Vector3 position = instance.RightHand.controllerTransform.position;
						Vector3 position2 = instance.LeftHand.controllerTransform.position;
						bool flag = TagAura.IsValidTarget(GorillaTagger.Instance.offlineVRRig);
						bool flag6 = TagAura.hitboxExpanderR == null;
						if (flag6)
						{
							TagAura.hitboxExpanderR = TagAura.GetClosestTarget();
							TagAura.leftHitbox = TagAura.hitboxExpanderR.GetComponent<Renderer>();
						}
						TagAura.hitboxExpanderR.transform.position = position;
						TagAura.hitboxExpanderR.transform.localScale = Vector3.one * (hitboxExpanderSize * 2f);
						bool flag7 = TagAura.leftHitbox != null;
						if (flag7)
						{
							TagAura.leftHitbox.material.color = color;
						}
						Renderer val = TagAura.leftHitbox;
						val.enabled = true;
						bool flag8 = TagAura.hitboxExpanderL == null;
						if (flag8)
						{
							TagAura.hitboxExpanderL = TagAura.GetClosestTarget();
							TagAura.rightHitbox = TagAura.hitboxExpanderL.GetComponent<Renderer>();
						}
						TagAura.hitboxExpanderL.transform.position = position2;
						TagAura.hitboxExpanderL.transform.localScale = Vector3.one * (hitboxExpanderSize * 2f);
						bool flag9 = TagAura.rightHitbox != null;
						if (flag9)
						{
							TagAura.rightHitbox.material.color = color;
						}
						Renderer val2 = TagAura.rightHitbox;
						val2.enabled = true;
						bool flag10 = !flag;
						if (flag10)
						{
							return;
						}
						try
						{
							bool disableMovement = GTPlayer.Instance.disableMovement;
							if (disableMovement)
							{
								return;
							}
							foreach (VRRig activeRig in VRRigCache.ActiveRigs)
							{
								bool flag11 = activeRig == null || activeRig == GorillaTagger.Instance.offlineVRRig || TagAura.IsValidTarget(activeRig);
								if (!flag11)
								{
									float num = float.MaxValue;
									bool flag12 = activeRig.headMesh != null;
									if (flag12)
									{
										Vector3 position3 = activeRig.headMesh.transform.position;
										float num2 = Vector3.Distance(position3, position);
										float num3 = Vector3.Distance(position3, position2);
										bool flag13 = num2 < num;
										if (flag13)
										{
											num = num2;
										}
										bool flag14 = num3 < num;
										if (flag14)
										{
											num = num3;
										}
									}
									Vector3 val3 = activeRig.transform.position + Vector3.up;
									float num4 = Vector3.Distance(val3, position);
									float num5 = Vector3.Distance(val3, position2);
									bool flag15 = num4 < num;
									if (flag15)
									{
										num = num4;
									}
									bool flag16 = num5 < num;
									if (flag16)
									{
										num = num5;
									}
									bool flag17 = activeRig.rightHand != null && activeRig.rightHand.rigTarget != null;
									if (flag17)
									{
										Vector3 position4 = activeRig.rightHand.rigTarget.position;
										float num6 = Vector3.Distance(position4, position);
										float num7 = Vector3.Distance(position4, position2);
										bool flag18 = num6 < num;
										if (flag18)
										{
											num = num6;
										}
										bool flag19 = num7 < num;
										if (flag19)
										{
											num = num7;
										}
									}
									bool flag20 = activeRig.leftHand != null && activeRig.leftHand.rigTarget != null;
									if (flag20)
									{
										Vector3 position5 = activeRig.leftHand.rigTarget.position;
										float num8 = Vector3.Distance(position5, position);
										float num9 = Vector3.Distance(position5, position2);
										bool flag21 = num8 < num;
										if (flag21)
										{
											num = num8;
										}
										bool flag22 = num9 < num;
										if (flag22)
										{
											num = num9;
										}
									}
									bool flag23 = num < hitboxExpanderSize;
									if (flag23)
									{
										TagAura.TagRig(activeRig);
									}
								}
							}
							return;
						}
						catch
						{
							return;
						}
					}
					TagAura.PerformDcFlick();
				}
				else
				{
					TagAura.PerformDcFlick();
				}
			}
		}
	}

	// Token: 0x04000445 RID: 1093
	private static bool flicking;

	// Token: 0x04000446 RID: 1094
	private static bool handSaved;

	// Token: 0x04000447 RID: 1095
	private static float autoHoldTimer;

	// Token: 0x04000448 RID: 1096
	private static float flickTimer;

	// Token: 0x04000449 RID: 1097
	private static Vector3 savedHandPos;

	// Token: 0x0400044A RID: 1098
	private static Quaternion savedHandRot;

	// Token: 0x0400044B RID: 1099
	public static GameObject hitboxExpanderR;

	// Token: 0x0400044C RID: 1100
	public static GameObject hitboxExpanderL;

	// Token: 0x0400044D RID: 1101
	private static Renderer leftHitbox;

	// Token: 0x0400044E RID: 1102
	private static Renderer rightHitbox;
}
