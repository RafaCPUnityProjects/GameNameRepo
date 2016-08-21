using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 10f;
	public GameObject hitBox;
	private Vector2 inputVector;
	private Rigidbody2D rb2D;
	private Animator animator;
	// Use this for initialization
	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (Input.GetButtonDown("Fire1"))
		{
			animator.SetTrigger("Attack");
			Attack();
		}
		else
		{
			Vector3 moveVector = transform.position + new Vector3(inputVector.x, inputVector.y) * Time.deltaTime * moveSpeed;
			rb2D.MovePosition(moveVector);
			animator.SetFloat("Horizontal", inputVector.x);
			animator.SetFloat("Vertical", inputVector.y);
		}
	}

	private void Attack()
	{
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
