#pragma once
#include "Globals.h"
#include "Application.h"
#include "GameObject.h"
#include "CParticleSystem.h"
#include "Component.h"

void PlayEmitter(MonoObject* go) {

	if (External == nullptr)
		return;

	GameObject* GO = External->moduleMono->GameObject_From_CSGO(go);
	if (GO == nullptr)
	{
		LOG("[ERROR] No Particle Game Object found (be sure the name is correct!)");
		return;
	}

	CParticleSystem* particleSystem = dynamic_cast<CParticleSystem*>(GO->GetComponent(ComponentType::PARTICLE));

	if (particleSystem != nullptr)
	{
		EmitterSpawner* spawner = (EmitterSpawner*)particleSystem->allEmitters.at(0)->modules.at(1);

		spawner->PlayTrigger();
	}
	else
	{
		LOG("[WARNING] Couldn't play the particle effect %s. Component was null pointer");
	}

	mono_free(go);
}