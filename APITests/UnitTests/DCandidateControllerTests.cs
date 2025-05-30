using mAPI.Controllers;
using mAPI.Database;
using mAPI.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace APITests.UnitTests;

public class DCandidateControllerTests
{
    private static Mock<DbSet<DCandidate>> CreateMockDbSet(List<DCandidate> data)
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<DCandidate>>();

        // Async support for EF Core
        mockSet.As<IAsyncEnumerable<DCandidate>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<DCandidate>(queryable.GetEnumerator()));
        mockSet.As<IQueryable<DCandidate>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<DCandidate>(queryable.Provider));
        mockSet.As<IQueryable<DCandidate>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<DCandidate>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<DCandidate>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
            .Returns<object[]>(ids =>
            {
                var id = (int)ids[0];
                var entity = data.Find(d => d.Id == id);
                return new ValueTask<DCandidate?>(entity);
            });
        return mockSet;
    }

    private static Mock<ApplicationDbContext> CreateMockContext(List<DCandidate> data)
    {
        var mockSet = CreateMockDbSet(data);
        var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        mockContext.Setup(c => c.DCandidates).Returns(mockSet.Object);
        mockContext.Setup(c => c.Set<DCandidate>()).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Entry(It.IsAny<DCandidate>())).Returns((EntityEntry<DCandidate>)null!);
        return mockContext;
    }

    [Fact]
    public async Task GetDCandidates_ReturnsAllCandidates()
    {
        var data = new List<DCandidate> { new() { Id = 1, FullName = "Test" } };
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.GetDCandidates();

        var actionResult = Assert.IsType<ActionResult<IEnumerable<DCandidate>>>(result);
        var candidates = Assert.IsAssignableFrom<IEnumerable<DCandidate>>(actionResult.Value);
        Assert.Single(candidates);
    }

    [Fact]
    public async Task GetDCandidate_ReturnsCandidate_WhenFound()
    {
        var data = new List<DCandidate> { new() { Id = 1, FullName = "Test" } };
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.GetDCandidate(1);

        var actionResult = Assert.IsType<ActionResult<DCandidate>>(result);
        var candidate = Assert.IsType<DCandidate>(actionResult.Value);
        Assert.Equal(1, candidate.Id);
    }

    [Fact]
    public async Task GetDCandidate_ReturnsNotFound_WhenMissing()
    {
        var data = new List<DCandidate>();
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.GetDCandidate(99);

        var actionResult = Assert.IsType<ActionResult<DCandidate>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task PutDCandidate_ReturnsBadRequest_WhenIdMismatch()
    {
        var data = new List<DCandidate> { new() { Id = 1, FullName = "Test" } };
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.PutDCandidate(2, new DCandidate { Id = 1 });

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task PutDCandidate_ReturnsNoContent_WhenSuccess()
    {
        var data = new List<DCandidate> { new() { Id = 1, FullName = "Test" } };
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.PutDCandidate(1, new DCandidate { Id = 1 });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutDCandidate_ReturnsNotFound_WhenConcurrencyExceptionAndNotExists()
    {
        var data = new List<DCandidate>();
        var mockContext = CreateMockContext(data);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException());
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.PutDCandidate(1, new DCandidate { Id = 1 });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PostDCandidate_CreatesCandidate()
    {
        var data = new List<DCandidate>();
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var candidate = new DCandidate { Id = 1, FullName = "Test" };
        var result = await controller.PostDCandidate(candidate);

        var actionResult = Assert.IsType<ActionResult<DCandidate>>(result);
        var createdAt = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal("GetDCandidate", createdAt.ActionName);
        Assert.Equal(candidate, createdAt.Value);
    }

    [Fact]
    public async Task DeleteDCandidate_RemovesCandidate_WhenFound()
    {
        var data = new List<DCandidate> { new() { Id = 1, FullName = "Test" } };
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.DeleteDCandidate(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteDCandidate_ReturnsNotFound_WhenMissing()
    {
        var data = new List<DCandidate>();
        var mockContext = CreateMockContext(data);
        var controller = new DCandidateController(mockContext.Object);

        var result = await controller.DeleteDCandidate(1);

        Assert.IsType<NotFoundResult>(result);
    }
}

// --------- Async helpers for EF Core mocking ---------
internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
    public T Current => _inner.Current;
    public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; }
    public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);

    public object Execute(Expression expression) => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) => new TestAsyncEnumerable<TResult>(expression);

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) =>
        Task.FromResult(Execute<TResult>(expression));

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(Expression expression) : base(expression) { }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}