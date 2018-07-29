namespace Moss.Hospital.Data.Providers.Repositories
{
    using System.Data.Common;

    public class SqlHelperException : DbException
    {
        private int _lineError;
        private readonly string _message;
        private readonly int _numberError;

        public SqlHelperException(string message)
        {
            this._message = "";
            this._numberError = -1;
            this._lineError = -1;
            this._message = message;
            this._numberError = -1;
        }

        public SqlHelperException(string message, int number)
        {
            this._message = "";
            this._numberError = -1;
            this._lineError = -1;
            this._message = message;
            this._numberError = number;
        }

        public SqlHelperException(string message, int number, int lineError)
        {
            this._message = "";
            this._numberError = -1;
            this._lineError = -1;
            this._message = message;
            this._numberError = number;
            this._lineError = lineError;
        }

        public int LineError =>
            this._lineError;

        public override string Message =>
            this._message;

        public int Number =>
            this._numberError;
    }
}

