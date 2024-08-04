namespace MomesCare.Api.ApiClient.Entitis
{
    public class BaseResponse
    {
        public object Result { get; set; }
        public List<string> ErrorsMessage { get; set; }
        public bool IsSuccess { get => ErrorsMessage == null || ErrorsMessage.Count == 0; set { } }

        public BaseResponse()
        {
            ErrorsMessage = new List<string>();
            IsSuccess = ErrorsMessage.Count == 0;
        }
    }
}
