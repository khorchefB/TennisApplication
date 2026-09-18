using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Tennis.Application.Dtos;

namespace Tennis.Tests.Tennis.TestIntegrations;

public class TennisApiTests
{
    [Fact]
    public async Task GetPlayer_AvecIdInconnu_DoitRetourner404ProblemDetails()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/players/99999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
    }

    [Fact]
    public async Task PostPlayer_DoitRetourner201Puis409PourUnDoublon()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        var joueur = new TennisPlayerDto
        {
            Id = 9_990_001,
            Firstname = "Integration",
            Lastname = "Test",
            Shortname = "I.TEST",
            Sex = "M",
            Country = new CountryDto { Code = "FRA" },
            Data = new PlayerDataDto
            {
                Rank = 999,
                Points = 0,
                Weight = 75_000,
                Height = 180,
                Age = 30,
                Last = [1, 0, 1]
            }
        };

        var creation = await client.PostAsJsonAsync("/api/players", joueur);
        Assert.Equal(HttpStatusCode.Created, creation.StatusCode);
        Assert.NotNull(creation.Headers.Location);

        var doublon = await client.PostAsJsonAsync("/api/players", joueur);
        Assert.Equal(HttpStatusCode.Conflict, doublon.StatusCode);
    }

    [Fact]
    public async Task PostPlayer_AvecModeleInvalide_DoitRetourner400()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        var joueurInvalide = new TennisPlayerDto
        {
            Id = 0,
            Firstname = "",
            Lastname = "",
            Shortname = "",
            Sex = "",
            Country = new CountryDto { Code = "" },
            Data = new PlayerDataDto()
        };

        var response = await client.PostAsJsonAsync("/api/players", joueurInvalide);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetStatistics_DoitRetournerLesValeursAttenduesDuJeuDeDonnees()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var statistiques = await client.GetFromJsonAsync<StatistiquesDto>("/api/statistics");

        Assert.NotNull(statistiques);
        Assert.Equal("SRB", statistiques.Country);
        Assert.Equal(185d, statistiques.Medianne);
        Assert.Equal(23.3578389955, statistiques.IMC, 6);
    }
}
