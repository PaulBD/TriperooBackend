﻿using System;
using System.Collections.Generic;
using Couchbase.Configuration.Client;

namespace library.couchbase
{
    public static class CouchbaseConfig
    {
        public static ClientConfiguration Initialize()
        {
            var config = new ClientConfiguration();
            config.BucketConfigs.Clear();

            config.Servers = new List<Uri>(new Uri[] { new Uri(CouchbaseConfigHelper.Instance.Server) });
            config.UseSsl = CouchbaseConfigHelper.Instance.UseSSL;
            config.BucketConfigs = new Dictionary<string, BucketConfiguration>
                {
                    { CouchbaseConfigHelper.Instance.BucketName, new BucketConfiguration
                    {
                        BucketName = CouchbaseConfigHelper.Instance.BucketName,
                        UseSsl = CouchbaseConfigHelper.Instance.UseSSL,
                        Password = CouchbaseConfigHelper.Instance.BucketPassword,
                        PoolConfiguration = new PoolConfiguration
                        {
                            MaxSize = 10,
                            MinSize = 5
                        }
                    }}
                };

            return config;
        }
    }
}