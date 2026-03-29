using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text[] cardTypesText;

    private Color[] typeColors =
    {
        Color.rosyBrown, //Character
        Color.gray, //Weapon
        Color.lightBlue // Room
    };
    private string cardName;
    void Start()
    {
        UpdateCardDisplay();   
    }

    string AddSoftHyphens(string input, int interval = 5)
    {
        var words = input.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > interval)
            {
                for (int j = interval; j < words[i].Length; j += interval)
                {
                    words[i] = words[i].Insert(j, "\u00AD");
                    j++; // adjust for inserted char
                }
            }
        }
        return string.Join(" ", words);
    }
    private void UpdateCardDisplay()
    {
        cardImage.color = typeColors[(int)cardData.cardType];

        nameText.text = cardName = AddSoftHyphens(cardData.cardName); ;

        for (int i = 0; i < cardTypesText.Length; i++)
        {
            int t = (int)cardData.cardType;
            if (i == t)
            {
                cardTypesText[i].gameObject.SetActive(true);
            }
        }
    }

}
