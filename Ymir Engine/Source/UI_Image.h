#pragma once
#ifndef __UI_IMAGE_H__
#define __UI_IMAGE_H__
#pragma once

#include "Globals.h"
#include "C_UI.h"
#include "CMaterial.h"

class UI_Image : public C_UI
{
public:
	// x = 0, y = 0, w = 100, y = 100
	UI_Image(GameObject* g, int x = 0, int y = 0, int w = 100, int h = 100, std::string shaderPath = "Assets/Shaders/UI Shader.glsl", std::string imgPath= "Assets/pato.png");
	~UI_Image();

	void OnInspector();
	void Draw(bool game) override;
	update_status Update(float dt);

	void SetNativeSize();

public:
	CMaterial* mat;
};
#endif // __UI_IMAGE_H__