using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 10f;
	public GameObject slash;
	public float attackTime;
	public GameObject hitBox;

	private Vector2 inputVector;
	private Rigidbody2D rb2D;
	private Animator animator;
	private bool isAttacking = false;
	private float slashTimer = 0;

	// Use this for initialization
	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		slash.SetActive (false);
	}

	// Update is called once per frame
	void Update()
	{

		if (isAttacking) {
			slashTimer -= Time.deltaTime;
			if (slashTimer < 0) {
				isAttacking = false;
				slash.SetActive (false);
			}
		}

		inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (Input.GetButtonDown("Fire1") && !isAttacking )
		{
			animator.SetTrigger("Attack");
			Attack();
		}

		if (!isAttacking) 
		{
			Vector3 moveVector = transform.position + new Vector3(inputVector.x, inputVector.y) * Time.deltaTime * moveSpeed;
			rb2D.MovePosition(moveVector);
			animator.SetFloat("Horizontal", inputVector.x);
			animator.SetFloat("Vertical", inputVector.y);
		}
	}

	private void Attack()
	{
		isAttacking = true;
		slash.SetActive (true);
		slashTimer = attackTime;


		if(Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y)) //horitontal
		{
			if(inputVector.x > 0) //right
			{

			}else //left
			{

			}
		}else //vertical
		{
			if (inputVector.y > 0) //up
			{

			}
			else //down
			{

			}
		}
	}
}
