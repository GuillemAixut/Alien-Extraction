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

#include "Component.h"

//
int GetUIState(MonoObject* object, int uiState)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	return (int)((C_UI*)(go->GetComponent(ComponentType::UI)))->state;
}

void SetUIState(MonoObject* object, int uiState)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	std::vector<Component*> vec = go->GetAllComponentsByType(ComponentType::UI);

	for (auto it = vec.begin(); it != vec.end(); ++it)
	{
		((C_UI*)(*it))->SetState((UI_STATE)uiState);

		if (((C_UI*)(*it))->tabNav_ && (UI_STATE)uiState == UI_STATE::FOCUSED)
		{
			int offset = 0;
			std::vector<C_UI*> listOffset;
			for (int i = 0; i < External->scene->vCanvas.size(); ++i)
			{
				External->scene->GetUINavigate(External->scene->vCanvas[i], listOffset);
			}

			for (auto i = 0; i < listOffset.size(); i++)
			{
				if (listOffset[i]->GetUID() != (int)(C_UI*)(*it)->GetUID())
				{
					offset++;
				}

				else
				{
					break;
				}
			}

			External->scene->onHoverUI = offset;
			std::vector<Component*> listComponents = External->scene->focusedUIGO->GetAllComponentsByType(ComponentType::UI);

			for (auto it = listComponents.begin(); it != listComponents.end(); ++it)
			{
				if (((C_UI*)(*it))->tabNav_)
				{
					((C_UI*)(*it))->SetState(UI_STATE::NORMAL);
				}
			}

			External->scene->focusedUIGO = ((C_UI*)(*it))->mOwner;
			External->scene->SetSelected(((C_UI*)(*it))->mOwner);

			listComponents = External->scene->focusedUIGO->GetAllComponentsByType(ComponentType::UI);

			for (auto it = listComponents.begin(); it != listComponents.end(); ++it)
			{
				if (((C_UI*)(*it))->tabNav_)
				{
					((C_UI*)(*it))->SetState(UI_STATE::FOCUSED);
				}
			}
		}
	}
}

MonoObject* CreateImageUI(MonoObject* pParent, MonoString* newImage, int x, int y)
{
	GameObject* ui_gameObject = External->moduleMono->GameObject_From_CSGO(pParent);
	std::string _newImage = mono_string_to_utf8(newImage);

	G_UI* tempGameObject = new G_UI(External->scene->mRootNode, 0, 0);

	tempGameObject->AddImage(_newImage, x, y, 100, 100);

	External->scene->PostUpdateCreateGameObject_UI((GameObject*)tempGameObject);

	return External->moduleMono->GoToCSGO(tempGameObject);
}

bool GetCanNav()
{
	return External->scene->canNav;
}

void SetCanNav(bool set)
{
	External->scene->canNav = set;
}

void ChangeImageUI(MonoObject* go, MonoString* newImage, int state)
{
	//Falta meter automaticamente que haga el change de Image
	GameObject* go_image_to_change = External->moduleMono->GameObject_From_CSGO(go);
	std::string _newImage = mono_string_to_utf8(newImage);

	UI_Image* image_to_change = static_cast<UI_Image*>(static_cast<G_UI*>(go_image_to_change)->GetComponentUI(UI_TYPE::IMAGE));
	image_to_change->SetImg(_newImage, (UI_STATE)state);
}

// Image Animations
int GetImageRows(MonoObject* object)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	return static_cast<UI_Image*>(go->GetComponentUI(UI_TYPE::IMAGE))->ssRows;
}

int GetImageColumns(MonoObject* object)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	return static_cast<UI_Image*>(go->GetComponentUI(UI_TYPE::IMAGE))->ssColumns;
}

int GetImageCurrentFrameX(MonoObject* object)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	return static_cast<UI_Image*>(go->GetComponentUI(UI_TYPE::IMAGE))->ssCoordsX;
}

int GetImageCurrentFrameY(MonoObject* object)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	return static_cast<UI_Image*>(go->GetComponentUI(UI_TYPE::IMAGE))->ssCoordsY;
}

