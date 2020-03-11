using System;
using System.Threading;
using System.Threading.Tasks;
using Doug.API;
using Doug.Models;
using NLua;

namespace Doug.Services
{
    public interface ILuaService
    {
        Task<DougResponse> ExecuteScript(Command input);
    }

    public class LuaService : ILuaService
    {
        private readonly DougApi _api;

        public LuaService(DougApi api)
        {
            _api = api;
        }

        public async Task<DougResponse> ExecuteScript(Command command)
        {
            using (var state = new Lua())
            {
                _api.AddContext(command.UserId, command.ChannelId);
                state["doug"] = _api;

                var cancellationSource = new CancellationTokenSource();
                cancellationSource.CancelAfter(TimeSpan.FromSeconds(20));

                try
                {
                    await Task.Run(() => state.DoString(command.Text), cancellationSource.Token);
                }
                catch (Exception e)
                {
                    return new DougResponse(e.Message);
                }
            }

            return new DougResponse();
        }
    }
}
