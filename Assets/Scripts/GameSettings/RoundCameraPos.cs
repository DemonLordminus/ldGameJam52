using UnityEngine;
using Cinemachine;
 
public class RoundCameraPos : CinemachineExtension
{
    //��һ�����絥Ԫ����ʾ32������
    public float PixelsPerUnit = 32;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        //�����������ڴ������ĸ��׶�
        if (stage == CinemachineCore.Stage.Body)
        {
            //��ȡ���������λ��
            Vector3 finalPos = state.FinalPosition;

            // ��������λ��
            Vector3 newPos = new Vector3(Round(finalPos.x), Round(finalPos.y), finalPos.z);

            state.PositionCorrection += newPos - finalPos;
        }
    }
 
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}