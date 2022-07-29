using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Assets.Scripts.Managers;
using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Controllers;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardController _cardController;
    private Camera _mainCamera;
    private Vector3 _offset;
    public Transform _defaultParent;
    public Transform _defaultTempCardParant;
    private GameObject _tempCardGo;

    public bool isDraggble;

    private int _startID;

    private void Awake()
    {
        _mainCamera = Camera.allCameras[0];
        _tempCardGo = GameObject.Find("TempCardGo");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(eventData.position);
        _defaultParent = _defaultTempCardParant = transform.parent;

        isDraggble = GameManager.instance.IsPlayerTurn &&
            ((_defaultParent.GetComponent<DropPlaceScript>().type == FieldType.SELF_HAND &&
            GameManager.instance._currentGame._player._mana >= _cardController._card.Manacost) ||
            (_defaultParent.GetComponent<DropPlaceScript>().type == FieldType.SELF_FIELD &&
            _cardController._card.CanAttack)
            );
            
        if (!isDraggble)
            return;

        _startID = transform.GetSiblingIndex();

        if(_cardController._card.isSpell || _cardController._card.CanAttack)
            GameManager.instance.HightLightTargets(_cardController,true);

        _tempCardGo.transform.SetParent(_defaultParent);
        _tempCardGo.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(_defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggble)
            return;

        Vector3 newPos = _mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + _offset;

        if(!_cardController._card.isSpell)
        {
            if (_tempCardGo.transform.parent != _defaultTempCardParant)
                _tempCardGo.transform.SetParent(_defaultTempCardParant);

            if (_defaultParent.GetComponent<DropPlaceScript>().type != FieldType.SELF_FIELD)
                CheckPosition();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggble)
            return;

        GameManager.instance.HightLightTargets(_cardController,false); 

        transform.SetParent(_defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(_tempCardGo.transform.GetSiblingIndex());
        _tempCardGo.transform.SetParent(GameObject.Find("GameCanvas").transform);
        _tempCardGo.transform.localPosition = new Vector3(1200, 0);
    }

    private void CheckPosition()
    {
        int newIndex = _defaultTempCardParant.childCount;

        for (int i = 0; i < _defaultTempCardParant.childCount; i++)
        {
            if(transform.position.x < _defaultTempCardParant.GetChild(i).position.x)
            {
                newIndex = 1;

                if (_tempCardGo.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }
        }

        if (_tempCardGo.transform.parent == _defaultParent)
            newIndex = _startID;

        _tempCardGo.transform.SetSiblingIndex(newIndex);      
    }

    public void MoveToField(Transform field)
    {
       transform.SetParent(GameObject.Find("GameCanvas").transform);
        transform.DOMove(field.position, 0.5f);
    }

    public void MoveToTarget(Transform target)
    {
        StartCoroutine(MoveToTargetCor(target));
    }


    public void MoveToHand(Transform field)
    {
        transform.SetParent(GameObject.Find("GameCanvas").transform);
        transform.DOMove(field.position, 0.7f).OnComplete(() => transform.SetParent(field)); 
    }

    private IEnumerator MoveToTargetCor(Transform target)
    {
        Vector3 pos = transform.position;
        Transform parent = transform.parent;
        int index = transform.GetSiblingIndex();

        if(transform.parent.GetComponent<HorizontalLayoutGroup>())
             transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;

        transform.SetParent(GameObject.Find("GameCanvas").transform);

        transform.DOMove(target.position, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOMove(pos, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.SetParent(parent);
        transform.SetSiblingIndex(index);

        if (transform.parent.GetComponent<HorizontalLayoutGroup>())
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }

}
