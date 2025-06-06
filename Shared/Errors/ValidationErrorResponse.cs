namespace Shared.Errors
{
    public class ValidationErrorResponse
    {


        public int StatusCode { get; set; }


        public string Message { get; set; }


        public IEnumerable<ValidationError> Errors { get; set; }

    }


    public class ValidationError
    {


        public string Field { get; set; }

        public IEnumerable<string> Errors { get; set; }

    }


}
