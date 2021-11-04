#pragma once
#include "DxLib.h"
#include <math.h>

typedef struct 
{
	double x;
	double y;
	double z;
	double w;
}QUATERNION;

class Quaternion
{
public:
	// 回転無しを表す
	QUATERNION Identity()
	{
		QUATERNION q;
		q.x = 0.0;
		q.y = 0.0;
		q.z = 0.0;
		q.w = 1.0;
		return q;
	};

	// 回転クォータニオン
	// 軸の Vec3 axis で double radian 回転する
	QUATERNION RotationQuaternion(double radian, VECTOR axis)
	{
		QUATERNION q;		// クォータニオン
		double normalize;	// VECTOR axis を正規化するために使用
		double s, c;		// sin cos 

		q.x = q.y = q.z = q.w = 0.0;

		// x^2 + y^2 + z^2 + w^2
		normalize = axis.x * axis.x + axis.y * axis.y + axis.z * axis.z;

		// 0以下でq(Identity値、たぶん無回転)を返す

		if (normalize <= 0.0)	return Identity();

		// 平方根と正規化の処理
		normalize = 1.0 / sqrt(normalize);
		axis.x *= normalize;
		axis.y *= normalize;
		axis.z *= normalize;

		// sin(theta/2)やcos(theta/2)を計算
		s = sin(0.5 * radian);
		c = cos(0.5 * radian);

		q.x = s * axis.x;
		q.y = s * axis.y;
		q.z = s * axis.z;
		q.w = c;

		return q;
	};

	// クォータニオンから回転行列変換を使用
	MATRIX QuaternionToMatrix(QUATERNION q)
	{
		MATRIX mat = MGetIdent();

		// X軸
		mat.m[0][0] = 1.0f - 2.0f * q.y * q.y - 2.0f * q.z * q.z;
		mat.m[0][1] = 2.0f * q.x * q.y + 2.0f * q.w * q.z;
		mat.m[0][2] = 2.0f * q.x * q.z - 2.0f * q.w * q.y;

		// Y軸
		mat.m[1][0] = 2.0f * q.x * q.y - 2.0f * q.w * q.z;
		mat.m[1][1] = 1.0f - 2.0f * q.x * q.x - 2.0f * q.z * q.z;
		mat.m[1][2] = 2.0f * q.y * q.z + 2.0f * q.w * q.x;

		// Z軸
		mat.m[2][0] = 2.0f * q.x * q.z + 2.0f * q.w * q.y;
		mat.m[2][1] = 2.0f * q.y * q.z - 2.0f * q.w * q.x;
		mat.m[2][2] = 1.0f - 2.0f * q.x * q.x - 2.0f * q.y * q.y;

		return mat;
	};

	// クォータニオン同士の内積
	float Dot(QUATERNION q1, QUATERNION q2)
	{
		return q1.x* q2.x + q1.y * q2.y + q1.z * q2.z + q1.w * q2.w;
	};

	//float Angle(QUATERNION q1, QUATERNION q2);


private:
	const float epsilon = 0.000001f;	//	イプシロン(ε)極小の値
};

// クォータニオン＊クォータニオン、wikiならハミルトン積と同義だと思う。(たぶん)
// lhs * rhs != rhs * lhs
QUATERNION operator*(QUATERNION lhs, QUATERNION rhs)
{
	QUATERNION q;
	q.x = lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y;
	q.y = lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z;
	q.z = lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x;
	q.w = lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z;
	return q;
};

// クォータニオンとVECTORを使ってVECTORを返す
VECTOR operator*(QUATERNION rotation, VECTOR point)
{
	float x = rotation.x * 2.0f;
	float y = rotation.y * 2.0f;
	float z = rotation.z * 2.0f;
	float xx = rotation.x * x;
	float yy = rotation.y * y;
	float zz = rotation.z * z;
	float xy = rotation.x * y;
	float xz = rotation.x * z;
	float yz = rotation.y * z;
	float wx = rotation.w * x;
	float wy = rotation.w * y;
	float wz = rotation.w * z;

	VECTOR v;
	v.x = (1.0f - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
	v.y = (xy + wz) * point.x + (1.0f - (xx + zz)) * point.y + (yz - wx) * point.z;
	v.z = (xz - wy) * point.x + (yz + wx) * point.y + (1.0f - (xx + yy)) * point.z;
	return v;
};



