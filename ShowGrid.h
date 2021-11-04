#pragma once
#include "DxLib.h"

class ShowGrid
{
public:

	// デバッグ用のグリッド線の色を設定
	void SetGridColor(int red, int green, int blue)
	{
		lineColor = GetColor(red, green, blue);
	}

	// デバッグ用のグリッド線を作成
	// useZBuffer : Zバッファを使用するかどうか。使ったほうがいいと思う
	// lineNum	  : 引くラインの数
	// lineArea   : ラインを引く範囲
	void MakeGrid(BOOL useZBuffer, int lineNum, float lineArea)
	{
		VECTOR Pos1;
		VECTOR Pos2;
		unsigned int _lineNum = lineNum;
		float _lineArea = lineArea;

		if (useZBuffer == TRUE)
		{
			// Zバッファを使用する
			SetUseZBufferFlag(TRUE);
		}

		Pos1 = VGet(-_lineArea / 2.0f, 0.0f, -_lineArea / 2.0f);
		Pos2 = VGet(-_lineArea / 2.0f, 0.0f, _lineArea / 2.0f);

		for (int i = 0; i <= _lineNum; i++)
		{
			DrawLine3D(Pos1, Pos2, lineColor);
			Pos1.x += _lineArea / _lineNum;
			Pos2.x += _lineArea / _lineNum;
		}
		Pos1 = VGet(-_lineArea / 2.0f, 0.0f, -_lineArea / 2.0f);
		Pos2 = VGet(_lineArea / 2.0f, 0.0f, -_lineArea / 2.0f);
		for (int i = 0; i <= _lineNum; i++)
		{
			DrawLine3D(Pos1, Pos2, lineColor);
			Pos1.z += _lineArea / _lineNum;
			Pos2.z += _lineArea / _lineNum;
		}

		// Zバッファ使用するのをデフォルトに戻す
		SetUseZBufferFlag(FALSE);
	};

private:
	unsigned int lineColor = GetColor(255, 255, 255);
};

