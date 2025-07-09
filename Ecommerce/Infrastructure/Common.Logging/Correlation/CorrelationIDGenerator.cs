namespace Common.Logging.Correlation
{
    public class CorrelationIDGenerator : ICorrelationIDGenerator
    {
        private string _correlationId = Guid.NewGuid().ToString("D");

        public string Get() => _correlationId;


        public void Set(string correlationId)
        {
            throw new NotImplementedException();
        }
    }
}
