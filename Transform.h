#pragma once
#include "Quaternion.h"
#include "Vector.h"

class Transform
{
public:
	VECTOR Forward(QUATERNION q, VECTOR v)
	{
		VECTOR vv;
		vv = q * v;
		return vv;
	};

private:

};