void SetImageCurrentFrame(MonoObject* object, int x, int y)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	static_cast<UI_Image*>(go->GetComponentUI(UI_TYPE::IMAGE))->ssCoordsX = x;
	static_cast<UI_Image*>(go->GetComponentUI(UI_TYPE::IMAGE))->ssCoordsY = y;

	static_cast<UI_Image*>(go->GetComponentUI(UI_TYPE::IMAGE))->SetSpriteSize();
}

//
void TextEdit(MonoObject* object, MonoString* text)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	static_cast<UI_Text*>(go->GetComponentUI(UI_TYPE::TEXT))->SetText(mono_string_to_utf8(text));
}

void SliderEdit(MonoObject* object, double value)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);
	static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->SetValue(value);
}

void SliderSetRange(MonoObject* object, double min, double max)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);

	if (static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->useFloat)
	{
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->minValue.fValue = min;
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->maxValue.fValue = max;
	}
	else
	{
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->minValue.iValue = min;
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->maxValue.iValue = max;
	}
}

void SliderSetMin(MonoObject* object, double value)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);

	if (static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->useFloat)
	{
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->minValue.fValue = value;
	}
	else
	{
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->minValue.iValue = value;
	}
}

void SliderSetMax(MonoObject* object, double value)
{
	G_UI* go = (G_UI*)External->moduleMono->GameObject_From_CSGO(object);

	if (static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->useFloat)
	{
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->maxValue.fValue = value;
	}
	else
	{
		static_cast<UI_Slider*>(go->GetComponentUI(UI_TYPE::SLIDER))->maxValue.iValue = value;
	}
}

// Inventory

MonoObject* GetSelected()
{
	if (External->scene->selectedUIGO != nullptr)
	{
		return External->moduleMono->GoToCSGO(External->scene->selectedUIGO);
	}

	return nullptr;
}

MonoObject* GetFocused()
{
	if (External->scene->focusedUIGO != nullptr)
	{
		return External->moduleMono->GoToCSGO(External->scene->focusedUIGO);
	}

	return nullptr;
}

void SwitchPosition(MonoObject* selectedObject, MonoObject* targetObject)
{
	G_UI* selectedgo = (G_UI*)External->moduleMono->GameObject_From_CSGO(selectedObject);
	UI_Transform* selectedTransform = static_cast<UI_Transform*>(selectedgo->GetComponent(ComponentType::UI_TRAMSFORM));

	G_UI* targetgo = (G_UI*)External->moduleMono->GameObject_From_CSGO(targetObject);
	UI_Transform* targetTransform = static_cast<UI_Transform*>(targetgo->GetComponent(ComponentType::UI_TRAMSFORM));

	float auxPosX = targetTransform->componentReference->posX;
	float auxPosY = targetTransform->componentReference->posY;
	//float auxWidth = targetTransform->componentReference->width;
	//float auxHeight = targetTransform->componentReference->height;

	targetTransform->componentReference->posX = selectedTransform->componentReference->posX;
	targetTransform->componentReference->posY = selectedTransform->componentReference->posY;

	selectedTransform->componentReference->posX = auxPosX;
	selectedTransform->componentReference->posY = auxPosY;

	//targetTransform->componentReference->width = selectedTransform->componentReference->width;
	//targetTransform->componentReference->height = selectedTransform->componentReference->height;

	//selectedTransform->componentReference->width = auxWidth;
	//selectedTransform->componentReference->height = auxHeight;

	targetTransform->UpdateUITransformChilds();
	targetTransform->componentReference->dirty_ = true;

	selectedTransform->UpdateUITransformChilds();
	selectedTransform->componentReference->dirty_ = true;


	for (int i = 0; i < External->scene->selectedUIGO->mComponents.size(); i++)
	{
		if (External->scene->selectedUIGO->mComponents[i]->ctype == ComponentType::UI)
		{
			if (static_cast<C_UI*>(External->scene->selectedUIGO->mComponents[i])->state == UI_STATE::SELECTED)
			{
				static_cast<C_UI*>(External->scene->selectedUIGO->mComponents[i])->state = UI_STATE::FOCUSED;
			}
		}
	}

	External->scene->swapList.insert(std::pair<GameObject*, GameObject*>(selectedgo, targetgo));
	//selectedgo->SwapChildren(targetgo);

	External->scene->focusedUIGO = External->scene->selectedUIGO;
	External->scene->selectedUIGO = nullptr;

}

