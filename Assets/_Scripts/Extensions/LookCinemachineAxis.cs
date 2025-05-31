using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Extensions
{
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")]
    public class LookCinemachineAxis : CinemachineExtension
    {
        public float xClampValue = 0;
        
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, 
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var position = state.RawPosition;
                position.x = xClampValue;
                state.RawPosition = position;
            }
        }
    }
}