
using BLL.Abstractions;
using BLL.AutoMapper;
using BLL.Events;
using BLL.Services;
using DAL.Abstractions;
using DAL.Repos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ADOItemRepository>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ADOItemRepository(config.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ADOItemService>();

builder.Services.AddScoped<IItemRepository>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ItemRepository(config.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IItemService, ItemService>();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("order-created-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("swagger/v1/swagger.json", "Item Service");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
