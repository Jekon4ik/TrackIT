using System;

namespace TrackIT.Api.Configuration;

public class CacheSettings
    {
        public int TransactionsExpirationMinutes { get; set; }
        public int SlidingExpirationMinutes { get; set; }
        public bool EnableCaching { get; set; }
          public int CategoriesExpirationMinutes { get; set; }
    }

    public class ApiSettings
    {
        public int MaxTransactionsPerRequest { get; set; }
    }
