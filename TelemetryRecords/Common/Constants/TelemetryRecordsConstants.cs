namespace TelemetryRecords.Common.Constants
{
    public static class TelemetryRecordsConstants
    {
        public static class Configuration
        {
            public const string MONGODB_CONFIG_SECTION = "MongoDbConfiguration";
        }

        public static class Collections
        {
            public const string ASSIGNMENTS_COLLECTION = "assignments";
        }

        public static class Fields
        {
            public const string CREATED_AT = "CreatedAt";
        }

        public static class ErrorMessages
        {
            public const string ASSIGNMENT_NOT_FOUND = "No assignment found.";
            public const string ASSIGNMENT_NOT_FOUND_FOR_DATE = "No assignment found for date {0}.";
            public const string INVALID_DATE_FORMAT = "Invalid date format. Use yyyy-MM-dd.";
        }
    }
}
