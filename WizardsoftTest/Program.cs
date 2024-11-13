using WizardsoftTest.Services;
using WizardsoftTest.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FoldersDatabaseSettings>(
    builder.Configuration.GetSection("CatalogueTreeDatabase"));
builder.Services.AddSingleton<FolderService>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