void NavigateGridHorizontal(MonoObject* go, int rows, int columns, bool isRight, bool navigateGrids, MonoObject* gridLeft, MonoObject* gridRight)
{
	if (External->scene->canNav)
	{
		// Get UI elements to navigate
		std::vector<C_UI*> listUI;
		GameObject* gameObject = External->moduleMono->GameObject_From_CSGO(go);
		External->scene->GetUINavigate(gameObject, listUI); bool isInGO = false;
		int offset = 0;

		std::vector<C_UI*> listOffset;
		for (int i = 0; i < External->scene->vCanvas.size(); ++i)
		{
			External->scene->GetUINavigate(External->scene->vCanvas[i], listOffset);
		}

		for (auto i = 0; i < listOffset.size(); i++)
		{
			if (listOffset[i]->mOwner->UID != gameObject->mChildren[0]->UID)
			{
				offset++;
			}

			else
			{
				break;
			}
		}

		if (External->scene->focusedUIGO == nullptr)
		{
			External->scene->SetSelected(listUI[0]->mOwner);
			External->scene->focusedUIGO = listUI[0]->mOwner;
			External->scene->onHoverUI = offset;
		}

		else
		{
			for (auto i = 0; i < listUI.size(); i++)
			{
				if (External->scene->focusedUIGO != nullptr)
				{
					if (listUI[i]->mOwner->UID == External->scene->focusedUIGO->UID)
					{
						isInGO = true;
						break;
					}
				}
			}
		}

		if (isInGO)
		{
			if (isRight)
			{
				if (External->scene->onHoverUI + rows >= listUI.size() + offset)
				{
					if (navigateGrids)
					{
						GameObject* gridGo = External->moduleMono->GameObject_From_CSGO(gridRight);

						if (gridGo != nullptr)
						{
							if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
							{
								listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
							}

							SetUIState(External->moduleMono->GoToCSGO(gridGo->mChildren[0]), (int)UI_STATE::FOCUSED);

							External->scene->canNav = false;

						}

						//else
						//{
						//	External->scene->SetSelected(listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->mOwner);

						//	External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->mOwner;

						//	if (listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->state != UI_STATE::SELECTED)
						//	{
						//		listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->SetState(UI_STATE::FOCUSED);
						//	}

						//	External->scene->onHoverUI -= (rows * (columns - 1));
						//}
					}

					else
					{
						if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
						}

						// Same as below, should make a function
						External->scene->SetSelected(listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->mOwner);

						External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->mOwner;

						if (listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset - (rows * (columns - 1))]->SetState(UI_STATE::FOCUSED);
						}

						External->scene->onHoverUI -= (rows * (columns - 1));
					}
				}

				else
				{
					External->scene->SetSelected(listUI[External->scene->onHoverUI - offset + rows]->mOwner);

					External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset + rows]->mOwner;

					if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
					}

					if (listUI[External->scene->onHoverUI - offset + rows]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset + rows]->SetState(UI_STATE::FOCUSED);
					}

					External->scene->onHoverUI += rows;
				}
			}

			else
			{
				if (External->scene->onHoverUI - rows < offset)
				{
					if (navigateGrids)
					{
						GameObject* gridGo = External->moduleMono->GameObject_From_CSGO(gridLeft);

						if (gridGo != nullptr)
						{
							if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
							{
								listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
							}

							SetUIState(External->moduleMono->GoToCSGO(gridGo->mChildren[0]), (int)UI_STATE::FOCUSED);

							External->scene->canNav = false;

						}

						//else
						//{
						//	// Same as below, should make a function
						//	External->scene->SetSelected(listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->mOwner);
						//	External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->mOwner;

						//	if (listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->state != UI_STATE::SELECTED)
						//	{
						//		listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->SetState(UI_STATE::FOCUSED);
						//	}

						//	External->scene->onHoverUI += (rows * (columns - 1));
						//}
					}

					else
					{
						if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
						}

						External->scene->SetSelected(listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->mOwner);
						External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->mOwner;

						if (listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset + (rows * (columns - 1))]->SetState(UI_STATE::FOCUSED);
						}

						External->scene->onHoverUI += (rows * (columns - 1));
					}
				}

				else
				{
					External->scene->SetSelected(listUI[External->scene->onHoverUI - offset - rows]->mOwner);
					External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset - rows]->mOwner;

					if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
					}

					if (listUI[External->scene->onHoverUI - offset - rows]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset - rows]->SetState(UI_STATE::FOCUSED);
					}

					External->scene->onHoverUI -= rows;
				}

			}
		}
	}
}

