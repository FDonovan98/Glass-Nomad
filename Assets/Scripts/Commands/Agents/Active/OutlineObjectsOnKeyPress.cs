using UnityEngine;

using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Threading.Tasks;
using System;
using System.Threading;

/// <summary>
/// Code modified from: https://github.com/michaelcurtiss/UnityOutlineFX.
/// </summary>

[CreateAssetMenu(fileName = "DefaultOutlineObjectsOnKeyPress", menuName = "Commands/Active/OutlineObjectsOnKeyPress", order = 0)]
public class OutlineObjectsOnKeyPress : ActiveCommandObject
{
    #region public vars
    [Header("General Settings")]
    public KeyCode activateOutlining = KeyCode.F;
    public float duration = 3.0f;
    public Color color = Color.red;
    public float moveSpeedDivider = 1.0f;

    [Header("Outline Settings")]
    [SerializeField]
    public CameraEvent BufferDrawEvent = CameraEvent.BeforeImageEffects;
    public float flashSpeed = 4f;

    // [Header("Blur Settings")]
    // [Range(0, 1)]
    // [Tooltip("Downsampling will make things more efficient, as well as make the outline a bit thicker")]
    // public int Downsample = 1;
    [Range(0.0f, 3.0f)]
    public float BlurSize = 1.0f;


    #endregion

    #region private field

    private Color OutlineColor;
    private CommandBuffer _commandBuffer;

    private int _outlineRTID, _blurredRTID, _temporaryRTID, _depthRTID, _idRTID;

    private List<List<Renderer>> _objectRenderers;

    private Material _outlineMaterial;
    private Camera _camera;

    private int _RTWidth = 512;
    private int _RTHeight = 512;
    
    // Harry's totally effective(TM) way of making it work onEnable.
    private List<CancellationTokenSource> cts;
    private int Downsample = 0;
    
    #endregion

