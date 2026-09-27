using Infrastructure.Messaging.Consumer;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    
    x.AddConsumer<PaymentProcessedConsumer>();    

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            builder.Configuration["RabbitMQ:Host"],
            ushort.Parse(builder.Configuration["RabbitMQ:Port"]!),
            "/",
            h =>
            {
                h.Username(
                    builder.Configuration["RabbitMQ:Username"]);

                h.Password(
                    builder.Configuration["RabbitMQ:Password"]);
            });        

        cfg.ReceiveEndpoint(
                builder.Configuration["RabbitMQ:Queues:FCG_User"],
                e =>
                {
                    e.ConfigureConsumer<UserCreatedConsumer>(
                        context);
                });

        cfg.ReceiveEndpoint(
                builder.Configuration["RabbitMQ:Queues:FCG_Notification"],
                e =>
                {
                    e.ConfigureConsumer<PaymentProcessedConsumer>(
                        context);
                });



    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
