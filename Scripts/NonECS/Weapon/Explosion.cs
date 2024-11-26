using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public BoxCollider BoxCollider;
    public RPGBullet RPGBullet;
    private void OnEnable()
    {
        MakeBoxCollision();
    }
    
    private IEnumerator MakeBoxCollision()
    {
        BoxCollider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        BoxCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<ZombieCombat>().Hit(RPGBullet.Damage);
        }
        else if (other.transform.CompareTag("Boss"))
        {
            if (!other.gameObject.activeSelf) return;
            other.gameObject.GetComponent<BossHitBox>().Hit(RPGBullet.Damage);
        }
    }
}
