using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� �� ����
/// </summary>
public class WeaponOrbit : MonoBehaviour
{
    public List<GameObject> weapons; // ȸ���� �����
    public List<GameObject> weapons2; // ȸ���� �����2
    public List<GameObject> weapons3; // ȸ���� �����3
    public Transform CenterObject; // ������ �߽� ������Ʈ
    public float OrbitRadius = 5.0f; // ���� ������
    public float OrbitSpeed = 5.0f; // ���� �ӵ�
    public float OrbitHeight = 1f;

    private float addedWeaponAngle;
    private bool isGameStop;
    private void Start()
    {
        addedWeaponAngle = 0f;
        for (int i = 0; i < weapons.Count; i++)
        {
            // ���� ������ŭ �� �ڸ���
            float angle = i * Mathf.PI * 2f / weapons.Count;
            // x,z��ǥ ���ϱ�
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;
            // �� ����
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
        
        // ȸ���̴ϱ� �����߰�
        addedWeaponAngle += Time.deltaTime;
        // 2����(360) ������ �Ȱ����ϱ� 2���� ���ֱ�
        if (addedWeaponAngle > Math.PI * 2) { addedWeaponAngle -= (float)Math.PI * 2; }
        for (int i = 0; i < weapons.Count; i++)
        {
            // �߰��� ���� ���
            float angle = i * Mathf.PI * 2f / weapons.Count + addedWeaponAngle;
            // x,z ��ǥ ���ϱ�
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;

            weapons[i].transform.position = new Vector3(CenterObject.position.x + x, OrbitHeight, CenterObject.position.z + z);           
        }
        SetOrbit2();
        SetOrbit3();
    }

    //�˵�2 ���� ����
    private void SetOrbit2()
    {
        for (int i = 0; i < weapons2.Count; i++)
        {
            // ���� ������ŭ �� �ڸ���
            float angle = i * Mathf.PI * 2f / weapons2.Count;
            // x,z��ǥ ���ϱ�
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;
            // �� ����
            weapons2[i].transform.position = new Vector3(CenterObject.position.x + x, OrbitHeight, CenterObject.position.z + z);
        }
    }

    //�˵�3 ���� ����
    private void SetOrbit3()
    {
        for (int i = 0; i < weapons3.Count; i++)
        {
            // ���� ������ŭ �� �ڸ���
            float angle = i * Mathf.PI * 2f / weapons3.Count;
            // x,z��ǥ ���ϱ�
            float x = Mathf.Cos(angle) * OrbitRadius;
            float z = Mathf.Sin(angle) * OrbitRadius;
            // �� ����
            weapons3[i].transform.position = new Vector3(CenterObject.position.x + x, OrbitHeight, CenterObject.position.z + z);
        }
    }
}
