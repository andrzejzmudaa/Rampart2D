using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBallScript : MonoBehaviour
{
	[Tooltip("Position we want to hit")]
	public Vector3 targetPos;

	[Tooltip("Horizontal speed, in units/sec")]
	public float speed = 10;

	[Tooltip("How high the arc should be, in units")]
	public float arcHeight = 1;

	[Tooltip("Horizontal speed, in units/sec")]
	public float minSpeed = 10f;

	bool allowMomvement;
	Vector3 startPos;
	Vector3 nextPos;
	float shootAngle;
	float tempspeed;
	float x0;
	float x1;
	float dist;
	void Start()
	{
		// Cache our start position, which is really the only thing we need
		// (in addition to our current position, and the target).
		startPos = transform.position;
		tempspeed = Mathf.Abs(Mathf.Sin(shootAngle * Mathf.Deg2Rad) * speed);
		x0 = startPos.x;
		x1 = targetPos.x;
		dist = x1 - x0;

	}
	void Update()
	{
		if (Mathf.Abs(dist) > 0.5f)
			arcMovement();
		else
			straightMovement();
	}
	void Arrived()
	{
		Destroy(this.gameObject);
	}

	/// 
	/// This is a 2D version of Quaternion.LookAt; it returns a quaternion
	/// that makes the local +X axis point in the given forward direction.
	/// 
	/// forward direction
	/// Quaternion that rotates +X to align with forward
	static Quaternion LookAt2D(Vector2 forward)
	{
		return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
	}

	public void setTargetPos(Vector3 _targetPos, float _shootAngle)
	{
		targetPos = _targetPos;
		allowMomvement = true;
		shootAngle = _shootAngle;
	}

	bool comparePosVector(Vector3 _vector1, Vector3 _vector2)
	{
		return Vector2.SqrMagnitude(new Vector2(_vector1.x, _vector1.y) - new Vector2(_vector2.x, _vector2.y)) < 0.0001;

	}
	void arcMovement()
	{
		if (!allowMomvement)
			return;
		// Compute the next position, with arc added in

		float nextX = Mathf.MoveTowards(transform.position.x, x1, tempspeed * Time.deltaTime);
		float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
		float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
		nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

		// Rotate to face the next position, and then move there
		transform.rotation = LookAt2D(nextPos - transform.position);
		transform.position = nextPos;

		// Do something when we reach the target
		if (comparePosVector(transform.position, targetPos))
			Arrived();
		Debug.Log("Arc movement " + dist);
	}



	void straightMovement()
	{
		Destroy(this.gameObject);

	}
}

