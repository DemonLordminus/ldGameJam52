using UnityEngine;
using Cinemachine;
 
public class RoundCameraPos : CinemachineExtension
{
    //在一个世界单元个显示32个像素
    public float PixelsPerUnit = 32;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        //检查摄像机后期处理处于哪个阶段
        if (stage == CinemachineCore.Stage.Body)
        {
            //获取摄像机最终位置
            Vector3 finalPos = state.FinalPosition;

            // 对齐像素位置
            Vector3 newPos = new Vector3(Round(finalPos.x), Round(finalPos.y), finalPos.z);

            state.PositionCorrection += newPos - finalPos;
        }
    }
 
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}