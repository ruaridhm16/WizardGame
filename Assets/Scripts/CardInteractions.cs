using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInteractions : MonoBehaviour
{
    public bool isDragging = false;
    private bool isSelected = false;
    private Vector2 destination;
    private Vector3 dragOffset;
    private Vector3 mouseDownPosition;

    [Header("Config")]
    public float dragThreshold = 0.15f;

    void OnMouseDown()
    {
        mouseDownPosition = GetMouseWorldPosition();
    }

    private void FixedUpdate() {
        if (isDragging) {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sortingOrder = DeckManager.Hand.Count;


            int cardIndex = DeckManager.HandCards.IndexOf(this.gameObject);
            if (cardIndex != DeckManager.HandCards.Count - 1 && transform.position.x >= DeckManager.HandCards[cardIndex + 1].transform.position.x) {
                
                GameObject temp = this.gameObject;
                DeckManager.HandCards[cardIndex] = DeckManager.HandCards[cardIndex + 1];
                DeckManager.HandCards[cardIndex + 1] = temp;
                DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();
            }
            else if (cardIndex != 0 && transform.position.x < DeckManager.HandCards[cardIndex - 1].transform.position.x) {
                GameObject temp = this.gameObject;
                DeckManager.HandCards[cardIndex] = DeckManager.HandCards[cardIndex - 1];
                DeckManager.HandCards[cardIndex - 1] = temp;
                DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();
            }
        }
    }

    void OnMouseDrag()
    {
        Vector3 currentMouse = GetMouseWorldPosition();
        float distance = Vector3.Distance(mouseDownPosition, currentMouse);

        if (!isDragging && distance > dragThreshold)
        {
            // Begin dragging

            transform.localScale = new Vector3(transform.localScale.x * 1.2f, transform.localScale.y * 1.2f, transform.localScale.z);

            foreach (GameObject card in DeckManager.HandCards) {
                if (card != this.gameObject && card.GetComponent<CardInteractions>().isSelected) {
                card.GetComponent<CardInteractions>().Deselect();
                card.transform.position -= new Vector3(0, 0.2f, 0);
                }
                else {
                    Deselect();
                }
            }

            destination = transform.position;
            isDragging = true;
            dragOffset = transform.position - mouseDownPosition;
        }

        if (isDragging)
        {
            Vector3 newPos = currentMouse + dragOffset;
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();
            transform.localScale = new Vector3(transform.localScale.x / 1.2f, transform.localScale.y / 1.2f, transform.localScale.z);


            // Finish drag
            isDragging = false;

            // Optionally detect drop targets here
            RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPosition(), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("DropZone"))
            {
                Debug.Log("Dropped on valid target!");
            }
            else
            {
                DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();
            }
        }
        else
        {
            // Mouse didn't move much — treat as a click
            HandleClick();
        }
    }
    
    public void Deselect() {
        isSelected = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = isSelected;
        DeckManager.SelectedCards.Remove(GetComponent<CardView>().card);
        DeckManager.SelectedPhysicalCards.Remove(gameObject);
        print(DeckManager.SelectedCards.Count);
    }

    void HandleClick()
    {
        if (isSelected) {
            transform.position -= new Vector3(0, 0.2f, 0);
            DeckManager.SelectedCards.Remove(GetComponent<CardView>().card);
            DeckManager.SelectedPhysicalCards.Remove(gameObject);
            print(DeckManager.SelectedCards.Count);
        }
        else {
            transform.position += new Vector3(0, 0.2f, 0);
            DeckManager.SelectedCards.Add(GetComponent<CardView>().card);
            DeckManager.SelectedPhysicalCards.Add(gameObject);
            print(DeckManager.SelectedCards.Count);
        }
        isSelected = !isSelected;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = isSelected;

        

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
