using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGAOE : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    //장판의 데미지
    public float Damage = 10f;
    //장판의 수명시간
    private float count = 0f;
    //데미지를 줄 시간 간격
    private float damageInterval = 0.5f;
    //데미지를 준 적들을 추적
    private HashSet<GameObject> damagedZombie = new HashSet<GameObject>();
    //게임 정지 체크
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
        Debug.Log("장판 Enable");
    }

    private void OnDisable()
    {
        count = 0f;
        damagedZombie.Clear();
        Debug.Log("장판 Disable");
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
            Debug.Log("좀비 Trigger 확인");
        }
        else if (other.transform.CompareTag("Boss"))
        {
            if (!other.gameObject.activeSelf) return;
            //damagedZombie.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 장판에서 나가면 List에서 제거
        if (other.CompareTag("Enemy"))
        {
            damagedZombie.Remove(other.gameObject);
        }
    }

    private IEnumerator DamageEnemies()
    {

        while (true)
        {
            // 0.5초 대기
            yield return new WaitForSeconds(damageInterval);
            Debug.Log("RPGAOE 게임 상태 확인 " + _isGameStop);

            if (!_isGameStop)
            {
                // HashSet에 있는 모든 적에게 한 번씩 데미지 부여
                foreach (GameObject enemy in damagedZombie)
                {
                    enemy.GetComponent<ZombieCombat>().Hit(Damage);
                    Debug.Log("장판데미지 " + Damage);
                }

                // 데미지를 준 후 HashSet을 비워서 다음 주기에 다시 검사할 수 있도록 함
                damagedZombie.Clear();
            }         
        }
    }
}
