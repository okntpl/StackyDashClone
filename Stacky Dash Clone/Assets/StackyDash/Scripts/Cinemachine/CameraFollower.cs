using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraZ : CinemachineExtension
{
   
   
    [SerializeField] GameObject target;
    [SerializeField] float Zoffset;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
         
            var pos = state.RawPosition;
            pos.z = target.transform.position.z+Zoffset;
            state.RawPosition = pos;
        }
    }
}