void NavigateGridVertical(MonoObject* go, int rows, int columns, bool isDown, bool navigateGrids, MonoObject* gridDown, MonoObject* gridUp)
{
	if (External->scene->canNav)
	{
		// Get UI elements to navigate
		std::vector<C_UI*> listUI;
		GameObject* gameObject = External->moduleMono->GameObject_From_CSGO(go);
		External->scene->GetUINavigate(gameObject, listUI); bool isInGO = false;
		int offset = 0;

		std::vector<C_UI*> listOffset;
		for (int i = 0; i < External->scene->vCanvas.size(); ++i)
		{
			External->scene->GetUINavigate(External->scene->vCanvas[i], listOffset);
		}

		for (auto i = 0; i < listOffset.size(); i++)
		{
			if (listOffset[i]->mOwner->UID != gameObject->mChildren[0]->UID)
			{
				offset++;
			}

			else
			{
				break;
			}
		}

		if (External->scene->focusedUIGO == nullptr)
		{
			External->scene->focusedUIGO = listUI[0]->mOwner;
			External->scene->SetSelected(listUI[0]->mOwner);
			External->scene->onHoverUI = offset;
		}

		else
		{
			for (auto i = 0; i < listUI.size(); i++)
			{
				if (External->scene->focusedUIGO != nullptr)
				{
					if (listUI[i]->mOwner->UID == External->scene->focusedUIGO->UID)
					{
						isInGO = true;
						break;
					}
				}
			}
		}

		if (isInGO)
		{
			if (isDown)
			{
				if ((External->scene->onHoverUI - offset + 1) % rows == 0 || External->scene->onHoverUI - offset == listUI.size() - 1)
				{
					if (navigateGrids)
					{
						GameObject* gridGo = External->moduleMono->GameObject_From_CSGO(gridDown);

						if (gridGo != nullptr)
						{
							if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
							{
								listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
							}

							SetUIState(External->moduleMono->GoToCSGO(gridGo->mChildren[0]), (int)UI_STATE::FOCUSED);

							External->scene->canNav = false;

						}

						//else
						//{
						//	External->scene->SetSelected(listUI[External->scene->onHoverUI - offset - rows + 1]->mOwner);

						//	External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset - rows + 1]->mOwner;

						//	if (listUI[External->scene->onHoverUI - offset - rows + 1]->state != UI_STATE::SELECTED)
						//	{
						//		listUI[External->scene->onHoverUI - offset - rows + 1]->SetState(UI_STATE::FOCUSED);
						//	}

						//	External->scene->onHoverUI -= (rows - 1);
						//}
					}

					else
					{
						if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
						}

						// Same as below, should make a function
						External->scene->SetSelected(listUI[External->scene->onHoverUI - offset - rows + 1]->mOwner);

						External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset - rows + 1]->mOwner;

						if (listUI[External->scene->onHoverUI - offset - rows + 1]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset - rows + 1]->SetState(UI_STATE::FOCUSED);
						}

						External->scene->onHoverUI -= (rows - 1);
					}
				}

				else
				{
					External->scene->SetSelected(listUI[External->scene->onHoverUI - offset + 1]->mOwner);

					External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset + 1]->mOwner;

					if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
					}

					if (listUI[External->scene->onHoverUI - offset + 1]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset + 1]->SetState(UI_STATE::FOCUSED);
					}

					External->scene->onHoverUI += 1;
				}
			}

			else
			{
				if ((External->scene->onHoverUI - offset) % rows == 0 || External->scene->onHoverUI - offset == 0)
				{
					if (navigateGrids)
					{
						GameObject* gridGo = External->moduleMono->GameObject_From_CSGO(gridUp);

						if (gridGo != nullptr)
						{
							if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
							{
								listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
							}

							SetUIState(External->moduleMono->GoToCSGO(gridGo->mChildren[0]), (int)UI_STATE::FOCUSED);

							External->scene->canNav = false;

						}

						//else
						//{
						//	// Same as below, should make a function
						//	External->scene->SetSelected(listUI[External->scene->onHoverUI - offset + rows - 1]->mOwner);
						//	External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset + rows - 1]->mOwner;

						//	if (listUI[External->scene->onHoverUI - offset + rows - 1]->state != UI_STATE::SELECTED)
						//	{
						//		listUI[External->scene->onHoverUI - offset + rows - 1]->SetState(UI_STATE::FOCUSED);
						//	}

						//	External->scene->onHoverUI += (rows - 1);
						//}
					}

					else
					{
						if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
						}

						External->scene->SetSelected(listUI[External->scene->onHoverUI - offset + rows - 1]->mOwner);
						External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset + rows - 1]->mOwner;

						if (listUI[External->scene->onHoverUI - offset + rows - 1]->state != UI_STATE::SELECTED)
						{
							listUI[External->scene->onHoverUI - offset + rows - 1]->SetState(UI_STATE::FOCUSED);
						}

						External->scene->onHoverUI += (rows - 1);
					}
				}

				else
				{
					External->scene->SetSelected(listUI[External->scene->onHoverUI - offset - 1]->mOwner);
					External->scene->focusedUIGO = listUI[External->scene->onHoverUI - offset - 1]->mOwner;

					if (listUI[External->scene->onHoverUI - offset]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset]->SetState(UI_STATE::NORMAL);
					}

					if (listUI[External->scene->onHoverUI - offset - 1]->state != UI_STATE::SELECTED)
					{
						listUI[External->scene->onHoverUI - offset - 1]->SetState(UI_STATE::FOCUSED);
					}

					External->scene->onHoverUI -= 1;
				}
			}
		}
	}
}

