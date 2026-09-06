using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000029 RID: 41
public static class Predictions
{
	// Token: 0x06000130 RID: 304 RVA: 0x00016511 File Offset: 0x00014711
	public static void Update()
	{
		Predictions.ApplyPredictionFrame();
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0001651C File Offset: 0x0001471C
	public static void ApplyPredictions(float amount)
	{
		float deltaTime = Time.deltaTime;
		bool flag = deltaTime <= 0.0001f;
		if (!flag)
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag2 = instance == null;
			if (!flag2)
			{
				Transform val = ((instance.turnParent != null) ? instance.turnParent.transform : null);
				bool flag3 = val == null;
				if (flag3)
				{
					val = instance.bodyCollider.transform.parent;
				}
				bool flag4 = val == null;
				if (!flag4)
				{
					Transform controllerTransform = instance.LeftHand.controllerTransform;
					Transform controllerTransform2 = instance.RightHand.controllerTransform;
					bool flag5 = controllerTransform == null || controllerTransform2 == null;
					if (!flag5)
					{
						Vector3 val2 = val.InverseTransformPoint(controllerTransform.position);
						Vector3 val3 = val.InverseTransformPoint(controllerTransform2.position);
						bool flag6 = !Predictions.handsSaved;
						if (flag6)
						{
							Predictions.leftSaved = val2;
							Predictions.rightSaved = val3;
							Predictions.handsSaved = true;
						}
						else
						{
							Vector3 val4 = (val2 - Predictions.leftSaved) / deltaTime;
							Vector3 val5 = (val3 - Predictions.rightSaved) / deltaTime;
							Predictions.leftHistory[Predictions.historyIndex] = val4;
							Predictions.rightHistory[Predictions.historyIndex] = val5;
							int num = Predictions.historyIndex;
							int num2 = num + 1;
							Predictions.historyIndex = num2 % 6;
							bool flag7 = Predictions.historyIndex == 0;
							if (flag7)
							{
								Predictions.historyReady = true;
							}
							int num3 = (Predictions.historyReady ? 6 : Predictions.historyIndex);
							bool flag8 = num3 == 0;
							if (flag8)
							{
								Predictions.leftSaved = val2;
								Predictions.rightSaved = val3;
							}
							else
							{
								Vector3 val6 = Vector3.zero;
								Vector3 val7 = Vector3.zero;
								float num4 = 0f;
								for (int i = 0; i < num3; i++)
								{
									int num5 = Predictions.historyIndex;
									int num6 = num5 - 1 - i;
									int num7 = num6 + 6;
									int num8 = num7 % 6;
									float num9 = (float)(num3 - i);
									val6 += Predictions.leftHistory[num8] * num9;
									val7 += Predictions.rightHistory[num8] * num9;
									num4 += num9;
								}
								val6 /= num4;
								val7 /= num4;
								float num10 = Mathf.Clamp01(deltaTime * 12f);
								Predictions.leftServer = Vector3.Lerp(Predictions.leftServer, val6, num10);
								Predictions.rightServer = Vector3.Lerp(Predictions.rightServer, val7, num10);
								float num11 = amount / 1000f;
								Vector3 val8 = val.TransformVector(Predictions.leftServer * num11);
								Vector3 val9 = val.TransformVector(Predictions.rightServer * num11);
								bool predsLeftHandEnabled = Settings.PredsLeftHandEnabled;
								bool predsRightHandEnabled = Settings.PredsRightHandEnabled;
								bool flag9 = predsLeftHandEnabled;
								if (flag9)
								{
									controllerTransform.position += val8;
								}
								bool flag10 = predsRightHandEnabled;
								if (flag10)
								{
									controllerTransform2.position += val9;
								}
								Predictions.bodyServer = (predsLeftHandEnabled ? val8 : Vector3.zero);
								Predictions.headServer = (predsRightHandEnabled ? val9 : Vector3.zero);
								Predictions.leftSaved = val2;
								Predictions.rightSaved = val3;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00016860 File Offset: 0x00014A60
	private static void ApplyPredictionFrame()
	{
		try
		{
			bool predsEnabled = Settings.PredsEnabled;
			if (predsEnabled)
			{
				bool flag = !Application.isFocused;
				if (flag)
				{
					Predictions.Reset();
				}
				else
				{
					Predictions.ApplyPredictions(Settings.PredsAmount);
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x000168B8 File Offset: 0x00014AB8
	public static void LateUpdate()
	{
		bool flag = !Settings.PredsEnabled;
		if (flag)
		{
			Predictions.ClearClientMarkers();
			Predictions.ClearServerMarkers();
		}
		else
		{
			Predictions.VisualizeClient();
			bool flag2 = !Settings.AntiPredEnabled;
			if (flag2)
			{
				Predictions.ClearClientMarkers();
			}
			else
			{
				try
				{
					bool flag3 = GorillaTagger.Instance != null && GorillaTagger.Instance.offlineVRRig != null;
					if (flag3)
					{
						VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
						bool flag4 = offlineVRRig.leftHand != null && offlineVRRig.leftHand.rigTarget != null && Predictions.bodyServer.sqrMagnitude > 0f;
						if (flag4)
						{
							Transform rigTarget = offlineVRRig.leftHand.rigTarget;
							rigTarget.position -= Predictions.bodyServer;
						}
						bool flag5 = offlineVRRig.rightHand != null && offlineVRRig.rightHand.rigTarget != null && Predictions.headServer.sqrMagnitude > 0f;
						if (flag5)
						{
							Transform rigTarget2 = offlineVRRig.rightHand.rigTarget;
							rigTarget2.position -= Predictions.headServer;
						}
					}
					Predictions.VisualizeServer();
				}
				catch
				{
				}
			}
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00016A10 File Offset: 0x00014C10
	private static void VisualizeClient()
	{
		bool visualiseClientEnabled = Settings.VisualiseClientEnabled;
		if (visualiseClientEnabled)
		{
			bool flag = Predictions.bodyServer.sqrMagnitude >= 1E-05f;
			if (flag)
			{
				GTPlayer val3 = GTPlayer.Instance;
				bool flag2 = val3 == null;
				if (flag2)
				{
					Predictions.ClearServerMarkers();
				}
				else
				{
					Transform val4 = val3.LeftHand.controllerTransform;
					Transform val5 = val3.RightHand.controllerTransform;
					bool flag3 = !(val4 == null);
					if (flag3)
					{
						bool flag4 = val5 == null;
						if (flag4)
						{
							Predictions.ClearServerMarkers();
						}
						else
						{
							bool flag5 = Predictions.bodyServer.sqrMagnitude <= 0f;
							if (flag5)
							{
								bool flag6 = Predictions.serverLeftMarker != null;
								if (flag6)
								{
									Object.Destroy(Predictions.serverLeftMarker);
									Predictions.serverLeftMarker = null;
								}
							}
							else
							{
								bool flag7 = !(Predictions.serverLeftMarker == null);
								if (!flag7)
								{
									Predictions.serverLeftMarker = Predictions.CreateMarker("PredClientGhost");
									Predictions.serverLeftRenderer = Predictions.serverLeftMarker.GetComponent<Renderer>();
								}
								bool flag8 = Predictions.serverLeftRenderer != null;
								if (flag8)
								{
								}
							}
							bool flag9 = Predictions.headServer.sqrMagnitude > 0f;
							if (flag9)
							{
								bool flag10 = Predictions.serverRightMarker == null;
								if (flag10)
								{
									Predictions.serverRightMarker = Predictions.CreateMarker("PredClientGhost");
									Predictions.serverRightRenderer = Predictions.serverRightMarker.GetComponent<Renderer>();
								}
								bool flag11 = Predictions.serverRightRenderer != null;
								if (flag11)
								{
								}
							}
							else
							{
								bool flag12 = Predictions.serverRightMarker != null;
								if (flag12)
								{
									Object.Destroy(Predictions.serverRightMarker);
									Predictions.serverRightMarker = null;
								}
							}
						}
					}
					else
					{
						Predictions.ClearServerMarkers();
					}
				}
			}
			else
			{
				bool flag13 = Predictions.headServer.sqrMagnitude >= 1E-05f;
				if (flag13)
				{
					GTPlayer val3 = GTPlayer.Instance;
					bool flag14 = val3 == null;
					if (flag14)
					{
						Predictions.ClearServerMarkers();
					}
					else
					{
						Transform val4 = val3.LeftHand.controllerTransform;
						Transform val5 = val3.RightHand.controllerTransform;
						bool flag15 = !(val4 == null);
						if (flag15)
						{
							bool flag16 = val5 == null;
							if (flag16)
							{
								Predictions.ClearServerMarkers();
							}
							else
							{
								bool flag17 = Predictions.bodyServer.sqrMagnitude <= 0f;
								if (flag17)
								{
									bool flag18 = Predictions.serverLeftMarker != null;
									if (flag18)
									{
										Object.Destroy(Predictions.serverLeftMarker);
										Predictions.serverLeftMarker = null;
									}
								}
								else
								{
									bool flag19 = !(Predictions.serverLeftMarker == null);
									if (!flag19)
									{
										Predictions.serverLeftMarker = Predictions.CreateMarker("PredClientGhost");
										Predictions.serverLeftRenderer = Predictions.serverLeftMarker.GetComponent<Renderer>();
									}
									bool flag20 = Predictions.serverLeftRenderer != null;
									if (flag20)
									{
									}
								}
								bool flag21 = Predictions.headServer.sqrMagnitude > 0f;
								if (flag21)
								{
									bool flag22 = Predictions.serverRightMarker == null;
									if (flag22)
									{
										Predictions.serverRightMarker = Predictions.CreateMarker("PredClientGhost");
										Predictions.serverRightRenderer = Predictions.serverRightMarker.GetComponent<Renderer>();
									}
									bool flag23 = Predictions.serverRightRenderer != null;
									if (flag23)
									{
									}
								}
								else
								{
									bool flag24 = Predictions.serverRightMarker != null;
									if (flag24)
									{
										Object.Destroy(Predictions.serverRightMarker);
										Predictions.serverRightMarker = null;
									}
								}
							}
						}
						else
						{
							Predictions.ClearServerMarkers();
						}
					}
				}
				else
				{
					Predictions.ClearServerMarkers();
				}
			}
		}
		else
		{
			Predictions.ClearServerMarkers();
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00016D94 File Offset: 0x00014F94
	private static void VisualizeServer()
	{
		Vector3 position = default(Vector3);
		bool visualiseServerEnabled = Settings.VisualiseServerEnabled;
		if (visualiseServerEnabled)
		{
			bool flag = Predictions.bodyServer.sqrMagnitude >= 1E-05f;
			if (flag)
			{
				GTPlayer val3 = GTPlayer.Instance;
				bool flag2 = val3 == null;
				if (flag2)
				{
					Predictions.ClearClientMarkers();
				}
				else
				{
					Transform val4 = val3.LeftHand.controllerTransform;
					Transform val5 = val3.RightHand.controllerTransform;
					bool flag3 = !(val4 == null);
					if (flag3)
					{
						bool flag4 = val5 == null;
						if (flag4)
						{
							Predictions.ClearClientMarkers();
							return;
						}
						position = val5.position - Predictions.headServer;
						bool flag5 = Predictions.bodyServer.sqrMagnitude <= 0f;
						if (flag5)
						{
							bool flag6 = Predictions.clientLeftMarker != null;
							if (flag6)
							{
								Object.Destroy(Predictions.clientLeftMarker);
								Predictions.clientLeftMarker = null;
							}
						}
						else
						{
							bool flag7 = Predictions.clientLeftMarker == null;
							if (flag7)
							{
								Predictions.clientLeftMarker = Predictions.CreateMarker("PredServerGhost");
								Predictions.clientLeftRenderer = Predictions.clientLeftMarker.GetComponent<Renderer>();
								bool flag8 = Predictions.clientLeftRenderer != null;
								if (flag8)
								{
								}
							}
							bool flag9 = Predictions.clientLeftRenderer != null;
							if (flag9)
							{
							}
						}
					}
					Predictions.ClearClientMarkers();
				}
			}
			else
			{
				bool flag10 = Predictions.headServer.sqrMagnitude < 1E-05f;
				if (flag10)
				{
					Predictions.ClearClientMarkers();
				}
				else
				{
					GTPlayer val3 = GTPlayer.Instance;
					bool flag11 = val3 == null;
					if (flag11)
					{
						Predictions.ClearClientMarkers();
					}
					else
					{
						Transform val4 = val3.LeftHand.controllerTransform;
						Transform val5 = val3.RightHand.controllerTransform;
						bool flag12 = !(val4 == null);
						if (flag12)
						{
							bool flag13 = val5 == null;
							if (flag13)
							{
								Predictions.ClearClientMarkers();
								return;
							}
							position = val5.position - Predictions.headServer;
							bool flag14 = Predictions.bodyServer.sqrMagnitude <= 0f;
							if (flag14)
							{
								bool flag15 = Predictions.clientLeftMarker != null;
								if (flag15)
								{
									Object.Destroy(Predictions.clientLeftMarker);
									Predictions.clientLeftMarker = null;
								}
							}
							else
							{
								bool flag16 = Predictions.clientLeftMarker == null;
								if (flag16)
								{
									Predictions.clientLeftMarker = Predictions.CreateMarker("PredServerGhost");
									Predictions.clientLeftRenderer = Predictions.clientLeftMarker.GetComponent<Renderer>();
									bool flag17 = Predictions.clientLeftRenderer != null;
									if (flag17)
									{
									}
								}
								bool flag18 = Predictions.clientLeftRenderer != null;
								if (flag18)
								{
								}
							}
						}
						Predictions.ClearClientMarkers();
					}
				}
			}
		}
		else
		{
			Predictions.ClearClientMarkers();
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00017048 File Offset: 0x00015248
	private static GameObject CreateMarker(string markerName)
	{
		GameObject val = GameObject.CreatePrimitive(0);
		val.name = markerName;
		SphereCollider component = val.GetComponent<SphereCollider>();
		bool flag = component != null;
		if (flag)
		{
			Object.DestroyImmediate(component);
		}
		Renderer component2 = val.GetComponent<Renderer>();
		component2.material = new Material(Shader.Find("GUI/Text Shader"))
		{
			hideFlags = 61
		};
		return val;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x000170B4 File Offset: 0x000152B4
	private static void ClearClientMarkers()
	{
		bool flag = !(Predictions.clientLeftMarker != null);
		if (!flag)
		{
			Object.Destroy(Predictions.clientLeftMarker);
			Predictions.clientLeftMarker = null;
		}
		bool flag2 = !(Predictions.clientRightMarker != null);
		if (!flag2)
		{
			Object.Destroy(Predictions.clientRightMarker);
			Predictions.clientRightMarker = null;
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00017118 File Offset: 0x00015318
	private static void ClearServerMarkers()
	{
		bool flag = !(Predictions.serverLeftMarker != null);
		if (!flag)
		{
			Object.Destroy(Predictions.serverLeftMarker);
			Predictions.serverLeftMarker = null;
		}
		bool flag2 = !(Predictions.serverRightMarker != null);
		if (!flag2)
		{
			Object.Destroy(Predictions.serverRightMarker);
			Predictions.serverRightMarker = null;
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0001717C File Offset: 0x0001537C
	public static void Reset()
	{
		Predictions.ClearClientMarkers();
		Predictions.ClearServerMarkers();
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 >= 6;
			if (flag)
			{
				break;
			}
			Predictions.leftHistory[num2] = Vector3.zero;
			Predictions.rightHistory[num2] = Vector3.zero;
			num2++;
		}
	}

	// Token: 0x04000250 RID: 592
	private const int predPoints = 6;

	// Token: 0x04000251 RID: 593
	private static Vector3[] leftHistory = (Vector3[])new Vector3[6];

	// Token: 0x04000252 RID: 594
	private static Vector3[] rightHistory = (Vector3[])new Vector3[6];

	// Token: 0x04000253 RID: 595
	private static int historyIndex = 0;

	// Token: 0x04000254 RID: 596
	private static bool historyReady = false;

	// Token: 0x04000255 RID: 597
	private static Vector3 leftSaved;

	// Token: 0x04000256 RID: 598
	private static Vector3 rightSaved;

	// Token: 0x04000257 RID: 599
	private static bool handsSaved = false;

	// Token: 0x04000258 RID: 600
	private static Vector3 leftServer;

	// Token: 0x04000259 RID: 601
	private static Vector3 rightServer;

	// Token: 0x0400025A RID: 602
	private static Vector3 bodyServer;

	// Token: 0x0400025B RID: 603
	private static Vector3 headServer;

	// Token: 0x0400025C RID: 604
	private static GameObject clientLeftMarker;

	// Token: 0x0400025D RID: 605
	private static GameObject clientRightMarker;

	// Token: 0x0400025E RID: 606
	private static Renderer clientLeftRenderer;

	// Token: 0x0400025F RID: 607
	private static Renderer clientRightRenderer;

	// Token: 0x04000260 RID: 608
	private static GameObject serverLeftMarker;

	// Token: 0x04000261 RID: 609
	private static GameObject serverRightMarker;

	// Token: 0x04000262 RID: 610
	private static Renderer serverLeftRenderer;

	// Token: 0x04000263 RID: 611
	private static Renderer serverRightRenderer;
}
