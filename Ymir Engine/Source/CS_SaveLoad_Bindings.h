#pragma once
#include "Globals.h"
#include "Application.h"
#include "GameObject.h"

#include "G_UI.h"
#include "UI_Image.h"
#include "UI_Text.h"
#include "UI_Button.h"
#include "UI_InputBox.h"
#include "UI_CheckBox.h"
#include "UI_Slider.h"
#include "UI_Transform.h"

#include "JsonFile.h"

#include "Component.h"

#pragma region Save
void CreateSaveGameFile(MonoString* dir, MonoString* name)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	JsonFile ygameFile;
	ygameFile.CreateJSON(fileDir + "/", fileName + ".ygame");

	LOG("Game file created: %s", (fileDir + "/", fileName + ".ygame").c_str());
}

void SaveGameInt(MonoString* dir, MonoString* name, MonoString* saveAs, int val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFile->SetInt(mono_string_to_utf8(saveAs), val);
}

void SaveGameFloat(MonoString* dir, MonoString* name, MonoString* saveAs, double val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFile->SetFloat(mono_string_to_utf8(saveAs), val);
}

void SaveGameBool(MonoString* dir, MonoString* name, MonoString* saveAs, bool val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFile->SetBoolean(mono_string_to_utf8(saveAs), val);
}

void SaveGameString(MonoString* dir, MonoString* name, MonoString* saveAs, MonoString* val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFile->SetString(mono_string_to_utf8(saveAs), mono_string_to_utf8(val));
}

void SaveGameIntArray(MonoString* dir, MonoString* name, MonoString* saveAs, int* val, int size)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFile->SetIntArray(mono_string_to_utf8(saveAs), val, size);
}

void SaveGameFloatArray(MonoString* dir, MonoString* name, MonoString* saveAs, float* val, int size)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFile->SetFloatArray(mono_string_to_utf8(saveAs), val, size);
}
#pragma endregion

#pragma region LoadGame
int LoadGameInt(MonoString* dir, MonoString* name, MonoString* saveAs)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return ygameFile->GetInt(mono_string_to_utf8(saveAs));
}

double LoadGameFloat(MonoString* dir, MonoString* name, MonoString* saveAs)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return ygameFile->GetFloat(mono_string_to_utf8(saveAs));
}

bool LoadGameBool(MonoString* dir, MonoString* name, MonoString* saveAs, bool val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return ygameFile->GetBoolean(mono_string_to_utf8(saveAs));
}

MonoString* LoadGameString(MonoString* dir, MonoString* name, MonoString* saveAs)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return mono_string_new(External->moduleMono->domain, (ygameFile->GetString(mono_string_to_utf8(saveAs)).c_str()));
}

//int* LoadGameIntArray(MonoString* dir, MonoString* name, MonoString* saveAs)
//{
//	std::string fileDir = mono_string_to_utf8(dir);
//	std::string fileName = mono_string_to_utf8(name);
//
//	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
//	return ygameFile->GetIntArray(mono_string_to_utf8(saveAs));
//}
//
//void LoadGameFloatArray(MonoString* dir, MonoString* name, MonoString* saveAs)
//{
//	std::string fileDir = mono_string_to_utf8(dir);
//	std::string fileName = mono_string_to_utf8(name);
//
//	std::unique_ptr<JsonFile> ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
//	return ygameFile->GetFloatArray(mono_string_to_utf8(saveAs));
//}
#pragma endregion