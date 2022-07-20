using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Assets.Scripts.Managers;
using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using UnityEngine.UI;

public class CardScripts : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera _mainCamera;
    private Vector3 _offset;
    public Transform _defaultParent, _defaultTempCardParent;
    private GameObject _tempCardGo;
    private GameManager _gameManager;
    private bool isDraggable;
    private void Awake()
    {
        _mainCamera = Camera.allCameras[0];
        _tempCardGo = GameObject.Find("TempCardGo");
        _gameManager = FindObjectOfType<GameManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(eventData.position);

        _defaultTempCardParent = transform.parent;
        _defaultParent = _defaultTempCardParent;

        isDraggable = _gameManager.IsPlayerTurn &&
            (
            (_defaultParent.GetComponent<DropPlaceScript>().Type == FieldType.SELF_HAND && 
            _gameManager.PlayerMana >= GetComponent<BattleCard>().ManaCostPoints) ||
            (_defaultParent.GetComponent<DropPlaceScript>().Type == FieldType.SELF_FIELD &&
            GetComponent<BattleCard>().IsCanAttack)
            );

        if (!isDraggable)
            return;

        if (GetComponent<BattleCard>().IsCanAttack)
            _gameManager.HightLightTarget(true);

        _tempCardGo.transform.SetParent(_defaultParent);
        _tempCardGo.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(_defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

       


        Vector3 newPos = _mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + _offset;

        if (_tempCardGo.transform.parent != _defaultTempCardParent)
            _tempCardGo.transform.SetParent(_defaultTempCardParent);

        if(_defaultParent.GetComponent<DropPlaceScript>().Type != FieldType.SELF_FIELD)
        CheckPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        _gameManager.HightLightTarget(false);

        transform.SetParent(_defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(_tempCardGo.transform.GetSiblingIndex());
        _tempCardGo.transform.SetParent(GameObject.Find("Canvas").transform);
        _tempCardGo.transform.localPosition = new Vector3(1200, 0);
    }

    private void CheckPosition()
    {
        if (!isDraggable)
            return;

        int newIndex = _defaultTempCardParent.childCount;

        for (int i = 0; i < _defaultTempCardParent.childCount; i++)
        {
            if(transform.position.x < _defaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;
                if (_tempCardGo.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                    break;
            }
        }
        _tempCardGo.transform.SetSiblingIndex(newIndex);
    }

    public void MoveToField(Transform field)
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.DOMove(field.position, 0.5f);
    }

    public void MoveToTarger(Transform target)
    {
        StartCoroutine(MoveToTargetCor(target));
    }

    IEnumerator MoveToTargetCor(Transform target)
    {
        Vector3 pos = transform.position;
        Transform parent = transform.parent;

        int index = transform.GetSiblingIndex();

        transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;

        transform.SetParent(GameObject.Find("Canvas").transform);

        transform.DOMove(target.position, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOMove(pos, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.SetParent(parent);
        transform.SetSiblingIndex(index);

        transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }
}
