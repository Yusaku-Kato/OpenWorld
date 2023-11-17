using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class TestSheep : MonoBehaviour
{
    private Animator animator;              //�A�j���[�^�[���g��
    private NavMeshAgent agent;             //NavMeshAgent���g��
    public Transform target;                //�^�[�Q�b�g�ɐݒ�ł���悤�ɂ���
    private Rigidbody rb;

    Vector3 newPosition;                    //this�̈ʒu
    Vector3 evadeDestination;               //this�̖ړI���W
    int hitPoint = 10;                      //this��HP

    // �Q�Ƃ������I�u�W�F�N�g���C���X�y�N�^�[����A�T�C������
    public GameObject targetObject;
    // �Q�Ƃ������X�N���v�g������ϐ�
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    //�A�j���[�^�[�����擾
        agent = GetComponent<NavMeshAgent>();   //NavMeshAgent�����擾
        //agent.destination = target.position;    //�G(agent)���v���[���[(target)�Ɍ�����
        rb = GetComponent<Rigidbody>();
        newPosition = this.transform.position;  //this�̈ʒu�擾
        evadeDestination = this.transform.position;

        // �Q�Ƃ������X�N���v�g���擾����
        playerScript = targetObject.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hitPoint <= 0)
        {
            animator.SetTrigger("Getdeath");
            GameObject.Destroy(this);
        }

        Vector3 prePosition = newPosition;      //1��O�̈ʒu��ۑ�
        newPosition = this.transform.position;  //���݂̈ʒu

        Vector3 positionDiff = target.position - newPosition;
        //if (positionDiff.magnitude <= 10)
        //{
        //    evadeDestination.x = this.transform.position.x + (this.transform.position.x - target.position.x);
        //    evadeDestination.z = this.transform.position.z + (this.transform.position.z - target.position.z);
        //    evadeDestination.y = this.transform.position.y;
        //    agent.destination = evadeDestination;    //�v���C���[���瓦����
        //}
        //else
        //{
        //    evadeDestination.x = this.transform.position.x + Random.Range(-5.0f, 5.0f);
        //    evadeDestination.z = this.transform.position.z + Random.Range(-5.0f, 5.0f);
        //    evadeDestination.y = this.transform.position.y + Random.Range(-5.0f, 5.0f);
        //    agent.destination = evadeDestination;   //���i�̓����_���ňړ�
        //}

        // �ړ��̃A�j���[�V����
        //animator.SetFloat("MoveSpeed", (evadeDestination - this.transform.position).magnitude);
        animator.SetFloat("MoveSpeed", (newPosition - prePosition).magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            // �U������
            if (playerScript.isAttacking == true)
            {
                //�v���C���[�̌�������������_���[�W�A�j���[�V��������
                animator.SetTrigger("Gethit");       
                hitPoint = hitPoint - damager.damage;
            }
        }
    }
}