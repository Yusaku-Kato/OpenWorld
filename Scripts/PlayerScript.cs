using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using System;

public class PlayerScript : MonoBehaviour
{
    //private NavMeshAgent agent;
    //private GameObject navDestinationPos = null;
    CharacterController con;
    Animator anim;
    CinemachineFreeLook freeLook;

    // Mボタンが押されたか
    private bool isPushedM = false;
    // ミニマップ拡大時に視点移動をさせないようにするためにFreeLookCameraオブジェクトを呼び出す
    public GameObject freeLookCamera;

    float normalSpeed = 6f; // 通常時の移動速度
    float sprintSpeed = 10f; // ダッシュ時の移動速度
    float jump = 16f;        // ジャンプ力
    float gravity = 20f;    // 重力の大きさ

    Vector3 moveDirection = Vector3.zero;
    public bool isAttacking = false;
    Vector3 startPos;

    // 参照したいオブジェクトをインスペクターからアサインする
    public GameObject targetObject;
    //public GameObject[] targetObject = GameObject.FindGameObjectsWithTag("Enemy");
    // 参照したいスクリプトを入れる変数
    private MOBChase mobChase;

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        //agent.destination = this.transform.position;
        con = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        freeLookCamera = GameObject.Find("FreeLook Camera");
        freeLook = freeLookCamera.GetComponent<CinemachineFreeLook>();

        // マウスカーソルを非表示にし、位置を固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startPos = transform.position;

        // 参照したいスクリプトを取得する
        mobChase = targetObject.GetComponent<MOBChase>();
    }

    void Update()
    {
        //ナビゲーション
        //navDestinationPos = GameObject.Find("NavDestination(Clone)");
        //agent.destination = navDestinationPos.transform.position;

        //Mボタンでマウスカーソル表示
        if (Input.GetKeyDown(KeyCode.M))
        {
            mouseCursor();
        }

        // 左クリックの判定初期化
        anim.SetBool("LeftClick", false);

        // 移動速度を取得
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;

        // カメラの向きを基準にした正面方向のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 前後左右の入力（WASDキー）から、移動のためのベクトルを計算
        // Input.GetAxis("Vertical") は前後（WSキー）の入力値
        // Input.GetAxis("Horizontal") は左右（ADキー）の入力値
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        Vector3 moveZ = cameraForward * v * speed;  //前後（カメラ基準）
        Vector3 moveX = Camera.main.transform.right * h * speed; // 左右（カメラ基準）
        //Debug.Log(v);
        //Debug.Log(h);

        //ゲームオブジェクト取得のテスト
        //GameObject obj = null;
        //obj = GameObject.Find("DogPolyart");
        //Debug.Log(obj);

        // isGrounded は地面にいるかどうかを判定します
        // 地面にいるときはジャンプを可能に
        if (con.isGrounded)
        {
            moveDirection = moveZ + moveX;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jump;
            }
        }
        else
        {
            // 重力を効かせる
            moveDirection = moveZ + moveX + new Vector3(0, moveDirection.y, 0);
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // 移動のアニメーション
        anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);


        // プレイヤーの向きを入力の向きに変更
        transform.LookAt(transform.position + moveZ + moveX);

        // Move は指定したベクトルだけ移動させる命令
        con.Move(moveDirection * Time.deltaTime);

        // 左クリックの判定
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("LeftClick", true);
        }

        // 右クリックの判定
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetBool("RightClick", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("RightClick", false);
        }
    }

    // ダメージ判定
    //private void OnTriggerEnter(Collider other)
    //{
    //    Damager damager = other.GetComponent<Damager>();
    //    if (damager != null)
    //    {
    //        // 攻撃判定
    //        if (mobChase.isAttacking == true)
    //        {
    //            // 敵の攻撃が当たったらダメージアニメーション発生
    //            anim.SetTrigger("Gethit");
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider collision)
    {
        MOBChase mobChase = collision.gameObject.GetComponent<MOBChase>();
        if (mobChase != null)
        {
            if (mobChase.isAttacking == true)
            {
                // 敵の攻撃が当たったらダメージアニメーション発生
                anim.SetTrigger("Gethit");
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

    //マップ拡大の際にマウスカーソルを表示する
    private void mouseCursor()
    {
        if (isPushedM)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            freeLook.m_XAxis.m_MaxSpeed = 900;
            freeLook.m_YAxis.m_MaxSpeed = 6;
            isPushedM = false;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            freeLook.m_XAxis.m_MaxSpeed = 0;
            freeLook.m_YAxis.m_MaxSpeed = 0;
            isPushedM = true;
        }
    }
}