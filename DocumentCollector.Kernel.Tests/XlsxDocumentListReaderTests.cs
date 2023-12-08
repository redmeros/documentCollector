using FluentAssertions;

namespace DocumentCollector.Kernel.Tests;

public class XlsxDocumentListReaderTests
{
    private const string TestListFileName = "714-PWW-I-KS-T00000-06.xlsx";
    
    [Fact]
    public void CanOpenTestFileName()
    {
        using var s = File.OpenRead(TestListFileName);
        s.CanWrite.Should().BeFalse();
        s.CanRead.Should().BeTrue();
    }
}