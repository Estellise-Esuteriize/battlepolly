using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour {

    public Text currentTrash;

    public Image imageDetails;
    public Text titleDetails;
    public Text descriptionDetails;
    public Text priceDetails;

    public void SetTrash(int trash) {
        currentTrash.text = trash.ToString();
    }

    public void ShowDetails(StringSets set, Sprite img) {

        imageDetails.sprite = img;
        titleDetails.text = set.title;
        descriptionDetails.text = set.description;
        priceDetails.text = set.price.ToString();

    }

	
}
