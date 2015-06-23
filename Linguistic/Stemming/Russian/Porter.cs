using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ArtTarasov.Linguistic.Stemming.Russian
{
    /// <summary>  
    /// Стеммер Портера
    /// Выделение основы слова
    /// </summary>
    public static class Porter
    {
        static readonly Regex PERFECTIVEGROUND = new Regex("((ив|ивши|ившись|ыв|ывши|ывшись)|((?<=[ая])(в|вши|вшись)))$", RegexOptions.Compiled);
        static readonly Regex REFLEXIVE = new Regex("(с[яь])$", RegexOptions.Compiled);
        static readonly Regex ADJECTIVE = new Regex("(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$", RegexOptions.Compiled);
        static readonly Regex PARTICIPLE = new Regex("((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$", RegexOptions.Compiled);
        static readonly Regex VERB = new Regex("((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$", RegexOptions.Compiled);
        static readonly Regex NOUN = new Regex("(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$", RegexOptions.Compiled);
        static readonly Regex RVRE = new Regex("^(.*?[аеиоуыэюя])(.*)$", RegexOptions.Compiled);
        static readonly Regex DERIVATIONAL = new Regex(".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$", RegexOptions.Compiled);
        static readonly Regex DER = new Regex("ость?$", RegexOptions.Compiled);
        static readonly Regex SUPERLATIVE = new Regex("(ейше|ейш)$", RegexOptions.Compiled);
        static readonly Regex I = new Regex("и$", RegexOptions.Compiled);
        static readonly Regex P = new Regex("ь$", RegexOptions.Compiled);
        static readonly Regex NN = new Regex("нн$", RegexOptions.Compiled);

        /// <summary>  
        /// Получение основы слова по алгоритму Портера
        /// </summary>  
        /// <param name="word">Исходное слово</param>  
        /// <returns>Основа слова</returns>  
        public static String Stemm(String word)
        {
            word = word.ToLower();
            word = word.Replace('ё', 'е');
            var m = RVRE.Match(word);
            if (RVRE.IsMatch(word))
            {
                String pre = m.Groups[1].Value;
                String rv = m.Groups[2].Value;
                String temp = PERFECTIVEGROUND.Replace(rv, "");
                if (temp == rv)
                {
                    rv = REFLEXIVE.Replace(rv, "");
                    temp = ADJECTIVE.Replace(rv, "");
                    if (temp != rv)
                    {
                        rv = temp;
                        rv = PARTICIPLE.Replace(rv, "");
                    }
                    else
                    {
                        temp = VERB.Replace(rv, "");
                        if (temp == rv)
                            rv = NOUN.Replace(rv, "");
                        else
                            rv = temp;
                    }

                }
                else
                    rv = temp;

                rv = I.Replace(rv, "");

                if (DERIVATIONAL.IsMatch(rv))
                    rv = DER.Replace(rv, "");

                temp = P.Replace(rv, "");
                if (temp == rv)
                {
                    rv = SUPERLATIVE.Replace(rv, "");
                    rv = NN.Replace(rv, "н");
                }
                else
                    rv = temp;
                word = pre + rv;
            }

            return word;
        }
    }     
}
