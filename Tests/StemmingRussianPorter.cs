using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtTarasov.Linguistic.Stemming.Russian;
using System.Linq;

namespace Tests
{
    /// <summary>
    /// Tests for class ArtTarasov.Linguistic.Stemming.Russian.Porter
    /// </summary>
    [TestClass]
    public class StemmingRussianPorter
    {
        /// <summary>
        /// Simple test
        /// </summary>
        [TestMethod]
        public void SingleWordTest()
        {
            string[][] wordsPair = { 
                                       new string[] { "Вышагивающая", "вышагива" }, 
                                       new string[] { "авенантненькая", "авенантненьк" },
                                       new string[] { "агамемнон", "агамемнон" },
                                       new string[] { "ободрившийся", "ободр" },
                                       new string[] { "следователей", "следовател" },
                                   };
            foreach(var pair in wordsPair)
                Assert.AreEqual(pair[1], Porter.Stem(pair[0]));
        }

        /// <summary>
        /// Porter test by dictionary with 49673 words
        /// Dictionary was taken from http://snowball.tartarus.org/algorithms/russian/diffs.txt
        /// </summary>
        [TestMethod]
        public void DictionaryTest()
        {
            var dict = Resource.StemmingRussianPorterDictionary
                .Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                    {
                        var split = s.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        return new { Word = split[0], Stem = split[1] };
                    });
            foreach (var pair in dict)
                Assert.AreEqual(pair.Stem, Porter.Stem(pair.Word));
        }
    }
}
