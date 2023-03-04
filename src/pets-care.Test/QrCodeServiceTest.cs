using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using pets_care.Services;
using Xunit;

namespace pets_care.Test;

public class QrCodeServiceTest
{
    [Theory]
    [InlineData("http://petcare.com/")]
    [InlineData("just a basic string")]
    public void ShouldReturnAnImageQrCode(string url)
    {
        // Action
        var result = QrCodeService.GenerateByteArray(url);
        var image = QrCodeService.GenerateImage(url);

        // Assert
        result.Should().BeOfType<byte[]>();
        image.Should().BeOfType<Bitmap>();
    }
}
