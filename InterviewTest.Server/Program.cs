using InterviewTest.Interfaces;
using InterviewTest.Repo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEmployeeRepo, SqlLiteEmployeeRepo>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

#region Prepare Sqlite
IEmployeeRepo repo = new SqlLiteEmployeeRepo();
repo.PrepareRepo();
#endregion