//
void SetActiveAllUI(MonoObject* go, bool isActive)
{
	GameObject* gameObject = External->moduleMono->GameObject_From_CSGO(go);

	// Get UI elements to navigate
	std::vector<C_UI*> listUI;
	External->scene->GetUINavigate(gameObject, listUI);

	for (auto i = 0; i < listUI.size(); i++)
	{
		listUI[i]->mOwner->mChildren[0]->active = isActive;
	}
}

void SetFirstFocused(MonoObject* go)
{
	// Get UI elements to navigate
	std::vector<C_UI*> listUI;
	GameObject* gameObject = External->moduleMono->GameObject_From_CSGO(go);
	External->scene->GetUINavigate(gameObject, listUI); bool isInGO = false;
	int offset = 0;

	std::vector<C_UI*> listOffset;
	for (int i = 0; i < External->scene->vCanvas.size(); ++i)
	{
		External->scene->GetUINavigate(External->scene->vCanvas[i], listOffset);
	}

	for (auto i = 0; i < listOffset.size(); i++)
	{
		if (listOffset[i]->mOwner->UID != listUI[0]->mOwner->UID)
		{
			offset++;
		}

		else
		{
			break;
		}
	}

	External->scene->focusedUIGO = listUI[0]->mOwner;
	External->scene->SetSelected(listUI[0]->mOwner);
	External->scene->onHoverUI = offset;
}