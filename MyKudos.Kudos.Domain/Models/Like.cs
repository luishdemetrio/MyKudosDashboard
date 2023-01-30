using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKudos.Kudos.Domain.Models;

public record Like(string KudosId, Person Person);

public record SendLike(string KudosId, string PersonId);
