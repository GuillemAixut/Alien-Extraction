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
}

//Esta funcion se utiliza para el disparo, recibe el vector hacia que el player está mirando y hace que las particulas se muevan hacia esa dirección
void ParticleShoot(MonoObject* go, MonoObject* vector)
{
	if (External == nullptr) return;

	//Vector hacia el que mira el player
	float3 directionShoot = External->moduleMono->UnboxVector(vector);

	//Game object del player
	//Se necesita para sacar el componente particula y por ende su EmitterPosition
	GameObject* GO = External->moduleMono->GameObject_From_CSGO(go);
	if (GO == nullptr)
	{
		LOG("[ERROR] No Particle Game Object found (be sure the name is correct!)");
		return;
	}

	CParticleSystem* particleSystem = dynamic_cast<CParticleSystem*>(GO->GetComponent(ComponentType::PARTICLE));

	if (particleSystem != nullptr)
	{
		EmitterPosition* pos = (EmitterPosition*)particleSystem->allEmitters.at(0)->modules.at(2);

		EmitterBase* base = (EmitterBase*)particleSystem->allEmitters.at(0)->modules.at(0);

		if(base->currentShape == SpawnAreaShape::PAR_CONE)
		{
			float angulo = math::Atan2(-directionShoot.z, directionShoot.x);
			pos->direction1 = { math::Cos(angulo + (5 / 9 * pi)),0.5, -math::Sin(angulo + (5 / 9 * pi)) };
			pos->direction2 = { math::Cos(angulo - (5 / 9 * pi)),-0.5,-math::Sin(angulo - (5 / 9 * pi)) };
		}
		else
		{
			pos->direction1 = directionShoot;
		}
	}
	else
	{
		LOG("[WARNING] Couldn't play the particle effect %s. Component was null pointer");
	}
}