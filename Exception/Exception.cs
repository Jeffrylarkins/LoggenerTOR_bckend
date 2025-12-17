
    namespace EmployeeManagementAPI.Exception
    {
        public class EmployeeNotFoundException : System.Exception 
        {
            public EmployeeNotFoundException(string message) : base(message) { }
        }

        public class InvalidEmployeeDataException :System.Exception
        {
            public InvalidEmployeeDataException(string message) : base(message) { }
        }
        //public class TimeoutException : System.Exception
        //{
        //    public TimeoutException(string message) : base(message) { }
        //}

        public class DatabaseConnectionException : System.Exception
        {
            public DatabaseConnectionException(string message) : base(message) { }
        }

        //////public class UnauthorizedAccessException : System.Exception
        //////{
        //////    public UnauthorizedAccessException(string message) : base(message) { }
        //////}

        //////public class InvalidOperationException : System.Exception
        //////{
        //////    public InvalidOperationException(string message) : base(message) { }
        //////}

        //////public class ArgumentNullException : System.Exception
        //////{
        //////    public ArgumentNullException(string message) : base(message) { }
        //////}

        //////public class ArgumentOutOfRangeException : System.Exception
        //////{
        //////    public ArgumentOutOfRangeException(string message) : base(message) { }
        //////}

        //////public class FileNotFoundException : System.Exception
        //////{
        //////    public FileNotFoundException(string message) : base(message) { }
        //////}

        //////public class IndexOutOfRangeException : System.Exception
        //////{
        //////    public IndexOutOfRangeException(string message) : base(message) { }
        //////}




    }


