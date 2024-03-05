#pragma once

#include <vector>
#include <string>

#include "Globals.h"

#include "Model.h"
#include "Component.h"
#include "CTransform.h"
#include "CMesh.h"
#include "CMaterial.h"
#include "CCamera.h"
#include "CAudioListener.h"
#include "CAudioSource.h"
#include "CRigidBody.h"
#include "CCollider.h"
#include "CAnimation.h"

#include "Random.h"

class GameObject {
public:

	GameObject();
	GameObject(std::string name, GameObject* parent);
	virtual ~GameObject();

	void Update();

	void Enable();
	void Disable();

	void SetParent(GameObject* newParent);
	void AddChild(GameObject* child);

	void AddComponent(Component* component);
	bool AddComponent(ComponentType ctype, void* var = NULL);
	Component* GetComponent(ComponentType ctype);
	void RemoveComponent(Component* component);

	void DeleteChild(GameObject* go);
	//Remove from children vector (do not use)
	void RemoveChild(GameObject* go);
	//void DestroyGameObject();

	static GameObject* GetGameObjectFromUID(const std::vector<GameObject*>& gameObjects, const uint& UID);

public:

	std::string name;
	uint UID;

	std::string type = "None";

	GameObject* mParent;
	std::vector<GameObject*> mChildren;
	std::vector<Component*> mComponents;

	CTransform* mTransform; 

	bool active;
	bool selected;

};