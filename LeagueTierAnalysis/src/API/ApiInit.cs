namespace LeagueTierLevels
{
    static class ApiInit
    {
        private static string m_ApiKey;
        private static ApiType m_ApiType;
        public static string API_KEY { get => m_ApiKey; }
        public static ApiType API_TYPE { get => m_ApiType; }

        public static void InitApiKey(string apiKey, string type)
        {
            m_ApiKey = apiKey;

            if (type == "personal")
            {
                m_ApiType = ApiType.PERSONAL;
            }
            else if (type == "development")
            {
                m_ApiType = ApiType.DEVELOPMENT;
            }
            else if (type == "production")
            {
                m_ApiType = ApiType.PRODUCTION;
            }
            else
            {
                m_ApiType = ApiType.DEFAULT;
            }
        }
    }
}
