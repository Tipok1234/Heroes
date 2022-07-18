using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackCard : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        BattleCard card = eventData.pointerDrag.GetComponent<BattleCard>();
        
        //if(card && card.)
    }
}
