using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGAOE : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    //������ ������
    public float Damage = 10f;
    //������ ����ð�
    private float count = 0f;
    //�������� �� �ð� ����
    private float damageInterval = 0.5f;
    //�������� �� ������ ����
    private HashSet<GameObject> damagedZombie = new HashSet<GameObject>();
    //���� ���� üũ
    private bool _isGameStop = false;

    private void Start()
    {
        Managers.Instance.Game.TimeController.GameStop += OnGameStop;
        Managers.Instance.Game.TimeController.GameResume += OnGameResume;
    }

    private void OnEnable()
    {
        count = 0f;
        damagedZombie.Clear();
        StartCoroutine(DamageEnemies());
        Debug.Log("���� Enable");
    }

    private void OnDisable()
    {
        count = 0f;
        damagedZombie.Clear();
        Debug.Log("���� Disable");
    }

    public void OnGameStop()
    {
        _isGameStop = true;
        ParticleSystem.Pause();
    }

    public void OnGameResume()
    {
        _isGameStop = false;
        ParticleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameStop)
            return;
        count += Time.deltaTime;

        if(count > 5f)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            damagedZombie.Add(other.gameObject);
            Debug.Log("���� Trigger Ȯ��");
        }
        else if (other.transform.CompareTag("Boss"))
        {
            if (!other.gameObject.activeSelf) return;
            //damagedZombie.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���ǿ��� ������ List���� ����
        if (other.CompareTag("Enemy"))
        {
            damagedZombie.Remove(other.gameObject);
        }
    }

    private IEnumerator DamageEnemies()
    {

        while (true)
        {
            // 0.5�� ���
            yield return new WaitForSeconds(damageInterval);
            Debug.Log("RPGAOE ���� ���� Ȯ�� " + _isGameStop);

            if (!_isGameStop)
            {
                // HashSet�� �ִ� ��� ������ �� ���� ������ �ο�
                foreach (GameObject enemy in damagedZombie)
                {
                    enemy.GetComponent<ZombieCombat>().Hit(Damage);
                    Debug.Log("���ǵ����� " + Damage);
                }

                // �������� �� �� HashSet�� ����� ���� �ֱ⿡ �ٽ� �˻��� �� �ֵ��� ��
                damagedZombie.Clear();
            }         
        }
    }
}
