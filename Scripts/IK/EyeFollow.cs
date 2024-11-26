using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollow : MonoBehaviour
{

    public Transform target;  // Ÿ�� ��ü�� Transform
    public Transform headTransform;  // ��(���)�� Transform
    public SkinnedMeshRenderer skinnedMeshRenderer;  // �����ڿ� ����� SkinnedMeshRenderer

    // BlendShape �ε���
    public int blendShapeIndexLeft;  // ���� BlendShape �ε���
    public int blendShapeIndexRight; // ������ BlendShape �ε���
    public int blendShapeIndexUp;    // ���� BlendShape �ε���
    public int blendShapeIndexDown;  // �Ʒ��� BlendShape �ε���

    public float maxBlendShapeValue = 100.0f;  // BlendShape ���� �ִ밪
    public float maxAngle = 45.0f;  // �ִ� ���� (�� ����)

    [Range(-45, 45)]
    public float horizontalAngle;  // ���� ���� ����

    [Range(-45, 45)]
    public float verticalAngle;    // ���� ���� ����

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

        // ��(���)�� ���� ���� ����
        Vector3 headForward = headTransform.forward;

        // ��(���)�� ��ġ
        Vector3 headPosition = headTransform.position;

        // Ÿ�ٰ� ��(���) ������ ���� ����
        Vector3 directionToTarget = target.position - headPosition;

        // ���� ���� ������ �������� Ÿ���� ��� ��ġ�� ��������
        Vector3 right = headTransform.right;
        Vector3 up = headTransform.up;

        // ����� ���� ������ Ÿ���� ��ġ�� �°� ����
        Vector3 projectedOnFace = Vector3.ProjectOnPlane(directionToTarget, up);
        Vector3 projectedOnRight = Vector3.ProjectOnPlane(directionToTarget, right);

        // ����� ���� ���� ���
        float horizontal = Vector3.SignedAngle(headForward, projectedOnFace, up);
        float vertical = Vector3.SignedAngle(headForward, projectedOnRight, right);

        // ������ ���� ���� Ŭ����
        horizontalAngle = Mathf.Clamp(horizontal, -maxAngle, maxAngle);
        verticalAngle = Mathf.Clamp(vertical, -maxAngle, maxAngle);

        // BlendShape ���� ������ ����ϵ��� ����
        float blendShapeValueHorizontal = (Mathf.Abs(horizontalAngle) / maxAngle) * maxBlendShapeValue;
        float blendShapeValueVertical = (Mathf.Abs(verticalAngle) / maxAngle) * maxBlendShapeValue;

        // Ÿ���� ���⿡ ���� BlendShape ���� ����
        float blendShapeValueLeft = (horizontalAngle < 0) ? blendShapeValueHorizontal : 0.0f;
        float blendShapeValueRight = (horizontalAngle > 0) ? blendShapeValueHorizontal : 0.0f;
        float blendShapeValueUp = (verticalAngle < 0) ? blendShapeValueVertical : 0.0f;
        float blendShapeValueDown = (verticalAngle > 0) ? blendShapeValueVertical : 0.0f;



        // BlendShape ���� ����
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
