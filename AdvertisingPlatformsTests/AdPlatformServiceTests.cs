using AdvertisingPlatforms.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Reflection;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvertisingPlatformsTests
{
    public class AdPlatformServiceTests
    {
        private readonly AdPlatformService service;
        private readonly ITestOutputHelper output;


        public AdPlatformServiceTests(ITestOutputHelper output)
        {
            service = new AdPlatformService();
            this.output = output;
        }

        [Fact]
        public void FindPlatformsForLocation_FindLocation_Platforms()
        {
            var fileContent = "������.������:/ru\n���������� �������:/ru/svrd/revda,/ru/svrd/pervik\n������ ��������� ���������:/ru/msk,/ru/permobl,/ru/chelobl\n������ �������:/ru/svrd";
            var file = CreateMockFile(fileContent).Object;

            service.LoadFile(file);
            
            // /ru
            var request = "/ru";
            var actual = new List<string>() { "������.������" };
            var responce = service.FindPlatformsForLocation(request);
            Xunit.Assert.Equal(responce, actual);

            // /ru/msk
            request = "/ru/msk";
            actual = new List<string>() { "������ ��������� ���������", "������.������" };
            responce = service.FindPlatformsForLocation(request);
            Xunit.Assert.Equal(responce, actual);

            // /ru/svrd
            request = "/ru/svrd";
            actual = new List<string>() { "������ �������", "������.������" };
            responce = service.FindPlatformsForLocation(request);
            Xunit.Assert.Equal(responce, actual);

            // /ru/svrd/revda
            request = "/ru/svrd/revda";
            actual = new List<string>() { "������ �������", "���������� �������", "������.������" };
            responce = service.FindPlatformsForLocation(request);
            Xunit.Assert.Equal(responce, actual);
        }

        private Mock<IFormFile> CreateMockFile(string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.FileName).Returns("test.txt");

            return fileMock;
        }
    }
}