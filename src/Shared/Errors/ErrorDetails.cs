namespace Shared.Errors
{
    public class ErrorDetails
    {


        public int StatusCode { get; set; }


        public string ErrorMessage { get; set; }




        public IEnumerable<string>? Errors { get; set; }
    }
}
