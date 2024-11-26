using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollow : MonoBehaviour
{

    public Transform target;  // 타겟 객체의 Transform
    public Transform headTransform;  // 얼굴(헤드)의 Transform
    public SkinnedMeshRenderer skinnedMeshRenderer;  // 눈동자에 적용된 SkinnedMeshRenderer

    // BlendShape 인덱스
    public int blendShapeIndexLeft;  // 왼쪽 BlendShape 인덱스
    public int blendShapeIndexRight; // 오른쪽 BlendShape 인덱스
    public int blendShapeIndexUp;    // 위쪽 BlendShape 인덱스
    public int blendShapeIndexDown;  // 아래쪽 BlendShape 인덱스

    public float maxBlendShapeValue = 100.0f;  // BlendShape 값의 최대값
    public float maxAngle = 45.0f;  // 최대 각도 (도 단위)

    [Range(-45, 45)]
    public float horizontalAngle;  // 수평 방향 각도

    [Range(-45, 45)]
    public float verticalAngle;    // 수직 방향 각도

    [SerializeField]
    private bool applyLeftFollow = false;
    [SerializeField]
    private bool applyRightFollow = false;
    [SerializeField]
    private bool applyUpFollow = false;
    [SerializeField]
    private bool applyDownFollow = false;
    
    private void Start()
    {
        blendShapeIndexLeft = GetBlendshapeIndex("blendShape1.VRM_LookLeft");
        blendShapeIndexRight = GetBlendshapeIndex("blendShape1.VRM_LoodRight");
        blendShapeIndexUp = GetBlendshapeIndex("blendShape1.VRM_LookUp");
        blendShapeIndexDown = GetBlendshapeIndex("blendShape1.VRM_LookDown");
    }

    private void Update()
    {
        if (target == null || skinnedMeshRenderer == null)
        {
            Debug.LogWarning("Target or SkinnedMeshRenderer is not assigned.");
            //return;
        }

        // 얼굴(헤드)의 정면 방향 벡터
        Vector3 headForward = headTransform.forward;

        // 얼굴(헤드)의 위치
        Vector3 headPosition = headTransform.position;

        // 타겟과 얼굴(헤드) 사이의 방향 벡터
        Vector3 directionToTarget = target.position - headPosition;

        // 얼굴의 정면 방향을 기준으로 타겟의 상대 위치를 프로젝션
        Vector3 right = headTransform.right;
        Vector3 up = headTransform.up;

        // 수평과 수직 각도를 타겟의 위치에 맞게 설정
        Vector3 projectedOnFace = Vector3.ProjectOnPlane(directionToTarget, up);
        Vector3 projectedOnRight = Vector3.ProjectOnPlane(directionToTarget, right);

        // 수평과 수직 각도 계산
        float horizontal = Vector3.SignedAngle(headForward, projectedOnFace, up);
        float vertical = Vector3.SignedAngle(headForward, projectedOnRight, right);

        // 각도를 범위 내로 클램프
        horizontalAngle = Mathf.Clamp(horizontal, -maxAngle, maxAngle);
        verticalAngle = Mathf.Clamp(vertical, -maxAngle, maxAngle);

        // BlendShape 값을 각도에 비례하도록 설정
        float blendShapeValueHorizontal = (Mathf.Abs(horizontalAngle) / maxAngle) * maxBlendShapeValue;
        float blendShapeValueVertical = (Mathf.Abs(verticalAngle) / maxAngle) * maxBlendShapeValue;

        // 타겟의 방향에 따라 BlendShape 값을 설정
        float blendShapeValueLeft = (horizontalAngle < 0) ? blendShapeValueHorizontal : 0.0f;
        float blendShapeValueRight = (horizontalAngle > 0) ? blendShapeValueHorizontal : 0.0f;
        float blendShapeValueUp = (verticalAngle < 0) ? blendShapeValueVertical : 0.0f;
        float blendShapeValueDown = (verticalAngle > 0) ? blendShapeValueVertical : 0.0f;



        // BlendShape 값을 설정
        if (applyLeftFollow == true)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndexLeft, blendShapeValueLeft);
        }
        if(applyRightFollow == true)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndexRight, blendShapeValueRight);
        }
        if(applyUpFollow == true)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndexUp, blendShapeValueUp);
        }
        if(applyDownFollow == true)
        {

            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndexDown, blendShapeValueDown);
        }
    }

    private int GetBlendshapeIndex(string blendShapeName)
    {
        Mesh mesh = skinnedMeshRenderer.sharedMesh;
        int blendShapeIndex = mesh.GetBlendShapeIndex(blendShapeName);
        return blendShapeIndex;
    }
}
