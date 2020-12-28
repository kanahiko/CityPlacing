using UnityEngine;
using System.Collections.Generic;

public class FizzBuzz : MonoBehaviour
{
    List<NumberWord> dictionary;
    void Start()
    {
        dictionary = new List<NumberWord>();
        dictionary.Add(new NumberWord(3,"Fizz"));
        dictionary.Add(new NumberWord(5, "Buzz"));
        int startNumber = 1;
        int endNumber = 100;

        for (int i = startNumber; i <= endNumber; i++)
        {
            string result = "";
            for(int j = 0; j < dictionary.Count; j++)
            {
                if (i % dictionary[j].number == 0)
                {
                    result += dictionary[j].word;
                }
            }

            if (result == "")
            {
                result = i.ToString();
            }
            Debug.Log(result);
        }
    }
}
public struct NumberWord
{
    public int number;
    public string word;

    public NumberWord(int number, string word)
    {
        this.number = number;
        this.word = word;
    }
}
