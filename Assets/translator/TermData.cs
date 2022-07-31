// TermData.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Obtains the ( audio / picture / translation ) that corresponds to a given term
public static class TermData
{

    public class Terms
    {

        // < language, index of a non-English language's term within a array-value of termTranslations >
        // English is paired to -1 to convey that it is the key and not within termTranslations's array-value
        public Dictionary<string, int> languageIndicies;

        // < englishText, [frenchText, italianText, ...] >
        // Example: < "Nope.", ["Nan.", "No.", "Nee."..] >
        public Dictionary<string, string[]> termTranslations;

        public Terms()
        {
            languageIndicies = new Dictionary<string, int>();
            termTranslations = new Dictionary<string, string[]>();
        }
    }
}