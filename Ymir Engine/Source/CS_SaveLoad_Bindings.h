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

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFilePtr->SetInt(mono_string_to_utf8(saveAs), val);

	ygameFilePtr->UpdateJSON_File((fileDir + "/" + fileName + ".ygame").c_str());
}

void SaveGameFloat(MonoString* dir, MonoString* name, MonoString* saveAs, double val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFilePtr->SetFloat(mono_string_to_utf8(saveAs), val);
}

void SaveGameBool(MonoString* dir, MonoString* name, MonoString* saveAs, bool val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFilePtr->SetBoolean(mono_string_to_utf8(saveAs), val);
}

void SaveGameString(MonoString* dir, MonoString* name, MonoString* saveAs, MonoString* val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFilePtr->SetString(mono_string_to_utf8(saveAs), mono_string_to_utf8(val));
}

void SaveGameIntArray(MonoString* dir, MonoString* name, MonoString* saveAs, int* val, int size)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFilePtr->SetIntArray(mono_string_to_utf8(saveAs), val, size);
}

void SaveGameFloatArray(MonoString* dir, MonoString* name, MonoString* saveAs, float* val, int size)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	ygameFilePtr->SetFloatArray(mono_string_to_utf8(saveAs), val, size);
}
#pragma endregion

#pragma region LoadGame
int LoadGameInt(MonoString* dir, MonoString* name, MonoString* saveAs)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return ygameFilePtr->GetInt(mono_string_to_utf8(saveAs));
}

double LoadGameFloat(MonoString* dir, MonoString* name, MonoString* saveAs)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return ygameFilePtr->GetFloat(mono_string_to_utf8(saveAs));
}

bool LoadGameBool(MonoString* dir, MonoString* name, MonoString* saveAs, bool val)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return ygameFilePtr->GetBoolean(mono_string_to_utf8(saveAs));
}

MonoString* LoadGameString(MonoString* dir, MonoString* name, MonoString* saveAs)
{
	std::string fileDir = mono_string_to_utf8(dir);
	std::string fileName = mono_string_to_utf8(name);

	std::unique_ptr<JsonFile> ygameFilePtr = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
	return mono_string_new(External->moduleMono->domain, (ygameFilePtr->GetString(mono_string_to_utf8(saveAs)).c_str()));
}

//int* LoadGameIntArray(MonoString* dir, MonoString* name, MonoString* saveAs)
//{
//	std::string fileDir = mono_string_to_utf8(dir);
//	std::string fileName = mono_string_to_utf8(name);
//
//	ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
//	return ygameFile->GetIntArray(mono_string_to_utf8(saveAs));
//}
//
//void LoadGameFloatArray(MonoString* dir, MonoString* name, MonoString* saveAs)
//{
//	std::string fileDir = mono_string_to_utf8(dir);
//	std::string fileName = mono_string_to_utf8(name);
//
//	ygameFile = JsonFile::GetJSON(fileDir + "/" + fileName + ".ygame");
//	return ygameFile->GetFloatArray(mono_string_to_utf8(saveAs));
//}
#pragma endregion