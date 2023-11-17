using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNav : MonoBehaviour
{
    private Animator animator;              //アニメーターを使う
    private NavMeshAgent agent;
    private GameObject navDestinationPos = null;
    private bool isPushed = false;
    private GameObject navON;

    // Mボタンが押されたか
    private bool isPushedM = false;
    // ミニマップ拡大時に視点移動をさせないようにするためにFreeLookCameraオブジェクトを呼び出す
    public GameObject freeLookCamera;
    CinemachineFreeLook freeLook;

    // アニメーションに使う移動速度計算用
    private Vector3 prePosition;
    private Vector3 postPosition;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navON = GameObject.Find("NavON");
        navON.gameObject.SetActive(false);

        //agent.destination = this.transform.position;
        agent.ResetPath();

        freeLookCamera = GameObject.Find("FreeLook Camera");
        freeLook = freeLookCamera.GetComponent<CinemachineFreeLook>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (isPushed)
            {
                agent.ResetPath();
                GetComponent<PlayerScript>().enabled = true;
                navON.gameObject.SetActive(false);
                isPushed = false;
            }
            else
            {
                GetComponent<PlayerScript>().enabled = false;
                navON.gameObject.SetActive(true);

                // マウスカーソルを非表示にし、位置を固定
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                navDestinationPos = null;
                navDestinationPos = GameObject.Find("NavDestination(Clone)");
                //ナビゲーション開始
                if (navDestinationPos != null)
                {
                    agent.destination = navDestinationPos.transform.position;
                }
                else
                {
                    agent.destination = this.transform.position;
                }
                isPushed = true;
            }
        }

        //ナビゲーションONの時はPlayerScriptがdisableになるので必要な機能のみONにする
        if (isPushed)
        {
            //Mボタンでマウスカーソル表示
            if (Input.GetKeyDown(KeyCode.M))
            {
                mouseCursor();
            }

            navDestinationPos = null;
            navDestinationPos = GameObject.Find("NavDestination(Clone)");
            //ナビゲーション開始
            if (navDestinationPos != null)
            {
                agent.destination = navDestinationPos.transform.position;
            }
            else
            {
                //agent.destination = this.transform.position;
                agent.ResetPath();
            }

            //アニメーション
            prePosition = postPosition;
            postPosition = this.transform.position;
            animator.SetFloat("MoveSpeed", (postPosition - prePosition).magnitude * 1000); //1000は何となくこれくらいという数字
            //animator.SetFloat("MoveSpeed", 10);
        }
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
