using System;
using Additions;
using Agent;
using UnityEditor;
using UnityEngine;
using Utilities;

public class Main : MonoBehaviour
{

    public ComputeShader moveAgentsCompute, fadeTrailsCompute;

    public SimulationSettings settings;

    private RenderTexture m_renderTexture;
    private ComputeBuffer m_agentsBuffer, m_speciesSettingsBuffer;

    private MouseManager m_mouseManager;

    private Agent.Agent[] m_agents;

    // Start is called before the first frame update
    void Start()
    {
        m_renderTexture = new RenderTexture(settings.width, settings.height, 4) { enableRandomWrite = true };
        m_renderTexture.Create();

        m_mouseManager = new MouseManager(settings.width, settings.height);

        m_agents = settings.Initialisation();
        m_agentsBuffer = AgentsComputeBuffer();
        m_speciesSettingsBuffer = SpeciesSettingsComputeBuffer();
        
        ComputeShaderConstSettings();
    }
    
    private ComputeBuffer AgentsComputeBuffer()
    {
        ComputeBuffer computeBuffer = new ComputeBuffer(settings.numAgents, AgentInitialisation.c_AgentByteSize);
        return computeBuffer;
    }

    private ComputeBuffer SpeciesSettingsComputeBuffer()
    {
        ComputeBuffer computeBuffer =
            new ComputeBuffer(settings.specieSettings.Length, SimulationSettings.c_SpecieSettingByteSize);
        return computeBuffer;
    }

    private void ComputeShaderConstSettings()
    {
        fadeTrailsCompute.SetTexture(0, "Result", m_renderTexture);
        fadeTrailsCompute.SetInt("width", settings.width);
        fadeTrailsCompute.SetInt("height", settings.height);

        moveAgentsCompute.SetTexture(0, "Result", m_renderTexture);
        moveAgentsCompute.SetBuffer(0, "agents", m_agentsBuffer);
        moveAgentsCompute.SetBuffer(0, "species_settings", m_speciesSettingsBuffer);
        moveAgentsCompute.SetInt("width", settings.width);
        moveAgentsCompute.SetInt("height", settings.height);
        moveAgentsCompute.SetInt("num_agent", settings.numAgents);
    }
    
    private void ComputeShaderFrameSettings()
    {
        m_mouseManager.Update(moveAgentsCompute);
        
        fadeTrailsCompute.SetFloat("delta_time", Time.deltaTime);
        fadeTrailsCompute.SetFloat("decay_rate", settings.decayRate);
        fadeTrailsCompute.SetFloat("blur_rate", settings.blurRate);

        moveAgentsCompute.SetFloat("time", Time.time);
        moveAgentsCompute.SetFloat("delta_time", Time.deltaTime);
        moveAgentsCompute.SetInt("mouse_effect_radius", settings.mouseEffectRadius);
        m_speciesSettingsBuffer.SetData(settings.specieSettings);
    }

    void FixedUpdate()
    {
        ComputeShaderFrameSettings();
        m_agentsBuffer.SetData(m_agents);
        ComputeHelper.Dispatch(moveAgentsCompute, settings.numAgents);
        m_agentsBuffer.GetData(m_agents);
        ComputeHelper.Dispatch(fadeTrailsCompute, settings.width, settings.height);
    }

    private void OnRenderImage(RenderTexture _src, RenderTexture _dest)
    {
        Graphics.Blit(m_renderTexture, _dest);
    }

    private void OnDestroy()
    {
        m_agentsBuffer.Release();
        m_speciesSettingsBuffer.Release();
    }
    
    
}
