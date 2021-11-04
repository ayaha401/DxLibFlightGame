#pragma once
#include "DxLib.h"
#include <math.h>

class Vector
{
public:
	VECTOR Zero() const { return VGet(0.0f, 0.0f, 0.0f); }
	VECTOR One() const { return VGet(1.0f, 1.0f, 1.0f); }

	VECTOR Forward() const { return VGet(0.0f, 0.0f, 1.0f); }
	VECTOR Back() const { return VGet(0.0f, 0.0f, -1.0f); }
	VECTOR Left() const { return VGet(-1.0f, 0.0f, 0.0f); }
	VECTOR Right() const { return VGet(1.0f, 0.0f, 0.0f); }
	VECTOR Up() const { return VGet(0.0f, 1.0f, 0.0f); }
	VECTOR Down() const { return VGet(0.0f, -1.0f, 0.0f); }

private:

};

VECTOR operator*(VECTOR v, float f)
{
	v.x *= f;
	v.y *= f;
	v.z *= f;
	return v;
};

VECTOR operator*(float f, VECTOR v)
{
	v.x *= f;
	v.y *= f;
	v.z *= f;
	return v;
};