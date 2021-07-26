using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBallScript : MonoBehaviour
{
	Vector3 targetPos;
	float speed;
	bool allowMomvement;
	Vector3 startPos;
	cannon_prefab_script parentCannonRef;
	float maxTime;
	float t;
	Game_Phase_Controller_Script parentControllerScript;
	void Start()
	{
		// Cache our start position, which is really the only thing we need
		// (in addition to our current position, and the target).
		//parentCannon = (cannon_prefab_script)
		
		parentControllerScript = this.gameObject.transform.parent.parent.GetComponent<Game_Phase_Controller_Script>();
		//parentCannon.shootedBallStillExist = true;




	}
	void Update()
	{
		straightMovement();
	}
	void Arrived()
	{
		parentCannonRef.shootedBallStillExist = false;
		Destroy(this.gameObject);
		parentControllerScript.hitPlayerMapField(targetPos , this.gameObject.transform.parent.GetComponent<PlayerManager>());
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

	public void setTargetPos(Vector3 _targetPos)
	{
		targetPos = _targetPos;
		startPos = transform.position;
		speed = 10f;
		maxTime = calculateTime(targetPos, startPos);
		allowMomvement = true;
	}

	public void setTargetParentCannonRef(cannon_prefab_script _parentCannonRef)
	{
		parentCannonRef = _parentCannonRef;
		parentCannonRef.shootedBallStillExist = true;
	}

	bool comparePosVector(Vector3 _vector1, Vector3 _vector2)
	{
		return Vector2.SqrMagnitude(new Vector2(_vector1.x, _vector1.y) - new Vector2(_vector2.x, _vector2.y)) < 0.01;

	}
	float calculateTime(Vector3 _vector1, Vector3 _vector2)
    {
		float distance = Vector2.SqrMagnitude(new Vector2(_vector1.x, _vector1.y) - new Vector2(_vector2.x, _vector2.y));
		float time = (Mathf.Sqrt(distance) / speed);
        if (time < 0.55f)
        {
            time = 0.6f;
        }
        return time;
		
    }
	

	void straightMovement()
	{
		if (!allowMomvement)
			return;
		// Compute the next position, with arc added in
		float height = 8f;
		t += Time.deltaTime ;
		transform.position = MathParabola.Parabola(startPos, targetPos, height, t / maxTime);
		if (comparePosVector(transform.position, targetPos) && t >= 0.5 || t> maxTime)
			Arrived();
	}

	public class MathParabola
{

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

}
}

