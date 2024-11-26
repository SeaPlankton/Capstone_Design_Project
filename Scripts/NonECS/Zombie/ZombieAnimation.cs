using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// VAT BAKING = 뼈대 애니메이션을 뼈대 없이 재생
/// VatBakerOutput 폴더에는 2개의 머티리얼이 있고 그 두개다 껴주면 자연스럽게 애니메이션이 재생하는 방식
/// Mesh
/// </summary>
[RequireComponent(typeof(Zombie))]
public class ZombieAnimation : MonoBehaviour
{
    public enum EnemyState
    {
        Idle = 0,
        WalkForward = 1,
        DeathStanding = 2,
        Death = 3,
        Attack = 4
    }
    public MeshRenderer _meshRenderer;
    public EnemyState PresentState;
    [HideInInspector]
    public Zombie Zombie;
    [Space]
    [Header("Vat Baker Materials")]
    public Material WalkForwardHead;
    public Material WalkForwardBody;
    public Material IdleHead;
    public Material IdleBody;
    public Material DeathHead;
    public Material DeathBody;
    public Material DeathStandingHead;
    public Material DeathStandingBody;
    public Material AttackHead;
    public Material AttackBody;


    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        Zombie = GetComponent<Zombie>();
    }
    public void ChangeState(EnemyState enemyState)
    {
        if (PresentState == enemyState) return;
        PresentState = enemyState;
        Material[] mat = _meshRenderer.materials;
        if (enemyState == EnemyState.Idle)
        {
            mat[0] = IdleHead;
            mat[1] = IdleBody;
        }
        else if (enemyState == EnemyState.WalkForward)
        {
            mat[0] = WalkForwardHead;
            mat[1] = WalkForwardBody;
        }
        else if (enemyState == EnemyState.DeathStanding)
        {
            mat[0] = DeathStandingHead;
            mat[1] = DeathStandingBody;
        }
        else if (enemyState == EnemyState.Death)
        {
            mat[0] = DeathHead;
            mat[1] = DeathBody;
        }
        else if (enemyState == EnemyState.Attack)
        {
            mat[0] = AttackHead;
            mat[1] = AttackBody;
        }
        _meshRenderer.materials = mat;
    }
}
