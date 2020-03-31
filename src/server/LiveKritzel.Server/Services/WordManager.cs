using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Services
{
    public class WordManager
    {
        private readonly WordsInMemoryDatabase _database;

        public WordManager(WordsInMemoryDatabase database)   
        {
            _database = database;
        }

        public string ActualWord { get; private set; }

        public IEnumerable<string> GetWordsToChoose(int numberOfWords)
        {
            var random = new Random();
            for (int i = 0; i < numberOfWords; i++)
            {
                var number = random.Next(0, _database.Words.Count - 1);
                yield return _database.Words[number];
            }
        }

        public void SetActualWord(string word)
        {
            ActualWord = word;
        }

    }
}
