using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Agent
{
	public static class AgentInitialisation
	{
		public const int c_AgentByteSize = 3 * sizeof(float) + sizeof(int);

		public delegate Agent[] AgentInit(int _width, int _height, int _numAgents, int _numSpecies);
		
		private static readonly Dictionary<AgentInitialisationEnum, AgentInit> EnumToDelegate =
			new Dictionary<AgentInitialisationEnum, AgentInit>()
			{
				{ AgentInitialisationEnum.InwardCircle, InwardCircleInitialisation },
				{ AgentInitialisationEnum.OutwardCircle, OutwardCircleInitialisation },
				{ AgentInitialisationEnum.FullRandom, RandomInitialisation }
			};

		private static Agent[] InwardCircleInitialisation(int _width, int _height, int _numAgents, int _numSpecies) =>
			CircleInitialisation(_width, _height, _numAgents, _numSpecies, true);

		private static Agent[] OutwardCircleInitialisation(int _width, int _height, int _numAgents, int _numSpecies) =>
			CircleInitialisation(_width, _height, _numAgents, _numSpecies, false);

		private static Agent[] CircleInitialisation(int _width, int _height, int _numAgents, int _numSpecies,
			bool _inward)
		{
			Agent[] agents = new Agent[_numAgents];
			Vector2 circleCenter = new Vector2((float)_width / 2, (float)_height / 2);
			for (int i = 0; i < _numAgents; i++)
			{
				Vector2 relative = Random.insideUnitCircle * _height / 2;
				int dirFact = _inward ? -1 : 1;
				float angle = Mathf.Atan2(dirFact * relative.y, dirFact * relative.x);
				int specieIndex = Random.Range(0, _numSpecies);
				agents[i] = new Agent { Position = circleCenter + relative, Angle = angle, SpecieIndex = specieIndex };
			}

			return agents;
		}

		private static Agent[] RandomInitialisation(int _width, int _height, int _numAgents, int _numSpecies)
		{
			Agent[] agents = new Agent[_numAgents];
			for (int i = 0; i < _numAgents; i++)
			{
				Vector2 position = new Vector2(Random.Range(0, _width), Random.Range(0, _height));
				float angle = Random.Range(0, (float)Math.PI * 2);
				int specieIndex = Random.Range(0, _numSpecies);
				agents[i] = new Agent { Position = position, Angle = angle, SpecieIndex = specieIndex };
			}

			return agents;
		}

		public static AgentInit EnumToInit(AgentInitialisationEnum _agentInitialisation)
		{
			return EnumToDelegate[_agentInitialisation];
		}
	}

	public enum AgentInitialisationEnum {
		InwardCircle, OutwardCircle, FullRandom
	}

	public struct Agent
	{
		public Vector2 Position;
		public float Angle;
		public int SpecieIndex;
	}
}