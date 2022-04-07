using System;
using System.ComponentModel;
using System.IO;
using NUnit.Framework;
using PushAlertsApi.Util;

namespace PushAlertsApi.Tests.Util
{
    public class MessageFormatterTests
    {
        [Test]
        public void LogError_NoInnerException()
        {
            const string message = "My test exception";
            var ex = new FileNotFoundException(message);
            var expected = $"Exception Message: {message}, Inner Exception Message: , Stack Trace: {ex.StackTrace}";
            var actual = MessageFormatter.LogError(ex);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LogError_InnerException()
        {
            const string innerMessage = "Enum wrong";
            const string message = "Argument wrong";
            var innerEx = new InvalidEnumArgumentException(innerMessage);
            var ex = new ArgumentException(message, innerEx);
            var expected =
                $"Exception Message: {message}, Inner Exception Message: {innerEx.Message}, Stack Trace: {ex.StackTrace}";
            var actual = MessageFormatter.LogError(ex);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ActionResultBadRequest()
        {
            const string expected = "Unexpected server error";
            var actual = MessageFormatter.ActionResultBadRequest();
            Assert.AreEqual(expected, actual);
        }
    }
}