    protected override void OnEnable()
    {
        keyTable.Add("Tracker Vision", activateOutlining);
    }
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.isLocalAgent)
        {
            agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        }
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (Input.GetKeyDown(activateOutlining))
        {
            AgentController agentController = (AgentController)agentInputHandler;

            if (!agentController.alienVisionIsActive)
            {
                agentController.ChangeStat(ResourceType.AlienVision, true);
                agentInputHandler.ChangeMovementSpeedModifier(moveSpeedDivider, false);

                Renderer[] objectsToOutline = FetchOtherAgentsToOutline(agentInputHandler);

                _objectRenderers = new List<List<Renderer>>();
                _commandBuffer = new CommandBuffer();
                _commandBuffer.name = "OutlineFX Command Buffer";

                _depthRTID = Shader.PropertyToID("_DepthRT");
                _outlineRTID = Shader.PropertyToID("_OutlineRT");
                _blurredRTID = Shader.PropertyToID("_BlurredRT");
                _temporaryRTID = Shader.PropertyToID("_TemporaryRT");
                _idRTID = Shader.PropertyToID("_idRT");

                _RTWidth = Screen.width;
                _RTHeight = Screen.height;

                _outlineMaterial = new Material(Shader.Find("Hidden/OutlineHighlight"));

                _camera = agentInputHandler.agentCamera;
                _camera.depthTextureMode = DepthTextureMode.Depth;
                _camera.AddCommandBuffer(BufferDrawEvent, _commandBuffer);

                int i = 0;
                cts = new List<CancellationTokenSource>();

                foreach (Renderer element in objectsToOutline)
                {
                    if (element != null)
                    {
                        cts.Add(new CancellationTokenSource());
                        try
                        {
                            ComputeForTime(element, duration, color, cts[i].Token);
                        }
                        catch (OperationCanceledException)
                        {
                        }
                        finally
                        {
                        }
                    }
                    i++;
                }
            }
            else
            {
                agentController.ChangeStat(ResourceType.AlienVision, false);
                agentInputHandler.ChangeMovementSpeedModifier(moveSpeedDivider, true);

                for (int i = 0; i < cts.Count; i++)
                {
                    cts[i].Cancel();
                }

                cts = new List<CancellationTokenSource>();
            }
        }
    }

    private void AddRenderer(Renderer rend)
    {
        _objectRenderers.Add(new List<Renderer>() { rend });
        RecreateCommandBuffer();
    }

    private void RemoveRenderers(List<Renderer> renderers)
    {
        _objectRenderers.Remove(renderers);
        RecreateCommandBuffer();
    }

    private void ClearOutlineData()
    {
        _objectRenderers.Clear();
        RecreateCommandBuffer();
    }

    public async void ComputeForTime(Renderer rend, float seconds, Color col, CancellationToken token)
    {
        if (seconds == 0.0f)
        {
            seconds = float.MaxValue;
        }

        AddRenderer(rend);
        // alpha = fill alpha - NOT effect outline alpha;
        // Expression for flashing colour: (Mathf.Sin(Time.time * speed) + 1) / 2.0f
        for (float t = 0; t < seconds; t += Time.deltaTime)
        {
            if (token.IsCancellationRequested)
            {
                float tempHolder = BlurSize;
                BlurSize = 0.0f;
                ChangeColourChange(Color.clear);
                BlurSize = tempHolder;
                return;
            }

            ChangeColourChange(Color.red);
            
            await Task.Delay(TimeSpan.FromSeconds(2.0f));      
        }

        ChangeColourChange(Color.clear);
    }

    private void ChangeColourChange(Color newCol)
    {
        OutlineColor = newCol;
        RecreateCommandBuffer();
    }

    private void RecreateCommandBuffer()
    {
        _commandBuffer.Clear();

        if (_objectRenderers.Count == 0)
            return;

        // initialization
        _commandBuffer.GetTemporaryRT(_depthRTID, _RTWidth, _RTHeight, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
        _commandBuffer.SetRenderTarget(_depthRTID, BuiltinRenderTextureType.CurrentActive);
        _commandBuffer.ClearRenderTarget(false, true, Color.clear);
        // render selected objects into a mask buffer, with different colors for visible vs occluded ones 
        float id = 0f;
        foreach (var collection in _objectRenderers)
        {
            id += 0.25f;
            _commandBuffer.SetGlobalFloat("_ObjectId", id);

            foreach (var render in collection)
            {
                _commandBuffer.DrawRenderer(render, _outlineMaterial, 0, 1);
                _commandBuffer.DrawRenderer(render, _outlineMaterial, 0, 0);
            }
        }


        // object ID edge dectection pass
        _commandBuffer.GetTemporaryRT(_idRTID, _RTWidth, _RTHeight, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
        _commandBuffer.Blit(_depthRTID, _idRTID, _outlineMaterial, 3);

        // Blur
        int rtW = _RTWidth >> Downsample;
        int rtH = _RTHeight >> Downsample;

        _commandBuffer.GetTemporaryRT(_temporaryRTID, rtW, rtH, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
        _commandBuffer.GetTemporaryRT(_blurredRTID, rtW, rtH, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);

        _commandBuffer.Blit(_idRTID, _blurredRTID);

        _commandBuffer.SetGlobalVector("_BlurDirection", new Vector2(BlurSize, 0));
        _commandBuffer.Blit(_blurredRTID, _temporaryRTID, _outlineMaterial, 2);
        _commandBuffer.SetGlobalVector("_BlurDirection", new Vector2(0, BlurSize));
        _commandBuffer.Blit(_temporaryRTID, _blurredRTID, _outlineMaterial, 2);


        // final overlay
        _commandBuffer.SetGlobalColor("_OutlineColor", OutlineColor);
        _commandBuffer.Blit(_blurredRTID, BuiltinRenderTextureType.CameraTarget, _outlineMaterial, 4);

        // release tempRTs
        _commandBuffer.ReleaseTemporaryRT(_blurredRTID);
        _commandBuffer.ReleaseTemporaryRT(_outlineRTID);
        _commandBuffer.ReleaseTemporaryRT(_temporaryRTID);
        _commandBuffer.ReleaseTemporaryRT(_depthRTID);
    }

    Renderer[] FetchOtherAgentsToOutline(AgentInputHandler agentInputHandler)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<Renderer> objectsToOutline = new List<Renderer>();;

        foreach (GameObject element in players)
        {
            if (element != agentInputHandler.agent)
            {
                objectsToOutline.AddRange(element.GetComponentsInChildren<Renderer>());
            }
        }

        return objectsToOutline.ToArray();
    }
}