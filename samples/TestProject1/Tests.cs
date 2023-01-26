namespace TestProject1
{
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Extensions.Hosting;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class Tests
	{
		private TestServer server;

		[SetUp]
		public async Task Setup()
		{
			this.server = await TestApplicationHost.RunAsync<TestApplication1Host>();
		}

		[TearDown]
		public void TearDown()
		{
			this.server?.Dispose();
			this.server = null;
		}

		[Test]
		public async Task ShouldExecuteController()
		{
			HttpClient httpClient = this.server.CreateClient();
			HttpResponseMessage response = await httpClient.GetAsync("/weatherforecast");

			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}
