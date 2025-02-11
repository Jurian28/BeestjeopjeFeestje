using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {
    internal class HttpContextAccessorFactory {
        public static IHttpContextAccessor GetHttpContextAccessorWithSession() {
            var sessionMock = new Mock<ISession>();

            // Setup GetString to return a stored value (simulating real session behavior)
            var sessionStorage = new Dictionary<string, byte[]>();
            sessionMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                       .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

            sessionMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                       .Returns((string key, out byte[] value) => {
                           var exists = sessionStorage.TryGetValue(key, out var storedValue);
                           value = storedValue;
                           return exists;
                       });

            var contextMock = new Mock<HttpContext>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            contextMock.Setup(ctx => ctx.Session).Returns(sessionMock.Object);
            httpContextAccessorMock.Setup(acc => acc.HttpContext).Returns(contextMock.Object);

            return httpContextAccessorMock.Object;
        }
    }
}
