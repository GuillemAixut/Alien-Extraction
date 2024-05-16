using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using YmirEngine;

public class Auto_Aim : YmirComponent
{
	List<GameObject> enemies;
	GameObject target;

	float angle;
	public void Start()
	{
        enemies = new List<GameObject>();
		target = null;
    }

    public void Update()
	{
		target = null;

		SetTarget();

        if (target != null)
		{
			float cross = gameObject.transform.globalPosition.magnitude * target.transform.globalPosition.magnitude;

			angle = (float)Math.Atan2(cross, Vector3.Dot(gameObject.transform.globalPosition, target.transform.globalPosition));
			Debug.Log("Angle: " + angle);		
        }
	}

	public void OnCollisionEnter(GameObject other)
	{
		if (other.Tag == "Enemy")
		{
			if (!enemies.Contains(other)) {  enemies.Add(other); }
		}
	}

	private void SetTarget()
	{
		float shortestDistance = 0f;

		for (int i = 0; i < enemies.Count; i++)
		{
			float distance = Vector3.Distance(gameObject.transform.globalPosition, enemies[i].transform.globalPosition);
			if (distance < shortestDistance || shortestDistance == 0f)
			{
                shortestDistance = distance;
				target = enemies[i];
            }
        }
	}
}