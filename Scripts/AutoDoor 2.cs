using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor2 : MonoBehaviour
{
    private Animator animator;
    private Vector3 doorPosition;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //doorPosition = GameObject.Find("Cube 1").transform.position;
        animator = GetComponent<Animator>();
        //�h�A�̍��W��ۑ�
        doorPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // �ړ��̃A�j���[�V����
        animator.SetFloat("Distance", (doorPosition - target.position).magnitude);
    }
}
