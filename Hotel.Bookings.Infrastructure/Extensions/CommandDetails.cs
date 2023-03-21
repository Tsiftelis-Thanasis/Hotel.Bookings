using System.Data;

namespace HotelBookings.Application.Extensions
{
    public struct CommandDetails
    {
        public string CommandText
        {
            get;
            private set;
        }

        public int? CommandTimeout
        {
            get;
            private set;
        }

        public CommandType? CommandType
        {
            get;
            private set;
        }

        public object Parameters
        {
            get;
            private set;
        }

        public IDbTransaction Transaction
        {
            get;
            private set;
        }

        public CommandDetails(string commandText, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            this = default(CommandDetails);
            CommandText = commandText;
            Parameters = parameters;
            Transaction = transaction;
            CommandTimeout = commandTimeout;
            CommandType = commandType;
        }

    }
}