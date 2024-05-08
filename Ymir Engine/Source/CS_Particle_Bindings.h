#pragma once
#include "Globals.h"
#include "Application.h"
#include "GameObject.h"
#include "CParticleSystem.h"
#include "Component.h"

//This function Starts the Particle System
void PlayParticles(MonoObject* go) {

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
		particleSystem->Play();
	}
	else
	{
		LOG("[WARNING] Couldn't play the particle effect %s. Component was null pointer");
	}
}

//This function Stops the Particle System
void StopParticles(MonoObject* go) {

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
		particleSystem->Stop();
	}
	else
	{
		LOG("[WARNING] Couldn't play the particle effect %s. Component was null pointer");
	}
}


//This function is needed to Shoot, it needs a GetForward vector of the game object who shoot
//to set the direction of the bullet in that direccion
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
		for (int i = 0; i < particleSystem->allEmitters.size(); i++)
		{
			for (int j = 0; j < particleSystem->allEmitters[i]->modules.size(); j++)
			{
				if (particleSystem->allEmitters.at(i)->modules.at(j)->type == EmitterType::PAR_POSITION)
				{
					EmitterPosition* pos = (EmitterPosition*)particleSystem->allEmitters.at(i)->modules.at(j);
					EmitterBase* base = (EmitterBase*)particleSystem->allEmitters.at(i)->modules.at(0);

					if (base->currentShape == SpawnAreaShape::PAR_CONE)
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
			}
		}
	}
	else
	{
		LOG("[WARNING] Couldn't play the particle effect %s. Component was null pointer");
	}
}

//This function activates a Trigger in a Particle System
void PlayParticlesTrigger(MonoObject* go) {

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
		for (uint i = 0; i < particleSystem->allEmitters.size(); i++)
		{
			EmitterSpawner* spawner = (EmitterSpawner*)particleSystem->allEmitters.at(i)->modules.at(1);
			spawner->PlayTrigger();
		}
	}
	else
	{
		LOG("[WARNING] Couldn't play the particle effect %s. Component was null pointer");
	}
}

//This function stops the particle system and then wakes it up again, in order to restart the particle triggers
void RestartParticles(MonoObject* go) {

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
		particleSystem->Stop();
		particleSystem->Play();
	}
	else
	{
		LOG("[WARNING] Couldn't play the particle effect %s. Component was null pointer");
	}
}
