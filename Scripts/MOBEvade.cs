using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MOBEvade : MonoBehaviour
{
    private Animator animator;              //アニメーターを使う
    private NavMeshAgent agent;             //NavMeshAgentを使う
    public Transform target;                //ターゲットに設定できるようにする
    private Rigidbody rb;

    Vector3 newPosition;                    //thisの位置
    Vector3 evadeDestination;               //thisの目的座標
    int hitPoint = 10;                      //thisのHP

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
        evadeDestination = this.transform.position;

        // 参照したいスクリプトを取得する
        playerScript = targetObject.GetComponent<PlayerScript>();

        //targetObject = GameObject.Find("DogPolyart");
    }

    // Update is called once per frame
    void Update()
    {
        //targetObject = GameObject.Find("DogPolyart");
        if (hitPoint <= 0)
        {
            this.tag = "Untagged";
            animator.SetTrigger("Getdeath");
            GameObject.Destroy(this);
        }

        Vector3 prePosition = newPosition;      //1回前の位置を保存
        newPosition = this.transform.position;  //現在の位置

        Vector3 positionDiff = target.position - newPosition;   //プレイヤーとの距離
        if (positionDiff.magnitude <= 10)   //プレイヤーが近づくと
        {
            evadeDestination.x = this.transform.position.x + (this.transform.position.x - target.position.x);
            evadeDestination.z = this.transform.position.z + (this.transform.position.z - target.position.z);
            evadeDestination.y = this.transform.position.y;
            agent.destination = evadeDestination;    //プレイヤーから逃げる
        }
        else    //普段の行動
        {
            evadeDestination.x = this.transform.position.x + Random.Range(-5.0f, 5.0f);
            evadeDestination.z = this.transform.position.z + Random.Range(-5.0f, 5.0f);
            evadeDestination.y = this.transform.position.y + Random.Range(-5.0f, 5.0f);
            agent.destination = evadeDestination;   //普段はランダムで移動
        }

        // 移動のアニメーション
        //animator.SetFloat("MoveSpeed", (evadeDestination - this.transform.position).magnitude);
        animator.SetFloat("MoveSpeed", (newPosition - prePosition).magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            // 攻撃判定
            if (playerScript.isAttacking == true)
            {
                //プレイヤーの剣が当たったらダメージアニメーション発生
                animator.SetTrigger("Gethit");
                hitPoint = hitPoint - damager.damage;
            }
        }
    }
}