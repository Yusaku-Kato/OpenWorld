using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MOBChase : MonoBehaviour
{
    private Animator animator;              //アニメーターを使う
    private NavMeshAgent agent;             //NavMeshAgentを使う
    public Transform target;                //ターゲットに設定できるようにする
    private Rigidbody rb;

    Vector3 newPosition;                    //thisの位置
    Vector3 ordinaryDestination;            //普段のランダムな移動先
    int hitPoint = 10;                      //thisのHP
    public bool isAttacking = false;
    int count = 0;                          //ランダム移動先変更の為のカウント

    // 参照したいオブジェクトをインスペクターからアサインする
    public GameObject targetObject;
    // 参照したいスクリプトを入れる変数
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    //アニメーターを情報取得
        agent = GetComponent<NavMeshAgent>();   //NavMeshAgentを情報取得
        //agent.destination = target.position;    //敵(agent)がプレーヤー(target)に向かう
        rb = GetComponent<Rigidbody>();
        newPosition = this.transform.position;  //thisの位置取得

        // 参照したいスクリプトを取得する
        playerScript = targetObject.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoint <= 0)
        {
            this.tag = "Untagged";
            animator.SetTrigger("Getdeath");
            GameObject.Destroy(this);
        }

        Vector3 prePosition = newPosition;      //1回前の位置を保存
        newPosition = this.transform.position;  //現在の位置

        Vector3 positionDiff = target.position - newPosition;   //プレイヤーとの距離
        if (positionDiff.magnitude <= 2)    //近づくと攻撃
        {
            animator.SetTrigger("Getattack");
        }
        else if (positionDiff.magnitude <= 20)  //プレイヤーのほうに向かっていく
        {
            agent.destination = target.position;
        }
        else if(count == 3) //普段はランダムで移動
        {
            count = 0;
            ordinaryDestination.x = this.transform.position.x + Random.Range(-5.0f, 5.0f);
            ordinaryDestination.z = this.transform.position.z + Random.Range(-5.0f, 5.0f);
            ordinaryDestination.y = this.transform.position.y + Random.Range(-5.0f, 5.0f);
            agent.destination = ordinaryDestination;
        }
        else 
        {
            count++; 
        }

        // 移動のアニメーション
        animator.SetFloat("MoveSpeed", (prePosition - newPosition).magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            // 攻撃判定
            if (playerScript.isAttacking == true)
            {
                animator.SetTrigger("Gethit");       //プレイヤーの剣が当たったらダメージアニメーション発生
                hitPoint = hitPoint - damager.damage;
            }
        }
    }

    // 攻撃判定を開始するメソッド（Animation Eventで呼び出す）
    public void StartAttack()
    {
        isAttacking = true;
    }

    // 攻撃判定を終了するメソッド（Animation Eventで呼び出す）
    public void EndAttack()
    {
        isAttacking = false;
    }
}