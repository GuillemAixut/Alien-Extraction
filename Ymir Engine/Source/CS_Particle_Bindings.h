#pragma once
#include "Globals.h"
#include "Application.h"
#include "GameObject.h"
#include "CAnimation.h"
#include "Component.h"


void PlayEmitter(MonoObject* go, MonoObject* id) {

	if (External == nullptr)
		return;

	GameObject* GO = External->moduleMono->GameObject_From_CSGO(go);


}

//TODO TONY / TODO TONI: Esto me esta llorando de todo mal, no se que le pasa asi que te lo dejo