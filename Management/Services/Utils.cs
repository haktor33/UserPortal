namespace Management.Services
{
    public class Utils
    {
        public static string GetKafkaConfigValue()
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            var kafkaServerPath = isDevelopment ? "localhost:9092" : "kafka:9092";
            Console.WriteLine("------------------------------------------------kafka--------------------------------");
            Console.WriteLine($"Kafka server path:{kafkaServerPath}");
            return kafkaServerPath;
        }
    }
}
