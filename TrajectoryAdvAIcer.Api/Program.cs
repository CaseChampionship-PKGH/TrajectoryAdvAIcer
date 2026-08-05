using System.Text;
using TrajectoryAdvAIcer.Api.DI;
using TrajectoryAdvAIcer.Api.Filters;
using TrajectoryAdvAIcer.Common.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterModule<ApiModule>();

builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<EnumSchemaFilter>();
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TrajectoryAdvAIcer.Api.xml"));
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TrajectoryAdvAIcer.Entities.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
