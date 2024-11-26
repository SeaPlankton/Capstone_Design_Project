#if UNITY_EDITOR 

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class AnimationKeyRecorder : EditorWindow
{
    public AnimationClip animationClip;

    public Transform CharacterIK;

    public Transform HipsIK;

    public Transform ChestIK;
    public Transform L_ShoulderIK;
    public Transform R_ShoulderIK;

    public Transform L_ArmIK;
    public Transform L_ArmHintIK;
    public Transform R_ArmIK;
    public Transform R_ArmHintIK;

    public Transform L_LegIK;
    public Transform L_LegHintIK;
    public Transform R_LegIK;
    public Transform R_LegHintIK;


    public Transform[] L_HandIK;
    public Transform[] R_HandIK;

    public Transform WeaponIK;

    public Transform L_GrabIK;
    public Transform L_GrabHintIK;
    public Transform R_GrabIK;
    public Transform R_GrabHintIK;




    public int FrameToRecord = 0;

    private float timeToRecord = 0.0f;


    private bool recordPosition = true;
    private bool recordRotation = false;

    private bool IsRecordCharacter = true;

    private bool IsRecordHips = true;

    private bool IsRecordChest = true;
    private bool IsRecordLShoulder = true;
    private bool IsRecordRShoulder = true;
    private bool IsRecordLArm = true;
    private bool IsRecordLArmHint = true;
    private bool IsRecordRArm = true;
    private bool IsRecordRArmHint = true;

    private bool IsRecordLLeg = true;
    private bool IsRecordLLegHint = true;
    private bool IsRecordRLeg = true;
    private bool IsRecordRLegHint = true;


    private bool IsRecordWeapon = true;

    private bool IsRecordLGrab = true;
    private bool IsRecordLGrabHint = true;
    private bool IsRecordRGrab = true;
    private bool IsRecordRGrabHint = true;






    [MenuItem("IK Recorder/Animation Key Recorder")]
    public static void ShowWindow()
    {
        GetWindow<AnimationKeyRecorder>("Animation Key Recorder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Key Recorder", EditorStyles.boldLabel);

        animationClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", animationClip, typeof(AnimationClip), false);
        GUILayout.Space(10); // Add space between checkbox and label

        GUILayout.BeginHorizontal();
        IsRecordCharacter = EditorGUILayout.Toggle("", IsRecordCharacter, GUILayout.Width(15));
        CharacterIK = (Transform)EditorGUILayout.ObjectField("Character_IK", CharacterIK, typeof(Transform), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        IsRecordHips = EditorGUILayout.Toggle("", IsRecordHips, GUILayout.Width(15));
        HipsIK = (Transform)EditorGUILayout.ObjectField("Hips_IK", HipsIK, typeof(Transform), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        IsRecordChest = EditorGUILayout.Toggle("", IsRecordChest, GUILayout.Width(15));
        ChestIK = (Transform)EditorGUILayout.ObjectField("Chest_IK", ChestIK, typeof(Transform), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        IsRecordLShoulder = EditorGUILayout.Toggle("", IsRecordLShoulder, GUILayout.Width(15));
        L_ShoulderIK = (Transform)EditorGUILayout.ObjectField("L_Shoulder_IK", L_ShoulderIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordRShoulder = EditorGUILayout.Toggle("", IsRecordRShoulder, GUILayout.Width(15));
        R_ShoulderIK = (Transform)EditorGUILayout.ObjectField("R_Shoulder_IK", R_ShoulderIK, typeof(Transform), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        IsRecordLArm = EditorGUILayout.Toggle("", IsRecordLArm, GUILayout.Width(15));
        L_ArmIK = (Transform)EditorGUILayout.ObjectField("L_Arm_IK", L_ArmIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordLArmHint = EditorGUILayout.Toggle("", IsRecordLArmHint, GUILayout.Width(15));
        L_ArmHintIK = (Transform)EditorGUILayout.ObjectField("L_ArmHint_IK", L_ArmHintIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordRArm = EditorGUILayout.Toggle("", IsRecordRArm, GUILayout.Width(15));
        R_ArmIK = (Transform)EditorGUILayout.ObjectField("R_Arm_IK", R_ArmIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordRArmHint = EditorGUILayout.Toggle("", IsRecordRArmHint, GUILayout.Width(15));
        R_ArmHintIK = (Transform)EditorGUILayout.ObjectField("R_ArmHint_IK", R_ArmHintIK, typeof(Transform), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        IsRecordLLeg = EditorGUILayout.Toggle("", IsRecordLLeg, GUILayout.Width(15));
        L_LegIK = (Transform)EditorGUILayout.ObjectField("L_Leg_IK", L_LegIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordRLeg = EditorGUILayout.Toggle("", IsRecordRLeg, GUILayout.Width(15));
        R_LegIK = (Transform)EditorGUILayout.ObjectField("R_Leg_IK", R_LegIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordLLegHint = EditorGUILayout.Toggle("", IsRecordLLegHint, GUILayout.Width(15));
        L_LegHintIK = (Transform)EditorGUILayout.ObjectField("L_LegHint_IK", L_LegHintIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordRLegHint = EditorGUILayout.Toggle("", IsRecordRLegHint, GUILayout.Width(15));
        R_LegHintIK = (Transform)EditorGUILayout.ObjectField("R_LegHint_IK", R_LegHintIK, typeof(Transform), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        IsRecordWeapon = EditorGUILayout.Toggle("", IsRecordWeapon, GUILayout.Width(15));
        WeaponIK = (Transform)EditorGUILayout.ObjectField("Weapon_IK", WeaponIK, typeof(Transform), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        IsRecordLGrab = EditorGUILayout.Toggle("", IsRecordLGrab, GUILayout.Width(15));
        L_GrabIK = (Transform)EditorGUILayout.ObjectField("L_Grab_IK", L_GrabIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordLGrabHint = EditorGUILayout.Toggle("", IsRecordLGrabHint, GUILayout.Width(15));
        L_GrabHintIK = (Transform)EditorGUILayout.ObjectField("L_GrabHint_IK", L_GrabHintIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordRGrab = EditorGUILayout.Toggle("", IsRecordRGrab, GUILayout.Width(15));
        R_GrabIK = (Transform)EditorGUILayout.ObjectField("R_Grab_IK", R_GrabIK, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        IsRecordRGrabHint = EditorGUILayout.Toggle("", IsRecordRGrabHint, GUILayout.Width(15));
        R_GrabHintIK = (Transform)EditorGUILayout.ObjectField("R_GrabHint_IK", R_GrabHintIK, typeof(Transform), true);
        GUILayout.EndHorizontal();



        // Position and rotation checkboxes
        GUILayout.Space(10);
        recordPosition = EditorGUILayout.Toggle("Record Position", recordPosition);
        recordRotation = EditorGUILayout.Toggle("Record Rotation", recordRotation);


        FrameToRecord = EditorGUILayout.IntField("Frame to Record (Frames)", FrameToRecord);
        timeToRecord = FrameToRecord / 60 + (FrameToRecord % 60) * 0.0166667f;

        GUILayout.Space(20);

        //=====================================================================================================================
        // 타겟 등록
        if (GUILayout.Button("Record Keyframes"))
        {
            if (animationClip == null)
            {
                Debug.LogError("Animation Clip이 설정되지 않았습니다.");
                return;
            }

            //여기서 타겟수 늘리는 작업 들어감 if (target1 != null) RecordKeyframe(timeToRecord, target1) <- 이렇게 한줄 늘리면 한개 추가됨
            if (IsRecordCharacter && CharacterIK != null) RecordKeyframe(timeToRecord, CharacterIK);
            
            if (IsRecordHips && HipsIK != null) RecordKeyframe(timeToRecord, HipsIK);
            
            if (IsRecordChest && ChestIK != null) RecordKeyframe(timeToRecord, ChestIK);
            if (IsRecordLShoulder && L_ShoulderIK != null) RecordKeyframe(timeToRecord, L_ShoulderIK);
            if (IsRecordRShoulder && R_ShoulderIK != null) RecordKeyframe(timeToRecord, R_ShoulderIK);
            if (IsRecordLArm && L_ArmIK != null) RecordKeyframe(timeToRecord, L_ArmIK);
            if (IsRecordLArmHint && L_ArmHintIK != null) RecordKeyframe(timeToRecord, L_ArmHintIK);
            if (IsRecordRArm && R_ArmIK != null) RecordKeyframe(timeToRecord, R_ArmIK);
            if (IsRecordRArmHint && R_ArmHintIK != null) RecordKeyframe(timeToRecord, R_ArmHintIK);
            if (IsRecordLLeg && L_LegIK != null) RecordKeyframe(timeToRecord, L_LegIK);
            if (IsRecordLLegHint && L_LegHintIK != null) RecordKeyframe(timeToRecord, L_LegHintIK);
            if (IsRecordRLeg && R_LegIK != null) RecordKeyframe(timeToRecord, R_LegIK);
            if (IsRecordRLegHint && R_LegHintIK != null) RecordKeyframe(timeToRecord, R_LegHintIK);

            if (IsRecordWeapon && WeaponIK != null) RecordKeyframe(timeToRecord, WeaponIK);

            if (IsRecordLGrab && L_GrabIK != null) RecordKeyframe(timeToRecord, L_GrabIK);
            if (IsRecordLGrabHint && L_GrabHintIK != null) RecordKeyframe(timeToRecord, L_GrabHintIK);
            if (IsRecordRGrab && R_GrabIK != null) RecordKeyframe(timeToRecord, R_GrabIK);
            if (IsRecordRGrabHint && R_GrabHintIK != null) RecordKeyframe(timeToRecord, R_GrabHintIK);


            Debug.Log("Keyframes recorded at " + FrameToRecord + " frames.");
        }

    }

    private void DrawTransformArray(string label, ref Transform[] array, ref bool isRecord)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(80));
        if (GUILayout.Button("Add", GUILayout.Width(60)))
        {
            List<Transform> tempArray = new List<Transform>(array) { null };
            array = tempArray.ToArray();
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < array.Length; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                List<Transform> tempArray = new List<Transform>(array);
                tempArray.RemoveAt(i);
                array = tempArray.ToArray();
                // Exit loop after removing to avoid index issues
                break;
            }
            array[i] = (Transform)EditorGUILayout.ObjectField(array[i], typeof(Transform), true);
            GUILayout.EndHorizontal();
        }
    }


    private void RecordKeyframe(float time, Transform target)
    {
        Debug.Log("====================================================");
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(animationClip);
        // 각 바인딩에서 AnimationCurve 가져오기
        Debug.Log($"Set Target ({target.name}) Local Transform : {target.localPosition}, time : {time}");


        RecordOrCreateCurve(target, curveBindings, "m_LocalPosition.x", time, target.localPosition.x);
        RecordOrCreateCurve(target, curveBindings, "m_LocalPosition.y", time, target.localPosition.y);
        RecordOrCreateCurve(target, curveBindings, "m_LocalPosition.z", time, target.localPosition.z);

        Debug.Log("===localEulerAnglesRaw.y===" + target.localEulerAngles.y);


        RecordOrCreateCurve(target, curveBindings, "localEulerAnglesRaw.x", time, target.localEulerAngles.x);
        RecordOrCreateCurve(target, curveBindings, "localEulerAnglesRaw.y", time, target.localEulerAngles.y);
        RecordOrCreateCurve(target, curveBindings, "localEulerAnglesRaw.z", time, target.localEulerAngles.z);


        foreach (var binding in curveBindings)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);
            Debug.Log($"Length: {curveBindings.Length}, Property: {binding.propertyName}, Curve: {curve}, path : {binding.path}");
            string[] parts = binding.path.Split('/');
            string name = parts[parts.Length - 1];
            if (recordPosition && name == target.name && binding.propertyName == "m_LocalPosition.x")
            {
                if (curve == null) curve = new AnimationCurve();
                Debug.Log($"1 : {target.localPosition.x}");

                bool exist = false;
                int i;

                for (i = 0; i < curve.keys.Length; i++)
                {
                    if (Mathf.Abs(time - curve.keys[i].time) < 0.001f)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    // 이미 애니메이션 키가 존재한다면 새롭게 교체
                    curve.MoveKey(i, new Keyframe(time, target.localPosition.x));
                }
                else
                {
                    // 없다면 그냥 더하기
                    curve.AddKey(new Keyframe(time, target.localPosition.x));
                }

            }
            else if (recordPosition && name == target.name && binding.propertyName == "m_LocalPosition.y")
            {
                if (curve == null) curve = new AnimationCurve();
                Debug.Log($"2 : {target.localPosition.y}");

                bool exist = false;
                int i;

                for (i = 0; i < curve.keys.Length; i++)
                {
                    if (Mathf.Abs(time - curve.keys[i].time) < 0.001f)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    // 이미 애니메이션 키가 존재한다면 새롭게 교체
                    curve.MoveKey(i, new Keyframe(time, target.localPosition.y));
                }
                else
                {
                    // 없다면 그냥 더하기
                    curve.AddKey(new Keyframe(time, target.localPosition.y));
                }
            }
            else if (recordPosition && name == target.name && binding.propertyName == "m_LocalPosition.z")
            {
                if (curve == null) curve = new AnimationCurve();
                Debug.Log($"3 : {target.localPosition.z}");

                bool exist = false;
                int i;

                for (i = 0; i < curve.keys.Length; i++)
                {
                    if (Mathf.Abs(time - curve.keys[i].time) < 0.001f)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    // 이미 애니메이션 키가 존재한다면 새롭게 교체
                    curve.MoveKey(i, new Keyframe(time, target.localPosition.z));
                }
                else
                {
                    // 없다면 그냥 더하기
                    curve.AddKey(new Keyframe(time, target.localPosition.z));
                }
            }
            else if (recordRotation && name == target.name && binding.propertyName == "localEulerAnglesRaw.x")
            {
                Quaternion rotation = target.localRotation;
                Vector3 eulerAngles = rotation.eulerAngles;

                if (curve == null) curve = new AnimationCurve();
                Debug.Log($"4 : {eulerAngles.x}");

                bool exist = false;
                int i;

                for (i = 0; i < curve.keys.Length; i++)
                {
                    if (Mathf.Abs(time - curve.keys[i].time) < 0.001f)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    // 이미 애니메이션 키가 존재한다면 새롭게 교체
                    curve.MoveKey(i, new Keyframe(time, eulerAngles.x));
                }
                else
                {
                    // 없다면 그냥 더하기
                    curve.AddKey(new Keyframe(time, eulerAngles.x));
                }
            }
            else if (recordRotation && name == target.name && binding.propertyName == "localEulerAnglesRaw.y")
            {
                Quaternion rotation = target.localRotation;
                Vector3 eulerAngles = rotation.eulerAngles;

                if (curve == null) curve = new AnimationCurve();
                Debug.Log($"5 : {eulerAngles.y}");

                bool exist = false;
                int i;

                for (i = 0; i < curve.keys.Length; i++)
                {
                    if (Mathf.Abs(time - curve.keys[i].time) < 0.001f)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    // 이미 애니메이션 키가 존재한다면 새롭게 교체
                    curve.MoveKey(i, new Keyframe(time, eulerAngles.y));
                }
                else
                {
                    // 없다면 그냥 더하기
                    curve.AddKey(new Keyframe(time, eulerAngles.y));
                }
            }
            else if (recordRotation && name == target.name && binding.propertyName == "localEulerAnglesRaw.z")
            {
                Quaternion rotation = target.localRotation;
                Vector3 eulerAngles = rotation.eulerAngles;

                if (curve == null) curve = new AnimationCurve();
                Debug.Log($"6 : {eulerAngles.z}");

                bool exist = false;
                int i;

                for (i = 0; i < curve.keys.Length; i++)
                {
                    if (Mathf.Abs(time - curve.keys[i].time) < 0.001f)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    // 이미 애니메이션 키가 존재한다면 새롭게 교체
                    curve.MoveKey(i, new Keyframe(time, eulerAngles.z));
                }
                else
                {
                    // 없다면 그냥 더하기
                    curve.AddKey(new Keyframe(time, eulerAngles.z));
                }
            }

            // Set Bezier interpolation to the curve
            SetBezierInterpolation(curve);

            AnimationUtility.SetEditorCurve(animationClip, binding, curve);
        }
    }

    private void RecordOrCreateCurve(Transform target, EditorCurveBinding[] bindings, string property, float time, float value)
    {
        string targetPath = AnimationUtility.CalculateTransformPath(target, target.root);
        EditorCurveBinding binding = new EditorCurveBinding
        {
            path = targetPath,
            type = typeof(Transform),
            propertyName = property
        };

        // 해당 바인딩이 있는지 확인
        AnimationCurve curve = null;
        foreach (var b in bindings)
        {
            if (b.path == targetPath && b.propertyName == property)
            {
                curve = AnimationUtility.GetEditorCurve(animationClip, b);
                break;
            }
        }

        // 바인딩이 없으면 새로 생성
        if (curve == null)
        {
            curve = new AnimationCurve();
            Debug.Log($"Creating new curve for {target.name} - {property}");
        }
        else
        {
            Debug.Log($"Found existing curve for {target.name} - {property}");
        }


        Debug.Log("커브 값 : " + value);

        // 키 추가
        curve.AddKey(new Keyframe(time, value));

        // 애니메이션 클립에 커브 설정
        animationClip.SetCurve(targetPath, typeof(Transform), property, curve);
    }


    //부드럽게 바꾸는 용도 
    private void SetBezierInterpolation(AnimationCurve curve)
    {
        for (int i = 0; i < curve.length; i++)
        {
            Keyframe keyframe = curve[i];
            keyframe.inTangent = 0f;
            keyframe.outTangent = 0f;
            curve.MoveKey(i, keyframe);
        }
    }
}

#endif