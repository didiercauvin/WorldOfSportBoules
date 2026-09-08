using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public interface IProvidePlayer
{
    IReadOnlyList<Player> GetAll();
}
