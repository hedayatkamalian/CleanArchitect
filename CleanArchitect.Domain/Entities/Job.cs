namespace CleanArchitect.Domain.Entities
{
    public class Job
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public string Body { get; set; }
        public bool Done { get; set; }
    }
}
