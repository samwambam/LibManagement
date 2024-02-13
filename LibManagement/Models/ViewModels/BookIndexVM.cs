namespace LibManagement.Models.ViewModels
{
    public class BookIndexVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int Total { get; set; }
        public int QuantityAvailable { get; set; }
        public List<string> Genres { get; set; } 
    }
}
