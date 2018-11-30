using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter3Console
{
    class MusicTrack
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public int Length { get; set; }

        public MusicTrack( string artist, string title, string published, int length )
        {
            Artist = artist;
            Title = title;
            Published = DateTime.Parse(published);
            Length = length;
        }

        public override string ToString()
        {
            return $"Artist: {Artist} Title: {Title} Published: {Published} Length: {Length}"; 
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MusicTrack mt = new MusicTrack("Rob Miles", "My Way", "01-01-1942", 150);

            Console.WriteLine(mt);

            Console.ReadKey();

            string mtJason = JsonConvert.SerializeObject(mt);

            Console.WriteLine(mtJason);
            Console.ReadKey();

            MusicTrack musicTrack = JsonConvert.DeserializeObject<MusicTrack>(mtJason);

            Console.WriteLine(musicTrack);

            Console.ReadKey(false);

        }
    }
}
