namespace Consumer
{
    public class ConsumerOptions
    {
        public const string SectionName = "Consumer";


        public string Hosts { get; set; }

        public string ConsumerGroupId { get; set; }

        public string ConsumerId { get; set; }
    }
}
