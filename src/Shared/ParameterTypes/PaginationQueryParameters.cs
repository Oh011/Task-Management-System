namespace Shared.ParameterTypes
{
    public class PaginationQueryParameters
    {



        public int PageIndex { get; set; } = 1;


        private const int MaxPageSize = 10;

        private const int _DefaultPageSize = 5;


        private int _PageSize = _DefaultPageSize;



        public int PageSize
        {

            get { return _PageSize; }

            set { _PageSize = value > MaxPageSize ? _DefaultPageSize : value; }

        }

    }
}
