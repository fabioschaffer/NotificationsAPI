using Application.DTOs;
using Domain.Events;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumer
{
    public class PaymentProcessedConsumer(IConfiguration _configuration) : IConsumer<PaymentProcessedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            var message = context.Message;

            if (message.Status.ToString() == "Approved")
                await EnviaEmail(message);
        }

        public async Task EnviaEmail(PaymentProcessedEvent evento)
        {
            var httpClient = new HttpClient();

            var url = _configuration["API_Users:URL"];

            var loginRequest = new LoginRequest
            {
                Email = _configuration["API_Users:email"],
                Senha = _configuration["API_Users:senha"]
            };

            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            var responseLogin = await httpClient.PostAsync($"{url}/login", content);

            if (responseLogin.IsSuccessStatusCode)
            {

                var json = await responseLogin.Content.ReadAsStringAsync();

                var auth = JsonSerializer.Deserialize<LoginResponse>(json);

                var token = auth.token;

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var responseUser = await httpClient.GetAsync($"{url}/usuario/{evento.UserId}");

                if (responseUser.IsSuccessStatusCode)
                {
                    var userData = await responseUser.Content.ReadAsStringAsync();

                    var user = JsonSerializer.Deserialize<UserResponse>(userData);

                    var resposneGame = await httpClient.GetAsync($"{_configuration["URL_API_Catalog"]}/game/{evento.GameId}");

                    string jogo = "";

                    if (resposneGame.IsSuccessStatusCode)
                    {
                        var gameData = await resposneGame.Content.ReadAsStringAsync();

                        var game = JsonSerializer.Deserialize<GameResponse>(gameData);

                        jogo = game.nome;
                    }

                    var body = $"\n\nPara: {user.email}\nOlá {user.nome}, sua compra do {jogo} foi aprovada!\nParabéns pela sua aquisição.";

                    Console.WriteLine(body);
                }
            }

            
        }
    }
}
