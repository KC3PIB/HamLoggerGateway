using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using HamLoggerGateway;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace HamLoggerGateway.Tests;

[TestClass]
public class ServerBaseTests
{
    private Mock<IMessageProcessor> _messageProcessorMock = null!;
    private Mock<ILogger> _loggerMock = null!;
    private ServerSettings _settings = null!;

    [TestInitialize]
    public void Setup()
    {
        _messageProcessorMock = new Mock<IMessageProcessor>();
        _loggerMock = new Mock<ILogger>();
        _settings = new ServerSettings
        {
            Address = "127.0.0.1",
            Port = 12345,
            BufferSize = 1024,
            EnableReuseAddress = true
        };
    }

    [TestMethod]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);

        Assert.AreEqual(_settings.Address, server.GetLocalEndpoint().Address.ToString());
        Assert.AreEqual(_settings.Port, server.GetLocalEndpoint().Port);
        Assert.AreEqual(_settings.BufferSize, server.GetBufferSize());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_ShouldThrowArgumentException_ForInvalidAddress()
    {
        _settings.Address = "invalid_address";

        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_ShouldThrowArgumentException_ForInvalidPort()
    {
        _settings.Port = 70000;

        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);
    }

    [TestMethod]
    public void Start_ShouldSetIsRunningToTrue()
    {
        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);
        server.Start();

        Assert.IsTrue(server.GetIsRunning());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Start_ShouldThrowInvalidOperationException_IfAlreadyRunning()
    {
        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);
        server.Start();
        server.Start();
    }

    [TestMethod]
    public void Stop_ShouldSetIsRunningToFalse()
    {
        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);
        server.Start();
        server.Stop();

        Assert.IsFalse(server.GetIsRunning());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Stop_ShouldThrowInvalidOperationException_IfNotRunning()
    {
        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);
        server.Stop();
    }

    [TestMethod]
    public void Dispose_ShouldSetIsDisposedToTrue()
    {
        var server = new TestServer(SocketType.Dgram, ProtocolType.Udp, _messageProcessorMock.Object, _settings,
            _loggerMock.Object);
        server.Dispose();

        Assert.IsTrue(server.GetIsDisposed());
    }

    internal class TestServer : ServerBase
    {
        public TestServer(SocketType socketType, ProtocolType protocolType, IMessageProcessor messageProcessor,
            ServerSettings settings, ILogger logger)
            : base(socketType, protocolType, messageProcessor, settings, logger)
        {
        }

        protected override Task HandleMessageLoopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public IPEndPoint GetLocalEndpoint()
        {
            return LocalEndpoint;
        }

        public int GetBufferSize()
        {
            return BufferSize;
        }

        public bool GetIsRunning()
        {
            return IsRunning;
        }

        public bool GetIsDisposed()
        {
            return IsDisposed;
        }
    }
}