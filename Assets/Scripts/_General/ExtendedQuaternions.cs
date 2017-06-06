using UnityEngine;

public static class ExtendedQuaternions
{
	public static Quaternion Diff(Quaternion a, Quaternion b)
	{
		Quaternion inv = a;
		inv = Inverse(inv);
		return inv * b;
	}

	public static Quaternion Inverse(Quaternion Quat)
	{
		Quaternion q = Conjugate(Quat);
		q = operationDivide(dot((Quat), (Quat)), q);

		return q;
	}

	public static Quaternion Conjugate(Quaternion Quat)
	{
		Quaternion q;
		q.x = -Quat.x;
		q.y = -Quat.y;
		q.z = -Quat.z;
		q.w = Quat.w;

		return q;
	}

	public static float dot(Quaternion q1, Quaternion q2)
	{
		return q1.x * q2.x + q1.y * q2.y + q1.z * q2.z + q1.w * q2.w;
	}

	public static Quaternion OperationMultiply(Quaternion a, Quaternion q)
	{
		Quaternion qu;
		qu.x = a.w * q.x + a.x * q.w + a.y * q.z - a.z * q.y;
		qu.y = a.w * q.y + a.y * q.w + a.z * q.x - a.x * q.z;
		qu.z = a.w * q.z + a.z * q.w + a.x * q.y - a.y * q.x;
		qu.w = a.w * q.w - a.x * q.x - a.y * q.y - a.z * q.z;
		return qu;
	}

	public static Quaternion operationDivide(float s, Quaternion q)
	{
		return new Quaternion(q.x / s, q.y / s, q.z / s, q.w / s);
	}

	public static Quaternion Normalize(Quaternion q)
	{
		float mag;

		mag = (q.x * q.x) + (q.y * q.y) + (q.z * q.z) + (q.w * q.w);
		q.x = q.x / mag;
		q.y = q.y / mag;
		q.z = q.z / mag;
		q.w = q.w / mag;

		return q;
	}

	public static Quaternion Slerp(Quaternion u, Quaternion v, float f)
	{
		float alpha, beta, theta, sin_t, cos_t;
		int flip;
		Quaternion result;

		flip = 0;

		// Force the input within range.
		f = Mathf.Min(f, 1.0f);
		f = Mathf.Max(f, 0.0f);

		cos_t = u.x * v.x + u.y * v.y + u.z * v.z + u.w * v.w;

		if (cos_t < 0.0f) { cos_t = -cos_t; flip = 1; }

		if ((1.0f - cos_t) < 0.000001f)
		{

			beta = 1.0f - f;
			alpha = f;

		}
		else
		{

			theta = Mathf.Acos(cos_t);
			sin_t = Mathf.Sin(theta);
			beta = Mathf.Sin(theta - f * theta) / sin_t;
			alpha = Mathf.Sin(f * theta) / sin_t;
		}

		if (flip != 0) alpha = -alpha;

		result.x = beta * u.x + alpha * v.x;
		result.y = beta * u.y + alpha * v.y;
		result.z = beta * u.z + alpha * v.z;
		result.w = beta * u.w + alpha * v.w;

		return result;
	}
}
