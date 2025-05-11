using Project.Core.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.WPF.Queries
{
    public interface IGetMessageQuery
    {
        [Get("/")]
        Task<Response> Execute();
    }
}
