using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerMovement : MonoBehaviour
{
    public Transform ItenPoint;
    public Transform ShotPoint; // ȭ���� �߻�� ��ġ
    public GameObject ItemPrefab;
    public GameObject ThrowPrefab;
    public GameObject BowPrefab;
    public GameObject ArrowPrefab; // ȭ�� ������ �߰�
    public float arrowSpeed = 10f; // ȭ�� �ӵ�
    public float arrowCooldown = 0.5f; // ȭ�� �߻� ��ٿ� (�� ����)
    private bool useBowPrefab = false; // BowPrefab ��� ���� ����
    private bool canShootArrow = true; // ȭ�� �߻� ���� ����
    Rigidbody2D rb;
    Animator animator;
    public float moveSpeed;
    private Vector2 lastDirection = new Vector2(1, 0); // �⺻������ �������� �ٶ�

    [SerializeField]
    private Transform shotPointTransform = null;

    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = (x == 0) ? Input.GetAxisRaw("Vertical") : 0.0f;

        // �÷��̾ �ٶ󺸴� ���� ����
        if (x != 0 || y != 0)
        {
            lastDirection = new Vector2(x, y).normalized;
            animator.SetFloat("x", x);
            animator.SetFloat("y", y);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        StartCoroutine(Action());
        StartCoroutine(Shot());
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed, 0);
    }

    IEnumerator Action()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Slash");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("Guard");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Item");
            Instantiate(ItemPrefab, ItenPoint.position, transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            animator.SetTrigger("Damage");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            animator.SetTrigger("Dead");
            this.transform.position = new Vector2(0f, -0.12f);
            for (var i = 0; i < 64; i++)
            {
                yield return null;
            }
            this.transform.position = Vector2.zero;
        }
    }

    IEnumerator Shot()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Throw");
            for (var i = 0; i < 30; i++)
            {
                yield return null;
            }
            Instantiate(ThrowPrefab, ShotPoint.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.C) && canShootArrow)
        {
            // ȭ�� �߻� ������ ���� ����
            animator.SetTrigger("Bow");

            // �߻� ��ٿ� ����
            canShootArrow = false;
            StartCoroutine(ArrowCooldown());

            for (var i = 0; i < 40; i++)
            {
                yield return null;
            }

            // useBowPrefab ������ ���� ó��
            if (useBowPrefab)
            {
                // BowPrefab ���� (������ ����)
                Instantiate(BowPrefab, ShotPoint.position, Quaternion.identity);
            }

            // ȭ�� �߻�
            ShootArrow();
        }
    }

    // ��ٿ� �ڷ�ƾ
    IEnumerator ArrowCooldown()
    {
        yield return new WaitForSeconds(arrowCooldown);
        canShootArrow = true;
    }

    // ȭ�� �߻� �Լ�
    private void ShootArrow()
    {
        if (ArrowPrefab == null)
        {
            Debug.LogError("Arrow Prefab is not assigned!");
            return;
        }

        // ȭ�� ���� ��ġ (ShotPoint ��ġ ���)
        Vector3 shootPosition = ShotPoint.position;

        // ȭ�� ���� ������Ʈ ���� - �θ� ���� ����
        GameObject arrow = Instantiate(ArrowPrefab, shootPosition, Quaternion.identity);

        // ArrowManager ������Ʈ�� �ִ��� Ȯ���ϰ� ���� ����
        ArrowManager arrowManager = arrow.GetComponent<ArrowManager>();
        if (arrowManager != null)
        {
            // ȭ�� ���� ����
            arrowManager.Setup(lastDirection);

            // ����� �α� �߰�
            Debug.Log("ȭ�� �߻�: ��ġ " + shootPosition + ", ���� " + lastDirection);
        }
        else
        {
            Debug.LogWarning("Arrow prefab does not have ArrowManager component!");
        }
    }
}