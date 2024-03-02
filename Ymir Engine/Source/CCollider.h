#pragma once
#include "Application.h"
#include "Module.h"
#include "ModulePhysics.h"
#include "Primitive.h"
#include "CTransform.h"
#include "PhysBody.h"

#include "Component.h"
#include <vector>

class btCollisionShape;
class btCollisionObject;
class btRigidBody;

enum ColliderType 
{
	BOX,
	SPHERE,
	CYLINDER,
	CONVEX_HULL,
	MESH_COLLIDER
};

enum physicsType {
	DYNAMIC,
	KINEMATIC,
	STATIC
};

class CCollider : public Component
{
public:
	CCollider(GameObject* owner);
	~CCollider();

	void Update();
	void OnInspector();

	btCollisionShape* GetShape();

	//TODO: funcions per canviar transform del collider, isTrigger, hull collider, mesh collider, etc.
	void SetBoxCollider();
	void SetSphereCollider();
	void SetCylinderCollider();
	void SetConvexCollider();
	void SetMeshCollider();

public:

	// Public para el Save/Load
	float3 size;
	float mass;
	btCollisionShape* shape;

private:

	ColliderType collType;


	btCollisionObject* collider;

	PhysBody* physBody;

	CTransform* transform;
	//TODO: crear variables: hull, mesh, sensor, edit collider, transform�?

	physicsType physType;
	bool gravity;
};