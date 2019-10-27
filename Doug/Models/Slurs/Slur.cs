namespace Doug.Models.Slurs
{
    public class Slur
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }

        public Slur(string text, string createdBy)
        {
            Text = text;
            CreatedBy = createdBy;
            Active = true;
        }
    }
}
