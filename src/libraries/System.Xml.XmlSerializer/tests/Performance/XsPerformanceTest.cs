﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Xunit.Performance;
using Xunit;

namespace System.Xml.XmlSerializer.Tests.Performance
{
    #region XmlSerializer performance tests

    public class XsPerformanceTest
    {
        public static IEnumerable<object[]> SerializeMemberData()
        {
            return PerformanceTestCommon.PerformanceMemberData();
        }

        [Benchmark]
        [MemberData(nameof(SerializeMemberData))]
        public void XsSerializationTest(int numberOfRuns, TestType testType, int testSize)
        {
            PerformanceTestCommon.RunSerializationPerformanceTest(numberOfRuns, testType, testSize, XsSerializerFactory.GetInstance());
        }

        [Benchmark]
        [MemberData(nameof(SerializeMemberData))]
        public void XsDeSerializationTest(int numberOfRuns, TestType testType, int testSize)
        {
            PerformanceTestCommon.RunDeserializationPerformanceTest(numberOfRuns, testType, testSize, XsSerializerFactory.GetInstance());
        }
    }

    #endregion

    #region XmlSerializer wrapper

    internal class XsSerializerFactory : ISerializerFactory
    {
        private static readonly XsSerializerFactory Instance = new XsSerializerFactory();
        public static XsSerializerFactory GetInstance()
        {
            return Instance;
        }

        public IPerfTestSerializer GetSerializer()
        {
            return new XsSerializer();
        }
    }

    internal class XsSerializer : IPerfTestSerializer
    {
        private Serialization.XmlSerializer _serializer;

        public void Deserialize(Stream stream)
        {
            Debug.Assert(_serializer != null);
            Debug.Assert(stream != null);
            _serializer.Deserialize(stream);
        }

        public void Init(object obj)
        {
            _serializer = new Serialization.XmlSerializer(obj.GetType());
        }

        public void Serialize(object obj, Stream stream)
        {
            Debug.Assert(_serializer != null);
            Debug.Assert(stream != null);
            _serializer.Serialize(stream, obj);
        }
    }

    #endregion
}
