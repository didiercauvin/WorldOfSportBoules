using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;

public static class InMemoryTeamData
{
    public static IReadOnlyList<Team> Teams { get; } =
    [
        Create("10000000-0000-0000-0000-000000000001", "Les Boulistes de Saint-Martin"),
        Create("10000000-0000-0000-0000-000000000002", "Amicale Bouliste Thouarsaise"),
        Create("10000000-0000-0000-0000-000000000003", "Boule Poitevine"),
        Create("10000000-0000-0000-0000-000000000004", "Étoile Bouliste Tourangelle"),
        Create("10000000-0000-0000-0000-000000000005", "Boule Sportive de Limoges"),
        Create("10000000-0000-0000-0000-000000000006", "Amicale Bouliste Niortaise"),
        Create("10000000-0000-0000-0000-000000000007", "Boule Melloise"),
        Create("10000000-0000-0000-0000-000000000008", "Boule Parthenaisienne"),
        Create("10000000-0000-0000-0000-000000000009", "Boule Bressuiraise"),
        Create("10000000-0000-0000-0000-000000000010", "Amicale Bouliste de Loudun"),
        Create("10000000-0000-0000-0000-000000000011", "Boule Châtelleraudaise"),
        Create("10000000-0000-0000-0000-000000000012", "Boule de Montmorillon"),
        Create("10000000-0000-0000-0000-000000000013", "Amicale Bouliste Saumuroise"),
        Create("10000000-0000-0000-0000-000000000014", "Boule Angevine"),
        Create("10000000-0000-0000-0000-000000000015", "Boule Nantaise"),
        Create("10000000-0000-0000-0000-000000000016", "Amicale Bouliste Choletaise"),
        Create("10000000-0000-0000-0000-000000000017", "Boule Rochelaise"),
        Create("10000000-0000-0000-0000-000000000018", "Boule Rochefortaise"),
        Create("10000000-0000-0000-0000-000000000019", "Amicale Bouliste Saintaise"),
        Create("10000000-0000-0000-0000-000000000020", "Boule de Cognac"),
        Create("10000000-0000-0000-0000-000000000021", "Boule Angoulêmoise"),
        Create("10000000-0000-0000-0000-000000000022", "Boule Périgourdine"),
        Create("10000000-0000-0000-0000-000000000023", "Amicale Bouliste Briviste"),
        Create("10000000-0000-0000-0000-000000000024", "Boule Corrézienne"),
        Create("10000000-0000-0000-0000-000000000025", "Boule de Guéret"),
        Create("10000000-0000-0000-0000-000000000026", "Amicale Bouliste Creusoise"),
        Create("10000000-0000-0000-0000-000000000027", "Boule Clermontoise"),
        Create("10000000-0000-0000-0000-000000000028", "Boule Bourbonnaise"),
        Create("10000000-0000-0000-0000-000000000029", "Amicale Bouliste Moulinoise"),
        Create("10000000-0000-0000-0000-000000000030", "Boule de Vichy"),
        Create("10000000-0000-0000-0000-000000000031", "Boule Roannaise"),
        Create("10000000-0000-0000-0000-000000000032", "Amicale Bouliste Stéphanoise"),
        Create("10000000-0000-0000-0000-000000000033", "Boule Lyonnaise de l'Ouest"),
        Create("10000000-0000-0000-0000-000000000034", "Boule Viennoise"),
        Create("10000000-0000-0000-0000-000000000035", "Amicale Bouliste Grenobloise"),
        Create("10000000-0000-0000-0000-000000000036", "Boule Chambérienne"),
        Create("10000000-0000-0000-0000-000000000037", "Boule Savoyarde"),
        Create("10000000-0000-0000-0000-000000000038", "Amicale Bouliste d'Annecy"),
        Create("10000000-0000-0000-0000-000000000039", "Boule de Valence"),
        Create("10000000-0000-0000-0000-000000000040", "Boule Drômoise"),
        Create("10000000-0000-0000-0000-000000000041", "Amicale Bouliste Avignonnaise"),
        Create("10000000-0000-0000-0000-000000000042", "Boule de Carpentras"),
        Create("10000000-0000-0000-0000-000000000043", "Boule Nîmoise"),
        Create("10000000-0000-0000-0000-000000000044", "Amicale Bouliste Montpelliéraine"),
        Create("10000000-0000-0000-0000-000000000045", "Boule Biterroise"),
        Create("10000000-0000-0000-0000-000000000046", "Boule Narbonnaise"),
        Create("10000000-0000-0000-0000-000000000047", "Amicale Bouliste Perpignanaise"),
        Create("10000000-0000-0000-0000-000000000048", "Boule Toulousaine"),
        Create("10000000-0000-0000-0000-000000000049", "Boule Montalbanaise"),
        Create("10000000-0000-0000-0000-000000000050", "Amicale Bouliste Albigeoise"),
        Create("10000000-0000-0000-0000-000000000051", "Boule Tarnaise"),
        Create("10000000-0000-0000-0000-000000000052", "Boule Agenaise"),
        Create("10000000-0000-0000-0000-000000000053", "Amicale Bouliste Landaise"),
        Create("10000000-0000-0000-0000-000000000054", "Boule Paloise"),
        Create("10000000-0000-0000-0000-000000000055", "Boule Basque"),
        Create("10000000-0000-0000-0000-000000000056", "Amicale Bouliste Bordelaise"),
        Create("10000000-0000-0000-0000-000000000057", "Boule Libournaise"),
        Create("10000000-0000-0000-0000-000000000058", "Boule Médocaine"),
        Create("10000000-0000-0000-0000-000000000059", "Amicale Bouliste Blésoise"),
        Create("10000000-0000-0000-0000-000000000060", "Boule Orléanaise"),
        Create("10000000-0000-0000-0000-000000000061", "Boule Chartraine"),
        Create("10000000-0000-0000-0000-000000000062", "Amicale Bouliste Vendômoise"),
        Create("10000000-0000-0000-0000-000000000063", "Boule Mancelle"),
        Create("10000000-0000-0000-0000-000000000064", "Boule Lavalloise"),
        Create("10000000-0000-0000-0000-000000000065", "Amicale Bouliste Rennaise"),
        Create("10000000-0000-0000-0000-000000000066", "Boule Vannetaise"),
        Create("10000000-0000-0000-0000-000000000067", "Boule Lorientaise"),
        Create("10000000-0000-0000-0000-000000000068", "Amicale Bouliste Brestoise"),
        Create("10000000-0000-0000-0000-000000000069", "Boule Rouennaise"),
        Create("10000000-0000-0000-0000-000000000070", "Amicale Bouliste de Croix")
    ];

    private static Team Create(
        string id,
        string name)
    {
        return new Team(
            Guid.Parse(id),
            name,
            TeamCategory.M4);
    }
}
