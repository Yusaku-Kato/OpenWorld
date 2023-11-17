using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNav : MonoBehaviour
{
    private Animator animator;              //�A�j���[�^�[���g��
    private NavMeshAgent agent;
    private GameObject navDestinationPos = null;
    private bool isPushed = false;
    private GameObject navON;

    // M�{�^���������ꂽ��
    private bool isPushedM = false;
    // �~�j�}�b�v�g�厞�Ɏ��_�ړ��������Ȃ��悤�ɂ��邽�߂�FreeLookCamera�I�u�W�F�N�g���Ăяo��
    public GameObject freeLookCamera;
    CinemachineFreeLook freeLook;

    // �A�j���[�V�����Ɏg���ړ����x�v�Z�p
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

                // �}�E�X�J�[�\�����\���ɂ��A�ʒu���Œ�
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                navDestinationPos = null;
                navDestinationPos = GameObject.Find("NavDestination(Clone)");
                //�i�r�Q�[�V�����J�n
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

        //�i�r�Q�[�V����ON�̎���PlayerScript��disable�ɂȂ�̂ŕK�v�ȋ@�\�̂�ON�ɂ���
        if (isPushed)
        {
            //M�{�^���Ń}�E�X�J�[�\���\��
            if (Input.GetKeyDown(KeyCode.M))
            {
                mouseCursor();
            }

            navDestinationPos = null;
            navDestinationPos = GameObject.Find("NavDestination(Clone)");
            //�i�r�Q�[�V�����J�n
            if (navDestinationPos != null)
            {
                agent.destination = navDestinationPos.transform.position;
            }
            else
            {
                //agent.destination = this.transform.position;
                agent.ResetPath();
            }

            //�A�j���[�V����
            prePosition = postPosition;
            postPosition = this.transform.position;
            animator.SetFloat("MoveSpeed", (postPosition - prePosition).magnitude * 1000); //1000�͉��ƂȂ����ꂭ�炢�Ƃ�������
            //animator.SetFloat("MoveSpeed", 10);
        }
    }

    //�}�b�v�g��̍ۂɃ}�E�X�J�[�\����\������
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
