using System;
using Agent;
using UnityEngine;

[CreateAssetMenu()]
public class SimulationSettings : ScriptableObject
{
	[Header("Simulation Settings")] public int width = 1280;
	public int height = 720;

	public int numAgents = 10;

	[Range(.1f, 1f)] public float decayRate, blurRate;
	[Range(10, 100)] public int mouseEffectRadius;

	public AgentInitialisationEnum agentInitialisation;

	public SpecieSetting[] specieSettings;

	public const int c_SpecieSettingByteSize = 3 * sizeof(int) + 5 * sizeof(float);

	[Serializable]
	public struct SpecieSetting
	{
		[Range(1, 100)] public int speed;
		[Range(5, 90)] public int senseSpread;
		[Range(10, 100)] public int senseLength;
		[Range(10f, 100f)] public float rotationSpeed;
		public Color color;
	}

	public Agent.Agent[] Initialisation()
	{
		return AgentInitialisation.EnumToInit(agentInitialisation)(width, height, numAgents, specieSettings.Length);
	}
}