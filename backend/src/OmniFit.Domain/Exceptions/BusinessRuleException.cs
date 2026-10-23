namespace OmniFit.Domain.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public string PropertyName { get; set; }

        public BusinessRuleException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}