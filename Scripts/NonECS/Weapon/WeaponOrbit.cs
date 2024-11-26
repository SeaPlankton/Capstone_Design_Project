using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 자전 및 공전
/// </summary>
public class WeaponOrbit : MonoBehaviour
{
    public List<GameObject> weapons; // 회전할 무기들
    public List<GameObject> weapons2; // 회전할 무기들2
    public List<GameObject> weapons3; // 회전할 무기들3
    public Transform CenterObject; // 공전할 중심 오브젝트
    public float OrbitRadius = 5.0f; // 공전 반지름
    public float OrbitSpeed = 5.0f; // 공전 속도
    public float OrbitHeight = 1f;

    private float addedWeaponAngle;
    private bool isGameStop;
    private void Start()
    {
        addedWeaponAngle = 0f;
        for (int i = 0; i < weapons.Count; i++)
        {
            // 웨폰 갯수만큼 원 자르기
            float angle = i * Mathf.PI * 2f / weapons.Count;
            // x,z좌표 구하기
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;
            // 원 설정
            weapons[i].transform.position = new Vector3(CenterObject.position.x + x, OrbitHeight, CenterObject.position.z + z);
        }
        Managers.Instance.Game.TimeController.GameStop += OnGameStop;
        Managers.Instance.Game.TimeController.GameResume += OnGameResume;
    }
    public void OnGameStop()
    {
        isGameStop = true;
    }
    public void OnGameResume()
    {
        isGameStop = false;
    }
    void Update()
    {
        if (isGameStop) return;
        
        // 회전이니까 각도추가
        addedWeaponAngle += Time.deltaTime;
        // 2파이(360) 넘으면 똑같으니까 2파이 빼주기
        if (addedWeaponAngle > Math.PI * 2) { addedWeaponAngle -= (float)Math.PI * 2; }
        for (int i = 0; i < weapons.Count; i++)
        {
            // 추가된 각도 계산
            float angle = i * Mathf.PI * 2f / weapons.Count + addedWeaponAngle;
            // x,z 좌표 구하기
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;

            weapons[i].transform.position = new Vector3(CenterObject.position.x + x, OrbitHeight, CenterObject.position.z + z);           
        }
        SetOrbit2();
        SetOrbit3();
    }

    //궤도2 각도 설정
    private void SetOrbit2()
    {
        for (int i = 0; i < weapons2.Count; i++)
        {
            // 웨폰 갯수만큼 원 자르기
            float angle = i * Mathf.PI * 2f / weapons2.Count;
            // x,z좌표 구하기
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;
            // 원 설정
            weapons2[i].transform.position = new Vector3(CenterObject.position.x + x, OrbitHeight, CenterObject.position.z + z);
        }
    }

    //궤도3 각도 설정
    private void SetOrbit3()
    {
        for (int i = 0; i < weapons3.Count; i++)
        {
            // 웨폰 갯수만큼 원 자르기
            float angle = i * Mathf.PI * 2f / weapons3.Count;
            // x,z좌표 구하기
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;
            // 원 설정
            weapons3[i].transform.position = new Vector3(CenterObject.position.x + x, OrbitHeight, CenterObject.position.z + z);
        }
    }
}
